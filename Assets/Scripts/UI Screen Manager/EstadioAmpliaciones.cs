#nullable enable

using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class EstadioAmpliaciones
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        private int tipoRemodelacion = 0;
        int aforo;

        private VisualElement root, escudoEquipo, seleccionado1, seleccionado2, seleccionado3, imagenEstadio,
                              imgExcavadora1, imgExcavadora2, imgExcavadora3, semanasRestantesFondo;
        private Label nombreEstadio, semanasRestantesTexto, popupText, precioRemodelacion1, precioRemodelacion2, precioRemodelacion3;
        private Button btnEmpezarObras;
        private VisualElement popupContainer;
        private Button btnCerrar;

        public EstadioAmpliaciones(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();
            aforo = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Aforo;

            // Referencias a objetos de la UI
            escudoEquipo = root.Q<VisualElement>("escudo-equipo");
            seleccionado1 = root.Q<VisualElement>("seleccionado-uno");
            seleccionado2 = root.Q<VisualElement>("seleccionado-dos");
            seleccionado3 = root.Q<VisualElement>("seleccionado-tres");
            imagenEstadio = root.Q<VisualElement>("imagen-estadio");
            imgExcavadora1 = root.Q<VisualElement>("imgExcavadora1");
            imgExcavadora2 = root.Q<VisualElement>("imgExcavadora2");
            imgExcavadora3 = root.Q<VisualElement>("imgExcavadora3");
            semanasRestantesFondo = root.Q<VisualElement>("semanasRestantesFondo");
            nombreEstadio = root.Q<Label>("nombre-estadio");
            semanasRestantesTexto = root.Q<Label>("aumentarCapacidad");
            precioRemodelacion1 = root.Q<Label>("precioRemodelacion1");
            precioRemodelacion2 = root.Q<Label>("precioRemodelacion2");
            precioRemodelacion3 = root.Q<Label>("precioRemodelacion3");
            btnEmpezarObras = root.Q<Button>("btnEmpezarObras");
            btnEmpezarObras.style.visibility = Visibility.Hidden;

            popupContainer = root.Q<VisualElement>("popup-container");
            btnCerrar = root.Q<Button>("btnCerrar");
            popupText = root.Q<Label>("popup-text");

            Sprite escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/{miEquipo.IdEquipo}");
            if (escudoSprite != null)
                escudoEquipo.style.backgroundImage = new StyleBackground(escudoSprite);

            nombreEstadio.text = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Estadio + " (" + EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Aforo.ToString("N0") + " asientos)";

            // Cargar precios remodelaciones
            precioRemodelacion1.text = Constants.CambioDivisa(5000000).ToString("N0") + " " + CargarSimboloMoneda();
            precioRemodelacion2.text = Constants.CambioDivisa(15000000).ToString("N0") + " " + CargarSimboloMoneda();
            precioRemodelacion3.text = Constants.CambioDivisa(30000000).ToString("N0") + " " + CargarSimboloMoneda();

            Remodelacion obraActiva = RemodelacionData.ComprobarRemodelacion(miEquipo.IdEquipo);
            CargarEstadoObras(obraActiva);

            Sprite seleccionadoSprite = Resources.Load<Sprite>($"Icons/seleccionado_icon");
            Sprite deseleccionadoSprite = Resources.Load<Sprite>($"Icons/noSeleccionado_icon");
            seleccionado1.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                btnEmpezarObras.style.visibility = Visibility.Visible;
                tipoRemodelacion = 1;

                if (seleccionadoSprite != null && deseleccionadoSprite != null)
                {
                    seleccionado1.style.backgroundImage = new StyleBackground(seleccionadoSprite);
                    seleccionado2.style.backgroundImage = new StyleBackground(deseleccionadoSprite);
                    seleccionado3.style.backgroundImage = new StyleBackground(deseleccionadoSprite);
                }
            });

            seleccionado2.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                btnEmpezarObras.style.visibility = Visibility.Visible;
                tipoRemodelacion = 2;

                if (seleccionadoSprite != null && deseleccionadoSprite != null)
                {
                    seleccionado1.style.backgroundImage = new StyleBackground(deseleccionadoSprite);
                    seleccionado2.style.backgroundImage = new StyleBackground(seleccionadoSprite);
                    seleccionado3.style.backgroundImage = new StyleBackground(deseleccionadoSprite);
                }
            });

            seleccionado3.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                btnEmpezarObras.style.visibility = Visibility.Visible;
                tipoRemodelacion = 3;

                if (seleccionadoSprite != null && deseleccionadoSprite != null)
                {
                    seleccionado1.style.backgroundImage = new StyleBackground(deseleccionadoSprite);
                    seleccionado2.style.backgroundImage = new StyleBackground(deseleccionadoSprite);
                    seleccionado3.style.backgroundImage = new StyleBackground(seleccionadoSprite);
                }
            });

            btnEmpezarObras.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarEmpezarObras();
            };
        }

        private void CargarEstadoObras(Remodelacion obraActiva)
        {
            if (obraActiva != null)
            {
                seleccionado1.style.visibility = Visibility.Hidden;
                seleccionado2.style.visibility = Visibility.Hidden;
                seleccionado3.style.visibility = Visibility.Hidden;
                btnEmpezarObras.style.visibility = Visibility.Hidden;

                semanasRestantesFondo.style.backgroundColor = new Color(0.55f, 0f, 0f);
                TimeSpan semanasRestantes = obraActiva.FechaFinal - FechaData.hoy;
                int semanas = (int)Math.Ceiling(semanasRestantes.TotalDays / 7);
                semanasRestantesTexto.text = "LAS OBRAS FINALIZARÁN EN " + semanas.ToString() + " SEMANAS";

                if (obraActiva.TipoRemodelacion == 1)
                {
                    imgExcavadora1.style.visibility = Visibility.Visible;
                }
                else if (obraActiva.TipoRemodelacion == 2)
                {
                    imgExcavadora2.style.visibility = Visibility.Visible;
                }
                else if (obraActiva.TipoRemodelacion == 3)
                {
                    imgExcavadora3.style.visibility = Visibility.Visible;
                }

                CargarImagenConstruccion(aforo);
            }
            else
            {
                seleccionado1.style.visibility = Visibility.Visible;
                seleccionado2.style.visibility = Visibility.Visible;
                seleccionado3.style.visibility = Visibility.Visible;

                imgExcavadora1.style.visibility = Visibility.Hidden;
                imgExcavadora2.style.visibility = Visibility.Hidden;
                imgExcavadora3.style.visibility = Visibility.Hidden;

                CargarImagen(aforo);
            }
        }

        private void CargarEmpezarObras()
        {
            if (miEquipo.Aforo < 150000)
            {
                int aumento = 0;
                int semanasObras = 0;
                if (tipoRemodelacion != 0)
                {
                    DateTime fechaFinal;
                    if (tipoRemodelacion == 1)
                    {
                        aumento = 500;
                        semanasObras = 20;
                        fechaFinal = FechaData.hoy.AddDays(20 * 7); // Fecha actual más 20 semanas
                        RemodelacionData.CrearNuevaRemodelacion(miEquipo.IdEquipo, fechaFinal, tipoRemodelacion);
                    }
                    else if (tipoRemodelacion == 2)
                    {
                        aumento = 1000;
                        semanasObras = 35;
                        fechaFinal = FechaData.hoy.AddDays(35 * 7); // Fecha actual más 35 semanas
                        RemodelacionData.CrearNuevaRemodelacion(miEquipo.IdEquipo, fechaFinal, tipoRemodelacion);
                    }
                    else if (tipoRemodelacion == 3)
                    {
                        aumento = 1500;
                        semanasObras = 50;
                        fechaFinal = FechaData.hoy.AddDays(50 * 7); // Fecha actual más 50 semanas
                        RemodelacionData.CrearNuevaRemodelacion(miEquipo.IdEquipo, fechaFinal, tipoRemodelacion);
                    }
                }

                seleccionado1.style.visibility = Visibility.Hidden;
                seleccionado2.style.visibility = Visibility.Hidden;
                seleccionado3.style.visibility = Visibility.Hidden;
                btnEmpezarObras.style.visibility = Visibility.Hidden;

                if (tipoRemodelacion == 1)
                {
                    imgExcavadora1.style.visibility = Visibility.Visible;
                }
                else if (tipoRemodelacion == 2)
                {
                    imgExcavadora2.style.visibility = Visibility.Visible;
                }
                else if (tipoRemodelacion == 3)
                {
                    imgExcavadora3.style.visibility = Visibility.Visible;
                }

                Remodelacion obraActiva = RemodelacionData.ComprobarRemodelacion(miEquipo.IdEquipo);

                if (obraActiva != null)
                {
                    TimeSpan semanasRestantes = obraActiva.FechaFinal - FechaData.hoy;
                    int semanas = (int)Math.Ceiling(semanasRestantes.TotalDays / 7);
                    semanasRestantesTexto.text = "LAS OBRAS FINALIZARÁN EN " + semanas.ToString() + " SEMANAS";
                    semanasRestantesFondo.style.backgroundColor = new Color(0.55f, 0f, 0f);

                    CargarImagenConstruccion(aforo);
                }

                // Creamos el mensaje
                Empleado? financiero = EmpleadoData.ObtenerEmpleadoPorPuesto("Financiero");
                string presidente = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Presidente;

                Mensaje mensajeObras = new Mensaje
                {
                    Fecha = FechaData.hoy,
                    Remitente = financiero != null ? financiero.Nombre : presidente,
                    Asunto = "Inicio de las Obras de Remodelación del Estadio",
                    Contenido = $"Hoy es un día importante para nuestro club. Me complace informarte que han comenzado oficialmente las obras de remodelación de las gradas del estadio. Esta inversión mejorará la comodidad de nuestros aficionados y aumentará la capacidad en {aumento.ToString("N0", new CultureInfo("es-ES"))} asientos, lo que se traducirá en mayores ingresos a futuro.\n\nAunque durante las próximas {semanasObras} semanas podremos experimentar ciertas limitaciones en la asistencia, estamos convencidos de que este paso es fundamental para crecer como institución.",
                    TipoMensaje = "Notificación",
                    IdEquipo = miEquipo.IdEquipo,
                    Leido = false,
                    Icono = 0 // 0 es icono de equipo
                };

                MensajeData.CrearMensaje(mensajeObras);
            }
            else
            {
                popupContainer.style.display = DisplayStyle.Flex;
                popupText.text = "La capacidad actual del estadio ya ha alcanzado el límite máximo permitido. No es posible realizar más ampliaciones en estas instalaciones.";

                // Importante: limpiar listeners previos para evitar duplicados
                btnCerrar.clicked -= OnbtnCerrarClick;

                void OnbtnCerrarClick()
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    popupContainer.style.display = DisplayStyle.None;
                }
            }
        }

        private void CargarImagenConstruccion(int aforo)
        {
            if (aforo >= 85000)
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/largeDeluxe-construction");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
            }
            else if (aforo >= 75000)
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/large-construction");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
            }
            else if (aforo >= 50000)
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/mediumDeluxe-construction");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
            }
            else if (aforo >= 25000)
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/medium-construction");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
            }
            else
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/small-construction");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
            }
        }

        private void CargarImagen(int aforo)
        {
            if (aforo >= 85000)
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/largeDeluxe");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
            }
            else if (aforo >= 75000)
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/large");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
            }
            else if (aforo >= 50000)
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/mediumDeluxe");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
            }
            else if (aforo >= 25000)
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/medium");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
            }
            else
            {
                Sprite estadioSprite = Resources.Load<Sprite>($"Estadios/small");
                if (estadioSprite != null)
                    imagenEstadio.style.backgroundImage = new StyleBackground(estadioSprite);
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
    }
}