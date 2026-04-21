using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class CompeticionesEstadisticas
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        int competicionActiva;
        int filtroActivo;

        private VisualElement root, estadisticasContainer, competicion1, competicion2, competicion3, competicion4;
        private Button btnGoles, btnAsistencias, btnTA, btnTR, btnMVP;
        private Label tituloVentana;

        public CompeticionesEstadisticas(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            estadisticasContainer = root.Q<VisualElement>("estadisticas-container");
            competicion1 = root.Q<VisualElement>("liga1-logo");
            competicion2 = root.Q<VisualElement>("liga2-logo");
            competicion3 = root.Q<VisualElement>("europa1-logo");
            competicion4 = root.Q<VisualElement>("europa2-logo");
            btnGoles = root.Q<Button>("btnGoles");
            btnAsistencias = root.Q<Button>("btnAsistencias");
            btnTA = root.Q<Button>("btnTA");
            btnTR = root.Q<Button>("btnTR");
            btnMVP = root.Q<Button>("btnMVP");
            tituloVentana = root.Q<Label>("titulo-ventana");

            // Preconfiguración de los botones
            btnGoles.SetEnabled(false);
            btnAsistencias.SetEnabled(true);
            btnTA.SetEnabled(true);
            btnTR.SetEnabled(true);
            btnMVP.SetEnabled(true);

            List<Button> botones = new List<Button> { btnGoles, btnAsistencias, btnTA, btnTR, btnMVP };
            competicionActiva = miEquipo.IdCompeticion;
            filtroActivo = 1;

            // Título de la ventana
            ActualizarTituloVentana(competicionActiva);

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

            // Eventos de los botones de Tipo de Estadística
            btnGoles.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                SetearBotones(botones, btnGoles);
                filtroActivo = 1;
                CargarEstadisticas(filtroActivo, competicionActiva);
            };

            btnAsistencias.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                SetearBotones(botones, btnAsistencias);
                filtroActivo = 2;
                CargarEstadisticas(filtroActivo, competicionActiva);
            };

            btnTA.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                SetearBotones(botones, btnTA);
                filtroActivo = 3;
                CargarEstadisticas(filtroActivo, competicionActiva);
            };

            btnTR.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                SetearBotones(botones, btnTR);
                filtroActivo = 4;
                CargarEstadisticas(filtroActivo, competicionActiva);
            };

            btnMVP.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                SetearBotones(botones, btnMVP);
                filtroActivo = 5;
                CargarEstadisticas(filtroActivo, competicionActiva);
            };

            // Eventos de los botones de las Competiciones
            competicion1.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 1;
                ActualizarTituloVentana(competicionActiva);
                CargarEstadisticas(filtroActivo, competicionActiva);
            });

            competicion2.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 2;
                ActualizarTituloVentana(competicionActiva);
                CargarEstadisticas(filtroActivo, competicionActiva);
            });

            competicion3.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 5;
                ActualizarTituloVentana(competicionActiva);
                CargarEstadisticas(filtroActivo, competicionActiva);
            });

            competicion4.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 6;
                ActualizarTituloVentana(competicionActiva);
                CargarEstadisticas(filtroActivo, competicionActiva);
            });

            CargarEstadisticas(filtroActivo, competicionActiva);
        }

        private void SetearBotones(List<Button> botones, Button btn)
        {
            foreach (var boton in botones)
            {
                if (boton == btn)
                    boton.SetEnabled(false);
                else
                    boton.SetEnabled(true);
            }
        }

        private void CargarEstadisticas(int filtro, int comp)
        {
            List<Estadistica> listaJugadores = EstadisticaJugadorData.MostrarEstadisticasTotales(filtro, comp);

            estadisticasContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 5f;      // FOTO
            float col2 = 32f;     // JUGADOR
            float col3 = 5f;      // ESCUDO
            float col4 = 5f;      // PJ
            float col5 = 10f;     // GOLES
            float col6 = 10f;     // ASISTENCIAS
            float col7 = 10f;     // TA
            float col8 = 10f;     // TR
            float col9 = 10f;     // MVP

            // FILAS
            int index = 0;
            foreach (var item in listaJugadores)
            {
                var fila = new VisualElement();
                fila.style.flexDirection = FlexDirection.Row;
                fila.style.width = Length.Percent(100);
                fila.style.minHeight = 51;
                fila.style.maxHeight = 51;
                fila.style.alignItems = Align.Center;

                // Color de fondo de fila alternante (para todas las columnas)
                // Color estándar alterno
                Color filaColor = (index % 2 == 0)
                    ? new Color32(255, 255, 255, 255)     // blanco
                    : new Color32(242, 242, 242, 255);    // gris suave

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                // 1) Foto del Jugador
                var foto = new VisualElement();
                foto.style.width = Length.Percent(col1);
                foto.style.height = 51;
                var FotoSprite = Resources.Load<Sprite>($"Jugadores/{item.IdJugador}");
                if (FotoSprite != null)
                {
                    foto.style.backgroundImage = new StyleBackground(FotoSprite);
                    foto.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(foto);

                // 2) Nombre del jugador
                fila.Add(CreateCell(JugadorData.MostrarDatosJugador(item.IdJugador).NombreCompleto, col2, Color.black, TextAnchor.MiddleLeft, false));

                // 3) Escudo
                var escudo = new VisualElement();
                escudo.style.width = Length.Percent(col3);
                escudo.style.height = 32;
                var escudoSprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{item.IdEquipo}");
                if (escudoSprite != null)
                {
                    escudo.style.backgroundImage = new StyleBackground(escudoSprite);
                    escudo.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(escudo);

                // 4) PJ
                fila.Add(CreateCell(item.PartidosJugados.ToString(), col4, Color.black, TextAnchor.MiddleCenter, false));

                // 5) GOLES
                fila.Add(CreateCell(item.Goles.ToString(), col5, Color.black, TextAnchor.MiddleCenter, false));

                // 6) ASISTENCIAS
                fila.Add(CreateCell(item.Asistencias.ToString(), col6, Color.black, TextAnchor.MiddleCenter, false));

                // 7) TA
                fila.Add(CreateCell(item.TarjetasAmarillas.ToString(), col7, Color.black, TextAnchor.MiddleCenter, false));

                // 8) TR
                fila.Add(CreateCell(item.TarjetasRojas.ToString(), col8, Color.black, TextAnchor.MiddleCenter, false));

                // 9) MVP
                fila.Add(CreateCell(item.MVP.ToString(), col9, Color.black, TextAnchor.MiddleCenter, false));

                estadisticasContainer.Add(fila);
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
            tituloVentana.text = $"ESTADÍSTICAS {CompeticionData.MostrarNombreCompeticion(comp).ToUpper()}";
        }
    }
}

