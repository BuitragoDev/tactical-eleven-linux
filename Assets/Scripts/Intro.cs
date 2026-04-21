using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

namespace TacticalEleven.Scripts
{
    public class Intro : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private float changeInterval = 2f; // segundos entre frases
        [SerializeField] private AudioClip introMusicClip;

        private Label loadingSentenceLabel;
        private ProgressBar loadingProgressBar;

        private readonly string[] loadingMessages =
        {
            "Inicializando jugadores...",
            "Inicializando datos de partida...",
            "Inicializando ligas..."
        };

        void Start()
        {
            // Reproducir música de fondo de la intro
            if (introMusicClip != null && AudioManager.Instance != null)
                AudioManager.Instance.PlayMusic(introMusicClip);
        }

        private void OnEnable()
        {
            var root = uiDocument.rootVisualElement;
            loadingSentenceLabel = root.Q<Label>("loading-sentence");
            loadingProgressBar = root.Q<ProgressBar>("progress-bar");

            if (loadingSentenceLabel != null && loadingProgressBar != null)
            {
                loadingProgressBar.lowValue = 0;
                loadingProgressBar.highValue = 1;
                loadingProgressBar.value = 0;

                StartCoroutine(ChangeLoadingTextWithProgress());
            }
            else
            {
                Debug.LogWarning("No se encontró el Label o ProgressBar en el UXML.");
            }
        }

        private IEnumerator ChangeLoadingTextWithProgress()
        {
            var remainingMessages = new List<string>(loadingMessages);
            int totalMessages = remainingMessages.Count;
            int shownCount = 0;

            while (remainingMessages.Count > 0)
            {
                int index = Random.Range(0, remainingMessages.Count);
                string newText = remainingMessages[index];

                loadingSentenceLabel.text = newText;
                remainingMessages.RemoveAt(index);
                shownCount++;

                float targetValue = (float)shownCount / totalMessages;

                float duration = changeInterval;
                float elapsed = 0f;
                float startValue = loadingProgressBar.value;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    loadingProgressBar.value = Mathf.Lerp(startValue, targetValue, elapsed / duration);
                    yield return null;
                }

                loadingProgressBar.value = targetValue;
            }

            loadingProgressBar.value = 1f;
            yield return new WaitForSeconds(0.3f);

            SceneLoader.Instance.LoadScene(Constants.MAIN_MENU_SCENE);
        }
    }
}