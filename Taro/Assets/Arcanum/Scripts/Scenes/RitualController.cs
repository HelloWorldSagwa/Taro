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

namespace Arcanum.Scenes
{
    public sealed class RitualController : MonoBehaviour
    {
        private Text promptText;
        private Text guidanceText;
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
            var canvas = ArcanumUiFactory.CreateCanvas("RitualCanvas");
            ArcanumUiFactory.CreatePanel(canvas.transform, "Background", Vector2.zero, Vector2.one, ArcanumUiFactory.StageBlack);
            TarotMasterPresenter.Create(canvas.GetComponent<RectTransform>()).SetExpression("focused");

            var table = ArcanumUiFactory.CreatePanel(canvas.transform, "Center_CardTable", ArcanumUiFactory.CenterColumnAnchorMin, ArcanumUiFactory.CenterColumnAnchorMax, ArcanumUiFactory.PanelGlass);
            promptText = ArcanumUiFactory.CreateText(table, "Prompt", "Choose the card that holds your eye", 34, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            promptText.rectTransform.anchorMin = new Vector2(0.06f, 0.85f);
            promptText.rectTransform.anchorMax = new Vector2(0.94f, 0.95f);
            promptText.rectTransform.offsetMin = Vector2.zero;
            promptText.rectTransform.offsetMax = Vector2.zero;

            CreateGuidancePanel(canvas.transform);

            CreateCards(table);
            CreateDialogue(canvas.transform, "Do not hurry the draw. Let the pointer rest where your attention keeps returning.");
        }

        private void CreateGuidancePanel(Transform parent)
        {
            var right = ArcanumUiFactory.CreatePanel(parent, "Right_RitualPanel", ArcanumUiFactory.RightColumnAnchorMin, ArcanumUiFactory.RightColumnAnchorMax, new Color(0.08f, 0.055f, 0.12f, 0.96f));

            var title = ArcanumUiFactory.CreateText(right, "GuideTitle", "CARD TABLE", 29, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.80f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.92f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            guidanceText = ArcanumUiFactory.CreateText(right, "GuideBody", "Six prototype arcana are on the cloth.\n\nPick one face-down card. The others will dim while the chosen card opens.", 23, TextAnchor.UpperLeft, ArcanumUiFactory.Ink);
            guidanceText.rectTransform.anchorMin = new Vector2(0.10f, 0.34f);
            guidanceText.rectTransform.anchorMax = new Vector2(0.90f, 0.74f);
            guidanceText.rectTransform.offsetMin = Vector2.zero;
            guidanceText.rectTransform.offsetMax = Vector2.zero;
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
            view.Reveal();
            EnlargeSelectedCard(view.GetComponent<RectTransform>());
            DisableOtherCards(view);

            if (promptText != null)
            {
                promptText.text = "The card has turned";
            }

            if (guidanceText != null)
            {
                guidanceText.text = "The table has answered.\n\nLuna is preparing the reading note.";
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
            ArcanumUiFactory.CreateDialogueBox(parent, "LUNA", line, out _);
        }

        private static IEnumerator GoToResult()
        {
            yield return new WaitForSeconds(0.65f);
            SceneFlow.Load(SceneFlow.ReadingResult);
        }
    }
}
