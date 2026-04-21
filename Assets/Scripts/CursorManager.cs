using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class CursorManager : MonoBehaviour
    {
        [Header("Cursor principal del juego")]
        [SerializeField] private Texture2D cursorTexture;
        [SerializeField] private Vector2 hotSpot = Vector2.zero;

        [Header("Cursores alternativos")]
        [SerializeField] private Texture2D cursorBusy;
        [SerializeField] private Texture2D cursorHand;
        [SerializeField] private Texture2D cursorError;

        [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

        private static CursorManager instance;
        private Texture2D currentCursor;

        private static bool cursorInitialized = false;

        private void Awake()
        {
            // Singleton persistente
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (!cursorInitialized)
            {
                SetCursor(cursorTexture);
                cursorInitialized = true;
            }
        }

        // Cambia el cursor a uno específico.
        public void SetCursor(Texture2D texture)
        {
            currentCursor = texture;
            if (texture != null)
                Cursor.SetCursor(texture, hotSpot, cursorMode);
            else
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }

        // Restaura el cursor normal del juego.
        public void SetDefaultCursor()
        {
            SetCursor(cursorTexture);
        }

        // Cambia a cursor de carga (“ocupado”).
        public void SetBusyCursor()
        {
            if (cursorBusy != null)
                SetCursor(cursorBusy);
        }

        // Cambia a cursor de “mano” o interacción.
        public void SetHandCursor()
        {
            if (cursorHand != null)
                SetCursor(cursorHand);
        }

        // Cambia a cursor de “error” o prohibido.
        public void SetErrorCursor()
        {
            if (cursorError != null)
                SetCursor(cursorError);
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }

        // Permite acceder fácilmente desde cualquier script:
        public static CursorManager Instance => instance;
    }
}