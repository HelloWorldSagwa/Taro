using UnityEngine;

namespace Arcanum.App
{
    public sealed class AppShell : MonoBehaviour
    {
        private static AppShell instance;

        public static AppShell Instance => instance;

        public static AppShell EnsureInitialized()
        {
            if (instance != null)
            {
                return instance;
            }

            var existingShell = FindFirstObjectByType<AppShell>();
            if (existingShell != null)
            {
                instance = existingShell;
                instance.ConfigureRuntime();
                return instance;
            }

            return new GameObject("AppShell").AddComponent<AppShell>();
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            ConfigureRuntime();
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        private void ConfigureRuntime()
        {
            Application.targetFrameRate = 60;

            if (Screen.width != 1920 || Screen.height != 1080 || Screen.fullScreenMode != FullScreenMode.Windowed)
            {
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
            }
        }
    }
}
