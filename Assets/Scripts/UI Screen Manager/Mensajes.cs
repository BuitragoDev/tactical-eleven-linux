using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class Mensajes
    {
        private AudioClip clickSFX;

        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root;
        private VisualElement mensajesContainer, correoLogoEquipo;
        private Label correoFechaMensaje, correoAutorMensaje, mensajeCabecera, mensajeContenido;
        private Button marcarLeido;
        private VisualTreeAsset mensajeTemplate;

        private int mensajeSeleccionadoId = -1;

        public Mensajes(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            mensajesContainer = root.Q<VisualElement>("mensajes-container");
            correoLogoEquipo = root.Q<VisualElement>("correo-logoEquipo");
            correoFechaMensaje = root.Q<Label>("correo-fechaMensaje");
            correoAutorMensaje = root.Q<Label>("correo-autorMensaje");
            mensajeCabecera = root.Q<Label>("mensaje-cabecera");
            mensajeContenido = root.Q<Label>("mensaje-contenido");
            marcarLeido = root.Q<Button>("btnMarcarLeido");

            mensajeTemplate = Resources.Load<VisualTreeAsset>("UI/MensajesScreen/CorreoItem");

            CargarListadoMensajes();

            marcarLeido.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                if (mensajeSeleccionadoId <= 0)
                    return;

                MensajeData.MarcarComoLeido(mensajeSeleccionadoId);
                CargarListadoMensajes();
            };
        }

        public void CargarListadoMensajes()
        {
            mensajesContainer.Clear();

            List<Mensaje> mensajes = MensajeData.MostrarMisMensajes();

            if (mensajes == null || mensajes.Count == 0)
                return;

            foreach (var m in mensajes)
            {
                // Instancia del template
                TemplateContainer item = mensajeTemplate.Instantiate();
                item.style.marginBottom = 6;

                // Obtener elementos del template
                var correoItem = item.Q<VisualElement>("correo-item");
                var logo = item.Q<VisualElement>("correo-logo");
                var lblFecha = item.Q<Label>("correo-fecha");
                var lblAutor = item.Q<Label>("correo-autor");
                var lblAsunto = item.Q<Label>("correo-asunto");
                var btnBorrar = item.Q<VisualElement>("btnBorrar");

                if (m.Leido)
                    correoItem.AddToClassList("mensaje-leido");
                else
                    correoItem.AddToClassList("mensaje-no-leido");

                // Rellenar datos
                lblFecha.text = m.Fecha.ToString("dd/MM/yyyy");
                lblAutor.text = m.Remitente;
                lblAsunto.text = m.Asunto;

                // Logo del mensaje
                if (m.Icono == 0)
                {
                    Sprite iconSprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{m.IdEquipo}");
                    if (iconSprite != null)
                        logo.style.backgroundImage = new StyleBackground(iconSprite);
                }
                else
                {
                    Sprite iconSprite = Resources.Load<Sprite>($"Jugadores/{m.Icono}");
                    if (iconSprite != null)
                        logo.style.backgroundImage = new StyleBackground(iconSprite);
                }

                // Botón borrar
                int idMensaje = m.IdMensaje;

                // Meter en el contenedor principal
                mensajesContainer.Add(item);

                // Eventos 
                btnBorrar.RegisterCallback<ClickEvent>(evt =>
                {
                    evt.StopPropagation();

                    AudioManager.Instance.PlaySFX(clickSFX);
                    MensajeData.BorrarMensaje(idMensaje);

                    // Recargar lista
                    CargarListadoMensajes();
                });

                item.RegisterCallback<ClickEvent>(evt =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    mensajeSeleccionadoId = m.IdMensaje;

                    // Cargar detalles del mensaje
                    correoFechaMensaje.text = m.Fecha.ToString("dd/MM/yyyy");
                    correoAutorMensaje.text = m.Remitente;
                    mensajeCabecera.text = m.Asunto;
                    mensajeContenido.text = m.Contenido;

                    if (m.Icono == 0)
                    {
                        Sprite correoLogoEquipoSprite = Resources.Load<Sprite>($"EscudosEquipos/120x120/{m.IdEquipo}");
                        if (correoLogoEquipoSprite != null)
                        {
                            correoLogoEquipo.style.backgroundImage = new StyleBackground(correoLogoEquipoSprite);
                            correoLogoEquipo.style.width = 120;
                            correoLogoEquipo.style.height = 120;
                        }
                    }
                    else
                    {
                        Sprite correoLogoEquipoSprite = Resources.Load<Sprite>($"Jugadores/{m.Icono}");
                        if (correoLogoEquipoSprite != null)
                            correoLogoEquipo.style.backgroundImage = new StyleBackground(correoLogoEquipoSprite);
                    }

                    marcarLeido.style.display = m.Leido ? DisplayStyle.None : DisplayStyle.Flex;
                });
            }
        }
    }
}