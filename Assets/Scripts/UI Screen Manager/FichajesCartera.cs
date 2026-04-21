using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class FichajesCartera
    {
        private AudioClip clickSFX;
        private MainScreen mainScreen;
        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root, carteraContainer, popupContainer;
        private Label popupText;
        private Button btnYes, btnCancel;

        public FichajesCartera(VisualElement rootInstance, Equipo equipo, Manager manager, MainScreen mainScreen)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            this.mainScreen = mainScreen;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            // Referencias a objetos de la UI
            carteraContainer = root.Q<VisualElement>("cartera-container");
            popupContainer = root.Q<VisualElement>("popup-container");
            btnYes = root.Q<Button>("btnYes");
            btnCancel = root.Q<Button>("btnCancel");
            popupText = root.Q<Label>("popup-text");

            CargarCartera();
        }

        private void CargarCartera()
        {
            carteraContainer.Clear();

            // Obtener lista completa
            List<Jugador> jugadores = OjearData.ListadoJugadoresOjeados();

            DateTime fechaHoy = FechaData.hoy;

            // PORCENTAJES DE COLUMNAS
            float col1 = 5f;     // FOTO
            float col2 = 17f;    // JUGADOR
            float col3 = 18f;    // EQUIPO
            float col4 = 5f;     // DEM
            float col5 = 5f;     // EDAD
            float col6 = 5f;     // ROL
            float col7 = 5f;     // MEDIA
            float col8 = 10f;    // SALARIO
            float col9 = 10f;    // CLAÚSULA
            float col10 = 5f;    // AÑOS
            float col11 = 10f;   // FECHA INFORME
            float col12 = 5f;    // BORRAR

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

            // Columna vacía de cabecera
            var headerEmpty = new VisualElement();
            headerEmpty.style.width = Length.Percent(col1);
            headerEmpty.style.height = 40;
            header.Add(headerEmpty);

            header.Add(CreateCell("JUGADOR", col2, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("EQUIPO", col3, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("DEM", col4, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("EDAD", col5, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("ROL", col6, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("MEDIA", col7, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("SALARIO", col8, headerText, TextAnchor.MiddleRight, true));
            header.Add(CreateCell("CLAÚSULA", col9, headerText, TextAnchor.MiddleRight, true));
            header.Add(CreateCell("AÑOS", col10, headerText, TextAnchor.MiddleCenter, true));
            header.Add(CreateCell("F.INFORME", col11, headerText, TextAnchor.MiddleCenter, true));

            var headerEmpty2 = new VisualElement();
            headerEmpty2.style.width = Length.Percent(col12);
            headerEmpty2.style.height = 40;
            header.Add(headerEmpty2);

            carteraContainer.Add(header);

            // FILAS
            int index = 0;
            foreach (var item in jugadores)
            {
                var fila = new VisualElement();
                fila.style.flexDirection = FlexDirection.Row;
                fila.style.width = Length.Percent(100);
                fila.style.minHeight = 110;
                fila.style.maxHeight = 110;
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
                foto.style.height = 80;
                var FotoSprite = Resources.Load<Sprite>($"Jugadores/{item.IdJugador}");
                if (FotoSprite != null)
                {
                    foto.style.backgroundImage = new StyleBackground(FotoSprite);
                    foto.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(foto);

                // 2 Jugador
                fila.Add(CreateCell(item.NombreCompleto, col2, Color.black, TextAnchor.MiddleLeft, false));

                // 3 EQUIPO
                fila.Add(CreateCell(EquipoData.ObtenerDetallesEquipo(item.IdEquipo).Nombre, col3, Color.black, TextAnchor.MiddleLeft, false));

                // 4 DEM
                fila.Add(CreateCell(Constants.RolIdToText(item.RolId), col4, Color.black, TextAnchor.MiddleCenter, false));

                // 5 EDAD
                fila.Add(CreateCell(item.Edad.ToString(), col5, Color.black, TextAnchor.MiddleCenter, false));

                if (DateTime.TryParse(item.FechaInforme, out DateTime fechaInforme))
                {
                    if (fechaInforme > fechaHoy)
                    {
                        fila.Add(CreateCell("-", col6, Color.black, TextAnchor.MiddleCenter, false));
                        fila.Add(CreateCell("-", col7, Color.black, TextAnchor.MiddleCenter, false));
                        fila.Add(CreateCell("-", col8, Color.black, TextAnchor.MiddleRight, false));
                        fila.Add(CreateCell("-", col9, Color.black, TextAnchor.MiddleRight, false));
                        fila.Add(CreateCell("-", col10, Color.black, TextAnchor.MiddleCenter, false));
                    }
                    else
                    {
                        // 6 ROL
                        var status = new VisualElement();
                        status.style.width = Length.Percent(col1);
                        status.style.height = 32;

                        Sprite statusSprite = null;

                        switch (item.Status)
                        {
                            case 1:
                                statusSprite = Resources.Load<Sprite>("Icons/rol1_icon");
                                break;
                            case 2:
                                statusSprite = Resources.Load<Sprite>("Icons/rol2_icon");
                                break;
                            case 3:
                                statusSprite = Resources.Load<Sprite>("Icons/rol3_icon");
                                break;
                            case 4:
                                statusSprite = Resources.Load<Sprite>("Icons/rol4_icon");
                                break;
                        }

                        if (statusSprite != null)
                        {
                            status.style.backgroundImage = new StyleBackground(statusSprite);
                            status.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                        }

                        fila.Add(status);

                        // 7 MEDIA
                        fila.Add(CreateCell(item.Media.ToString(), col7, DeterminarColor(item.Media), TextAnchor.MiddleCenter, false));

                        // 8 SALARIO
                        fila.Add(CreateCell($"{Constants.CambioDivisaNullable(item.SalarioTemporada):N0} {CargarSimboloMoneda()}", col8, Color.black, TextAnchor.MiddleRight, false));

                        // 9 CLAÚSULA
                        fila.Add(CreateCell($"{Constants.CambioDivisaNullable(item.ClausulaRescision):N0} {CargarSimboloMoneda()}", col9, Color.black, TextAnchor.MiddleRight, false));

                        // 10 AÑOS
                        fila.Add(CreateCell(item.AniosContrato.ToString(), col10, Color.black, TextAnchor.MiddleCenter, false));
                    }
                }

                // 11 FECHA INFORME
                string fecha_informe = DateTime.Parse(item.FechaInforme).ToString("dd/MM/yyyy");
                fila.Add(CreateCell(fecha_informe, col11, Color.black, TextAnchor.MiddleCenter, false));

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
                        new FichaJugador(instancia, miEquipo, miManager, item.IdJugador, 4, mainScreen);
                    });
                });

                borrar.RegisterCallback<MouseDownEvent>(evt =>
                {
                    evt.StopPropagation();
                    AudioManager.Instance.PlaySFX(clickSFX);

                    popupContainer.style.display = DisplayStyle.Flex;
                    popupText.text = "¿ Estás seguro de que quieres quitar de la cartera a " + JugadorData.MostrarDatosJugador(item.IdJugador).NombreCompleto.ToUpper() + "?";

                    // Importante: limpiar listeners previos para evitar duplicados
                    btnYes.clicked -= OnYesClick;
                    btnCancel.clicked -= OnCancelClick;

                    void OnYesClick()
                    {
                        AudioManager.Instance.PlaySFX(clickSFX);

                        OjearData.QuitarJugadorCartera(item.IdJugador);
                        CargarCartera();

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

                carteraContainer.Add(fila);
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