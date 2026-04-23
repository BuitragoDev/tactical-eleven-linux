using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;
using System.Linq;

namespace TacticalEleven.Scripts
{
    public class MainScreen : MonoBehaviour
    {
        [Header("Sound Clips")]
        [SerializeField] private AudioClip clickSFX;

        // UI Elements
        private VisualElement miEquipoEscudo, cabeceraManagerValoracion;
        private VisualElement homeIcon, clubIcon, alineacionIcon, competicionesIcon, calendarioIcon,
                              fichajesIcon, finanzasIcon, estadioIcon, managerIcon, mensajesIcon, ajustesIcon;
        public VisualElement clubMenu, alineacionMenu, competicionMenu, calendarioMenu, fichajesMenu, finanzasMenu,
               estadioMenu, managerMenu, mensajesMenu;
        private VisualElement mainContainer, popupContainer, resumenPartido, imgEscudoLocal, imgEscudoVisitante, fotoMvp;
        private Button btnSeguir, btnCerrar, resumenPartidoBtnContinuar;
        private Label miEquipoNombre, managerNombre, fecha1, fecha2, popupText, resumenPartidoCabeceraTitulo,
                lblTituloJornada, lblGolesLocal, lblGolesVisitante, lblNombreLocal, lblNombreVisitante, lblAsistenciaPartido,
                lblMvpDemarcacion, lblMvpNombre, lblMvpEstadisticas;
        private DiaTipo diaTipoActual = DiaTipo.Continuar;
        public Label miPresupuesto;
        private Manager miManager;
        private Equipo miEquipo;
        private static Random random = new Random(); //Random global

        // Elementos Top Menu
        public Label lblInformacion, lblPlantilla, lblEmpleados, lblLesionados, lblManagerFicha, lblManagerPalmares,
                      lblClasificacion, lblResultados, lblEstadisticas, lblPalmaresEquipos, lblPalmaresJugadores,
                      lblIngresos, lblGastos, lblPatrocinadores, lblTelevision, lblPrestamos,
                      lblEstadioInformacion, lblEntradas, lblAmpliaciones,
                      lblAlineacion, lblEntrenamiento, lblRival,
                      lblMercado, lblBusqueda, lblCartera, lblEstadoOfertas, lblListaTraspasos;

        void OnEnable()
        {
            SceneLoader.setSettingsParameter(1);

            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // --- Contenedores y elementos UI ---
            miEquipoEscudo = root.Q<VisualElement>("cabecera-escudo");
            miEquipoNombre = root.Q<Label>("miEquipoNombre");
            miPresupuesto = root.Q<Label>("miPresupuesto");
            managerNombre = root.Q<Label>("managerNombre");
            cabeceraManagerValoracion = root.Q<VisualElement>("cabecera-manager-valoracion");
            fecha1 = root.Q<Label>("fecha1");
            fecha2 = root.Q<Label>("fecha2");
            btnSeguir = root.Q<Button>("btnSeguir");

            homeIcon = root.Q<VisualElement>("home-icon");
            clubIcon = root.Q<VisualElement>("club-icon");
            alineacionIcon = root.Q<VisualElement>("alineacion-icon");
            competicionesIcon = root.Q<VisualElement>("competiciones-icon");
            calendarioIcon = root.Q<VisualElement>("calendario-icon");
            fichajesIcon = root.Q<VisualElement>("fichajes-icon");
            finanzasIcon = root.Q<VisualElement>("finanzas-icon");
            estadioIcon = root.Q<VisualElement>("estadio-icon");
            managerIcon = root.Q<VisualElement>("manager-icon");
            mensajesIcon = root.Q<VisualElement>("mensajes-icon");
            ajustesIcon = root.Q<VisualElement>("ajustes-icon");

            // Top menu elements
            lblInformacion = root.Q<Label>("lblInformacion");
            lblPlantilla = root.Q<Label>("lblPlantilla");
            lblEmpleados = root.Q<Label>("lblEmpleados");
            lblLesionados = root.Q<Label>("lblLesionados");
            lblManagerFicha = root.Q<Label>("lblFicha");
            lblManagerPalmares = root.Q<Label>("lblPalmaresManager");
            lblClasificacion = root.Q<Label>("lblClasificacion");
            lblResultados = root.Q<Label>("lblResultados");
            lblEstadisticas = root.Q<Label>("lblEstadisticas");
            lblPalmaresEquipos = root.Q<Label>("lblPalmaresE");
            lblPalmaresJugadores = root.Q<Label>("lblPalmaresJ");
            lblEstadioInformacion = root.Q<Label>("lblInformacionEstadio");
            lblEntradas = root.Q<Label>("lblEntradas");
            lblAmpliaciones = root.Q<Label>("lblAmpliaciones");
            lblAlineacion = root.Q<Label>("lblAlineacion");
            lblEntrenamiento = root.Q<Label>("lblEntrenamiento");
            lblRival = root.Q<Label>("lblRival");
            lblIngresos = root.Q<Label>("lblIngresos");
            lblGastos = root.Q<Label>("lblGastos");
            lblPatrocinadores = root.Q<Label>("lblPatrocinadores");
            lblTelevision = root.Q<Label>("lblTelevision");
            lblPrestamos = root.Q<Label>("lblPrestamos");
            lblMercado = root.Q<Label>("lblMercado");
            lblBusqueda = root.Q<Label>("lblBusqueda");
            lblCartera = root.Q<Label>("lblCartera");
            lblEstadoOfertas = root.Q<Label>("lblEstadoOfertas");
            lblListaTraspasos = root.Q<Label>("lblListaTraspasos");

            mainContainer = root.Q<VisualElement>("main-container");

            // CLUB
            clubMenu = root.Q<VisualElement>("clubMenu");
            alineacionMenu = root.Q<VisualElement>("entrenadorMenu");
            competicionMenu = root.Q<VisualElement>("competicionMenu");
            calendarioMenu = root.Q<VisualElement>("calendarioMenu");
            fichajesMenu = root.Q<VisualElement>("fichajesMenu");
            finanzasMenu = root.Q<VisualElement>("finanzasMenu");
            estadioMenu = root.Q<VisualElement>("estadioMenu");
            managerMenu = root.Q<VisualElement>("managerMenu");
            mensajesMenu = root.Q<VisualElement>("mensajesMenu");

            // Popup
            popupContainer = root.Q<VisualElement>("popup-container");
            btnCerrar = root.Q<Button>("btnCerrar");
            popupText = root.Q<Label>("popup-text");

            // Resumen del partido
            resumenPartido = root.Q<VisualElement>("resumen-partido");
            resumenPartidoCabeceraTitulo = root.Q<Label>("resumen-partidos-titulo"); 
            resumenPartidoBtnContinuar = root.Q<Button>("btnContinuar");
            lblTituloJornada = root.Q<Label>("lblTituloJornada"); 
            lblGolesLocal = root.Q<Label>("goles-local");
            lblGolesVisitante = root.Q<Label>("goles-visitante");
            lblNombreLocal = root.Q<Label>("lblNombreLocal");
            lblNombreVisitante = root.Q<Label>("lblNombreVisitante");
            lblAsistenciaPartido = root.Q<Label>("lblAsistenciaPartido");
            imgEscudoLocal = root.Q<VisualElement>("escudo-local");
            imgEscudoVisitante = root.Q<VisualElement>("escudo-visitante");
            fotoMvp = root.Q<VisualElement>("foto-mvp");
            lblMvpDemarcacion = root.Q<Label>("lblMvpDemarcacion");
            lblMvpNombre = root.Q<Label>("lblMvpNombre");
            lblMvpEstadisticas = root.Q<Label>("lblMvpEstadisticas");

            // Listas por sección
            List<Label> clubList = new List<Label> { lblInformacion, lblPlantilla, lblEmpleados, lblLesionados, lblClasificacion, lblResultados,
                                                     lblEstadisticas, lblPalmaresJugadores, lblPalmaresEquipos, lblEstadioInformacion, lblEntradas,
                                                     lblAmpliaciones, lblAlineacion, lblEntrenamiento, lblRival, lblIngresos, lblGastos,
                                                     lblPatrocinadores, lblTelevision, lblPrestamos, lblMercado, lblBusqueda, lblCartera,
                                                     lblEstadoOfertas, lblListaTraspasos };

            // --- UIManager ---
            if (UIManager.Instance == null)
            {
                var go = new GameObject("UI Manager");
                go.AddComponent<UIManager>();
                Debug.Log("UIManager creado dinámicamente");
            }

            mainContainer = root.Q<VisualElement>("main-container");
            UIManager.Instance.SetMainContainer(mainContainer);

            // --- Cargar datos del manager y equipo ---
            miManager = ManagerData.MostrarManager();
            miEquipo = EquipoData.ObtenerDetallesEquipo((int)miManager.IdEquipo);

            // Escudo
            var sprite = Resources.Load<Sprite>($"EscudosEquipos/120x120/{miManager.IdEquipo}");
            if (sprite != null)
                miEquipoEscudo.style.backgroundImage = new StyleBackground(sprite);

            // Nombre del equipo
            miEquipoNombre.text = miEquipo.Nombre;

            // Presupuesto
            float presupuestoConversion = miEquipo.Presupuesto * Constants.EURO_VALUE;
            string symbol = Constants.EURO_SYMBOL;

            string currency = PlayerPrefs.GetString("Currency", string.Empty);
            if (currency != string.Empty)
            {
                switch (currency)
                {
                    case Constants.EURO_NAME:
                        presupuestoConversion = miEquipo.Presupuesto * Constants.EURO_VALUE;
                        symbol = Constants.EURO_SYMBOL;
                        break;
                    case Constants.POUND_NAME:
                        presupuestoConversion = miEquipo.Presupuesto * Constants.POUND_VALUE;
                        symbol = Constants.POUND_SYMBOL;
                        break;
                    case Constants.DOLLAR_NAME:
                        presupuestoConversion = miEquipo.Presupuesto * Constants.DOLLAR_VALUE;
                        symbol = Constants.DOLLAR_SYMBOL;
                        break;
                    default:
                        presupuestoConversion = miEquipo.Presupuesto * Constants.EURO_VALUE;
                        symbol = Constants.EURO_SYMBOL;
                        break;
                }
            }

            miPresupuesto.text = $"{presupuestoConversion.ToString("N0")} {symbol}";

            // Nombre del manager
            managerNombre.text = $"{miManager.Nombre} {miManager.Apellido}";

            // Valoración del manager
            MostrarEstrellas(miManager.Reputacion);

            // Fecha
            Fecha fechaObjeto = FechaData.ObtenerFechaHoy();
            DateTime hoy = DateTime.Parse(fechaObjeto.Hoy);
            CultureInfo culturaEspañol = new CultureInfo("es-ES");
            string dia = hoy.ToString("dd", culturaEspañol);
            string mes = hoy.ToString("MMM", culturaEspañol).ToUpper();
            string año = hoy.ToString("yyyy", culturaEspañol);
            fecha1.text = $"{dia} {mes} {año}";

            string diaSemana = hoy.ToString("dddd", culturaEspañol);
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);
            fecha2.text = diaSemana;

            // --- Cargar portada al iniciar ---
            CargarPortada();

            // --- Botón seguir ---
            btnSeguir.clicked += () => AudioManager.Instance.PlaySFX(clickSFX);
            btnSeguir.clicked += OnBtnSeguirClicked;

            // -- Botón Continuar Resumen partido ---
            resumenPartidoBtnContinuar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                resumenPartido.style.display = DisplayStyle.None;
            };

            ActualizarBotonSeguir();

            // --- Eventos iconos menú lateral ---
            List<VisualElement> menuList = new List<VisualElement> { clubMenu, alineacionMenu, competicionMenu,
                                                                     calendarioMenu, fichajesMenu, finanzasMenu,
                                                                     estadioMenu, managerMenu, mensajesMenu
                                                                   };

            // ---------------------------------------------------- Evento HOME ICON
            homeIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, null);
                CargarPortada();
            });

            // ---------------------------------------------------- Eventos CLUB
            clubIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, clubMenu);
                CargarClubInformacion(clubList);
            });
            lblInformacion.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClubInformacion(clubList);
            });
            lblPlantilla.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClubPlantilla(clubList);
            });
            lblEmpleados.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClubEmpleados(clubList);
            });
            lblLesionados.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClubLesionados(clubList);
            });

            // ---------------------------------------------------- Eventos ALINEACION
            alineacionIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, alineacionMenu);
                CargarEntrenadorAlineacion(clubList);
            });
            lblAlineacion.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarEntrenadorAlineacion(clubList);
            });
            lblEntrenamiento.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarEntrenadorEntrenamiento(clubList);
            });
            lblRival.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarEntrenadorRival(clubList);
            });

            // ---------------------------------------------------- Eventos COMPETICIONES
            competicionesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, competicionMenu);
                CargarCompeticionesClasificacion(clubList);
            });
            lblClasificacion.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarCompeticionesClasificacion(clubList);
            });
            lblEstadisticas.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarCompeticionesEstadisticas(clubList);
            });
            lblPalmaresEquipos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarCompeticionesPalmaresEquipos(clubList);
            });
            lblPalmaresJugadores.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarCompeticionesPalmaresJugadores(clubList);
            });
            lblResultados.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarCompeticionesResultados(clubList);
            });

            // ---------------------------------------------------- Evento CALENDARIO
            calendarioIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, calendarioMenu);
                CargarCalendario();
            });

            // ---------------------------------------------------- Evento FICHAJES
            fichajesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, fichajesMenu);
                CargarFichajesMercado(clubList);
            });
            lblMercado.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFichajesMercado(clubList);
            });
            lblBusqueda.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFichajesBusqueda(clubList);
            });
            lblCartera.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFichajesCartera(clubList);
            });
            lblListaTraspasos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFichajesListaTraspasos(clubList);
            });
            lblEstadoOfertas.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFichajesEstadoOfertas(clubList);
            });

            // ---------------------------------------------------- Eventos FINANZAS
            finanzasIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, finanzasMenu);
                CargarFinanzasIngresos(clubList);
            });
            lblIngresos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFinanzasIngresos(clubList);
            });
            lblGastos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFinanzasGastos(clubList);
            });
            lblPatrocinadores.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFinanzasPatrocinador(clubList);
            });
            lblTelevision.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFinanzasTelevision(clubList);
            });
            lblPrestamos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarFinanzasPrestamos(clubList);
            });

            // ---------------------------------------------------- Eventos ESTADIO
            estadioIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, estadioMenu);
                CargarEstadioInformacion(clubList);
            });
            lblEstadioInformacion.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarEstadioInformacion(clubList);
            });
            lblEntradas.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarEstadioEntradas(clubList);
            });
            lblAmpliaciones.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarEstadioAmpliaciones(clubList);
            });

            // ---------------------------------------------------- Eventos MÁNAGER
            managerIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, managerMenu);
                CargarManagerFicha(clubList);
            });
            lblManagerFicha.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarManagerFicha(clubList);
            });
            lblManagerPalmares.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarManagerPalmares(clubList);
            });

            // ---------------------------------------------------- Evento MENSAJES ICON
            mensajesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, mensajesMenu);
                CargarMensajes();
            });

            // ---------------------------------------------------- Evento AJUSTES ICON
            ajustesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                SceneLoader.Instance.LoadScene(Constants.SETTINGS_SCREEN_SCENE);
            });
        }

        public void MenuVisibility(List<VisualElement> menus, VisualElement visibleMenu)
        {
            foreach (var menu in menus)
            {
                if (menu == visibleMenu && visibleMenu != null)
                {
                    menu.style.display = DisplayStyle.Flex;
                }
                else
                {
                    menu.style.display = DisplayStyle.None;
                }
            }
        }

        private void MostrarEstrellas(int reputacion)
        {
            cabeceraManagerValoracion.Clear();

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
                cabeceraManagerValoracion.Add(estrella);
            }

            cabeceraManagerValoracion.style.flexDirection = FlexDirection.Row;
        }

        private void OnBtnSeguirClicked()
        {
            Fecha f = FechaData.ObtenerFechaHoy();

            if (diaTipoActual == DiaTipo.Partido)
            {
                Debug.Log("Mi equipo juega hoy - Ir a pantalla de partido");              
                Partido proximoPartido = PartidoData.ObtenerProximoPartido(miEquipo.IdEquipo, f.ToDateTime());

                // Comprobamos si hay jugadores lesionados o sancionados en la alineacion titular en partidos de Liga            
                int cont = ComprobarLesionadosSancionados(proximoPartido);
                if (cont > 0)
                {
                    // Mostrar ventana avisando de que la alineacion es incorrecta
                    popupContainer.style.display = DisplayStyle.Flex;
                    popupText.text = "Por favor revisa la alineación, has incluido jugadores que están lesionados o sancionados y no pueden jugar el partido.";
      
                    btnCerrar.clicked -= OnCerrarClick; // Importante: limpiar listeners previos para evitar duplicados

                    void OnCerrarClick()
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);

                        btnCerrar.clicked -= OnCerrarClick;
                        popupContainer.style.display = DisplayStyle.None;
                    }

                    btnCerrar.clicked += OnCerrarClick;

                }
                else
                {
                    // Simular Mi Partido
                    SimularPartido(proximoPartido);

                    // Mostrar pantalla resumen de partido
                    resumenPartido.style.display = DisplayStyle.Flex;

                    CargarPortada();
                }

            }
            else if (diaTipoActual == DiaTipo.Simular)
            {
                Debug.Log("Hay partidos hoy - Simular");
                CargarPortada();
            }
            else
            {
                Debug.Log("Avanzar un día");
                CargarPortada();
            }

            if (FechaData.AvanzarUnDia())
            {
                ActualizarFecha();
                ActualizarBotonSeguir();
            }
        }

        private int ComprobarLesionadosSancionados(Partido miPartido)
        {
            List<Jugador> alineacion = JugadorData.MostrarAlineacion(1, 11);
            int cont = 0;
            if (miPartido.IdCompeticion >= 1 && miPartido.IdCompeticion >= 2)
            {
                foreach (var jugador in alineacion)
                {
                    if (jugador.Lesion > 0 || jugador.Sancionado > 0)
                    {
                        cont++;
                    }
                }
            }
            else
            {
                foreach (var jugador in alineacion)
                {
                    if (jugador.Lesion > 0)
                    {
                        cont++;
                    }
                }
            }

            return cont;
        }

        private void SimularPartido(Partido partido)
        {
            int comp = partido.IdCompeticion; // IdCompeticion del partido
            string nombreCompeticion = CompeticionData.MostrarNombreCompeticion(comp);
            Debug.Log($"Nombre competicion: {nombreCompeticion}");
            int miCompeticion = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).IdCompeticion;
            int miCompeticionEuropea = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).CompeticionEuropea;

            if (comp == 4)
            {
                int ronda = partido.Ronda ?? 0; // Ronda de Copa

                string nombreRonda = PartidoData.ObtenerNombreRonda(ronda);
                lblTituloJornada.text = $"{nombreCompeticion} ({nombreRonda})";
            }

            if (comp == 5)
            {
                if (partido.Ronda == 0)
                {
                    int jornada = partido.Jornada ?? 0; // Jornada de Copa de Europa 1
                    lblTituloJornada.text = $"{nombreCompeticion} (Jornada {jornada})";
                } 
                else
                {
                    int ronda = partido.Ronda ?? 0; // Ronda de Copa Europa 1

                    string nombreRonda = PartidoData.ObtenerNombreRonda(ronda);
                    lblTituloJornada.text = $"{nombreCompeticion} ({nombreRonda})";
                }
            }

            if (comp == 6)
            {
                if (partido.Ronda == 0)
                {
                    int jornada = partido.Jornada ?? 0; // Jornada de Copa de Europa 2
                    lblTituloJornada.text = $"{nombreCompeticion} (Jornada {jornada})";
                } 
                else
                {
                    int ronda = partido.Ronda ?? 0; // Ronda de Copa Europa 2

                    string nombreRonda = PartidoData.ObtenerNombreRonda(ronda);
                    lblTituloJornada.text = $"{nombreCompeticion} ({nombreRonda})";
                }
            }
            else if (comp >= 1 && comp <= 2)
            {
                int jornada = partido.Jornada ?? 0; // Jornada de Liga
                lblTituloJornada.text = $"{nombreCompeticion} (Jornada {jornada})";
            }

            // Simulación
            List<Jugador> jugadoresLocal;
            List<Jugador> jugadoresVisitante;
            bool soyLocal = miEquipo != null &&
                partido.IdEquipoLocal == miEquipo.IdEquipo;

            if (soyLocal)
            {
                jugadoresLocal = JugadorData.JugadoresMiEquipoJueganPartido(partido.IdEquipoLocal);
                jugadoresVisitante = JugadorData.JugadoresJueganPartido(partido.IdEquipoVisitante);
            }
            else
            {
                jugadoresLocal = JugadorData.JugadoresJueganPartido(partido.IdEquipoLocal);
                jugadoresVisitante = JugadorData.JugadoresMiEquipoJueganPartido(partido.IdEquipoVisitante);
            }

            // Solo tu equipo puede tener penalizaciones
            bool yoSinPortero = !JugadorData.TengoPortero(miEquipo.IdEquipo);
            bool yoSinDefensas = !JugadorData.TengoDefensas(miEquipo.IdEquipo);
            bool yoSinDelanteros = !JugadorData.TengoDelanteros(miEquipo.IdEquipo);

            // Penalizaciones propias
            bool penalizarAtaqueLocal = soyLocal && yoSinDelanteros;
            bool penalizarDefensaLocal = soyLocal && (yoSinPortero || yoSinDefensas);

            bool penalizarAtaqueVisitante = !soyLocal && yoSinDelanteros;
            bool penalizarDefensaVisitante = !soyLocal && (yoSinPortero || yoSinDefensas);

            // Simular goles
            int golesLocal = CalcularGoles(
                jugadoresLocal,
                jugadoresVisitante,
                penalizarAtaqueLocal,
                penalizarDefensaVisitante // penaliza al rival solo si TU defensa está mal
            );

            int golesVisitante = CalcularGoles(
                jugadoresVisitante,
                jugadoresLocal,
                penalizarAtaqueVisitante,
                penalizarDefensaLocal // penaliza al rival solo si TU defensa está mal
            );

            partido.GolesLocal = golesLocal;
            partido.GolesVisitante = golesVisitante;

            // Mostrar nombre de los equipos
            lblNombreLocal.text = EquipoData.ObtenerDetallesEquipo(partido.IdEquipoLocal).Nombre;
            lblNombreVisitante.text = EquipoData.ObtenerDetallesEquipo(partido.IdEquipoVisitante).Nombre;

            // Escudo equipo local
            var escudoLocal = Resources.Load<Sprite>($"EscudosEquipos/120x120/{partido.IdEquipoLocal}");
            if (escudoLocal != null)
                imgEscudoLocal.style.backgroundImage = new StyleBackground(escudoLocal);

            // Escudo equipo visitante
            var escudoVisitante = Resources.Load<Sprite>($"EscudosEquipos/120x120/{partido.IdEquipoVisitante}");
            if (escudoVisitante != null)
                imgEscudoVisitante.style.backgroundImage = new StyleBackground(escudoVisitante);

            // Mostrar el marcador
            lblGolesLocal.text = golesLocal.ToString();
            lblGolesLocal.text = golesVisitante.ToString();

            // Asignar goleadores y asistentes
            List<(Jugador, Jugador?)> golesYAsistencias = AsignarGolesYAsistencias(golesLocal, jugadoresLocal, random)
                .Concat(AsignarGolesYAsistencias(golesVisitante, jugadoresVisitante, random))
                .ToList();

            // Asignar tarjetas
            List<(Jugador, string)> tarjetas = AsignarTarjetas(jugadoresLocal, jugadoresVisitante, random);

            // Determinar y Mostrar el MVP
            Jugador mvp = DeterminarMVP(golesYAsistencias, jugadoresLocal, jugadoresVisitante);
            lblMvpDemarcacion.text = $"{JugadorData.MostrarDatosJugador(mvp.IdJugador).Rol}";
            lblMvpNombre.text = $"{JugadorData.MostrarDatosJugador(mvp.IdJugador).NombreCompleto}";
            int golesMVP = golesYAsistencias.Count(ga => ga.Item1.IdJugador == mvp.IdJugador);
            int asistenciasMVP = golesYAsistencias.Count(ga => ga.Item2 != null && ga.Item2.IdJugador == mvp.IdJugador);
            lblMvpEstadisticas.text = $"({golesMVP} {(golesMVP == 1 ? "gol" : "goles")} / " +
                                      $"{asistenciasMVP} {(asistenciasMVP == 1 ? "asistencia" : "asistencias")})";var imagenMVP = Resources.Load<Sprite>($"{mvp.RutaImagen}");
            if (imagenMVP != null)
                fotoMvp.style.backgroundImage = new StyleBackground(imagenMVP);

            // Calcular asistencia al estadio
            partido.Asistencia = EquipoData.CalcularAsistencia(partido.IdEquipoLocal);

            // Calcular recaudacion
            Taquilla taquilla = TaquillaData.RecuperarPreciosTaquilla(miEquipo.IdEquipo);
            double? recaudacion = taquilla.PrecioEntradaGeneral * (partido.Asistencia * 0.50) + 
                                 taquilla.PrecioEntradaTribuna * (partido.Asistencia * 0.40) + 
                                 taquilla.PrecioEntradaVip * (partido.Asistencia * 0.10);

            // Redondeamos a un número entero
            int recaudacionEntera = recaudacion.HasValue
                        ? (int)Math.Round(recaudacion.Value)  // Redondeamos el valor y lo convertimos a int
                        : 0;  // Si es null, asignamos 0

            lblAsistenciaPartido.text = $"{EquipoData.ObtenerDetallesEquipo(partido.IdEquipoLocal).Estadio} | {partido.Asistencia?.ToString("N0") ?? "0"} espectadores | {recaudacionEntera.ToString("N0")} €";

            // ---------------- SUMAR LA RECAUDACION DE LA TAQUILLA
            if (partido.IdEquipoLocal == miEquipo.IdEquipo)
            {
                Finanza nuevoIngresoTaquilla = new Finanza
                {
                    IdEquipo = miEquipo.IdEquipo,
                    Temporada = FechaData.temporadaActual.ToString(),
                    IdConcepto = 1,
                    Tipo = 1,
                    Cantidad = recaudacionEntera,
                    Fecha = FechaData.hoy.Date
                };
                FinanzaData.CrearIngreso(nuevoIngresoTaquilla);

                // Sumar cantidad al Presupuesto
                EquipoData.SumarCantidadAPresupuesto(miEquipo.IdEquipo, recaudacionEntera);
            }
        }

        private int CalcularGoles(List<Jugador> jugadores, List<Jugador> jugadoresRival,
                          bool penalizarAtaque, bool rivalConDefensaDebil)
        {
            if (jugadores.Count == 0 || jugadoresRival.Count == 0)
                return 0;

            // Nivel ofensivo del equipo que ataca
            double ataque = jugadores.Average(j =>
                (j.Remate + j.Pase + j.Calidad + j.Tiro + j.Regate + j.Velocidad) / 6.0);

            // Penalización si NO tienes delanteros
            if (penalizarAtaque)
                ataque *= 0.5;

            // Nivel defensivo del rival
            double defensaRival = jugadoresRival.Average(j =>
                (j.Entradas + j.Resistencia + j.Agresividad + j.Velocidad) / 4.0);

            // Si la defensa del rival está mal (porque eres tú sin portero o sin defensas)
            if (rivalConDefensaDebil)
                defensaRival *= 0.5;

            // Diferencia entre ataque y defensa
            double diferencia = ataque - defensaRival;
            double factor = 0.5 + (diferencia / 10.0);
            factor = Math.Clamp(factor, 0.2, 1.2); // para evitar resultados extremos

            // Cálculo de goles esperados con un poco de aleatoriedad
            double golesEsperados = factor * 2.0;
            double variacion = (random.NextDouble() * 2.0) - 1.0;
            int goles = (int)Math.Round(golesEsperados + variacion);

            return Math.Clamp(goles, 0, 7);
        }

        private List<(Jugador, Jugador?)> AsignarGolesYAsistencias(int goles, List<Jugador> jugadores, Random random)
        {
            List<(Jugador, Jugador?)> lista = new List<(Jugador, Jugador?)>();

            // Filtrar jugadores que no sean porteros
            var jugadoresNoPorteros = jugadores.Where(j => j.RolId != 1).ToList();

            // Asignar pesos basados en atributos y posición
            var pesosGoleadores = jugadoresNoPorteros.Select(j =>
                                                                (jugador: j, peso: (j.Remate * 1.5 + j.Tiro * 1.5 + j.Regate * 1.5 + j.Calidad) *
                                                                (j.RolId == 10 ? 10 : (j.RolId >= 7 && j.RolId <= 9 ? 5 : 0.5))) // RolId 10 tiene un peso de 10, y 7-9 tienen un peso de 5
                                                             ).ToList();

            var pesosAsistentes = jugadoresNoPorteros.Select(j =>
                (jugador: j, peso: (j.Pase * 1.5 + j.Calidad) * (j.RolId >= 6 && j.RolId <= 10 ? 2 : 1))
            ).ToList();

            // Normalizar pesos
            double totalPesoGoleador = pesosGoleadores.Sum(p => p.peso);
            double totalPesoAsistente = pesosAsistentes.Sum(p => p.peso);

            for (int i = 0; i < goles; i++)
            {
                // Selección ponderada de goleador
                Jugador goleador = SeleccionarJugadorPonderado(pesosGoleadores, totalPesoGoleador, random);

                // 80% de probabilidad de asistencia
                Jugador? asistente = random.NextDouble() > 0.2 ?
                    SeleccionarJugadorPonderado(pesosAsistentes, totalPesoAsistente, random) : null;

                lista.Add((goleador, asistente));
            }

            return lista;
        }

        // Método para seleccionar un jugador de forma ponderada
        private Jugador SeleccionarJugadorPonderado(List<(Jugador jugador, double peso)> listaPesos, double totalPeso, Random random)
        {
            double valorAleatorio = random.NextDouble() * totalPeso;
            double suma = 0;

            foreach (var (jugador, peso) in listaPesos)
            {
                suma += peso;
                if (valorAleatorio <= suma)
                    return jugador;
            }

            return listaPesos.Last().jugador; // En caso de error, devolver el último
        }
        private List<(Jugador, string)> AsignarTarjetas(List<Jugador> jugadoresLocal, List<Jugador> jugadoresVisitante, Random random)
        {
            List<(Jugador, string)> tarjetas = new List<(Jugador, string)>();

            // Calcular cuántas tarjetas habrá (máximo 6 por equipo)
            int totalTarjetasLocal = random.Next(0, 6);
            int totalTarjetasVisitante = random.Next(0, 6);

            // Asignar tarjetas a cada equipo
            AsignarTarjetasEquipo(jugadoresLocal, totalTarjetasLocal, tarjetas, random);
            AsignarTarjetasEquipo(jugadoresVisitante, totalTarjetasVisitante, tarjetas, random);

            return tarjetas;
        }
        private void AsignarTarjetasEquipo(List<Jugador> jugadores, int totalTarjetas, List<(Jugador, string)> tarjetas, Random random)
        {
            // Limitar el número de tarjetas al número de jugadores disponibles
            totalTarjetas = Math.Min(totalTarjetas, jugadores.Count);

            // Seleccionamos jugadores más agresivos para recibir tarjetas
            List<Jugador> jugadoresAgresivos = jugadores
                        .Where(j => j.Agresividad > 60)
                        .OrderByDescending(j => j.Agresividad)
                        .Take(totalTarjetas)
                        .ToList();

            Dictionary<int, int> tarjetasRecibidas = new Dictionary<int, int>(); // Control de tarjetas por jugador

            for (int i = 0; i < totalTarjetas; i++)
            {
                Jugador jugador = jugadoresAgresivos[i];

                // Ver cuántas tarjetas tiene ya este jugador
                if (!tarjetasRecibidas.ContainsKey(jugador.IdJugador))
                    tarjetasRecibidas[jugador.IdJugador] = 0;

                double probabilidad = random.NextDouble();

                if (probabilidad <= 0.50) // 50% de probabilidad de recibir una amarilla
                {
                    // Si el jugador ya tiene amarilla, solo 10% de probabilidad de recibir otra
                    if (tarjetasRecibidas[jugador.IdJugador] == 1 && random.NextDouble() > 0.20)
                        continue;

                    tarjetas.Add((jugador, "amarilla"));
                    tarjetasRecibidas[jugador.IdJugador]++;

                    // Si el jugador tiene 2 amarillas, se convierte en roja
                    if (tarjetasRecibidas[jugador.IdJugador] == 2)
                    {
                        tarjetas.Add((jugador, "dobleamarilla")); // Segunda amarilla y roja
                        tarjetasRecibidas[jugador.IdJugador] = 3; // Para evitar más tarjetas
                    }
                }
                else if (probabilidad > 0.95) // 5% de probabilidad de roja directa
                {
                    if (tarjetasRecibidas[jugador.IdJugador] == 0) // No dar roja si ya tiene amarilla
                    {
                        tarjetas.Add((jugador, "roja"));
                        tarjetasRecibidas[jugador.IdJugador] = 3; // Para evitar más tarjetas
                    }
                }
            }
        }

        private Jugador DeterminarMVP(List<(Jugador, Jugador?)> golesYAsistencias, List<Jugador> local, List<Jugador> visitante)
        {
            Dictionary<Jugador, int> puntuaciones = new Dictionary<Jugador, int>();

            foreach ((Jugador goleador, Jugador? asistente) in golesYAsistencias)
            {
                if (!puntuaciones.ContainsKey(goleador)) puntuaciones[goleador] = 0;
                puntuaciones[goleador] += 3;

                if (asistente != null)
                {
                    if (!puntuaciones.ContainsKey(asistente)) puntuaciones[asistente] = 0;
                    puntuaciones[asistente] += 2;
                }  
            }

            // Si hay puntuaciones, devolver el jugador con más puntos
            if (puntuaciones.Count > 0)
            {
                return puntuaciones.OrderByDescending(p => p.Value).First().Key;
            }
            else
            {
                // Si no hay goleadores ni asistentes, elegir un jugador aleatorio de los que jugaron
                List<Jugador> todosJugadores = local.Concat(visitante).ToList();
                if (todosJugadores.Count == 0) throw new Exception("No hay jugadores disponibles para elegir MVP.");

                Random random = new Random();
                return todosJugadores[random.Next(todosJugadores.Count)];
            }
        }

        private void ActualizarBotonSeguir()
        {
            Fecha fechaObjeto = FechaData.ObtenerFechaHoy();
            DateTime fechaActual = DateTime.Parse(fechaObjeto.Hoy);
            diaTipoActual = ObtenerDiaTipoActual(fechaActual, miEquipo.IdEquipo);
            var colorTexto = new StyleColor(new Color32(24, 58, 39, 255));

            switch (diaTipoActual)
            {
                case DiaTipo.Partido:
                    btnSeguir.text = "PARTIDO";
                    btnSeguir.style.color = colorTexto;
                    break;
                case DiaTipo.Simular:
                    btnSeguir.text = "SIMULAR";
                    btnSeguir.style.color = colorTexto;
                    break;
                default:
                    btnSeguir.text = "CONTINUAR";
                    break;
            }
        }

        private DiaTipo ObtenerDiaTipoActual(DateTime fecha, int idEquipo)
        {
            if (PartidoData.MiEquipoJuegaEl(fecha, idEquipo))
            {
                return DiaTipo.Partido;
            }

            if (PartidoData.HayPartidosEl(fecha))
            {
                return DiaTipo.Simular;
            }

            return DiaTipo.Continuar;
        }

        private void ActualizarFecha()
        {
            Fecha fechaObjeto = FechaData.ObtenerFechaHoy();
            DateTime hoy = DateTime.Parse(fechaObjeto.Hoy);
            CultureInfo culturaEspañol = new CultureInfo("es-ES");

            string dia = hoy.ToString("dd", culturaEspañol);
            string mes = hoy.ToString("MMM", culturaEspañol).ToUpper();
            string año = hoy.ToString("yyyy", culturaEspañol);
            fecha1.text = $"{dia} {mes} {año}";

            string diaSemana = hoy.ToString("dddd", culturaEspañol);
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);
            fecha2.text = diaSemana;
        }

        private void CargarPortada()
        {
            UIManager.Instance.CargarPantalla("UI/PortadaScreen/Portada", instancia =>
            {
                new PortadaManager(instancia, miEquipo, miManager);
            });
        }

        private void CargarClubInformacion(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblInformacion);
            UIManager.Instance.CargarPantalla("UI/Club/Informacion/ClubInformacion", instancia =>
            {
                new ClubInformacion(instancia, miEquipo, miManager);
            });
        }

        private void CargarClubPlantilla(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblPlantilla);
            UIManager.Instance.CargarPantalla("UI/Club/Plantilla/ClubPlantilla", instancia =>
            {
                new ClubPlantilla(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarClubEmpleados(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblEmpleados);
            UIManager.Instance.CargarPantalla("UI/Club/Empleados/ClubEmpleados", instancia =>
            {
                new ClubEmpleados(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarClubLesionados(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblLesionados);
            UIManager.Instance.CargarPantalla("UI/Club/Lesionados/ClubLesionados", instancia =>
            {
                new ClubLesionados(instancia, miEquipo, miManager);
            });
        }

        private void CargarEntrenadorAlineacion(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblAlineacion);
            UIManager.Instance.CargarPantalla("UI/Entrenador/Alineacion/EntrenadorAlineacion", instancia =>
            {
                new EntrenadorAlineacion(instancia, miEquipo, miManager);
            });
        }

        private void CargarEntrenadorEntrenamiento(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblEntrenamiento);
            UIManager.Instance.CargarPantalla("UI/Entrenador/Entrenamiento/EntrenadorEntrenamiento", instancia =>
            {
                new EntrenadorEntrenamiento(instancia, miEquipo, miManager);
            });
        }

        private void CargarEntrenadorRival(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblRival);
            UIManager.Instance.CargarPantalla("UI/Entrenador/Rival/EntrenadorRival", instancia =>
            {
                new EntrenadorRival(instancia, miEquipo, miManager);
            });
        }

        private void CargarCompeticionesClasificacion(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblClasificacion);
            UIManager.Instance.CargarPantalla("UI/Competiciones/Clasificacion/CompeticionesClasificacion", instancia =>
            {
                new CompeticionesClasificacion(instancia, miEquipo, miManager);
            });
        }

        private void CargarCompeticionesEstadisticas(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblEstadisticas);
            UIManager.Instance.CargarPantalla("UI/Competiciones/Estadisticas/CompeticionesEstadisticas", instancia =>
            {
                new CompeticionesEstadisticas(instancia, miEquipo, miManager);
            });
        }

        private void CargarCompeticionesPalmaresEquipos(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblPalmaresEquipos);
            UIManager.Instance.CargarPantalla("UI/Competiciones/PalmaresEquipos/CompeticionesPalmaresEquipos", instancia =>
            {
                new CompeticionesPalmaresEquipos(instancia, miEquipo, miManager);
            });
        }

        private void CargarCompeticionesPalmaresJugadores(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblPalmaresJugadores);
            UIManager.Instance.CargarPantalla("UI/Competiciones/PalmaresJugadores/CompeticionesPalmaresJugadores", instancia =>
            {
                new CompeticionesPalmaresJugadores(instancia, miEquipo, miManager);
            });
        }

        private void CargarCompeticionesResultados(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblResultados);
            UIManager.Instance.CargarPantalla("UI/Competiciones/Resultados/CompeticionesResultados", instancia =>
            {
                new CompeticionesResultados(instancia, miEquipo, miManager);
            });
        }

        private void CargarCalendario()
        {
            UIManager.Instance.CargarPantalla("UI/CalendarioScreen/Calendario", instancia =>
            {
                new Calendario(instancia, miEquipo, miManager);
            });
        }

        private void CargarFichajesMercado(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblMercado);
            UIManager.Instance.CargarPantalla("UI/Fichajes/Mercado/FichajesMercado", instancia =>
            {
                new FichajesMercado(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarFichajesBusqueda(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblBusqueda);
            UIManager.Instance.CargarPantalla("UI/Fichajes/BuscarPorEquipo/FichajesBuscarPorEquipo", instancia =>
            {
                new FichajesBuscarPorEquipo(instancia, miEquipo, miManager, this, -1);
            });
        }

        private void CargarFichajesCartera(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblCartera);
            UIManager.Instance.CargarPantalla("UI/Fichajes/Cartera/FichajesCartera", instancia =>
            {
                new FichajesCartera(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarFichajesListaTraspasos(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblListaTraspasos);
            UIManager.Instance.CargarPantalla("UI/Fichajes/ListaTraspasos/FichajesListaTraspasos", instancia =>
            {
                new FichajesListaTraspasos(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarFichajesEstadoOfertas(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblEstadoOfertas);
            UIManager.Instance.CargarPantalla("UI/Fichajes/EstadoOfertas/FichajesEstadoOfertas", instancia =>
            {
                new FichajesEstadoOfertas(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarFinanzasIngresos(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblIngresos);
            UIManager.Instance.CargarPantalla("UI/Finanzas/Ingresos/FinanzasIngresos", instancia =>
            {
                new FinanzasIngresos(instancia, miEquipo, miManager);
            });
        }

        private void CargarFinanzasGastos(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblGastos);
            UIManager.Instance.CargarPantalla("UI/Finanzas/Gastos/FinanzasGastos", instancia =>
            {
                new FinanzasGastos(instancia, miEquipo, miManager);
            });
        }

        private void CargarFinanzasPatrocinador(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblPatrocinadores);
            UIManager.Instance.CargarPantalla("UI/Finanzas/Patrocinador/FinanzasPatrocinador", instancia =>
            {
                new FinanzasPatrocinador(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarFinanzasTelevision(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblTelevision);
            UIManager.Instance.CargarPantalla("UI/Finanzas/Television/FinanzasTelevision", instancia =>
            {
                new FinanzasTelevision(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarFinanzasPrestamos(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblPrestamos);
            UIManager.Instance.CargarPantalla("UI/Finanzas/Prestamos/FinanzasPrestamos", instancia =>
            {
                new FinanzasPrestamos(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarEstadioInformacion(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblEstadioInformacion);
            UIManager.Instance.CargarPantalla("UI/Estadio/EstadioInformacion/EstadioInformacion", instancia =>
            {
                new EstadioInformacion(instancia, miEquipo, miManager);
            });
        }

        private void CargarEstadioEntradas(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblEntradas);
            UIManager.Instance.CargarPantalla("UI/Estadio/Entradas/EstadioEntradas", instancia =>
            {
                new EstadioEntradas(instancia, miEquipo, miManager);
            });
        }

        private void CargarEstadioAmpliaciones(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblAmpliaciones);
            UIManager.Instance.CargarPantalla("UI/Estadio/Ampliaciones/EstadioAmpliaciones", instancia =>
            {
                new EstadioAmpliaciones(instancia, miEquipo, miManager);
            });
        }

        private void CargarManagerFicha(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblManagerFicha);
            UIManager.Instance.CargarPantalla("UI/Manager/Ficha/ManagerFicha", instancia =>
            {
                new ManagerFicha(instancia, miEquipo, miManager);
            });
        }

        private void CargarManagerPalmares(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblManagerPalmares);
            UIManager.Instance.CargarPantalla("UI/Manager/Palmares/ManagerPalmares", instancia =>
            {
                new ManagerPalmares(instancia, miEquipo, miManager);
            });
        }

        private void CargarMensajes()
        {
            UIManager.Instance.CargarPantalla("UI/MensajesScreen/Mensajes", instancia =>
            {
                new Mensajes(instancia, miEquipo, miManager);
            });
        }

        public void CambiarColorTextoClub(List<Label> clubList, Label label)
        {
            foreach (var item in clubList)
            {
                if (item == label)
                {
                    item.style.color = (Color)new Color32(0x1E, 0x72, 0x3C, 0xFF);
                }
                else
                {
                    item.style.color = Color.white;
                }
            }
        }
    }
}