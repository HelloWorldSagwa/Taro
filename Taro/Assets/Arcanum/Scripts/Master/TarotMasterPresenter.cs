using Arcanum.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arcanum.Master
{
    public sealed class TarotMasterPresenter : MonoBehaviour
    {
        [SerializeField] private Text expressionText;

        public void SetExpression(string token)
        {
            if (expressionText != null)
            {
                expressionText.text = LocalizeExpression(token);
            }
        }

        private static string LocalizeExpression(string token)
        {
            switch (token)
            {
                case "welcome":
                    return "맞이하는 중";
                case "listening":
                    return "듣는 중";
                case "waiting":
                    return "기다리는 중";
                case "focused":
                    return "집중하는 중";
                case "certain":
                    return "확신하는 중";
                case "calm":
                    return "차분함";
                default:
                    return token;
            }
        }

        public static TarotMasterPresenter Create(RectTransform parent)
        {
            var root = ArcanumUiFactory.CreatePanel(parent, "TarotMaster", ArcanumUiFactory.LeftColumnAnchorMin, ArcanumUiFactory.LeftColumnAnchorMax, new Color(0.14f, 0.08f, 0.18f, 0.96f));

            var halo = ArcanumUiFactory.CreatePanel(root, "PortraitGlow", new Vector2(0.15f, 0.14f), new Vector2(0.85f, 0.86f), new Color(0.52f, 0.2f, 0.58f, 0.28f));

            var title = ArcanumUiFactory.CreateText(root, "Name", "타로 마스터 루나", 34, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.82f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.94f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var portrait = ArcanumUiFactory.CreateText(halo, "PortraitPlaceholder", "루나 기본 컷\n차분한 표정\n상반신 포즈", 30, TextAnchor.MiddleCenter, Color.white);
            ArcanumUiFactory.Stretch(portrait.rectTransform, 18, 18, -18, -18);

            var expression = ArcanumUiFactory.CreateText(root, "Expression", "차분함", 24, TextAnchor.MiddleCenter, ArcanumUiFactory.Ink);
            expression.rectTransform.anchorMin = new Vector2(0.12f, 0.05f);
            expression.rectTransform.anchorMax = new Vector2(0.88f, 0.14f);
            expression.rectTransform.offsetMin = Vector2.zero;
            expression.rectTransform.offsetMax = Vector2.zero;

            var presenter = root.gameObject.AddComponent<TarotMasterPresenter>();
            presenter.expressionText = expression;
            return presenter;
        }
    }
}
