using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class FichaJugador
    {
        private AudioClip clickSFX;
        private MainScreen mainScreen;
        private Equipo miEquipo;
        private Manager miManager;
        private int elJugador;
        private int procedencia;
        int opcionBotones = 1;
        int respuestaEquipo = 0;

        private VisualElement root, popupContainer, popupContainerTransferible, popupContainerNoTransferible, popupContainerOjear, popupContainerRenovar,
                              fotoJugador, imagenPosicion, escudo, moral, estadoAnimo, situacionMercado, lesionado, rol;
        private Label dorsal, nombreJugador, mediaTotal, nombreEquipo, posicion, altura, peso, edad, nacionalidad, valorMercado,
                      condicionFisica, salario, aniosRestantes, clausula, bonusPartidos, bonusGol,
                      velocidad, resistencia, agresividad, calidad, potencial, portero, pase, regate, remate, entradas, tiro, popupText,
                      popupTextTransferible, popupTextNoTransferible, popupTextOjear, popupTextRenovar;
        private Button btnVolver, btnDespedir, btnRenovar, btnPonerEnMercado, btnQuitarMercado, btnContratar, btnOjear, btnYes, btnCancel,
                       btnTransferible, btnCedible, btnSalir, btnNoTransferible, btnSalirTransferible, btnAceptar, btnYesRenovar, btnCancelRenovar;

        public FichaJugador(VisualElement rootInstance, Equipo equipo, Manager manager, int jugador, int proc, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            elJugador = jugador;
            procedencia = proc;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            fotoJugador = root.Q<VisualElement>("foto-jugador");
            imagenPosicion = root.Q<VisualElement>("imagen-campo");
            escudo = root.Q<VisualElement>("escudo-equipo");
            moral = root.Q<VisualElement>("moral-img");
            estadoAnimo = root.Q<VisualElement>("animo-img");
            situacionMercado = root.Q<VisualElement>("situacionMercado-img");
            lesionado = root.Q<VisualElement>("lesionado-img");
            rol = root.Q<VisualElement>("rol-img");
            dorsal = root.Q<Label>("dorsal-jugador");
            nombreJugador = root.Q<Label>("nombre-jugador");
            mediaTotal = root.Q<Label>("media-jugador");
            nombreEquipo = root.Q<Label>("nombre-equipo");
            posicion = root.Q<Label>("posicion-jugador");
            altura = root.Q<Label>("altura");
            peso = root.Q<Label>("peso");
            edad = root.Q<Label>("edad");
            nacionalidad = root.Q<Label>("nacionalidad");
            valorMercado = root.Q<Label>("valor-mercado");
            condicionFisica = root.Q<Label>("condicion-fisica");
            salario = root.Q<Label>("salario");
            aniosRestantes = root.Q<Label>("anios");
            clausula = root.Q<Label>("clausula");
            bonusPartidos = root.Q<Label>("bonus-partidos");
            bonusGol = root.Q<Label>("bonus-gol");
            velocidad = root.Q<Label>("velocidad");
            resistencia = root.Q<Label>("resistencia");
            agresividad = root.Q<Label>("agresividad");
            calidad = root.Q<Label>("calidad");
            potencial = root.Q<Label>("potencial");
            portero = root.Q<Label>("portero");
            pase = root.Q<Label>("pase");
            regate = root.Q<Label>("regate");
            remate = root.Q<Label>("remate");
            entradas = root.Q<Label>("entradas");
            tiro = root.Q<Label>("tiro");
            btnVolver = root.Q<Button>("btnVolver");
            btnDespedir = root.Q<Button>("btnDespedir");
            btnRenovar = root.Q<Button>("btnRenovar");
            btnPonerEnMercado = root.Q<Button>("btnPonerEnMercado");
            btnQuitarMercado = root.Q<Button>("btnQuitarEnMercado");
            btnContratar = root.Q<Button>("btnContratar");
            btnOjear = root.Q<Button>("btnOjear");

            popupContainer = root.Q<VisualElement>("popup-container");
            btnYes = root.Q<Button>("btnYes");
            btnCancel = root.Q<Button>("btnCancel");
            popupText = root.Q<Label>("popup-text");

            popupContainerTransferible = root.Q<VisualElement>("popup-container-transferible");
            btnTransferible = root.Q<Button>("btnTransferible");
            btnCedible = root.Q<Button>("btnCedible");
            btnSalir = root.Q<Button>("btnSalir");
            popupTextTransferible = root.Q<Label>("popup-text-transferible");

            popupContainerNoTransferible = root.Q<VisualElement>("popup-container-noTransferible");
            btnNoTransferible = root.Q<Button>("btnNoTransferible");
            btnSalirTransferible = root.Q<Button>("btnSalirTransferible");
            popupTextNoTransferible = root.Q<Label>("popup-text-noTransferible");

            popupContainerOjear = root.Q<VisualElement>("popup-container-ojear");
            btnAceptar = root.Q<Button>("btnAceptar");
            popupTextOjear = root.Q<Label>("popup-text-ojear");

            popupContainerRenovar = root.Q<VisualElement>("popup-container-renovar");
            btnYesRenovar = root.Q<Button>("btnYesRenovar");
            btnCancelRenovar = root.Q<Button>("btnCancelRenovar");
            popupTextRenovar = root.Q<Label>("popup-text-renovar");

            Jugador jugadorSeleccionado = JugadorData.MostrarDatosJugador(elJugador);

            // Lista con los label de Mercado.
            List<Label> clubList = new List<Label> { mainScreen.lblInformacion, mainScreen.lblPlantilla, mainScreen.lblEmpleados, mainScreen.lblLesionados, mainScreen.lblClasificacion, mainScreen.lblResultados,
                                                     mainScreen.lblEstadisticas, mainScreen.lblPalmaresJugadores, mainScreen.lblPalmaresEquipos, mainScreen.lblEstadioInformacion, mainScreen.lblEntradas,
                                                     mainScreen.lblAmpliaciones, mainScreen.lblAlineacion, mainScreen.lblEntrenamiento, mainScreen.lblRival, mainScreen.lblIngresos, mainScreen.lblGastos,
                                                     mainScreen.lblPatrocinadores, mainScreen.lblTelevision, mainScreen.lblPrestamos, mainScreen.lblMercado, mainScreen.lblBusqueda, mainScreen.lblCartera,
                                                     mainScreen.lblEstadoOfertas, mainScreen.lblListaTraspasos };

            List<VisualElement> menuList = new List<VisualElement> { mainScreen.clubMenu, mainScreen.alineacionMenu, mainScreen.competicionMenu,
                                                                     mainScreen.calendarioMenu, mainScreen.fichajesMenu, mainScreen.finanzasMenu,
                                                                     mainScreen.estadioMenu, mainScreen.managerMenu, mainScreen.mensajesMenu };

            // Cargar sección Datos del Jugador
            CargarDatosJugador(jugadorSeleccionado);
            CargarOtrosDatosJugador(jugadorSeleccionado);
            CargarBotones(jugadorSeleccionado);
            ComprobarEquipoOfertaAceptadaTextoBotones(jugadorSeleccionado);

            if (procedencia == 0)
                btnVolver.text = "CERRAR";

            btnVolver.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                switch (procedencia)
                {
                    case 1:
                        UIManager.Instance.CargarPantalla("UI/Club/Plantilla/ClubPlantilla", instancia =>
                        {
                            new ClubPlantilla(instancia, miEquipo, miManager, mainScreen);
                        });
                        mainScreen.MenuVisibility(menuList, mainScreen.clubMenu);
                        mainScreen.CambiarColorTextoClub(clubList, mainScreen.lblPlantilla);
                        break;
                    case 2:
                        UIManager.Instance.CargarPantalla("UI/Fichajes/Mercado/FichajesMercado", instancia =>
                        {
                            new FichajesMercado(instancia, miEquipo, miManager, mainScreen);
                        });
                        mainScreen.MenuVisibility(menuList, mainScreen.fichajesMenu);
                        mainScreen.CambiarColorTextoClub(clubList, mainScreen.lblMercado);
                        break;
                    case 3:
                        UIManager.Instance.CargarPantalla("UI/Fichajes/BuscarPorEquipo/FichajesBuscarPorEquipo", instancia =>
                        {
                            new FichajesBuscarPorEquipo(instancia, miEquipo, miManager, mainScreen, jugadorSeleccionado.IdEquipo);
                        });
                        mainScreen.MenuVisibility(menuList, mainScreen.fichajesMenu);
                        mainScreen.CambiarColorTextoClub(clubList, mainScreen.lblBusqueda);
                        break;
                    case 4:
                        UIManager.Instance.CargarPantalla("UI/Fichajes/Cartera/FichajesCartera", instancia =>
                        {
                            new FichajesCartera(instancia, miEquipo, miManager, mainScreen);
                        });
                        mainScreen.MenuVisibility(menuList, mainScreen.fichajesMenu);
                        mainScreen.CambiarColorTextoClub(clubList, mainScreen.lblCartera);
                        break;
                    case 5:
                        UIManager.Instance.CargarPantalla("UI/Fichajes/EstadoOfertas/FichajesEstadoOfertas", instancia =>
                        {
                            new FichajesEstadoOfertas(instancia, miEquipo, miManager, mainScreen);
                        });
                        mainScreen.MenuVisibility(menuList, mainScreen.fichajesMenu);
                        mainScreen.CambiarColorTextoClub(clubList, mainScreen.lblEstadoOfertas);
                        break;
                    case 0:
                        UIManager.Instance.CargarPantalla("UI/PortadaScreen/Portada", instancia =>
                        {
                            new PortadaManager(instancia, miEquipo, miManager);
                        });
                        mainScreen.MenuVisibility(menuList, null);
                        break;
                    case -1:
                        UIManager.Instance.CargarPantalla("UI/Fichajes/EstadoOfertas/FichajesEstadoOfertas", instancia =>
                        {
                            new FichajesEstadoOfertas(instancia, miEquipo, miManager, mainScreen);
                        });
                        mainScreen.MenuVisibility(menuList, mainScreen.fichajesMenu);
                        mainScreen.CambiarColorTextoClub(clubList, mainScreen.lblEstadoOfertas);
                        break;
                }
            };

            btnDespedir.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                int indemizacion = (jugadorSeleccionado.SalarioTemporada ?? 0) * (jugadorSeleccionado.AniosContrato ?? 0);
                if (jugadorSeleccionado != null)
                {
                    popupContainer.style.display = DisplayStyle.Flex;
                    popupText.text = "¿Estás seguro de despedir a " + jugadorSeleccionado.NombreCompleto.ToUpper() +
                                     "? Al despedirle antes de terminar su contrato deberás indemnizarle con " +
                                     Constants.CambioDivisa(indemizacion).ToString("N0") + " " + CargarSimboloMoneda() + ".";

                    // Importante: limpiar listeners previos para evitar duplicados
                    btnYes.clicked -= OnYesClick;
                    btnCancel.clicked -= OnCancelClick;

                    void OnYesClick()
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);

                        // Eliminar el id_equipo de la tabla jugadores.
                        JugadorData.DespedirJugador(jugadorSeleccionado.IdJugador);

                        // Eliminar el contrato del jugador.
                        JugadorData.EliminarContratoJugador(jugadorSeleccionado.IdJugador);

                        // Restar indemnización del Presupuesto
                        EquipoData.RestarCantidadAPresupuesto(miEquipo.IdEquipo, indemizacion);

                        // Crear la entrada de Gasto en la tabla "finanzas" de la BD
                        Finanza nuevoGasto = new Finanza
                        {
                            IdEquipo = miEquipo.IdEquipo,
                            Temporada = FechaData.temporadaActual.ToString(),
                            IdConcepto = 12,
                            Tipo = 2,
                            Cantidad = indemizacion,
                            Fecha = FechaData.hoy.Date
                        };
                        FinanzaData.CrearGasto(nuevoGasto);

                        // Crear el mensaje con el Despido de Jugador
                        string nombreJugador = jugadorSeleccionado.Nombre + " " + jugadorSeleccionado.Apellido;
                        string nombreManager = ManagerData.MostrarManager().Nombre + " " +
                                               ManagerData.MostrarManager().Apellido;
                        Mensaje mensajeDespido = new Mensaje
                        {
                            Fecha = FechaData.hoy,
                            Remitente = nombreJugador != null ? nombreJugador : "Desconocido",
                            Asunto = "Despido de jugador",
                            Contenido = "Estimado/a " + nombreManager.ToUpper() + "\n\nViendo mi salida del equipo solo puedo decirte que no " +
                                        "son maneras de hacer las cosas. Aunque tuviera pocos minutos, mi trabajo en cada entreno no ha bajado " +
                                        "nada y he intentado ganarme mi lugar como los otros compañeros. No me merecía esta salida y creo que " +
                                        "lo sabes y comprendes mi enfado.\n\nEspero que le vaya bien al equipo, porque el club, la afición y " +
                                        "los demás compañeros se lo merecen. De ti ya me guardo la opinión...",
                            TipoMensaje = "Respuesta del jugador",
                            IdEquipo = miEquipo.IdEquipo,
                            Leido = false,
                            Icono = jugadorSeleccionado.IdJugador // Distinto de 0 es icono de jugador
                        };
                        MensajeData.CrearMensaje(mensajeDespido);

                        // Actualizar Presupuesto en MainScreen
                        float presupuestoConversion = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presupuesto * Constants.EURO_VALUE;
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

                        mainScreen.miPresupuesto.text = $"{presupuestoConversion.ToString("N0")} {symbol}";

                        btnYes.clicked -= OnYesClick;
                        btnCancel.clicked -= OnCancelClick;
                        popupContainer.style.display = DisplayStyle.None;

                        UIManager.Instance.CargarPantalla("UI/Club/Plantilla/ClubPlantilla", instancia =>
                        {
                            new ClubPlantilla(instancia, miEquipo, miManager, mainScreen);
                        });
                    }

                    void OnCancelClick()
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);
                        popupContainer.style.display = DisplayStyle.None;
                        btnYes.clicked -= OnYesClick;
                        btnCancel.clicked -= OnCancelClick;
                    }

                    btnYes.clicked += OnYesClick;
                    btnCancel.clicked += OnCancelClick;
                }
            };

            btnRenovar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                btnYesRenovar.clicked -= OnAceptar;
                btnCancelRenovar.clicked -= OnCancelar;

                Jugador _jugador = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador);
                int? aniosContrato = _jugador.AniosContrato;
                DateTime? proximaNegociacion = jugadorSeleccionado.ProximaNegociacion;
                string mensajeProximaNegociacion = proximaNegociacion.HasValue
                    ? proximaNegociacion.Value.ToString("dd/MM/yyyy")
                    : "(No hay una fecha disponible)";

                if (proximaNegociacion == null)
                {
                    if (_jugador.NombreCompleto != null)
                    {
                        if (aniosContrato <= 3)
                        {
                            popupContainerRenovar.style.display = DisplayStyle.Flex;
                            popupTextRenovar.text = "¿Estás seguro de renovar a " + _jugador.NombreCompleto.ToUpper() + "?";
                        }
                        else
                        {
                            popupContainerRenovar.style.display = DisplayStyle.Flex;
                            btnYesRenovar.style.display = DisplayStyle.None;
                            btnCancelRenovar.text = "CERRAR";
                            btnCancelRenovar.style.backgroundColor = new Color(0.094f, 0.227f, 0.153f);
                            popupTextRenovar.text = "A " + _jugador.NombreCompleto.ToUpper() + " aún le quedan " + aniosContrato + " años de contrato y no puede ser renovado.";
                        }
                    }
                }
                else
                {
                    popupContainerRenovar.style.display = DisplayStyle.Flex;
                    popupTextRenovar.text = "En estos momentos " + (_jugador.NombreCompleto ?? "el jugador") + " no quiere reunirse contigo. A partir del próximo " + (proximaNegociacion?.ToString("dd/MM/yyyy") ?? "No disponible") + " puedes volver a intentarlo.";
                }

                void OnAceptar()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);

                    popupContainerRenovar.style.display = DisplayStyle.None;

                    UIManager.Instance.CargarPantalla("UI/Negociaciones/NegociacionesOfertaJugador", instancia =>
                    {
                        new NegociacionesOfertaJugador(instancia, miEquipo, miManager, jugadorSeleccionado, mainScreen);
                    });

                    btnYesRenovar.clicked -= OnAceptar;
                }

                void OnCancelar()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);

                    popupContainerRenovar.style.display = DisplayStyle.None;
                    btnYesRenovar.style.display = DisplayStyle.Flex;
                    btnCancelRenovar.text = "CANCELAR";
                    btnCancelRenovar.style.backgroundColor = new Color(0.7f, 0.0f, 0.0f);
                    btnCancelRenovar.clicked -= OnCancelar;
                }

                btnYesRenovar.clicked += OnAceptar;
                btnCancelRenovar.clicked += OnCancelar;
            };

            btnPonerEnMercado.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                if (jugadorSeleccionado != null)
                {
                    popupContainerTransferible.style.display = DisplayStyle.Flex;
                    popupTextTransferible.text = "¿Dónde quieres colocar a " + jugadorSeleccionado.NombreCompleto.ToUpper() + "?";

                    // Importante: limpiar listeners previos para evitar duplicados
                    btnTransferible.clicked -= OnTransferibleClick;
                    btnCedible.clicked -= OnCedibleClick;
                    btnSalir.clicked -= OnSalirClick;

                    void OnTransferibleClick()
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);

                        // Poner al jugador Transferible en la BD
                        JugadorData.PonerJugadorEnMercado(jugadorSeleccionado.IdJugador, 1);

                        // Actualizamos el objeto Jugador
                        Jugador nuevoJugador = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador);

                        // Crear el mensaje
                        string nombreManager = ManagerData.MostrarManager().Nombre + " " +
                                               ManagerData.MostrarManager().Apellido;
                        string contenido = "";
                        if (jugadorSeleccionado.Moral < 50 && jugadorSeleccionado.EstadoAnimo < 40)
                        {
                            contenido = "Estimado/a " + nombreManager.ToUpper() + "\n\nEstoy muy emocionado por esta oportunidad de estar en " +
                                        "el mercado de transferibles. Siempre es gratificante saber que mi talento y esfuerzo están siendo " +
                                        "reconocidos. Siento que he alcanzado un gran nivel esta temporada, y poder aspirar a nuevos horizontes " +
                                        "me llena de ilusión. Estoy convencido de que puedo aportar mucho a cualquier equipo que apueste por " +
                                        "mí, y estoy ansioso por demostrar mis habilidades en el campo.\n\nNo es una despedida fácil, pero estoy " +
                                        "preparado para darlo todo en cualquier proyecto nuevo que se presente. Sea donde sea, seguiré " +
                                        "trabajando con la misma dedicación y pasión por este deporte.";
                        }
                        else if (jugadorSeleccionado.Moral >= 50 && jugadorSeleccionado.EstadoAnimo >= 40)
                        {
                            contenido = "Estimado/a " + nombreManager.ToUpper() + "\n\nNo puedo creer que me hayan puesto en el mercado de " +
                                        "transferibles sin tener una conversación previa conmigo. Me he esforzado al máximo en cada " +
                                        "entrenamiento y partido, sacrificando muchas cosas por el bien del equipo. Sentía que había " +
                                        "construido una relación sólida con el club y la afición, así que esto me ha pillado completamente " +
                                        "por sorpresa.\n\nMe duele pensar que mi compromiso no ha sido valorado lo suficiente. Entiendo que el " +
                                        "fútbol es un negocio, pero esperaba más respeto después de todo lo que he dado por esta camiseta. " +
                                        "Voy a necesitar tiempo para procesar esto y pensar qué es lo mejor para mi futuro como jugador.";
                        }
                        Mensaje mensajeDespido = new Mensaje
                        {
                            Fecha = FechaData.hoy,
                            Remitente = jugadorSeleccionado.NombreCompleto != null ? jugadorSeleccionado.NombreCompleto : "Desconocido",
                            Asunto = "Jugador en el Mercado de Transferibles",
                            Contenido = contenido,
                            TipoMensaje = "Respuesta del jugador",
                            IdEquipo = miEquipo.IdEquipo,
                            Leido = false,
                            Icono = jugadorSeleccionado.IdJugador // Distinto de 0 es icono de jugador
                        };
                        MensajeData.CrearMensaje(mensajeDespido);

                        btnTransferible.clicked -= OnTransferibleClick;
                        btnCedible.clicked -= OnCedibleClick;
                        btnSalir.clicked -= OnSalirClick;
                        popupContainerTransferible.style.display = DisplayStyle.None;

                        CargarOtrosDatosJugador(nuevoJugador);
                    }

                    void OnCedibleClick()
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);

                        // Poner al jugador Transferible en la BD
                        JugadorData.PonerJugadorEnMercado(jugadorSeleccionado.IdJugador, 2);

                        // Actualizamos el objeto Jugador
                        Jugador nuevoJugador = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador);

                        // Crear el mensaje
                        string nombreManager = ManagerData.MostrarManager().Nombre + " " +
                                               ManagerData.MostrarManager().Apellido;
                        string contenido = "";
                        if (jugadorSeleccionado.Moral < 50 && jugadorSeleccionado.EstadoAnimo < 40)
                        {
                            contenido = "Estimado/a " + nombreManager.ToUpper() + "\n\nEstoy muy emocionado por esta oportunidad de estar en " +
                                        "el mercado de transferibles. Siempre es gratificante saber que mi talento y esfuerzo están siendo " +
                                        "reconocidos. Siento que he alcanzado un gran nivel esta temporada, y poder aspirar a nuevos horizontes " +
                                        "me llena de ilusión. Estoy convencido de que puedo aportar mucho a cualquier equipo que apueste por " +
                                        "mí, y estoy ansioso por demostrar mis habilidades en el campo.\n\nNo es una despedida fácil, pero estoy " +
                                        "preparado para darlo todo en cualquier proyecto nuevo que se presente. Sea donde sea, seguiré " +
                                        "trabajando con la misma dedicación y pasión por este deporte.";
                        }
                        else if (jugadorSeleccionado.Moral >= 50 && jugadorSeleccionado.EstadoAnimo >= 40)
                        {
                            contenido = "Estimado/a " + nombreManager.ToUpper() + "\n\nNo puedo creer que me hayan puesto en el mercado de " +
                                        "transferibles sin tener una conversación previa conmigo. Me he esforzado al máximo en cada " +
                                        "entrenamiento y partido, sacrificando muchas cosas por el bien del equipo. Sentía que había " +
                                        "construido una relación sólida con el club y la afición, así que esto me ha pillado completamente " +
                                        "por sorpresa.\n\nMe duele pensar que mi compromiso no ha sido valorado lo suficiente. Entiendo que el " +
                                        "fútbol es un negocio, pero esperaba más respeto después de todo lo que he dado por esta camiseta. " +
                                        "Voy a necesitar tiempo para procesar esto y pensar qué es lo mejor para mi futuro como jugador.";
                        }
                        Mensaje mensajeDespido = new Mensaje
                        {
                            Fecha = FechaData.hoy,
                            Remitente = jugadorSeleccionado.NombreCompleto != null ? jugadorSeleccionado.NombreCompleto : "Desconocido",
                            Asunto = "Jugador en el Mercado de Cesiones",
                            Contenido = contenido,
                            TipoMensaje = "Respuesta del jugador",
                            IdEquipo = miEquipo.IdEquipo,
                            Leido = false,
                            Icono = jugadorSeleccionado.IdJugador // Distinto de 0 es icono de jugador
                        };
                        MensajeData.CrearMensaje(mensajeDespido);

                        popupContainerTransferible.style.display = DisplayStyle.None;
                        btnTransferible.clicked -= OnTransferibleClick;
                        btnCedible.clicked -= OnCedibleClick;
                        btnSalir.clicked -= OnSalirClick;

                        CargarOtrosDatosJugador(nuevoJugador);
                    }

                    void OnSalirClick()
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);
                        popupContainerTransferible.style.display = DisplayStyle.None;

                        btnTransferible.clicked -= OnTransferibleClick;
                        btnCedible.clicked -= OnCedibleClick;
                        btnSalir.clicked -= OnSalirClick;
                    }

                    btnTransferible.clicked += OnTransferibleClick;
                    btnCedible.clicked += OnCedibleClick;
                    btnSalir.clicked += OnSalirClick;
                }
            };

            btnQuitarMercado.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                if (jugadorSeleccionado != null)
                {
                    popupContainerNoTransferible.style.display = DisplayStyle.Flex;
                    popupTextNoTransferible.text = "¿Estás seguro de que quieres quitar del mercado a " + jugadorSeleccionado.NombreCompleto.ToUpper() + "?";

                    // Importante: limpiar listeners previos para evitar duplicados
                    btnNoTransferible.clicked -= OnNoTransferibleClick;
                    btnSalirTransferible.clicked -= OnSalirTransferibleClick;

                    void OnNoTransferibleClick()
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);

                        // Quitar al jugador Transferible en la BD
                        JugadorData.QuitarJugadorDeMercado(jugadorSeleccionado.IdJugador);

                        // Actualizamos el objeto Jugador
                        Jugador nuevoJugador = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador);

                        // Crear el mensaje
                        string nombreManager = ManagerData.MostrarManager().Nombre + " " +
                                               ManagerData.MostrarManager().Apellido;
                        string contenido = "";
                        if (jugadorSeleccionado.Moral < 50 && jugadorSeleccionado.EstadoAnimo < 40)
                        {
                            contenido = "Estimado/a " + nombreManager.ToUpper() + "\n\nNo puedo negar que estoy decepcionado por esta decisión. " +
                                "Sentía que salir del club era lo mejor para mi futuro, ya que estaba buscando nuevos desafíos y una " +
                                "oportunidad para crecer en otro entorno. Haber sido retirado del mercado de transferibles sin cumplir " +
                                "ese objetivo me ha dejado con una mezcla de frustración y desilusión.\n\nAunque siempre respetaré la camiseta " +
                                "que llevo, es difícil seguir motivado cuando sientes que no se han respetado tus deseos. Ahora tendré que " +
                                "reflexionar y decidir cómo manejar esta situación en adelante, pero mi mentalidad seguirá siendo " +
                                "profesional mientras esté aquí.";
                        }
                        else if (jugadorSeleccionado.Moral >= 50 && jugadorSeleccionado.EstadoAnimo >= 40)
                        {
                            contenido = "Estimado/a " + nombreManager.ToUpper() + "\n\nMe siento aliviado y muy feliz de saber que ya no estoy en " +
                                "el mercado de transferibles. Siempre he querido seguir formando parte de este club, y la idea de salir " +
                                "nunca fue algo que me entusiasmara. Este equipo significa mucho para mí, y ahora que la situación está " +
                                "clara, puedo concentrarme al 100% en mi rendimiento.\n\nQuiero demostrarle al cuerpo técnico, a mis compañeros " +
                                "y a los aficionados que pertenezco aquí y que estoy dispuesto a darlo todo en el campo. Esta es una nueva " +
                                "oportunidad para seguir construyendo mi carrera en este club, y no la voy a desaprovechar.";
                        }
                        Mensaje mensajeDespido = new Mensaje
                        {
                            Fecha = FechaData.hoy,
                            Remitente = jugadorSeleccionado.NombreCompleto != null ? jugadorSeleccionado.NombreCompleto : "Desconocido",
                            Asunto = "Jugador deja de estar en el Mercado",
                            Contenido = contenido,
                            TipoMensaje = "Respuesta del jugador",
                            IdEquipo = miEquipo.IdEquipo,
                            Leido = false,
                            Icono = jugadorSeleccionado.IdJugador // Distinto de 0 es icono de jugador
                        };
                        MensajeData.CrearMensaje(mensajeDespido);

                        popupContainerNoTransferible.style.display = DisplayStyle.None;
                        btnNoTransferible.clicked -= OnNoTransferibleClick;
                        btnSalirTransferible.clicked -= OnSalirTransferibleClick;

                        CargarOtrosDatosJugador(nuevoJugador);
                    }

                    void OnSalirTransferibleClick()
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);

                        popupContainerNoTransferible.style.display = DisplayStyle.None;
                        btnNoTransferible.clicked -= OnNoTransferibleClick;
                        btnSalirTransferible.clicked -= OnSalirTransferibleClick;
                    }

                    btnNoTransferible.clicked += OnNoTransferibleClick;
                    btnSalirTransferible.clicked += OnSalirTransferibleClick;
                }
            };

            btnContratar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                btnAceptar.clicked -= OnAceptar;

                DateTime? proximaNegociacion = jugadorSeleccionado.ProximaNegociacion;
                string mensajeProximaNegociacion = proximaNegociacion.HasValue
                    ? proximaNegociacion.Value.ToString("dd/MM/yyyy")
                    : "(No hay una fecha disponible)";

                if (jugadorSeleccionado.ProximaNegociacion > FechaData.hoy)
                {
                    popupContainerOjear.style.display = DisplayStyle.Flex;
                    popupTextOjear.text = "En estos momentos el " + (jugadorSeleccionado.NombreEquipo ?? "el equipo") + " no quiere reunirse contigo. A partir del próximo " + (proximaNegociacion?.ToString("dd/MM/yyyy") ?? "No disponible") + " puedes volver a intentarlo.";
                }
                else
                {
                    if (respuestaEquipo == 1)
                    {
                        // Ir a Negociar con el jugador
                        UIManager.Instance.CargarPantalla("UI/Negociaciones/NegociacionesOfertaJugador", instancia =>
                        {
                            new NegociacionesOfertaJugador(instancia, miEquipo, miManager, jugadorSeleccionado, mainScreen);
                        });
                    }
                    else
                    {
                        // Ir a Negociar con el equipo
                        UIManager.Instance.CargarPantalla("UI/Negociaciones/NegociacionesOfertaEquipo", instancia =>
                        {
                            new NegociacionesOfertaEquipo(instancia, miEquipo, miManager, jugadorSeleccionado, mainScreen);
                        });
                    }
                }

                void OnAceptar()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);

                    // Actualizamos el objeto Jugador
                    Jugador nuevoJugador = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador);

                    CargarOtrosDatosJugador(nuevoJugador);
                    CargarBotones(nuevoJugador);

                    popupContainerOjear.style.display = DisplayStyle.None;
                    btnAceptar.clicked -= OnAceptar;
                }

                btnAceptar.clicked += OnAceptar;
            };

            btnOjear.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                // Comprobar si se tiene contratado un Director Técnico.
                string mensaje = "";
                bool encontrado = EmpleadoData.EmpleadoEncontrado("Director Técnico");

                List<Jugador> listaJugadoresOjeados = OjearData.ListadoJugadoresOjeados();
                int numJugadoresOjeados = listaJugadoresOjeados.Count();

                Jugador jugador = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador);

                btnAceptar.clicked -= OnAceptar;

                if (encontrado)
                {
                    if (numJugadoresOjeados >= 8)
                    {
                        mensaje = "Ya tienes el número máximo de jugadores en Cartera. Elimina alguno antes de ojear un nuevo jugador.";

                        // Mostrar Popup
                        popupContainerOjear.style.display = DisplayStyle.Flex;
                        popupTextOjear.text = mensaje;
                    }
                    else
                    {
                        if (jugador != null)
                        {
                            // Comprobar si el jugador ya está siendo ojeado.
                            bool jugadorYaOjeado = OjearData.ComprobarSiJugadorEnCartera(jugadorSeleccionado.IdJugador);

                            if (jugadorYaOjeado)
                            {
                                mensaje = jugador.NombreCompleto.ToUpper() + " ya está siendo ojeado. Puedes ver los detalles entrando en tu cartera.";

                                // Mostrar Popup
                                popupContainerOjear.style.display = DisplayStyle.Flex;
                                popupTextOjear.text = mensaje;
                            }
                            else
                            {
                                int categoria = 0;
                                int diasInforme = 0;
                                // Categoría del Director Técnico
                                List<Empleado> empleados = EmpleadoData.MostrarListaEmpleadosContratados(miEquipo.IdEquipo);
                                foreach (var empleado in empleados)
                                {
                                    if (empleado.Puesto.Equals("Director Técnico"))
                                    {
                                        categoria = empleado.Categoria;
                                    }
                                }

                                if (categoria == 1)
                                {
                                    diasInforme = 30;
                                }
                                else if (categoria == 2)
                                {
                                    diasInforme = 25;
                                }
                                else if (categoria == 3)
                                {
                                    diasInforme = 20;
                                }
                                else if (categoria == 4)
                                {
                                    diasInforme = 15;
                                }
                                else if (categoria == 5)
                                {
                                    diasInforme = 10;
                                }

                                mensaje = jugador.NombreCompleto.ToUpper() + " ha sido añadido a tu cartera. El día " + FechaData.hoy.AddDays(diasInforme).ToString("dd/MM/yyyy") + " tendrás disponible su ficha completa.";

                                // Mostrar Popup
                                popupContainerOjear.style.display = DisplayStyle.Flex;
                                popupTextOjear.text = mensaje;

                                // Añadir el jugador a la cartera en la base de datos.
                                OjearData.PonerJugadorCartera(jugadorSeleccionado.IdJugador, diasInforme);
                            }
                        }
                    }
                }
                else
                {
                    mensaje = "No tienes ningún Director Técnico contratado. Para poder añadir este jugador a tu cartera debes contratar uno.";

                    // Mostrar Popup
                    popupContainerOjear.style.display = DisplayStyle.Flex;
                    popupTextOjear.text = mensaje;
                }

                void OnAceptar()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);

                    // Actualizamos el objeto Jugador
                    Jugador nuevoJugador = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador);

                    CargarOtrosDatosJugador(nuevoJugador);

                    popupContainerOjear.style.display = DisplayStyle.None;
                    btnAceptar.clicked -= OnAceptar;
                }

                btnAceptar.clicked += OnAceptar;
            };
        }

        private void ComprobarEquipoOfertaAceptadaTextoBotones(Jugador jugadorSeleccionado)
        {
            // Comprobar si el jugador tiene oferta aceptada por el equipo
            respuestaEquipo = TransferenciaData.ComprobarRespuestaEquipo(jugadorSeleccionado.IdJugador, miEquipo.IdEquipo, jugadorSeleccionado.IdEquipo);
            if (respuestaEquipo == 1)
                btnContratar.text = "NEGOCIAR CONTRATO";
            else
                btnContratar.text = "CONTRATAR";
        }

        private void CargarAtributosSinOjear(Jugador jugadorSeleccionado)
        {
            velocidad.text = "-";
            resistencia.text = "-";
            agresividad.text = "-";
            calidad.text = "-";
            potencial.text = "-";

            portero.text = "-";
            pase.text = "-";
            regate.text = "-";
            remate.text = "-";
            entradas.text = "-";
            tiro.text = "-";
        }

        private void CargarAtributosOjeado(Jugador jugadorSeleccionado)
        {
            velocidad.text = $"{jugadorSeleccionado.Velocidad}";
            resistencia.text = $"{jugadorSeleccionado.Resistencia}";
            agresividad.text = $"{jugadorSeleccionado.Agresividad}";
            calidad.text = $"{jugadorSeleccionado.Calidad}";
            potencial.text = $"{jugadorSeleccionado.Potencial}";

            portero.text = $"{jugadorSeleccionado.Portero}";
            pase.text = $"{jugadorSeleccionado.Pase}";
            regate.text = $"{jugadorSeleccionado.Regate}";
            remate.text = $"{jugadorSeleccionado.Remate}";
            entradas.text = $"{jugadorSeleccionado.Entradas}";
            tiro.text = $"{jugadorSeleccionado.Tiro}";
        }

        private void CargarContratoJugadorSinOjear(Jugador jugadorSeleccionado)
        {
            salario.text = "-";
            aniosRestantes.text = "-";
            clausula.text = "-";
            bonusPartidos.text = "-";
            bonusGol.text = "-";
        }

        private void CargarContratoJugadorOjeado(Jugador jugadorSeleccionado)
        {
            salario.text = $"{Constants.CambioDivisaNullable(jugadorSeleccionado.SalarioTemporada):N0} {CargarSimboloMoneda()}";
            aniosRestantes.text = $"{jugadorSeleccionado.AniosContrato}";
            clausula.text = $"{Constants.CambioDivisaNullable(jugadorSeleccionado.ClausulaRescision):N0} {CargarSimboloMoneda()}";
            bonusPartidos.text = $"{Constants.CambioDivisaNullable(jugadorSeleccionado.BonusPartido):N0} {CargarSimboloMoneda()}";
            bonusGol.text = $"{Constants.CambioDivisaNullable(jugadorSeleccionado.BonusGoles):N0} {CargarSimboloMoneda()}";
        }

        private void CargarEstadoActualJugadorOjeado(Jugador jugador)
        {
            // Estado de Forma
            condicionFisica.text = $"{jugador.EstadoForma}";

            // Moral
            int moralValue = jugador.Moral;
            if (moralValue >= 70)
            {
                var moralSprite = Resources.Load<Sprite>($"Icons/arriba_icon");
                if (moralSprite != null)
                    moral.style.backgroundImage = new StyleBackground(moralSprite);
            }
            else if (moralValue >= 35)
            {
                var moralSprite = Resources.Load<Sprite>($"Icons/right_icon");
                if (moralSprite != null)
                    moral.style.backgroundImage = new StyleBackground(moralSprite);
            }
            else
            {
                var moralSprite = Resources.Load<Sprite>($"Icons/abajo_icon");
                if (moralSprite != null)
                    moral.style.backgroundImage = new StyleBackground(moralSprite);
            }

            // Estado de Ánimo
            int animoValue = jugador.EstadoAnimo;
            if (animoValue >= 70)
            {
                var animoSprite = Resources.Load<Sprite>($"Icons/arriba_icon");
                if (animoSprite != null)
                    estadoAnimo.style.backgroundImage = new StyleBackground(animoSprite);
            }
            else if (animoValue >= 35)
            {
                var animoSprite = Resources.Load<Sprite>($"Icons/right_icon");
                if (animoSprite != null)
                    estadoAnimo.style.backgroundImage = new StyleBackground(animoSprite);
            }
            else
            {
                var animoSprite = Resources.Load<Sprite>($"Icons/abajo_icon");
                if (animoSprite != null)
                    estadoAnimo.style.backgroundImage = new StyleBackground(animoSprite);
            }

            // Situación de Mercado
            int situacionMercadoValue = jugador.SituacionMercado;
            if (situacionMercadoValue > 0)
            {
                var situacionMercadoSprite = Resources.Load<Sprite>($"Icons/transferible_icon");
                if (situacionMercadoSprite != null)
                    situacionMercado.style.backgroundImage = new StyleBackground(situacionMercadoSprite);
            }
            else
            {
                situacionMercado.style.backgroundImage = null;
            }

            // Lesionado
            int lesionadoValue = jugador.Lesion;
            if (lesionadoValue > 0)
            {
                var lesionadoSprite = Resources.Load<Sprite>($"Icons/lesion");
                if (lesionadoSprite != null)
                    lesionado.style.backgroundImage = new StyleBackground(lesionadoSprite);
            }
            else
            {
                lesionado.style.backgroundImage = null;
            }

            // Status
            int status = jugador.Status;
            if (status == 1)
            {
                var rolSprite = Resources.Load<Sprite>($"Icons/rol1_icon");
                if (rolSprite != null)
                    rol.style.backgroundImage = new StyleBackground(rolSprite);
            }
            else if (status == 2)
            {
                var rolSprite = Resources.Load<Sprite>($"Icons/rol2_icon");
                if (rolSprite != null)
                    rol.style.backgroundImage = new StyleBackground(rolSprite);
            }
            else if (status == 3)
            {
                var rolSprite = Resources.Load<Sprite>($"Icons/rol3_icon");
                if (rolSprite != null)
                    rol.style.backgroundImage = new StyleBackground(rolSprite);
            }
            else if (status == 4)
            {
                var rolSprite = Resources.Load<Sprite>($"Icons/rol4_icon");
                if (rolSprite != null)
                    rol.style.backgroundImage = new StyleBackground(rolSprite);
            }
        }

        private void CargarEstadoActualJugadorSinOjear(Jugador jugador)
        {
            // Estado de Forma
            condicionFisica.text = "";
            moral.style.backgroundImage = null;
            estadoAnimo.style.backgroundImage = null;
            situacionMercado.style.backgroundImage = null;

            // Lesionado
            int lesionadoValue = jugador.Lesion;
            if (lesionadoValue > 0)
            {
                var lesionadoSprite = Resources.Load<Sprite>($"Icons/lesion");
                if (lesionadoSprite != null)
                    lesionado.style.backgroundImage = new StyleBackground(lesionadoSprite);
            }
            else
            {
                lesionado.style.backgroundImage = null;
            }

            rol.style.backgroundImage = null;
        }

        private void CargarDatosJugador(Jugador jugador)
        {
            var caraSprite = Resources.Load<Sprite>($"Jugadores/{jugador.IdJugador}");
            if (caraSprite != null)
                fotoJugador.style.backgroundImage = new StyleBackground(caraSprite);

            var escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/{jugador.IdEquipo}");
            if (escudoSprite != null)
                escudo.style.backgroundImage = new StyleBackground(escudoSprite);

            var posicionSprite = Resources.Load<Sprite>($"Demarcaciones/{jugador.RolId}");
            if (posicionSprite != null)
                imagenPosicion.style.backgroundImage = new StyleBackground(posicionSprite);

            dorsal.text = $"{jugador.Dorsal}";
            nombreJugador.text = $"{jugador.NombreCompleto}";
            mediaTotal.text = $"{jugador.Media}";
            mediaTotal.style.color = DeterminarColor(jugador.Media);
            nombreEquipo.text = $"{EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre}";
            posicion.text = $"{jugador.Rol}";
            altura.text = $"{jugador.Altura} cms";
            peso.text = $"{jugador.Peso} kg";
            edad.text = $"{jugador.Edad} años";
            nacionalidad.text = $"{jugador.Nacionalidad}";
            valorMercado.text = $"{Constants.CambioDivisa(jugador.ValorMercado):N0} {CargarSimboloMoneda()}";
        }

        private void CargarOtrosDatosJugador(Jugador jugadorSeleccionado)
        {
            List<Jugador> jugadores = OjearData.ListadoJugadoresOjeados();
            DateTime fecha_informe = DateTime.MinValue;

            foreach (var item in jugadores)
            {
                if (item.IdJugador == jugadorSeleccionado.IdJugador)
                {
                    fecha_informe = DateTime.Parse(item.FechaInforme);
                    break;
                }
            }

            DateTime fechaHoy = FechaData.hoy;

            if (jugadorSeleccionado.IdEquipo == miEquipo.IdEquipo)
            {
                CargarEstadoActualJugadorOjeado(jugadorSeleccionado);
                CargarContratoJugadorOjeado(jugadorSeleccionado);
                CargarAtributosOjeado(jugadorSeleccionado);

                btnDespedir.style.display = DisplayStyle.Flex;
                btnRenovar.style.display = DisplayStyle.Flex;
                if (jugadorSeleccionado.SituacionMercado == 0)
                {
                    btnPonerEnMercado.style.display = DisplayStyle.Flex;
                    btnQuitarMercado.style.display = DisplayStyle.None;
                }
                else
                {
                    btnPonerEnMercado.style.display = DisplayStyle.None;
                    btnQuitarMercado.style.display = DisplayStyle.Flex;
                }
                btnContratar.style.display = DisplayStyle.None;
                btnOjear.style.display = DisplayStyle.None;
            }
            else
            {
                if (fecha_informe <= fechaHoy)
                {
                    CargarEstadoActualJugadorOjeado(jugadorSeleccionado);
                    CargarContratoJugadorOjeado(jugadorSeleccionado);
                    CargarAtributosOjeado(jugadorSeleccionado);

                    btnOjear.style.display = DisplayStyle.None;
                }
                else
                {
                    CargarContratoJugadorSinOjear(jugadorSeleccionado);
                    CargarAtributosSinOjear(jugadorSeleccionado);

                    btnOjear.style.display = DisplayStyle.Flex;
                }

                btnDespedir.style.display = DisplayStyle.None;
                btnRenovar.style.display = DisplayStyle.None;
                btnPonerEnMercado.style.display = DisplayStyle.None;
                btnQuitarMercado.style.display = DisplayStyle.None;
                btnContratar.style.display = DisplayStyle.Flex;
            }
        }

        private void CargarBotones(Jugador jugadorSeleccionado)
        {
            bool ojeado = OjearData.ComprobarJugadorOjeado(jugadorSeleccionado.IdJugador);

            if (jugadorSeleccionado.IdEquipo == miEquipo.IdEquipo)
                opcionBotones = 1;
            else
                opcionBotones = 2;

            if (opcionBotones == 1)
            {
                btnVolver.style.display = DisplayStyle.Flex;
                btnDespedir.style.display = DisplayStyle.Flex;
                btnRenovar.style.display = DisplayStyle.Flex;

                if (jugadorSeleccionado.SituacionMercado != 0)
                {
                    btnPonerEnMercado.style.display = DisplayStyle.None;
                    btnQuitarMercado.style.display = DisplayStyle.Flex;
                }
                else
                {
                    btnPonerEnMercado.style.display = DisplayStyle.Flex;
                    btnQuitarMercado.style.display = DisplayStyle.None;
                }

                btnContratar.style.display = DisplayStyle.None;
                btnOjear.style.display = DisplayStyle.None;
            }
            else
            {
                btnVolver.style.display = DisplayStyle.Flex;
                btnDespedir.style.display = DisplayStyle.None;
                btnRenovar.style.display = DisplayStyle.None;
                btnPonerEnMercado.style.display = DisplayStyle.None;
                btnQuitarMercado.style.display = DisplayStyle.None;
                btnContratar.style.display = DisplayStyle.Flex; ;
                btnOjear.style.display = DisplayStyle.Flex; ;
            }

            List<Transferencia> ofertas = TransferenciaData.ListarTraspasos();
            DateTime fechaHoy = FechaData.hoy;
            DateTime fechaManiana = fechaHoy.AddDays(1);

            foreach (var oferta in ofertas)
            {
                if (!string.IsNullOrWhiteSpace(oferta.FechaTraspaso) &&
                    DateTime.TryParse(oferta.FechaTraspaso, out DateTime fechaTraspaso) &&
                    oferta.IdJugador == jugadorSeleccionado.IdJugador &&
                    fechaTraspaso == fechaManiana)
                {
                    btnContratar.SetEnabled(false);
                    btnOjear.SetEnabled(false);
                }
            }
        }

        private string CargarSimboloMoneda()
        {
            string currency = PlayerPrefs.GetString("Currency", string.Empty);

            // Elegir símbolo según moneda
            string simbolo = currency switch
            {
                Constants.EURO_NAME => Constants.EURO_SYMBOL,
                Constants.POUND_NAME => Constants.POUND_SYMBOL,
                Constants.DOLLAR_NAME => Constants.DOLLAR_SYMBOL,
                _ => Constants.EURO_SYMBOL // default
            };

            return simbolo;
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