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

        private const string PrimaryButtonSprite = "Art/UI/UI_BUTTON_PRIMARY_GOLD_IDLE";
        private const string SecondaryButtonSprite = "Art/UI/UI_BUTTON_SECONDARY_GLASS_IDLE";
        private const string DialogueBoxSprite = "Art/UI/UI_DIALOGUE_BOX_JRPG_9SLICE";
        private const string NameplateSprite = "Art/UI/UI_NAMEPLATE_MASTER_KO_9SLICE";
        private const string RightPanelSprite = "Art/UI/UI_PANEL_RIGHT_RESULT_9SLICE";
        private const string ProfilePanelSprite = "Art/UI/UI_PANEL_PROFILE_FORM_9SLICE";
        private const string InputSprite = "Art/UI/UI_INPUT_NAMEPLATE_KO_9SLICE";

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

        public static RectTransform CreateBackground(Transform parent, string assetName)
        {
            var background = CreatePanel(parent, "Background", Vector2.zero, Vector2.one, StageBlack);
            ApplySprite(background.GetComponent<Image>(), $"Art/Backgrounds/{assetName}", Color.white);
            background.SetAsFirstSibling();
            return background;
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

        public static RectTransform CreateSkinnedPanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Color fallbackColor, string resourcePath)
        {
            var panel = CreatePanel(parent, name, anchorMin, anchorMax, fallbackColor);
            ApplySprite(panel.GetComponent<Image>(), resourcePath, Color.white);
            return panel;
        }

        public static RectTransform CreateRightPanel(Transform parent, string name)
        {
            return CreateSkinnedPanel(parent, name, RightColumnAnchorMin, RightColumnAnchorMax, new Color(0.08f, 0.055f, 0.12f, 0.96f), RightPanelSprite);
        }

        public static RectTransform CreateProfilePanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax)
        {
            return CreateSkinnedPanel(parent, name, anchorMin, anchorMax, PanelGlass, ProfilePanelSprite);
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
            return CreateButton(parent, name, label, color, PrimaryButtonSprite);
        }

        public static Button CreateSecondaryButton(Transform parent, string name, string label)
        {
            return CreateButton(parent, name, label, new Color(0.32f, 0.2f, 0.45f), SecondaryButtonSprite);
        }

        private static Button CreateButton(Transform parent, string name, string label, Color color, string spritePath)
        {
            var buttonObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObject.transform.SetParent(parent, false);
            ApplySprite(buttonObject.GetComponent<Image>(), spritePath, color);

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
            ApplySprite(dialogue.GetComponent<Image>(), DialogueBoxSprite, Color.white);

            var namePlate = CreatePanel(dialogue, "SpeakerPlate", new Vector2(0.03f, 0.68f), new Vector2(0.18f, 0.94f), new Color(0.18f, 0.12f, 0.25f, 0.98f));
            ApplySprite(namePlate.GetComponent<Image>(), NameplateSprite, Color.white);
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

        public static void ApplyInputSkin(Image image)
        {
            ApplySprite(image, InputSprite, new Color(0.13f, 0.10f, 0.18f, 0.98f));
        }

        public static void ApplySprite(Image image, string resourcePath, Color fallbackColor)
        {
            if (image == null)
            {
                return;
            }

            var sprite = LoadSprite(resourcePath);
            if (sprite == null)
            {
                image.sprite = null;
                image.color = fallbackColor;
                return;
            }

            image.sprite = sprite;
            image.color = Color.white;
            image.preserveAspect = false;
        }

        public static Sprite LoadSprite(string resourcePath)
        {
            if (string.IsNullOrEmpty(resourcePath))
            {
                return null;
            }

            var sprite = Resources.Load<Sprite>(resourcePath);
            if (sprite != null)
            {
                return sprite;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return null;
            }

            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
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
