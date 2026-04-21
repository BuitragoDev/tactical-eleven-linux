using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

namespace TacticalEleven.Scripts
{
    public class NegociacionesOfertaEquipo
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        private Jugador jugador;
        private MainScreen mainScreen;

        private int cantidadActual;
        private const int Paso = 50000;
        private const int PasoRapido = 1000000;
        private const int MinCantidad = 0;
        private const int MaxCantidad = 300_000_000;

        IVisualElementScheduledItem incrementoSchedule;
        IVisualElementScheduledItem decrementoSchedule;

        private VisualElement root, escudo, foto, popupContainer;
        private Label nombreEquipo, dorsal, nombreJugador, media, posicion, rol, salario, clausula, anios, valor, cantidad, divisa, popupText;
        private Button btnMenos, btnMenosMenos, btnMas, btnMasMas, btnCesion, btnCancelarOferta, btnEnviarOferta, btnCerrar;

        public NegociacionesOfertaEquipo(VisualElement rootInstance, Equipo equipo, Manager manager, Jugador jug, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            jugador = jug;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            escudo = root.Q<VisualElement>("escudo-equipo");
            foto = root.Q<VisualElement>("foto-jugador");
            btnMenos = root.Q<Button>("btnMenos");
            btnMas = root.Q<Button>("btnMas");
            btnMenosMenos = root.Q<Button>("btnMenosMenos");
            btnMasMas = root.Q<Button>("btnMasMas");
            btnCesion = root.Q<Button>("btnCesion");
            btnCancelarOferta = root.Q<Button>("btnCancelarOferta");
            btnEnviarOferta = root.Q<Button>("btnEnviarOferta");
            nombreEquipo = root.Q<Label>("nombre-equipo");
            dorsal = root.Q<Label>("dorsal-jugador");
            nombreJugador = root.Q<Label>("nombre-jugador");
            media = root.Q<Label>("media-jugador");
            posicion = root.Q<Label>("posicion-jugador");
            rol = root.Q<Label>("rol-jugador");
            salario = root.Q<Label>("salario-jugador");
            clausula = root.Q<Label>("clausula-jugador");
            anios = root.Q<Label>("anios-jugador");
            valor = root.Q<Label>("valor-jugador");
            cantidad = root.Q<Label>("cantidad-ofrecida");
            divisa = root.Q<Label>("texto-divisa");

            // Referencias a objetos de la UI
            popupContainer = root.Q<VisualElement>("popup-container");
            btnCerrar = root.Q<Button>("btnCerrar");
            popupText = root.Q<Label>("popup-text");

            btnCancelarOferta.text = "CANCELAR NEGOCIACIÓN";
            btnCesion.SetEnabled(true);
            btnMenos.SetEnabled(true);
            btnMenosMenos.SetEnabled(true);
            btnMas.SetEnabled(true);
            btnMasMas.SetEnabled(true);

            CargarDatosEquipo();
            CargarDatosJugador();
            CargarZonaOfertas();

            btnCesion.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                // Importante: limpiar listeners previos para evitar duplicados
                btnCerrar.clicked -= OnCerrarClick;

                Transferencia transaccion = TransferenciaData.EvaluarOfertaEquipo(jug.IdJugador, miEquipo.IdEquipo, 0, 2);
                DateTime? proximaNegociacion = jugador.ProximaNegociacion;
                string mensajeProximaNegociacion = proximaNegociacion.HasValue
                    ? proximaNegociacion.Value.ToString("dd/MM/yyyy")
                    : "(No hay una fecha disponible)";

                if (jugador.ProximaNegociacion > FechaData.hoy)
                {
                    popupContainer.style.display = DisplayStyle.Flex;
                    popupText.text = "En estos momentos el " + (jugador.NombreEquipo ?? "el equipo") + " no quiere reunirse contigo. A partir del próximo " + (proximaNegociacion?.ToString("dd/MM/yyyy") ?? "No disponible") + " puedes volver a intentarlo.";
                }
                else
                {
                    // Verificar disponibilidad para la cesion.
                    DateTime hoy = FechaData.hoy;

                    // Ventanas de fichajes
                    DateTime inicioVerano = new DateTime(hoy.Year, 7, 1);
                    DateTime finVerano = new DateTime(hoy.Year, 8, 30);
                    DateTime inicioInvierno = new DateTime(hoy.Year, 1, 1);
                    DateTime finInvierno = new DateTime(hoy.Year, 1, 31);

                    DateTime siguienteFecha = DateTime.MinValue;

                    if ((hoy >= inicioVerano && hoy <= finVerano) || (hoy >= inicioInvierno && hoy <= finInvierno))
                    {
                        // 📌 Sistema de Puntuación
                        int puntosCesion = 0;
                        int tipoRechazoCesion = 0;
                        Random rnd = new Random();

                        if (transaccion.Status == 1 || transaccion.Status == 2)
                        {
                            puntosCesion += 0;
                            tipoRechazoCesion = 1;
                        }
                        else if (transaccion.Status == 3)
                        {
                            int numero = rnd.Next(1, 5); // Genera 1 a 4 (25% opciones)
                            if (numero == 2)
                            {
                                puntosCesion += 1;
                            }
                        }
                        else if (transaccion.Status == 4)
                        {
                            int numero = rnd.Next(1, 3); // Genera 1 o 2 (50% opciones)
                            if (numero == 1)
                            {
                                puntosCesion += 1;
                            }
                        }

                        if (puntosCesion > 0)
                        {
                            // Comprobamos la fecha de Traspaso dentro de los periodos de fichajes
                            DateTime fechaTraspaso;

                            // Comprobar si hoy está en un rango válido
                            bool enRangoEnero = hoy >= inicioInvierno && hoy <= finInvierno;
                            bool enRangoVerano = hoy >= inicioVerano && hoy <= finVerano;

                            if (enRangoEnero || enRangoVerano)
                            {
                                fechaTraspaso = hoy.AddDays(1);
                            }
                            else
                            {
                                // Determinar el próximo rango válido
                                if (hoy < inicioInvierno)
                                {
                                    fechaTraspaso = inicioInvierno;
                                }
                                else if (hoy < inicioVerano)
                                {
                                    fechaTraspaso = inicioVerano;
                                }
                                else
                                {
                                    // Después del 30 de agosto, ir al enero del siguiente año
                                    fechaTraspaso = new DateTime(hoy.Year + 1, 1, 1);
                                }
                            }


                            Transferencia oferta = new Transferencia
                            {
                                IdJugador = jugador.IdJugador,
                                IdEquipoOrigen = jugador.IdEquipo,
                                IdEquipoDestino = miEquipo.IdEquipo,
                                TipoFichaje = 2,
                                MontoOferta = 0,
                                FechaOferta = FechaData.hoy.ToString("yyyy-MM-dd"),
                                FechaTraspaso = fechaTraspaso.ToString("yyyy-MM-dd"),
                                RespuestaEquipo = 1,
                                RespuestaJugador = 1,
                                SalarioAnual = 0,
                                ClausulaRescision = 0,
                                Duracion = 0,
                                BonoPorGoles = 0,
                                BonoPorPartidos = 0,
                            };

                            // Comprobar si el jugador ya tiene una oferta de mi equipo
                            bool comprobacion = TransferenciaData.ComprobarOfertaActiva(jugador.IdJugador, miEquipo.IdEquipo, jugador.IdEquipo);
                            if (comprobacion == true)
                            {
                                // Actualizar la oferta
                                TransferenciaData.ActualizarOferta(oferta);
                            }
                            else
                            {
                                // Registrar la oferta
                                TransferenciaData.RegistrarOferta(oferta);
                            }

                            popupContainer.style.display = DisplayStyle.Flex;
                            popupText.text = $"El {jugador.NombreEquipo} ha aceptado la oferta de cesión por {jugador.NombreCompleto}.";

                            // Registrar la transferencia ya confirmada
                            TransferenciaData.RegistrarTransferencia(oferta);

                            // Crear el mensaje confirmando la cesión de un jugador
                            Empleado director = EmpleadoData.ObtenerEmpleadoPorPuesto("Director Técnico");
                            string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;
                            string nombreJugador = JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto;

                            Mensaje mensajeJugadorCedido = new Mensaje
                            {
                                Fecha = FechaData.hoy,
                                Remitente = nombreJugador != null ? nombreJugador : director.Nombre,
                                Asunto = "Confirmación de cesión",
                                Contenido = $"Nos complace informarte que hemos cerrado satisfactoriamente la cesión de {JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto.ToUpper()} procedente del {EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre.ToUpper()}. El acuerdo es válido hasta el final de la presente temporada y el jugador estará disponible en nuestra plantilla a partir de mañana.\n\nCreemos que esta cesión será beneficiosa para su desarrollo, ya que contará con más minutos en un entorno competitivo. Estaremos atentos a su evolución durante su estancia en su nuevo club.\n\nAgradecemos tu gestión y quedamos a tu disposición para cualquier consulta adicional.",
                                TipoMensaje = "Notificación",
                                IdEquipo = miEquipo.IdEquipo,
                                Leido = false,
                                Icono = jugador.IdJugador
                            };

                            MensajeData.CrearMensaje(mensajeJugadorCedido);
                        }
                        else
                        {
                            if (tipoRechazoCesion == 1)
                            {
                                popupContainer.style.display = DisplayStyle.Flex;
                                popupText.text = $"El {jugador.NombreEquipo} ha rechazado la oferta de cesión por {jugador.NombreCompleto} ya que lo considera un jugador clave de su equipo.";
                            }
                            else
                            {
                                popupContainer.style.display = DisplayStyle.Flex;
                                popupText.text = $"El {jugador.NombreEquipo} no quiere ceder a su jugador {jugador.NombreCompleto} en estos momentos.";
                            }

                            Transferencia oferta = new Transferencia
                            {
                                IdJugador = jugador.IdJugador,
                                IdEquipoOrigen = jugador.IdEquipo,
                                IdEquipoDestino = miEquipo.IdEquipo,
                                TipoFichaje = 2,
                                MontoOferta = 0,
                                FechaOferta = FechaData.hoy.ToString(),
                                RespuestaEquipo = 0,
                                RespuestaJugador = 0
                            };

                            // Comprobar si el jugador ya tiene una oferta de mi equipo
                            bool comprobacion = TransferenciaData.ComprobarOfertaActiva(jugador.IdJugador, miEquipo.IdEquipo, jugador.IdEquipo);
                            if (comprobacion == true)
                            {
                                // Actualizar la oferta
                                TransferenciaData.ActualizarOferta(oferta);
                            }
                            else
                            {
                                // Registrar la oferta
                                TransferenciaData.RegistrarOferta(oferta);
                            }

                            JugadorData.NegociacionCancelada(jugador.IdJugador, 7);

                            // Crear el mensaje confirmando la cesión de un jugador
                            Empleado? director = EmpleadoData.ObtenerEmpleadoPorPuesto("Director Técnico");
                            string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;
                            string nombreJugador = JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto;

                            Mensaje mensajeJugadorCedido = new Mensaje
                            {
                                Fecha = FechaData.hoy,
                                Remitente = nombreJugador != null ? nombreJugador : director.Nombre,
                                Asunto = "Oferta de cesión rechazada",
                                Contenido = $"Lamentamos informarte que el {EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre.ToUpper()} ha rechazado nuestra oferta de cesión por {JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto.ToUpper()}.\n\nTras evaluar la propuesta, nos han comunicado que el jugador es parte de sus planes a corto plazo y no contemplan su salida en este momento.",
                                TipoMensaje = "Notificación",
                                IdEquipo = miEquipo.IdEquipo,
                                Leido = false,
                                Icono = jugador.IdJugador
                            };

                            MensajeData.CrearMensaje(mensajeJugadorCedido);
                        }
                    }
                    else
                    {
                        // Determinar la próxima fecha disponible
                        if (hoy < inicioInvierno)
                        {
                            siguienteFecha = inicioInvierno;
                        }
                        else if (hoy > finInvierno && hoy < inicioVerano)
                        {
                            siguienteFecha = inicioVerano;
                        }
                        else // hoy > finVerano
                        {
                            siguienteFecha = new DateTime(hoy.Year + 1, 1, 1);
                        }

                        popupContainer.style.display = DisplayStyle.Flex;
                        popupText.text = $"En este momento el mercado de traspasos está cerrado.\nInténtalo a partir del próximo {siguienteFecha:dd MMMM yyyy}.";
                    }
                }

                void OnCerrarClick()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);

                    btnCerrar.clicked -= OnCerrarClick;
                    popupContainer.style.display = DisplayStyle.None;
                }

                btnCerrar.clicked += OnCerrarClick;
            };

            btnCancelarOferta.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                UIManager.Instance.CargarPantalla("UI/Ficha/Ficha", instancia =>
                {
                    new FichaJugador(instancia, miEquipo, miManager, jugador.IdJugador, -1, mainScreen);
                });
            };

            btnEnviarOferta.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                // Importante: limpiar listeners previos para evitar duplicados
                btnCerrar.clicked -= OnCerrarClick;

                // Recogemos el valor de la oferta de la caja de texto
                string textoMontoOferta = cantidad.text; // Texto original con puntos y símbolo €
                string textoMontoOfertaSinSimbolos = System.Text.RegularExpressions.Regex.Replace(textoMontoOferta, @"[^\d]", ""); // Elimina todo lo que no sea un dígito
                int ofertaEquipo = int.Parse(textoMontoOfertaSinSimbolos); // Convierte el texto limpio a int

                Transferencia oferta = TransferenciaData.EvaluarOfertaEquipo(jugador.IdJugador, miEquipo.IdEquipo, ofertaEquipo, 1);

                // Recogida de datos
                int valorMercado = oferta.ValorMercado;
                int situacionMercado = oferta.SituacionMercado;
                int moral = oferta.Moral;
                int estadoAnimo = oferta.EstadoAnimo;
                string fechaFinContrato = oferta.FinContrato;
                int clausulaRescision = oferta.ClausulaRescision;
                int equipoActual = oferta.IdEquipoOrigen;
                int presupuestoEquipoVendedor = oferta.PresupuestoVendedor;
                int rival = oferta.Rival;
                int presupuestoEquipoComprador = oferta.PresupuestoComprador;
                int montoOferta = oferta.MontoOferta;
                int equipoOrigen = oferta.IdEquipoOrigen;
                int equipoDestino = oferta.IdEquipoDestino;
                int status = oferta.Status;

                // 📌 Sistema de Puntuación
                int puntosOferta = 0;
                int tipoRechazo = 0;

                // ✔ Factor 1: Comparación con el valor de mercado
                if (montoOferta >= valorMercado * 1.5) puntosOferta += 3;
                else if (montoOferta >= valorMercado * 1.2) puntosOferta += 2;
                else if (montoOferta >= valorMercado * 1.0) puntosOferta += 1;
                else puntosOferta -= 2; // Oferta demasiado baja

                // ✔ Factor 2: Situación en el mercado (jugador transferible o cedible)
                if (situacionMercado == 1 || situacionMercado == 2) puntosOferta += 3; // Más fácil de aceptar si es transferible

                // ✔ Factor 3: Estado del jugador
                if (estadoAnimo < 30 || moral < 30) puntosOferta += 2; // Jugador infeliz, más probable que lo vendan

                // ✔ Factor 4: Necesidad económica del equipo
                if (presupuestoEquipoVendedor < 5000000 && montoOferta > valorMercado * 0.8) puntosOferta += 3;

                // ✔ Factor 5: Duración del contrato
                DateTime fechaFin = DateTime.Parse(fechaFinContrato);
                DateTime hoy = DateTime.Today;
                int mesesRestantes = ((fechaFin.Year - hoy.Year) * 12) + fechaFin.Month - hoy.Month;

                if (mesesRestantes <= 6 && montoOferta > valorMercado * 0.50) puntosOferta += 2;

                // ✔ Factor 6: Status en el Equipo
                if (status >= 3 && montoOferta > valorMercado * 0.75) puntosOferta += 2;

                // ❌ Factor 7: Rivalidad entre equipos
                if (equipoOrigen == rival)
                {
                    puntosOferta -= 5;
                }

                // 📌 Factor 8: Cláusula de rescisión (aceptación instantánea)
                if (montoOferta >= clausulaRescision)
                {
                    puntosOferta = 5;
                }

                // ❌ Factor 9: Presupuesto del comprador
                if (presupuestoEquipoComprador < montoOferta)
                {
                    puntosOferta = 0;
                    tipoRechazo = 1;
                }

                // ✔ Decisión final: ¿La oferta es suficientemente buena?
                if (puntosOferta >= 5)
                {
                    // Comprobar si el equipo ya esta negociando
                    bool enNegociacion = TransferenciaData.ComprobarOfertaActiva(jugador.IdJugador, miEquipo.IdEquipo, jugador.IdEquipo);
                    Transferencia ofertaAceptada = new Transferencia
                    {
                        IdJugador = oferta.IdJugador,
                        IdEquipoOrigen = JugadorData.MostrarDatosJugador(oferta.IdJugador).IdEquipo,
                        IdEquipoDestino = miEquipo.IdEquipo,
                        TipoFichaje = 1,
                        MontoOferta = ofertaEquipo,
                        FechaOferta = FechaData.hoy.ToString("yyyy-MM-dd"),
                        FechaTraspaso = "",
                        RespuestaEquipo = 1,
                        RespuestaJugador = 0,
                        SalarioAnual = 0,
                        ClausulaRescision = 0,
                        Duracion = 0,
                        BonoPorGoles = 0,
                        BonoPorPartidos = 0,
                    };

                    if (enNegociacion != true)
                    {
                        TransferenciaData.RegistrarOferta(ofertaAceptada);
                    }
                    else
                    {
                        TransferenciaData.ActualizarOferta(ofertaAceptada);
                    }

                    // Crear el mensaje confirmando la aceptacion de un oferta de traspaso
                    Empleado? director = EmpleadoData.ObtenerEmpleadoPorPuesto("Director Técnico");
                    string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;
                    string nombreJugador = JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto;

                    Mensaje mensajeEquipoAceptaOferta = new Mensaje
                    {
                        Fecha = FechaData.hoy,
                        Remitente = nombreJugador != null ? nombreJugador : director.Nombre,
                        Asunto = "Oferta aceptada",
                        Contenido = $"El {EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre.ToUpper()} ha aceptado nuestra oferta de {ofertaEquipo.ToString("N0", new CultureInfo("es-ES"))}€ por {JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto.ToUpper()}. Nos han autorizado a iniciar conversaciones directas con el jugador y su representante para negociar los términos personales del contrato.\n\nPuedes comenzar la negociación en cuanto lo consideres oportuno. Te recomendamos actuar con agilidad para cerrar el acuerdo antes de que otros clubes se interpongan.",
                        TipoMensaje = "Notificación",
                        IdEquipo = miEquipo.IdEquipo,
                        Leido = false,
                        Icono = jugador.IdJugador
                    };

                    MensajeData.CrearMensaje(mensajeEquipoAceptaOferta);

                    // Ventana emergente diciendo que el equipo ha aceptado la oferta.
                    popupContainer.style.display = DisplayStyle.Flex;
                    popupText.text = $"El {EquipoData.ObtenerDetallesEquipo(equipoOrigen).Nombre.ToUpper()} ha aceptado tu oferta por {JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto.ToUpper()}. Ahora ya puedes negociar con el jugador.";
                }
                else
                {
                    string mensaje = "";
                    if (tipoRechazo == 0)
                    {
                        mensaje = $"La oferta ha sido rechazada por el {EquipoData.ObtenerDetallesEquipo(equipoOrigen).Nombre.ToUpper()}. La considera insuficiente";
                    }
                    else if (tipoRechazo == 1)
                    {
                        mensaje = $"La oferta ha sido cancelada ya que no dispones de suficiente dinero.";
                    }
                    popupContainer.style.display = DisplayStyle.Flex;
                    popupText.text = mensaje;

                    // Comprobar si el equipo ya esta negociando
                    bool enNegociacion = TransferenciaData.ComprobarOfertaActiva(jugador.IdJugador, miEquipo.IdEquipo, jugador.IdEquipo);
                    Transferencia ofertaRechazada = new Transferencia
                    {
                        IdJugador = oferta.IdJugador,
                        IdEquipoOrigen = JugadorData.MostrarDatosJugador(oferta.IdJugador).IdEquipo,
                        IdEquipoDestino = miEquipo.IdEquipo,
                        TipoFichaje = 1,
                        MontoOferta = ofertaEquipo,
                        FechaOferta = FechaData.hoy.ToString("yyyy-MM-dd"),
                        FechaTraspaso = "",
                        RespuestaEquipo = 0,
                        RespuestaJugador = 0,
                        SalarioAnual = 0,
                        ClausulaRescision = 0,
                        Duracion = 0,
                        BonoPorGoles = 0,
                        BonoPorPartidos = 0,
                    };


                    if (enNegociacion != true)
                    {
                        TransferenciaData.RegistrarOferta(ofertaRechazada);
                    }
                    else
                    {
                        TransferenciaData.ActualizarOferta(ofertaRechazada);
                    }

                    // No querer negociar en 2 semanas
                    JugadorData.NegociacionCancelada(oferta.IdJugador, 14);

                    // Crear el mensaje confirmando el rechazo de la oferta de traspaso
                    Empleado? director = EmpleadoData.ObtenerEmpleadoPorPuesto("Director Técnico");
                    string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;
                    string nombreJugador = JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto;

                    Mensaje mensajeEquipoAceptaOferta = new Mensaje
                    {
                        Fecha = FechaData.hoy,
                        Remitente = nombreJugador != null ? nombreJugador : director.Nombre,
                        Asunto = "Oferta rechazada",
                        Contenido = $"El {EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre.ToUpper()} ha rechazado nuestra oferta de {ofertaEquipo.ToString("N0", new CultureInfo("es-ES"))}€ por {JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto.ToUpper()}. El club considera que las condiciones económicas ofrecidas no se ajustan a sus expectativas o al valor que otorgan al futbolista en su plantilla.\n\nSi deseas, podemos revisar y ajustar la propuesta para intentar una nueva ofensiva dentro de 2 semanas, o explorar otras opciones en el mercado que se ajusten a las necesidades del equipo.",
                        TipoMensaje = "Notificación",
                        IdEquipo = miEquipo.IdEquipo,
                        Leido = false,
                        Icono = jugador.IdJugador
                    };

                    MensajeData.CrearMensaje(mensajeEquipoAceptaOferta);

                    btnEnviarOferta.style.display = DisplayStyle.None;
                    btnCancelarOferta.text = "ABANDONAR NEGOCIACIÓN";
                    btnCesion.SetEnabled(false);
                    btnMenos.SetEnabled(false);
                    btnMenosMenos.SetEnabled(false);
                    btnMas.SetEnabled(false);
                    btnMasMas.SetEnabled(false);
                }

                void OnCerrarClick()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);

                    btnCerrar.clicked -= OnCerrarClick;
                    popupContainer.style.display = DisplayStyle.None;

                    UIManager.Instance.CargarPantalla("UI/Ficha/Ficha", instancia =>
                    {
                        new FichaJugador(instancia, miEquipo, miManager, jugador.IdJugador, -1, mainScreen);
                    });
                }

                btnCerrar.clicked += OnCerrarClick;
            };

            // Botones lentos
            btnMenos.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0) return;

                AudioManager.Instance.PlaySFX(clickSFX);
                ModificarCantidad(-Paso);

                decrementoSchedule = btnMenos.schedule.Execute(() =>
                {
                    ModificarCantidad(-Paso);
                }).Every(120).StartingIn(200);

            }, TrickleDown.TrickleDown);

            btnMenos.RegisterCallback<MouseUpEvent>(_ =>
            {
                decrementoSchedule?.Pause();
            }, TrickleDown.TrickleDown);

            btnMenos.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                decrementoSchedule?.Pause();
            }, TrickleDown.TrickleDown);

            btnMas.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0) return;

                AudioManager.Instance.PlaySFX(clickSFX);
                ModificarCantidad(+Paso);

                // Repetición solo después de un pequeño delay
                incrementoSchedule = btnMas.schedule.Execute(() =>
                {
                    ModificarCantidad(+Paso);
                }).Every(120).StartingIn(200);

            }, TrickleDown.TrickleDown);

            btnMas.RegisterCallback<MouseUpEvent>(_ =>
            {
                incrementoSchedule?.Pause();
            }, TrickleDown.TrickleDown);

            btnMas.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                incrementoSchedule?.Pause();
            }, TrickleDown.TrickleDown);

            // Botones rápidos
            btnMenosMenos.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0) return;

                AudioManager.Instance.PlaySFX(clickSFX);
                ModificarCantidad(-PasoRapido);

                decrementoSchedule = btnMenos.schedule.Execute(() =>
                {
                    ModificarCantidad(-PasoRapido);
                }).Every(120).StartingIn(200);

            }, TrickleDown.TrickleDown);

            btnMenosMenos.RegisterCallback<MouseUpEvent>(_ =>
            {
                decrementoSchedule?.Pause();
            }, TrickleDown.TrickleDown);

            btnMenosMenos.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                decrementoSchedule?.Pause();
            }, TrickleDown.TrickleDown);

            btnMasMas.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0) return;

                AudioManager.Instance.PlaySFX(clickSFX);
                ModificarCantidad(+PasoRapido);

                incrementoSchedule = btnMas.schedule.Execute(() =>
                {
                    ModificarCantidad(+PasoRapido);
                }).Every(120).StartingIn(200);

            }, TrickleDown.TrickleDown);

            btnMasMas.RegisterCallback<MouseUpEvent>(_ =>
            {
                incrementoSchedule?.Pause();
            }, TrickleDown.TrickleDown);

            btnMasMas.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                incrementoSchedule?.Pause();
            }, TrickleDown.TrickleDown);
        }

        private void ModificarCantidad(int delta)
        {
            cantidadActual = Math.Clamp(
                cantidadActual + delta,
                MinCantidad,
                MaxCantidad
            );

            ActualizarLabelCantidad();
        }

        private void CargarZonaOfertas()
        {
            // Cambiar el texto según el tipo de moneda
            divisa.text = $"CANTIDAD OFRECIDA (EN {Constants.nombreMoneda().ToUpper()})";

            // Cargar la cantidad por defecto dependiendo del valor del jugador
            decimal valorBase = (decimal)jugador.ValorMercado / 2m;
            cantidadActual = (int)(
                Math.Round(valorBase / Paso, MidpointRounding.AwayFromZero) * Paso
            );

            ActualizarLabelCantidad();
        }

        private void CargarDatosJugador()
        {
            bool ojeado = OjearData.ComprobarJugadorOjeado(jugador.IdJugador);

            var caraSprite = Resources.Load<Sprite>($"Jugadores/{jugador.IdJugador}");
            if (caraSprite != null)
                foto.style.backgroundImage = new StyleBackground(caraSprite);

            dorsal.text = $"{jugador.Dorsal}";
            nombreJugador.text = $"{jugador.NombreCompleto}";
            media.style.color = DeterminarColor(jugador.Media);
            media.text = $"{jugador.Media}";
            posicion.text = $"{jugador.Rol}";

            if (ojeado)
            {
                switch (jugador.Status)
                {
                    case 1:
                        rol.text = "Clave";
                        break;
                    case 2:
                        rol.text = "Importante";
                        break;
                    case 3:
                        rol.text = "Rotación";
                        break;
                    case 4:
                        rol.text = "Ocasional";
                        break;
                }
                salario.text = $"{Constants.CambioDivisaNullable(jugador.SalarioTemporada):N0} {CargarSimboloMoneda()}";
                clausula.text = $"{Constants.CambioDivisaNullable(jugador.ClausulaRescision):N0} {CargarSimboloMoneda()}";
                anios.text = $"{jugador.AniosContrato}";
                valor.text = $"{Constants.CambioDivisa(jugador.ValorMercado):N0} {CargarSimboloMoneda()}";
            }
            else
            {
                rol.text = "?";
                salario.text = "?";
                clausula.text = "?";
                anios.text = "?";
                valor.text = "?";
            }
        }

        private void CargarDatosEquipo()
        {
            var escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/{jugador.IdEquipo}");
            if (escudoSprite != null)
                escudo.style.backgroundImage = new StyleBackground(escudoSprite);

            nombreEquipo.text = $"{EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre}";
        }

        private void ActualizarLabelCantidad()
        {
            cantidad.text = $"{cantidadActual:N0} {CargarSimboloMoneda()}";
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
    }
}