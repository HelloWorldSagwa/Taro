using Arcanum.App;
using Arcanum.Master;
using Arcanum.Profile;
using Arcanum.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arcanum.Scenes
{
    public sealed class MainMenuController : MonoBehaviour
    {
        private bool initialized;

        private void Awake()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            var canvas = ArcanumUiFactory.CreateCanvas("MainMenuCanvas");
            ArcanumUiFactory.CreatePanel(canvas.transform, "Background", Vector2.zero, Vector2.one, ArcanumUiFactory.StageBlack);
            TarotMasterPresenter.Create(canvas.GetComponent<RectTransform>()).SetExpression("waiting");

            var profile = ProfileSession.Current;
            var panel = ArcanumUiFactory.CreatePanel(canvas.transform, "Center_MainMenu", ArcanumUiFactory.CenterColumnAnchorMin, ArcanumUiFactory.CenterColumnAnchorMax, ArcanumUiFactory.PanelGlass);
            var title = ArcanumUiFactory.CreateText(panel, "Title", "아르카눔 스테이지", 50, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.06f, 0.74f);
            title.rectTransform.anchorMax = new Vector2(0.94f, 0.91f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var subtitleValue = profile == null
                ? "상담 프로필을 만들고 오늘의 운명을 펼쳐 보세요."
                : $"{profile.SafeDisplayName}님, 오늘의 흐름을 다시 확인해 볼까요?";
            var subtitle = ArcanumUiFactory.CreateText(panel, "Subtitle", subtitleValue, 25, TextAnchor.MiddleCenter, ArcanumUiFactory.Ink);
            subtitle.rectTransform.anchorMin = new Vector2(0.10f, 0.56f);
            subtitle.rectTransform.anchorMax = new Vector2(0.90f, 0.68f);
            subtitle.rectTransform.offsetMin = Vector2.zero;
            subtitle.rectTransform.offsetMax = Vector2.zero;

            var start = ArcanumUiFactory.CreateButton(panel, "StartButton", profile == null ? "상담 준비하기" : "타로 보러 가기", ArcanumUiFactory.Rose);
            var startRect = start.GetComponent<RectTransform>();
            startRect.anchorMin = new Vector2(0.20f, 0.34f);
            startRect.anchorMax = new Vector2(0.80f, 0.47f);
            startRect.offsetMin = Vector2.zero;
            startRect.offsetMax = Vector2.zero;
            start.onClick.AddListener(() => SceneFlow.Load(profile == null ? SceneFlow.ProfileCreate : SceneFlow.HomeTable));

            var newProfile = ArcanumUiFactory.CreateButton(panel, "NewProfileButton", "새 프로필로 시작", new Color(0.32f, 0.2f, 0.45f));
            var newProfileRect = newProfile.GetComponent<RectTransform>();
            newProfileRect.anchorMin = new Vector2(0.20f, 0.18f);
            newProfileRect.anchorMax = new Vector2(0.80f, 0.30f);
            newProfileRect.offsetMin = Vector2.zero;
            newProfileRect.offsetMax = Vector2.zero;
            newProfile.onClick.AddListener(() =>
            {
                ProfileSession.Clear();
                SceneFlow.Load(SceneFlow.ProfileCreate);
            });
            newProfile.gameObject.SetActive(profile != null);

            CreateInfoPanel(canvas.transform, profile);
            ArcanumUiFactory.CreateDialogueBox(canvas.transform, "루나", "어서 오세요. 먼저 당신을 부를 이름을 알려 주세요.", out _);
        }

        private static void CreateInfoPanel(Transform parent, ProfileData profile)
        {
            var right = ArcanumUiFactory.CreatePanel(parent, "Right_MenuPanel", ArcanumUiFactory.RightColumnAnchorMin, ArcanumUiFactory.RightColumnAnchorMax, new Color(0.08f, 0.055f, 0.12f, 0.96f));
            var title = ArcanumUiFactory.CreateText(right, "GuideTitle", "오늘의 입장", 29, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.80f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.92f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var bodyValue = profile == null
                ? "1. 상담 프로필 생성\n2. 타로 테이블 입장\n3. 카드 선택과 해석"
                : $"이름: {profile.SafeDisplayName}\n탄생월: {profile.SafeBirthMonth}\n관심사: {profile.SafeFocus}";
            var body = ArcanumUiFactory.CreateText(right, "GuideBody", bodyValue, 23, TextAnchor.UpperLeft, ArcanumUiFactory.Ink);
            body.rectTransform.anchorMin = new Vector2(0.10f, 0.36f);
            body.rectTransform.anchorMax = new Vector2(0.90f, 0.72f);
            body.rectTransform.offsetMin = Vector2.zero;
            body.rectTransform.offsetMax = Vector2.zero;
        }
    }
}
