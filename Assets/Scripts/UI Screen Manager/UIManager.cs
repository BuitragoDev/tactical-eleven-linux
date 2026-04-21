using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private VisualElement mainContainer;
        private VisualElement pantallaActual;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Llamado desde MainScreen para asignar el contenedor principal.
        public void SetMainContainer(VisualElement container)
        {
            mainContainer = container;
        }

        // Carga un UXML dentro del contenedor y ejecuta un callback con la instancia creada.
        public void CargarPantalla(string rutaUxml, Action<VisualElement> onLoaded = null)
        {
            if (mainContainer == null)
            {
                Debug.LogError("UIManager → mainContainer NO está asignado.");
                return;
            }

            mainContainer.Clear();

            var uxml = Resources.Load<VisualTreeAsset>(rutaUxml);

            if (uxml == null)
            {
                Debug.LogError($"UIManager → No se encontró la pantalla: {rutaUxml}");
                return;
            }

            pantallaActual = uxml.Instantiate();

            // Ajustar tamaño para que ocupe todo el contenedor
            pantallaActual.style.flexGrow = 1;
            pantallaActual.style.flexShrink = 1;
            pantallaActual.style.flexBasis = Length.Percent(100);
            pantallaActual.style.width = Length.Percent(100);
            pantallaActual.style.height = Length.Percent(100);

            mainContainer.Add(pantallaActual);

            onLoaded?.Invoke(pantallaActual);
        }
    }
}