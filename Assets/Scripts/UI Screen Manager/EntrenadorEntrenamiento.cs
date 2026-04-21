using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class EntrenadorEntrenamiento
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        Jugador jugadorSeleccionado = null;

        private VisualElement root, jugadoresContainer, imagenEntrenamiento, fotoJugador, popupContainer;
        private Label nombreJugador, tipoEntrenamiento, popupText;
        private Button btnPortero, btnPase, btnEntradas, btnRemate, btnTiro, btnRegate, btnSinEntrenamiento, btnCerrar;

        public EntrenadorEntrenamiento(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            jugadoresContainer = root.Q<VisualElement>("jugadores-container");
            imagenEntrenamiento = root.Q<VisualElement>("imagen-entrenamiento");
            fotoJugador = root.Q<VisualElement>("foto-jugador");
            nombreJugador = root.Q<Label>("nombre-jugador");
            tipoEntrenamiento = root.Q<Label>("entrenamiento-nombre");
            popupText = root.Q<Label>("popup-text");
            btnPortero = root.Q<Button>("btnPortero");
            btnPase = root.Q<Button>("btnPase");
            btnEntradas = root.Q<Button>("btnEntradas");
            btnRemate = root.Q<Button>("btnRemate");
            btnTiro = root.Q<Button>("btnTiro");
            btnRegate = root.Q<Button>("btnRegate");
            btnSinEntrenamiento = root.Q<Button>("btnSinEntrenamiento");

            popupContainer = root.Q<VisualElement>("popup-container");
            btnCerrar = root.Q<Button>("btnCerrar");

            // Obtener la lista de jugadores de mi equipo
            List<Jugador> listaEquipo = JugadorData.ListadoJugadoresCompleto(miEquipo.IdEquipo);
            CargarListajugadores(listaEquipo);

            Empleado? preparador = EmpleadoData.ObtenerEmpleadoPorPuesto("Preparador Físico");
            btnPortero.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                if (preparador != null)
                {
                    // Actualizar Entrenamiento
                    ComprobarEntrenamientosMaximos(jugadorSeleccionado.IdJugador, 1);
                }
                else
                {
                    MostrarMensajePreparador();
                }
            };
            btnEntradas.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                if (preparador != null)
                {
                    // Actualizar Entrenamiento
                    ComprobarEntrenamientosMaximos(jugadorSeleccionado.IdJugador, 2);
                }
                else
                {
                    MostrarMensajePreparador();
                }
            };
            btnRemate.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                if (preparador != null)
                {
                    // Actualizar Entrenamiento
                    ComprobarEntrenamientosMaximos(jugadorSeleccionado.IdJugador, 3);
                }
                else
                {
                    MostrarMensajePreparador();
                }
            };
            btnPase.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                if (preparador != null)
                {
                    // Actualizar Entrenamiento
                    ComprobarEntrenamientosMaximos(jugadorSeleccionado.IdJugador, 4);
                }
                else
                {
                    MostrarMensajePreparador();
                }
            };
            btnRegate.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                if (preparador != null)
                {
                    // Actualizar Entrenamiento
                    ComprobarEntrenamientosMaximos(jugadorSeleccionado.IdJugador, 5);
                }
                else
                {
                    MostrarMensajePreparador();
                }
            };
            btnTiro.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                if (preparador != null)
                {
                    // Actualizar Entrenamiento
                    ComprobarEntrenamientosMaximos(jugadorSeleccionado.IdJugador, 6);
                }
                else
                {
                    MostrarMensajePreparador();
                }
            };
            btnSinEntrenamiento.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                if (preparador != null)
                {
                    // Actualizar Entrenamiento
                    ComprobarEntrenamientosMaximos(jugadorSeleccionado.IdJugador, 0);
                }
                else
                {
                    MostrarMensajePreparador();
                }
            };

            btnCerrar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                popupContainer.style.display = DisplayStyle.None;
            };
        }

        private void CargarListajugadores(List<Jugador> jugadores)
        {
            jugadoresContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 10f;     // BANDERA
            float col2 = 57f;     // JUGADOR
            float col3 = 15f;     // DEMARCACION
            float col4 = 15f;     // MEDIA

            // COLORES
            Color headerBg = new Color32(56, 78, 63, 255);
            Color headerText = Color.white;

            // CABECERA
            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.backgroundColor = new StyleColor(headerBg);
            header.style.minHeight = 30;
            header.style.maxHeight = 30;
            header.style.width = Length.Percent(100);
            header.style.alignItems = Align.Center;
            header.style.justifyContent = Justify.FlexStart;
            header.style.unityFontStyleAndWeight = FontStyle.Bold;

            header.Add(CreateCell("NAC", col1, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("JUGADOR", col2, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("DEM", col3, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("MED", col4, headerText, TextAnchor.MiddleCenter, true));

            jugadoresContainer.Add(header);

            // FILAS
            int index = 0;
            foreach (var item in jugadores)
            {
                var fila = new VisualElement();
                fila.style.flexDirection = FlexDirection.Row;
                fila.style.width = Length.Percent(100);
                fila.style.minHeight = 30;
                fila.style.maxHeight = 30;
                fila.style.alignItems = Align.Center;

                // Color de fondo de fila alternante (para todas las columnas)
                // Color estándar alterno
                Color filaColor = (index % 2 == 0)
                    ? new Color32(255, 255, 255, 255)     // blanco
                    : new Color32(242, 242, 242, 255);    // gris suave

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                // 1) BANDERA
                var bandera = new VisualElement();
                bandera.style.width = Length.Percent(col1);
                bandera.style.height = 30;
                bandera.style.flexGrow = 0;
                bandera.style.flexShrink = 0;

                var sprite = Resources.Load<Sprite>($"Banderas/{Constants.ObtenerCodigoBanderas(item.Nacionalidad)}");
                if (sprite != null)
                {
                    bandera.style.backgroundImage = new StyleBackground(sprite);
                    bandera.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(bandera);

                // 2) JUGADOR
                fila.Add(CreateCell(item.NombreCompleto, col2, Color.black, TextAnchor.MiddleLeft, false));

                // 3) DEMARCACIÓN
                fila.Add(CreateCell(Constants.RolIdToText(item.RolId), col3, Color.black, TextAnchor.MiddleCenter, false));

                // 4) MED
                fila.Add(CreateCell(item.Media.ToString(), col4, DeterminarColor(item.Media), TextAnchor.MiddleCenter, false));

                // **Registrar evento de click**
                fila.RegisterCallback<MouseDownEvent>(evt =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    jugadorSeleccionado = item;

                    nombreJugador.text = item.NombreCompleto;

                    var fotoSprite = Resources.Load<Sprite>($"Jugadores/{item.IdJugador}");
                    if (fotoSprite != null)
                        fotoJugador.style.backgroundImage = new StyleBackground(fotoSprite);

                    // Comprobar el tipo de entrenamiento del jugador
                    int entrenamiento = JugadorData.EntrenamientoJugador(item.IdJugador);
                    ConfiguracionBotones(entrenamiento);

                });

                jugadoresContainer.Add(fila);
                index++;
            }
        }

        private void ConfiguracionBotones(int entrenamiento)
        {
            // Mostrar la foto y el texto del entrenamiento
            // Diccionario de textos por tipo de entrenamiento
            var textos = new Dictionary<int, string>
            {
                { 1, "PORTERO" },
                { 2, "ENTRADAS" },
                { 3, "REMATE" },
                { 4, "PASE" },
                { 5, "REGATE" },
                { 6, "TIRO" }
            };

            // Diccionario de imágenes (si todas usan "portero", puedes poner una sola ruta)
            var imagenes = new Dictionary<int, string>
            {
                { 1, "Backgrounds/portero" },
                { 2, "Backgrounds/entradas" },
                { 3, "Backgrounds/remate" },
                { 4, "Backgrounds/pase" },
                { 5, "Backgrounds/regate" },
                { 6, "Backgrounds/tiro" }
            };

            // Texto
            textos.TryGetValue(entrenamiento, out string texto);
            tipoEntrenamiento.text = texto ?? "";

            // Imagen
            string imagenPath = imagenes.ContainsKey(entrenamiento)
                ? imagenes[entrenamiento]
                : "Backgrounds/sinEntrenamiento";

            var sprite = Resources.Load<Sprite>(imagenPath);

            if (sprite != null)
                imagenEntrenamiento.style.backgroundImage = new StyleBackground(sprite);


            // Definir los botones en una lista para facilitar el acceso
            var botones = new Dictionary<int, Button>
                        {
                            { 1, btnPortero },
                            { 2, btnEntradas },
                            { 3, btnRemate },
                            { 4, btnPase },
                            { 5, btnRegate },
                            { 6, btnTiro },
                            { 0, btnSinEntrenamiento }
                        };

            foreach (var kvp in botones)
            {
                var btn = kvp.Value;

                if (kvp.Key == entrenamiento)
                {
                    btn.SetEnabled(false);
                }
                else
                {
                    btn.SetEnabled(true);
                }
            }
        }

        private VisualElement CreateCell(string texto, float anchoPercent, Color color, TextAnchor alineacion, bool esHeader)
        {
            var cell = new VisualElement();
            cell.style.width = Length.Percent(anchoPercent);
            cell.style.flexDirection = FlexDirection.Row;
            cell.style.alignItems = Align.Center;

            // Ajustar el positionamiento horizontal según el alineado del texto
            if (alineacion == TextAnchor.MiddleLeft)
                cell.style.justifyContent = Justify.FlexStart;
            else if (alineacion == TextAnchor.MiddleCenter)
                cell.style.justifyContent = Justify.Center;
            else if (alineacion == TextAnchor.MiddleRight)
                cell.style.justifyContent = Justify.FlexEnd;

            var label = new Label(texto);
            label.style.color = color;
            label.style.unityTextAlign = alineacion;

            var fontPath = esHeader
                ? "Fonts/Poppins-SemiBold SDF"
                : "Fonts/Poppins-Regular SDF";

            var fontAsset = Resources.Load<UnityEngine.TextCore.Text.FontAsset>(fontPath);
            label.style.unityFontDefinition = new StyleFontDefinition(fontAsset);
            label.style.fontSize = 16;

            label.style.flexShrink = 0;
            label.style.flexGrow = 0;

            cell.Add(label);
            return cell;
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

        private void ComprobarEntrenamientosMaximos(int idJugadorSeleccionado, int tipo)
        {
            List<Jugador> jugadoresEquipo = JugadorData.ListadoJugadoresCompleto(miEquipo.IdEquipo);
            Empleado? preparador = EmpleadoData.ObtenerEmpleadoPorPuesto("Preparador Físico");
            int maximo = 0;
            if (preparador != null)
            {
                if (preparador.Categoria == 5)
                {
                    maximo = 10;
                }
                else if (preparador.Categoria == 4)
                {
                    maximo = 8;
                }
                else if (preparador.Categoria == 3)
                {
                    maximo = 6;
                }
                else if (preparador.Categoria == 2)
                {
                    maximo = 4;
                }
                else if (preparador.Categoria == 1)
                {
                    maximo = 2;
                }
            }

            int contador = 0;
            foreach (var jugador in jugadoresEquipo)
            {
                if (jugador.Entrenamiento > 0 && jugador.IdJugador != idJugadorSeleccionado)
                {
                    contador++;
                }
            }

            if (contador >= maximo)
            {
                // Mostrar mensaje
                popupText.text = "Ya tienes el máximo de jugadores entrenando. Si quieres entrenar a este jugador debes quitarle el entrenamiento a otro jugador para dejar a su preparador físico libre.";
                popupContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                // Actualizar Entrenamiento
                JugadorData.EntrenarJugador(idJugadorSeleccionado, tipo);
                int entrenamiento = JugadorData.EntrenamientoJugador(idJugadorSeleccionado);
                ConfiguracionBotones(entrenamiento);
            }
        }

        private void MostrarMensajePreparador()
        {
            // Mostrar ventana emergente
            popupContainer.style.display = DisplayStyle.Flex;
            popupText.style.flexWrap = Wrap.Wrap;
            popupText.text = "Para poder desarrollar sesiones específicas para cada jugador, es imprescindible contar con un preparador físico en el cuerpo técnico. Te recomiendo revisar la disponibilidad de profesionales en la sección EMPLEADOS.";
        }
    }
}