using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

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
        private VisualElement mainContainer;
        private Button btnSeguir;
        private Label miEquipoNombre, managerNombre, fecha1, fecha2;
        public Label miPresupuesto;
        private Manager miManager;
        private Equipo miEquipo;

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