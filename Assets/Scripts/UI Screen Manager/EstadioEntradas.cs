using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class EstadioEntradas
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;

        int valueEntradaVip, valueEntradaTribuna, valueEntradaGeneral, valueAbonoVip, valueAbonoTribuna, valueAbonoGeneral;

        private VisualElement root, escudoEquipo, escudoLocal, escudoVisitante;
        private VisualElement btnEntradaVipMenos, btnEntradaVipMas, btnEntradaTribunaMenos, btnEntradaTribunaMas, btnEntradaGeneralMenos, btnEntradaGeneralMas,
                              btnAbonoVipMenos, btnAbonoVipMas, btnAbonoTribunaMenos, btnAbonoTribunaMas, btnAbonoGeneralMenos, btnAbonoGeneralMas;
        private Label fechaPartido, precioEntradaVip, precioEntradaTribuna, precioEntradaGeneral,
                      precioAbonoVip, precioAbonoTribuna, precioAbonoGeneral,
                      precioEntradaVipMoneda, precioEntradaTribunaMoneda, precioEntradaGeneralMoneda,
                      precioAbonoVipMoneda, precioAbonoTribunaMoneda, precioAbonoGeneralMoneda;
        private Button btnPrecioEntradas, btnPrecioAbonos;

        public EstadioEntradas(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            escudoEquipo = root.Q<VisualElement>("escudo-equipo");
            escudoLocal = root.Q<VisualElement>("escudo-local");
            escudoVisitante = root.Q<VisualElement>("escudo-visitante");
            btnEntradaVipMenos = root.Q<VisualElement>("entrada-vip-menos");
            btnEntradaVipMas = root.Q<VisualElement>("entrada-vip-mas");
            btnEntradaTribunaMenos = root.Q<VisualElement>("entrada-tribuna-menos");
            btnEntradaTribunaMas = root.Q<VisualElement>("entrada-tribuna-mas");
            btnEntradaGeneralMenos = root.Q<VisualElement>("entrada-general-menos");
            btnEntradaGeneralMas = root.Q<VisualElement>("entrada-general-mas");
            btnAbonoVipMenos = root.Q<VisualElement>("abono-vip-menos");
            btnAbonoVipMas = root.Q<VisualElement>("abono-vip-mas");
            btnAbonoTribunaMenos = root.Q<VisualElement>("abono-tribuna-menos");
            btnAbonoTribunaMas = root.Q<VisualElement>("abono-tribuna-mas");
            btnAbonoGeneralMenos = root.Q<VisualElement>("abono-general-menos");
            btnAbonoGeneralMas = root.Q<VisualElement>("abono-general-mas");

            fechaPartido = root.Q<Label>("fecha-partido");
            precioEntradaVip = root.Q<Label>("entrada-vip-precio");
            precioEntradaTribuna = root.Q<Label>("entrada-tribuna-precio");
            precioEntradaGeneral = root.Q<Label>("entrada-general-precio");
            precioAbonoVip = root.Q<Label>("abono-vip-precio");
            precioAbonoTribuna = root.Q<Label>("abono-tribuna-precio");
            precioAbonoGeneral = root.Q<Label>("abono-general-precio");
            precioEntradaVipMoneda = root.Q<Label>("entrada-vip-moneda");
            precioEntradaTribunaMoneda = root.Q<Label>("entrada-tribuna-moneda");
            precioEntradaGeneralMoneda = root.Q<Label>("entrada-general-moneda");
            precioAbonoVipMoneda = root.Q<Label>("abono-vip-moneda");
            precioAbonoTribunaMoneda = root.Q<Label>("abono-tribuna-moneda");
            precioAbonoGeneralMoneda = root.Q<Label>("abono-general-moneda");

            btnPrecioEntradas = root.Q<Button>("btnEstablecerPrecioEntradas");
            btnPrecioAbonos = root.Q<Button>("btnEstablecerPrecioAbonos");

            Sprite escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/{miEquipo.IdEquipo}");
            if (escudoSprite != null)
                escudoEquipo.style.backgroundImage = new StyleBackground(escudoSprite);

            Partido proximoPartido = PartidoData.MostrarProximoPartidoLocal(miEquipo.IdEquipo, FechaData.ObtenerFechaHoy());
            fechaPartido.text = proximoPartido.FechaPartido.ToString("d 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

            Sprite escudoLocalSprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{proximoPartido.IdEquipoLocal}");
            if (escudoLocalSprite != null)
                escudoLocal.style.backgroundImage = new StyleBackground(escudoLocalSprite);

            Sprite escudoVisitanteSprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{proximoPartido.IdEquipoVisitante}");
            if (escudoVisitanteSprite != null)
                escudoVisitante.style.backgroundImage = new StyleBackground(escudoVisitanteSprite);

            CargarPrecios();
            CargarSimboloMoneda();
            ComprobarAbonosAsignados();

            ConfigurarBotonPrecio(btnEntradaVipMenos, precioEntradaVip, -10, 50, 500, clickSFX);
            ConfigurarBotonPrecio(btnEntradaVipMas, precioEntradaVip, +10, 50, 500, clickSFX);
            ConfigurarBotonPrecio(btnEntradaTribunaMenos, precioEntradaTribuna, -10, 40, 400, clickSFX);
            ConfigurarBotonPrecio(btnEntradaTribunaMas, precioEntradaTribuna, +10, 40, 400, clickSFX);
            ConfigurarBotonPrecio(btnEntradaGeneralMenos, precioEntradaGeneral, -10, 10, 300, clickSFX);
            ConfigurarBotonPrecio(btnEntradaGeneralMas, precioEntradaGeneral, +10, 10, 300, clickSFX);

            ConfigurarBotonPrecio(btnAbonoVipMenos, precioAbonoVip, -50, 200, 3000, clickSFX);
            ConfigurarBotonPrecio(btnAbonoVipMas, precioAbonoVip, +50, 200, 3000, clickSFX);
            ConfigurarBotonPrecio(btnAbonoTribunaMenos, precioAbonoTribuna, -50, 100, 2000, clickSFX);
            ConfigurarBotonPrecio(btnAbonoTribunaMas, precioAbonoTribuna, +50, 100, 2000, clickSFX);
            ConfigurarBotonPrecio(btnAbonoGeneralMenos, precioAbonoGeneral, -50, 50, 1000, clickSFX);
            ConfigurarBotonPrecio(btnAbonoGeneralMas, precioAbonoGeneral, +50, 50, 1000, clickSFX);

            btnPrecioEntradas.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                TaquillaData.EstablecerPrecioEntradas(miEquipo.IdEquipo, int.Parse(precioEntradaGeneral.text.Trim()),
                                                                         int.Parse(precioEntradaTribuna.text.Trim()),
                                                                         int.Parse(precioEntradaVip.text.Trim()));
                btnPrecioEntradas.style.visibility = Visibility.Hidden;
            };

            btnPrecioAbonos.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                TaquillaData.EstablecerPrecioAbonos(miEquipo.IdEquipo, int.Parse(precioAbonoGeneral.text.Trim()),
                                                                       int.Parse(precioAbonoTribuna.text.Trim()),
                                                                       int.Parse(precioAbonoVip.text.Trim()));
                btnPrecioAbonos.style.visibility = Visibility.Hidden;
            };
        }

        private void ComprobarAbonosAsignados()
        {
            //Comprobar si se ha establecido el precio de los abonos.
            bool verificadoAbono = TaquillaData.ComprobarAbono(miEquipo.IdEquipo);
            if (verificadoAbono)
            {
                btnPrecioAbonos.style.visibility = Visibility.Hidden;
            }
            else
            {
                btnPrecioAbonos.style.visibility = Visibility.Visible;
            }

            // Comprobar si ha pasado la fecha de abonados.
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();
            DateTime fechaAbonados = ObtenerSegundoLunesDeAgosto(FechaData.temporadaActual);
            DateTime hoy = FechaData.hoy;
            DateTime limite = new DateTime(FechaData.temporadaActual + 1, 6, 30); // 30 de junio del año siguiente

            if (fechaAbonados <= hoy && hoy < limite)
            {
                btnPrecioAbonos.SetEnabled(false);
                btnPrecioAbonos.style.visibility = Visibility.Hidden;
            }
        }

        private void CargarSimboloMoneda()
        {
            string currency = PlayerPrefs.GetString("Currency", string.Empty);

            // Lista de labels a actualizar
            var labelsMoneda = new[]
            {
                precioEntradaVipMoneda,
                precioEntradaTribunaMoneda,
                precioEntradaGeneralMoneda,
                precioAbonoVipMoneda,
                precioAbonoTribunaMoneda,
                precioAbonoGeneralMoneda
            };

            // Elegir símbolo según moneda
            string simbolo = currency switch
            {
                Constants.EURO_NAME => Constants.EURO_SYMBOL,
                Constants.POUND_NAME => Constants.POUND_SYMBOL,
                Constants.DOLLAR_NAME => Constants.DOLLAR_SYMBOL,
                _ => Constants.EURO_SYMBOL // default
            };

            // Aplicar a todos los labels
            foreach (var label in labelsMoneda)
            {
                label.text = simbolo;
            }
        }

        private void CargarPrecios()
        {
            Taquilla miTaquilla = TaquillaData.RecuperarPreciosTaquilla(miEquipo.IdEquipo);
            precioEntradaGeneral.text = miTaquilla.PrecioEntradaGeneral.ToString();
            precioEntradaTribuna.text = miTaquilla.PrecioEntradaTribuna.ToString();
            precioEntradaVip.text = miTaquilla.PrecioEntradaVip.ToString();
            if (miTaquilla.PrecioAbonoGeneral == 0)
            {
                precioAbonoGeneral.text = "250";
            }
            else
            {
                precioAbonoGeneral.text = miTaquilla.PrecioAbonoGeneral.ToString();
            }

            if (miTaquilla.PrecioAbonoTribuna == 0)
            {
                precioAbonoTribuna.text = "500";
            }
            else
            {
                precioAbonoTribuna.text = miTaquilla.PrecioAbonoTribuna.ToString();
            }

            if (miTaquilla.PrecioAbonoVip == 0)
            {
                precioAbonoVip.text = "1000";
            }
            else
            {
                precioAbonoVip.text = miTaquilla.PrecioAbonoVip.ToString();
            }
        }

        private void ConfigurarBotonPrecio(VisualElement boton, Label label, int cambio, int min, int max, AudioClip clickSFX)
        {
            boton.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                int valorActual = int.Parse(label.text);
                valorActual += cambio;

                valorActual = Mathf.Clamp(valorActual, min, max);

                label.text = valorActual.ToString();
            });
        }

        public static DateTime ObtenerSegundoLunesDeAgosto(int anio)
        {
            DateTime fecha = new DateTime(anio, 8, 1);
            int lunesEncontrados = 0;

            while (fecha.Month == 8)
            {
                if (fecha.DayOfWeek == DayOfWeek.Monday)
                {
                    lunesEncontrados++;
                    if (lunesEncontrados == 2)
                    {
                        return fecha;
                    }
                }

                fecha = fecha.AddDays(1);
            }

            throw new Exception("No se encontró el tercer sábado de agosto.");
        }
    }
}