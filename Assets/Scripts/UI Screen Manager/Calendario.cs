using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class Calendario
    {
        private AudioClip clickSFX;
        private Fecha hoy = FechaData.ObtenerFechaHoy();

        private List<Partido> partidos;
        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root;
        private VisualElement logoCompeticion, lugarPartido, escudoLocal, escudoVisitante, calendarContainer;
        private Button btnMas, btnMenos;
        private Label txtMes, txtFecha, txtEstadio, equipoLocal, equipoVisitante;

        private DateTime mesActual;

        public Calendario(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;

            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a elementos internos 
            calendarContainer = root.Q<VisualElement>("calendar-container");
            logoCompeticion = root.Q<VisualElement>("logoCompeticion");
            lugarPartido = root.Q<VisualElement>("lugarPartido");
            escudoLocal = root.Q<VisualElement>("escudoLocal");
            escudoVisitante = root.Q<VisualElement>("escudoVisitante");
            btnMas = root.Q<Button>("btnMas");
            btnMenos = root.Q<Button>("btnMenos");
            txtMes = root.Q<Label>("txtMes");
            txtFecha = root.Q<Label>("txtFecha");
            txtEstadio = root.Q<Label>("txtEstadio");
            equipoLocal = root.Q<Label>("equipoLocal");
            equipoVisitante = root.Q<Label>("equipoVisitante");

            // Eventos
            btnMas.clicked += MesSiguiente;
            btnMenos.clicked += MesAnterior;

            // Cargar partidos
            partidos = PartidoData.MostrarMisPartidos(miEquipo.IdEquipo) ?? new List<Partido>();

            // Mes actual = hoy
            mesActual = DateTime.Parse(hoy.Hoy);

            // Generar calendario inicial
            GenerarCalendario();
        }

        private void MesSiguiente()
        {
            mesActual = mesActual.AddMonths(1);
            GenerarCalendario();
        }

        private void MesAnterior()
        {
            mesActual = mesActual.AddMonths(-1);
            GenerarCalendario();
        }

        private void GenerarCalendario()
        {
            calendarContainer.Clear();

            txtMes.text = mesActual.ToString("MMMM yyyy").ToUpper();

            int diasEnMes = DateTime.DaysInMonth(mesActual.Year, mesActual.Month);
            int primerDiaSemana = ((int)new DateTime(mesActual.Year, mesActual.Month, 1).DayOfWeek + 6) % 7; // lunes=0

            // Fila de días de la semana
            VisualElement filaDias = new VisualElement();
            filaDias.style.flexDirection = FlexDirection.Row;
            string[] diasSemana = { "LUN", "MAR", "MIÉ", "JUE", "VIE", "SÁB", "DOM" };
            foreach (var dia in diasSemana)
            {
                Label l = new Label(dia)
                {
                    style =
                    {
                        flexGrow = 1,
                        unityTextAlign = TextAnchor.MiddleCenter,
                        fontSize = 14,
                        unityFontStyleAndWeight = FontStyle.Bold
                    }
                };
                filaDias.Add(l);
            }
            calendarContainer.Add(filaDias);

            // Grid del mes
            VisualElement grid = new VisualElement();
            grid.style.flexDirection = FlexDirection.Row;
            grid.style.flexWrap = Wrap.Wrap;
            calendarContainer.Add(grid);

            // Espacios vacíos antes del primer día
            for (int i = 0; i < primerDiaSemana; i++)
                grid.Add(CreateEmptyCell());

            // Días del mes
            for (int dia = 1; dia <= diasEnMes; dia++)
            {
                DateTime fecha = new DateTime(mesActual.Year, mesActual.Month, dia);
                var partidosDia = partidos.Where(p => p.FechaPartido.Date == fecha).ToList();
                grid.Add(CreateDiaCell(fecha, partidosDia));
            }

            // Bloquear botón atrás si estamos en el mes mínimo
            DateTime mesMinimo = new DateTime(2025, 7, 25); // julio 2025
            if (mesActual <= mesMinimo)
            {
                btnMenos.SetEnabled(false); // deshabilita el botón
                btnMenos.style.visibility = Visibility.Hidden; // opcional: lo oculta
            }
            else
            {
                btnMenos.SetEnabled(true);
                btnMenos.style.visibility = Visibility.Visible;
            }
        }

        private VisualElement CreateEmptyCell()
        {
            VisualElement ve = new VisualElement();
            ve.style.width = Length.Percent(14.28f); // 100/7
            ve.style.height = 120;
            return ve;
        }

        private VisualElement CreateDiaCell(DateTime fecha, List<Partido> partidosDia)
        {
            VisualElement cell = new VisualElement();
            cell.style.width = Length.Percent(14.28f);
            cell.style.height = 120;
            cell.style.borderBottomColor = Color.gray;
            cell.style.borderBottomWidth = 1;
            cell.style.borderTopWidth = 1;
            cell.style.marginBottom = 2;

            // Día actual
            if (fecha.Date == DateTime.Parse(hoy.Hoy).Date)
                cell.style.backgroundColor = new Color(197f / 255f, 232f / 255f, 202f / 255f);

            // Número del día
            Font miFuente = Resources.Load<Font>("Fonts/Poppins-Bold");
            Label lblDia = new Label(fecha.Day.ToString())
            {
                style =
        {
            unityTextAlign = TextAnchor.UpperRight,
            marginRight = 5,
            paddingRight = 10,
            fontSize = 14,
            color = Color.white,
            backgroundColor = new Color(56f / 255f, 78f / 255f, 63f / 255f),
            unityFont = miFuente
        }
            };
            cell.Add(lblDia);

            // Escudo del rival si hay partido
            if (partidosDia.Count > 0)
            {
                Partido p = partidosDia[0]; // si hay varios, solo el primero
                int idRival = (p.IdEquipoLocal == miEquipo.IdEquipo) ? p.IdEquipoVisitante : p.IdEquipoLocal;
                Sprite escudo = Resources.Load<Sprite>($"EscudosEquipos/80x80/{idRival}");
                if (escudo != null)
                {
                    VisualElement veEscudo = new VisualElement();
                    veEscudo.style.width = 80;
                    veEscudo.style.height = 80;
                    veEscudo.style.alignSelf = Align.Center;
                    veEscudo.style.marginTop = 1;

                    veEscudo.style.backgroundImage = new StyleBackground(escudo.texture);
                    veEscudo.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                    veEscudo.style.unityBackgroundImageTintColor = Color.white;

                    cell.Add(veEscudo);
                }
            }

            // Click sobre el día
            cell.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                if (partidosDia.Count > 0)
                {
                    // Solo el primer partido si hay varios
                    Partido p = partidosDia[0];

                    txtFecha.text = p.FechaPartido.ToString("dd/MM/yyyy");
                    txtEstadio.text = EquipoData.ObtenerDetallesEquipo(p.IdEquipoLocal).Estadio;
                    equipoLocal.text = EquipoData.ObtenerDetallesEquipo(p.IdEquipoLocal).Nombre;
                    equipoVisitante.text = EquipoData.ObtenerDetallesEquipo(p.IdEquipoVisitante).Nombre;

                    // Competición
                    if (p.IdCompeticion == 4)
                    {
                        var competicionSprite = Resources.Load<Sprite>($"LogosCompeticiones/copa200");
                        if (competicionSprite != null)
                            logoCompeticion.style.backgroundImage = new StyleBackground(competicionSprite);
                    }
                    else
                    {
                        var competicionSprite = Resources.Load<Sprite>($"LogosCompeticiones/{p.IdCompeticion}");
                        if (competicionSprite != null)
                            logoCompeticion.style.backgroundImage = new StyleBackground(competicionSprite);
                    }

                    // Lugar partido
                    if (p.IdEquipoLocal == miEquipo.IdEquipo)
                    {
                        var lugarPartidoSprite = Resources.Load<Sprite>($"Icons/casa_icon64");
                        if (lugarPartidoSprite != null)
                            lugarPartido.style.backgroundImage = new StyleBackground(lugarPartidoSprite);
                    }
                    else
                    {
                        var lugarPartidoSprite = Resources.Load<Sprite>($"Icons/avion-icon64");
                        if (lugarPartidoSprite != null)
                            lugarPartido.style.backgroundImage = new StyleBackground(lugarPartidoSprite);
                    }

                    // Escudo equipo local
                    var localSprite = Resources.Load<Sprite>($"EscudosEquipos/120x120/{p.IdEquipoLocal}");
                    if (localSprite != null)
                        escudoLocal.style.backgroundImage = new StyleBackground(localSprite);

                    // Escudo equipo visitante
                    var visitanteSprite = Resources.Load<Sprite>($"EscudosEquipos/120x120/{p.IdEquipoVisitante}");
                    if (visitanteSprite != null)
                        escudoVisitante.style.backgroundImage = new StyleBackground(visitanteSprite);
                }
                else
                {
                    // No hay partido en esa fecha, limpiar labels
                    txtFecha.text = "";
                    txtEstadio.text = "";
                    equipoLocal.text = "";
                    equipoVisitante.text = "";
                    logoCompeticion.style.backgroundImage = null;
                    escudoLocal.style.backgroundImage = null;
                    escudoVisitante.style.backgroundImage = null;
                }
            });

            return cell;
        }
    }
}