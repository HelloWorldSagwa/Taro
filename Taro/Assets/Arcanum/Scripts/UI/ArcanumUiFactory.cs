using Arcanum.Master;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Arcanum.UI
{
    public static class ArcanumUiFactory
    {
        public static readonly Color StageBlack = new Color(0.035f, 0.035f, 0.065f);
        public static readonly Color PanelDark = new Color(0.075f, 0.055f, 0.12f, 0.94f);
        public static readonly Color PanelGlass = new Color(0.055f, 0.043f, 0.09f, 0.97f);
        public static readonly Color DialogueBlack = new Color(0.015f, 0.015f, 0.032f, 0.97f);
        public static readonly Color Gold = new Color(0.86f, 0.67f, 0.29f);
        public static readonly Color Rose = new Color(0.82f, 0.26f, 0.48f);
        public static readonly Color Ink = new Color(0.92f, 0.9f, 0.86f);

        public static readonly Vector2 DialogueAnchorMin = new Vector2(0.025f, 0.03f);
        public static readonly Vector2 DialogueAnchorMax = new Vector2(0.975f, 0.205f);
        public static readonly Vector2 LeftColumnAnchorMin = new Vector2(0.025f, 0.235f);
        public static readonly Vector2 LeftColumnAnchorMax = new Vector2(0.305f, 0.955f);
        public static readonly Vector2 CenterColumnAnchorMin = new Vector2(0.325f, 0.235f);
        public static readonly Vector2 CenterColumnAnchorMax = new Vector2(0.725f, 0.955f);
        public static readonly Vector2 RightColumnAnchorMin = new Vector2(0.745f, 0.235f);
        public static readonly Vector2 RightColumnAnchorMax = new Vector2(0.975f, 0.955f);

        public static Canvas CreateCanvas(string name)
        {
            EnsureEventSystem();

            var canvasObject = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            return canvas;
        }

        public static RectTransform CreatePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Color color)
        {
            var panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);

            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            panel.GetComponent<Image>().color = color;
            return rect;
        }

        public static Text CreateText(Transform parent, string name, string value, int fontSize, TextAnchor anchor, Color color)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            var text = textObject.GetComponent<Text>();
            text.text = value;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.alignment = anchor;
            text.color = color;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            text.lineSpacing = 1.08f;
            text.raycastTarget = false;

            return text;
        }

        public static Button CreateButton(Transform parent, string name, string label, Color color)
        {
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            buttonObject.GetComponent<Image>().color = color;

            var button = buttonObject.GetComponent<Button>();
            var colors = button.colors;
            colors.highlightedColor = Color.Lerp(color, Color.white, 0.12f);
            colors.pressedColor = Color.Lerp(color, Color.black, 0.18f);
            colors.disabledColor = new Color(0.18f, 0.16f, 0.22f, 0.62f);
            button.colors = colors;

            var labelText = CreateText(buttonObject.transform, "Label", label, 24, TextAnchor.MiddleCenter, Color.white);
            Stretch(labelText.rectTransform, 20, 8, -20, -8);
            return button;
        }

        public static RectTransform CreateDialogueBox(Transform parent, string speaker, string line, out Text lineText)
        {
            var dialogue = CreatePanel(parent, "DialogueBox", DialogueAnchorMin, DialogueAnchorMax, DialogueBlack);

            var namePlate = CreatePanel(dialogue, "SpeakerPlate", new Vector2(0.03f, 0.68f), new Vector2(0.18f, 0.94f), new Color(0.18f, 0.12f, 0.25f, 0.98f));
            var speakerText = CreateText(namePlate, "Speaker", speaker, 21, TextAnchor.MiddleCenter, Gold);
            Stretch(speakerText.rectTransform, 10, 2, -10, -2);

            lineText = CreateText(dialogue, "Line", line, 29, TextAnchor.MiddleLeft, Ink);
            lineText.resizeTextForBestFit = true;
            lineText.resizeTextMinSize = 22;
            lineText.resizeTextMaxSize = 29;
            lineText.verticalOverflow = VerticalWrapMode.Overflow;
            Stretch(lineText.rectTransform, 44, 20, -44, -42);

            var prompt = CreateText(dialogue, "AdvanceHint", "클릭: 문장 바로 보기", 17, TextAnchor.MiddleRight, new Color(0.68f, 0.62f, 0.78f, 0.9f));
            prompt.rectTransform.anchorMin = new Vector2(0.72f, 0.05f);
            prompt.rectTransform.anchorMax = new Vector2(0.97f, 0.22f);
            prompt.rectTransform.offsetMin = Vector2.zero;
            prompt.rectTransform.offsetMax = Vector2.zero;

            var runner = dialogue.gameObject.AddComponent<DialogueRunner>();
            runner.Bind(lineText);
            runner.TypingStarted += () => TarotMasterPresenter.Current?.BeginSpeaking();
            runner.TypingCompleted += () => TarotMasterPresenter.Current?.EndSpeaking();
            runner.Show(line);

            var clickTarget = dialogue.gameObject.AddComponent<Button>();
            clickTarget.transition = Selectable.Transition.None;
            clickTarget.onClick.AddListener(runner.Complete);

            return dialogue;
        }

        public static void Stretch(RectTransform rect, float left, float bottom, float right, float top)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(right, top);
        }

        public static void SetInset(RectTransform rect, float xMin, float yMin, float xMax, float yMax)
        {
            rect.offsetMin = new Vector2(xMin, yMin);
            rect.offsetMax = new Vector2(-xMax, -yMax);
        }

        private static void EnsureEventSystem()
        {
            if (Object.FindAnyObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            Object.DontDestroyOnLoad(eventSystem);
        }
    }
}
