using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class ClubLesionados
    {
        private AudioClip clickSFX;

        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root, valoracionContenedor, lesionadosContenedor;
        private Label nombreMedico, tiempoRecuperacion;
        private Button btnTratarLesion;

        int reputacionMedico = 0;
        int reduccion = 0;
        Jugador jugadorSeleccionado = null;
        Empleado medico = null;

        public ClubLesionados(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            valoracionContenedor = root.Q<VisualElement>("estrellas-contenedor");
            lesionadosContenedor = root.Q<VisualElement>("lesionados-contenedor");
            nombreMedico = root.Q<Label>("medico-nombre");
            tiempoRecuperacion = root.Q<Label>("tiempo-recuperacion");
            btnTratarLesion = root.Q<Button>("btnTratarLesion");
            btnTratarLesion.SetEnabled(false);

            CargarEquipoMedico();
            CargarListaLesionados();

            btnTratarLesion.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                int jugadorTratado = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador).LesionTratada;
                if (jugadorSeleccionado != null && jugadorTratado != 1)
                {
                    JugadorData.TratarLesion(jugadorSeleccionado.IdJugador, reduccion);

                    JugadorData.ActivarTratamientoLesion(jugadorSeleccionado.IdJugador, 1);

                    CargarListaLesionados();

                    jugadorTratado = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador).LesionTratada;
                    if (jugadorSeleccionado != null && jugadorTratado == 1)
                    {
                        btnTratarLesion.SetEnabled(false);
                    }
                    else
                    {
                        btnTratarLesion.SetEnabled(true);
                    }
                }
            };
        }

        private void CargarEquipoMedico()
        {
            medico = EmpleadoData.ObtenerEmpleadoPorPuesto("Equipo Médico");

            if (medico != null)
            {
                nombreMedico.text = medico.Nombre;
                reputacionMedico = medico.Categoria;
                MostrarEstrellas(reputacionMedico, valoracionContenedor);

                string[] tratamiento = {
                    "Nuestro equipo médico puede reducir el tiempo de recuperación un 10%.",
                    "Nuestro equipo médico puede reducir el tiempo de recuperación un 20%.",
                    "Nuestro equipo médico puede reducir el tiempo de recuperación un 30%.",
                    "Nuestro equipo médico puede reducir el tiempo de recuperación un 40%.",
                    "Nuestro equipo médico puede reducir el tiempo de recuperación un 50%."
                };

                if (reputacionMedico >= 1 && reputacionMedico <= 5)
                {
                    tiempoRecuperacion.text = tratamiento[reputacionMedico - 1];
                }

                int[] porcentaje = {
                        10, 20, 30, 40, 50
                    };

                if (medico.Categoria >= 1 && medico.Categoria <= 5)
                {
                    reduccion = porcentaje[medico.Categoria - 1];
                }
            }
            else
            {
                btnTratarLesion.SetEnabled(false);
                nombreMedico.text = "No tienes ningún equipo médico contratado";
                tiempoRecuperacion.text = "-";
            }
        }

        private void CargarListaLesionados()
        {
            List<Jugador> listaJugadores = JugadorData.ListadoJugadoresCompleto(miEquipo.IdEquipo);

            lesionadosContenedor.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 20f;    // JUGADOR
            float col2 = 58f;    // TIPO LESIÓN
            float col3 = 13f;    // SEMANAS
            float col4 = 5f;    // TRATAMIENTO

            // COLORES
            Color headerBg = new Color32(30, 30, 30, 255);
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

            header.Add(CreateCell("JUGADOR", col1, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("TIPO DE LESIÓN", col2, headerText, TextAnchor.MiddleLeft, true));

            header.Add(CreateCell("SEMANAS", col3, headerText, TextAnchor.MiddleLeft, true));

            var headerEmpty = new VisualElement();
            headerEmpty.style.width = Length.Percent(col4);
            headerEmpty.style.height = 30;
            headerEmpty.style.flexGrow = 0;
            headerEmpty.style.flexShrink = 0;
            header.Add(headerEmpty);

            lesionadosContenedor.Add(header);

            // FILAS
            int index = 0;
            foreach (var item in listaJugadores)
            {
                if (item.Lesion > 0)
                {
                    var fila = new VisualElement();
                    fila.style.flexDirection = FlexDirection.Row;
                    fila.style.width = Length.Percent(100);
                    fila.style.minHeight = 50;
                    fila.style.maxHeight = 50;
                    fila.style.alignItems = Align.Center;

                    // Color de fondo de fila alternante (para todas las columnas)
                    // Color estándar alterno
                    Color filaColor = (index % 2 == 0)
                        ? new Color32(255, 255, 255, 255)     // blanco
                        : new Color32(242, 242, 242, 255);    // gris suave

                    // Aplicar color final
                    fila.userData = filaColor;

                    // 1) JUGADOR
                    fila.Add(CreateCell(JugadorData.MostrarDatosJugador(item.IdJugador).NombreCompleto, col1, Color.black, TextAnchor.MiddleLeft, false));

                    // 2) TIPO LESIÓN
                    fila.Add(CreateCell(item.TipoLesion, col2, Color.black, TextAnchor.MiddleLeft, false));

                    // 3) SEMANAS
                    string lesionTexto = item.Lesion > 0
                        ? $"{item.Lesion} semanas"
                        : string.Empty; // o "OK", o vacío

                    Color lesionColor = item.LesionTratada == 1
                    ? (Color)new Color32(0xA3, 0x1E, 0x1E, 0xFF)
                    : Color.black;

                    fila.Add(CreateCell(lesionTexto, col3, lesionColor, TextAnchor.MiddleLeft, false));

                    // 4) LESIÓN TRATADA
                    if (item.LesionTratada == 1)
                    {
                        var lesionTratada = new VisualElement();
                        lesionTratada.style.width = Length.Percent(col4);
                        lesionTratada.style.height = 30;
                        lesionTratada.style.flexGrow = 0;
                        lesionTratada.style.flexShrink = 0;

                        var sprite = Resources.Load<Sprite>($"Icons/tratamiento");
                        if (sprite != null)
                        {
                            lesionTratada.style.backgroundImage = new StyleBackground(sprite);
                            lesionTratada.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                        }

                        fila.Add(lesionTratada);
                    }
                    else
                    {
                        fila.Add(CreateCell(string.Empty, col3, Color.black, TextAnchor.MiddleCenter, false));
                    }


                    // **Registrar evento de click**
                    fila.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        jugadorSeleccionado = item; // Guardar jugador seleccionado

                        foreach (var f in lesionadosContenedor.Children())
                        {
                            if (f == fila)
                                f.style.backgroundColor = new StyleColor(new Color32(235, 203, 197, 255)); // color seleccionado
                            else if (f.userData is Color originalColor)
                                f.style.backgroundColor = new StyleColor(new Color32(229, 229, 229, 255)); // restaurar color original
                        }

                        // Habilitar botón si corresponde
                        int jugadorTratado = JugadorData.MostrarDatosJugador(jugadorSeleccionado.IdJugador).LesionTratada;
                        btnTratarLesion.SetEnabled(jugadorSeleccionado != null && jugadorTratado != 1 && medico != null);
                    });

                    lesionadosContenedor.Add(fila);
                    index++;
                }
            }
        }

        private VisualElement CreateCell(string texto, float anchoPercent, Color color, TextAnchor alineacion, bool esHeader)
        {
            var label = new Label(texto);

            label.style.width = Length.Percent(anchoPercent);
            label.style.color = color;
            label.style.unityTextAlign = alineacion;

            // Fuente Poppins-Bold
            var fontPath = esHeader
                ? "Fonts/Poppins-SemiBold SDF"
                : "Fonts/Poppins-Regular SDF";

            var fontAsset = Resources.Load<UnityEngine.TextCore.Text.FontAsset>(fontPath);
            label.style.unityFontDefinition = new StyleFontDefinition(fontAsset);

            label.style.fontSize = 18;

            // Flex para que no se comprima ni expanda
            label.style.display = DisplayStyle.Flex;
            label.style.flexDirection = FlexDirection.Row;
            label.style.alignItems = Align.Center;
            label.style.flexShrink = 0;
            label.style.flexGrow = 0;

            // Padding según alineación
            label.style.paddingLeft = (alineacion == TextAnchor.MiddleLeft) ? 6 : 0;
            label.style.paddingRight = (alineacion == TextAnchor.MiddleLeft) ? 0 : 6;

            return label;
        }

        private void MostrarEstrellas(int reputacion, VisualElement contenedor)
        {
            contenedor.Clear();

            Sprite estrellaON = Resources.Load<Sprite>("Icons/estrellaOn");
            Sprite estrellaOFF = Resources.Load<Sprite>("Icons/estrellaOff");

            if (estrellaON == null || estrellaOFF == null)
            {
                Debug.LogError("No se pudieron cargar las imágenes de las estrellas");
                return;
            }

            int numeroEstrellas = 0;

            if (reputacion == 5)
            {
                numeroEstrellas = 5;
            }
            else if (reputacion == 4)
            {
                numeroEstrellas = 4;
            }
            else if (reputacion == 3)
            {
                numeroEstrellas = 3;
            }
            else if (reputacion == 2)
            {
                numeroEstrellas = 2;
            }
            else if (reputacion == 1)
            {
                numeroEstrellas = 1;
            }
            else
            {
                numeroEstrellas = 0;
            }

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
                contenedor.Add(estrella);
            }

            contenedor.style.flexDirection = FlexDirection.Row;
        }
    }
}