using Arcanum.App;
using Arcanum.Master;
using Arcanum.Profile;
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

            var profile = ProfileSession.Current;
            var displayName = profile == null ? "손님" : profile.SafeDisplayName;
            var focus = profile == null ? "오늘의 흐름" : profile.SafeFocus;

            var table = ArcanumUiFactory.CreatePanel(canvas.transform, "Center_CardTable", ArcanumUiFactory.CenterColumnAnchorMin, ArcanumUiFactory.CenterColumnAnchorMax, ArcanumUiFactory.PanelGlass);
            var title = ArcanumUiFactory.CreateText(table, "Title", "타로 테이블", 52, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.06f, 0.73f);
            title.rectTransform.anchorMax = new Vector2(0.94f, 0.91f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var subtitle = ArcanumUiFactory.CreateText(table, "Subtitle", $"{displayName}님의 오늘 초점은 '{focus}'입니다.", 25, TextAnchor.MiddleCenter, ArcanumUiFactory.Ink);
            subtitle.rectTransform.anchorMin = new Vector2(0.10f, 0.52f);
            subtitle.rectTransform.anchorMax = new Vector2(0.90f, 0.66f);
            subtitle.rectTransform.offsetMin = Vector2.zero;
            subtitle.rectTransform.offsetMax = Vector2.zero;

            var button = ArcanumUiFactory.CreateButton(table, "TodayCardButton", "오늘의 카드 보기", ArcanumUiFactory.Rose);
            var buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.20f, 0.23f);
            buttonRect.anchorMax = new Vector2(0.80f, 0.36f);
            buttonRect.offsetMin = Vector2.zero;
            buttonRect.offsetMax = Vector2.zero;
            button.onClick.AddListener(() => SceneFlow.Load(SceneFlow.Ritual));

            CreateGuidePanel(canvas.transform);
            CreateDialogue(canvas.transform, "좋아요. 이제 테이블을 열게요. 카드를 고르는 순간부터 리딩은 시작됩니다.");
        }

        private static void CreateGuidePanel(Transform parent)
        {
            var right = ArcanumUiFactory.CreatePanel(parent, "Right_SessionPanel", ArcanumUiFactory.RightColumnAnchorMin, ArcanumUiFactory.RightColumnAnchorMax, new Color(0.08f, 0.055f, 0.12f, 0.96f));
            var title = ArcanumUiFactory.CreateText(right, "GuideTitle", "오늘의 리딩", 29, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.80f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.92f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var body = ArcanumUiFactory.CreateText(right, "GuideBody", "방식: 원 카드 리딩\n초점: 오늘의 신호\n흐름: 고르기, 뒤집기, 듣기", 23, TextAnchor.UpperLeft, ArcanumUiFactory.Ink);
            body.rectTransform.anchorMin = new Vector2(0.10f, 0.38f);
            body.rectTransform.anchorMax = new Vector2(0.90f, 0.72f);
            body.rectTransform.offsetMin = Vector2.zero;
            body.rectTransform.offsetMax = Vector2.zero;
        }

        private static void CreateDialogue(Transform parent, string line)
        {
            ArcanumUiFactory.CreateDialogueBox(parent, "루나", line, out _);
        }
    }
}
