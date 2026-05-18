using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Arcanum.UI
{
    public sealed class DialogueRunner : MonoBehaviour
    {
        [SerializeField] private Text target;
        [SerializeField] private float characterDelay = 0.018f;

        private Coroutine activeRoutine;
        private string currentLine = string.Empty;

        public void Bind(Text text)
        {
            target = text;
        }

        public void Show(string line)
        {
            currentLine = line;

            if (activeRoutine != null)
            {
                StopCoroutine(activeRoutine);
            }

            activeRoutine = StartCoroutine(TypeLine(line));
        }

        public void Complete()
        {
            if (activeRoutine != null)
            {
                StopCoroutine(activeRoutine);
                activeRoutine = null;
            }

            if (target != null)
            {
                target.text = currentLine;
            }
        }

        private IEnumerator TypeLine(string line)
        {
            target.text = string.Empty;

            for (var i = 0; i < line.Length; i++)
            {
                target.text = line.Substring(0, i + 1);
                yield return new WaitForSeconds(characterDelay);
            }

            activeRoutine = null;
        }
    }
}
