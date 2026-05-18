using System;
using UnityEngine.SceneManagement;

namespace Arcanum.App
{
    public static class SceneFlow
    {
        public const string Boot = "Boot";
        public const string MainMenu = "MainMenu";
        public const string ProfileCreate = "ProfileCreate";
        public const string HomeTable = "HomeTable";
        public const string Ritual = "Ritual";
        public const string ReadingResult = "ReadingResult";

        public static void Load(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                throw new ArgumentException("Scene name must be provided.", nameof(sceneName));
            }

            if (SceneManager.GetActiveScene().name == sceneName)
            {
                return;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}
