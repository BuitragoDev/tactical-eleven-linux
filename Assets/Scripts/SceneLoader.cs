#nullable enable

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TacticalEleven.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;
        public static int settingsParameter;

        [Header("Opcional: sonido de transición, y click")]
        public AudioClip clickSFX;
        public AudioClip transitionSFX;

        private void Awake()
        {
            // Patrón Singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        /// <summary>
        /// Carga una escena por su nombre.
        /// </summary>
        public void LoadScene(string sceneName)
        {
            // Reproducir sonido de transición (si existe)
            if (transitionSFX != null && AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(transitionSFX);

            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Recarga la escena actual.
        /// </summary>
        public void ReloadCurrentScene()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            LoadScene(currentScene);
        }

        /// <summary>
        /// Sale del juego (funciona en build, no en el editor).
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("Saliendo del juego...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        public static void setSettingsParameter(int value)
        {
            settingsParameter = value;
        }

        public static int getSettingsParameter()
        {
            return settingsParameter;
        }
    }
}