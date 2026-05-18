using UnityEngine;

namespace Arcanum.App
{
    public sealed class BootController : MonoBehaviour
    {
        private void Start()
        {
            AppShell.EnsureInitialized();
            SceneFlow.Load(SceneFlow.MainMenu);
        }
    }
}
