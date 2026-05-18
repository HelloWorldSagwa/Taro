using Arcanum.Data;
using Arcanum.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arcanum.Cards
{
    public sealed class TarotCardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image cardImage;
        [SerializeField] private Image frameImage;
        [SerializeField] private Image glowImage;
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

            if (cardImage != null)
            {
                var resourcePath = IsRevealed && hasCard ? CardFacePath(Card) : "Art/Cards/CARD_BACK_DEFAULT";
                ArcanumUiFactory.ApplySprite(cardImage, resourcePath, IsRevealed && hasCard ? Card.AccentColor : ArcanumUiFactory.PanelDark);
                cardImage.preserveAspect = true;
            }

            if (frameImage != null)
            {
                frameImage.color = IsRevealed && hasCard ? Color.Lerp(Card.AccentColor, Color.white, 0.35f) : Color.white;
            }

            if (title != null)
            {
                title.text = IsRevealed && hasCard ? Card.DisplayName : "\uC544\uB974\uCE74\uB214";
            }

            if (subtitle != null)
            {
                subtitle.text = IsRevealed && hasCard ? $"\uBA54\uC774\uC800 {Card.Number:00}" : "\uC120\uD0DD\uD574\uC11C \uC5F4\uAE30";
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetGlow(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetGlow(false);
        }

        private void SetGlow(bool visible)
        {
            if (glowImage != null && !IsRevealed)
            {
                glowImage.enabled = visible;
            }
        }

        private static string CardFacePath(TarotCardDefinition card)
        {
            switch (card.CardId)
            {
                case "major_00_fool":
                    return "Art/Cards/CARD_MAJOR_00_FOOL_FACE";
                case "major_01_magician":
                    return "Art/Cards/CARD_MAJOR_01_MAGICIAN_FACE";
                case "major_06_lovers":
                    return "Art/Cards/CARD_MAJOR_06_LOVERS_FACE";
                case "major_13_death":
                    return "Art/Cards/CARD_MAJOR_13_DEATH_FACE";
                case "major_18_moon":
                    return "Art/Cards/CARD_MAJOR_18_MOON_FACE";
                case "major_21_world":
                    return "Art/Cards/CARD_MAJOR_21_WORLD_FACE";
                default:
                    return "Art/Cards/CARD_BACK_DEFAULT";
            }
        }

        public static TarotCardView Create(Transform parent, TarotCardDefinition card, bool revealed)
        {
            var root = new GameObject($"Card_{card.CardId}", typeof(RectTransform), typeof(Image), typeof(Button), typeof(TarotCardView));
            root.transform.SetParent(parent, false);

            var view = root.GetComponent<TarotCardView>();
            view.cardImage = root.GetComponent<Image>();
            view.cardImage.preserveAspect = true;

            var glowObject = new GameObject("HoverGlow", typeof(RectTransform), typeof(Image));
            glowObject.transform.SetParent(root.transform, false);
            var glowRect = glowObject.GetComponent<RectTransform>();
            glowRect.anchorMin = new Vector2(-0.11f, -0.08f);
            glowRect.anchorMax = new Vector2(1.11f, 1.08f);
            glowRect.offsetMin = Vector2.zero;
            glowRect.offsetMax = Vector2.zero;
            view.glowImage = glowObject.GetComponent<Image>();
            view.glowImage.raycastTarget = false;
            view.glowImage.enabled = false;
            ArcanumUiFactory.ApplySprite(view.glowImage, "Art/FX/FX_CARD_HOVER_GLOW", new Color(1f, 0.8f, 0.25f, 0.55f));

            var frameObject = new GameObject("Frame", typeof(RectTransform), typeof(Image));
            frameObject.transform.SetParent(root.transform, false);
            var frameRect = frameObject.GetComponent<RectTransform>();
            ArcanumUiFactory.Stretch(frameRect, 0, 0, 0, 0);
            view.frameImage = frameObject.GetComponent<Image>();
            view.frameImage.raycastTarget = false;
            view.frameImage.preserveAspect = false;
            ArcanumUiFactory.ApplySprite(view.frameImage, "Art/Cards/FRAME_TAROT_DEFAULT", Color.white);

            view.title = ArcanumUiFactory.CreateText(root.transform, "Title", string.Empty, 24, TextAnchor.MiddleCenter, Color.white);
            var titleRect = view.title.rectTransform;
            titleRect.anchorMin = new Vector2(0.08f, 0.06f);
            titleRect.anchorMax = new Vector2(0.92f, 0.16f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            view.subtitle = ArcanumUiFactory.CreateText(root.transform, "Subtitle", string.Empty, 17, TextAnchor.MiddleCenter, ArcanumUiFactory.Ink);
            var subtitleRect = view.subtitle.rectTransform;
            subtitleRect.anchorMin = new Vector2(0.10f, 0.86f);
            subtitleRect.anchorMax = new Vector2(0.90f, 0.95f);
            subtitleRect.offsetMin = Vector2.zero;
            subtitleRect.offsetMax = Vector2.zero;

            view.Bind(card, revealed);
            return view;
        }
    }
}
