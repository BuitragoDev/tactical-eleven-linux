using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

namespace TacticalEleven.Scripts
{
    public class NegociacionesOfertaJugador
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        private Jugador jugador;
        private MainScreen mainScreen;
        int porcNegociacion = 0;
        int porcPaciencia = 100;
        private int _tipoNegociacion;
        private int cantidadActualSalario = 0, cantidadActualClausula = 0, cantidadActualAnios = 1,
                    cantidadActualRol = 1, cantidadActualBonusPartidos = 0, cantidadActualBonusGoles = 0;
        private const int PasoBonus = 10000;
        private const int PasoBonusRapido = 50000;
        private const int Paso = 100000;
        private const int PasoRapido = 2000000;
        private const int PasoLento = 1;
        private const int SalarioMinCantidad = 0;
        private const int SalarioMaxCantidad = 50_000_000;
        private const int ClausulaMinCantidad = 0;
        private const int ClausulaMaxCantidad = 1_000_000_000;
        private const int AniosMinCantidad = 1;
        private const int AniosMaxCantidad = 6;
        private const int RolMinCantidad = 1;
        private const int RolMaxCantidad = 4;
        private const int BonusPartidosMinCantidad = 0;
        private const int BonusPartidosMaxCantidad = 5_000_000;
        private const int BonusGolesMinCantidad = 0;
        private const int BonusGolesMaxCantidad = 5_000_000;

        IVisualElementScheduledItem incrementoSchedule;
        IVisualElementScheduledItem decrementoSchedule;

        private VisualElement root, barraNegociacion, barraPaciencia, foto, escudo, popupContainer;
        private Label dorsal, nombreJugador, porcentajeNegociacion, porcentajePaciencia, estadoNegociacion,
                      demandaSalario, demandaClausula, demandaAnios, demandaRol, media, nombreEquipo, posicion, rol, salario, clausula, anios,
                      ofertaSalario, ofertaClausula, ofertaAnios, ofertaRol, ofertaBonusPartidos, ofertaBonusGoles, popupText;
        private Toggle cbBonusPartidos, cbBonusGoles;
        private Button salarioMenos, salarioMenosMenos, salarioMas, salarioMasMas,
                       clausulaMenos, clausulaMenosMenos, clausulaMas, clausulaMasMas,
                       aniosMenos, aniosMas, rolMenos, rolMas, bonusPartidosMenos, bonusPartidosMenosMenos, bonusPartidosMas, bonusPartidosMasMas,
                       bonusGolesMenos, bonusGolesMenosMenos, bonusGolesMas, bonusGolesMasMas, btnEnviar, btnIgualar, btnCancelar, btnConfirmar,
                       btnCerrar;

        public NegociacionesOfertaJugador(VisualElement rootInstance, Equipo equipo, Manager manager, Jugador jug, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            jugador = jug;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            if (jugador.IdEquipo == miEquipo.IdEquipo)
                _tipoNegociacion = 1;
            else
                _tipoNegociacion = 2;

            // Referencias a objetos de la UI
            barraNegociacion = root.Q<VisualElement>("barra-negociacion");
            barraPaciencia = root.Q<VisualElement>("barra-paciencia");
            foto = root.Q<VisualElement>("foto");
            escudo = root.Q<VisualElement>("escudo");
            dorsal = root.Q<Label>("dorsal");
            nombreJugador = root.Q<Label>("nombre-jugador");
            porcentajeNegociacion = root.Q<Label>("porcentaje-negociacion");
            porcentajePaciencia = root.Q<Label>("porcentaje-paciencia");
            estadoNegociacion = root.Q<Label>("estado-negociacion");
            demandaSalario = root.Q<Label>("demanda-salario");
            demandaClausula = root.Q<Label>("demanda-clausula");
            demandaAnios = root.Q<Label>("demanda-anios");
            demandaRol = root.Q<Label>("demanda-rol");
            media = root.Q<Label>("media");
            nombreEquipo = root.Q<Label>("nombre-equipo");
            posicion = root.Q<Label>("posicion");
            rol = root.Q<Label>("rol");
            salario = root.Q<Label>("salario");
            clausula = root.Q<Label>("clausula");
            anios = root.Q<Label>("anios");
            ofertaSalario = root.Q<Label>("oferta-salario");
            ofertaClausula = root.Q<Label>("oferta-clausula");
            ofertaAnios = root.Q<Label>("oferta-anios");
            ofertaRol = root.Q<Label>("oferta-rol");
            ofertaBonusPartidos = root.Q<Label>("oferta-bonus-partidos");
            ofertaBonusGoles = root.Q<Label>("oferta-bonus-goles");
            cbBonusPartidos = root.Q<Toggle>("cbBonusPartidos");
            cbBonusGoles = root.Q<Toggle>("cbBonusGoles");
            salarioMenos = root.Q<Button>("salarioMenos");
            salarioMenosMenos = root.Q<Button>("salarioMenosMenos");
            salarioMas = root.Q<Button>("salarioMas");
            salarioMasMas = root.Q<Button>("salarioMasMas");
            clausulaMenos = root.Q<Button>("clausulaMenos");
            clausulaMenosMenos = root.Q<Button>("clausulaMenosMenos");
            clausulaMas = root.Q<Button>("clausulaMas");
            clausulaMasMas = root.Q<Button>("clausulaMasMas");
            aniosMenos = root.Q<Button>("aniosMenos");
            aniosMas = root.Q<Button>("aniosMas");
            rolMenos = root.Q<Button>("rolMenos");
            rolMas = root.Q<Button>("rolMas");
            bonusPartidosMenos = root.Q<Button>("bonusPartidosMenos");
            bonusPartidosMenosMenos = root.Q<Button>("bonusPartidosMenosMenos");
            bonusPartidosMas = root.Q<Button>("bonusPartidosMas");
            bonusPartidosMasMas = root.Q<Button>("bonusPartidosMasMas");
            bonusGolesMenos = root.Q<Button>("bonusGolesMenos");
            bonusGolesMenosMenos = root.Q<Button>("bonusGolesMenosMenos");
            bonusGolesMas = root.Q<Button>("bonusGolesMas");
            bonusGolesMasMas = root.Q<Button>("bonusGolesMasMas");
            btnEnviar = root.Q<Button>("btnEnviar");
            btnIgualar = root.Q<Button>("btnIgualar");
            btnCancelar = root.Q<Button>("btnCancelar");
            btnConfirmar = root.Q<Button>("btnConfirmar");
            btnConfirmar.style.display = DisplayStyle.None;

            popupContainer = root.Q<VisualElement>("popup-container");
            btnCerrar = root.Q<Button>("btnCerrar");
            popupText = root.Q<Label>("popup-text");

            // Actualizar valores y moneda al iniciar
            ActualizarLabel(cantidadActualSalario, ofertaSalario);
            ActualizarLabel(cantidadActualClausula, ofertaClausula);
            ActualizarLabelAnios(cantidadActualAnios, ofertaAnios);
            ActualizarLabel(cantidadActualBonusPartidos, ofertaBonusPartidos);
            ActualizarLabel(cantidadActualBonusGoles, ofertaBonusGoles);
            ActualizarLabelRol(cantidadActualRol, ofertaRol);

            // Cargar Porcentajes
            CargarPorcentajes();

            // Comprobar si el jugador ha sido ojeado
            Transferencia jugadorConOferta = TransferenciaData.MostrarDetallesOferta(jugador.IdJugador);

            if (jugador.IdEquipo == miEquipo.IdEquipo || jugadorConOferta != null)
            {
                CargarDatosJugador();
                CargarDemanda();
            }
            else
            {
                if (jugador.IdEquipo == 0)
                {
                    CargarDatosVaciosJugador();

                    demandaSalario.text = $"{Constants.CambioDivisaNullable(JugadorData.SalarioMedioJugadores(jugador.IdJugador)):N0} {CargarSimboloMoneda()}";
                    demandaClausula.text = $"{Constants.CambioDivisaNullable(JugadorData.ClausulaMediaJugadores(jugador.IdJugador)):N0} {CargarSimboloMoneda()}";
                    if (jugador.Edad > 30)
                    {
                        demandaAnios.text = "1";
                    }
                    else
                    {
                        demandaAnios.text = "3";
                    }
                    demandaRol.text = "Rotación";
                    cbBonusGoles.value = false;
                    cbBonusPartidos.value = false;
                }
                else
                {
                    CargarDatosVaciosJugador();
                    demandaSalario.text = "";
                    demandaClausula.text = "";
                    demandaAnios.text = "";
                    demandaRol.text = "";
                    cbBonusGoles.value = false;
                    cbBonusPartidos.value = false;
                }
            }

            // Botones Salario
            ConfigurarBotonRepetible(salarioMenos, () => cantidadActualSalario, v => cantidadActualSalario = v, -Paso, SalarioMinCantidad, SalarioMaxCantidad, v => ActualizarLabel(v, ofertaSalario));
            ConfigurarBotonRepetible(salarioMas, () => cantidadActualSalario, v => cantidadActualSalario = v, +Paso, SalarioMinCantidad, SalarioMaxCantidad, v => ActualizarLabel(v, ofertaSalario));
            ConfigurarBotonRepetible(salarioMenosMenos, () => cantidadActualSalario, v => cantidadActualSalario = v, -PasoRapido, SalarioMinCantidad, SalarioMaxCantidad, v => ActualizarLabel(v, ofertaSalario));
            ConfigurarBotonRepetible(salarioMasMas, () => cantidadActualSalario, v => cantidadActualSalario = v, +PasoRapido, SalarioMinCantidad, SalarioMaxCantidad, v => ActualizarLabel(v, ofertaSalario));

            // Botones Clausula
            ConfigurarBotonRepetible(clausulaMenos, () => cantidadActualClausula, v => cantidadActualClausula = v, -Paso, ClausulaMinCantidad, ClausulaMaxCantidad, v => ActualizarLabel(v, ofertaClausula));
            ConfigurarBotonRepetible(clausulaMas, () => cantidadActualClausula, v => cantidadActualClausula = v, +Paso, ClausulaMinCantidad, ClausulaMaxCantidad, v => ActualizarLabel(v, ofertaClausula));
            ConfigurarBotonRepetible(clausulaMenosMenos, () => cantidadActualClausula, v => cantidadActualClausula = v, -PasoRapido, ClausulaMinCantidad, ClausulaMaxCantidad, v => ActualizarLabel(v, ofertaClausula));
            ConfigurarBotonRepetible(clausulaMasMas, () => cantidadActualClausula, v => cantidadActualClausula = v, +PasoRapido, ClausulaMinCantidad, ClausulaMaxCantidad, v => ActualizarLabel(v, ofertaClausula));

            // Botones Años de Contrato
            ConfigurarBotonRepetible(aniosMenos, () => cantidadActualAnios, v => cantidadActualAnios = v, -PasoLento, AniosMinCantidad, AniosMaxCantidad, v => ActualizarLabelAnios(v, ofertaAnios));
            ConfigurarBotonRepetible(aniosMas, () => cantidadActualAnios, v => cantidadActualAnios = v, +PasoLento, AniosMinCantidad, AniosMaxCantidad, v => ActualizarLabelAnios(v, ofertaAnios));

            // Botones Rol
            ConfigurarBotonRepetible(rolMenos, () => cantidadActualRol, v => cantidadActualRol = v, -PasoLento, RolMinCantidad, RolMaxCantidad, v => ActualizarLabelRol(v, ofertaRol));
            ConfigurarBotonRepetible(rolMas, () => cantidadActualRol, v => cantidadActualRol = v, +PasoLento, RolMinCantidad, RolMaxCantidad, v => ActualizarLabelRol(v, ofertaRol));

            // Botones Bonus Partidos
            ConfigurarBotonRepetible(bonusPartidosMenos, () => cantidadActualBonusPartidos, v => cantidadActualBonusPartidos = v, -PasoBonus, BonusPartidosMinCantidad, BonusPartidosMaxCantidad, v => ActualizarLabel(v, ofertaBonusPartidos));
            ConfigurarBotonRepetible(bonusPartidosMas, () => cantidadActualBonusPartidos, v => cantidadActualBonusPartidos = v, +PasoBonus, BonusPartidosMinCantidad, BonusPartidosMaxCantidad, v => ActualizarLabel(v, ofertaBonusPartidos));
            ConfigurarBotonRepetible(bonusPartidosMenosMenos, () => cantidadActualBonusPartidos, v => cantidadActualBonusPartidos = v, -PasoBonusRapido, BonusPartidosMinCantidad, BonusPartidosMaxCantidad, v => ActualizarLabel(v, ofertaBonusPartidos));
            ConfigurarBotonRepetible(bonusPartidosMasMas, () => cantidadActualBonusPartidos, v => cantidadActualBonusPartidos = v, +PasoBonusRapido, BonusPartidosMinCantidad, BonusPartidosMaxCantidad, v => ActualizarLabel(v, ofertaBonusPartidos));

            // Botones Bonus Goles
            ConfigurarBotonRepetible(bonusGolesMenos, () => cantidadActualBonusGoles, v => cantidadActualBonusGoles = v, -PasoBonus, BonusGolesMinCantidad, BonusGolesMaxCantidad, v => ActualizarLabel(v, ofertaBonusGoles));
            ConfigurarBotonRepetible(bonusGolesMas, () => cantidadActualBonusGoles, v => cantidadActualBonusGoles = v, +PasoBonus, BonusGolesMinCantidad, BonusGolesMaxCantidad, v => ActualizarLabel(v, ofertaBonusGoles));
            ConfigurarBotonRepetible(bonusGolesMenosMenos, () => cantidadActualBonusGoles, v => cantidadActualBonusGoles = v, -PasoBonusRapido, BonusGolesMinCantidad, BonusGolesMaxCantidad, v => ActualizarLabel(v, ofertaBonusGoles));
            ConfigurarBotonRepetible(bonusGolesMasMas, () => cantidadActualBonusGoles, v => cantidadActualBonusGoles = v, +PasoBonusRapido, BonusGolesMinCantidad, BonusGolesMaxCantidad, v => ActualizarLabel(v, ofertaBonusGoles));

            btnEnviar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                // Limpiar listeners previos para evitar duplicados
                btnCerrar.clicked -= OnCerrarClick;

                CargarEnviarOferta();

                void OnCerrarClick()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);

                    btnCerrar.clicked -= OnCerrarClick;
                    popupContainer.style.display = DisplayStyle.None;
                }

                btnCerrar.clicked += OnCerrarClick;
            };

            btnIgualar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarIgualarOferta();
            };

            btnCancelar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                // Cancelar las negociaciones en la base de datos.
                JugadorData.NegociacionCancelada(jugador.IdJugador, 14);

                UIManager.Instance.CargarPantalla("UI/Ficha/Ficha", instancia =>
                {
                    new FichaJugador(instancia, miEquipo, miManager, jugador.IdJugador, -1, mainScreen);
                });
            };

            btnConfirmar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                CargarConfirmarOferta();
                UIManager.Instance.CargarPantalla("UI/Ficha/Ficha", instancia =>
                {
                    new FichaJugador(instancia, miEquipo, miManager, jugador.IdJugador, -1, mainScreen);
                });
            };
        }

        private void CargarEnviarOferta()
        {
            // Valores demandados por el jugador.
            string textoDemandaSalario2 = demandaSalario.text; // Texto original con puntos y símbolo €
            string textoDemandaSalarioSinSimbolos = System.Text.RegularExpressions.Regex.Replace(textoDemandaSalario2, @"[^\d]", ""); // Elimina todo lo que no sea un dígito
            int demandaSalario2 = int.Parse(textoDemandaSalarioSinSimbolos); // Convierte el texto limpio a int

            string textoDemandaClausula2 = demandaClausula.text; // Texto original con puntos y símbolo €
            string textoDemandaClausulaSinSimbolos = System.Text.RegularExpressions.Regex.Replace(textoDemandaClausula2, @"[^\d]", ""); // Elimina todo lo que no sea un dígito
            int demandaClausula2 = int.Parse(textoDemandaClausulaSinSimbolos); // Convierte el texto limpio a int

            string textoAnios2 = demandaAnios.text; // Texto original con años
            string textoSoloNumeros = System.Text.RegularExpressions.Regex.Replace(textoAnios2, @"[^\d]", ""); // Elimina todo lo que no sea un dígito
            int demandaAnios2 = int.Parse(textoSoloNumeros); // Convierte el texto limpio a int

            string demandaRol2 = demandaRol.text;
            int? demandaBonusPartidos2 = cbBonusPartidos.value == true ? ((jugador.SalarioTemporada / 38) * 13) : 0;
            int? demandaBonusGoles2 = cbBonusGoles.value == true ? (jugador.SalarioTemporada / 50) : 0;

            // Valores ofrecidos por el equipo.
            int ofertaSalario2 = ParseCantidad(ofertaSalario);
            int ofertaClausula2 = ParseCantidad(ofertaClausula);
            int ofertaBonusPartidos2 = ParseCantidad(ofertaBonusPartidos);
            int ofertaBonusGoles2 = ParseCantidad(ofertaBonusGoles);
            int ofertaAnios2 = int.Parse(ofertaAnios.text);
            string ofertaRol2 = ofertaRol.text;

            int satisfaccionTotal = CalcularPorcentajeSatisfaccion(demandaSalario2, demandaClausula2, demandaAnios2, demandaRol2, demandaBonusPartidos2, demandaBonusGoles2,
                                                                   ofertaSalario2, ofertaClausula2, ofertaAnios2, ofertaRol2, ofertaBonusPartidos2, ofertaBonusGoles2);

            // Mostrar en la barra de progreso del Estado de la Negociación
            barraNegociacion.style.width = Length.Percent(porcNegociacion);
            porcNegociacion = satisfaccionTotal;
            porcentajeNegociacion.text = $"{satisfaccionTotal}%";
            if (satisfaccionTotal == 100)
            {
                estadoNegociacion.text = "El jugador acepta la oferta realizada.";
                barraNegociacion.style.backgroundColor = new Color(0.0f, 0.6f, 0.0f); // Verde oscuro

                porcPaciencia = porcPaciencia - 0;

                CambiarAnchoBarras();
                ColorBarraPaciencia((int)porcPaciencia);
            }
            else if (satisfaccionTotal >= 85)
            {
                estadoNegociacion.text = "El jugador está muy contento con la oferta.";
                barraNegociacion.style.backgroundColor = new Color(0.4f, 0.8f, 0.4f); // Verde claro

                porcPaciencia = porcPaciencia - 10;

                CambiarAnchoBarras();
                ColorBarraPaciencia((int)porcPaciencia);
            }
            else if (satisfaccionTotal >= 60)
            {
                estadoNegociacion.text = "El jugador está receptivo, pero espera algo más.";
                barraNegociacion.style.backgroundColor = new Color(0.9f, 0.5f, 0.0f); // Naranja oscuro

                porcPaciencia = porcPaciencia - 15;

                CambiarAnchoBarras();
                ColorBarraPaciencia((int)porcPaciencia);
            }
            else if (satisfaccionTotal >= 50)
            {
                estadoNegociacion.text = "El jugador considera alejada de sus pretensiones.";
                barraNegociacion.style.backgroundColor = new Color(1.0f, 0.65f, 0.0f); // Naranja

                porcPaciencia = porcPaciencia - 25;

                CambiarAnchoBarras();
                ColorBarraPaciencia((int)porcPaciencia);
            }
            else
            {
                estadoNegociacion.text = "El jugador considera la oferta ridícula.";
                barraNegociacion.style.backgroundColor = new Color(0.7f, 0.0f, 0.0f); // Rojo oscuro

                porcPaciencia = porcPaciencia - 50;

                CambiarAnchoBarras();
                ColorBarraPaciencia((int)porcPaciencia);
            }
            porcentajePaciencia.text = porcPaciencia + "%";

            // Comprobar si el estado de la negociación ha llegado a 100% de interés del jugador.
            if (satisfaccionTotal == 100)
            {

                List<Button> botonesCantidades = new List<Button> { salarioMenos, salarioMenosMenos, salarioMas, salarioMasMas,
                       clausulaMenos, clausulaMenosMenos, clausulaMas, clausulaMasMas,
                       aniosMenos, aniosMas, rolMenos, rolMas, bonusPartidosMenos, bonusPartidosMenosMenos, bonusPartidosMas, bonusPartidosMasMas,
                       bonusGolesMenos, bonusGolesMenosMenos, bonusGolesMas, bonusGolesMasMas, btnIgualar };

                foreach (Button boton in botonesCantidades)
                {
                    boton.SetEnabled(false);
                }
                btnEnviar.style.display = DisplayStyle.None;
                btnConfirmar.style.display = DisplayStyle.Flex;

                // Ventana emergente diciendo que el jugador ha llegado a un acuerdo.
                popupContainer.style.display = DisplayStyle.Flex;
                string nombreJugador = jugador?.NombreCompleto ?? "El jugador";
                popupText.text = $"{nombreJugador.ToUpper()} y el {EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Nombre.ToUpper()} han llegado a un acuerdo.";

                // Si es un jugador sin equipo registramos la oferta aceptada
                if (jugador.IdEquipo == 0)
                {
                    bool enNegociacion = TransferenciaData.ComprobarOfertaActiva(jugador.IdJugador, miEquipo.IdEquipo, jugador.IdEquipo);

                    // Verificar disponibilidad para la cesion.
                    DateTime hoy = FechaData.hoy;

                    // Ventanas de fichajes
                    DateTime inicioVerano = new DateTime(hoy.Year, 7, 1);
                    DateTime finVerano = new DateTime(hoy.Year, 8, 30);
                    DateTime inicioInvierno = new DateTime(hoy.Year, 1, 1);
                    DateTime finInvierno = new DateTime(hoy.Year, 1, 31);

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
                        TipoFichaje = 1,
                        FechaOferta = FechaData.hoy.ToString("yyyy-MM-dd"),
                        FechaTraspaso = fechaTraspaso.ToString("yyyy-MM-dd"),
                        RespuestaEquipo = 1,
                        RespuestaJugador = 1,
                        MontoOferta = 0,
                        SalarioAnual = ofertaSalario2,
                        ClausulaRescision = ofertaClausula2,
                        Duracion = ofertaAnios2,
                        BonoPorGoles = ofertaBonusGoles2,
                        BonoPorPartidos = ofertaBonusPartidos2,
                    };

                    if (enNegociacion != true)
                    {
                        TransferenciaData.RegistrarOferta(oferta);
                    }
                    else
                    {
                        TransferenciaData.ActualizarOferta(oferta);
                    }
                }
            }

            // Comprobar si la paciencia del jugador ha llegado a cero y no dejarle hacer mas ofertas.
            if (porcPaciencia == 0)
            {
                List<Button> botonesCantidades2 = new List<Button> { salarioMenos, salarioMenosMenos, salarioMas, salarioMasMas,
                       clausulaMenos, clausulaMenosMenos, clausulaMas, clausulaMasMas,
                       aniosMenos, aniosMas, rolMenos, rolMas, bonusPartidosMenos, bonusPartidosMenosMenos, bonusPartidosMas, bonusPartidosMasMas,
                       bonusGolesMenos, bonusGolesMenosMenos, bonusGolesMas, bonusGolesMasMas, btnEnviar, btnIgualar };

                foreach (Button boton in botonesCantidades2)
                {
                    boton.SetEnabled(false);
                }

                // Ventana emergente diciendo que el jugador ya no quiere negociar.
                popupContainer.style.display = DisplayStyle.Flex;
                string nombreJugador = jugador?.NombreCompleto ?? "El jugador";
                popupText.text = nombreJugador.ToUpper() + " se ha cansado de negociar contigo y ha decidido no volver a reunirse contigo durante las próximas 2 semanas.";

                // Si es un jugador sin equipo registramos la oferta rechazada
                if (jugador.IdEquipo == 0)
                {
                    bool enNegociacion = TransferenciaData.ComprobarOfertaActiva(jugador.IdJugador, miEquipo.IdEquipo, jugador.IdEquipo);

                    Transferencia oferta = new Transferencia
                    {
                        IdJugador = jugador.IdJugador,
                        IdEquipoOrigen = JugadorData.MostrarDatosJugador(jugador.IdJugador).IdEquipo,
                        IdEquipoDestino = miEquipo.IdEquipo,
                        TipoFichaje = 1,
                        MontoOferta = 0,
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
                        TransferenciaData.RegistrarOferta(oferta);
                    }
                    else
                    {
                        TransferenciaData.ActualizarOferta(oferta);
                    }
                }

                // Cancelar las negociaciones en la base de datos.
                JugadorData.NegociacionCancelada(jugador.IdJugador, 14);

                // Crear el mensaje confirmando que el jugador rechaza la oferta de traspaso
                Empleado? director = EmpleadoData.ObtenerEmpleadoPorPuesto("Director Técnico");
                string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;

                Mensaje mensajeJugadorRechazaOferta = new Mensaje
                {
                    Fecha = FechaData.hoy,
                    Remitente = nombreJugador != null ? nombreJugador : director.Nombre,
                    Asunto = "Negociación fallida",
                    Contenido = $"Lamentablemente, no ha sido posible llegar a un acuerdo con {nombreJugador.ToUpper()} tras la negociación de los términos contractuales.\n\nEl jugador y su representante han rechazado nuestra propuesta y han decidido no continuar con las conversaciones. Esto significa que, por el momento, el fichaje queda descartado.\n\nPuedes optar por realizar una nueva oferta en 2 semanas para modificar las condiciones o centrarte en otras alternativas disponibles en el mercado.",
                    TipoMensaje = "Notificación",
                    IdEquipo = miEquipo.IdEquipo,
                    Leido = false,
                    Icono = jugador.IdJugador
                };

                MensajeData.CrearMensaje(mensajeJugadorRechazaOferta);

                btnCerrar.clicked += () =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    popupContainer.style.display = DisplayStyle.None;
                };
            }
        }

        private void CambiarAnchoBarras()
        {
            barraNegociacion.style.width = Length.Percent(porcNegociacion);
            barraPaciencia.style.width = Length.Percent(porcPaciencia);
        }

        private void CargarDemanda()
        {
            DemandaContrato demanda = GenerarDemandaContrato(jugador);

            demandaSalario.text = $"{Constants.CambioDivisaNullable(demanda.SalarioDeseado):N0} {CargarSimboloMoneda()}";
            demandaClausula.text = $"{Constants.CambioDivisaNullable(demanda.ClausulaDeseada):N0} {CargarSimboloMoneda()}";
            demandaAnios.text = demanda.DuracionContrato.ToString();
            demandaRol.text = ObtenerNombreRol(demanda.RolDeseado);

            // Bonuses
            int? bonusGolesActual = jugador.BonusGoles;

            if (jugador.RolId >= 7)
            { // Si es un jugador atacante pedirá clausula por goles
                cbBonusGoles.value = true;
            }
            if (bonusGolesActual > 0)
            { // Si el jugador ya tiene clausula por goles también la pedirá
                cbBonusGoles.value = true;
            }

            if (jugador.Status >= 3)
            { // Si el jugador no es titular pedirá la claúsula
                cbBonusPartidos.value = true;
            }
        }

        public DemandaContrato GenerarDemandaContrato(Jugador jugador)
        {
            DemandaContrato demanda = new DemandaContrato();

            // ----------------------------------------------------------------------- SALARIO
            int salarioActual = jugador.SalarioTemporada ?? 1000000;
            double porcentajeAjuste;

            // Ajuste según edad: los jóvenes piden más, los veteranos menos
            if (jugador.Edad <= 22)
                porcentajeAjuste = 1.05; // Pide un 5% más
            else if (jugador.Edad <= 27)
                porcentajeAjuste = 1.15; // Pide un 15% más
            else if (jugador.Edad <= 31)
                porcentajeAjuste = 1.05; // Pide un 5% más
            else if (jugador.Edad <= 34)
                porcentajeAjuste = 0.90; // Acepta un 10% menos
            else
                porcentajeAjuste = 0.75; // Acepta un 25% menos

            // Ajuste adicional por media
            if (jugador.Media >= 90)
                porcentajeAjuste += 0.2;
            else if (jugador.Media >= 85)
                porcentajeAjuste += 0.1;
            else if (jugador.Media >= 80)
                porcentajeAjuste += 0.05;

            demanda.SalarioDeseado = (int)(salarioActual * porcentajeAjuste);

            // ----------------------------------------------------------------------- CLAUSULA DE RESCISION
            // FACTORES
            double factorEdad;
            if (jugador.Edad <= 21)
                factorEdad = 1.1;
            else if (jugador.Edad <= 25)
                factorEdad = 1.2;
            else if (jugador.Edad <= 30)
                factorEdad = 1.0;
            else if (jugador.Edad <= 34)
                factorEdad = 0.7;
            else
                factorEdad = 0.5;

            double factorRol;
            switch (jugador.Status)
            {
                case 1: // Clave
                    factorRol = 1.5;
                    break;
                case 2: // Importante
                    factorRol = 1.2;
                    break;
                case 3: // Rotación
                    factorRol = 1.0;
                    break;
                case 4: // Ocasional
                    factorRol = 0.8;
                    break;
                default:
                    factorRol = 1.0;
                    break;
            }

            // CÁLCULO FINAL
            int clausulaMaxima = 1_000_000_000;
            int clausulaAnterior = jugador.ClausulaRescision ?? 1000000;
            int clausulaBase = (int)(clausulaAnterior * factorEdad * factorRol);

            // Aplica el límite máximo
            demanda.ClausulaDeseada = Math.Min(clausulaBase, clausulaMaxima);

            // ----------------------------------------------------------------------- DURACION DEL CONTRATO
            if (jugador.Edad < 24)
                demanda.DuracionContrato = 5;
            else if (jugador.Edad < 28)
                demanda.DuracionContrato = 4;
            else if (jugador.Edad < 32)
                demanda.DuracionContrato = 3;
            else
                demanda.DuracionContrato = 2;

            // ----------------------------------------------------------------------- ROL DESEADO
            int valor = jugador.Media;
            if (valor >= 88)
                demanda.RolDeseado = 1; // Clave
            else if (valor >= 80)
                demanda.RolDeseado = 2; // Importante
            else if (valor >= 70)
                demanda.RolDeseado = 3; // Rotación
            else
                demanda.RolDeseado = 4; // Ocasional

            return demanda;
        }

        private void CargarDatosJugador()
        {
            dorsal.text = $"{jugador.Dorsal}";
            nombreJugador.text = $"{jugador.NombreCompleto}";
            nombreEquipo.text = $"{EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre}";
            posicion.text = $"{jugador.Rol}";
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

            var caraSprite = Resources.Load<Sprite>($"Jugadores/{jugador.IdJugador}");
            if (caraSprite != null)
                foto.style.backgroundImage = new StyleBackground(caraSprite);

            var escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/{jugador.IdEquipo}");
            if (escudoSprite != null)
                escudo.style.backgroundImage = new StyleBackground(escudoSprite);

            media.style.color = DeterminarColor(jugador.Media);
            media.text = $"{jugador.Media}";
        }

        private void CargarDatosVaciosJugador()
        {
            dorsal.text = $"{jugador.Dorsal}";
            nombreJugador.text = $"{jugador.NombreCompleto}";
            nombreEquipo.text = $"{EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre}";
            posicion.text = $"{jugador.Rol}";
            rol.text = "";
            salario.text = "";
            clausula.text = "";
            anios.text = "";

            var caraSprite = Resources.Load<Sprite>($"Jugadores/{jugador.IdJugador}");
            if (caraSprite != null)
                foto.style.backgroundImage = new StyleBackground(caraSprite);

            var escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/{jugador.IdEquipo}");
            if (escudoSprite != null)
                escudo.style.backgroundImage = new StyleBackground(escudoSprite);

            media.style.color = DeterminarColor(jugador.Media);
            media.text = $"{jugador.Media}";
        }

        private void CargarIgualarOferta()
        {
            // Salario
            string textoDemandaSalario = demandaSalario.text;

            if (string.IsNullOrEmpty(textoDemandaSalario))
            {
                ofertaSalario.text = "0 ";
            }
            else
            {
                string textoDemandaSalarioSinSimbolos = System.Text.RegularExpressions.Regex.Replace(textoDemandaSalario, @"[^\d]", "");

                if (int.TryParse(textoDemandaSalarioSinSimbolos, out int demandaSalario))
                {
                    ofertaSalario.text = $"{demandaSalario:N0} {CargarSimboloMoneda()}";
                }
                else
                {
                    ofertaSalario.text = "0";
                }
            }

            // Claúsula de Rescisión
            string textoDemandaClausula = demandaClausula.text;

            if (string.IsNullOrEmpty(textoDemandaClausula))
            {
                ofertaClausula.text = "0";
            }
            else
            {
                string textoDemandaClausulaSinSimbolos = System.Text.RegularExpressions.Regex.Replace(textoDemandaClausula, @"[^\d]", "");

                if (int.TryParse(textoDemandaClausulaSinSimbolos, out int demandaClausula))
                {
                    ofertaClausula.text = $"{demandaClausula:N0} {CargarSimboloMoneda()}";
                }
                else
                {
                    ofertaClausula.text = "0";
                }
            }


            // Años Contrato
            string textoAnios = demandaAnios.text;

            if (string.IsNullOrEmpty(textoAnios))
            {
                ofertaAnios.text = "1";
            }
            else
            {
                string textoSoloNumeros = System.Text.RegularExpressions.Regex.Replace(textoAnios, @"[^\d]", "");

                if (int.TryParse(textoSoloNumeros, out int demandaAnios))
                {
                    ofertaAnios.text = demandaAnios.ToString();
                }
                else
                {
                    ofertaAnios.text = "1";
                }
            }

            ofertaRol.text = string.IsNullOrEmpty(demandaRol.text) ? "Ocasional" : demandaRol.text;

            int? salario = jugador.SalarioTemporada;

            if (cbBonusPartidos.value == true)
            {
                ofertaBonusPartidos.text = $"{Constants.CambioDivisaNullable(salario / 50):N0} {CargarSimboloMoneda()}";
            }

            if (cbBonusGoles.value == true)
            {
                ofertaBonusGoles.text = $"{Constants.CambioDivisaNullable(salario / 35):N0} {CargarSimboloMoneda()}";
            }
        }

        private void CargarConfirmarOferta()
        {
            // Actualizar el contrato del jugador.
            int ofertaSalario3 = ParseCantidad(ofertaSalario);
            int ofertaClausula3 = ParseCantidad(ofertaClausula);
            int ofertaBonusPartidos3 = ParseCantidad(ofertaBonusPartidos);
            int ofertaBonusGoles3 = ParseCantidad(ofertaBonusGoles);
            int ofertaAnios3 = int.Parse(ofertaAnios.text);
            int ofertaRol3 = 0;
            if (ofertaRol.text == "Clave")
            {
                ofertaRol3 = 1;
            }
            else if (ofertaRol.text == "Importante")
            {
                ofertaRol3 = 2;
            }
            else if (ofertaRol.text == "Rotación")
            {
                ofertaRol3 = 3;
            }
            else
            {
                ofertaRol3 = 4;
            }

            if (_tipoNegociacion == 1)
            {
                JugadorData.RenovarStatusJugador(jugador.IdJugador, ofertaRol3);
                JugadorData.RenovarContratoJugador(jugador.IdJugador, ofertaSalario3, ofertaClausula3, ofertaAnios3, ofertaBonusPartidos3, ofertaBonusGoles3);
                // Cancelar las negociaciones en la base de datos durante 6 meses.
                JugadorData.NegociacionCancelada(jugador.IdJugador, 180);

                // Crear el mensaje con la Renovación de Jugador
                string nombreJugador = jugador.Nombre + " " + jugador.Apellido;
                Mensaje mensajeRenovacion = new Mensaje
                {
                    Fecha = FechaData.hoy,
                    Remitente = nombreJugador != null ? nombreJugador : "Desconocido",
                    Asunto = "Renovación de Jugador",
                    Contenido = $"Has renovado a {jugador.NombreCompleto.ToUpper()}. El nuevo contrato tiene una duración de {ofertaAnios} años, un salario de {ofertaSalario.text} € y su nueva claúsula de rescisión es de {ofertaClausula.text} €.",
                    TipoMensaje = "Respuesta del Jugador",
                    IdEquipo = miEquipo.IdEquipo,
                    Leido = false,
                    Icono = jugador.IdJugador // Distinto de 0 es icono de jugador
                };
                MensajeData.CrearMensaje(mensajeRenovacion);
            }
            else
            {
                bool enNegociacion = TransferenciaData.ComprobarOfertaActiva(jugador.IdJugador, miEquipo.IdEquipo, jugador.IdEquipo);

                // Verificar disponibilidad para la cesion.
                DateTime hoy = FechaData.hoy;

                // Ventanas de fichajes
                DateTime inicioVerano = new DateTime(hoy.Year, 7, 1);
                DateTime finVerano = new DateTime(hoy.Year, 8, 30);
                DateTime inicioInvierno = new DateTime(hoy.Year, 1, 1);
                DateTime finInvierno = new DateTime(hoy.Year, 1, 31);

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
                    TipoFichaje = 1,
                    FechaOferta = FechaData.hoy.ToString("yyyy-MM-dd"),
                    FechaTraspaso = fechaTraspaso.ToString("yyyy-MM-dd"),
                    RespuestaEquipo = 1,
                    RespuestaJugador = 1,
                    MontoOferta = TransferenciaData.MostrarDetallesOferta(jugador.IdJugador).MontoOferta,
                    SalarioAnual = ofertaSalario3,
                    ClausulaRescision = ofertaClausula3,
                    Duracion = ofertaAnios3,
                    BonoPorGoles = ofertaBonusGoles3,
                    BonoPorPartidos = ofertaBonusPartidos3,
                };

                if (enNegociacion != true)
                {
                    TransferenciaData.RegistrarOferta(oferta);
                }
                else
                {
                    TransferenciaData.ActualizarOferta(oferta);
                }

                // Agregar el traspaso a la tabla transferencias
                TransferenciaData.RegistrarTransferencia(oferta);

                // Cancelar las negociaciones en la base de datos durante 6 meses.
                JugadorData.NegociacionCancelada(jugador.IdJugador, 180);

                // Crear el mensaje confirmando que el jugador acepta la oferta de traspaso
                Empleado? director = EmpleadoData.ObtenerEmpleadoPorPuesto("Director Técnico");
                string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;
                string nombreJugador = JugadorData.MostrarDatosJugador(jugador.IdJugador).NombreCompleto;

                Mensaje mensajeJugadorAceptaOferta = new Mensaje
                {
                    Fecha = FechaData.hoy,
                    Remitente = nombreJugador != null ? nombreJugador : director.Nombre,
                    Asunto = "Traspaso confirmado",
                    Contenido = $"{nombreJugador.ToUpper()} ha aceptado los términos personales que le propusimos " +
                                $"y se ha convertido oficialmente en nuevo jugador del {EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre.ToUpper().ToUpper()}.\n\n" +
                                "Los detalles finales del acuerdo son los siguientes:\n\n" +
                                $"• Tipo de operación: Traspaso\n" +
                                $"• Coste del fichaje: {TransferenciaData.MostrarDetallesOferta(jugador.IdJugador).MontoOferta.ToString("N0", new CultureInfo("es-ES"))}€\n" +
                                $"• Duración del contrato: {ofertaAnios3} temporadas\n" +
                                $"• Salario acordado: {ofertaSalario3.ToString("N0", new CultureInfo("es-ES"))}€\n" +
                                $"• Rol en la plantilla: {ofertaRol.text}\n" +
                                $"• Bonus por partidos: {ofertaBonusPartidos3.ToString("N0", new CultureInfo("es-ES"))}€\n" +
                                $"• Bonus por goles: {ofertaBonusGoles3.ToString("N0", new CultureInfo("es-ES"))}€\n\n" +
                                "El jugador se incorporará de inmediato a los entrenamientos y estará disponible para el próximo partido, salvo indicaciones médicas.",
                    TipoMensaje = "Notificación",
                    IdEquipo = miEquipo.IdEquipo,
                    Leido = false,
                    Icono = jugador.IdJugador
                };

                MensajeData.CrearMensaje(mensajeJugadorAceptaOferta);

                // Crear el gasto del fichaje
                int cantidadFichaje = TransferenciaData.MostrarDetallesOferta(jugador.IdJugador).MontoOferta;
                Finanza nuevoGasto = new Finanza
                {
                    IdEquipo = miEquipo.IdEquipo,
                    Temporada = FechaData.temporadaActual.ToString(),
                    IdConcepto = 18,
                    Tipo = 2,
                    Cantidad = cantidadFichaje,
                    Fecha = FechaData.hoy.Date
                };
                FinanzaData.CrearGasto(nuevoGasto);

                // Restar la indemnización al Presupuesto
                EquipoData.RestarCantidadAPresupuesto(miEquipo.IdEquipo, cantidadFichaje);
            }
        }

        private void CargarPorcentajes()
        {
            barraNegociacion.style.width = Length.Percent(porcNegociacion);
            barraPaciencia.style.width = Length.Percent(porcPaciencia);

            porcentajeNegociacion.text = porcNegociacion + "%";
            porcentajePaciencia.text = porcPaciencia + "%";
        }

        private void ConfigurarBotonRepetible(Button boton, Func<int> getter, Action<int> setter, int paso, int min, int max, Action<int> actualizarUI)
        {
            IVisualElementScheduledItem schedule = null;

            boton.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0) return;

                AudioManager.Instance.PlaySFX(clickSFX);

                int nuevoValor = Math.Clamp(getter() + paso, min, max);
                setter(nuevoValor);
                actualizarUI(nuevoValor);

                schedule = boton.schedule.Execute(() =>
                {
                    int v = Math.Clamp(getter() + paso, min, max);
                    setter(v);
                    actualizarUI(v);
                })
                .Every(120)
                .StartingIn(200);

            }, TrickleDown.TrickleDown);

            boton.RegisterCallback<MouseUpEvent>(_ => schedule?.Pause(), TrickleDown.TrickleDown);
            boton.RegisterCallback<MouseLeaveEvent>(_ => schedule?.Pause(), TrickleDown.TrickleDown);
        }

        private void PintarSalario(int v) =>
            ActualizarLabel(v, ofertaSalario);

        private void PintarClausula(int v) =>
            ActualizarLabel(v, ofertaClausula);

        private void PintarAnios(int v) =>
            ActualizarLabelAnios(v, ofertaAnios);

        private void PintarRol(int v) =>
            ActualizarLabelRol(v, ofertaRol);

        private void ActualizarLabel(int cantidad, Label label)
        {
            label.text = $"{cantidad:N0} {CargarSimboloMoneda()}";
        }

        private void ActualizarLabelAnios(int cantidad, Label label)
        {
            label.text = $"{cantidad}";
        }

        private void ActualizarLabelRol(int cantidad, Label label)
        {
            switch (cantidad)
            {
                case 1:
                    label.text = "Clave";
                    break;
                case 2:
                    label.text = "Importante";
                    break;
                case 3:
                    label.text = "Rotación";
                    break;
                case 4:
                    label.text = "Ocasional";
                    break;
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

        private string ObtenerNombreRol(int rolId)
        {
            return rolId switch
            {
                1 => "Clave",
                2 => "Importante",
                3 => "Rotación",
                4 => "Ocasional",
                _ => "Desconocido"
            };
        }

        private int ParseCantidad(Label label)
        {
            // Elimina todo excepto dígitos
            string soloNumeros = Regex.Replace(label.text, @"[^\d]", "");
            return int.TryParse(soloNumeros, out int valor) ? valor : 0;
        }

        private int CalcularPorcentajeSatisfaccion(int demandaSalario, int demandaClausula, int demandaAnios, string demandaRol, int? demandaBonusPartidos, int? demandaBonusGoles,
                                                   int ofertaSalario, int ofertaClausula, int ofertaAnios, string ofertaRol, int ofertaBonusPartidos, int ofertaBonusGoles)
        {
            int porcentajeSatisfaccion = 0;

            Dictionary<string, int> nivelesRol = new Dictionary<string, int>
                                                 {
                                                    { "Clave", 3 },
                                                    { "Importante", 2 },
                                                    { "Rotación", 1 },
                                                    { "Ocasional", 0 }
                                                 };

            // Obtener el nivel de los roles demandado y ofertado
            int nivelDemanda = nivelesRol[demandaRol];
            int nivelOferta = nivelesRol[ofertaRol];

            // Calcular la diferencia de niveles
            int diferenciaRol = nivelOferta - nivelDemanda;

            // Comparar el salario (35%)
            double satisfaccionSalario = ((ofertaSalario * 40) / demandaSalario) + diferenciaRol;
            porcentajeSatisfaccion += (int)satisfaccionSalario;

            // Comparar la cláusula de rescisión (20%)
            double porcentajeOferta = ((double)ofertaClausula / demandaClausula) * 100;

            // Diccionario con los umbrales de satisfacción según el rol
            var satisfaccionPorRolClausula = new Dictionary<string, List<(int umbral, int satisfaccion)>>
                                    {
                                        { "Clave",       new List<(int, int)> { (75, 20), (55, 10), (35, 5), (0, 0) } },
                                        { "Importante",  new List<(int, int)> { (80, 20), (65, 10), (50, 5), (0, 0) } },
                                        { "Rotación",    new List<(int, int)> { (90, 20), (70, 10), (50, 5), (0, 0) } },
                                        { "Ocasional",   new List<(int, int)> { (95, 20), (75, 10), (55, 5), (0, 0) } }
                                    };

            // Buscar el valor de satisfacción correspondiente
            if (satisfaccionPorRolClausula.TryGetValue(demandaRol, out var reglas))
            {
                foreach (var (umbral, sat) in reglas)
                {
                    if (porcentajeOferta >= umbral)
                    {
                        porcentajeSatisfaccion += sat;
                        break;
                    }
                }
            }

            // Comparar los años de contrato (10%)
            int diferencia = ofertaAnios - demandaAnios;

            if (diferencia == 0)
            {
                porcentajeSatisfaccion += 15;
            }
            else if ((demandaRol == "Clave" && diferencia <= 2) ||
                     (demandaRol == "Importante" && diferencia <= 1) ||
                     (demandaRol == "Rotación" && diferencia >= -1) ||
                     (demandaRol == "Ocasional" && diferencia >= -2))
            {
                porcentajeSatisfaccion += 12; // Dentro del rango ideal
            }
            else if ((demandaRol == "Clave" && diferencia <= 3) ||
                       (demandaRol == "Importante" && diferencia <= 2) ||
                       (demandaRol == "Rotación" && diferencia >= -2) ||
                       (demandaRol == "Ocasional" && diferencia >= -3))
            {
                porcentajeSatisfaccion += 5;
            }
            else
            {
                porcentajeSatisfaccion += 0;
            }

            // Comparar el rol (24%)
            int diferenciaPorRol = nivelDemanda - nivelOferta; // Calcular la diferencia de niveles

            var satisfaccionPorRol = new Dictionary<string, List<(int diferenciaPorRol, int satisfaccion)>>
                                    {
                                        { "Clave",       new List<(int, int)> { (0, 24), (1, 20), (int.MaxValue, 10) } },
                                        { "Importante",  new List<(int, int)> { (0, 24), (1, 20), (int.MaxValue, 10) } },
                                        { "Rotación",    new List<(int, int)> { (-2, 30), (0, 24), (int.MaxValue, 10) } },
                                        { "Ocasional",   new List<(int, int)> { (-3, 30), (-2, 30), (-1, 24), (int.MaxValue, 24) } }
                                    };

            // Buscar el valor de satisfacción correspondiente
            if (satisfaccionPorRol.TryGetValue(demandaRol, out var rules))
            {
                foreach (var (dif, sat) in rules)
                {
                    if (diferencia <= dif)
                    {
                        porcentajeSatisfaccion += sat;
                        break;
                    }
                }
            }

            // Comparar bonus por partidos (5%)
            int? diffBonusPartidos = demandaBonusPartidos - ofertaBonusPartidos;

            if (diffBonusPartidos <= 0)
            {
                porcentajeSatisfaccion += 5;
            }
            else
            {
                if (diffBonusPartidos > 0 && diffBonusPartidos < (demandaBonusPartidos * 0.25))
                {
                    porcentajeSatisfaccion += 3;
                }
                else
                {
                    porcentajeSatisfaccion += 0;
                }
            }

            // Comparar bonus por goles (5%)
            int? diffBonusGoles = demandaBonusGoles - ofertaBonusGoles;

            if (diffBonusGoles <= 0)
            {
                porcentajeSatisfaccion += 5;
            }
            else
            {
                if (diffBonusGoles > 0 && diffBonusGoles < (demandaBonusGoles * 0.25))
                {
                    porcentajeSatisfaccion += 3;
                }
                else
                {
                    porcentajeSatisfaccion += 0;
                }
            }

            // Evitamos valores por debajo de 0 o de 100.
            if (porcentajeSatisfaccion < 0)
            {
                porcentajeSatisfaccion = 0;
            }
            else if (porcentajeSatisfaccion > 100)
            {
                porcentajeSatisfaccion = 100;
            }

            return (int)porcentajeSatisfaccion;
        }

        private void ColorBarraPaciencia(int valor)
        {
            Color color =
                valor >= 100 ? new Color(0.0f, 0.6f, 0.0f) :
                valor >= 80 ? new Color(0.4f, 0.8f, 0.4f) :
                valor >= 65 ? new Color(0.9f, 0.5f, 0.0f) :
                valor >= 50 ? new Color(1.0f, 0.65f, 0.0f) :
                               new Color(0.7f, 0.0f, 0.0f);

            barraPaciencia.style.backgroundColor = color;
        }
    }
}