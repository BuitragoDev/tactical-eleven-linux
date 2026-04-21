using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class FichajesBuscarPorEquipo
    {
        private AudioClip clickSFX;
        private MainScreen mainScreen;
        private Equipo miEquipo;
        private Manager miManager;
        private int equipoSeleccionadoId = -1;
        private VisualElement escudoSeleccionado = null;

        private VisualElement root, equiposContainer, jugadoresContainer, logoLiga1, logoLiga2, logoReservas, logoEuropa, logoSinEquipo;
        private Label listadoJugadores;

        public FichajesBuscarPorEquipo(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen, int eqSeleccionado)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            equipoSeleccionadoId = eqSeleccionado;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            equiposContainer = root.Q<VisualElement>("equipos-container");
            jugadoresContainer = root.Q<VisualElement>("jugadores-container");
            logoLiga1 = root.Q<VisualElement>("logo-liga1");
            logoLiga1.SetEnabled(false);
            logoLiga2 = root.Q<VisualElement>("logo-liga2");
            logoReservas = root.Q<VisualElement>("logo-reservas");
            logoEuropa = root.Q<VisualElement>("logo-europa");
            logoSinEquipo = root.Q<VisualElement>("logo-sinEquipo");
            listadoJugadores = root.Q<Label>("txtListadoJugadores");

            List<VisualElement> logos = new List<VisualElement> { logoLiga1, logoLiga2, logoReservas, logoEuropa, logoSinEquipo };

            if (equipoSeleccionadoId != -1)
            {
                int competicionEquipoSeleccionado = EquipoData.ObtenerDetallesEquipo(equipoSeleccionadoId).IdCompeticion;

                if (competicionEquipoSeleccionado == 1)
                {
                    CargarEscudosEquipos(1);
                    ActivarDesactivarLogos(logos, logoLiga1);
                }
                else if (competicionEquipoSeleccionado == 2)
                {
                    CargarEscudosEquipos(2);
                    ActivarDesactivarLogos(logos, logoLiga2);
                }
                else if (competicionEquipoSeleccionado == 3)
                {
                    CargarEscudosEquipos(3);
                    ActivarDesactivarLogos(logos, logoReservas);
                }
                else if (competicionEquipoSeleccionado == 5)
                {
                    CargarEscudosEquipos(5);
                    ActivarDesactivarLogos(logos, logoEuropa);
                }
                else
                {
                    CargarEscudosEquipos(0);
                    ActivarDesactivarLogos(logos, logoSinEquipo);
                }

                CargarLogosLigas();
                listadoJugadores.text = "LISTADO DE JUGADORES DEL " + EquipoData.ObtenerDetallesEquipo(equipoSeleccionadoId).Nombre.ToUpper();
                CargarJugadores(equipoSeleccionadoId);
            }
            else
            {
                CargarLogosLigas();
                CargarEscudosEquipos(1);
            }

            logoLiga1.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ActivarDesactivarLogos(logos, logoLiga1);
                CargarEscudosEquipos(1);
            });

            logoLiga2.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ActivarDesactivarLogos(logos, logoLiga2);
                CargarEscudosEquipos(2);
            });

            logoReservas.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ActivarDesactivarLogos(logos, logoReservas);
                CargarEscudosEquipos(3);
            });

            logoEuropa.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ActivarDesactivarLogos(logos, logoEuropa);
                CargarEscudosEquipos(5);
            });

            logoSinEquipo.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ActivarDesactivarLogos(logos, logoSinEquipo);
                listadoJugadores.text = "LISTADO DE JUGADORES SIN EQUIPO";
                CargarEscudosEquipos(0);
            });
        }

        private void CargarLogosLigas()
        {
            Sprite liga1Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/1");
            if (liga1Sprite != null)
                logoLiga1.style.backgroundImage = new StyleBackground(liga1Sprite);

            Sprite liga2Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/2");
            if (liga2Sprite != null)
                logoLiga2.style.backgroundImage = new StyleBackground(liga2Sprite);

            Sprite reservasSprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/3");
            if (reservasSprite != null)
                logoReservas.style.backgroundImage = new StyleBackground(reservasSprite);

            Sprite europaSprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/euLogo");
            if (europaSprite != null)
                logoEuropa.style.backgroundImage = new StyleBackground(europaSprite);

            Sprite freeAgentSprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/freeAgent_icon");
            if (freeAgentSprite != null)
                logoSinEquipo.style.backgroundImage = new StyleBackground(freeAgentSprite);
        }

        private void CargarEscudosEquipos(int idCompeticion)
        {
            equiposContainer.Clear();

            // Recogemos la lista de equipos.
            List<Equipo> listaEquipos = new List<Equipo>();

            if (idCompeticion == 5)
            {
                listaEquipos = EquipoData.ObtenerEquiposPorCompeticion(idCompeticion)
                            .Concat(EquipoData.ObtenerEquiposPorCompeticion(6))
                            .ToList();
            }
            else
            {
                listaEquipos = EquipoData.ObtenerEquiposPorCompeticion(idCompeticion);
            }

            // Crear filas de 10 escudos
            VisualElement fila = null;
            int contador = 0;

            foreach (var equipo in listaEquipos)
            {
                if (contador % 10 == 0)
                {
                    fila = new VisualElement();
                    fila.style.flexDirection = FlexDirection.Row;
                    fila.style.flexWrap = Wrap.NoWrap;
                    fila.style.marginBottom = 50;
                    equiposContainer.Add(fila);
                }

                var sprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{equipo.IdEquipo}");
                var escudo = CrearElementoEscudo(sprite, equipo.IdEquipo);
                fila.Add(escudo);

                contador++;
            }
        }

        private VisualElement CrearElementoEscudo(Sprite sprite, int idEquipo)
        {
            var imagen = new VisualElement();
            imagen.style.width = 80;
            imagen.style.height = 80;
            imagen.AddToClassList("escudo");

            if (sprite != null)
            {
                imagen.style.backgroundImage = new StyleBackground(sprite);
            }

            if (equipoSeleccionadoId == idEquipo)
            {
                imagen.AddToClassList("escudo-seleccionado"); // marcar el nuevo
                escudoSeleccionado = imagen;
            }
            else
            {
                imagen.RemoveFromClassList("escudo-seleccionado"); // desmarcar
            }

            // Callback al pulsar el escudo
            imagen.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                // --- RESALTAR ESCUDO ---
                if (escudoSeleccionado != null)
                    escudoSeleccionado.RemoveFromClassList("escudo-seleccionado"); // desmarcar anterior

                imagen.AddToClassList("escudo-seleccionado"); // marcar el nuevo
                escudoSeleccionado = imagen;

                equipoSeleccionadoId = idEquipo;
                listadoJugadores.text = "LISTADO DE JUGADORES DEL " + EquipoData.ObtenerDetallesEquipo(equipoSeleccionadoId).Nombre.ToUpper();
                CargarJugadores(equipoSeleccionadoId);
            });

            return imagen;
        }

        private void CargarJugadores(int equipoSeleccionadoId)
        {
            jugadoresContainer.Clear();

            // Obtener lista completa
            List<Jugador> jugadores = JugadorData.ListadoJugadoresCompleto(equipoSeleccionadoId);

            // PORCENTAJES DE COLUMNAS
            float col1 = 10f;    // BANDERA
            float col2 = 30f;   // JUGADOR
            float col3 = 10f;    // DEMARCACIÓN
            float col4 = 10f;    // ALTURA
            float col5 = 10f;    // EDAD
            float col6 = 10f;    // MEDIA
            float col7 = 10f;    // MORAL
            float col8 = 10f;    // ESTADO FORMA

            // COLORES CABECERA
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
            header.style.unityFontStyleAndWeight = FontStyle.Bold;

            var headerEmpty = new VisualElement();
            headerEmpty.style.width = Length.Percent(col1);
            headerEmpty.style.height = 30;
            headerEmpty.style.flexGrow = 0;
            headerEmpty.style.flexShrink = 0;
            header.Add(headerEmpty);

            header.Add(CreateCell("JUGADOR", col2, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("DEMARCACIÓN", col3, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("ALTURA", col4, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("EDAD", col5, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("MEDIA", col6, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("MORAL", col7, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("E.FORMA", col8, headerText, TextAnchor.MiddleCenter, true));

            jugadoresContainer.Add(header);

            // FILAS SOLO DE ESTA PÁGINA
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

                // 1) Bandera
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

                fila.Add(CreateCell(item.NombreCompleto, col2, Color.black, TextAnchor.MiddleLeft, false));
                fila.Add(CreateCell(Constants.RolIdToText(item.RolId), col3, Color.black, TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell($"{item.AlturaEnMetros.ToString()} m", col4, Color.black, TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell(item.Edad.ToString(), col5, Color.black, TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell(item.Media.ToString(), col6, DeterminarColor(item.Media), TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell(item.Moral.ToString(), col7, Color.black, TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell(item.EstadoForma.ToString(), col8, Color.black, TextAnchor.MiddleCenter, false));

                // **Registrar evento de click**
                fila.RegisterCallback<MouseDownEvent>(evt =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    UIManager.Instance.CargarPantalla("UI/Ficha/Ficha", instancia =>
                    {
                        new FichaJugador(instancia, miEquipo, miManager, item.IdJugador, 3, mainScreen);
                    });
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
            {
                cell.style.justifyContent = Justify.FlexStart;
                cell.style.paddingLeft = 20;
            }
            else if (alineacion == TextAnchor.MiddleCenter)
            {
                cell.style.justifyContent = Justify.Center;
            }
            else if (alineacion == TextAnchor.MiddleRight)
            {
                cell.style.justifyContent = Justify.FlexEnd;
            }

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

        private void ActivarDesactivarLogos(List<VisualElement> logos, VisualElement logo)
        {
            foreach (var item in logos)
            {
                if (item == logo)
                    item.SetEnabled(false);
                else
                    item.SetEnabled(true);
            }
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
    }
}