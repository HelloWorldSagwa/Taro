using Arcanum.Data;
using Arcanum.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arcanum.Cards
{
    public sealed class TarotCardView : MonoBehaviour
    {
        [SerializeField] private Image frame;
        [SerializeField] private Text title;
        [SerializeField] private Text subtitle;

        public TarotCardDefinition Card { get; private set; }
        public bool IsRevealed { get; private set; }

        public void Bind(TarotCardDefinition card, bool revealed)
        {
            Card = card;
            IsRevealed = revealed;
            Refresh();
        }

        public void Reveal()
        {
            IsRevealed = true;
            Refresh();
        }

        private void Refresh()
        {
            var hasCard = Card != null;

            if (frame != null)
            {
                frame.color = IsRevealed && hasCard ? Card.AccentColor : ArcanumUiFactory.PanelDark;
            }

            if (title != null)
            {
                title.text = IsRevealed && hasCard ? Card.DisplayName : "ARCANUM";
            }

            if (subtitle != null)
            {
                subtitle.text = IsRevealed && hasCard ? $"Major {Card.Number:00}" : "Click to reveal";
            }
        }

        public static TarotCardView Create(Transform parent, TarotCardDefinition card, bool revealed)
        {
            var root = new GameObject($"Card_{card.CardId}", typeof(RectTransform), typeof(Image), typeof(Button), typeof(TarotCardView));
            root.transform.SetParent(parent, false);

            var view = root.GetComponent<TarotCardView>();
            view.frame = root.GetComponent<Image>();

            view.title = ArcanumUiFactory.CreateText(root.transform, "Title", string.Empty, 24, TextAnchor.MiddleCenter, Color.white);
            var titleRect = view.title.rectTransform;
            titleRect.anchorMin = new Vector2(0.08f, 0.42f);
            titleRect.anchorMax = new Vector2(0.92f, 0.72f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            view.subtitle = ArcanumUiFactory.CreateText(root.transform, "Subtitle", string.Empty, 17, TextAnchor.MiddleCenter, ArcanumUiFactory.Ink);
            var subtitleRect = view.subtitle.rectTransform;
            subtitleRect.anchorMin = new Vector2(0.1f, 0.14f);
            subtitleRect.anchorMax = new Vector2(0.9f, 0.34f);
            subtitleRect.offsetMin = Vector2.zero;
            subtitleRect.offsetMax = Vector2.zero;

            view.Bind(card, revealed);
            return view;
        }
    }
}
