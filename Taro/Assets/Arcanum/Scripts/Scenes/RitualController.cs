using System.Collections;
using System.Collections.Generic;
using Arcanum.App;
using Arcanum.Cards;
using Arcanum.Data;
using Arcanum.Master;
using Arcanum.Reading;
using Arcanum.UI;
using UnityEngine;
using UnityEngine.UI;
using ProfileSession = Arcanum.Profile.ProfileSession;

namespace Arcanum.Scenes
{
    public sealed class RitualController : MonoBehaviour
    {
        private Text promptText;
        private Text guidanceText;
        private TarotMasterPresenter masterPresenter;
        private readonly List<TarotCardView> cardViews = new List<TarotCardView>();
        private bool initialized;
        private bool selectionLocked;

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

            var canvas = ArcanumUiFactory.CreateCanvas("RitualCanvas");
            ArcanumUiFactory.CreateBackground(canvas.transform, "BG_RITUAL_DAILY_TABLE_16X9");
            masterPresenter = TarotMasterPresenter.Create(canvas.GetComponent<RectTransform>());
            masterPresenter.SetExpression("focused");

            var table = ArcanumUiFactory.CreateProfilePanel(canvas.transform, "Center_CardTable", ArcanumUiFactory.CenterColumnAnchorMin, ArcanumUiFactory.CenterColumnAnchorMax);
            CreateMagicCircle(table);
            promptText = ArcanumUiFactory.CreateText(table, "Prompt", "\uC2DC\uC120\uC774 \uBA38\uBB34\uB294 \uCE74\uB4DC\uB97C \uC120\uD0DD\uD558\uC138\uC694", 34, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            promptText.rectTransform.anchorMin = new Vector2(0.06f, 0.85f);
            promptText.rectTransform.anchorMax = new Vector2(0.94f, 0.95f);
            promptText.rectTransform.offsetMin = Vector2.zero;
            promptText.rectTransform.offsetMax = Vector2.zero;

            CreateGuidancePanel(canvas.transform);
            CreateCards(table);
            CreateDialogue(canvas.transform, "\uC11C\uB450\uB974\uC9C0 \uB9C8\uC138\uC694. \uC790\uAFB8 \uC2DC\uC120\uC774 \uB3CC\uC544\uAC00\uB294 \uACF3\uC5D0 \uC190\uB05D\uC744 \uBA38\uBB3C\uAC8C \uD574\uBCF4\uC138\uC694.");
        }

        private void CreateGuidancePanel(Transform parent)
        {
            var right = ArcanumUiFactory.CreateRightPanel(parent, "Right_RitualPanel");

            var title = ArcanumUiFactory.CreateText(right, "GuideTitle", "\uCE74\uB4DC \uD14C\uC774\uBE14", 29, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.80f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.92f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            guidanceText = ArcanumUiFactory.CreateText(right, "GuideBody", "\uC5EC\uC12F \uC7A5\uC758 \uC544\uB974\uCE74\uB098\uAC00 \uCC9C \uC704\uC5D0 \uB193\uC5EC \uC788\uC2B5\uB2C8\uB2E4.\n\n\uB4B7\uBA74 \uCE74\uB4DC \uD558\uB098\uB97C \uACE0\uB974\uC138\uC694. \uC120\uD0DD\uD55C \uCE74\uB4DC\uAC00 \uC5F4\uB9AC\uB294 \uB3D9\uC548 \uB098\uBA38\uC9C0\uB294 \uC870\uC6A9\uD788 \uBB3C\uB7EC\uB0A9\uB2C8\uB2E4.", 23, TextAnchor.UpperLeft, ArcanumUiFactory.Ink);
            guidanceText.rectTransform.anchorMin = new Vector2(0.10f, 0.34f);
            guidanceText.rectTransform.anchorMax = new Vector2(0.90f, 0.74f);
            guidanceText.rectTransform.offsetMin = Vector2.zero;
            guidanceText.rectTransform.offsetMax = Vector2.zero;
        }

        private static void CreateMagicCircle(RectTransform table)
        {
            var circleObject = new GameObject("DailyMagicCircle", typeof(RectTransform), typeof(Image));
            circleObject.transform.SetParent(table, false);
            var rect = circleObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.11f, 0.11f);
            rect.anchorMax = new Vector2(0.89f, 0.82f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var image = circleObject.GetComponent<Image>();
            image.raycastTarget = false;
            image.preserveAspect = true;
            ArcanumUiFactory.ApplySprite(image, "Art/FX/FX_MAGIC_CIRCLE_DAILY", new Color(1f, 0.75f, 0.25f, 0.20f));
        }

        private void CreateCards(RectTransform table)
        {
            var cards = TodayCardService.GetSpreadForDate(System.DateTime.Today);
            const int columns = 3;
            const float cardWidth = 0.235f;
            const float cardHeight = 0.285f;

            for (var i = 0; i < cards.Count; i++)
            {
                var view = TarotCardView.Create(table, cards[i], false);
                cardViews.Add(view);
                var rect = view.GetComponent<RectTransform>();
                var row = i / columns;
                var column = i % columns;
                var x = 0.09f + column * 0.335f;
                var y = 0.49f - row * 0.34f;
                rect.anchorMin = new Vector2(x, y);
                rect.anchorMax = new Vector2(x + cardWidth, y + cardHeight);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                var card = cards[i];
                view.GetComponent<Button>().onClick.AddListener(() => SelectCard(view, card));
            }
        }

        private void SelectCard(TarotCardView view, TarotCardDefinition card)
        {
            if (selectionLocked)
            {
                return;
            }

            selectionLocked = true;
            ReadingSession.Select(card);
            masterPresenter?.PlayRevealGesture();
            view.Reveal();
            EnlargeSelectedCard(view.GetComponent<RectTransform>());
            DisableOtherCards(view);

            if (promptText != null)
            {
                promptText.text = "\uCE74\uB4DC\uAC00 \uC5F4\uB838\uC2B5\uB2C8\uB2E4";
            }

            if (guidanceText != null)
            {
                guidanceText.text = "\uD14C\uC774\uBE14\uC774 \uB2F5\uD588\uC2B5\uB2C8\uB2E4.\n\n\uB8E8\uB098\uAC00 \uD574\uC11D \uB178\uD2B8\uB97C \uC900\uBE44\uD558\uACE0 \uC788\uC2B5\uB2C8\uB2E4.";
            }

            StartCoroutine(GoToResult());
        }

        private static void EnlargeSelectedCard(RectTransform rect)
        {
            rect.anchorMin = new Vector2(0.34f, 0.19f);
            rect.anchorMax = new Vector2(0.66f, 0.79f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.SetAsLastSibling();
        }

        private void DisableOtherCards(TarotCardView selected)
        {
            for (var i = 0; i < cardViews.Count; i++)
            {
                var button = cardViews[i].GetComponent<Button>();
                if (button != null)
                {
                    button.interactable = cardViews[i] == selected;
                }
            }
        }

        private static void CreateDialogue(Transform parent, string line)
        {
            ArcanumUiFactory.CreateDialogueBox(parent, "\uB8E8\uB098", line, out _);
        }

        private static IEnumerator GoToResult()
        {
            yield return new WaitForSeconds(0.65f);
            SceneFlow.Load(SceneFlow.ReadingResult);
        }
    }
}
