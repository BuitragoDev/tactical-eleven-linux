using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class EntrenadorRival
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        Fecha f = FechaData.ObtenerFechaHoy();
        Jugador jugadorSeleccionado = null;

        private VisualElement root, jugadoresContainer, fotoJugador, fotoEntrenador, contenedorEstrellas, partidosContainer;
        private Label tituloVentana, nombreMiEquipo, nombreJugador, jugadorGoles, jugadorAsistencias, jugadorMVPs,
                      nombreEntrenador, tacticaFavorita;

        public EntrenadorRival(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            jugadoresContainer = root.Q<VisualElement>("jugadores-container");
            partidosContainer = root.Q<VisualElement>("partidos-container");
            fotoJugador = root.Q<VisualElement>("foto-jugador");
            fotoEntrenador = root.Q<VisualElement>("foto-entrenador");
            contenedorEstrellas = root.Q<VisualElement>("contenedor-estrellas");
            tituloVentana = root.Q<Label>("lblTituloVentana");
            nombreMiEquipo = root.Q<Label>("lblNombreMiEquipo");
            nombreJugador = root.Q<Label>("nombre-jugador");
            jugadorGoles = root.Q<Label>("jugador-goles");
            jugadorAsistencias = root.Q<Label>("jugador-asistencias");
            jugadorMVPs = root.Q<Label>("jugador-mvps");
            nombreEntrenador = root.Q<Label>("nombre-entrenador");
            tacticaFavorita = root.Q<Label>("tactica-favorita");

            // Obtener el próximo partido
            Partido proximoPartido = PartidoData.ObtenerProximoPartido(miEquipo.IdEquipo, f.ToDateTime());

            // Determinar el ID del equipo rival
            int rival = (proximoPartido.IdEquipoLocal == miEquipo.IdEquipo) ? proximoPartido.IdEquipoVisitante : proximoPartido.IdEquipoLocal;

            ActualizarTituloVentana(EquipoData.ObtenerDetallesEquipo(rival).Nombre);

            // Obtener la lista de jugadores del equipo rival
            List<Jugador> listaEquipo = JugadorData.ListadoJugadoresCompleto(rival);
            CargarListajugadores(listaEquipo);

            CargarDatosEntrenador(rival);

            CargarPartidos(rival);
        }

        private void CargarPartidos(int rival)
        {
            List<Partido> listaPartidos = PartidoData.UltimosCincoPartidos(rival);
            MostrarPartidos(listaPartidos);
        }

        private void MostrarPartidos(List<Partido> partidos)
        {
            for (int i = 0; i < partidos.Count; i++)
            {
                var fila = CrearFilaPartido(partidos[i]);

                partidosContainer.Add(fila);
            }
        }

        private VisualElement CrearColumna()
        {
            var col = new VisualElement();
            col.style.flexDirection = FlexDirection.Column;
            col.style.width = Length.Percent(48);
            col.style.paddingLeft = 10;
            col.style.paddingRight = 10;
            return col;
        }

        private VisualElement CrearFilaPartido(Partido p)
        {
            var fila = new VisualElement();
            fila.style.flexDirection = FlexDirection.Row;
            fila.style.width = Length.Percent(100);
            fila.style.minHeight = 50;
            fila.style.maxHeight = 50;

            fila.style.alignItems = Align.Center;

            Color bg = new Color32(245, 245, 245, 255);
            fila.style.backgroundColor = bg;

            // ESCUDO LOCAL
            fila.Add(CrearEscudo(p.IdEquipoLocal));

            // NOMBRE LOCAL
            if (p.IdEquipoLocal == miEquipo.IdEquipo)
                fila.Add(CrearTexto(EquipoData.ObtenerDetallesEquipo(p.IdEquipoLocal).Nombre, 35, TextAnchor.MiddleLeft, true));
            else
                fila.Add(CrearTexto(EquipoData.ObtenerDetallesEquipo(p.IdEquipoLocal).Nombre, 35, TextAnchor.MiddleLeft, false));

            // GOLES LOCAL
            fila.Add(CrearTexto(p.GolesLocal.ToString(), 5, TextAnchor.MiddleCenter, true));

            // ESCUDO VISITANTE
            fila.Add(CrearEscudo(p.IdEquipoVisitante));

            // NOMBRE VISITANTE
            if (p.IdEquipoVisitante == miEquipo.IdEquipo)
                fila.Add(CrearTexto(EquipoData.ObtenerDetallesEquipo(p.IdEquipoVisitante).Nombre, 30, TextAnchor.MiddleLeft, true));
            else
                fila.Add(CrearTexto(EquipoData.ObtenerDetallesEquipo(p.IdEquipoVisitante).Nombre, 30, TextAnchor.MiddleLeft, false));

            // GOLES VISITANTE
            fila.Add(CrearTexto(p.GolesVisitante.ToString(), 5, TextAnchor.MiddleCenter, true));

            return fila;
        }

        private VisualElement CrearEscudo(int idEquipo)
        {
            var escudo = new VisualElement();
            escudo.style.width = Length.Percent(10);
            escudo.style.height = 32;

            var sprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{idEquipo}");
            if (sprite != null)
            {
                escudo.style.backgroundImage = new StyleBackground(sprite);
                escudo.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            }

            return escudo;
        }

        private VisualElement CrearTexto(string contenido, float widthPercent, TextAnchor anchor, bool negrita)
        {
            // Fuente Poppins-Bold
            var fontPath = negrita
                ? "Fonts/Poppins-SemiBold SDF"
                : "Fonts/Poppins-Regular SDF";


            var container = new VisualElement();
            container.style.width = Length.Percent(widthPercent);
            container.style.flexDirection = FlexDirection.Row;
            container.style.alignItems = Align.Center;

            var label = new Label(contenido);
            label.style.unityTextAlign = anchor;
            label.style.fontSize = 18;
            var fontAsset = Resources.Load<UnityEngine.TextCore.Text.FontAsset>(fontPath);
            label.style.unityFontDefinition = new StyleFontDefinition(fontAsset);

            container.Add(label);
            return container;
        }

        private void CargarDatosEntrenador(int rival)
        {
            Entrenador coach = EntrenadorData.MostrarEntrenador(rival);

            nombreEntrenador.text = coach.NombreCompleto;
            MostrarEstrellas(coach.Reputacion, contenedorEstrellas);
            tacticaFavorita.text = coach.TacticaFavorita;
        }

        private void CargarListajugadores(List<Jugador> jugadores)
        {
            jugadoresContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 10f;     // BANDERA
            float col2 = 57f;     // JUGADOR
            float col3 = 15f;     // DEMARCACION
            float col4 = 15f;     // MEDIA

            // COLORES
            Color headerBg = new Color32(56, 78, 63, 255);
            Color headerText = Color.white;

            // CABECERA
            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.backgroundColor = new StyleColor(headerBg);
            header.style.minHeight = 30;
            header.style.maxHeight = 30;
            header.style.width = Length.Percent(100);
            header.style.alignItems = Align.Center;
            header.style.justifyContent = Justify.FlexStart;
            header.style.unityFontStyleAndWeight = FontStyle.Bold;

            header.Add(CreateCell("NAC", col1, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("JUGADOR", col2, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("DEM", col3, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("MED", col4, headerText, TextAnchor.MiddleCenter, true));

            jugadoresContainer.Add(header);

            // FILAS
            int index = 0;
            foreach (var item in jugadores)
            {
                var fila = new VisualElement();
                fila.style.flexDirection = FlexDirection.Row;
                fila.style.width = Length.Percent(100);
                fila.style.minHeight = 30;
                fila.style.maxHeight = 30;
                fila.style.alignItems = Align.Center;

                // Color de fondo de fila alternante (para todas las columnas)
                // Color estándar alterno
                Color filaColor = (index % 2 == 0)
                    ? new Color32(255, 255, 255, 255)     // blanco
                    : new Color32(242, 242, 242, 255);    // gris suave

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                // 1) BANDERA
                var bandera = new VisualElement();
                bandera.style.width = Length.Percent(col1);
                bandera.style.height = 30;
                bandera.style.flexGrow = 0;
                bandera.style.flexShrink = 0;

                var sprite = Resources.Load<Sprite>($"Banderas/{Constants.ObtenerCodigoBanderas(item.Nacionalidad)}");
                if (sprite != null)
                {
                    bandera.style.backgroundImage = new StyleBackground(sprite);
                    bandera.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(bandera);

                // 2) JUGADOR
                fila.Add(CreateCell(item.NombreCompleto, col2, Color.black, TextAnchor.MiddleLeft, false));

                // 3) DEMARCACIÓN
                fila.Add(CreateCell(Constants.RolIdToText(item.RolId), col3, Color.black, TextAnchor.MiddleCenter, false));

                // 4) MED
                fila.Add(CreateCell(item.Media.ToString(), col4, DeterminarColor(item.Media), TextAnchor.MiddleCenter, false));

                // **Registrar evento de click**
                fila.RegisterCallback<MouseDownEvent>(evt =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    jugadorSeleccionado = item; // Guardar jugador seleccionado

                    nombreJugador.text = item.NombreCompleto;

                    var fotoSprite = Resources.Load<Sprite>($"Jugadores/{item.IdJugador}");
                    if (fotoSprite != null)
                        fotoJugador.style.backgroundImage = new StyleBackground(fotoSprite);

                    // Estadisticas del Jugador
                    Estadistica stats = EstadisticaJugadorData.MostrarEstadisticasJugador(item.IdJugador);
                    jugadorGoles.text = stats.Goles.ToString();
                    jugadorAsistencias.text = stats.Asistencias.ToString();
                    jugadorMVPs.text = stats.MVP.ToString();
                });

                jugadoresContainer.Add(fila);
                index++;
            }
        }

        private VisualElement CreateCell(string texto, float anchoPercent, Color color, TextAnchor alineacion, bool esHeader)
        {
            var cell = new VisualElement();
            cell.style.width = Length.Percent(anchoPercent);
            cell.style.flexDirection = FlexDirection.Row;
            cell.style.alignItems = Align.Center;

            // Ajustar el positionamiento horizontal según el alineado del texto
            if (alineacion == TextAnchor.MiddleLeft)
                cell.style.justifyContent = Justify.FlexStart;
            else if (alineacion == TextAnchor.MiddleCenter)
                cell.style.justifyContent = Justify.Center;
            else if (alineacion == TextAnchor.MiddleRight)
                cell.style.justifyContent = Justify.FlexEnd;

            var label = new Label(texto);
            label.style.color = color;
            label.style.unityTextAlign = alineacion;

            var fontPath = esHeader
                ? "Fonts/Poppins-SemiBold SDF"
                : "Fonts/Poppins-Regular SDF";

            var fontAsset = Resources.Load<UnityEngine.TextCore.Text.FontAsset>(fontPath);
            label.style.unityFontDefinition = new StyleFontDefinition(fontAsset);
            label.style.fontSize = 16;

            label.style.flexShrink = 0;
            label.style.flexGrow = 0;

            cell.Add(label);
            return cell;
        }

        private Color DeterminarColor(int puntos)
        {
            if (puntos > 70)
                return new Color32(0x1E, 0x72, 0x3C, 0xFF);   // Verde 
            else if (puntos >= 50)
                return new Color32(0xC6, 0x76, 0x17, 0xFF); // Naranja 
            else
                return new Color32(0xA3, 0x1E, 0x1E, 0xFF); // rojo
        }

        private void ActualizarTituloVentana(string nombre)
        {
            tituloVentana.text = $"VER RIVAL ({nombre.ToUpper()})";
        }

        private void MostrarEstrellas(int reputacion, VisualElement contenedor)
        {
            contenedor.Clear();

            Sprite estrellaON = Resources.Load<Sprite>("Icons/estrellaOn");
            Sprite estrellaOFF = Resources.Load<Sprite>("Icons/estrellaOff");

            if (estrellaON == null || estrellaOFF == null)
            {
                Debug.LogError("No se pudieron cargar las imágenes de las estrellas");
                return;
            }

            int numeroEstrellas = reputacion switch
            {
                100 => 5,
                >= 90 => 4,
                >= 70 => 3,
                >= 50 => 2,
                >= 25 => 1,
                _ => 0
            };

            for (int i = 0; i < 5; i++)
            {
                Image estrella = new Image
                {
                    image = i < numeroEstrellas ? estrellaON.texture : estrellaOFF.texture,
                    scaleMode = ScaleMode.ScaleToFit,
                    style =
                    {
                        width = 32,
                        height = 32,
                        marginRight = 3
                    }
                };
                contenedor.Add(estrella);
            }

            contenedor.style.flexDirection = FlexDirection.Row;
        }
    }
}