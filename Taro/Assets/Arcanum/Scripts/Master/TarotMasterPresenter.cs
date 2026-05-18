using System.Collections;
using Arcanum.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Arcanum.Master
{
    public sealed class TarotMasterPresenter : MonoBehaviour
    {
        private const string MasterResourceRoot = "Art/Masters/";

        [SerializeField] private RectTransform motionRoot;
        [SerializeField] private Image auraImage;
        [SerializeField] private Image portraitImage;
        [SerializeField] private Text fallbackPortraitText;
        [SerializeField] private Text expressionText;
        [SerializeField] private CanvasGroup portraitGroup;

        private Coroutine crossfadeRoutine;
        private string currentToken = "waiting";
        private string tokenBeforeSpeaking = "waiting";
        private bool speaking;

        public static TarotMasterPresenter Current { get; private set; }

        private void Start()
        {
            StartCoroutine(IdleBreathLoop());
            StartCoroutine(BlinkLoop());
        }

        private void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }
        }

        public void SetExpression(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                token = "waiting";
            }

            currentToken = token;
            speaking = string.Equals(token, "speaking", System.StringComparison.Ordinal);

            if (expressionText != null)
            {
                expressionText.text = LocalizeExpression(token);
            }

            ApplySpriteForToken(token);
            ApplyAuraForToken(token);
        }

        public void BeginSpeaking()
        {
            if (speaking)
            {
                return;
            }

            tokenBeforeSpeaking = currentToken;
            SetExpression("speaking");
        }

        public void EndSpeaking()
        {
            if (!speaking)
            {
                return;
            }

            SetExpression(tokenBeforeSpeaking);
        }

        public void PlayRevealGesture()
        {
            SetExpression("reveal");
            PulseAura(0.42f, 1.15f);
        }

        public void PlayResultEmphasis()
        {
            SetExpression("certain");
            PulseAura(0.34f, 1.08f);
        }

        private void ApplySpriteForToken(string token)
        {
            if (portraitImage == null)
            {
                return;
            }

            var sprite = LoadMasterSprite(AssetNameForToken(token));
            if (sprite == null && string.Equals(token, "speaking", System.StringComparison.Ordinal))
            {
                sprite = LoadMasterSprite(AssetNameForToken(tokenBeforeSpeaking));
            }

            if (sprite == null)
            {
                portraitImage.sprite = null;
                portraitImage.color = FallbackColorForToken(token);
                if (fallbackPortraitText != null)
                {
                    fallbackPortraitText.gameObject.SetActive(true);
                    fallbackPortraitText.text = FallbackPortraitLabel(token);
                }

                return;
            }

            portraitImage.sprite = sprite;
            portraitImage.color = Color.white;
            portraitImage.preserveAspect = true;
            if (fallbackPortraitText != null)
            {
                fallbackPortraitText.gameObject.SetActive(false);
            }

            if (crossfadeRoutine != null)
            {
                StopCoroutine(crossfadeRoutine);
            }

            crossfadeRoutine = StartCoroutine(CrossfadeIn());
        }

        private void ApplyAuraForToken(string token)
        {
            if (auraImage == null)
            {
                return;
            }

            var spriteName = string.Equals(token, "reveal", System.StringComparison.Ordinal)
                ? "FX_MASTER_AURA_REVEAL"
                : "FX_MASTER_AURA_IDLE";
            var sprite = LoadMasterSprite(spriteName);
            if (sprite != null)
            {
                auraImage.sprite = sprite;
                auraImage.preserveAspect = true;
                auraImage.color = Color.white;
                return;
            }

            auraImage.sprite = null;
            auraImage.color = string.Equals(token, "reveal", System.StringComparison.Ordinal)
                ? new Color(0.95f, 0.67f, 0.26f, 0.36f)
                : new Color(0.52f, 0.2f, 0.58f, 0.28f);
        }

        private IEnumerator IdleBreathLoop()
        {
            var t = 0f;
            while (true)
            {
                t += Time.deltaTime;
                var breathWave = (Mathf.Sin(t * Mathf.PI * 2f / 2.8f) + 1f) * 0.5f;
                var talkWave = (Mathf.Sin(t * Mathf.PI * 2f / 0.32f) + 1f) * 0.5f;
                var listenWave = Mathf.Sin(t * Mathf.PI * 2f / 5.2f);
                var y = Mathf.Lerp(-4f, 8f, breathWave) + (speaking ? Mathf.Lerp(0f, 5f, talkWave) : 0f);
                var scale = Mathf.Lerp(1f, speaking ? 1.018f : 1.012f, breathWave);
                var rotation = IsListeningToken(currentToken) ? listenWave * 0.6f : 0f;

                if (motionRoot != null)
                {
                    motionRoot.anchoredPosition = new Vector2(0f, y);
                    motionRoot.localScale = new Vector3(scale, scale, 1f);
                    motionRoot.localRotation = Quaternion.Euler(0f, 0f, rotation);
                }

                yield return null;
            }
        }

        private IEnumerator BlinkLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(4f, 7f));
                if (!speaking && !string.Equals(currentToken, "reveal", System.StringComparison.Ordinal))
                {
                    var before = currentToken;
                    ApplySpriteForToken("blink");
                    yield return new WaitForSeconds(0.12f);
                    ApplySpriteForToken(before);
                }
            }
        }

        private IEnumerator CrossfadeIn()
        {
            if (portraitGroup == null)
            {
                yield break;
            }

            portraitGroup.alpha = 0.72f;
            const float duration = 0.18f;
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                portraitGroup.alpha = Mathf.Lerp(0.72f, 1f, elapsed / duration);
                yield return null;
            }

            portraitGroup.alpha = 1f;
        }

        private void PulseAura(float alpha, float scale)
        {
            if (auraImage == null)
            {
                return;
            }

            auraImage.color = new Color(auraImage.color.r, auraImage.color.g, auraImage.color.b, alpha);
            auraImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
        }

        private static string AssetNameForToken(string token)
        {
            switch (token)
            {
                case "welcome":
                case "smile":
                    return "MASTER_LUNA_SMILE";
                case "focused":
                case "listening":
                    return "MASTER_LUNA_FOCUSED";
                case "reveal":
                    return "MASTER_LUNA_REVEAL_GESTURE";
                case "certain":
                case "serious":
                    return "MASTER_LUNA_SERIOUS";
                case "reassurance":
                    return "MASTER_LUNA_REASSURANCE";
                case "blink":
                    return "MASTER_LUNA_IDLE_BLINK";
                case "speaking":
                case "waiting":
                case "calm":
                default:
                    return "MASTER_LUNA_IDLE_CALM";
            }
        }

        private static Sprite LoadMasterSprite(string assetName)
        {
            var resourcePath = MasterResourceRoot + assetName;
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

        private static bool IsListeningToken(string token)
        {
            return string.Equals(token, "waiting", System.StringComparison.Ordinal)
                || string.Equals(token, "listening", System.StringComparison.Ordinal)
                || string.Equals(token, "focused", System.StringComparison.Ordinal);
        }

        private static Color FallbackColorForToken(string token)
        {
            switch (token)
            {
                case "reveal":
                    return new Color(0.75f, 0.42f, 0.18f, 0.96f);
                case "focused":
                    return new Color(0.20f, 0.17f, 0.42f, 0.96f);
                case "certain":
                case "serious":
                    return new Color(0.18f, 0.21f, 0.32f, 0.96f);
                case "speaking":
                    return new Color(0.36f, 0.16f, 0.38f, 0.96f);
                default:
                    return new Color(0.14f, 0.08f, 0.18f, 0.96f);
            }
        }

        private static string FallbackPortraitLabel(string token)
        {
            switch (token)
            {
                case "reveal":
                    return "루나\n카드 공개 제스처";
                case "focused":
                    return "루나\n집중하는 표정";
                case "certain":
                    return "루나\n확신하는 표정";
                case "speaking":
                    return "루나\n말하는 중";
                case "blink":
                    return "루나\n눈 깜빡임";
                default:
                    return "루나\n대기 모션";
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
                case "speaking":
                    return "말하는 중";
                case "reveal":
                    return "카드를 여는 중";
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
            var root = ArcanumUiFactory.CreatePanel(parent, "TarotMaster", ArcanumUiFactory.LeftColumnAnchorMin, ArcanumUiFactory.LeftColumnAnchorMax, new Color(0.08f, 0.045f, 0.12f, 0.90f));

            var motion = new GameObject("MotionRoot", typeof(RectTransform));
            motion.transform.SetParent(root, false);
            var motionRect = motion.GetComponent<RectTransform>();
            motionRect.anchorMin = Vector2.zero;
            motionRect.anchorMax = Vector2.one;
            motionRect.offsetMin = Vector2.zero;
            motionRect.offsetMax = Vector2.zero;

            var halo = ArcanumUiFactory.CreatePanel(motionRect, "PortraitAura", new Vector2(0.05f, 0.04f), new Vector2(0.95f, 0.92f), new Color(0.52f, 0.2f, 0.58f, 0.28f));
            var aura = halo.GetComponent<Image>();

            var portraitObject = new GameObject("Portrait", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
            portraitObject.transform.SetParent(motionRect, false);
            var portraitRect = portraitObject.GetComponent<RectTransform>();
            portraitRect.anchorMin = new Vector2(0.02f, 0.08f);
            portraitRect.anchorMax = new Vector2(0.98f, 0.88f);
            portraitRect.offsetMin = Vector2.zero;
            portraitRect.offsetMax = Vector2.zero;
            var portraitImage = portraitObject.GetComponent<Image>();
            portraitImage.color = new Color(0.14f, 0.08f, 0.18f, 0.96f);
            portraitImage.preserveAspect = true;

            var title = ArcanumUiFactory.CreateText(root, "Name", "타로 마스터 루나", 34, TextAnchor.MiddleCenter, ArcanumUiFactory.Gold);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.84f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.96f);
            title.rectTransform.offsetMin = Vector2.zero;
            title.rectTransform.offsetMax = Vector2.zero;

            var portrait = ArcanumUiFactory.CreateText(portraitObject.transform, "PortraitFallback", "루나\n대기 모션", 30, TextAnchor.MiddleCenter, Color.white);
            ArcanumUiFactory.Stretch(portrait.rectTransform, 18, 18, -18, -18);

            var expression = ArcanumUiFactory.CreateText(root, "Expression", "차분함", 24, TextAnchor.MiddleCenter, ArcanumUiFactory.Ink);
            expression.rectTransform.anchorMin = new Vector2(0.12f, 0.05f);
            expression.rectTransform.anchorMax = new Vector2(0.88f, 0.14f);
            expression.rectTransform.offsetMin = Vector2.zero;
            expression.rectTransform.offsetMax = Vector2.zero;

            var presenter = root.gameObject.AddComponent<TarotMasterPresenter>();
            presenter.motionRoot = motionRect;
            presenter.auraImage = aura;
            presenter.portraitImage = portraitImage;
            presenter.fallbackPortraitText = portrait;
            presenter.expressionText = expression;
            presenter.portraitGroup = portraitObject.GetComponent<CanvasGroup>();
            Current = presenter;
            presenter.SetExpression("waiting");
            return presenter;
        }
    }
}
