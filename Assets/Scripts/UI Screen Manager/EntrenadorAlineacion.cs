using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class EntrenadorAlineacion
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        Jugador jugadorSeleccionado = null;
        string tactica;
        private int jugadorMarcado = 0;
        private int idJugadorUno = 0;
        private int idJugadorDos = 0;
        private int posicionUno = 0;
        private int posicionDos = 0;

        private bool primerJugadorSeleccionado = false;

        private VisualElement root, titularesContainer, reservasContainer, fotoJugador, posicionIdeal, posicionActual, fotoCambio1, fotoCambio2,
                              cambio1Container, cambio2Container;
        private Label nombreJugador, demarcacionJugador, lblPortero, lblPase, lblRegate, lblRemate, lblEntradas, lblTiro, jugadorCambio1, jugadorCambio2;
        private Button btnTactica1, btnTactica2, btnTactica3, btnTactica4, btnTactica5, btnTactica6;

        public EntrenadorAlineacion(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");
            tactica = ManagerData.MostrarManager().Tactica;

            // Referencias a objetos de la UI
            titularesContainer = root.Q<VisualElement>("titulares-container");
            reservasContainer = root.Q<VisualElement>("reservas-container");
            fotoJugador = root.Q<VisualElement>("foto-jugador");
            posicionIdeal = root.Q<VisualElement>("posicion-ideal");
            posicionActual = root.Q<VisualElement>("posicion-actual");
            fotoCambio1 = root.Q<VisualElement>("foto-cambio1");
            fotoCambio2 = root.Q<VisualElement>("foto-cambio2");
            cambio1Container = root.Q<VisualElement>("cambio1-container");
            cambio2Container = root.Q<VisualElement>("cambio2-container");
            nombreJugador = root.Q<Label>("nombre-jugador");
            demarcacionJugador = root.Q<Label>("demarcacion-jugador");
            lblPortero = root.Q<Label>("portero-value");
            lblPase = root.Q<Label>("pase-value");
            lblRegate = root.Q<Label>("regate-value");
            lblRemate = root.Q<Label>("remate-value");
            lblEntradas = root.Q<Label>("entradas-value");
            lblTiro = root.Q<Label>("tiro-value");
            jugadorCambio1 = root.Q<Label>("jugador-cambio1");
            jugadorCambio2 = root.Q<Label>("jugador-cambio2");
            btnTactica1 = root.Q<Button>("btnTactica1");
            btnTactica2 = root.Q<Button>("btnTactica2");
            btnTactica3 = root.Q<Button>("btnTactica3");
            btnTactica4 = root.Q<Button>("btnTactica4");
            btnTactica5 = root.Q<Button>("btnTactica5");
            btnTactica6 = root.Q<Button>("btnTactica6");

            if (tactica == "5-4-1")
                CargarTactica(btnTactica1);
            else if (tactica == "5-3-2")
                CargarTactica(btnTactica2);
            else if (tactica == "4-5-1")
                CargarTactica(btnTactica3);
            else if (tactica == "4-4-2")
                CargarTactica(btnTactica4);
            else if (tactica == "4-3-3")
                CargarTactica(btnTactica5);
            else if (tactica == "3-5-2")
                CargarTactica(btnTactica6);

            CargarTitulares();
            CargarReservas();

            btnTactica1.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ManagerData.CambiarTactica("5-4-1");
                CargarTactica(btnTactica1);
            };
            btnTactica2.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ManagerData.CambiarTactica("5-3-2");
                CargarTactica(btnTactica2);
            };
            btnTactica3.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ManagerData.CambiarTactica("4-5-1");
                CargarTactica(btnTactica3);
            };
            btnTactica4.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ManagerData.CambiarTactica("4-4-2");
                CargarTactica(btnTactica4);
            };
            btnTactica5.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ManagerData.CambiarTactica("4-3-3");
                CargarTactica(btnTactica5);
            };
            btnTactica6.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                ManagerData.CambiarTactica("3-5-2");
                CargarTactica(btnTactica6);
            };
        }

        private void CargarTactica(Button boton)
        {
            btnTactica1.style.backgroundColor = new StyleColor(new Color32(30, 30, 30, 255));
            btnTactica2.style.backgroundColor = new StyleColor(new Color32(30, 30, 30, 255));
            btnTactica3.style.backgroundColor = new StyleColor(new Color32(30, 30, 30, 255));
            btnTactica4.style.backgroundColor = new StyleColor(new Color32(30, 30, 30, 255));
            btnTactica5.style.backgroundColor = new StyleColor(new Color32(30, 30, 30, 255));
            btnTactica6.style.backgroundColor = new StyleColor(new Color32(30, 30, 30, 255));
            boton.style.backgroundColor = new StyleColor(new Color32(56, 78, 63, 255));

            btnTactica1.SetEnabled(true);
            btnTactica2.SetEnabled(true);
            btnTactica3.SetEnabled(true);
            btnTactica4.SetEnabled(true);
            btnTactica5.SetEnabled(true);
            btnTactica6.SetEnabled(true);
            boton.SetEnabled(false);

            tactica = ManagerData.MostrarManager().Tactica;
            CargarTitulares();
            CargarReservas();
        }

        private void CargarTitulares()
        {
            List<Jugador> titulares = JugadorData.MostrarAlineacion(1, 11);

            titularesContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 5f;       // Nº
            float col2 = 8f;       // BANDERA
            float col3 = 32f;      // JUGADOR
            float col4 = 15f;      // DEMARCACIÓN
            float col5 = 8f;       // MEDIA
            float col6 = 8f;       // MO
            float col7 = 8f;       // EF
            float col8 = 8f;       // LESIÓN
            float col9 = 8f;       // SANCIÓN

            // FILAS
            int index = 0;
            foreach (var item in titulares)
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
                    ? new Color32(186, 220, 199, 255)   // verde claro luminoso
                    : new Color32(162, 202, 175, 255);  // verde claro apagado

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                // 1) Nº
                fila.Add(CreateCell(item.PosicionAlineacion.ToString(), col1, Color.black, TextAnchor.MiddleCenter, false));

                // 2) Bandera
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

                // 3) Jugador
                fila.Add(CreateCell(item.NombreCompleto, col3, Color.black, TextAnchor.MiddleLeft, false));

                // 4) Demarcación
                fila.Add(CreateCell(Constants.RolIdToText(JugadorData.MostrarDatosJugador(item.IdJugador).RolId), col4, Color.black, TextAnchor.MiddleCenter, false));

                // 5) MED
                fila.Add(CreateCell(ComprobarMedia(item.Media, item.RolId, item.PosicionAlineacion).ToString(), col5, DeterminarColor(ComprobarMedia(item.Media, item.RolId, item.PosicionAlineacion)), TextAnchor.MiddleCenter, false));

                // 6) MO
                fila.Add(CreateCell(item.Moral.ToString(), col6, Color.black, TextAnchor.MiddleCenter, false));

                // 7) EF
                fila.Add(CreateCell(item.EstadoForma.ToString(), col7, Color.black, TextAnchor.MiddleCenter, false));

                // 8) LESIONADO
                string lesionPath = item.Lesion > 0 ? "Icons/lesion" : null;
                fila.Add(CreateIconCell(lesionPath, col8));

                // 9) SANCIONADO
                string sancionPath = item.Sancionado > 0 ? "Icons/sancion" : null;
                fila.Add(CreateIconCell(sancionPath, col9));

                // **Registrar evento de click**
                fila.RegisterCallback<MouseDownEvent>(evt =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    jugadorSeleccionado = item;

                    SeleccionarJugador(jugadorSeleccionado);
                });

                // **Registrar evento de mouse over y mouse out**
                fila.RegisterCallback<MouseOverEvent>(evt =>
                {
                    jugadorSeleccionado = item;

                    CargarDatosJugador(jugadorSeleccionado);

                    if (!primerJugadorSeleccionado)
                    {
                        // Antes de seleccionar el primer jugador, mostrar en imgCambio1
                        MostrarDatosCambio1(jugadorSeleccionado);
                    }
                    else
                    {
                        // Después de seleccionar el primer jugador, mostrar en imgCambio2
                        MostrarDatosCambio2(jugadorSeleccionado);
                    }

                });

                fila.RegisterCallback<MouseOutEvent>(evt =>
                {
                    LimpiarDatosJugador();

                    if (!primerJugadorSeleccionado)
                    {
                        // Antes de seleccionar el primer jugador, limpiar imgCambio1
                        LimpiarCambio1();
                    }
                    else
                    {
                        // Después de seleccionar el primer jugador, limpiar imgCambio2
                        LimpiarCambio2();
                    }
                });

                titularesContainer.Add(fila);
                index++;
            }
        }

        private void CargarReservas()
        {
            List<Jugador> reservas = JugadorData.MostrarAlineacion(12, 30);

            reservasContainer.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 5f;       // Nº
            float col2 = 8f;       // BANDERA
            float col3 = 32f;      // JUGADOR
            float col4 = 15f;      // DEMARCACIÓN
            float col5 = 8f;       // MEDIA
            float col6 = 8f;       // MO
            float col7 = 8f;       // EF
            float col8 = 8f;       // LESIÓN
            float col9 = 8f;       // SANCIÓN

            // FILAS
            int index = 0;
            foreach (var item in reservas)
            {
                var fila = new VisualElement();
                fila.style.flexDirection = FlexDirection.Row;
                fila.style.width = Length.Percent(100);
                fila.style.minHeight = 30;
                fila.style.maxHeight = 30;
                fila.style.alignItems = Align.Center;

                // Color de fondo de fila alternante (para todas las columnas)
                // Color estándar alterno
                Color filaColor;
                if (index >= 0 && index <= 4)
                {
                    filaColor = (index % 2 == 0)
                    ? new Color32(186, 220, 199, 255)   // verde claro luminoso
                    : new Color32(162, 202, 175, 255);  // verde claro apagado
                }
                else
                {
                    filaColor = (index % 2 == 0)
                    ? new Color32(255, 255, 255, 255)     // blanco
                    : new Color32(242, 242, 242, 255);    // gris suave
                }

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                // 1) Nº
                fila.Add(CreateCell(item.PosicionAlineacion.ToString(), col1, Color.black, TextAnchor.MiddleCenter, false));

                // 2) Bandera
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

                // 3) Jugador
                fila.Add(CreateCell(item.NombreCompleto, col3, Color.black, TextAnchor.MiddleLeft, false));

                // 4) Demarcación
                fila.Add(CreateCell(Constants.RolIdToText(JugadorData.MostrarDatosJugador(item.IdJugador).RolId), col4, Color.black, TextAnchor.MiddleCenter, false));

                // 5) MED
                fila.Add(CreateCell(item.Media.ToString(), col5, DeterminarColor(item.Media), TextAnchor.MiddleCenter, false));

                // 6) MO
                fila.Add(CreateCell(item.Moral.ToString(), col6, Color.black, TextAnchor.MiddleCenter, false));

                // 7) EF
                fila.Add(CreateCell(item.EstadoForma.ToString(), col7, Color.black, TextAnchor.MiddleCenter, false));

                // 8) LESIONADO
                string lesionPath = item.Lesion > 0 ? "Icons/lesion" : null;
                fila.Add(CreateIconCell(lesionPath, col8));

                // 9) SANCIONADO
                string sancionPath = item.Sancionado > 0 ? "Icons/sancion" : null;
                fila.Add(CreateIconCell(sancionPath, col9));

                // **Registrar evento de click**
                fila.RegisterCallback<MouseDownEvent>(evt =>
                {
                    AudioManager.Instance.PlaySFX(clickSFX);
                    jugadorSeleccionado = item;

                    SeleccionarJugador(jugadorSeleccionado);
                });

                // **Registrar evento de mouse over y mouse out**
                fila.RegisterCallback<MouseOverEvent>(evt =>
                {
                    jugadorSeleccionado = item;

                    CargarDatosJugador(jugadorSeleccionado);

                    if (!primerJugadorSeleccionado)
                    {
                        // Antes de seleccionar el primer jugador, mostrar en imgCambio1
                        MostrarDatosCambio1(jugadorSeleccionado);
                    }
                    else
                    {
                        // Después de seleccionar el primer jugador, mostrar en imgCambio2
                        MostrarDatosCambio2(jugadorSeleccionado);
                    }

                });

                fila.RegisterCallback<MouseOutEvent>(evt =>
                {
                    LimpiarDatosJugador();

                    if (!primerJugadorSeleccionado)
                    {
                        // Antes de seleccionar el primer jugador, limpiar imgCambio1
                        LimpiarCambio1();
                    }
                    else
                    {
                        // Después de seleccionar el primer jugador, limpiar imgCambio2
                        LimpiarCambio2();
                    }
                });

                reservasContainer.Add(fila);
                index++;
            }
        }

        private VisualElement CreateCell(string texto, float anchoPercent, Color color, TextAnchor alineacion, bool esHeader)
        {
            var cell = new VisualElement();
            cell.style.width = Length.Percent(anchoPercent);
            cell.style.flexDirection = FlexDirection.Row;
            cell.style.alignItems = Align.Center;
            cell.style.flexShrink = 0;
            cell.style.flexGrow = 0;


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

        private VisualElement CreateIconCell(string iconPath, float anchoPercent)
        {
            var cell = new VisualElement();
            cell.style.width = Length.Percent(anchoPercent);
            cell.style.height = 20;
            cell.style.flexShrink = 0;
            cell.style.flexGrow = 0;
            cell.style.alignItems = Align.Center;
            cell.style.justifyContent = Justify.Center;

            if (!string.IsNullOrEmpty(iconPath))
            {
                var sprite = Resources.Load<Sprite>(iconPath);
                if (sprite != null)
                {
                    cell.style.backgroundImage = new StyleBackground(sprite);
                    cell.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
            }

            return cell;
        }

        private void CargarDatosJugador(Jugador jugadorSeleccionado)
        {
            nombreJugador.text = jugadorSeleccionado.NombreCompleto;
            demarcacionJugador.text = jugadorSeleccionado.Rol;

            var fotoSprite = Resources.Load<Sprite>($"Jugadores/{jugadorSeleccionado.IdJugador}");
            if (fotoSprite != null)
                fotoJugador.style.backgroundImage = new StyleBackground(fotoSprite);

            lblPortero.text = jugadorSeleccionado.Portero.ToString();
            lblPase.text = jugadorSeleccionado.Pase.ToString();
            lblEntradas.text = jugadorSeleccionado.Entradas.ToString();
            lblRemate.text = jugadorSeleccionado.Remate.ToString();
            lblRegate.text = jugadorSeleccionado.Regate.ToString();
            lblTiro.text = jugadorSeleccionado.Tiro.ToString();

            var posicionIdealSprite = Resources.Load<Sprite>($"Demarcaciones/{jugadorSeleccionado.RolId}");
            if (posicionIdealSprite != null)
                posicionIdeal.style.backgroundImage = new StyleBackground(posicionIdealSprite);

            // Diccionario que mapea cada táctica con sus respectivas posiciones
            var posicionesPorTactica = new Dictionary<string, Dictionary<int, int>>
            {
                ["5-4-1"] = new Dictionary<int, int> { { 1, 1 }, { 2, 4 }, { 3, 4 }, { 4, 4 }, { 5, 2 }, { 6, 3 }, { 7, 6 }, { 8, 6 }, { 9, 11 }, { 10, 12 }, { 11, 10 } },
                ["5-3-2"] = new Dictionary<int, int> { { 1, 1 }, { 2, 4 }, { 3, 4 }, { 4, 4 }, { 5, 2 }, { 6, 3 }, { 7, 5 }, { 8, 6 }, { 9, 6 }, { 10, 10 }, { 11, 10 } },
                ["4-5-1"] = new Dictionary<int, int> { { 1, 1 }, { 2, 4 }, { 3, 4 }, { 4, 2 }, { 5, 3 }, { 6, 5 }, { 7, 6 }, { 8, 6 }, { 9, 11 }, { 10, 12 }, { 11, 10 } },
                ["4-4-2"] = new Dictionary<int, int> { { 1, 1 }, { 2, 4 }, { 3, 4 }, { 4, 2 }, { 5, 3 }, { 6, 5 }, { 7, 6 }, { 8, 6 }, { 9, 7 }, { 10, 10 }, { 11, 10 } },
                ["4-3-3"] = new Dictionary<int, int> { { 1, 1 }, { 2, 4 }, { 3, 4 }, { 4, 2 }, { 5, 3 }, { 6, 5 }, { 7, 6 }, { 8, 6 }, { 9, 8 }, { 10, 9 }, { 11, 10 } },
                ["3-5-2"] = new Dictionary<int, int> { { 1, 1 }, { 2, 4 }, { 3, 4 }, { 4, 4 }, { 5, 5 }, { 6, 6 }, { 7, 6 }, { 8, 11 }, { 9, 12 }, { 10, 10 }, { 11, 10 } }
            };

            // Obtener la posición según la táctica y la alineación del jugador
            int posicion = posicionesPorTactica.TryGetValue(tactica, out var posiciones) && posiciones.TryGetValue(jugadorSeleccionado.PosicionAlineacion, out var pos)
                ? pos
                : 0;

            var posicionActualSprite = Resources.Load<Sprite>($"Demarcaciones/p{posicion}");
            if (posicionActualSprite != null)
                posicionActual.style.backgroundImage = new StyleBackground(posicionActualSprite);
        }

        private void SeleccionarJugador(Jugador jugadorSeleccionado)
        {
            if (jugadorSeleccionado == null) return;

            if (!primerJugadorSeleccionado)
            {
                // Primer jugador seleccionado
                idJugadorUno = jugadorSeleccionado.IdJugador;
                posicionUno = jugadorSeleccionado.PosicionAlineacion;
                jugadorMarcado = 1;
                primerJugadorSeleccionado = true;
            }
            else
            {
                // Segundo jugador seleccionado
                idJugadorDos = jugadorSeleccionado.IdJugador;
                posicionDos = jugadorSeleccionado.PosicionAlineacion;
                jugadorMarcado = 0;
                primerJugadorSeleccionado = false; // Se resetea para futuras selecciones

                // Intercambiar posiciones en la base de datos
                JugadorData.IntercambioPosicion(idJugadorUno, idJugadorDos, posicionUno, posicionDos);

                CargarTitulares();
                CargarReservas();

                // Vaciar imágenes y textos
                LimpiarSeleccion();
            }
        }

        private void MostrarDatosCambio1(Jugador jugador)
        {
            cambio1Container.style.backgroundColor = new StyleColor(new Color32(177, 232, 165, 255));
            jugadorCambio1.text = jugador.NombreCompleto;
            var fotoCambio1Sprite = Resources.Load<Sprite>($"Jugadores/{jugadorSeleccionado.IdJugador}");
            if (fotoCambio1Sprite != null)
                fotoCambio1.style.backgroundImage = new StyleBackground(fotoCambio1Sprite);
        }

        private void MostrarDatosCambio2(Jugador jugador)
        {
            cambio2Container.style.backgroundColor = new StyleColor(new Color32(222, 160, 155, 255));
            jugadorCambio2.text = jugador.NombreCompleto;
            var fotoCambio2Sprite = Resources.Load<Sprite>($"Jugadores/{jugadorSeleccionado.IdJugador}");
            if (fotoCambio2Sprite != null)
                fotoCambio2.style.backgroundImage = new StyleBackground(fotoCambio2Sprite);
        }

        private void LimpiarCambio1()
        {
            cambio1Container.style.backgroundColor = new StyleColor(new Color32(229, 229, 229, 255));
            jugadorCambio1.text = string.Empty;
            fotoCambio1.style.backgroundImage = null;
        }

        private void LimpiarCambio2()
        {
            cambio2Container.style.backgroundColor = new StyleColor(new Color32(229, 229, 229, 255));
            jugadorCambio2.text = string.Empty;
            fotoCambio2.style.backgroundImage = null;
        }

        private void LimpiarSeleccion()
        {
            LimpiarCambio1();
            LimpiarCambio2();
        }

        private void LimpiarDatosJugador()
        {
            nombreJugador.text = string.Empty;
            demarcacionJugador.text = string.Empty;
            fotoJugador.style.backgroundImage = null;
            lblPortero.text = "-";
            lblPase.text = "-";
            lblEntradas.text = "-";
            lblRemate.text = "-";
            lblRegate.text = "-";
            lblTiro.text = "-";
        }

        private int ComprobarMedia(int mediaReal, int posIdeal, int posActual)
        {
            int mediaActualizada = mediaReal;

            if (Math.Abs(posIdeal - posActual) > 8)
            {
                mediaActualizada -= 10;
            }
            else if (Math.Abs(posIdeal - posActual) > 5)
            {
                mediaActualizada -= 8;
            }
            else if (Math.Abs(posIdeal - posActual) > 3)
            {
                mediaActualizada -= 5;
            }
            else
            {
                mediaActualizada = mediaReal;
            }

            return mediaActualizada;
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
    }
}
