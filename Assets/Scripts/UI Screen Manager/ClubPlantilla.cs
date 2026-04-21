using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class ClubPlantilla
    {
        private AudioClip clickSFX;
        private MainScreen mainScreen;
        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root, plantillaContenedor, puntuacion, datos, estadisticas;


        public ClubPlantilla(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            plantillaContenedor = root.Q<VisualElement>("plantilla-contenedor");
            puntuacion = root.Q<VisualElement>("puntuacion-icon");
            datos = root.Q<VisualElement>("datos-icon");
            estadisticas = root.Q<VisualElement>("estadisticas-icon");

            CargarPlantilla(miEquipo.IdEquipo, 1);

            puntuacion.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarPlantilla(miEquipo.IdEquipo, 1);
            });

            datos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarPlantilla(miEquipo.IdEquipo, 2);
            });

            estadisticas.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarPlantilla(miEquipo.IdEquipo, 3);
            });
        }

        private void CargarPlantilla(int equipo, int opcion)
        {
            if (opcion == 1)
            {
                List<Jugador> jugadores = JugadorData.ListadoJugadoresCompleto(equipo);

                plantillaContenedor.Clear();

                // PORCENTAJES DE COLUMNAS
                float col1 = 5f;    // Nº
                float col2 = 5f;    // BANDERA
                float col3 = 20f;   // JUGADOR
                float col4 = 10f;    // DEMARCACION
                float col5 = 10f;   // ALTURA
                float col6 = 5f;    // EDAD
                float col7 = 5f;    // MED
                float col8 = 5f;    // MO
                float col9 = 5f;    // EF
                float col10 = 10f;  // LESIONADO
                float col11 = 10f;  // SANCIONADO
                float col12 = 5f;   // AÑOS CONTRATO

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

                header.Add(CreateCell("Nº", col1, headerText, TextAnchor.MiddleCenter, true));

                var headerEmpty = new VisualElement();
                headerEmpty.style.width = Length.Percent(col2);
                headerEmpty.style.height = 30;
                headerEmpty.style.flexGrow = 0;
                headerEmpty.style.flexShrink = 0;
                header.Add(headerEmpty);

                header.Add(CreateCell("JUGADOR", col3, headerText, TextAnchor.MiddleLeft, true));
                header.Add(CreateCell("DEMARCACIÓN", col4, headerText, TextAnchor.MiddleLeft, true));
                header.Add(CreateCell("ALTURA", col5, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("EDAD", col6, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("MED", col7, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("MO", col8, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("EF", col9, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("LESIONADO", col10, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("SANCIONADO", col11, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("AÑOS", col12, headerText, TextAnchor.MiddleCenter, true));

                plantillaContenedor.Add(header);

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

                    // 1) Nº
                    fila.Add(CreateCell(item.Dorsal.ToString(), col1, Color.black, TextAnchor.MiddleCenter, false));

                    // 2) BANDERA
                    var bandera = new VisualElement();
                    bandera.style.width = Length.Percent(col2);
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

                    // 3) JUGADOR
                    fila.Add(CreateCell(item.NombreCompleto, col3, Color.black, TextAnchor.MiddleLeft, false));

                    // 4) DEMARCACIÓN
                    fila.Add(CreateCell(item.Rol.ToString(), col4, Color.black, TextAnchor.MiddleLeft, false));

                    // 5) ALTURA (cm → m)
                    float alturaMetros = item.Altura / 100f;
                    string alturaTexto = $"{alturaMetros:0.00} m";

                    fila.Add(CreateCell(alturaTexto, col5, Color.black, TextAnchor.MiddleCenter, false));

                    // 6) EDAD
                    fila.Add(CreateCell(item.Edad.ToString(), col6, Color.black, TextAnchor.MiddleCenter, false));

                    // 7) MED
                    fila.Add(CreateCell(item.Media.ToString(), col7, Color.black, TextAnchor.MiddleCenter, false));

                    // 8) MO
                    fila.Add(CreateCell(item.Moral.ToString(), col8, Color.black, TextAnchor.MiddleCenter, false));

                    // 9) EF
                    fila.Add(CreateCell(item.EstadoForma.ToString(), col9, Color.black, TextAnchor.MiddleCenter, false));

                    // 10) LESIONADO
                    string lesionTexto = item.Lesion > 0
                        ? $"{item.Lesion} semanas"
                        : string.Empty; // o "OK", o vacío

                    Color lesionColor = item.Lesion > 0
                        ? (Color)new Color32(0xA3, 0x1E, 0x1E, 0xFF)
                        : Color.black;

                    fila.Add(CreateCell(lesionTexto, col10, lesionColor, TextAnchor.MiddleCenter, false));

                    // 11) SANCIONADO
                    string sancionTexto = item.Sancionado > 0
                        ? $"{item.Sancionado} partidos"
                        : string.Empty; // o "OK", o vacío

                    Color sancionColor = item.Sancionado > 0
                        ? (Color)new Color32(0xA3, 0x1E, 0x1E, 0xFF)
                        : Color.black;

                    fila.Add(CreateCell(sancionTexto, col11, sancionColor, TextAnchor.MiddleCenter, false));

                    // 12) AÑOS CONTRATO
                    fila.Add(CreateCell(item.AniosContrato.ToString(), col12, (item.AniosContrato == 1) ? (Color)new Color32(0xA3, 0x1E, 0x1E, 0xFF) : Color.black, TextAnchor.MiddleCenter, false));

                    // **Registrar evento de click**
                    fila.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);
                        UIManager.Instance.CargarPantalla("UI/Ficha/Ficha", instancia =>
                        {
                            new FichaJugador(instancia, miEquipo, miManager, item.IdJugador, 1, mainScreen);
                        });
                    });

                    plantillaContenedor.Add(fila);
                    index++;
                }
            }
            else if (opcion == 2)
            {
                List<Jugador> jugadores = JugadorData.ListadoJugadoresCompleto(equipo);

                plantillaContenedor.Clear();

                // PORCENTAJES DE COLUMNAS
                float col1 = 5f;    // Nº
                float col2 = 5f;    // BANDERA
                float col3 = 20f;   // JUGADOR
                float col4 = 5f;    // DEMARCACION
                float col5 = 5f;    // VEL
                float col6 = 5f;    // RES
                float col7 = 5f;    // AGR
                float col8 = 5f;    // CAL
                float col9 = 5f;    // POT
                float col10 = 5f;   // POR
                float col11 = 5f;   // PAS
                float col12 = 5f;   // REG
                float col13 = 5f;   // REM
                float col14 = 5f;   // ENT
                float col15 = 5f;   // TIR

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

                header.Add(CreateCell("Nº", col1, headerText, TextAnchor.MiddleCenter, true));

                var headerEmpty = new VisualElement();
                headerEmpty.style.width = Length.Percent(col2);
                headerEmpty.style.height = 30;
                headerEmpty.style.flexGrow = 0;
                headerEmpty.style.flexShrink = 0;
                header.Add(headerEmpty);

                header.Add(CreateCell("JUGADOR", col3, headerText, TextAnchor.MiddleLeft, true));
                header.Add(CreateCell("DEM", col4, headerText, TextAnchor.MiddleLeft, true));
                header.Add(CreateCell("VEL", col5, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("RES", col6, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("AGR", col7, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("CAL", col8, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("POT", col9, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("POR", col10, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("PAS", col11, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("REG", col12, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("REM", col13, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("ENT", col14, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("TIR", col15, headerText, TextAnchor.MiddleCenter, true));

                plantillaContenedor.Add(header);

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

                    // 1) Nº
                    fila.Add(CreateCell(item.Dorsal.ToString(), col1, Color.black, TextAnchor.MiddleCenter, false));

                    // 2) BANDERA
                    var bandera = new VisualElement();
                    bandera.style.width = Length.Percent(col2);
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

                    // 3) JUGADOR
                    fila.Add(CreateCell(item.NombreCompleto, col3, Color.black, TextAnchor.MiddleLeft, false));

                    // 4) DEMARCACIÓN
                    string demarcacion = Constants.RolIdToText(item.RolId);
                    fila.Add(CreateCell(demarcacion, col4, Color.black, TextAnchor.MiddleLeft, false));

                    // 5) VEL
                    fila.Add(CreateCell(item.Velocidad.ToString(), col5, Color.black, TextAnchor.MiddleCenter, false));

                    // 6) RES
                    fila.Add(CreateCell(item.Resistencia.ToString(), col6, Color.black, TextAnchor.MiddleCenter, false));

                    // 7) AGR
                    fila.Add(CreateCell(item.Agresividad.ToString(), col7, Color.black, TextAnchor.MiddleCenter, false));

                    // 8) CAL
                    fila.Add(CreateCell(item.Calidad.ToString(), col8, Color.black, TextAnchor.MiddleCenter, false));

                    // 9) POT
                    fila.Add(CreateCell(item.Potencial.ToString(), col9, Color.black, TextAnchor.MiddleCenter, false));

                    // 10) POR
                    fila.Add(CreateCell(item.Portero.ToString(), col10, Color.black, TextAnchor.MiddleCenter, false));

                    // 11) PAS
                    fila.Add(CreateCell(item.Pase.ToString(), col11, Color.black, TextAnchor.MiddleCenter, false));

                    // 12) REG
                    fila.Add(CreateCell(item.Regate.ToString(), col12, Color.black, TextAnchor.MiddleCenter, false));

                    // 13) REM
                    fila.Add(CreateCell(item.Remate.ToString(), col13, Color.black, TextAnchor.MiddleCenter, false));

                    // 14) ENT
                    fila.Add(CreateCell(item.Entradas.ToString(), col14, Color.black, TextAnchor.MiddleCenter, false));

                    // 15) TIR
                    fila.Add(CreateCell(item.Tiro.ToString(), col15, Color.black, TextAnchor.MiddleCenter, false));

                    plantillaContenedor.Add(fila);
                    index++;
                }
            }
            else if (opcion == 3)
            {
                List<Estadistica> estadisticas = EstadisticaJugadorData.MostrarEstadisticasEquipo(miEquipo.IdEquipo);

                plantillaContenedor.Clear();

                // PORCENTAJES DE COLUMNAS
                float col1 = 5f;    // Nº
                float col2 = 5f;    // BANDERA
                float col3 = 20f;   // JUGADOR
                float col4 = 5f;    // DEMARCACION
                float col5 = 10f;    // PJ
                float col6 = 10f;    // GOLES
                float col7 = 10f;    // ASISTENCIAS
                float col8 = 10f;    // TA
                float col9 = 10f;    // TR
                float col10 = 10f;   // MVP

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

                header.Add(CreateCell("Nº", col1, headerText, TextAnchor.MiddleCenter, true));

                var headerEmpty = new VisualElement();
                headerEmpty.style.width = Length.Percent(col2);
                headerEmpty.style.height = 30;
                headerEmpty.style.flexGrow = 0;
                headerEmpty.style.flexShrink = 0;
                header.Add(headerEmpty);

                header.Add(CreateCell("JUGADOR", col3, headerText, TextAnchor.MiddleLeft, true));
                header.Add(CreateCell("DEM", col4, headerText, TextAnchor.MiddleLeft, true));
                header.Add(CreateCell("PARTIDOS", col5, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("GOLES", col6, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("ASISTENCIAS", col7, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("T.AMARILLAS", col8, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("T.ROJAS", col9, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("MVP", col10, headerText, TextAnchor.MiddleCenter, true));

                plantillaContenedor.Add(header);

                // FILAS
                int index = 0;
                foreach (var item in estadisticas)
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

                    // 1) Nº
                    fila.Add(CreateCell(item.Dorsal.ToString(), col1, Color.black, TextAnchor.MiddleCenter, false));

                    // 2) BANDERA
                    var bandera = new VisualElement();
                    bandera.style.width = Length.Percent(col2);
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

                    // 3) JUGADOR
                    fila.Add(CreateCell(item.NombreCompleto, col3, Color.black, TextAnchor.MiddleLeft, false));

                    // 4) DEMARCACIÓN
                    string demarcacion = Constants.RolIdToText(JugadorData.MostrarDatosJugador(item.IdJugador).RolId);
                    fila.Add(CreateCell(demarcacion, col4, Color.black, TextAnchor.MiddleLeft, false));

                    // 5) PJ
                    fila.Add(CreateCell(item.PartidosJugados.ToString(), col5, Color.black, TextAnchor.MiddleCenter, false));

                    // 6) GOLES
                    fila.Add(CreateCell(item.Goles.ToString(), col6, Color.black, TextAnchor.MiddleCenter, false));

                    // 7) ASISTENCIAS
                    fila.Add(CreateCell(item.Asistencias.ToString(), col7, Color.black, TextAnchor.MiddleCenter, false));

                    // 8) TA
                    fila.Add(CreateCell(item.TarjetasAmarillas.ToString(), col8, Color.black, TextAnchor.MiddleCenter, false));

                    // 9) TR
                    fila.Add(CreateCell(item.TarjetasRojas.ToString(), col9, Color.black, TextAnchor.MiddleCenter, false));

                    // 10) MVP
                    fila.Add(CreateCell(item.MVP.ToString(), col10, Color.black, TextAnchor.MiddleCenter, false));

                    plantillaContenedor.Add(fila);
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

            label.style.fontSize = 16;

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
    }
}