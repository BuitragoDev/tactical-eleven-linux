using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class CompeticionesPalmaresJugadores
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root, palmaresContainer, jugadoresContainer, balonOroLogo, botaOroLogo, banner;
        private Label label1, label2, label3;

        public CompeticionesPalmaresJugadores(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            palmaresContainer = root.Q<VisualElement>("palmares-container");
            jugadoresContainer = root.Q<VisualElement>("palmares-jugadores-container");
            balonOroLogo = root.Q<VisualElement>("balonOro-logo");
            balonOroLogo.SetEnabled(false);
            botaOroLogo = root.Q<VisualElement>("botaOro-logo");
            banner = root.Q<VisualElement>("banner");
            label1 = root.Q<Label>("label1");
            label2 = root.Q<Label>("label2");
            label3 = root.Q<Label>("label3");

            // Eventos de los botones de los Premios
            balonOroLogo.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                balonOroLogo.SetEnabled(false);
                botaOroLogo.SetEnabled(true);

                Sprite bannerSprite = Resources.Load<Sprite>($"Backgrounds/bannerBalonOro");
                if (bannerSprite != null)
                    banner.style.backgroundImage = new StyleBackground(bannerSprite);

                label1.text = "BALÓN DE ORO";
                label2.text = "BALÓN DE PLATA";
                label3.text = "BALÓN DE BRONCE";

                CargarPalmares(1);
                CargarPartidosPalmares(1);
            });

            botaOroLogo.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                balonOroLogo.SetEnabled(true);
                botaOroLogo.SetEnabled(false);

                Sprite bannerSprite = Resources.Load<Sprite>($"Backgrounds/bannerBotaOro");
                if (bannerSprite != null)
                    banner.style.backgroundImage = new StyleBackground(bannerSprite);

                label1.text = "BOTA DE ORO";
                label2.text = "BOTA DE PLATA";
                label3.text = "BOTA DE BRONCE";

                CargarPalmares(2);
                CargarPartidosPalmares(2);
            });

            CargarPalmares(1);
            CargarPartidosPalmares(1);
        }

        private void CargarPalmares(int premio)
        {
            List<PalmaresJugador> palmares = null;
            if (premio == 1)
            {
                palmares = PalmaresData.MostrarPalmaresBalonOroTotal();
            }
            else if (premio == 2)
            {
                palmares = PalmaresData.MostrarPalmaresBotaOroTotal();
            }

            palmaresContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 10f;      // FOTO
            float col2 = 72f;      // JUGADOR
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

                // 1) Foto Jugador
                var escudo = new VisualElement();
                escudo.style.width = Length.Percent(col1);
                escudo.style.height = 32;
                var escudoSprite = Resources.Load<Sprite>($"Jugadores/{item.IdJugador}");
                if (escudoSprite != null)
                {
                    escudo.style.backgroundImage = new StyleBackground(escudoSprite);
                    escudo.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(escudo);

                // 2) Jugador
                fila.Add(CreateCell(JugadorData.MostrarDatosJugador(item.IdJugador).NombreCompleto, col2, Color.black, TextAnchor.MiddleLeft, false));

                // 3) Titulos
                fila.Add(CreateCell(item.Titulos.ToString(), col3, Color.black, TextAnchor.MiddleCenter, false));

                palmaresContainer.Add(fila);
                index++;
            }
        }

        private void CargarPartidosPalmares(int premio)
        {
            List<HistorialJugador> historial = null;
            if (premio == 1)
            {
                historial = PalmaresData.MostrarPalmaresBalonOro();
            }
            else if (premio == 2)
            {
                historial = PalmaresData.MostrarPalmaresBotaOro();
            }

            jugadoresContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 15f;       // TEMPORADA
            float col2 = 27f;       // ORO
            float col3 = 27f;       // PLATA
            float col4 = 27f;       // BRONCE

            // FILAS
            int index = 0;
            foreach (var item in historial)
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

                // 2) Oro
                fila.Add(CreateCell(item.NombreJugadorOro, col2, Color.black, TextAnchor.MiddleLeft, false));

                // 3) Plata
                fila.Add(CreateCell(item.NombreJugadorPlata, col3, Color.black, TextAnchor.MiddleLeft, false));

                // 4) Bronce
                fila.Add(CreateCell(item.NombreJugadorBronce, col4, Color.black, TextAnchor.MiddleLeft, false));

                jugadoresContainer.Add(fila);
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
    }
}