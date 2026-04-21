using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class FichajesEstadoOfertas
    {
        private AudioClip clickSFX;
        private MainScreen mainScreen;
        private Equipo miEquipo;
        private Manager miManager;

        private int paginaActual = 1;
        private int itemsPorPagina = 8;
        private int totalPaginas = 1;
        int reputacionDirectorTecnico = 0;

        private VisualElement root, listaContainer, estrellasContainer, popupContainer;
        private Label numPagina, directorTenicoNombre, numeroOperaciones, popupText;
        private Button btnYes, btnCancel;

        public FichajesEstadoOfertas(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
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
            estrellasContainer = root.Q<VisualElement>("estrellas-contenedor");
            numPagina = root.Q<Label>("numPagina");
            directorTenicoNombre = root.Q<Label>("directorTecnico-nombre");
            numeroOperaciones = root.Q<Label>("numero-operaciones");

            popupContainer = root.Q<VisualElement>("popup-container");
            btnYes = root.Q<Button>("btnYes");
            btnCancel = root.Q<Button>("btnCancel");
            popupText = root.Q<Label>("popup-text");

            CargarOfertas();
        }

        private void CargarOfertas()
        {
            Empleado director = EmpleadoData.ObtenerEmpleadoPorPuesto("Director Técnico");

            if (director != null)
            {
                directorTenicoNombre.text = director.Nombre;
                reputacionDirectorTecnico = director.Categoria;
                MostrarEstrellas(reputacionDirectorTecnico, estrellasContainer);

                string[] seguimiento = {
                    "Nuestro Director Técnico nos permite hacer 1 operación al mismo tiempo.",
                    "Nuestro Director Técnico nos permite hacer hasta 2 operaciones al mismo tiempo.",
                    "Nuestro Director Técnico nos permite hacer hasta 3 operaciones al mismo tiempo.",
                    "Nuestro Director Técnico nos permite hacer hasta 4 operaciones al mismo tiempo.",
                    "Nuestro Director Técnico nos permite hacer hasta 5 operaciones al mismo tiempo."
                };

                if (reputacionDirectorTecnico >= 1 && reputacionDirectorTecnico <= 5)
                {
                    numeroOperaciones.text = seguimiento[reputacionDirectorTecnico - 1];
                }

                // Cargar Ofertas
                listaContainer.Clear();

                // Obtener lista completa
                List<Transferencia> ofertas = TransferenciaData.ListarOfertasSinFinalizar();

                DateTime fechaHoy = FechaData.hoy;

                // PORCENTAJES DE COLUMNAS
                float col1 = 5f;     // FOTO
                float col2 = 15f;    // JUGADOR
                float col3 = 15f;    // EQUIPO
                float col4 = 5f;     // DEM
                float col5 = 5f;     // MEDIA
                float col6 = 10f;    // OFERTA
                float col7 = 10f;    // CONTRATO
                float col8 = 5f;     // AÑOS
                float col9 = 10f;    // FECHA
                float col10 = 10f;   // TIPO OFERTA
                float col11 = 5f;    // ESTADO
                float col12 = 5f;    // BORRAR

                // COLORES CABECERA
                Color headerBg = new Color32(56, 78, 63, 255);
                Color headerText = Color.white;

                // CABECERA
                var header = new VisualElement();
                header.style.flexDirection = FlexDirection.Row;
                header.style.backgroundColor = new StyleColor(headerBg);
                header.style.minHeight = 50;
                header.style.maxHeight = 50;
                header.style.width = Length.Percent(100);
                header.style.alignItems = Align.Center;
                header.style.unityFontStyleAndWeight = FontStyle.Bold;

                // Columna vacía de cabecera
                var headerEmpty = new VisualElement();
                headerEmpty.style.width = Length.Percent(col1);
                headerEmpty.style.height = 50;
                header.Add(headerEmpty);

                header.Add(CreateCell("JUGADOR", col2, headerText, TextAnchor.MiddleLeft, true));
                header.Add(CreateCell("EQUIPO", col3, headerText, TextAnchor.MiddleLeft, true));
                header.Add(CreateCell("DEM", col4, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("MEDIA", col5, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("OFERTA", col6, headerText, TextAnchor.MiddleRight, true));
                header.Add(CreateCell("CONTRATO", col7, headerText, TextAnchor.MiddleRight, true));
                header.Add(CreateCell("AÑOS", col8, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("FECHA", col9, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("TIPO OFERTA", col10, headerText, TextAnchor.MiddleCenter, true));
                header.Add(CreateCell("ESTADO", col11, headerText, TextAnchor.MiddleCenter, true));

                var headerEmpty2 = new VisualElement();
                headerEmpty2.style.width = Length.Percent(col12);
                headerEmpty2.style.height = 50;
                header.Add(headerEmpty2);

                listaContainer.Add(header);

                // FILAS
                int index = 0;
                foreach (var item in ofertas)
                {
                    Jugador jugador = JugadorData.MostrarDatosJugador(item.IdJugador);

                    var fila = new VisualElement();
                    fila.style.flexDirection = FlexDirection.Row;
                    fila.style.width = Length.Percent(100);
                    fila.style.minHeight = 100;
                    fila.style.maxHeight = 100;
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
                    foto.style.height = 50;
                    var FotoSprite = Resources.Load<Sprite>($"Jugadores/{item.IdJugador}");
                    if (FotoSprite != null)
                    {
                        foto.style.backgroundImage = new StyleBackground(FotoSprite);
                        foto.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                    }
                    fila.Add(foto);

                    // 2 JUGADOR
                    fila.Add(CreateCell(jugador.NombreCompleto, col2, Color.black, TextAnchor.MiddleLeft, false));

                    // 3 EQUIPO
                    fila.Add(CreateCell(EquipoData.ObtenerDetallesEquipo(jugador.IdEquipo).Nombre, col3, Color.black, TextAnchor.MiddleLeft, false));

                    // 4 DEM
                    fila.Add(CreateCell(Constants.RolIdToText(jugador.RolId), col4, Color.black, TextAnchor.MiddleCenter, false));

                    // 5 MEDIA
                    fila.Add(CreateCell(jugador.Media.ToString(), col5, DeterminarColor(jugador.Media), TextAnchor.MiddleCenter, false));

                    // 6 OFERTA
                    fila.Add(CreateCell($"{Constants.CambioDivisaNullable(item.MontoOferta):N0} {CargarSimboloMoneda()}", col6, Color.black, TextAnchor.MiddleRight, false));

                    // 7 CONTRATO
                    fila.Add(CreateCell($"{Constants.CambioDivisaNullable(item.SalarioAnual):N0} {CargarSimboloMoneda()}", col7, Color.black, TextAnchor.MiddleRight, false));

                    // 8 AÑOS
                    fila.Add(CreateCell(item.Duracion != 0 ? item.Duracion.ToString() : "", col8, Color.black, TextAnchor.MiddleCenter, false));

                    // 9 FECHA
                    string fecha_oferta = DateTime.Parse(item.FechaOferta).ToString("dd/MM/yyyy");
                    fila.Add(CreateCell(fecha_oferta, col9, Color.black, TextAnchor.MiddleCenter, false));

                    // 10 TIPO OFERTA
                    fila.Add(CreateCell(item.TipoFichaje == 1 ? "TRASPASO" : "CESIÓN", col10, Color.black, TextAnchor.MiddleCenter, false));

                    // 11 ESTADO
                    var estado = new VisualElement();
                    estado.style.width = Length.Percent(col11);
                    estado.style.height = 32;

                    Sprite estadoSprite;
                    if (item.RespuestaEquipo == 1)
                        estadoSprite = Resources.Load<Sprite>($"Icons/aceptado");
                    else
                        estadoSprite = Resources.Load<Sprite>($"Icons/rechazado");

                    if (estadoSprite != null)
                    {
                        estado.style.backgroundImage = new StyleBackground(estadoSprite);
                        estado.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                    }
                    fila.Add(estado);

                    // 12) BOTÓN BORRAR
                    var borrar = new VisualElement();
                    borrar.style.width = Length.Percent(col12);
                    borrar.style.height = 32;
                    var BorrarSprite = Resources.Load<Sprite>($"Icons/papelera-black");
                    if (BorrarSprite != null)
                    {
                        borrar.style.backgroundImage = new StyleBackground(BorrarSprite);
                        borrar.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                    }
                    fila.Add(borrar);

                    // **Registrar evento de click**
                    fila.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);
                        UIManager.Instance.CargarPantalla("UI/Ficha/Ficha", instancia =>
                        {
                            new FichaJugador(instancia, miEquipo, miManager, item.IdJugador, 5, mainScreen);
                        });
                    });

                    borrar.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        evt.StopPropagation();
                        AudioManager.Instance.PlaySFX(clickSFX);

                        popupContainer.style.display = DisplayStyle.Flex;
                        popupText.text = "¿ Estás seguro de que quieres borrar la oferta por " + jugador.NombreCompleto + "?";

                        // Importante: limpiar listeners previos para evitar duplicados
                        btnYes.clicked -= OnYesClick;
                        btnCancel.clicked -= OnCancelClick;

                        void OnYesClick()
                        {
                            AudioManager.Instance.PlaySFX(clickSFX);

                            TransferenciaData.BorrarOferta(item.IdJugador);
                            CargarOfertas();

                            btnYes.clicked -= OnYesClick;
                            btnCancel.clicked -= OnCancelClick;
                            popupContainer.style.display = DisplayStyle.None;
                        }

                        void OnCancelClick()
                        {
                            AudioManager.Instance.PlaySFX(clickSFX);
                            popupContainer.style.display = DisplayStyle.None;

                            btnYes.clicked -= OnYesClick;
                            btnCancel.clicked -= OnCancelClick;
                        }

                        btnYes.clicked += OnYesClick;
                        btnCancel.clicked += OnCancelClick;
                    });

                    listaContainer.Add(fila);
                    index++;
                }

            }
            else
            {
                directorTenicoNombre.text = "No tienes ningún Director Técnico contratado";
                numeroOperaciones.text = "-";
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