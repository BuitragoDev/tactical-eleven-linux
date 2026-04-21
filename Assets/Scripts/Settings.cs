using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class Settings : MonoBehaviour
    {
        [Header("Sound Clips")]
        [SerializeField] private AudioClip clickSFX;

        private ModernSlider sliderMusic, sliderSFX;
        private Button backButton, salirMenu, salirEscritorio, btnYes, btnCancel, btnEuro, btnPound, btnDollar,
                       btnCalidadAlta, btnCalidadMedia, btnCalidadBaja;
        private VisualElement popupContainer, sliderMusicContainer, sliderSFXContainer;

        void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // Contenedores vacíos donde pondremos los sliders
            sliderMusicContainer = root.Q<VisualElement>("slider-musica");
            sliderSFXContainer = root.Q<VisualElement>("slider-sfx");
            backButton = root.Q<Button>("btnVolver");
            salirMenu = root.Q<Button>("btnSalirMenu");
            salirEscritorio = root.Q<Button>("btnSalirEscritorio");
            btnEuro = root.Q<Button>("btnEuro");
            btnPound = root.Q<Button>("btnPound");
            btnDollar = root.Q<Button>("btnDollar");
            btnCalidadAlta = root.Q<Button>("btnCalidadAlta");
            btnCalidadMedia = root.Q<Button>("btnCalidadMedia");
            btnCalidadBaja = root.Q<Button>("btnCalidadBaja");

            popupContainer = root.Q<VisualElement>("popup-container");
            btnYes = root.Q<Button>("btnYes");
            btnCancel = root.Q<Button>("btnCancel");

            // Cargar Moneda guardada
            string moneda = PlayerPrefs.GetString("Currency", string.Empty);
            switch (moneda)
            {
                case Constants.EURO_NAME:
                    setEuro();
                    break;
                case Constants.POUND_NAME:
                    setPound();
                    break;
                case Constants.DOLLAR_NAME:
                    setDollar();
                    break;
            }

            // Cargar Calidad Gráficos Guardada
            int graficos = PlayerPrefs.GetInt("QualityLevel", -1);
            int indexHigh = Array.IndexOf(QualitySettings.names, "High");
            int indexMedium = Array.IndexOf(QualitySettings.names, "Medium");
            int indexLow = Array.IndexOf(QualitySettings.names, "Low");

            if (graficos == indexHigh)
            {
                setHighQuality();
            }
            else if (graficos == indexMedium)
            {
                setMediumQuality();
            }
            else if (graficos == indexLow)
            {
                setLowQuality();
            }

            // Crear sliders modernos
            sliderMusic = new ModernSlider(sliderMusicContainer);
            sliderSFX = new ModernSlider(sliderSFXContainer);

            // Inicializar con los valores actuales del AudioManager
            sliderMusic.SetValue(AudioManager.Instance.musicVolume);
            sliderSFX.SetValue(AudioManager.Instance.sfxVolume);

            // ---------------------------------------------------------- Eventos CALIDAD DE LOS GRÁFICOS
            btnCalidadAlta.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                setHighQuality();
            };

            btnCalidadMedia.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                setMediumQuality();
            };

            btnCalidadBaja.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                setLowQuality();
            };

            // ---------------------------------------------------------- Eventos SONIDOS
            // Vincular eventos de cambio
            sliderMusic.OnValueChanged += (value) =>
            {
                AudioManager.Instance.SetMusicVolume(value);
            };

            sliderSFX.OnValueChanged += (value) =>
            {
                AudioManager.Instance.SetSFXVolume(value);
            };

            // ---------------------------------------------------------- Eventos MONEDA
            btnEuro.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                setEuro();
            };

            btnPound.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                setPound();
            };

            btnDollar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                setDollar();
            };

            // ---------------------------------------------------------- Eventos SALIR DEL JUEGO
            salirMenu.clicked += () =>
            {
                SceneLoader.Instance.LoadScene(Constants.MAIN_MENU_SCENE);
            };

            salirEscritorio.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                popupContainer.style.display = DisplayStyle.Flex;
            };

            btnYes.clicked += () =>
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
            };

            btnCancel.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                popupContainer.style.display = DisplayStyle.None;
            };

            backButton.clicked += () =>
            {
                int settingParameter = SceneLoader.getSettingsParameter();
                if (settingParameter == 0)
                {
                    SceneLoader.Instance.LoadScene(Constants.MAIN_MENU_SCENE);
                }
                else if (settingParameter == 1)
                {
                    SceneLoader.Instance.LoadScene(Constants.MAIN_SCREEN_SCENE);
                }
            };
        }

        private void setEuro()
        {
            btnEuro.style.backgroundColor = new Color(0.2196f, 0.3059f, 0.2471f);   // verde
            btnPound.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f);  // gris
            btnDollar.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f); // gris
            PlayerPrefs.SetString("Currency", Constants.EURO_NAME);
        }

        private void setPound()
        {
            btnPound.style.backgroundColor = new Color(0.2196f, 0.3059f, 0.2471f);   // verde
            btnEuro.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f);  // gris
            btnDollar.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f); // gris
            PlayerPrefs.SetString("Currency", Constants.POUND_NAME);
        }

        private void setDollar()
        {
            btnDollar.style.backgroundColor = new Color(0.2196f, 0.3059f, 0.2471f);   // verde
            btnPound.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f);  // gris
            btnEuro.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f); // gris
            PlayerPrefs.SetString("Currency", Constants.DOLLAR_NAME);
        }

        private void setHighQuality()
        {
            btnCalidadAlta.style.backgroundColor = new Color(0.2196f, 0.3059f, 0.2471f);   // verde
            btnCalidadMedia.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f);  // gris
            btnCalidadBaja.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f); // gris

            int index = Array.IndexOf(QualitySettings.names, "High");
            QualitySettings.SetQualityLevel(index, true);
            PlayerPrefs.SetInt("QualityLevel", index);
        }

        private void setMediumQuality()
        {
            btnCalidadMedia.style.backgroundColor = new Color(0.2196f, 0.3059f, 0.2471f);   // verde
            btnCalidadAlta.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f);  // gris
            btnCalidadBaja.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f); // gris

            int index = Array.IndexOf(QualitySettings.names, "Medium");
            QualitySettings.SetQualityLevel(index, true);
            PlayerPrefs.SetInt("QualityLevel", index);
        }

        private void setLowQuality()
        {
            btnCalidadBaja.style.backgroundColor = new Color(0.2196f, 0.3059f, 0.2471f);   // verde
            btnCalidadMedia.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f);  // gris
            btnCalidadAlta.style.backgroundColor = new Color(0.2471f, 0.2549f, 0.2471f); // gris

            int index = Array.IndexOf(QualitySettings.names, "Low");
            QualitySettings.SetQualityLevel(index, true);
            PlayerPrefs.SetInt("QualityLevel", index);
        }
    }
}
