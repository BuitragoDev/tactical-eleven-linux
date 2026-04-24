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
        private VisualElement goleadoresLocalArea, tarjetasLocalArea, goleadoresVisitanteArea, tarjetasVisitanteArea;
        public VisualElement clubMenu, alineacionMenu, competicionMenu, calendarioMenu, fichajesMenu, finanzasMenu,
               estadioMenu, managerMenu, mensajesMenu;
        private VisualElement mainContainer, popupContainer, resumenPartido, imgEscudoLocal, imgEscudoVisitante, fotoMvp,
                goleadoresLocalContainer, goleadoresVisitanteContainer, tarjetasLocalContainer, tarjetasVisitanteContainer,
                resumenJornada, listaPartidosLeft, listaPartidosRight;
        private Button btnSeguir, btnCerrar, resumenPartidoBtnContinuar, resumenJornadaBtnContinuar;
        private Label miEquipoNombre, managerNombre, fecha1, fecha2, popupText, resumenPartidoCabeceraTitulo,
                lblTituloJornada, lblGolesLocal, lblGolesVisitante, lblNombreLocal, lblNombreVisitante, lblAsistenciaPartido,
                lblMvpDemarcacion, lblMvpNombre, lblMvpEstadisticas, lblTituloJornada2;
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
            goleadoresLocalContainer = root.Q<VisualElement>("goleadores-local-area");
            tarjetasLocalContainer = root.Q<VisualElement>("tarjetas-local-area");
            goleadoresVisitanteContainer = root.Q<VisualElement>("goleadores-visitante-area");
            tarjetasVisitanteContainer = root.Q<VisualElement>("tarjetas-visitante-area");

            // Resumen de la jornada
            resumenJornada = root.Q<VisualElement>("resumen-jornada");
            resumenJornadaBtnContinuar = root.Q<Button>("btnContinuarJornada");
            lblTituloJornada2 = root.Q<Label>("lblTituloJornada2");
            listaPartidosLeft = root.Q<VisualElement>("lista-partidos-left-area");
            listaPartidosRight = root.Q<VisualElement>("lista-partidos-right-area");

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

                // Comprobamos si hay otros partidos hoy
                List<Partido> listaPartidos = PartidoData.PartidosHoy(miEquipo.IdEquipo);
                Debug.Log($"Partidos encontrados hoy (sin mi equipo): {listaPartidos?.Count ?? 0}");

                if (listaPartidos != null && listaPartidos.Count > 0)
                {
                    // Simular todos los partidos y guardar en BD
                    foreach (var partido in listaPartidos)
                    {
                        SimularPartidoYGuardar(partido, false);
                    }
                    
                    // Pintar resumen jornada
                    SimularJornada(listaPartidos);
                    resumenJornada.style.display = DisplayStyle.Flex;
                }
                else
                {
                    // No hay más partidos, avanzar el día
                    if (FechaData.AvanzarUnDia())
                    {
                        ActualizarFecha();
                        ActualizarBotonSeguir();
                        CargarPortada();
                    }
                }
            };

            // -- Botón Continuar Resumen jornada ---
            resumenJornadaBtnContinuar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                resumenJornada.style.display = DisplayStyle.None;

                // Avanzar el día después de ver los partidos
                if (FechaData.AvanzarUnDia())
                {
                    ActualizarFecha();
                    ActualizarBotonSeguir();
                    CargarPortada();
                }
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
            bool diaAvanzado = false;

            if (diaTipoActual == DiaTipo.Partido)
            {
                Partido proximoPartido = PartidoData.ObtenerProximoPartido(miEquipo.IdEquipo, f.ToDateTime());

                // Comprobamos si hay jugadores lesionados o sancionados en la alineacion titular en partidos de Liga            
                int cont = ComprobarLesionadosSancionados(proximoPartido);
                if (cont > 0)
                {
                    // Mostrar ventana avisando de que la alineacion es incorrecta
                    popupContainer.style.display = DisplayStyle.Flex;
                    popupText.text = "Por favor revisa la alineación, has incluido jugadores que están lesionados o sancionados y no pueden jugar el partido.";
       
                    btnCerrar.clicked -= OnCerrarClick;

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
                    // Simular Mi Partido y guardar en BD
                    DatosSimulacion datosMiPartido = SimularPartidoYGuardar(proximoPartido, true);

                    // Pintar resumen de mi partido
                    PintarResumenMiPartido(proximoPartido, datosMiPartido);

                    // Mostrar pantalla resumen de partido
                    resumenPartido.style.display = DisplayStyle.Flex;
                }
            }
            else if (diaTipoActual == DiaTipo.Simular)
            {
                Debug.Log("Hay partidos hoy - Simular");

                if (FechaData.AvanzarUnDia())
                {
                    ActualizarFecha();
                    ActualizarBotonSeguir();
                    CargarPortada();
                    diaAvanzado = true;
                }
            }
            else
            {
                Debug.Log("Avanzar un día");

                if (FechaData.AvanzarUnDia())
                {
                    ActualizarFecha();
                    ActualizarBotonSeguir();
                    CargarPortada();
                    diaAvanzado = true;
                }
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
            MostrarCompeticionRonda(partido);

            // ------------------------------------------------------------------- SIMULACIÓN
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
            var escudoLocal = Resources.Load<Sprite>($"EscudosEquipos/{partido.IdEquipoLocal}");
            if (escudoLocal != null)
                imgEscudoLocal.style.backgroundImage = new StyleBackground(escudoLocal);

            // Escudo equipo visitante
            var escudoVisitante = Resources.Load<Sprite>($"EscudosEquipos/{partido.IdEquipoVisitante}");
            if (escudoVisitante != null)
                imgEscudoVisitante.style.backgroundImage = new StyleBackground(escudoVisitante);

            // Mostrar el marcador
            lblGolesLocal.text = golesLocal.ToString();
            lblGolesVisitante.text = golesVisitante.ToString();

            // Asignar goleadores y asistentes
            List<(Jugador, Jugador?)> goleadoresLocal = AsignarGolesYAsistencias(golesLocal, jugadoresLocal, random);
            List<(Jugador, Jugador?)> goleadoresVisitante = AsignarGolesYAsistencias(golesVisitante, jugadoresVisitante, random);
            List<(Jugador, Jugador?)> golesYAsistencias = goleadoresLocal.Concat(goleadoresVisitante).ToList();

            // Asignar tarjetas
            var (tarjetasLocal, tarjetasVisitante) = AsignarTarjetas(jugadoresLocal, jugadoresVisitante, random);

            // Mostrar goleadores y tarjetas en el UI
            MostrarGoleadoresYAsistentes(goleadoresLocalContainer, goleadoresLocal);
            MostrarGoleadoresYAsistentes(goleadoresVisitanteContainer, goleadoresVisitante);
            MostrarTarjetas(tarjetasLocalContainer, tarjetasLocal);
            MostrarTarjetas(tarjetasVisitanteContainer, tarjetasVisitante);

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

            // ------------------------------------------------------------------- OTRAS GESTIONES
            // Calcular recaudacion
            Taquilla taquilla = TaquillaData.RecuperarPreciosTaquilla(miEquipo.IdEquipo);
            double? recaudacion = taquilla.PrecioEntradaGeneral * (partido.Asistencia * 0.50) + 
                                 taquilla.PrecioEntradaTribuna * (partido.Asistencia * 0.40) + 
                                 taquilla.PrecioEntradaVip * (partido.Asistencia * 0.10);

            // Redondeamos a un número entero
            int recaudacionEntera = recaudacion.HasValue
                        ? (int)Math.Round(recaudacion.Value)  // Redondeamos el valor y lo convertimos a int
                        : 0;  // Si es null, asignamos 0

            lblAsistenciaPartido.text = $"{EquipoData.ObtenerDetallesEquipo(partido.IdEquipoLocal).Estadio} " +
                                        $" 🏟️  {partido.Asistencia?.ToString("N0") ?? "0"} espectadores " +
                                        $" 💰  {recaudacionEntera.ToString("N0")} €";
            // ------------------------------------------------------------- SUMAR LA RECAUDACION DE LA TAQUILLA
            if (partido.IdEquipoLocal == miEquipo.IdEquipo)
            {
                // Crear ingreso del pago por taquilla
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

                // Sumar la indemnización al Presupuesto
                EquipoData.SumarCantidadAPresupuesto(miEquipo.IdEquipo, recaudacionEntera);
            }

            // ----------------------------------------------- CREAR INGRESO EXTRA TELEVISION POR PARTIDO DE COPA
            if (partido.IdEquipoLocal == miEquipo.IdEquipo && partido.IdCompeticion == 4)
            {
                int bonusPartidoCopa = 1500000;
                // Crear ingreso del pago por taquilla
                Finanza nuevoIngresoTaquilla = new Finanza
                {
                    IdEquipo = miEquipo.IdEquipo,
                    Temporada = FechaData.temporadaActual.ToString(),
                    IdConcepto = 1,
                    Tipo = 1,
                    Cantidad = bonusPartidoCopa,
                    Fecha = FechaData.hoy.Date
                };
                FinanzaData.CrearIngreso(nuevoIngresoTaquilla);

                // Sumar la indemnización al Presupuesto
                EquipoData.SumarCantidadAPresupuesto(miEquipo.IdEquipo, bonusPartidoCopa);
                
            }

            // ----------------------------------------------------------- ACTUALIZAR RESULTADO SI ES UN AMISTOSO
            if (partido.IdCompeticion == 10)
            {
                PartidoData.ActualizarPartido(partido);
            }

            // -------------------------------------------------------- ACTUALIZAR DATOS SI ES UN PARTIDO DE LIGA
            if (partido.IdCompeticion >= 1 && partido.IdCompeticion <= 2)
            {
                PartidoData.ActualizarPartido(partido);

                // Actualizar la clasificacion
                Clasificacion cla_local;
                Clasificacion cla_visitante;
                if (golesLocal == golesVisitante)
                {
                    cla_local = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoLocal,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 1,
                        Perdidos = 0,
                        Puntos = 1,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesLocal,
                        GolesContra = golesVisitante,
                        Racha = 0
                    };
                    cla_visitante = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoVisitante,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 1,
                        Perdidos = 0,
                        Puntos = 1,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesVisitante,
                        GolesContra = golesLocal,
                        Racha = 0
                    };
                }
                else if (golesLocal > golesVisitante)
                {
                    cla_local = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoLocal,
                        Jugados = 1,
                        Ganados = 1,
                        Empatados = 0,
                        Perdidos = 0,
                        Puntos = 3,
                        LocalVictorias = 1,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesLocal,
                        GolesContra = golesVisitante,
                        Racha = 1
                    };
                    cla_visitante = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoVisitante,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 0,
                        Perdidos = 1,
                        Puntos = 0,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 1,
                        GolesFavor = golesVisitante,
                        GolesContra = golesLocal,
                        Racha = -1
                    };
                }
                else
                {
                    cla_local = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoLocal,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 0,
                        Perdidos = 1,
                        Puntos = 0,
                        LocalVictorias = 0,
                        LocalDerrotas = 1,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesLocal,
                        GolesContra = golesVisitante,
                        Racha = -1
                    };
                    cla_visitante = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoVisitante,
                        Jugados = 1,
                        Ganados = 1,
                        Empatados = 0,
                        Perdidos = 0,
                        Puntos = 3,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 1,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesVisitante,
                        GolesContra = golesLocal,
                        Racha = 1
                    };
                }

                if (partido.IdCompeticion == 1)
                {
                    // Actualizar la Clasificacion de la Division de los equipos de mi equipo
                    ClasificacionData.ActualizarClasificacion(cla_local);
                    ClasificacionData.ActualizarClasificacion(cla_visitante);
                }
                else
                {
                    // Actualizar la Clasificacion de la Division de los equipos de mi equipo
                    ClasificacionData.ActualizarClasificacion2(cla_local);
                    ClasificacionData.ActualizarClasificacion2(cla_visitante);
                }

                // Actualizar estadísticas de cada jugador en la base de datos
                List<(Jugador, string)> tarjetas = tarjetasLocal.Concat(tarjetasVisitante).ToList();
                ActualizarEstadisticasPartido(jugadoresLocal, jugadoresVisitante, golesYAsistencias, tarjetas, mvp);

                // Actualizar la BD de jugadores sancionados
                ActualizarJugadoresSancionados(tarjetasLocal, tarjetasVisitante);  
            }

            // -------------------------------------------------------- ACTUALIZAR DATOS SI ES UN PARTIDO DE COPA
            if (partido.IdCompeticion == 4)
            {
                PartidoData.ActualizarPartidoCopaNacional(partido);
            }

            // -------------------------------------------- ACTUALIZAR DATOS SI ES UN PARTIDO DE COPA DE EUROPA 1
            if (partido.IdCompeticion == 5)
            {
                PartidoData.ActualizarPartidoCopaEuropa1(partido);

                // Actualizar la clasificacion
                Clasificacion cla_local;
                Clasificacion cla_visitante;
                if (golesLocal == golesVisitante)
                {
                    cla_local = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoLocal,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 1,
                        Perdidos = 0,
                        Puntos = 1,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesLocal,
                        GolesContra = golesVisitante,
                        Racha = 0
                    };
                    cla_visitante = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoVisitante,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 1,
                        Perdidos = 0,
                        Puntos = 1,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesVisitante,
                        GolesContra = golesLocal,
                        Racha = 0
                    };
                }
                else if (golesLocal > golesVisitante)
                {
                    cla_local = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoLocal,
                        Jugados = 1,
                        Ganados = 1,
                        Empatados = 0,
                        Perdidos = 0,
                        Puntos = 3,
                        LocalVictorias = 1,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesLocal,
                        GolesContra = golesVisitante,
                        Racha = 1
                    };
                    cla_visitante = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoVisitante,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 0,
                        Perdidos = 1,
                        Puntos = 0,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 1,
                        GolesFavor = golesVisitante,
                        GolesContra = golesLocal,
                        Racha = -1
                    };
                }
                else
                {
                    cla_local = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoLocal,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 0,
                        Perdidos = 1,
                        Puntos = 0,
                        LocalVictorias = 0,
                        LocalDerrotas = 1,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesLocal,
                        GolesContra = golesVisitante,
                        Racha = -1
                    };
                    cla_visitante = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoVisitante,
                        Jugados = 1,
                        Ganados = 1,
                        Empatados = 0,
                        Perdidos = 0,
                        Puntos = 3,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 1,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesVisitante,
                        GolesContra = golesLocal,
                        Racha = 1
                    };
                }

                // Comprobar si es un partido de grupo
                if (partido.Jornada <= 8)
                {
                    // Actualizar la Clasificacion de la Copa de Europa 1
                    ClasificacionData.ActualizarClasificacionEuropa1(cla_local);
                    ClasificacionData.ActualizarClasificacionEuropa1(cla_visitante);
                }
                
                // Actualizar estadísticas de cada jugador en la base de datos
                List<(Jugador, string)> tarjetas = tarjetasLocal.Concat(tarjetasVisitante).ToList();
                ActualizarEstadisticasPartidoEuropa(jugadoresLocal, jugadoresVisitante, golesYAsistencias, tarjetas, mvp);
            }

            // -------------------------------------------- ACTUALIZAR DATOS SI ES UN PARTIDO DE COPA DE EUROPA 2
            if (partido.IdCompeticion == 6)
            {
                PartidoData.ActualizarPartidoCopaEuropa2(partido);

                // Actualizar la clasificacion
                Clasificacion cla_local;
                Clasificacion cla_visitante;
                if (golesLocal == golesVisitante)
                {
                    cla_local = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoLocal,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 1,
                        Perdidos = 0,
                        Puntos = 1,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesLocal,
                        GolesContra = golesVisitante,
                        Racha = 0
                    };
                    cla_visitante = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoVisitante,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 1,
                        Perdidos = 0,
                        Puntos = 1,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesVisitante,
                        GolesContra = golesLocal,
                        Racha = 0
                    };
                }
                else if (golesLocal > golesVisitante)
                {
                    cla_local = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoLocal,
                        Jugados = 1,
                        Ganados = 1,
                        Empatados = 0,
                        Perdidos = 0,
                        Puntos = 3,
                        LocalVictorias = 1,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesLocal,
                        GolesContra = golesVisitante,
                        Racha = 1
                    };
                    cla_visitante = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoVisitante,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 0,
                        Perdidos = 1,
                        Puntos = 0,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 1,
                        GolesFavor = golesVisitante,
                        GolesContra = golesLocal,
                        Racha = -1
                    };
                }
                else
                {
                    cla_local = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoLocal,
                        Jugados = 1,
                        Ganados = 0,
                        Empatados = 0,
                        Perdidos = 1,
                        Puntos = 0,
                        LocalVictorias = 0,
                        LocalDerrotas = 1,
                        VisitanteVictorias = 0,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesLocal,
                        GolesContra = golesVisitante,
                        Racha = -1
                    };
                    cla_visitante = new Clasificacion
                    {
                        IdEquipo = partido.IdEquipoVisitante,
                        Jugados = 1,
                        Ganados = 1,
                        Empatados = 0,
                        Perdidos = 0,
                        Puntos = 3,
                        LocalVictorias = 0,
                        LocalDerrotas = 0,
                        VisitanteVictorias = 1,
                        VisitanteDerrotas = 0,
                        GolesFavor = golesVisitante,
                        GolesContra = golesLocal,
                        Racha = 1
                    };
                }

                // Comprobar si es un partido de grupo
                if (partido.Jornada <= 8)
                {
                    // Actualizar la Clasificacion de la Copa de Europa 1
                    ClasificacionData.ActualizarClasificacionEuropa2(cla_local);
                    ClasificacionData.ActualizarClasificacionEuropa2(cla_visitante);
                }
                
                // Actualizar estadísticas de cada jugador en la base de datos
                List<(Jugador, string)> tarjetas = tarjetasLocal.Concat(tarjetasVisitante).ToList();
                ActualizarEstadisticasPartidoEuropa(jugadoresLocal, jugadoresVisitante, golesYAsistencias, tarjetas, mvp);
            }

            // Comprobar si ha habido algun lesionado y actualizarlo en la BD
            ComprobarJugadoresLesionados(jugadoresLocal, jugadoresVisitante);

            // Actualizar las confianzas
            ActualizarConfianzaManager(partido, golesLocal, golesVisitante);

            // Actualizar atributos de los jugadores
            ActualizacionAtributos(jugadoresLocal, jugadoresVisitante, partido.IdEquipoLocal,
                                   partido.IdEquipoVisitante, golesLocal, golesVisitante, golesYAsistencias, mvp);

            ActualizarPresupuestoMainScreen();
        }

        private void SimularJornada(List<Partido> listaPartidos)
        {
            listaPartidosLeft.Clear();
            listaPartidosRight.Clear();

            int comp = listaPartidos[0].IdCompeticion;
            string nombreCompeticion = "";
            if(comp == 1 || comp == 2)
            {
                nombreCompeticion = "LIGA";
            }
            else
            {
                nombreCompeticion = CompeticionData.MostrarNombreCompeticion(comp);
            }            
            lblTituloJornada2.text = $"{nombreCompeticion.ToUpper()} (";

            Fecha fechaObjeto = FechaData.ObtenerFechaHoy();
            if (fechaObjeto == null || string.IsNullOrEmpty(fechaObjeto.Hoy))
            {
                lblTituloJornada2.text = "No hay partidos hoy";
            }
            else
            {
                DateTime hoy = DateTime.Parse(fechaObjeto.Hoy);
                CultureInfo culturaEspañol = new CultureInfo("es-ES");
                string dia = hoy.ToString("dd", culturaEspañol);
                string mes = hoy.ToString("MM", culturaEspañol).ToUpper();
                string año = hoy.ToString("yyyy", culturaEspañol);
                lblTituloJornada2.text += $"{dia}/{mes}/{año})";
            }
            
            int cont = 0;
            foreach (var partido in listaPartidos)
            {
                if (partido.IdCompeticion == 1 || partido.IdCompeticion == 2)
                {
                    if (partido.IdCompeticion == 1)
                    {
                        var fila = CrearFilaPartido(partido);
                        listaPartidosLeft.Add(fila);
                    }

                    if (partido.IdCompeticion == 2)
                    {
                        var fila = CrearFilaPartido(partido);
                        listaPartidosRight.Add(fila);
                    }
                }
                else if (partido.IdCompeticion == 4)
                {
                    var fila = CrearFilaPartido(partido);

                    if (cont < 16)
                        listaPartidosLeft.Add(fila);
                    else
                        listaPartidosRight.Add(fila);
                }
                else if (partido.IdCompeticion == 5)
                {
                    var fila = CrearFilaPartido(partido);

                    if (cont < 11)
                        listaPartidosLeft.Add(fila);
                    else
                        listaPartidosRight.Add(fila);
                }
                else if (partido.IdCompeticion == 6)
                {
                    var fila = CrearFilaPartido(partido);

                    if (cont < 11)
                        listaPartidosLeft.Add(fila);
                    else
                        listaPartidosRight.Add(fila);
                }
                else
                {
                    var fila = CrearFilaPartido(partido);

                    if (cont < 16)
                        listaPartidosLeft.Add(fila);
                    else
                        listaPartidosRight.Add(fila);
                }

                cont++;
            }
        }

        private VisualElement CrearFilaPartido(Partido p)
        {
            var fila = new VisualElement();
            fila.style.flexDirection = FlexDirection.Row;
            fila.style.width = Length.Percent(100);
            fila.style.minHeight = 50;
            fila.style.maxHeight = 50;

            fila.style.alignItems = Align.Center;

            Color bg = new Color32(255, 255, 255, 255);
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
        private (List<(Jugador, string)>, List<(Jugador, string)>) AsignarTarjetas(List<Jugador> jugadoresLocal, List<Jugador> jugadoresVisitante, Random random)
        {
            List<(Jugador, string)> tarjetasLocal = new List<(Jugador, string)>();
            List<(Jugador, string)> tarjetasVisitante = new List<(Jugador, string)>();

            // Calcular cuántas tarjetas habrá (máximo 6 por equipo)
            int totalTarjetasLocal = random.Next(0, 6);
            int totalTarjetasVisitante = random.Next(0, 6);

            // Asignar tarjetas a cada equipo
            AsignarTarjetasEquipo(jugadoresLocal, totalTarjetasLocal, tarjetasLocal, random);
            AsignarTarjetasEquipo(jugadoresVisitante, totalTarjetasVisitante, tarjetasVisitante, random);

            return (tarjetasLocal, tarjetasVisitante);
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

        private void MostrarGoleadoresYAsistentes(VisualElement area, List<(Jugador, Jugador?)> goleadores)
        {
            area.Clear();

            if (goleadores.Count == 0)
            {
                Label lbl = new Label
                {
                    text = "-",
                    style =
                    {
                        color = new Color(0f, 0f, 0f, 0.7f),
                        fontSize = 18,
                        marginTop = 5
                    }
                };
                area.Add(lbl);
                return;
            }

            foreach (var (goleador, asistente) in goleadores)
            {
                string texto = $"{JugadorData.MostrarDatosJugador(goleador.IdJugador).NombreCompleto}";
                if (asistente != null)
                {
                    texto += $" ({JugadorData.MostrarDatosJugador(asistente.IdJugador).NombreCompleto})";
                }

                Label lbl = new Label
                {
                    text = texto,
                    style =
                    {
                        color = Color.black,
                        fontSize = 18,
                        marginTop = 5
                    }
                };
                area.Add(lbl);
            }
        }

        private void MostrarTarjetas(VisualElement area, List<(Jugador, string)> tarjetas)
        {
            area.Clear();

            if (tarjetas.Count == 0)
            {
                Label lbl = new Label
                {
                    text = "-",
                    style =
                    {
                        color = new Color(0f, 0f, 0f, 0.7f),
                        fontSize = 18,
                        marginTop = 5
                    }
                };
                area.Add(lbl);
                return;
            }

            foreach (var (jugador, tipo) in tarjetas)
            {
                string texto = $"{JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto}";
                
                string colorTarjeta = tipo switch
                {
                    "amarilla" => "🟨",
                    "dobleamarilla" => "🟨🟥",
                    "roja" => "🟥",
                    _ => "🟨"
                };

                Label lbl = new Label
                {
                    text = $"{colorTarjeta} {texto}",
                    style =
                    {
                        color = Color.black,
                        fontSize = 18,
                        marginTop = 5
                    }
                };
                area.Add(lbl);
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

        private void ActualizarPresupuestoMainScreen()
        {
            // Actualizar Presupuesto en MainScreen
            Equipo equipo = EquipoData.ObtenerDetallesEquipo((int)miManager.IdEquipo);
            float presupuestoConversion = equipo.Presupuesto * Constants.EURO_VALUE;
            string symbol = Constants.EURO_SYMBOL;

            string currency = PlayerPrefs.GetString("Currency", string.Empty);
            if (currency != string.Empty)
            {
                switch (currency)
                {
                    case Constants.EURO_NAME:
                        presupuestoConversion = equipo.Presupuesto * Constants.EURO_VALUE;
                        symbol = Constants.EURO_SYMBOL;
                        break;
                    case Constants.POUND_NAME:
                        presupuestoConversion = equipo.Presupuesto * Constants.POUND_VALUE;
                        symbol = Constants.POUND_SYMBOL;
                        break;
                    case Constants.DOLLAR_NAME:
                        presupuestoConversion = equipo.Presupuesto * Constants.DOLLAR_VALUE;
                        symbol = Constants.DOLLAR_SYMBOL;
                        break;
                    default:
                        presupuestoConversion = equipo.Presupuesto * Constants.EURO_VALUE;
                        symbol = Constants.EURO_SYMBOL;
                        break;
                }
            }

            miPresupuesto.text = $"{presupuestoConversion.ToString("N0")} {symbol}";
        }

        private void ActualizarEstadisticasPartido(List<Jugador> jugadoresLocal, List<Jugador> jugadoresVisitante,
                                           List<(Jugador, Jugador?)> golesYAsistencias,
                                           List<(Jugador, string)> tarjetas,
                                           Jugador mvp)
        {
            Dictionary<int, Estadistica> estadisticas = new Dictionary<int, Estadistica>();

            // Asegurar que todos los jugadores del partido sumen 1 partido jugado
            foreach (var jugador in jugadoresLocal.Concat(jugadoresVisitante))
            {
                estadisticas[jugador.IdJugador] = new Estadistica
                {
                    IdJugador = jugador.IdJugador,
                    PartidosJugados = 1, // Todos los jugadores suman 1 partido
                    Goles = 0,
                    Asistencias = 0,
                    TarjetasAmarillas = 0,
                    TarjetasRojas = 0,
                    MVP = 0
                };

                // -------------- CREAR GASTO POR PARTIDO
                Jugador player = JugadorData.MostrarDatosJugador(jugador.IdJugador);

                // Bonus
                int bonusPartido = player.BonusPartido ?? 0;
                if (bonusPartido != 0)
                {
                    Finanza nuevoGasto = new Finanza
                        {
                            IdEquipo = miEquipo.IdEquipo,
                            Temporada = FechaData.temporadaActual.ToString(),
                            IdConcepto = 16,
                            Tipo = 2,
                            Cantidad = (double)bonusPartido,
                            Fecha = FechaData.hoy.Date
                        };
                    FinanzaData.CrearGasto(nuevoGasto);

                    // Restar la indemnización al Presupuesto
                    EquipoData.RestarCantidadAPresupuesto(miEquipo.IdEquipo, bonusPartido);
                }
            }

            // Sumar goles y asistencias
            foreach (var (goleador, asistente) in golesYAsistencias)
            {
                estadisticas[goleador.IdJugador].Goles++;

                if (asistente != null)
                {
                    estadisticas[asistente.IdJugador].Asistencias++;
                }
            }

            // Sumar tarjetas
            foreach (var (jugador, tipoTarjeta) in tarjetas)
            {
                if (tipoTarjeta.Contains("roja")) estadisticas[jugador.IdJugador].TarjetasRojas++;
                if (tipoTarjeta.Contains("amarilla")) estadisticas[jugador.IdJugador].TarjetasAmarillas++;
            }

            // Sumar MVP
            estadisticas[mvp.IdJugador].MVP++;

            // Guardar estadísticas en la base de datos
            foreach (var estadistica in estadisticas.Values)
            {
                EstadisticaJugadorData.ActualizarEstadisticas(estadistica);
            }
        }

        private void ActualizarEstadisticasPartidoEuropa(List<Jugador> jugadoresLocal, List<Jugador> jugadoresVisitante,
                                           List<(Jugador, Jugador?)> golesYAsistencias,
                                           List<(Jugador, string)> tarjetas,
                                           Jugador mvp)
        {
            Dictionary<int, Estadistica> estadisticas = new Dictionary<int, Estadistica>();

            // Asegurar que todos los jugadores del partido sumen 1 partido jugado
            foreach (var jugador in jugadoresLocal.Concat(jugadoresVisitante))
            {
                estadisticas[jugador.IdJugador] = new Estadistica
                {
                    IdJugador = jugador.IdJugador,
                    PartidosJugados = 1, // Todos los jugadores suman 1 partido
                    Goles = 0,
                    Asistencias = 0,
                    TarjetasAmarillas = 0,
                    TarjetasRojas = 0,
                    MVP = 0
                };

                // -------------- CREAR GASTO POR PARTIDO
                Jugador player = JugadorData.MostrarDatosJugador(jugador.IdJugador);

                // Bonus
                int bonusPartido = player.BonusPartido ?? 0;
                if (bonusPartido != 0)
                {
                    Finanza nuevoGasto = new Finanza
                        {
                            IdEquipo = miEquipo.IdEquipo,
                            Temporada = FechaData.temporadaActual.ToString(),
                            IdConcepto = 16,
                            Tipo = 2,
                            Cantidad = (double)bonusPartido,
                            Fecha = FechaData.hoy.Date
                        };
                    FinanzaData.CrearGasto(nuevoGasto);

                    // Restar la indemnización al Presupuesto
                    EquipoData.RestarCantidadAPresupuesto(miEquipo.IdEquipo, bonusPartido);
                }
            }

            // Sumar goles y asistencias
            foreach (var (goleador, asistente) in golesYAsistencias)
            {
                estadisticas[goleador.IdJugador].Goles++;

                if (asistente != null)
                {
                    estadisticas[asistente.IdJugador].Asistencias++;
                }
            }

            // Sumar tarjetas
            foreach (var (jugador, tipoTarjeta) in tarjetas)
            {
                if (tipoTarjeta.Contains("roja")) estadisticas[jugador.IdJugador].TarjetasRojas++;
                if (tipoTarjeta.Contains("amarilla")) estadisticas[jugador.IdJugador].TarjetasAmarillas++;
            }

            // Sumar MVP
            estadisticas[mvp.IdJugador].MVP++;

            // Guardar estadísticas en la base de datos
            foreach (var estadistica in estadisticas.Values)
            {
                EstadisticaJugadorData.ActualizarEstadisticasEuropa(estadistica);
            }
        }

        private void ActualizarJugadoresSancionados(List<(Jugador, string)> tarjetasLocal, List<(Jugador, string)> tarjetasVisitante)
        {
            List<(Jugador, string)> tarjetas = tarjetasLocal.Concat(tarjetasVisitante).ToList();
            foreach (var tarjeta in tarjetas)
            {
                Jugador jugador = tarjeta.Item1;
                string tipoTarjeta = tarjeta.Item2;

                // Comprobar cuantas amarillas tiene y si es multiplo de 2 se le aplica 1 partido de sancion
                Estadistica statJugador = EstadisticaJugadorData.MostrarEstadisticasJugador(jugador.IdJugador);

                if (tipoTarjeta == "amarilla" || tipoTarjeta == "dobleamarilla")
                {
                    if (statJugador.TarjetasAmarillas % 5 == 0)
                    {
                        JugadorData.PonerJugadorSancionado(jugador.IdJugador, 1);

                        // Si es un jugador de mi equipo...
                        if (JugadorData.EsDeMiEquipo(jugador.IdJugador, miEquipo.IdEquipo))
                        {
                            // Crear el mensaje
                            Mensaje mensajeDespido = new Mensaje
                            {
                                Fecha = FechaData.hoy,
                                Remitente = JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto,
                                Asunto = "Jugador Sancionado",
                                Contenido = "Como delegado del " + EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Nombre + " te informo de que " + JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto + " ha sido sancionado con 1 partidos sin poder jugar por acumulación de tarjetas amarillas.",
                                TipoMensaje = "Notificación",
                                IdEquipo = miEquipo.IdEquipo,
                                Leido = false,
                                Icono = jugador.IdJugador
                            };
                            MensajeData.CrearMensaje(mensajeDespido);
                        }
                    }
                }
                else if (tipoTarjeta == "roja")
                {
                    JugadorData.PonerJugadorSancionado(jugador.IdJugador, 2);

                    // Si es un jugador de mi equipo...
                    if (JugadorData.EsDeMiEquipo(jugador.IdJugador, miEquipo.IdEquipo))
                    {
                        // Crear el mensaje
                        Mensaje mensajeDespido = new Mensaje
                        {
                            Fecha = FechaData.hoy,
                            Remitente = JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto,
                            Asunto = "Jugador Sancionado",
                            Contenido = "Como delegado del " + EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Nombre + " te informo de que " + JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto + " ha sido sancionado con 2 partidos sin poder jugar tras haber recibido una tarjeta roja.",
                            TipoMensaje = "Notificación",
                            IdEquipo = miEquipo.IdEquipo,
                            Leido = false,
                            Icono = jugador.IdJugador
                        };
                        MensajeData.CrearMensaje(mensajeDespido);
                    }
                } 
            }
        }

        private void ComprobarJugadoresLesionados(List<Jugador> jugadoresLocal, List<Jugador> jugadoresVisitante)
        {
            List<int> lesionados = SimularLesiones(jugadoresLocal, jugadoresVisitante);

            List<(string Descripcion, int MinSemanas, int MaxSemanas)> lesionesComunes = new List<(string, int, int)>
            {
                ("Contusión leve", 1, 1),
                ("Distensión muscular leve", 2, 3),
                ("Contractura lumbar", 2, 4),
                ("Esguince de tobillo grado 1", 2, 4),
                ("Tendinitis rotuliana", 3, 5),
                ("Rotura fibrilar leve", 3, 5),
                ("Esguince de rodilla grado 2", 5, 7),
                ("Fractura de dedo del pie", 6, 8),
                ("Rotura fibrilar moderada", 6, 9),
                ("Desgarro isquiotibial", 7, 10)
            };

            List<(string Descripcion, int MinSemanas, int MaxSemanas)> lesionesGraves = new List<(string, int, int)>
            {
                ("Rotura completa del ligamento cruzado anterior", 20, 28),
                ("Fractura de tibia", 25, 32),
                ("Doble rotura ligamentosa con cirugía", 30, 40)
            };

            foreach (int jugador in lesionados)
            {
                bool esLesionGrave = random.NextDouble() < 0.1; // 10% de probabilidad de lesión grave

                (string Descripcion, int MinSemanas, int MaxSemanas) lesion;

                if (esLesionGrave)
                {
                    lesion = lesionesGraves[random.Next(lesionesGraves.Count)];
                }
                else
                {
                    lesion = lesionesComunes[random.Next(lesionesComunes.Count)];
                }

                int semanasLesion = random.Next(lesion.MinSemanas, lesion.MaxSemanas + 1);

                JugadorData.PonerJugadorLesionado(jugador, semanasLesion, lesion.Descripcion);

                if (JugadorData.EsDeMiEquipo(jugador, miEquipo.IdEquipo))
                {
                    // Crear Mensaje
                    Mensaje mensajeLesion = new Mensaje
                    {
                        Fecha = FechaData.hoy,
                        Remitente = JugadorData.MostrarDatosJugador(jugador).NombreCompleto,
                        Asunto = "Jugador Lesionado",
                        Contenido = $"Desde el equipo médico del {EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Nombre} te informamos de que {JugadorData.MostrarDatosJugador(jugador).NombreCompleto} se ha lesionado ({lesion.Descripcion}), y permanecerá de baja durante {semanasLesion} semanas.",
                        TipoMensaje = "Notificación",
                        IdEquipo = miEquipo.IdEquipo,
                        Leido = false,
                        Icono = jugador
                    };
                    MensajeData.CrearMensaje(mensajeLesion);
                }
            }
        }

        public List<int> SimularLesiones(List<Jugador> jugadoresLocal, List<Jugador> jugadoresVisitante)
        {
            List<int> lesionados = new List<int>();
            int maxLesionesPorPartido = random.Next(0, 2); // 0 o 1 lesiones

            List<Jugador> todos = jugadoresLocal.Concat(jugadoresVisitante).OrderBy(j => random.Next()).ToList();

            foreach (var jugador in todos)
            {
                if (maxLesionesPorPartido == 0)
                    break;

                if (random.Next(0, 100) < 5) // 5% probabilidad
                {
                    lesionados.Add(jugador.IdJugador);
                    maxLesionesPorPartido--;
                }
            }

            return lesionados;
        }

        public int ObtenerPuestoEquipo(int equipoId, int competicion)
        {
            List<Clasificacion> clasificaciones = ClasificacionData.MostrarClasificacion(competicion);

            // Buscar la posición del equipo en la lista
            var equipo = clasificaciones.FirstOrDefault(c => c.IdEquipo == equipoId);

            return equipo != null ? equipo.Posicion : -1; // Retorna -1 si no encuentra el equipo
        }

        private void ActualizarConfianzaManager(Partido partido, int golesLocal, int golesVisitante)
        {
            int myTeam, rival, golesEquipo, golesRival;

            // Comprobamos si mi equipo es el Local o el Visitante
            if (partido.IdEquipoLocal == miEquipo.IdEquipo)
            {
                myTeam = partido.IdEquipoLocal;
                rival = partido.IdEquipoVisitante;
                golesEquipo = golesLocal;
                golesRival = golesVisitante;
            }
            else
            {
                rival = partido.IdEquipoLocal;
                myTeam = partido.IdEquipoVisitante;
                golesRival = golesLocal;
                golesEquipo = golesVisitante;
            }

            // Actualizamos las confianzas
            int rankingmyTeam = ObtenerPuestoEquipo(myTeam, 1);
            int rankingRival = ObtenerPuestoEquipo(rival, 1);
            int rivalidad = EquipoData.ObtenerDetallesEquipo(myTeam).Rival;
            int diferenciaGoles = Math.Abs(golesEquipo - golesRival);

            // Base de confianza (valores iniciales)
            int cambioDirectiva = 0;
            int cambioFans = 0;
            int cambioJugadores = 0;

            bool esRivalHistorico = rival == rivalidad;
            bool victoria = golesEquipo > golesRival;
            bool derrota = golesEquipo < golesRival;
            bool empate = golesEquipo == golesRival;
            bool goleada = diferenciaGoles >= 3;

            if (victoria)
            {
                // Directiva: evalúa rendimiento según fuerza del rival
                cambioDirectiva = rankingmyTeam > rankingRival ? 4 : 2;
                // Fans: celebran victoria, más si es contra rival histórico
                cambioFans = 4;
                if (esRivalHistorico) cambioFans += 6;
                // Jugadores: sube moral por victoria
                cambioJugadores = 3;

                if (goleada)
                {
                    cambioDirectiva += 3;
                    cambioFans += 5;
                    cambioJugadores += 3;
                }

                if (esRivalHistorico)
                {
                    cambioDirectiva += 3;
                    cambioJugadores += 2;
                }

                ManagerData.ActualizarResultadoManager(miManager.IdManager, 1, 1, 0, 0, 3);
                if (partido.IdCompeticion == 1 || partido.IdCompeticion == 2)
                {
                    ManagerData.ActualizarManagerTemporal(new Historial
                    {
                        PartidosJugados = 1,
                        PartidosGanados = 1,
                        PartidosEmpatados = 0,
                        PartidosPerdidos = 0,
                        GolesMarcados = golesEquipo,
                        GolesRecibidos = golesRival
                    });
                }     
            }
            else if (derrota)
            {
                cambioDirectiva = rankingmyTeam < rankingRival ? -2 : -4;
                cambioFans = -4;
                cambioJugadores = -3;

                if (goleada)
                {
                    cambioFans -= 2;
                    cambioJugadores -= 1;
                }

                if (esRivalHistorico)
                {
                    cambioFans -= 6;
                    cambioDirectiva -= 2;
                    cambioJugadores -= 2;
                }

                ManagerData.ActualizarResultadoManager(miManager.IdManager, 1, 0, 0, 1, 0);
                if (partido.IdCompeticion == 1 || partido.IdCompeticion == 2)
                {
                    ManagerData.ActualizarManagerTemporal(new Historial
                    {
                        PartidosJugados = 1,
                        PartidosGanados = 0,
                        PartidosEmpatados = 0,
                        PartidosPerdidos = 1,
                        GolesMarcados = golesEquipo,
                        GolesRecibidos = golesRival
                    });
                }      
            }
            else if (empate)
            {
                cambioDirectiva = rankingmyTeam > rankingRival ? 1 : -1;
                cambioFans = rankingmyTeam > rankingRival ? 2 : -2;
                cambioJugadores = rankingmyTeam > rankingRival ? 1 : -2;

                if (esRivalHistorico)
                {
                    cambioFans += 1;
                    cambioJugadores += 1;
                }

                ManagerData.ActualizarResultadoManager(miManager.IdManager, 1, 0, 1, 0, 1);
                if (partido.IdCompeticion == 1 || partido.IdCompeticion == 2)
                {
                    ManagerData.ActualizarManagerTemporal(new Historial
                    {
                        PartidosJugados = 1,
                        PartidosGanados = 0,
                        PartidosEmpatados = 1,
                        PartidosPerdidos = 0,
                        GolesMarcados = golesEquipo,
                        GolesRecibidos = golesRival
                    });
                } 
            }

            // Limitar el rango de impacto según tipo de confianza
            cambioDirectiva = Math.Clamp(cambioDirectiva, -5, 5);
            cambioFans = Math.Clamp(cambioFans, -8, 8);
            cambioJugadores = Math.Clamp(cambioJugadores, -6, 6);

            ManagerData.ActualizarConfianza(cambioDirectiva, cambioFans, cambioJugadores);
        }

        public void ActualizacionAtributos(List<Jugador> jugadoresLocal, List<Jugador> jugadoresVisitante, int idEquipolocal, int idEquipoVisitante,
                                           int golesLocal, int golesVisitante, List<(Jugador, Jugador?)> golesYAsistencias, Jugador mvp)
        {
            
        }

        private void MostrarCompeticionRonda(Partido partido)
        {
            int comp = partido.IdCompeticion; // IdCompeticion del partido
            string nombreCompeticion = CompeticionData.MostrarNombreCompeticion(comp);
            int miCompeticion = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).IdCompeticion;
            int miCompeticionEuropea = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).CompeticionEuropea;

            if (comp == 4)
            {
                int ronda = partido.Ronda ?? 0; // Ronda de Copa

                string nombreRonda = PartidoData.ObtenerNombreRonda(ronda);
                lblTituloJornada.text = $"{nombreCompeticion.ToUpper()} ({nombreRonda})";
            }

            if (comp == 5)
            {
                if (partido.Ronda == 0)
                {
                    int jornada = partido.Jornada ?? 0; // Jornada de Copa de Europa 1
                    lblTituloJornada.text = $"{nombreCompeticion.ToUpper()} (JORNADA {jornada})";
                } 
                else
                {
                    int ronda = partido.Ronda ?? 0; // Ronda de Copa Europa 1

                    string nombreRonda = PartidoData.ObtenerNombreRonda(ronda);
                    lblTituloJornada.text = $"{nombreCompeticion.ToUpper()} ({nombreRonda})";
                }
            }

            if (comp == 6)
            {
                if (partido.Ronda == 0)
                {
                    int jornada = partido.Jornada ?? 0; // Jornada de Copa de Europa 2
                    lblTituloJornada.text = $"{nombreCompeticion.ToUpper()} (JORNADA {jornada})";
                } 
                else
                {
                    int ronda = partido.Ronda ?? 0; // Ronda de Copa Europa 2

                    string nombreRonda = PartidoData.ObtenerNombreRonda(ronda);
                    lblTituloJornada.text = $"{nombreCompeticion.ToUpper()} ({nombreRonda})";
                }
            }
            else if (comp >= 1 && comp <= 2)
            {
                int jornada = partido.Jornada ?? 0; // Jornada de Liga
                lblTituloJornada.text = $"{nombreCompeticion.ToUpper()} (JORNADA {jornada})";
            }
        }

        // -------------------------------------- MÉTODO SIMULAR PARTIDO Y GUARDAR (SIN PINTAR)
        private DatosSimulacion SimularPartidoYGuardar(Partido partido, bool esMiEquipo)
        {
            List<Jugador> jugadoresLocal = JugadorData.JugadoresJueganPartido(partido.IdEquipoLocal);
            List<Jugador> jugadoresVisitante = JugadorData.JugadoresJueganPartido(partido.IdEquipoVisitante);

            // Penalizaciones si es mi equipo
            bool soyLocal = esMiEquipo && miEquipo != null && partido.IdEquipoLocal == miEquipo.IdEquipo;
            
            bool penalizarAtaqueLocal = soyLocal && !JugadorData.TengoDelanteros(partido.IdEquipoLocal);
            bool penalizarDefensaLocal = soyLocal && (!JugadorData.TengoPortero(partido.IdEquipoLocal) || !JugadorData.TengoDefensas(partido.IdEquipoLocal));

            bool penalizarAtaqueVisitante = !soyLocal && esMiEquipo && !JugadorData.TengoDelanteros(partido.IdEquipoVisitante);
            bool penalizarDefensaVisitante = !soyLocal && esMiEquipo && (!JugadorData.TengoPortero(partido.IdEquipoVisitante) || !JugadorData.TengoDefensas(partido.IdEquipoVisitante));

            // Simular goles
            int golesLocal = CalcularGoles(jugadoresLocal, jugadoresVisitante, penalizarAtaqueLocal, penalizarDefensaVisitante);
            int golesVisitante = CalcularGoles(jugadoresVisitante, jugadoresLocal, penalizarAtaqueVisitante, penalizarDefensaLocal);

            partido.GolesLocal = golesLocal;
            partido.GolesVisitante = golesVisitante;

            // Asignar goleadores y asistentes
            List<(Jugador, Jugador?)> goleadoresLocal = AsignarGolesYAsistencias(golesLocal, jugadoresLocal, random);
            List<(Jugador, Jugador?)> goleadoresVisitante = AsignarGolesYAsistencias(golesVisitante, jugadoresVisitante, random);

// Asignar tarjetas
            var (tarjetasLocal, tarjetasVisitante) = AsignarTarjetas(jugadoresLocal, jugadoresVisitante, random);

            // Calcular asistencia
            int asistencia = EquipoData.CalcularAsistencia(partido.IdEquipoLocal);
            partido.Asistencia = asistencia;

            // Calcular recaudación
            int recauda = 0;
            if (partido.IdEquipoLocal == miEquipo?.IdEquipo)
            {
                Taquilla taq = TaquillaData.RecuperarPreciosTaquilla(miEquipo.IdEquipo);
                double? rec = taq.PrecioEntradaGeneral * (asistencia * 0.50) + 
                           taq.PrecioEntradaTribuna * (asistencia * 0.40) + 
                           taq.PrecioEntradaVip * (asistencia * 0.10);
                recauda = (int)Math.Round(rec ?? 0);
            }

            // Crear datos de simulación
            DatosSimulacion datos = new DatosSimulacion
            {
                GolesLocal = golesLocal,
                GolesVisitante = golesVisitante,
                goleadoresLocal = goleadoresLocal,
                goleadoresVisitante = goleadoresVisitante,
                tarjetasLocal = tarjetasLocal,
                tarjetasVisitante = tarjetasVisitante,
                Asistencia = asistencia,
                Recaudacion = recauda,
                JugadoresLocal = jugadoresLocal,
                JugadoresVisitante = jugadoresVisitante
            };

            // Determinar MVP
            var todosGoles = goleadoresLocal.Concat(goleadoresVisitante).ToList();
            datos.MVP = DeterminarMVP(todosGoles, jugadoresLocal, jugadoresVisitante);
            datos.GolesMVP = todosGoles.Count(ga => ga.Item1.IdJugador == datos.MVP.IdJugador);
            datos.AsistenciasMVP = todosGoles.Count(ga => ga.Item2 != null && ga.Item2.IdJugador == datos.MVP.IdJugador);

// --------------------- GUARDAR EN BASE DE DATOS SEGÚN COMPETICIÓN
            // Amistosos: solo resultado
            if (partido.IdCompeticion == 10)
            {
                PartidoData.ActualizarPartido(partido);
            }
            // Liga (competiciones 1 y 2)
            else if (partido.IdCompeticion >= 1 && partido.IdCompeticion <= 2)
            {
                PartidoData.ActualizarPartido(partido);
                GuardarClasificacion(partido, goleadoresLocal, goleadoresVisitante);
                GuardarEstadisticas(jugadoresLocal, jugadoresVisitante, goleadoresLocal, goleadoresVisitante, tarjetasLocal, tarjetasVisitante, datos.MVP, esMiEquipo);
            }
            // Copa Nacional (4)
            else if (partido.IdCompeticion == 4)
            {
                PartidoData.ActualizarPartidoCopaNacional(partido);
                GuardarClasificacionCopaEuropa(partido, goleadoresLocal, goleadoresVisitante);
            }
            // Copa Europa 1 (5)
            else if (partido.IdCompeticion == 5)
            {
                PartidoData.ActualizarPartidoCopaEuropa1(partido);
                GuardarClasificacionCopaEuropa(partido, goleadoresLocal, goleadoresVisitante);
                GuardarEstadisticasEuropa(jugadoresLocal, jugadoresVisitante, goleadoresLocal, goleadoresVisitante, tarjetasLocal, tarjetasVisitante, datos.MVP, esMiEquipo);
            }
            // Copa Europa 2 (6)
            else if (partido.IdCompeticion == 6)
            {
                PartidoData.ActualizarPartidoCopaEuropa2(partido);
                GuardarClasificacionCopaEuropa(partido, goleadoresLocal, goleadoresVisitante);
                GuardarEstadisticasEuropa(jugadoresLocal, jugadoresVisitante, goleadoresLocal, goleadoresVisitante, tarjetasLocal, tarjetasVisitante, datos.MVP, esMiEquipo);
            }

            return datos;
        }

        // -------------------------------------- GUARDAR CLASIFICACIÓN LIGA
        private void GuardarClasificacion(Partido partido, List<(Jugador, Jugador?)> goleLocal, List<(Jugador, Jugador?)> goleVisitante)
        {
            int gf = goleLocal.Sum(g => g.Item1 != null ? 1 : 0);
            int gc = goleVisitante.Sum(g => g.Item1 != null ? 1 : 0);
            
            Clasificacion cla_local = CrearClasificacion(partido.IdEquipoLocal, gf, gc);
            Clasificacion cla_visitante = CrearClasificacion(partido.IdEquipoVisitante, gc, gf);

            if (partido.IdCompeticion == 1)
            {
                ClasificacionData.ActualizarClasificacion(cla_local);
                ClasificacionData.ActualizarClasificacion(cla_visitante);
            }
            else
            {
                ClasificacionData.ActualizarClasificacion2(cla_local);
                ClasificacionData.ActualizarClasificacion2(cla_visitante);
            }
        }

        // -------------------------------------- GUARDAR CLASIFICACIÓN COPA EUROPA
        private void GuardarClasificacionCopaEuropa(Partido partido, List<(Jugador, Jugador?)> goleLocal, List<(Jugador, Jugador?)> goleVisitante)
        {
            int gf = goleLocal.Sum(g => g.Item1 != null ? 1 : 0);
            int gc = goleVisitante.Sum(g => g.Item1 != null ? 1 : 0);
            
            Clasificacion cla_local = CrearClasificacion(partido.IdEquipoLocal, gf, gc);
            Clasificacion cla_visitante = CrearClasificacion(partido.IdEquipoVisitante, gc, gf);

            if (partido.IdCompeticion == 5)
            {
                ClasificacionData.ActualizarClasificacionEuropa1(cla_local);
                ClasificacionData.ActualizarClasificacionEuropa1(cla_visitante);
            }
            else if (partido.IdCompeticion == 6)
            {
                ClasificacionData.ActualizarClasificacionEuropa2(cla_local);
                ClasificacionData.ActualizarClasificacionEuropa2(cla_visitante);
            }
        }

        // -------------------------------------- CREAR OBJETO CLASIFICACIÓN
        private Clasificacion CrearClasificacion(int idEquipo, int gf, int gc)
        {
            int resultado = gf.CompareTo(gc);
            
            return new Clasificacion
            {
                IdEquipo = idEquipo,
                Jugados = 1,
                Ganados = resultado > 0 ? 1 : 0,
                Empatados = resultado == 0 ? 1 : 0,
                Perdidos = resultado < 0 ? 1 : 0,
                Puntos = resultado > 0 ? 3 : (resultado == 0 ? 1 : 0),
                LocalVictorias = resultado > 0 ? 1 : 0,
                LocalDerrotas = resultado < 0 ? 1 : 0,
                VisitanteVictorias = 0,
                VisitanteDerrotas = 0,
                GolesFavor = gf,
                GolesContra = gc,
                Racha = resultado
            };
        }

        // -------------------------------------- GUARDAR ESTADÍSTICAS JUGADORES LIGA
        private void GuardarEstadisticas(List<Jugador> jugLocal, List<Jugador> jugVisit,
                               List<(Jugador, Jugador?)> golLocal, List<(Jugador, Jugador?)> golVisit,
                               List<(Jugador, string)> tarjLocal, List<(Jugador, string)> tarjVisit,
                               Jugador mvp, bool esMiEquipo)
        {
            var tarjetas = tarjLocal.Concat(tarjVisit).ToList();
            var todosGoles = golLocal.Concat(golVisit).ToList();
            ActualizarEstadisticasPartido(jugLocal, jugVisit, todosGoles, tarjetas, mvp);
            ActualizarJugadoresSancionados(tarjLocal, tarjVisit);
        }

        // -------------------------------------- GUARDAR ESTADÍSTICAS JUGADORES EUROPA
        private void GuardarEstadisticasEuropa(List<Jugador> jugLocal, List<Jugador> jugVisit,
                                          List<(Jugador, Jugador?)> golLocal, List<(Jugador, Jugador?)> golVisit,
                                          List<(Jugador, string)> tarjLocal, List<(Jugador, string)> tarjVisit,
                                          Jugador mvp, bool esMiEquipo)
        {
            var tarjetas = tarjLocal.Concat(tarjVisit).ToList();
            var todosGoles = golLocal.Concat(golVisit).ToList();
            ActualizarEstadisticasPartidoEuropa(jugLocal, jugVisit, todosGoles, tarjetas, mvp);
            ActualizarJugadoresSancionados(tarjLocal, tarjVisit);
        }

        // -------------------------------------- PINTAR RESUMEN DE MI PARTIDO
        private void PintarResumenMiPartido(Partido partido, DatosSimulacion datos)
        {
            MostrarCompeticionRonda(partido);

            // Nombre de los equipos
            lblNombreLocal.text = EquipoData.ObtenerDetallesEquipo(partido.IdEquipoLocal).Nombre;
            lblNombreVisitante.text = EquipoData.ObtenerDetallesEquipo(partido.IdEquipoVisitante).Nombre;

            // Escudos
            var escudoLocal = Resources.Load<Sprite>($"EscudosEquipos/{partido.IdEquipoLocal}");
            if (escudoLocal != null)
                imgEscudoLocal.style.backgroundImage = new StyleBackground(escudoLocal);

            var escudoVisitante = Resources.Load<Sprite>($"EscudosEquipos/{partido.IdEquipoVisitante}");
            if (escudoVisitante != null)
                imgEscudoVisitante.style.backgroundImage = new StyleBackground(escudoVisitante);

            // Marcador
            lblGolesLocal.text = datos.GolesLocal.ToString();
            lblGolesVisitante.text = datos.GolesVisitante.ToString();

            // Goleadores y tarjetas
            MostrarGoleadoresYAsistentes(goleadoresLocalContainer, datos.goleadoresLocal);
            MostrarGoleadoresYAsistentes(goleadoresVisitanteContainer, datos.goleadoresVisitante);
            MostrarTarjetas(tarjetasLocalContainer, datos.tarjetasLocal);
            MostrarTarjetas(tarjetasVisitanteContainer, datos.tarjetasVisitante);

            // MVP
            lblMvpDemarcacion.text = $"{JugadorData.MostrarDatosJugador(datos.MVP.IdJugador).Rol}";
            lblMvpNombre.text = $"{JugadorData.MostrarDatosJugador(datos.MVP.IdJugador).NombreCompleto}";
            lblMvpEstadisticas.text = $"({datos.GolesMVP} {(datos.GolesMVP == 1 ? "gol" : "goles")} / " +
                                       $"{datos.AsistenciasMVP} {(datos.AsistenciasMVP == 1 ? "asistencia" : "asistencias")})";
            var imagenMVP = Resources.Load<Sprite>($"{datos.MVP.RutaImagen}");
            if (imagenMVP != null)
                fotoMvp.style.backgroundImage = new StyleBackground(imagenMVP);

            // Asistencia y recaudación
            lblAsistenciaPartido.text = $"{EquipoData.ObtenerDetallesEquipo(partido.IdEquipoLocal).Estadio} " +
                                       $" {datos.Asistencia.ToString("N0")} espectadores " +
                                       $" {datos.Recaudacion.ToString("N0")} €";
        }
    }
}