using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TacticalEleven.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        private VisualElement root;

        void OnEnable()
        {
            SceneLoader.setSettingsParameter(0);

            // Obtenemos root del UIDocument
            var uiDocument = GetComponent<UIDocument>();
            root = uiDocument.rootVisualElement;

            // Iconos del menú
            var managerMode = root.Q<VisualElement>(Constants.MANAGER_MODE_BUTTON);
            var careerMode = root.Q<VisualElement>(Constants.CAREER_MODE_BUTTON);
            var settings = root.Q<VisualElement>(Constants.SETTINGS_BUTTON);
            var editor = root.Q<VisualElement>(Constants.EDITOR_BUTTON);
            var cargarPartida = root.Q<VisualElement>(Constants.CARGAR_BUTTON);

            var exitIcon = root.Q<VisualElement>(Constants.EXIT_BUTTON);
            var creditsIcon = root.Q<VisualElement>(Constants.CREDITS_BUTTON);
            var webIcon = root.Q<VisualElement>(Constants.WEB_BUTTON);

            // --- MODO MANAGER ICON ---
            managerMode.RegisterCallback<ClickEvent>((evt) =>
            {
                if (SceneLoader.Instance != null && SceneLoader.Instance.transitionSFX != null && AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX(SceneLoader.Instance.transitionSFX);

                // Cambiar a la escena de Modo Manager
                if (SceneLoader.Instance != null)
                {
                    SceneLoader.Instance.LoadScene(Constants.CREATE_MANAGER_SCENE);
                }
                else
                {
                    Debug.LogWarning("SceneLoader.Instance no encontrado. Asegúrate de que exista en la escena inicial.");
                }
            });

            // --- AJUSTES ICON ICON ---
            settings.RegisterCallback<ClickEvent>((evt) =>
            {
                if (SceneLoader.Instance != null && SceneLoader.Instance.transitionSFX != null && AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX(SceneLoader.Instance.transitionSFX);

                // Cambiar a la escena de Modo Manager
                if (SceneLoader.Instance != null)
                {
                    SceneLoader.Instance.LoadScene(Constants.SETTINGS_SCREEN_SCENE);
                }
                else
                {
                    Debug.LogWarning("SceneLoader.Instance no encontrado. Asegúrate de que exista en la escena inicial.");
                }
            });

            // --- CARGAR PARTIDA ICON ---
            cargarPartida.RegisterCallback<ClickEvent>((evt) =>
            {
                if (SceneLoader.Instance != null && SceneLoader.Instance.transitionSFX != null && AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX(SceneLoader.Instance.transitionSFX);

                // Cambiar a la escena de Modo Manager
                if (SceneLoader.Instance != null)
                {
                    SceneLoader.Instance.LoadScene(Constants.LOAD_SCREEN_SCENE);
                }
                else
                {
                    Debug.LogWarning("SceneLoader.Instance no encontrado. Asegúrate de que exista en la escena inicial.");
                }
            });

            // --- EXIT ICON ---
            exitIcon.RegisterCallback<ClickEvent>((evt) =>
            {
                if (SceneLoader.Instance != null && SceneLoader.Instance.transitionSFX != null && AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX(SceneLoader.Instance.transitionSFX);

#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
            });

            // --- CREDITS ICON ---
            creditsIcon.RegisterCallback<ClickEvent>((evt) =>
            {
                if (SceneLoader.Instance != null && SceneLoader.Instance.transitionSFX != null && AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX(SceneLoader.Instance.transitionSFX);

                // Cambiar a la escena de créditos
                if (SceneLoader.Instance != null)
                {
                    SceneLoader.Instance.LoadScene(Constants.CREDITS_SCENE);
                }
                else
                {
                    Debug.LogWarning("SceneLoader.Instance no encontrado. Asegúrate de que exista en la escena inicial.");
                }
            });

            // --- WEB ICON ---
            webIcon.RegisterCallback<ClickEvent>((evt) =>
            {
                if (SceneLoader.Instance != null && SceneLoader.Instance.clickSFX != null && AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX(SceneLoader.Instance.clickSFX);

                Application.OpenURL("https://www.antoniobuitrago.es");
            });
        }
    }
}