using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class FichajesMercado
    {
        private AudioClip clickSFX;
        private MainScreen mainScreen;
        private Equipo miEquipo;
        private Manager miManager;

        private int paginaActual = 1;
        private int itemsPorPagina = 19;
        private int totalPaginas = 1;
        int tipoStart = 1;
        int tipoEnd = 1;

        private VisualElement root, mercadoContainer, btnPaginaAnterior, btnPaginaSiguiente;
        private Label numPagina;
        private DropdownField dpTipo;

        public FichajesMercado(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            mercadoContainer = root.Q<VisualElement>("mercado-container");
            numPagina = root.Q<Label>("numPagina");
            btnPaginaAnterior = root.Q<VisualElement>("btnPaginaAnterior");
            btnPaginaSiguiente = root.Q<VisualElement>("btnPaginaSiguiente");
            dpTipo = root.Q<DropdownField>("dpTipo");

            CargarTablaMercado(tipoStart, tipoEnd);

            btnPaginaAnterior.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                paginaActual--;
                CargarTablaMercado(tipoStart, tipoEnd);

            });

            btnPaginaSiguiente.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                paginaActual++;
                CargarTablaMercado(tipoStart, tipoEnd);

            });

            dpTipo.RegisterValueChangedCallback(evt =>
            {
                int index = dpTipo.index;  // 0 = Transferible, 1 = Cedible

                if (index == 0)        // Transferible
                {
                    tipoStart = 1;
                    tipoEnd = 1;
                }
                else if (index == 1)   // Cedible
                {
                    tipoStart = 2;
                    tipoEnd = 2;
                }
                else                   // Ambos
                {
                    tipoStart = 1;
                    tipoEnd = 2;
                }

                CargarTablaMercado(tipoStart, tipoEnd);
            });
        }

        private void CargarTablaMercado(int tipoStart, int tipoEnd)
        {
            mercadoContainer.Clear();

            // Obtener lista completa
            List<Jugador> jugadores = JugadorData.ListadoJugadoresMercado(
                miEquipo.IdEquipo,
                tipoStart, tipoEnd
            );

            // CALCULAR TOTAL DE PÁGINAS
            totalPaginas = Mathf.CeilToInt(jugadores.Count / (float)itemsPorPagina);

            // Ajustar página si se sale del rango
            if (paginaActual < 1) paginaActual = 1;
            if (paginaActual > totalPaginas) paginaActual = totalPaginas;

            // ACTUALIZAR LABEL
            numPagina.text = $"{paginaActual} / {totalPaginas}";

            // Cortar la lista según la página actual
            int inicio = (paginaActual - 1) * itemsPorPagina;
            List<Jugador> pagina = jugadores.Skip(inicio).Take(itemsPorPagina).ToList();

            // PORCENTAJES DE COLUMNAS
            float col1 = 20f;   // JUGADOR
            float col2 = 8f;    // DEMARCACIÓN
            float col3 = 8f;    // MEDIA
            float col4 = 8f;    // MORAL
            float col5 = 8f;    // ESTADO FORMA
            float col6 = 8f;    // LESIONADO
            float col7 = 10f;   // SALARIO
            float col8 = 10f;   // CLAÚSULA
            float col9 = 10f;   // VALOR MERCADO
            float col10 = 8f;   // AÑOS

            // COLORES CABECERA
            Color headerBg = new Color32(56, 78, 63, 255);
            Color headerText = Color.white;

            // CABECERA
            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.backgroundColor = new StyleColor(headerBg);
            header.style.minHeight = 35;
            header.style.maxHeight = 35;
            header.style.width = Length.Percent(100);
            header.style.alignItems = Align.Center;
            header.style.unityFontStyleAndWeight = FontStyle.Bold;

            header.Add(CreateCell("JUGADOR", col1, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("DEMARCACIÓN", col2, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("MEDIA", col3, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("MORAL", col4, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("E.FORMA", col5, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("LESIONADO", col6, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("SALARIO", col7, headerText, TextAnchor.MiddleRight, true));
            header.Add(CreateCell("CLAÚSULA", col8, headerText, TextAnchor.MiddleRight, true));
            header.Add(CreateCell("VALOR MERCADO", col9, headerText, TextAnchor.MiddleRight, true));
            header.Add(CreateCell("AÑOS", col10, headerText, TextAnchor.MiddleCenter, true));

            mercadoContainer.Add(header);

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

                Color filaColor;
                if (item.SituacionMercado == 1)
                    filaColor = new Color32(192, 241, 213, 255);
                else
                    filaColor = new Color32(201, 224, 238, 255);

                fila.style.backgroundColor = new StyleColor(filaColor);

                fila.Add(CreateCell(item.NombreCompleto, col1, Color.black, TextAnchor.MiddleLeft, false));
                fila.Add(CreateCell(Constants.RolIdToText(item.RolId), col2, Color.black, TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell(item.Media.ToString(), col3, Color.black, TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell(item.Moral.ToString(), col4, Color.black, TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell(item.EstadoForma.ToString(), col5, Color.black, TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell(LesionadoToTexto(item.Lesion), col6, Color.black, TextAnchor.MiddleCenter, false));
                fila.Add(CreateCell($"{Constants.CambioDivisaNullable(item.SalarioTemporada):N0} {CargarSimboloMoneda()}", col7, Color.black, TextAnchor.MiddleRight, false));
                fila.Add(CreateCell($"{Constants.CambioDivisaNullable(item.ClausulaRescision):N0} {CargarSimboloMoneda()}", col8, Color.black, TextAnchor.MiddleRight, false));
                fila.Add(CreateCell($"{Constants.CambioDivisaNullable(item.ValorMercado):N0} {CargarSimboloMoneda()}", col9, Color.black, TextAnchor.MiddleRight, false));
                fila.Add(CreateCell(item.AniosContrato.ToString(), col10, Color.black, TextAnchor.MiddleCenter, false));

                // **Registrar evento de click**
                fila.RegisterCallback<MouseDownEvent>(evt =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    UIManager.Instance.CargarPantalla("UI/Ficha/Ficha", instancia =>
                    {
                        new FichaJugador(instancia, miEquipo, miManager, item.IdJugador, 2, mainScreen);
                    });
                });

                mercadoContainer.Add(fila);
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

        private string SituacionMercadoToTexto(int situacion)
        {
            return situacion == 1 ? "Transferible" : situacion == 2 ? "Cedible" : "Desconocido";
        }

        private string LesionadoToTexto(int lesion)
        {
            return lesion > 0 ? "SI" : "NO";
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