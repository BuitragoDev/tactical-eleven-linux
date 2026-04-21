using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using System.Data.SQLite;
using System.Collections.Generic;

namespace TacticalEleven.Scripts
{
    public class CargarPartida : MonoBehaviour
    {
        [Header("Sound Clips")]
        [SerializeField] private AudioClip clickSFX;

        private ScrollView scrollSavedGames;
        private Button btnVolver, btnCargar, btnBorrar;
        private VisualElement popupContainer;
        private Button btnYes, btnCancel;


        private class SaveGameInfo
        {
            public string FilePath;
            public string ManagerNombre;
            public string EquipoNombre;
            public int IdEquipo;
            public VisualElement RowElement;
        }

        private SaveGameInfo partidaSeleccionada;

        void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            scrollSavedGames = root.Q<ScrollView>("saves-scrollview");
            // Solo scroll vertical
            scrollSavedGames.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            scrollSavedGames.mode = ScrollViewMode.Vertical;

            // Opcional pero recomendable:
            scrollSavedGames.style.overflow = Overflow.Hidden;

            popupContainer = root.Q<VisualElement>("popup-container");
            btnYes = root.Q<Button>("btnYes");
            btnCancel = root.Q<Button>("btnCancel");


            btnVolver = root.Q<Button>("btnVolver");
            btnCargar = root.Q<Button>("btnCargar");
            btnBorrar = root.Q<Button>("btnBorrar");
            btnCargar.SetEnabled(false);
            btnBorrar.SetEnabled(false);

            CargarTablaPartidas();

            btnVolver.clicked += () => SceneLoader.Instance.LoadScene(Constants.MAIN_MENU_SCENE);
            btnCargar.clicked += CargarPartidaSeleccionada;
            btnBorrar.clicked += () =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    if (partidaSeleccionada != null)
                        popupContainer.style.display = DisplayStyle.Flex;
                };
            btnYes.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                BorrarPartidaSeleccionada();
                popupContainer.style.display = DisplayStyle.None;
            };

            btnCancel.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                popupContainer.style.display = DisplayStyle.None;
            };
        }

        private void CargarTablaPartidas()
        {
            scrollSavedGames.Clear();

            // ============================
            // ENCABEZADO
            // ============================
            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.width = Length.Percent(100);
            header.style.height = 40;
            header.style.alignItems = Align.Center;
            header.style.backgroundColor = new Color(24f / 255f, 58f / 255f, 39f / 255f);

            header.Add(CrearCeldaImagen(true));
            header.Add(CrearCelda("MANAGER", true));
            header.Add(CrearCelda("EQUIPO", true));
            header.Add(CrearCelda("FECHA", true));
            scrollSavedGames.Add(header);

            // ============================
            // LEER ARCHIVOS GUARDADOS
            // ============================
            string savesPath = Path.Combine(Application.persistentDataPath, "SavedGames");
            if (!Directory.Exists(savesPath)) return;

            var saveFiles = Directory.GetFiles(savesPath, "*.db");
            var saveInfos = new List<SaveGameInfo>();
            bool filaPar = true;

            foreach (var saveFile in saveFiles)
            {
                try
                {
                    string connString = $"Data Source={saveFile};Version=3;";
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();

                        string query = @"SELECT m.nombre, m.apellido, m.id_equipo, 
                                                e.nombre AS equipoNombre
                                         FROM managers m
                                         LEFT JOIN equipos e ON m.id_equipo = e.id_equipo
                                         WHERE m.despedido = 0
                                         LIMIT 1";

                        using (var cmd = new SQLiteCommand(query, conn))
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Nombre del manager
                                string manager = reader.GetString(0) + " " + reader.GetString(1);

                                // ID del equipo (para cargar el escudo)
                                int idEquipo = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);

                                // Nombre del equipo (la columna 3)
                                string equipo = reader.IsDBNull(3) ? "—" : reader.GetString(3);

                                // Fecha en formato dd/MM/yyyy
                                string fecha = File.GetLastWriteTime(saveFile).ToString("dd/MM/yyyy");

                                // Crear la fila con escudo incluido
                                var fila = CrearFila(manager, equipo, fecha, filaPar, idEquipo);

                                var info = new SaveGameInfo
                                {
                                    FilePath = saveFile,
                                    ManagerNombre = manager,
                                    EquipoNombre = equipo,
                                    RowElement = fila,
                                    IdEquipo = idEquipo
                                };

                                saveInfos.Add(info);
                                scrollSavedGames.Add(fila);

                                filaPar = !filaPar;
                            }
                        }

                        conn.Close();
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error leyendo la partida {saveFile}: {ex.Message}");
                }
            }

            // ============================
            // SELECCIÓN DE FILA
            // ============================
            foreach (var save in saveInfos)
            {
                save.RowElement.RegisterCallback<ClickEvent>(evt =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    SeleccionarFila(save);
                    btnCargar.SetEnabled(true);
                    btnBorrar.SetEnabled(true);
                });
            }
        }

        // ============================
        // CREAR UNA FILA
        // ============================
        private VisualElement CrearFila(string manager, string equipo, string fecha, bool filaPar, int idEquipo)
        {
            var fila = new VisualElement();
            fila.style.flexDirection = FlexDirection.Row;
            fila.style.width = Length.Percent(100);
            fila.style.height = 74;
            fila.style.alignItems = Align.Center;
            fila.style.backgroundColor = filaPar
                ? new Color(1f, 1f, 1f)
                : new Color(229f / 255f, 229f / 255f, 229f / 255f);

            // ------- COLUMNA 1: ESCUDO -------
            fila.Add(CrearCeldaImagen(false, idEquipo));

            // ------- COLUMNA 2-4: TEXTO -------
            fila.Add(CrearCelda(manager));
            fila.Add(CrearCelda(equipo));
            fila.Add(CrearCelda(fecha));

            return fila;
        }

        // ============================
        // SELECCIONAR FILA
        // ============================
        private void SeleccionarFila(SaveGameInfo save)
        {
            // Restaurar color anterior
            if (partidaSeleccionada != null)
            {
                bool filaPar = (scrollSavedGames.IndexOf(partidaSeleccionada.RowElement) % 2 == 1);

                partidaSeleccionada.RowElement.style.backgroundColor = filaPar
                    ? Color.white
                    : new Color(229f / 255f, 229f / 255f, 229f / 255f);
            }

            // Marcar nueva
            save.RowElement.style.backgroundColor = new Color(180f / 255f, 240f / 255f, 180f / 255f);
            partidaSeleccionada = save;
        }

        // ============================
        // CREAR CELDA IMAGEN
        // ============================
        private VisualElement CrearCeldaImagen(bool esCabecera, int idEquipo = 0)
        {
            var cont = new VisualElement();
            cont.style.width = 74;
            cont.style.height = 74;
            cont.style.alignItems = Align.Center;
            cont.style.justifyContent = Justify.Center;
            cont.style.flexShrink = 0;

            if (esCabecera)
            {
                // Columna vacía
                return cont;
            }

            // Cargar el escudo
            Sprite escudo = Resources.Load<Sprite>($"EscudosEquipos/64x64/{idEquipo}");
            if (escudo != null)
            {
                var img = new Image();
                img.sprite = escudo;
                img.scaleMode = ScaleMode.ScaleToFit;
                img.style.width = 64;
                img.style.height = 64;
                cont.Add(img);
            }

            return cont;
        }

        // ============================
        // CARGAR PARTIDA
        // ============================
        private void CargarPartidaSeleccionada()
        {
            if (partidaSeleccionada == null)
            {
                Debug.LogWarning("No se ha seleccionado ninguna partida.");
                return;
            }

            DatabaseManager.SetActiveDatabase(partidaSeleccionada.FilePath);

            Debug.Log($"Partida seleccionada: {partidaSeleccionada.FilePath}");
            SceneLoader.Instance.LoadScene(Constants.MAIN_SCREEN_SCENE);
        }

        // ============================
        // CELDA
        // ============================
        private Label CrearCelda(string texto, bool esCabecera = false)
        {
            var label = new Label(texto);

            label.style.flexGrow = 1;
            label.style.flexBasis = Length.Percent(33.33f);
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.paddingLeft = 20;

            label.style.fontSize = 18;
            label.style.color = esCabecera ? Color.white : Color.black;
            label.style.unityFontStyleAndWeight = esCabecera ? FontStyle.Bold : FontStyle.Normal;

            return label;
        }

        private void BorrarPartidaSeleccionada()
        {
            if (partidaSeleccionada == null)
            {
                Debug.LogWarning("No hay ninguna partida seleccionada para borrar.");
                return;
            }

            string file = partidaSeleccionada.FilePath;

            // Confirmación opcional
            Debug.Log("Borrando archivo: " + file);

            try
            {
                if (File.Exists(file))
                    File.Delete(file);

                // Quitar visualmente la fila de la lista
                scrollSavedGames.Remove(partidaSeleccionada.RowElement);

                partidaSeleccionada = null;

                // Desactivar botones
                btnCargar.SetEnabled(false);
                btnBorrar.SetEnabled(false);

                // Recargar tabla para recolorear filas correctamente
                CargarTablaPartidas();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al borrar la partida: " + ex.Message);
            }
        }

    }
}