using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class CompeticionesPalmaresEquipos
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        int competicionActiva;

        private VisualElement root, palmaresContainer, partidosContainer, competicion1, competicion3, competicion4;
        private Label tituloVentana;

        public CompeticionesPalmaresEquipos(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            palmaresContainer = root.Q<VisualElement>("palmares-container");
            partidosContainer = root.Q<VisualElement>("palmares-partidos-container");
            competicion1 = root.Q<VisualElement>("liga1-logo");
            competicion3 = root.Q<VisualElement>("europa1-logo");
            competicion4 = root.Q<VisualElement>("europa2-logo");
            tituloVentana = root.Q<Label>("titulo-ventana");

            competicionActiva = miEquipo.IdCompeticion;

            // Título de la ventana
            ActualizarTituloVentana(competicionActiva);

            // Iconos de las competiciones
            Sprite competicion1Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/1");
            if (competicion1Sprite != null)
                competicion1.style.backgroundImage = new StyleBackground(competicion1Sprite);

            Sprite competicion5Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/5");
            if (competicion5Sprite != null)
                competicion3.style.backgroundImage = new StyleBackground(competicion5Sprite);

            Sprite competicion6Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/6");
            if (competicion6Sprite != null)
                competicion4.style.backgroundImage = new StyleBackground(competicion6Sprite);

            // Eventos de los botones de las Competiciones
            competicion1.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 1;
                ActualizarTituloVentana(competicionActiva);
                CargarPalmares(competicionActiva);
                CargarPartidosPalmares(competicionActiva);
            });

            competicion3.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 5;
                ActualizarTituloVentana(competicionActiva);
                CargarPalmares(competicionActiva);
                CargarPartidosPalmares(competicionActiva);
            });

            competicion4.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 6;
                ActualizarTituloVentana(competicionActiva);
                CargarPalmares(competicionActiva);
                CargarPartidosPalmares(competicionActiva);
            });

            CargarPalmares(competicionActiva);
            CargarPartidosPalmares(competicionActiva);
        }

        private void CargarPalmares(int comp)
        {
            List<Palmares> palmares = PalmaresData.MostrarPalmaresCompleto(comp);

            palmaresContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 10f;      // ESCUDO
            float col2 = 72f;      // EQUIPO
            float col3 = 15f;      // TITULOS

            // FILAS
            int index = 0;
            foreach (var item in palmares)
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

                // 1) Escudo
                var escudo = new VisualElement();
                escudo.style.width = Length.Percent(col1);
                escudo.style.height = 32;
                var escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{item.IdEquipo}");
                if (escudoSprite != null)
                {
                    escudo.style.backgroundImage = new StyleBackground(escudoSprite);
                    escudo.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(escudo);

                // 2) Equipo
                fila.Add(CreateCell(EquipoData.ObtenerDetallesEquipo(item.IdEquipo).Nombre, col2, Color.black, TextAnchor.MiddleLeft, false));

                // 3) Titulos
                fila.Add(CreateCell(item.Titulos.ToString(), col3, Color.black, TextAnchor.MiddleCenter, false));

                palmaresContainer.Add(fila);
                index++;
            }
        }

        private void CargarPartidosPalmares(int comp)
        {
            List<HistorialFinales> palmares = PalmaresData.MostrarHistorialFinales(comp);

            partidosContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 15f;      // TEMPORADA
            float col2 = 7f;       // ESCUDO CAMPEÓN
            float col3 = 34f;      // CAMPEÓN
            float col4 = 7f;       // ESCUDO SUBCAMPEÓN
            float col5 = 34f;      // SUBCAMPEÓN

            // FILAS
            int index = 0;
            foreach (var item in palmares)
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

                // 1) Temporada
                fila.Add(CreateCell(item.Temporada, col1, Color.black, TextAnchor.MiddleCenter, false));

                // 2) Escudo Campeón
                var escudoCampeon = new VisualElement();
                escudoCampeon.style.width = Length.Percent(col2);
                escudoCampeon.style.height = 32;
                var escudoCampeonSprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{item.IdEquipoCampeon}");
                if (escudoCampeonSprite != null)
                {
                    escudoCampeon.style.backgroundImage = new StyleBackground(escudoCampeonSprite);
                    escudoCampeon.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(escudoCampeon);

                // 3) Equipo Campeón
                fila.Add(CreateCell(EquipoData.ObtenerDetallesEquipo(item.IdEquipoCampeon).Nombre, col3, Color.black, TextAnchor.MiddleLeft, false));

                // 4) Escudo SubCampeón
                var escudoSubcampeon = new VisualElement();
                escudoSubcampeon.style.width = Length.Percent(col4);
                escudoSubcampeon.style.height = 32;
                var escudoSubcampeonSprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{item.IdEquipoFinalista}");
                if (escudoSubcampeonSprite != null)
                {
                    escudoSubcampeon.style.backgroundImage = new StyleBackground(escudoSubcampeonSprite);
                    escudoSubcampeon.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(escudoSubcampeon);

                // 5) Equipo Subcampeón
                fila.Add(CreateCell(EquipoData.ObtenerDetallesEquipo(item.IdEquipoFinalista).Nombre, col5, Color.black, TextAnchor.MiddleLeft, false));

                partidosContainer.Add(fila);
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

        private void ActualizarTituloVentana(int comp)
        {
            tituloVentana.text = $"PALMARÉS ({CompeticionData.MostrarNombreCompeticion(comp).ToUpper()})";
        }
    }
}