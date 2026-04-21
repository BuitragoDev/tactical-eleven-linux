using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class ManagerPalmares
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root, copaContainer, historialContainer;
        private Label palmaresTexto, competicion1, competicion2, competicion3, competicion4, competicion5;

        public ManagerPalmares(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");


            // Referencias a objetos de la UI
            copaContainer = root.Q<VisualElement>("copas-container");
            historialContainer = root.Q<VisualElement>("historial-container");
            historialContainer.style.flexShrink = 0;
            historialContainer.style.flexGrow = 0;
            palmaresTexto = root.Q<Label>("palmares-texto");
            competicion1 = root.Q<Label>("lblCompeticion1-value");
            competicion2 = root.Q<Label>("lblCompeticion2-value");
            competicion3 = root.Q<Label>("lblCompeticion3-value");
            competicion4 = root.Q<Label>("lblCompeticion4-value");
            competicion5 = root.Q<Label>("lblCompeticion5-value");
            CargarPalmares();
            CargarHistorial();
        }

        private void CargarHistorial()
        {
            List<Historial> historial = HistorialData.MostrarHistorialManager();
            int filas = historial.Count;
            historialContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 10f;     // TEMPORADA
            float col2;
            if (filas <= 13)
            {
                col2 = 27f; // EQUIPO
            }
            else
            {
                col2 = 28f; // EQUIPO
            }

            float col3 = 5f;      // POS
            float col4 = 5f;      // PJ
            float col5 = 5f;      // PG
            float col6 = 5f;      // PE
            float col7 = 5f;      // PP
            float col8 = 5f;      // GF
            float col9 = 5f;      // GC
            float col10 = 6f;     // TITULOS
            float col11 = 6f;     // CDIRECTIVA
            float col12 = 6f;     // CFANS
            float col13 = 6f;     // CJUGADORES

            // FILAS
            int index = 0;
            foreach (var item in historial)
            {
                var fila = new VisualElement();
                fila.style.flexDirection = FlexDirection.Row;
                fila.style.width = Length.Percent(100);
                fila.style.minHeight = 40;
                fila.style.maxHeight = 40;
                fila.style.alignItems = Align.Center;

                // Color de fondo de fila alternante (para todas las columnas)
                // Color estándar alterno
                Color filaColor = (index % 2 == 0)
                    ? new Color32(255, 255, 255, 255)     // blanco
                    : new Color32(242, 242, 242, 255);    // gris suave

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                // 1) TEMPORADA
                fila.Add(CreateCell(item.Temporada, col1, Color.black, TextAnchor.MiddleCenter, false));

                // 2) EQUIPO
                fila.Add(CreateCell(item.NombreEquipo, col2, Color.black, TextAnchor.MiddleLeft, true));

                // 3) POS
                fila.Add(CreateCell(item.PosicionLiga.ToString(), col3, Color.black, TextAnchor.MiddleCenter, false));

                // 4) PJ
                fila.Add(CreateCell(item.PartidosJugados.ToString(), col4, Color.black, TextAnchor.MiddleCenter, false));

                // 5) PG
                fila.Add(CreateCell(item.PartidosGanados.ToString(), col5, Color.black, TextAnchor.MiddleCenter, false));

                // 6) PE
                fila.Add(CreateCell(item.PartidosEmpatados.ToString(), col6, Color.black, TextAnchor.MiddleCenter, false));

                // 7) PP
                fila.Add(CreateCell(item.PartidosPerdidos.ToString(), col7, Color.black, TextAnchor.MiddleCenter, false));

                // 8) GF
                fila.Add(CreateCell(item.GolesMarcados.ToString(), col8, Color.black, TextAnchor.MiddleCenter, false));

                // 9) GC
                fila.Add(CreateCell(item.GolesRecibidos.ToString(), col9, Color.black, TextAnchor.MiddleCenter, false));

                // 10) TITULOS
                fila.Add(CreateCell(item.TitulosInternacionales.ToString(), col10, Color.black, TextAnchor.MiddleCenter, false));

                // 11) CDIRECTIVA
                fila.Add(CreateCell(item.CDirectiva.ToString(), col11, Color.black, TextAnchor.MiddleCenter, false));

                // 12) CFANS
                fila.Add(CreateCell(item.CFans.ToString(), col12, Color.black, TextAnchor.MiddleCenter, false));

                // 13) CJUGADORES
                fila.Add(CreateCell(item.CJugadores.ToString(), col13, Color.black, TextAnchor.MiddleCenter, false));

                historialContainer.Add(fila);
                index++;
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

        private void CargarPalmares()
        {
            // Elementos del Palmarés
            List<PalmaresManager> palmares = PalmaresData.MostrarPalmaresManager(miEquipo.IdEquipo);
            int contador = palmares.Count;
            palmaresTexto.text = $"PALMARÉS ({contador})";

            int contadorComp1 = 0;
            int contadorComp2 = 0;
            int contadorComp3 = 0;
            int contadorComp4 = 0;
            int contadorComp5 = 0;

            foreach (var item in palmares)
            {
                switch (item.IdCompeticion)
                {
                    case 1:
                        contadorComp1++;
                        break;
                    case 2:
                        contadorComp2++;
                        break;
                    case 4:
                        contadorComp3++;
                        break;
                    case 5:
                        contadorComp4++;
                        break;
                    case 6:
                        contadorComp5++;
                        break;
                }
            }

            competicion1.text = contadorComp1.ToString();
            competicion2.text = contadorComp2.ToString();
            competicion3.text = contadorComp3.ToString();
            competicion4.text = contadorComp4.ToString();
            competicion5.text = contadorComp5.ToString();
        }
    }
}