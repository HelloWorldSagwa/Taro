using Arcanum.App;
using Arcanum.Master;
using Arcanum.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arcanum.Scenes
{
    public sealed class HomeTableController : MonoBehaviour
    {
        private bool initialized;

        private void Awake()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            var canvas = ArcanumUiFactory.CreateCanvas("HomeTableCanvas");
            ArcanumUiFactory.CreatePanel(canvas.transform, "Background", Vector2.zero, Vector2.one, ArcanumUiFactory.StageBlack);
            TarotMasterPresenter.Create(canvas.GetComponent<RectTransform>()).SetExpression("waiting");

            var table = ArcanumUiFactory.CreatePanel(canvas.transform, "Center_CardTable", ArcanumUiFactory.CenterColumnAnchorMin, ArcanumUiFactory.CenterColumnAnchorMax, ArcanumUiFactory.PanelGlass);
            var title = ArcanumUiFactory.CreateText(table, "Title", "ARCANUM STAGE", 52, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.06f, 0.73f);
            title.rectTransform.anchorMax = new Vector2(0.94f, 0.91f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var subtitle = ArcanumUiFactory.CreateText(table, "Subtitle", "A quiet one-card consultation for the next step in front of you.", 25, TextAnchor.MiddleCenter, ArcanumUiFactory.Ink);
            subtitle.rectTransform.anchorMin = new Vector2(0.10f, 0.52f);
            subtitle.rectTransform.anchorMax = new Vector2(0.90f, 0.66f);
            subtitle.rectTransform.offsetMin = Vector2.zero;
            subtitle.rectTransform.offsetMax = Vector2.zero;

            var button = ArcanumUiFactory.CreateButton(table, "TodayCardButton", "Begin Today's Reading", ArcanumUiFactory.Rose);
            var buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.20f, 0.23f);
            buttonRect.anchorMax = new Vector2(0.80f, 0.36f);
            buttonRect.offsetMin = Vector2.zero;
            buttonRect.offsetMax = Vector2.zero;
            button.onClick.AddListener(() => SceneFlow.Load(SceneFlow.Ritual));

            CreateGuidePanel(canvas.transform);

            CreateDialogue(canvas.transform, "Welcome back. When you are ready, I will open the table and guide the first card.");
        }

        private static void CreateGuidePanel(Transform parent)
        {
            var right = ArcanumUiFactory.CreatePanel(parent, "Right_SessionPanel", ArcanumUiFactory.RightColumnAnchorMin, ArcanumUiFactory.RightColumnAnchorMax, new Color(0.08f, 0.055f, 0.12f, 0.96f));

            var title = ArcanumUiFactory.CreateText(right, "GuideTitle", "TODAY'S RITUAL", 29, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.80f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.92f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var body = ArcanumUiFactory.CreateText(right, "GuideBody", "Mode: One-card reading\nFocus: practical signal\nFlow: choose, reveal, listen", 23, TextAnchor.UpperLeft, ArcanumUiFactory.Ink);
            body.rectTransform.anchorMin = new Vector2(0.10f, 0.38f);
            body.rectTransform.anchorMax = new Vector2(0.90f, 0.72f);
            body.rectTransform.offsetMin = Vector2.zero;
            body.rectTransform.offsetMax = Vector2.zero;
        }

        private static void CreateDialogue(Transform parent, string line)
        {
            ArcanumUiFactory.CreateDialogueBox(parent, "LUNA", line, out _);
        }
    }
}
