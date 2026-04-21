#nullable enable

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Globalization;

namespace TacticalEleven.Scripts
{
    public class FinanzasTelevision
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        private MainScreen mainScreen;

        int cadenaTVSeleccionada = 0;
        int[]? TelevisionPrincipal;
        int[]? cantidadesPrincipal;
        int[]? mensualidadPrincipal;
        int[]? duracionesPrincipal;

        private VisualElement root, escudo, logoTelevision, logoTelevision1, logoTelevision2, logoTelevision3,
                            seleccionado1, seleccionado2, seleccionado3, popupContainer;
        private Button btnOfertasTelevisiones, btnConfirmarTelevision;
        private Label nombreTelevision, pagoInicial, pagoMensual, duracionContrato, pagoAnticipado1, pagoAnticipado2, pagoAnticipado3,
                      pagoMensual1, pagoMensual2, pagoMensual3, duracion1, duracion2, duracion3;

        public FinanzasTelevision(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            escudo = root.Q<VisualElement>("escudo-equipo");
            popupContainer = root.Q<VisualElement>("popup-container");
            popupContainer.style.display = DisplayStyle.None;
            logoTelevision = root.Q<VisualElement>("logo-cadenatv");
            logoTelevision1 = root.Q<VisualElement>("logo-cadenatv1");
            logoTelevision2 = root.Q<VisualElement>("logo-cadenatv2");
            logoTelevision3 = root.Q<VisualElement>("logo-cadenatv3");
            seleccionado1 = root.Q<VisualElement>("seleccionado1");
            seleccionado2 = root.Q<VisualElement>("seleccionado2");
            seleccionado3 = root.Q<VisualElement>("seleccionado3");
            btnOfertasTelevisiones = root.Q<Button>("btnOfertasTelevision");
            btnConfirmarTelevision = root.Q<Button>("btnConfirmarOfertaTV");
            pagoInicial = root.Q<Label>("pago-inicial");
            pagoMensual = root.Q<Label>("pago-mensual");
            duracionContrato = root.Q<Label>("duracion-contrato");
            pagoAnticipado1 = root.Q<Label>("pago-anticipado1");
            pagoAnticipado2 = root.Q<Label>("pago-anticipado2");
            pagoAnticipado3 = root.Q<Label>("pago-anticipado3");
            pagoMensual1 = root.Q<Label>("pago-mensual1");
            pagoMensual2 = root.Q<Label>("pago-mensual2");
            pagoMensual3 = root.Q<Label>("pago-mensual3");
            duracion1 = root.Q<Label>("duracion1");
            duracion2 = root.Q<Label>("duracion2");
            duracion3 = root.Q<Label>("duracion3");
            nombreTelevision = root.Q<Label>("nombre-cadenatv");

            CargarDatosTelevision();

            btnOfertasTelevisiones.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                popupContainer.style.display = DisplayStyle.Flex;
                CargarOfertasTelevisiones();
                btnOfertasTelevisiones.SetEnabled(false);
            };

            btnConfirmarTelevision.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                TelevisionData.AnadirUnaTelevision(TelevisionPrincipal[cadenaTVSeleccionada - 1], cantidadesPrincipal[cadenaTVSeleccionada - 1], mensualidadPrincipal[cadenaTVSeleccionada - 1], duracionesPrincipal[cadenaTVSeleccionada - 1], miEquipo.IdEquipo);

                // Crear el mensaje
                Empleado? financiero = EmpleadoData.ObtenerEmpleadoPorPuesto("Financiero");
                string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;
                string? nombreCadena = TelevisionData.NombreTelevision(TelevisionPrincipal[cadenaTVSeleccionada - 1]);

                Mensaje mensajeCadena = new Mensaje
                {
                    Fecha = FechaData.hoy,
                    Remitente = financiero != null ? financiero.Nombre : presidente,
                    Asunto = "Acuerdo de Derechos Televisivos Cerrado",
                    Contenido = $"¡Grandes noticias para el club! Hemos cerrado un acuerdo con la cadena {nombreCadena} para la retransmisión de nuestros partidos. Este contrato nos reportará un pago inicial de {cantidadesPrincipal[cadenaTVSeleccionada - 1].ToString("N0", new CultureInfo("es-ES"))}€ y un pago mensual de {mensualidadPrincipal[cadenaTVSeleccionada - 1].ToString("N0", new CultureInfo("es-ES"))}€ por temporada durante {duracionesPrincipal[cadenaTVSeleccionada - 1]} años y aumentará notablemente nuestra visibilidad a nivel nacional e internacional.\r\n\r\nEs un paso importante que no solo mejorará nuestras finanzas, sino también la imagen del club. El creciente interés mediático es un reflejo directo del buen trabajo que estás realizando.",
                    TipoMensaje = "Notificación",
                    IdEquipo = miEquipo.IdEquipo,
                    Leido = false,
                    Icono = 0 // 0 es icono de equipo
                };

                MensajeData.CrearMensaje(mensajeCadena);

                // Crear ingreso del pago inicial de la television
                int pagoTelevision = cantidadesPrincipal[cadenaTVSeleccionada - 1];
                Finanza nuevoIngresoTelevision = new Finanza
                {
                    IdEquipo = miEquipo.IdEquipo,
                    Temporada = FechaData.temporadaActual.ToString(),
                    IdConcepto = 7,
                    Tipo = 1,
                    Cantidad = pagoTelevision,
                    Fecha = FechaData.hoy.Date
                };
                FinanzaData.CrearIngreso(nuevoIngresoTelevision);

                // Restar la indemnización al Presupuesto
                EquipoData.SumarCantidadAPresupuesto(miEquipo.IdEquipo, pagoTelevision);

                popupContainer.style.display = DisplayStyle.None;
                btnOfertasTelevisiones.SetEnabled(true);
                CargarDatosTelevision();
                ActualizarPresupuestoMainScreen();
            };

            seleccionado1.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                cadenaTVSeleccionada = 1;
                Sprite seleccionado1Sprite = Resources.Load<Sprite>($"Icons/seleccionado_icon");
                if (seleccionado1Sprite != null)
                    seleccionado1.style.backgroundImage = new StyleBackground(seleccionado1Sprite);
                Sprite seleccionado2Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado2Sprite != null)
                    seleccionado2.style.backgroundImage = new StyleBackground(seleccionado2Sprite);
                Sprite seleccionado3Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado3Sprite != null)
                    seleccionado3.style.backgroundImage = new StyleBackground(seleccionado3Sprite);
                btnConfirmarTelevision.style.display = DisplayStyle.Flex;
            });

            seleccionado2.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                cadenaTVSeleccionada = 2;
                Sprite seleccionado1Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado1Sprite != null)
                    seleccionado1.style.backgroundImage = new StyleBackground(seleccionado1Sprite);
                Sprite seleccionado2Sprite = Resources.Load<Sprite>($"Icons/seleccionado_icon");
                if (seleccionado2Sprite != null)
                    seleccionado2.style.backgroundImage = new StyleBackground(seleccionado2Sprite);
                Sprite seleccionado3Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado3Sprite != null)
                    seleccionado3.style.backgroundImage = new StyleBackground(seleccionado3Sprite);
                btnConfirmarTelevision.style.display = DisplayStyle.Flex;
            });

            seleccionado3.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                cadenaTVSeleccionada = 3;
                Sprite seleccionado1Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado1Sprite != null)
                    seleccionado1.style.backgroundImage = new StyleBackground(seleccionado1Sprite);
                Sprite seleccionado2Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado2Sprite != null)
                    seleccionado2.style.backgroundImage = new StyleBackground(seleccionado2Sprite);
                Sprite seleccionado3Sprite = Resources.Load<Sprite>($"Icons/seleccionado_icon");
                if (seleccionado3Sprite != null)
                    seleccionado3.style.backgroundImage = new StyleBackground(seleccionado3Sprite);
                btnConfirmarTelevision.style.display = DisplayStyle.Flex;
            });
        }

        private void CargarOfertasTelevisiones()
        {
            int reputacionEquipo = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Reputacion;

            List<Television> listaTelevisiones = TelevisionData.MostrarListaTelevisiones();

            // Generar lista de Televisiones Principales
            GenerarTelevisionesPrincipales(reputacionEquipo, listaTelevisiones, out TelevisionPrincipal, out cantidadesPrincipal, out mensualidadPrincipal, out duracionesPrincipal);

            // Cargar datos en la ventana
            Sprite logoTelevision1Sprite = Resources.Load<Sprite>($"Televisiones/{TelevisionPrincipal[0]}");
            if (logoTelevision1Sprite != null)
                logoTelevision1.style.backgroundImage = new StyleBackground(logoTelevision1Sprite);

            Sprite logoTelevision2Sprite = Resources.Load<Sprite>($"Televisiones/{TelevisionPrincipal[1]}");
            if (logoTelevision2Sprite != null)
                logoTelevision2.style.backgroundImage = new StyleBackground(logoTelevision2Sprite);

            Sprite logoTelevision3Sprite = Resources.Load<Sprite>($"Televisiones/{TelevisionPrincipal[2]}");
            if (logoTelevision3Sprite != null)
                logoTelevision3.style.backgroundImage = new StyleBackground(logoTelevision3Sprite);

            pagoAnticipado1.text = Constants.CambioDivisa(cantidadesPrincipal[0]).ToString("N0") + " " + CargarSimboloMoneda();
            pagoAnticipado2.text = Constants.CambioDivisa(cantidadesPrincipal[1]).ToString("N0") + " " + CargarSimboloMoneda();
            pagoAnticipado3.text = Constants.CambioDivisa(cantidadesPrincipal[2]).ToString("N0") + " " + CargarSimboloMoneda();

            pagoMensual1.text = Constants.CambioDivisa(mensualidadPrincipal[0]).ToString("N0") + " " + CargarSimboloMoneda();
            pagoMensual2.text = Constants.CambioDivisa(mensualidadPrincipal[1]).ToString("N0") + " " + CargarSimboloMoneda();
            pagoMensual3.text = Constants.CambioDivisa(mensualidadPrincipal[2]).ToString("N0") + " " + CargarSimboloMoneda();

            if (duracionesPrincipal[0] == 1)
            {
                duracion1.text = duracionesPrincipal[0].ToString() + " año";
            }
            else
            {
                duracion1.text = duracionesPrincipal[0].ToString() + " años";
            }

            if (duracionesPrincipal[1] == 1)
            {
                duracion2.text = duracionesPrincipal[1].ToString() + " año";
            }
            else
            {
                duracion2.text = duracionesPrincipal[1].ToString() + " años";
            }

            if (duracionesPrincipal[2] == 1)
            {
                duracion3.text = duracionesPrincipal[2].ToString() + " año";
            }
            else
            {
                duracion3.text = duracionesPrincipal[2].ToString() + " años";
            }
        }

        public void GenerarTelevisionesPrincipales(int reputacionEquipo, List<Television> listaTelevisiones,
                  out int[] TelevisionPrincipal, out int[] cantidadesPrincipal, out int[] mensualidadPrincipal, out int[] duracionesPrincipal)
        {
            int limiteReputacion;
            int cantidadMin = 20; // en millones
            int cantidadMax = 50;
            int mensualidadMin = 200000;
            int mensualidadMax = 1000000;

            if (reputacionEquipo >= 50 && reputacionEquipo <= 60)
            {
                limiteReputacion = 1;
                cantidadMin = 10;
                cantidadMax = 25;
                mensualidadMin = 200000;
                mensualidadMax = 500000;
            }
            else if (reputacionEquipo >= 61 && reputacionEquipo <= 70)
            {
                limiteReputacion = 2;
                cantidadMin = 15;
                cantidadMax = 30;
                mensualidadMin = 300000;
                mensualidadMax = 600000;
            }
            else if (reputacionEquipo >= 71 && reputacionEquipo <= 80)
            {
                limiteReputacion = 3;
                cantidadMin = 20;
                cantidadMax = 40;
                mensualidadMin = 400000;
                mensualidadMax = 800000;
            }
            else if (reputacionEquipo >= 81 && reputacionEquipo <= 90)
            {
                limiteReputacion = 4;
                cantidadMin = 25;
                cantidadMax = 45;
                mensualidadMin = 500000;
                mensualidadMax = 900000;
            }
            else if (reputacionEquipo >= 91 && reputacionEquipo <= 100)
            {
                limiteReputacion = 5;
                cantidadMin = 30;
                cantidadMax = 50;
                mensualidadMin = 600000;
                mensualidadMax = 1000000;
            }
            else
            {
                limiteReputacion = 0;
                cantidadMin = 5;
                cantidadMax = 15;
                mensualidadMin = 50000;
                mensualidadMax = 100000;
            }

            // Filtrar Televisiones según el límite de reputación
            var TelevisionesFiltrados = listaTelevisiones
                .Where(p => p.Reputacion <= limiteReputacion)
                .ToList();

            // Barajar los Televisiones para obtener una selección aleatoria
            System.Random random = new System.Random(); // <--- IMPORTANTE

            var seleccionAleatoria = TelevisionesFiltrados
                .OrderBy(x => random.Next())
                .Take(3)
                .ToList();

            // Inicializar arrays
            TelevisionPrincipal = seleccionAleatoria.Select(p => p.IdTelevision).ToArray();
            cantidadesPrincipal = seleccionAleatoria
                .Select(p => random.Next(cantidadMin, cantidadMax + 1) * 500000)
                .ToArray();
            mensualidadPrincipal = seleccionAleatoria
                .Select(p => random.Next(mensualidadMin / 50000, mensualidadMax / 50000 + 1) * 50000)
                .ToArray();
            duracionesPrincipal = seleccionAleatoria
                .Select(p => random.Next(1, 4))
                .ToArray();
        }

        private void CargarDatosTelevision()
        {
            Sprite escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/{miEquipo.IdEquipo}");
            if (escudoSprite != null)
                escudo.style.backgroundImage = new StyleBackground(escudoSprite);

            // Consultar si hay Televisiones contratados
            Television? Television = TelevisionData.TelevisionesContratadas(miEquipo.IdEquipo);
            if (Television != null)
            {
                nombreTelevision.text = Television.Nombre!.ToUpper();

                Sprite logoTelevisionSprite = Resources.Load<Sprite>($"Televisiones/{Television.IdTelevision}");
                if (logoTelevisionSprite != null)
                    logoTelevision.style.backgroundImage = new StyleBackground(logoTelevisionSprite);

                pagoInicial.text = Constants.CambioDivisa(Television.Cantidad).ToString("N0") + " " + CargarSimboloMoneda();
                pagoMensual.text = Constants.CambioDivisa(Television.Mensualidad).ToString("N0") + " " + CargarSimboloMoneda();
                if (Television.DuracionContrato == 1)
                {
                    duracionContrato.text = Television.DuracionContrato.ToString() + " año";
                }
                else
                {
                    duracionContrato.text = Television.DuracionContrato.ToString() + " años";
                }
                btnOfertasTelevisiones.style.visibility = Visibility.Hidden;
            }
            else
            {
                btnOfertasTelevisiones.style.visibility = Visibility.Visible;
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

            mainScreen.miPresupuesto.text = $"{presupuestoConversion.ToString("N0")} {symbol}";
        }
    }
}
