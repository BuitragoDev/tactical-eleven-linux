using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class FichajesListaTraspasos
    {
        private AudioClip clickSFX;
        private MainScreen mainScreen;
        private Equipo miEquipo;
        private Manager miManager;

        private int paginaActual = 1;
        private int itemsPorPagina = 21;
        private int totalPaginas = 1;

        private VisualElement root, listaContainer, btnPaginaAnterior, btnPaginaSiguiente;
        private Label numPagina;

        public FichajesListaTraspasos(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            listaContainer = root.Q<VisualElement>("lista-container");
            numPagina = root.Q<Label>("numPagina");
            btnPaginaAnterior = root.Q<VisualElement>("btnPaginaAnterior");
            btnPaginaSiguiente = root.Q<VisualElement>("btnPaginaSiguiente");

            CargarListaTraspasos();

            btnPaginaAnterior.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                paginaActual--;
                CargarListaTraspasos();

            });

            btnPaginaSiguiente.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                paginaActual++;
                CargarListaTraspasos();

            });
        }

        private void CargarListaTraspasos()
        {
            listaContainer.Clear();

            // Obtener lista completa
            List<Transferencia> jugadores = TransferenciaData.ListarTraspasos();

            // CALCULAR TOTAL DE PÁGINAS
            totalPaginas = Mathf.CeilToInt(jugadores.Count / (float)itemsPorPagina);

            // Ajustar página si se sale del rango
            if (paginaActual < 1) paginaActual = 1;
            if (paginaActual > totalPaginas) paginaActual = totalPaginas;

            // ACTUALIZAR LABEL
            numPagina.text = $"{paginaActual} / {totalPaginas}";

            // Cortar la lista según la página actual
            int inicio = (paginaActual - 1) * itemsPorPagina;
            List<Transferencia> pagina = jugadores.Skip(inicio).Take(itemsPorPagina).ToList();

            DateTime fechaHoy = FechaData.hoy;

            // PORCENTAJES DE COLUMNAS
            float col1 = 20f;     // NOMBRE
            float col2 = 4f;      // ESCUDO VENDEDOR
            float col3 = 16f;     // EQUIPO VENDEDOR
            float col4 = 4f;      // ESCUDO COMPRADOR
            float col5 = 16f;     // EQUIPO COMPRADOR
            float col6 = 15f;     // VALOR
            float col7 = 10f;     // DEMARCACIÓN
            float col8 = 10f;     // MEDIA
            float col9 = 15f;     // TIPO
            float col10 = 10f;    // FECHA

            // COLORES CABECERA
            Color headerBg = new Color32(56, 78, 63, 255);
            Color headerText = Color.white;

            // CABECERA
            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.backgroundColor = new StyleColor(headerBg);
            header.style.minHeight = 40;
            header.style.maxHeight = 40;
            header.style.width = Length.Percent(100);
            header.style.alignItems = Align.Center;
            header.style.unityFontStyleAndWeight = FontStyle.Bold;

            header.Add(CreateCell("JUGADOR", col1, headerText, TextAnchor.MiddleLeft, true));

            // Columna vacía de cabecera
            var headerEmpty = new VisualElement();
            headerEmpty.style.width = Length.Percent(col2);
            headerEmpty.style.height = 40;
            header.Add(headerEmpty);

            header.Add(CreateCell("EQUIPO VENDEDOR", col3, headerText, TextAnchor.MiddleLeft, true));

            // Columna vacía de cabecera
            var headerEmpty2 = new VisualElement();
            headerEmpty2.style.width = Length.Percent(col4);
            headerEmpty2.style.height = 40;
            header.Add(headerEmpty2);

            header.Add(CreateCell("EQUIPO COMPRADOR", col5, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("VALOR", col6, headerText, TextAnchor.MiddleRight, true));
            header.Add(CreateCell("DEM", col7, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("MEDIA", col8, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("TIPO", col9, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("FECHA", col10, headerText, TextAnchor.MiddleCenter, true));

            listaContainer.Add(header);

            // FILAS SOLO DE ESTA PÁGINA
            int index = 0;
            foreach (var item in pagina)
            {
                var fila = new VisualElement();
                fila.style.flexDirection = FlexDirection.Row;
                fila.style.width = Length.Percent(100);
                fila.style.minHeight = 35;
                fila.style.maxHeight = 35;
                fila.style.alignItems = Align.Center;

                // Color de fondo de fila alternante (para todas las columnas)
                // Color estándar alterno
                Color filaColor = (index % 2 == 0)
                    ? new Color32(255, 255, 255, 255)     // blanco
                    : new Color32(242, 242, 242, 255);    // gris suave

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                // 1 Jugador
                fila.Add(CreateCell(JugadorData.MostrarDatosJugador(item.IdJugador).NombreCompleto, col1, Color.black, TextAnchor.MiddleLeft, false));

                // 2) Escudo Vendedor
                var escudoVendedor = new VisualElement();
                escudoVendedor.style.width = Length.Percent(col2);
                escudoVendedor.style.height = 32;
                var escudoVendedorSprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{item.IdEquipoOrigen}");
                if (escudoVendedorSprite != null)
                {
                    escudoVendedor.style.backgroundImage = new StyleBackground(escudoVendedorSprite);
                    escudoVendedor.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(escudoVendedor);

                // 3 EQUIPO VENDEDOR
                fila.Add(CreateCell(EquipoData.ObtenerDetallesEquipo(item.IdEquipoOrigen).Nombre, col3, Color.black, TextAnchor.MiddleLeft, false));

                // 4) Escudo Comprador
                var escudoComprador = new VisualElement();
                escudoComprador.style.width = Length.Percent(col4);
                escudoComprador.style.height = 32;
                var escudoCompradorSprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{item.IdEquipoDestino}");
                if (escudoCompradorSprite != null)
                {
                    escudoComprador.style.backgroundImage = new StyleBackground(escudoCompradorSprite);
                    escudoComprador.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(escudoComprador);

                // 5 EQUIPO COMPRADOR
                fila.Add(CreateCell(EquipoData.ObtenerDetallesEquipo(item.IdEquipoDestino).Nombre, col5, Color.black, TextAnchor.MiddleLeft, false));

                // 6 VALOR
                fila.Add(CreateCell($"{Constants.CambioDivisaNullable(item.MontoOferta):N0} {CargarSimboloMoneda()}", col6, Color.black, TextAnchor.MiddleRight, false));

                // 7 DEMARCACIÓN
                fila.Add(CreateCell(Constants.RolIdToText(JugadorData.MostrarDatosJugador(item.IdJugador).RolId), col7, Color.black, TextAnchor.MiddleCenter, false));

                // 8 MEDIA
                fila.Add(CreateCell(JugadorData.MostrarDatosJugador(item.IdJugador).Media.ToString(), col8, DeterminarColor(JugadorData.MostrarDatosJugador(item.IdJugador).Media), TextAnchor.MiddleCenter, false));

                // 9 TIPO
                if (item.TipoFichaje == 1)
                    fila.Add(CreateCell("TRAPASO", col9, Color.black, TextAnchor.MiddleCenter, false));
                else
                    fila.Add(CreateCell("CESIÓN", col9, Color.black, TextAnchor.MiddleCenter, false));

                // 10 FECHA
                fila.Add(CreateCell(DateTime.Parse(item.FechaTraspaso).ToString("dd/MM/yyyy"), col10, Color.black, TextAnchor.MiddleCenter, false));


                listaContainer.Add(fila);
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
            {
                cell.style.justifyContent = Justify.FlexStart;
                cell.style.paddingLeft = 20;
            }
            else if (alineacion == TextAnchor.MiddleCenter)
            {
                cell.style.justifyContent = Justify.Center;
            }
            else if (alineacion == TextAnchor.MiddleRight)
            {
                cell.style.justifyContent = Justify.FlexEnd;
            }

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