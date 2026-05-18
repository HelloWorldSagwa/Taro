using System.Linq;
using Arcanum.App;
using Arcanum.Cards;
using Arcanum.Master;
using Arcanum.Reading;
using Arcanum.UI;
using UnityEngine;
using UnityEngine.UI;
using ProfileSession = Arcanum.Profile.ProfileSession;

namespace Arcanum.Scenes
{
    public sealed class ReadingResultController : MonoBehaviour
    {
        private bool initialized;

        private void Awake()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            if (!ProfileSession.HasProfile)
            {
                SceneFlow.Load(SceneFlow.ProfileCreate);
                return;
            }

            var card = ReadingSession.EnsureSelectionOrTodayCard();
            var profile = ProfileSession.Current;
            var canvas = ArcanumUiFactory.CreateCanvas("ReadingResultCanvas");
            ArcanumUiFactory.CreateBackground(canvas.transform, "BG_RESULT_DAILY_TABLE_16X9");
            var master = TarotMasterPresenter.Create(canvas.GetComponent<RectTransform>());
            master.PlayResultEmphasis();

            var cardPanel = ArcanumUiFactory.CreateProfilePanel(canvas.transform, "Center_SelectedCardStage", ArcanumUiFactory.CenterColumnAnchorMin, ArcanumUiFactory.CenterColumnAnchorMax);
            CreateRevealFx(cardPanel);
            var cardView = TarotCardView.Create(cardPanel, card, true);
            var cardRect = cardView.GetComponent<RectTransform>();
            cardRect.anchorMin = new Vector2(0.31f, 0.18f);
            cardRect.anchorMax = new Vector2(0.69f, 0.83f);
            cardRect.offsetMin = Vector2.zero;
            cardRect.offsetMax = Vector2.zero;

            var stageLabel = ArcanumUiFactory.CreateText(cardPanel, "StageLabel", "오늘 열린 아르카나", 23, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            stageLabel.rectTransform.anchorMin = new Vector2(0.12f, 0.86f);
            stageLabel.rectTransform.anchorMax = new Vector2(0.88f, 0.94f);
            stageLabel.rectTransform.offsetMin = Vector2.zero;
            stageLabel.rectTransform.offsetMax = Vector2.zero;

            var right = ArcanumUiFactory.CreateRightPanel(canvas.transform, "Right_ResultPanel");
            var title = ArcanumUiFactory.CreateText(right, "CardTitle", card.DisplayName, 34, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.80f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.92f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var keywords = ArcanumUiFactory.CreateText(right, "Keywords", string.Join("  /  ", card.Keywords.Select(k => k)), 20, TextAnchor.MiddleCenter, ArcanumUiFactory.Rose);
            keywords.rectTransform.anchorMin = new Vector2(0.08f, 0.69f);
            keywords.rectTransform.anchorMax = new Vector2(0.92f, 0.78f);
            keywords.rectTransform.offsetMin = Vector2.zero;
            keywords.rectTransform.offsetMax = Vector2.zero;

            var focus = profile == null ? "오늘의 흐름" : profile.SafeFocus;
            var message = ArcanumUiFactory.CreateText(right, "TodayMessage", $"질문\n{focus}\n\n루나의 해석\n{card.TodayMessage}", 23, TextAnchor.UpperLeft, ArcanumUiFactory.Ink);
            message.rectTransform.anchorMin = new Vector2(0.08f, 0.24f);
            message.rectTransform.anchorMax = new Vector2(0.92f, 0.65f);
            message.rectTransform.offsetMin = Vector2.zero;
            message.rectTransform.offsetMax = Vector2.zero;

            var restart = ArcanumUiFactory.CreateSecondaryButton(right, "BackButton", "테이블로 돌아가기");
            var restartRect = restart.GetComponent<RectTransform>();
            restartRect.anchorMin = new Vector2(0.16f, 0.07f);
            restartRect.anchorMax = new Vector2(0.84f, 0.17f);
            restartRect.offsetMin = Vector2.zero;
            restartRect.offsetMax = Vector2.zero;
            restart.onClick.AddListener(() => SceneFlow.Load(SceneFlow.HomeTable));

            CreateDialogue(canvas.transform, card.MasterLine);
        }

        private static void CreateRevealFx(RectTransform parent)
        {
            var fxObject = new GameObject("CardRevealFx", typeof(RectTransform), typeof(Image));
            fxObject.transform.SetParent(parent, false);
            var rect = fxObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.18f, 0.07f);
            rect.anchorMax = new Vector2(0.82f, 0.88f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var image = fxObject.GetComponent<Image>();
            image.raycastTarget = false;
            image.preserveAspect = true;
            ArcanumUiFactory.ApplySprite(image, "Art/FX/FX_CARD_REVEAL_GOLD_SMOKE", new Color(1f, 0.72f, 0.26f, 0.35f));
        }

        private void CreateDialogue(Transform parent, string line)
        {
            ArcanumUiFactory.CreateDialogueBox(parent, "루나", line, out _);
        }
    }
}
