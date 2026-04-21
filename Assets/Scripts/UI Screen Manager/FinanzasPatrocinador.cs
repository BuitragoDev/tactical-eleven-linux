#nullable enable

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Globalization;

namespace TacticalEleven.Scripts
{
    public class FinanzasPatrocinador
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        private MainScreen mainScreen;

        int patrocinadorPrincipalSeleccionado = 0;
        int[]? patrocinadorPrincipal;
        int[]? cantidadesPrincipal;
        int[]? mensualidadPrincipal;
        int[]? duracionesPrincipal;

        private VisualElement root, escudo, logoPatrocinador, logoPatrocinador1, logoPatrocinador2, logoPatrocinador3,
                            seleccionado1, seleccionado2, seleccionado3, popupContainer;
        private Button btnOfertasPatrocinadores, btnConfirmarPatrocinador;
        private Label nombrePatrocinador, pagoInicial, pagoMensual, duracionContrato, pagoAnticipado1, pagoAnticipado2, pagoAnticipado3,
                      pagoMensual1, pagoMensual2, pagoMensual3, duracion1, duracion2, duracion3;

        public FinanzasPatrocinador(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
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
            logoPatrocinador = root.Q<VisualElement>("logo-patrocinador");
            logoPatrocinador1 = root.Q<VisualElement>("logo-patrocinador1");
            logoPatrocinador2 = root.Q<VisualElement>("logo-patrocinador2");
            logoPatrocinador3 = root.Q<VisualElement>("logo-patrocinador3");
            seleccionado1 = root.Q<VisualElement>("seleccionado1");
            seleccionado2 = root.Q<VisualElement>("seleccionado2");
            seleccionado3 = root.Q<VisualElement>("seleccionado3");
            btnOfertasPatrocinadores = root.Q<Button>("btnOfertasPatrocinadores");
            btnConfirmarPatrocinador = root.Q<Button>("btnConfirmarPatrocinador");
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
            nombrePatrocinador = root.Q<Label>("nombre-patrocinador");

            CargarDatosPatrocinador();

            btnOfertasPatrocinadores.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                popupContainer.style.display = DisplayStyle.Flex;
                CargarOfertasPatrocinadores();
                btnOfertasPatrocinadores.SetEnabled(false);
            };

            btnConfirmarPatrocinador.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                PatrocinadorData.AnadirUnPatrocinador(patrocinadorPrincipal[patrocinadorPrincipalSeleccionado - 1], cantidadesPrincipal[patrocinadorPrincipalSeleccionado - 1], mensualidadPrincipal[patrocinadorPrincipalSeleccionado - 1], duracionesPrincipal[patrocinadorPrincipalSeleccionado - 1], miEquipo.IdEquipo);

                // Crear el mensaje
                Empleado? financiero = EmpleadoData.ObtenerEmpleadoPorPuesto("Financiero");
                string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;
                string? nombrePatrocinador = PatrocinadorData.NombrePatrocinador(patrocinadorPrincipal[patrocinadorPrincipalSeleccionado - 1]);

                Mensaje mensajePatrocinador = new Mensaje
                {
                    Fecha = FechaData.hoy,
                    Remitente = financiero != null ? financiero.Nombre : presidente,
                    Asunto = "Nuevo Acuerdo de Patrocinio Cerrado",
                    Contenido = $"Me complace anunciar que hemos alcanzado un acuerdo con un nuevo patrocinador: {nombrePatrocinador}. Este contrato nos aportará un pago inicial de {cantidadesPrincipal[patrocinadorPrincipalSeleccionado - 1].ToString("N0", new CultureInfo("es-ES"))}€ y un pago mensual de {mensualidadPrincipal[patrocinadorPrincipalSeleccionado - 1].ToString("N0", new CultureInfo("es-ES"))}€ por temporada durante {duracionesPrincipal[patrocinadorPrincipalSeleccionado - 1]} años y refleja el creciente interés comercial en nuestro proyecto deportivo.\n\nEste tipo de alianzas son fundamentales para mejorar nuestra situación financiera y nos permitirán tener mayor margen de maniobra en futuras operaciones.\n\nFelicidades por la imagen que estás proyectando del club. ¡Sigamos creciendo juntos!",
                    TipoMensaje = "Notificación",
                    IdEquipo = miEquipo.IdEquipo,
                    Leido = false,
                    Icono = 0 // 0 es icono de equipo
                };

                MensajeData.CrearMensaje(mensajePatrocinador);

                // Crear ingreso del pago inicial del patrocinador
                int pagoPatrocinador = cantidadesPrincipal[patrocinadorPrincipalSeleccionado - 1];
                Finanza nuevoIngresoPatrocinio = new Finanza
                {
                    IdEquipo = miEquipo.IdEquipo,
                    Temporada = FechaData.temporadaActual.ToString(),
                    IdConcepto = 6,
                    Tipo = 1,
                    Cantidad = pagoPatrocinador,
                    Fecha = FechaData.hoy.Date
                };
                FinanzaData.CrearIngreso(nuevoIngresoPatrocinio);

                // Restar la indemnización al Presupuesto
                EquipoData.SumarCantidadAPresupuesto(miEquipo.IdEquipo, pagoPatrocinador);

                popupContainer.style.display = DisplayStyle.None;
                btnOfertasPatrocinadores.SetEnabled(true);
                CargarDatosPatrocinador();
                ActualizarPresupuestoMainScreen();
            };

            seleccionado1.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                patrocinadorPrincipalSeleccionado = 1;
                Sprite seleccionado1Sprite = Resources.Load<Sprite>($"Icons/seleccionado_icon");
                if (seleccionado1Sprite != null)
                    seleccionado1.style.backgroundImage = new StyleBackground(seleccionado1Sprite);
                Sprite seleccionado2Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado2Sprite != null)
                    seleccionado2.style.backgroundImage = new StyleBackground(seleccionado2Sprite);
                Sprite seleccionado3Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado3Sprite != null)
                    seleccionado3.style.backgroundImage = new StyleBackground(seleccionado3Sprite);
                btnConfirmarPatrocinador.style.display = DisplayStyle.Flex;
            });

            seleccionado2.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                patrocinadorPrincipalSeleccionado = 2;
                Sprite seleccionado1Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado1Sprite != null)
                    seleccionado1.style.backgroundImage = new StyleBackground(seleccionado1Sprite);
                Sprite seleccionado2Sprite = Resources.Load<Sprite>($"Icons/seleccionado_icon");
                if (seleccionado2Sprite != null)
                    seleccionado2.style.backgroundImage = new StyleBackground(seleccionado2Sprite);
                Sprite seleccionado3Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado3Sprite != null)
                    seleccionado3.style.backgroundImage = new StyleBackground(seleccionado3Sprite);
                btnConfirmarPatrocinador.style.display = DisplayStyle.Flex;
            });

            seleccionado3.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                patrocinadorPrincipalSeleccionado = 3;
                Sprite seleccionado1Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado1Sprite != null)
                    seleccionado1.style.backgroundImage = new StyleBackground(seleccionado1Sprite);
                Sprite seleccionado2Sprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
                if (seleccionado2Sprite != null)
                    seleccionado2.style.backgroundImage = new StyleBackground(seleccionado2Sprite);
                Sprite seleccionado3Sprite = Resources.Load<Sprite>($"Icons/seleccionado_icon");
                if (seleccionado3Sprite != null)
                    seleccionado3.style.backgroundImage = new StyleBackground(seleccionado3Sprite);
                btnConfirmarPatrocinador.style.display = DisplayStyle.Flex;
            });
        }

        private void CargarOfertasPatrocinadores()
        {
            int reputacionEquipo = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Reputacion;

            List<Patrocinador> listaPatrocinadores = PatrocinadorData.MostrarListaPatrocinadores();

            // Generar lista de Patrocinadores Principales
            GenerarPatrocinadoresPrincipales(reputacionEquipo, listaPatrocinadores, out patrocinadorPrincipal, out cantidadesPrincipal, out mensualidadPrincipal, out duracionesPrincipal);

            // Cargar datos en la ventana
            Sprite logoPatrocinador1Sprite = Resources.Load<Sprite>($"Patrocinadores/{patrocinadorPrincipal[0]}");
            if (logoPatrocinador1Sprite != null)
                logoPatrocinador1.style.backgroundImage = new StyleBackground(logoPatrocinador1Sprite);

            Sprite logoPatrocinador2Sprite = Resources.Load<Sprite>($"Patrocinadores/{patrocinadorPrincipal[1]}");
            if (logoPatrocinador2Sprite != null)
                logoPatrocinador2.style.backgroundImage = new StyleBackground(logoPatrocinador2Sprite);

            Sprite logoPatrocinador3Sprite = Resources.Load<Sprite>($"Patrocinadores/{patrocinadorPrincipal[2]}");
            if (logoPatrocinador3Sprite != null)
                logoPatrocinador3.style.backgroundImage = new StyleBackground(logoPatrocinador3Sprite);

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

        public void GenerarPatrocinadoresPrincipales(int reputacionEquipo, List<Patrocinador> listaPatrocinadores,
                  out int[] patrocinadorPrincipal, out int[] cantidadesPrincipal, out int[] mensualidadPrincipal, out int[] duracionesPrincipal)
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

            // Filtrar patrocinadores según el límite de reputación
            var patrocinadoresFiltrados = listaPatrocinadores
                .Where(p => p.Reputacion <= limiteReputacion)
                .ToList();

            // Barajar los patrocinadores para obtener una selección aleatoria
            System.Random random = new System.Random(); // <--- IMPORTANTE

            var seleccionAleatoria = patrocinadoresFiltrados
                .OrderBy(x => random.Next())
                .Take(3)
                .ToList();

            // Inicializar arrays
            patrocinadorPrincipal = seleccionAleatoria.Select(p => p.IdPatrocinador).ToArray();
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

        private void CargarDatosPatrocinador()
        {
            Sprite escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/{miEquipo.IdEquipo}");
            if (escudoSprite != null)
                escudo.style.backgroundImage = new StyleBackground(escudoSprite);

            // Consultar si hay patrocinadores contratados
            Patrocinador? patrocinador = PatrocinadorData.PatrocinadoresContratados(miEquipo.IdEquipo);
            if (patrocinador != null)
            {
                nombrePatrocinador.text = patrocinador.Nombre!.ToUpper();

                Sprite logoPatrocinadorSprite = Resources.Load<Sprite>($"Patrocinadores/{patrocinador.IdPatrocinador}");
                if (logoPatrocinadorSprite != null)
                    logoPatrocinador.style.backgroundImage = new StyleBackground(logoPatrocinadorSprite);

                pagoInicial.text = Constants.CambioDivisa(patrocinador.Cantidad).ToString("N0") + " " + CargarSimboloMoneda();
                pagoMensual.text = Constants.CambioDivisa(patrocinador.Mensualidad).ToString("N0") + " " + CargarSimboloMoneda();
                if (patrocinador.DuracionContrato == 1)
                {
                    duracionContrato.text = patrocinador.DuracionContrato.ToString() + " año";
                }
                else
                {
                    duracionContrato.text = patrocinador.DuracionContrato.ToString() + " años";
                }
                btnOfertasPatrocinadores.style.visibility = Visibility.Hidden;
            }
            else
            {
                btnOfertasPatrocinadores.style.visibility = Visibility.Visible;
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