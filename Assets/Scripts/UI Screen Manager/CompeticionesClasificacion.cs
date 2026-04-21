using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class CompeticionesClasificacion
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        Equipo equipo;

        private VisualElement root, clasificacionContainer, competicion1, competicion2, competicion3, competicion4,
                              mejorAtaqueEscudo, mejorDefensaEscudo, mejorRachaEscudo, mejorLocalEscudo, mejorVisitanteEscudo;
        private Label tituloVentana, mejorAtaqueNombre, mejorDefensaNombre, mejorRachaNombre, mejorLocalNombre, mejorVisitanteNombre,
                      mejorAtaqueValue, mejorDefensaValue, mejorRachaValue, mejorLocalValue, mejorVisitanteValue;

        public CompeticionesClasificacion(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            clasificacionContainer = root.Q<VisualElement>("clasificacion-container");
            competicion1 = root.Q<VisualElement>("liga1-logo");
            competicion2 = root.Q<VisualElement>("liga2-logo");
            competicion3 = root.Q<VisualElement>("europa1-logo");
            competicion4 = root.Q<VisualElement>("europa2-logo");
            mejorAtaqueEscudo = root.Q<VisualElement>("mejorAtaque-escudo");
            mejorDefensaEscudo = root.Q<VisualElement>("mejorDefensa-escudo");
            mejorRachaEscudo = root.Q<VisualElement>("mejorRacha-escudo");
            mejorLocalEscudo = root.Q<VisualElement>("mejorLocal-escudo");
            mejorVisitanteEscudo = root.Q<VisualElement>("mejorVisitante-escudo");
            tituloVentana = root.Q<Label>("titulo-ventana");
            mejorAtaqueNombre = root.Q<Label>("mejorAtaque-nombre");
            mejorDefensaNombre = root.Q<Label>("mejorDefensa-nombre");
            mejorRachaNombre = root.Q<Label>("mejorRacha-nombre");
            mejorLocalNombre = root.Q<Label>("mejorLocal-nombre");
            mejorVisitanteNombre = root.Q<Label>("mejorVisitante-nombre");
            mejorAtaqueValue = root.Q<Label>("mejorAtaque-value");
            mejorDefensaValue = root.Q<Label>("mejorDefensa-value");
            mejorRachaValue = root.Q<Label>("mejorRacha-value");
            mejorLocalValue = root.Q<Label>("mejorLocal-value");
            mejorVisitanteValue = root.Q<Label>("mejorVisitante-value");

            competicion1.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClasificacion(1);
                CargarMejoresEquipos(1);
            });

            competicion2.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClasificacion(2);
                CargarMejoresEquipos(2);
            });

            competicion3.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClasificacion(5);
                CargarMejoresEquipos(5);
            });

            competicion4.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClasificacion(6);
                CargarMejoresEquipos(6);
            });

            // Título de la ventana
            tituloVentana.text = $"CLASIFICACIÓN {CompeticionData.MostrarNombreCompeticion(miEquipo.IdCompeticion).ToUpper()}";

            // Iconos de las competiciones
            Sprite competicion1Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/1");
            if (competicion1Sprite != null)
                competicion1.style.backgroundImage = new StyleBackground(competicion1Sprite);

            Sprite competicion2Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/2");
            if (competicion2Sprite != null)
                competicion2.style.backgroundImage = new StyleBackground(competicion2Sprite);

            Sprite competicion5Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/5");
            if (competicion5Sprite != null)
                competicion3.style.backgroundImage = new StyleBackground(competicion5Sprite);

            Sprite competicion6Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/6");
            if (competicion6Sprite != null)
                competicion4.style.backgroundImage = new StyleBackground(competicion6Sprite);

            CargarClasificacion(miEquipo.IdCompeticion);
            CargarMejoresEquipos(miEquipo.IdCompeticion);
        }

        private void CargarMejoresEquipos(int comp)
        {
            // Mostrar Mejor Ataque
            Clasificacion mejorAtaque = ClasificacionData.MostrarMejorAtaque(comp);
            equipo = EquipoData.ObtenerDetallesEquipo(mejorAtaque.IdEquipo);
            mejorAtaqueNombre.text = mejorAtaque.NombreEquipo;
            mejorAtaqueValue.text = mejorAtaque.GolesFavor.ToString();
            Sprite mejorAtaqueSprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{equipo.IdEquipo}");
            if (mejorAtaqueSprite != null)
                mejorAtaqueEscudo.style.backgroundImage = new StyleBackground(mejorAtaqueSprite);

            // Mostrar Mejor Defensa
            Clasificacion mejorDefensa = ClasificacionData.MostrarMejorDefensa(comp);
            equipo = EquipoData.ObtenerDetallesEquipo(mejorDefensa.IdEquipo);
            mejorDefensaNombre.text = mejorDefensa.NombreEquipo;
            mejorDefensaValue.text = mejorDefensa.GolesContra.ToString();
            Sprite mejorDefensaSprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{equipo.IdEquipo}");
            if (mejorDefensaSprite != null)
                mejorDefensaEscudo.style.backgroundImage = new StyleBackground(mejorDefensaSprite);

            // Mostrar Mejor Racha
            Clasificacion mejorRacha = ClasificacionData.MostrarMejorRacha(comp);
            equipo = EquipoData.ObtenerDetallesEquipo(mejorRacha.IdEquipo);
            mejorRachaNombre.text = mejorRacha.NombreEquipo;
            mejorRachaValue.text = mejorRacha.Racha.ToString();
            Sprite mejorRachaSprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{equipo.IdEquipo}");
            if (mejorRachaSprite != null)
                mejorRachaEscudo.style.backgroundImage = new StyleBackground(mejorRachaSprite);

            // Mejor Equipo Local
            Clasificacion mejorLocal = ClasificacionData.MostrarMejorEquipoLocal(comp);
            equipo = EquipoData.ObtenerDetallesEquipo(mejorLocal.IdEquipo);
            mejorLocalNombre.text = mejorLocal.NombreEquipo;
            mejorLocalValue.text = mejorLocal.LocalVictorias.ToString();
            Sprite mejorLocalSprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{equipo.IdEquipo}");
            if (mejorLocalSprite != null)
                mejorLocalEscudo.style.backgroundImage = new StyleBackground(mejorLocalSprite);

            // Mejor Equipo Visitante
            Clasificacion mejorVisitante = ClasificacionData.MostrarMejorEquipoVisitante(comp);
            equipo = EquipoData.ObtenerDetallesEquipo(mejorVisitante.IdEquipo);
            mejorVisitanteNombre.text = mejorVisitante.NombreEquipo;
            mejorVisitanteValue.text = mejorVisitante.VisitanteVictorias.ToString();
            Sprite mejorVisitanteSprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{equipo.IdEquipo}");
            if (mejorVisitanteSprite != null)
                mejorVisitanteEscudo.style.backgroundImage = new StyleBackground(mejorVisitanteSprite);
        }

        private void CargarClasificacion(int comp)
        {
            clasificacionContainer.Clear();

            var cl = ClasificacionData.MostrarClasificacion(comp);

            // PORCENTAJES DE COLUMNAS
            float col1 = 5f;     // columna vacía / indicador clasificación
            float col2 = 5f;     // POS
            float col3 = 10f;    // ESCUDO
            float col4 = 45f;    // EQUIPO
            float col5 = 5f;     // PJ
            float col6 = 5f;     // PG
            float col7 = 5f;     // PE
            float col8 = 5f;     // PP
            float col9 = 5f;     // GF
            float col10 = 5f;    // GC
            float col11 = 5f;    // PTOS


            // COLORES
            Color headerBg = new Color32(56, 78, 63, 255);
            Color headerText = Color.white;

            // CABECERA
            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.backgroundColor = new StyleColor(headerBg);
            header.style.minHeight = 38;
            header.style.maxHeight = 38;
            header.style.width = Length.Percent(100);
            header.style.alignItems = Align.Center;
            header.style.justifyContent = Justify.FlexStart;
            header.style.unityFontStyleAndWeight = FontStyle.Bold;

            // Columna vacía de cabecera
            var headerEmpty = new VisualElement();
            headerEmpty.style.width = Length.Percent(col1);
            headerEmpty.style.height = 38;
            header.Add(headerEmpty);

            header.Add(CreateCell("POS", col2, headerText, TextAnchor.MiddleCenter, true));

            // Columna ESCUDO de cabecera
            var headerEscudo = new VisualElement();
            headerEscudo.style.width = Length.Percent(col3);
            headerEscudo.style.height = 38;
            header.Add(headerEscudo);

            header.Add(CreateCell("EQUIPO", col4, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("PJ", col5, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("PG", col6, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("PE", col7, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("PP", col8, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("GF", col9, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("GC", col10, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("PTOS", col11, headerText, TextAnchor.MiddleCenter, true));

            clasificacionContainer.Add(header);

            // FILAS
            int index = 0;
            foreach (var item in cl)
            {
                var fila = new VisualElement();
                fila.style.flexDirection = FlexDirection.Row;
                fila.style.width = Length.Percent(100);
                fila.style.minHeight = 38;
                fila.style.maxHeight = 38;
                fila.style.alignItems = Align.Center;

                // Color de fondo de fila alternante (para todas las columnas)
                // Color estándar alterno
                Color filaColor = (index % 2 == 0)
                    ? new Color32(255, 255, 255, 255)     // blanco
                    : new Color32(242, 242, 242, 255);    // gris suave

                // Si esta fila es de mi equipo → color verde
                if (item.IdEquipo == miEquipo.IdEquipo)
                {
                    filaColor = new Color32(197, 232, 202, 255); // #C5E8CA verde
                }

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                // 1) Primera columna coloreada según posición (encima del fondo alternante)
                var colEmpty = new VisualElement();
                colEmpty.style.width = Length.Percent(col1);
                colEmpty.style.height = 38;
                colEmpty.style.display = DisplayStyle.Flex;

                if (comp == 1)
                {
                    if (index >= 0 && index <= 3)           // filas 1 a 4 -> SteelBlue
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(70, 130, 180, 255));
                    else if (index >= 4 && index <= 5)      // filas 5 y 6 -> DarkGreen
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(0, 100, 0, 255));
                    else if (index >= 17 && index <= 19)    // filas 18 a 20 -> DarkRed
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(139, 0, 0, 255));
                }
                else if (comp == 2)
                {
                    if (index >= 0 && index <= 2)               // filas 1 a 3 -> SteelBlue
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(70, 130, 180, 255));
                    else if (index >= 17 && index <= 19)        // filas 18 a 20 -> DarkRed
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(139, 0, 0, 255));
                }
                else
                {
                    if (index >= 0 && index <= 15)               // filas 1 a 3 -> SteelBlue
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(70, 130, 180, 255));
                }

                fila.Add(colEmpty);

                // 2) Posición
                fila.Add(CreateCell(item.Posicion.ToString(), col2, Color.black, TextAnchor.MiddleCenter, false));

                // 3) Escudo
                var escudo = new VisualElement();
                escudo.style.width = Length.Percent(col3);
                escudo.style.height = 32;
                var sprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{item.IdEquipo}");
                if (sprite != null)
                {
                    escudo.style.backgroundImage = new StyleBackground(sprite);
                    escudo.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(escudo);

                // 4) Nombre del equipo
                fila.Add(CreateCell(item.NombreEquipo, col4, Color.black, TextAnchor.MiddleLeft, false));

                // 5) PJ
                fila.Add(CreateCell(item.Jugados.ToString(), col5, Color.black, TextAnchor.MiddleCenter, false));

                // 6) PG
                fila.Add(CreateCell(item.Ganados.ToString(), col6, Color.black, TextAnchor.MiddleCenter, false));

                // 7) PE
                fila.Add(CreateCell(item.Empatados.ToString(), col7, Color.black, TextAnchor.MiddleCenter, false));

                // 8) PP
                fila.Add(CreateCell(item.Perdidos.ToString(), col8, Color.black, TextAnchor.MiddleCenter, false));

                // 9) GF
                fila.Add(CreateCell(item.GolesFavor.ToString(), col9, Color.black, TextAnchor.MiddleCenter, false));

                // 10) GC
                fila.Add(CreateCell(item.GolesContra.ToString(), col10, Color.black, TextAnchor.MiddleCenter, false));

                // 11) PTOS
                fila.Add(CreateCell(item.Puntos.ToString(), col11, Color.black, TextAnchor.MiddleCenter, false));

                clasificacionContainer.Add(fila);
                index++;
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