using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class TeamObjectives : MonoBehaviour
    {
        [Header("Sound Clips")]
        [SerializeField] private AudioClip clickSFX;

        // UI Elements
        private VisualElement miEquipoEscudo, division1Logo, division2Logo, tablaContainer;
        private Button btnSeguir;
        private Label miEquipoNombre, miObjetivo;
        private ProgressBar progressBar;

        // Mi Equipo seleccionado
        int idEquipoSeleccionado, miCompeticion;

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // Referencias a elementos pantalla principal
            tablaContainer = root.Q<VisualElement>("left");
            miEquipoEscudo = root.Q<VisualElement>("miEquipo-escudo");
            division1Logo = root.Q<VisualElement>("division1-logo");
            division2Logo = root.Q<VisualElement>("division2-logo");
            btnSeguir = root.Q<Button>("btnSeguir");
            miEquipoNombre = root.Q<Label>("miEquipo-nombre");
            miObjetivo = root.Q<Label>("miObjetivo");

            progressBar = root.Q<ProgressBar>("loading");
            progressBar.style.visibility = Visibility.Hidden;

            idEquipoSeleccionado = PlayerPrefs.GetInt("MyTeam", -1);
            miCompeticion = EquipoData.ObtenerDetallesEquipo(idEquipoSeleccionado).IdCompeticion;
            if (miCompeticion == 1)
            {
                division1Logo.SetEnabled(false);
                SetLogoSprite(division1Logo, "LogosCompeticiones/1off");
                SetLogoSprite(division2Logo, "LogosCompeticiones/2");
            }
            else if (miCompeticion == 2)
            {
                division2Logo.SetEnabled(false);
                SetLogoSprite(division1Logo, "LogosCompeticiones/1");
                SetLogoSprite(division2Logo, "LogosCompeticiones/2off");
            }

            List<Equipo> equipos = EquipoData.GetObjetivosEquiposPorCompeticion(miCompeticion);
            foreach (var equipo in equipos)
            {
                if (equipo.IdEquipo == idEquipoSeleccionado)
                {
                    // Mostrar mi equipo aparte
                    var sprite = Resources.Load<Sprite>($"EscudosEquipos/{equipo.IdEquipo}");
                    if (sprite != null)
                        miEquipoEscudo.style.backgroundImage = new StyleBackground(sprite);

                    miEquipoNombre.text = equipo.Nombre;
                    miObjetivo.text = equipo.Objetivo;
                }
            }

            btnSeguir.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                btnSeguir.SetEnabled(false);
                StartCoroutine(GenerarTemporada());
            };

            // Click en división 1
            division1Logo.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                // Cambiar sprites de los logos
                SetLogoSprite(division1Logo, "LogosCompeticiones/1off");
                SetLogoSprite(division2Logo, "LogosCompeticiones/2");

                division1Logo.SetEnabled(false);
                division2Logo.SetEnabled(true);

                // Obtener equipos
                equipos = EquipoData.GetObjetivosEquiposPorCompeticion(1);
                CargarTabla(equipos);
            });

            // Click en división 2
            division2Logo.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                // Cambiar sprites de los logos
                SetLogoSprite(division1Logo, "LogosCompeticiones/1");
                SetLogoSprite(division2Logo, "LogosCompeticiones/2off");

                division1Logo.SetEnabled(true);
                division2Logo.SetEnabled(false);

                // Obtener equipos
                equipos = EquipoData.GetObjetivosEquiposPorCompeticion(2);
                CargarTabla(equipos);
            });

            CargarTabla(equipos);
        }

        private void CargarTabla(List<Equipo> equipos)
        {
            // Limpia la tabla actual
            tablaContainer.Clear();

            // Encabezado
            VisualElement header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.width = Length.Percent(100);
            header.style.backgroundColor = new Color(24f / 255f, 58f / 255f, 39f / 255f);
            header.style.marginBottom = 4;
            header.style.height = 30;

            header.Add(CrearCelda("EQUIPO", true));
            header.Add(CrearCelda("ENTRENADOR", true));
            header.Add(CrearCelda("OBJETIVO", true));

            tablaContainer.Add(header);

            int filaIndex = 0;
            foreach (var equipo in equipos)
            {
                if (equipo.IdEquipo != idEquipoSeleccionado)
                {
                    VisualElement fila = new VisualElement();
                    fila.style.flexDirection = FlexDirection.Row;
                    fila.style.width = Length.Percent(100);
                    fila.style.height = 40;
                    fila.style.backgroundColor = (filaIndex % 2 == 0)
                        ? new Color(229f / 255f, 229f / 255f, 229f / 255f)
                        : Color.white;

                    fila.Add(CrearCelda(equipo.Nombre));
                    fila.Add(CrearCelda(equipo.Entrenador ?? "—"));
                    fila.Add(CrearCelda(equipo.Objetivo ?? "—"));

                    tablaContainer.Add(fila);
                    filaIndex++;
                }
            }
        }

        private VisualElement CrearCelda(string texto, bool esCabecera = false)
        {
            Label label = new Label(texto);
            label.style.flexGrow = 1;
            label.style.flexBasis = Length.Percent(33.33f); // 3 columnas iguales
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.paddingLeft = 50;
            label.style.color = esCabecera ? Color.white : Color.black;
            label.style.fontSize = esCabecera ? 18 : 16;
            label.style.unityFontDefinition = esCabecera
                ? new StyleFontDefinition(Resources.Load<Font>("Fonts/Poppins-SemiBold"))
                : new StyleFontDefinition(Resources.Load<Font>("Fonts/Poppins-Regular"));
            label.style.unityTextAlign = TextAnchor.MiddleLeft;

            return label;
        }

        // Función auxiliar para cambiar el sprite de un logo
        private void SetLogoSprite(VisualElement logo, string spritePath)
        {
            // spritePath debe ser la ruta relativa dentro de Resources, sin extensión
            Sprite sprite = Resources.Load<Sprite>(spritePath);
            if (sprite != null)
            {
                logo.style.backgroundImage = new StyleBackground(sprite);
            }
        }

        private void GenerarCalendarioCopa(List<Equipo> listaEquipos)
        {
            PartidoData.GenerarTreintaidosavosCopa(listaEquipos, FechaData.temporadaActual, 4);
        }

        private System.Collections.IEnumerator GenerarTemporada()
        {
            progressBar.style.visibility = Visibility.Visible;
            progressBar.value = 0;
            CursorManager.Instance.SetBusyCursor();

            // Generar calendarios
            progressBar.title = "Generando partidos de competiciones nacionales...";
            PartidoData.GenerarCalendario(FechaData.temporadaActual, 1);
            progressBar.value = 10;
            yield return null;

            PartidoData.GenerarCalendario(FechaData.temporadaActual, 2);
            progressBar.value = 20;
            yield return null;

            List<Equipo> listaEquipos = EquipoData.ObtenerEquiposPorPais("ESP");
            GenerarCalendarioCopa(listaEquipos);
            progressBar.value = 30;
            yield return null;

            // Europa
            progressBar.title = "Generando partidos de competiciones europeas...";
            var equiposChampions = EquipoData.EquiposJueganEuropa1(1)
                                    .Concat(EquipoData.ObtenerEquiposPorCompeticion(5))
                                    .OrderBy(e => Guid.NewGuid())
                                    .ToList();
            PartidoData.GenerarCalendarioChampions(equiposChampions, 5, PartidoData.ObtenerPrimerMartesOctubre(FechaData.temporadaActual));
            progressBar.value = 40;
            yield return null;

            var equiposUefa = EquipoData.EquiposJueganEuropa2(1)
                                .Concat(EquipoData.ObtenerEquiposPorCompeticion(6))
                                .OrderBy(e => Guid.NewGuid())
                                .ToList();
            PartidoData.GenerarCalendarioChampions2(equiposUefa, 6, PartidoData.ObtenerPrimerJuevesOctubre(FechaData.temporadaActual));
            progressBar.value = 50;
            yield return null;

            // Clasificaciones
            progressBar.title = "Generando clasificaciones...";
            ClasificacionData.RellenarClasificacionLiga1(1);
            ClasificacionData.RellenarClasificacionLiga2(2);
            ClasificacionData.RellenarClasificacionEuropa1(equiposChampions);
            ClasificacionData.RellenarClasificacionEuropa2(equiposUefa);
            progressBar.value = 55;
            yield return null;

            // Estadísticas
            progressBar.title = "Generando estadísticas...";
            EstadisticaJugadorData.InsertarEstadisticasJugadores();
            EstadisticaJugadorData.InsertarEstadisticasJugadoresEuropa();
            progressBar.value = 75;
            yield return null;

            // Asignar equipo a la tabla "taquilla"
            progressBar.title = "Generando taquilla...";
            TaquillaData.GenerarTaquilla(idEquipoSeleccionado);
            progressBar.value = 80;
            yield return null;

            // Generar el primer registro del historial
            progressBar.title = "Generando historial de mánagers...";
            HistorialData.CrearLineaHistorial(idEquipoSeleccionado, "2025/2026");
            progressBar.value = 85;
            yield return null;

            // Generar la alineacion del equipo
            progressBar.title = "Generando alineaciones...";
            JugadorData.CrearAlineacion(idEquipoSeleccionado);
            progressBar.value = 90;
            yield return null;

            // Crear los mensaje de inicio de partida
            progressBar.title = "Generando mensajes...";
            Mensaje mensajeInicio1 = new Mensaje
            {
                Fecha = new DateTime(2025, 7, 15),
                Remitente = EquipoData.ObtenerDetallesEquipo(idEquipoSeleccionado).Presidente,
                Asunto = "Bienvenido al Club",
                Contenido = "Desde la Directiva del " + EquipoData.ObtenerDetallesEquipo(idEquipoSeleccionado).Nombre + " te damos la bienvenida. Tenemos muchas esperanzas puestas en tí, y estamos seguros de que contigo empieza una nueva etapa en nuestro club, y que nos esperan grandes éxitos.\n\nLa junta directiva y los empleados te irán informando a través de correos electrónicos de las cosas que sucedan en el club.",
                TipoMensaje = "Notificación",
                IdEquipo = idEquipoSeleccionado,
                Leido = false,
                Icono = 0 // 0 es icono de equipo
            };

            // Crear el mensaje aconsejando poner precio a los abonos de temporada
            string presidente = EquipoData.ObtenerDetallesEquipo(idEquipoSeleccionado).Presidente;

            Mensaje mensajeAbonados = new Mensaje
            {
                Fecha = FechaData.hoy,
                Remitente = presidente,
                Asunto = "Campaña de abonados",
                Contenido = $"¡Se ha iniciado la campaña de abonados al club!\n\nPuedes establecer los precios de los abonos de temporada en la sección de ESTADIO.\n\nRecuerda que los abonos solamente pueden realizarse antes del inicio de la competición de Liga.",
                TipoMensaje = "Notificación",
                IdEquipo = idEquipoSeleccionado,
                Leido = false,
                Icono = 0 // 0 es icono de equipo
            };

            MensajeData.CrearMensaje(mensajeAbonados);

            MensajeData.CrearMensaje(mensajeInicio1);
            progressBar.value = 95;
            yield return null;

            // Guardado (usa hilo principal)
            string newSavePath = DatabaseManager.SaveCurrentGame();
            if (!string.IsNullOrEmpty(newSavePath))
            {
                DatabaseManager.DeleteTempDatabase();
                DatabaseManager.SetActiveDatabase(newSavePath);
                Debug.Log($"Nueva base de datos activa: {newSavePath}");
            }
            else
            {
                Debug.LogError("No se pudo guardar la partida correctamente.");
            }
            progressBar.value = 100;
            yield return new WaitForSeconds(0.3f);

            // Restaurar UI y cursor
            CursorManager.Instance.SetDefaultCursor();
            progressBar.style.visibility = Visibility.Hidden;
            btnSeguir.SetEnabled(true);

            // Cambiar de escena si quieres
            SceneLoader.Instance.LoadScene(Constants.MAIN_SCREEN_SCENE);
        }
    }
}