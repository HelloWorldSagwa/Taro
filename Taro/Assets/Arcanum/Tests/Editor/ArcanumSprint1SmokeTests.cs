using System.Linq;
using Arcanum.Data;
using Arcanum.Profile;
using Arcanum.Reading;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Arcanum.Tests.Editor
{
    public sealed class ArcanumSprint1SmokeTests
    {
        private static readonly string[] RequiredScenePaths =
        {
            "Assets/Arcanum/Scenes/Boot.unity",
            "Assets/Arcanum/Scenes/MainMenu.unity",
            "Assets/Arcanum/Scenes/ProfileCreate.unity",
            "Assets/Arcanum/Scenes/HomeTable.unity",
            "Assets/Arcanum/Scenes/Ritual.unity",
            "Assets/Arcanum/Scenes/ReadingResult.unity"
        };

        [Test]
        public void PrototypeTarotDeck_ContainsSixCards()
        {
            Assert.That(PrototypeTarotDeck.Cards, Is.Not.Null);
            Assert.That(PrototypeTarotDeck.Cards.Count, Is.EqualTo(6));
            Assert.That(PrototypeTarotDeck.Cards, Has.All.Not.Null);
            Assert.That(PrototypeTarotDeck.Cards.Select(card => card.CardId), Is.Unique);
        }

        [Test]
        public void PrototypeTarotDeck_CardsHaveRequiredSprintOneContent()
        {
            foreach (var card in PrototypeTarotDeck.Cards)
            {
                Assert.That(card.CardId, Is.Not.Empty, "CardId must be populated.");
                Assert.That(card.DisplayName, Is.Not.Empty, $"{card.CardId} display name must be populated.");
                Assert.That(card.Keywords, Is.Not.Null, $"{card.CardId} keywords must not be null.");
                Assert.That(card.Keywords, Is.Not.Empty, $"{card.CardId} must have at least one keyword.");
                Assert.That(card.Keywords, Has.All.Not.Null.And.Not.Empty, $"{card.CardId} keywords must be populated.");
                Assert.That(card.MasterLine, Is.Not.Empty, $"{card.CardId} master line must be populated.");
                Assert.That(card.TodayMessage, Is.Not.Empty, $"{card.CardId} today message must be populated.");
            }
        }

        [Test]
        public void TodayCardService_ReturnsPrototypeCardWithoutNulls()
        {
            var todayCard = TodayCardService.GetTodayCard();

            Assert.That(todayCard, Is.Not.Null);
            Assert.That(PrototypeTarotDeck.Cards, Does.Contain(todayCard));
        }

        [Test]
        public void TodayCardService_ReturnsPrototypeCardForRepresentativeDatesWithoutNulls()
        {
            var dates = Enumerable.Range(0, 366)
                .Select(offset => new System.DateTime(2026, 1, 1).AddDays(offset));

            foreach (var date in dates)
            {
                var card = TodayCardService.GetCardForDate(date);

                Assert.That(card, Is.Not.Null, $"Today card must not be null for {date:yyyy-MM-dd}.");
                Assert.That(PrototypeTarotDeck.Cards, Does.Contain(card), $"Today card must come from prototype deck for {date:yyyy-MM-dd}.");
            }
        }

        [Test]
        public void TodayCardService_IsDeterministicForSameDate()
        {
            var date = new System.DateTime(2026, 5, 18);

            Assert.That(TodayCardService.GetCardForDate(date), Is.SameAs(TodayCardService.GetCardForDate(date)));
            Assert.That(
                TodayCardService.GetSpreadForDate(date).Select(card => card.CardId).ToArray(),
                Is.EqualTo(TodayCardService.GetSpreadForDate(date).Select(card => card.CardId).ToArray()));
        }

        [Test]
        public void ReadingSession_FallbackTodayCardPersistsSelection()
        {
            ReadingSession.Clear();

            var fallback = ReadingSession.EnsureSelectionOrTodayCard();

            Assert.That(fallback, Is.Not.Null);
            Assert.That(ReadingSession.SelectedCard, Is.SameAs(fallback));
            Assert.That(ReadingSession.SelectedCardId, Is.EqualTo(fallback.CardId));
            ReadingSession.Clear();
        }

        [Test]
        public void ProfileSession_DefaultsKeepKoreanFallbacks()
        {
            ProfileSession.Clear();

            var profile = ProfileSession.Create(string.Empty, string.Empty, string.Empty);

            Assert.That(profile.SafeDisplayName, Is.EqualTo("\uC190\uB2D8"));
            Assert.That(profile.SafeBirthMonth, Is.EqualTo("\uBBF8\uC785\uB825"));
            Assert.That(profile.SafeFocus, Is.EqualTo("\uC624\uB298\uC758 \uD750\uB984"));
            Assert.That(ProfileSession.HasProfile, Is.True);
            ProfileSession.Clear();
        }

        [Test]
        public void RequiredSprintOneScenes_ExistAsAssets()
        {
            foreach (var scenePath in RequiredScenePaths)
            {
                Assert.That(AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath), Is.Not.Null, scenePath);
            }
        }

        [Test]
        public void ArcanumScenesFolder_ContainsExactlyRequiredSprintOneScenes()
        {
            var actualScenePaths = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Arcanum/Scenes" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(path => path)
                .ToArray();

            Assert.That(actualScenePaths, Is.EquivalentTo(RequiredScenePaths));
        }

        [Test]
        public void RequiredSprintOneScenes_AreEnabledInEditorBuildSettings()
        {
            var enabledBuildScenePaths = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

            foreach (var scenePath in RequiredScenePaths)
            {
                Assert.That(enabledBuildScenePaths, Does.Contain(scenePath));
            }
        }

        [Test]
        public void StandalonePlayerSettings_TargetPcWindowed1920x1080()
        {
            Assert.That(PlayerSettings.defaultScreenWidth, Is.EqualTo(1920));
            Assert.That(PlayerSettings.defaultScreenHeight, Is.EqualTo(1080));
            Assert.That(PlayerSettings.defaultIsNativeResolution, Is.False);
            Assert.That(PlayerSettings.fullScreenMode, Is.EqualTo(FullScreenMode.Windowed));
            Assert.That(PlayerSettings.resizableWindow, Is.False);
        }
    }
}
