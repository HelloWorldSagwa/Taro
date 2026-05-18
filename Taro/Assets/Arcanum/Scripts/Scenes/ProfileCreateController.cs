using Arcanum.App;
using Arcanum.Master;
using Arcanum.Reading;
using Arcanum.UI;
using UnityEngine;
using UnityEngine.UI;
using ProfileSession = Arcanum.Profile.ProfileSession;

namespace Arcanum.Scenes
{
    public sealed class ProfileCreateController : MonoBehaviour
    {
        private InputField nameInput;
        private InputField birthMonthInput;
        private InputField focusInput;
        private Text validationText;
        private bool initialized;

        private void Awake()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            var canvas = ArcanumUiFactory.CreateCanvas("ProfileCreateCanvas");
            ArcanumUiFactory.CreateBackground(canvas.transform, "BG_PROFILE_ATELIER_16X9");
            TarotMasterPresenter.Create(canvas.GetComponent<RectTransform>()).SetExpression("focused");

            var panel = ArcanumUiFactory.CreateProfilePanel(canvas.transform, "Center_ProfileForm", ArcanumUiFactory.CenterColumnAnchorMin, ArcanumUiFactory.CenterColumnAnchorMax);
            var title = ArcanumUiFactory.CreateText(panel, "Title", "상담 프로필", 44, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.84f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.95f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            nameInput = CreateLabeledInput(panel, "Name", "이름", "예: 연우", 0.67f);
            birthMonthInput = CreateLabeledInput(panel, "BirthMonth", "탄생월", "예: 5월", 0.48f);
            focusInput = CreateLabeledInput(panel, "Focus", "오늘의 질문", "예: 일, 관계, 선택", 0.29f);

            validationText = ArcanumUiFactory.CreateText(panel, "Validation", string.Empty, 20, TextAnchor.MiddleCenter, ArcanumUiFactory.Rose);
            validationText.rectTransform.anchorMin = new Vector2(0.10f, 0.19f);
            validationText.rectTransform.anchorMax = new Vector2(0.90f, 0.25f);
            validationText.rectTransform.offsetMin = Vector2.zero;
            validationText.rectTransform.offsetMax = Vector2.zero;

            var submit = ArcanumUiFactory.CreateButton(panel, "SubmitButton", "타로 테이블로 이동", ArcanumUiFactory.Rose);
            var submitRect = submit.GetComponent<RectTransform>();
            submitRect.anchorMin = new Vector2(0.18f, 0.07f);
            submitRect.anchorMax = new Vector2(0.82f, 0.17f);
            submitRect.offsetMin = Vector2.zero;
            submitRect.offsetMax = Vector2.zero;
            submit.onClick.AddListener(Submit);

            CreateGuidePanel(canvas.transform);
            ArcanumUiFactory.CreateDialogueBox(canvas.transform, "루나", "이름 하나면 충분해요. 나머지는 오늘의 해석을 더 선명하게 만드는 단서입니다.", out _);
        }

        private static InputField CreateLabeledInput(Transform parent, string name, string label, string placeholder, float y)
        {
            var labelText = ArcanumUiFactory.CreateText(parent, name + "Label", label, 22, TextAnchor.MiddleLeft, ArcanumUiFactory.Gold);
            labelText.rectTransform.anchorMin = new Vector2(0.12f, y + 0.10f);
            labelText.rectTransform.anchorMax = new Vector2(0.88f, y + 0.16f);
            labelText.rectTransform.offsetMin = Vector2.zero;
            labelText.rectTransform.offsetMax = Vector2.zero;

            var inputObject = new GameObject(name + "Input", typeof(RectTransform), typeof(Image), typeof(InputField));
            inputObject.transform.SetParent(parent, false);
            ArcanumUiFactory.ApplyInputSkin(inputObject.GetComponent<Image>());

            var rect = inputObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.12f, y);
            rect.anchorMax = new Vector2(0.88f, y + 0.09f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var text = ArcanumUiFactory.CreateText(inputObject.transform, "Text", string.Empty, 24, TextAnchor.MiddleLeft, ArcanumUiFactory.Ink);
            ArcanumUiFactory.Stretch(text.rectTransform, 18, 4, -18, -4);
            text.supportRichText = false;

            var placeholderText = ArcanumUiFactory.CreateText(inputObject.transform, "Placeholder", placeholder, 23, TextAnchor.MiddleLeft, new Color(0.58f, 0.54f, 0.66f));
            ArcanumUiFactory.Stretch(placeholderText.rectTransform, 18, 4, -18, -4);

            var input = inputObject.GetComponent<InputField>();
            input.textComponent = text;
            input.placeholder = placeholderText;
            input.characterLimit = 24;
            return input;
        }

        private static void CreateGuidePanel(Transform parent)
        {
            var right = ArcanumUiFactory.CreateRightPanel(parent, "Right_ProfilePanel");
            var title = ArcanumUiFactory.CreateText(right, "GuideTitle", "입장 준비", 29, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.80f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.92f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var body = ArcanumUiFactory.CreateText(right, "GuideBody", "이름은 필수입니다.\n\n탄생월과 질문은 비워도 기본값으로 진행합니다.\n\n저장 후 바로 타로 테이블로 이동합니다.", 23, TextAnchor.UpperLeft, ArcanumUiFactory.Ink);
            body.rectTransform.anchorMin = new Vector2(0.10f, 0.28f);
            body.rectTransform.anchorMax = new Vector2(0.90f, 0.72f);
            body.rectTransform.offsetMin = Vector2.zero;
            body.rectTransform.offsetMax = Vector2.zero;
        }

        private void Submit()
        {
            if (nameInput == null || string.IsNullOrWhiteSpace(nameInput.text))
            {
                validationText.text = "이름을 입력해 주세요.";
                return;
            }

            ProfileSession.Create(nameInput.text, birthMonthInput.text, focusInput.text);
            ReadingSession.Clear();
            SceneFlow.Load(SceneFlow.HomeTable);
        }
    }
}
