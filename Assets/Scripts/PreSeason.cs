using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class PreSeason : MonoBehaviour
    {
        [Header("Sound Clips")]
        [SerializeField] private AudioClip clickSFX;

        // UI Elements
        private Label titulo_ventana, equipo_seleccionado, fecha1, fecha2, fecha3, fecha4,
                      fecha5, estadio1, estadio2, estadio3, estadio4, estadio5, nombre_equipo1,
                      nombre_equipo2, nombre_equipo3, nombre_equipo4, nombre_equipo5;
        private Button btnSeguir, btnVolver, btnCerrar;
        private VisualElement mapContainer, map, listaEquipos, escudo_equipo1, escudo_equipo2,
                              escudo_equipo3, escudo_equipo4, escudo_equipo5, borrar1, borrar2, borrar3,
                              borrar4, borrar5, arrow1, arrow2, arrow3, arrow4, arrow5;
        private Texture2D mapTexture;
        private VisualElement escudoSeleccionado = null;

        private int equipoSeleccionadoId;
        private int indiceSeleccionado = 0;
        private string colorHex;

        // Lista con las fechas de los partidos
        private List<string> fechas;

        // Lista con los equipos seleccionados.
        private List<int> rivales = new List<int> { 0, 0, 0, 0, 0 };

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // Referencias a elementos pantalla UI
            escudo_equipo1 = root.Q<VisualElement>("escudo-equipo1");
            escudo_equipo2 = root.Q<VisualElement>("escudo-equipo2");
            escudo_equipo3 = root.Q<VisualElement>("escudo-equipo3");
            escudo_equipo4 = root.Q<VisualElement>("escudo-equipo4");
            escudo_equipo5 = root.Q<VisualElement>("escudo-equipo5");
            arrow1 = root.Q<VisualElement>("arrow-icon1");
            arrow2 = root.Q<VisualElement>("arrow-icon2");
            arrow3 = root.Q<VisualElement>("arrow-icon3");
            arrow4 = root.Q<VisualElement>("arrow-icon4");
            arrow5 = root.Q<VisualElement>("arrow-icon5");
            borrar1 = root.Q<VisualElement>("borrar1");
            borrar2 = root.Q<VisualElement>("borrar2");
            borrar3 = root.Q<VisualElement>("borrar3");
            borrar4 = root.Q<VisualElement>("borrar4");
            borrar5 = root.Q<VisualElement>("borrar5");
            titulo_ventana = root.Q<Label>("window-title");
            fecha1 = root.Q<Label>("fecha1");
            fecha2 = root.Q<Label>("fecha2");
            fecha3 = root.Q<Label>("fecha3");
            fecha4 = root.Q<Label>("fecha4");
            fecha5 = root.Q<Label>("fecha5");
            estadio1 = root.Q<Label>("estadio1");
            estadio2 = root.Q<Label>("estadio2");
            estadio3 = root.Q<Label>("estadio3");
            estadio4 = root.Q<Label>("estadio4");
            estadio5 = root.Q<Label>("estadio5");
            nombre_equipo1 = root.Q<Label>("nombre-equipo1");
            nombre_equipo2 = root.Q<Label>("nombre-equipo2");
            nombre_equipo3 = root.Q<Label>("nombre-equipo3");
            nombre_equipo4 = root.Q<Label>("nombre-equipo4");
            nombre_equipo5 = root.Q<Label>("nombre-equipo5");
            btnVolver = root.Q<Button>("btnVolver");
            btnSeguir = root.Q<Button>("btnSeguir");

            mapContainer = root.Q<VisualElement>("map-container");
            mapContainer.RemoveFromClassList("visible");
            listaEquipos = root.Q<VisualElement>("lista-equipos");
            map = root.Q<VisualElement>("map");
            btnCerrar = root.Q<Button>("btnCerrar");
            equipo_seleccionado = root.Q<Label>("equipo-seleccionado");

            // Carga la textura original usada en el USS
            mapTexture = Resources.Load<Texture2D>("Maps/europe");

            // Escucha el click del usuario sobre el mapa
            map.RegisterCallback<ClickEvent>(OnMapClick);

            // Asegurase de que la temporada esté inicializada
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            fechas = new List<string>
            {
                new DateTime(FechaData.temporadaActual, 7, 29).ToString("dd/MM/yyyy"),
                new DateTime(FechaData.temporadaActual, 7, 31).ToString("dd/MM/yyyy"),
                new DateTime(FechaData.temporadaActual, 8, 2).ToString("dd/MM/yyyy"),
                new DateTime(FechaData.temporadaActual, 8, 4).ToString("dd/MM/yyyy"),
                new DateTime(FechaData.temporadaActual, 8, 6).ToString("dd/MM/yyyy")
            };

            // ID de Mi equipo
            equipoSeleccionadoId = PlayerPrefs.GetInt("MyTeam");

            // Actualizar el título de la ventana con el nombre del equipo
            Equipo equipo = EquipoData.ObtenerDetallesEquipo(equipoSeleccionadoId);
            titulo_ventana.text = $"{titulo_ventana.text} - {equipo.Nombre.ToUpper()}";

            arrow1.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MostrarMapa(1);
            });

            arrow2.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MostrarMapa(2);
            });

            arrow3.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MostrarMapa(3);
            });

            arrow4.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MostrarMapa(4);
            });

            arrow5.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MostrarMapa(5);
            });

            borrar1.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                fecha1.text = string.Empty;
                nombre_equipo1.text = string.Empty;
                estadio1.text = string.Empty;
                escudo_equipo1.style.backgroundImage = null;
                rivales[0] = 0;
            });

            borrar2.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                fecha2.text = string.Empty;
                nombre_equipo2.text = string.Empty;
                estadio2.text = string.Empty;
                escudo_equipo2.style.backgroundImage = null;
                rivales[1] = 0;
            });

            borrar3.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                fecha3.text = string.Empty;
                nombre_equipo3.text = string.Empty;
                estadio3.text = string.Empty;
                escudo_equipo3.style.backgroundImage = null;
                rivales[2] = 0;
            });

            borrar4.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                fecha4.text = string.Empty;
                nombre_equipo4.text = string.Empty;
                estadio4.text = string.Empty;
                escudo_equipo4.style.backgroundImage = null;
                rivales[3] = 0;
            });

            borrar5.RegisterCallback<ClickEvent>((evt) =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                fecha5.text = string.Empty;
                nombre_equipo5.text = string.Empty;
                estadio5.text = string.Empty;
                escudo_equipo5.style.backgroundImage = null;
                rivales[4] = 0;
            });

            btnVolver.clicked += () =>
            {
                // Vaciar lista de partidos amistosos
                PartidoData.EliminarPartidos(Partido.idsPartidos);

                SceneLoader.Instance.LoadScene(Constants.TEAM_SELECTION_SCENE);
            };

            btnSeguir.clicked += () =>
            {
                // Limpiamos la lista de ids.
                Partido.idsPartidos.Clear();

                // Competición siempre será 10 para amistosos
                int competicion = 10;

                // Recorrer las 5 fechas, donde mi equipo es el visitante
                for (int i = 0; i < 5; i++)
                {
                    if (rivales[i] != 0)
                    {
                        string fechaFormateada = DateTime.Parse(fechas[i]).ToString("yyyy-MM-dd"); // Convertir a string sin hora

                        int idPartido = PartidoData.CrearPartido(
                            rivales[i],
                            equipoSeleccionadoId,
                            fechaFormateada,
                            competicion,
                            0
                        );

                        if (idPartido != -1)
                        {
                            Partido.idsPartidos.Add(idPartido); // Guardar el id del partido
                        }
                    }
                }

                // Cargar siguiente escena
                SceneLoader.Instance.LoadScene(Constants.TEAM_OBJECTIVES_SCENE);
            };


            btnCerrar.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                OcultarMapa();
            };
        }

        private void MostrarMapa(int num)
        {
            indiceSeleccionado = num;
            mapContainer.AddToClassList("visible");
        }

        private void OcultarMapa()
        {
            mapContainer.RemoveFromClassList("visible");
        }

        private void OnMapClick(ClickEvent evt)
        {
            AudioManager.Instance.PlaySFX(clickSFX);

            Vector2 localPos = evt.localPosition;
            float elemWidth = map.resolvedStyle.width;
            float elemHeight = map.resolvedStyle.height;

            float texWidth = mapTexture.width;
            float texHeight = mapTexture.height;

            // --- Cálculo del escalado con background-size: cover ---
            float elemAspect = elemWidth / elemHeight;
            float texAspect = texWidth / texHeight;

            float drawWidth, drawHeight;
            float offsetX = 0f, offsetY = 0f;

            if (texAspect > elemAspect)
            {
                // La textura es más ancha que el elemento → recorta en X
                drawHeight = elemHeight;
                drawWidth = texWidth * (elemHeight / texHeight);
                offsetX = (drawWidth - elemWidth) / 2f;
            }
            else
            {
                // La textura es más alta que el elemento → recorta en Y
                drawWidth = elemWidth;
                drawHeight = texHeight * (elemWidth / texWidth);
                offsetY = (drawHeight - elemHeight) / 2f;
            }

            // --- Convertir posición del clic a coordenadas en la textura ---
            float xNorm = (localPos.x + offsetX) / drawWidth;
            float yNorm = 1f - ((localPos.y + offsetY) / drawHeight);

            int texX = Mathf.Clamp(Mathf.RoundToInt(xNorm * texWidth), 0, mapTexture.width - 1);
            int texY = Mathf.Clamp(Mathf.RoundToInt(yNorm * texHeight), 0, mapTexture.height - 1);

            // --- Obtener el color ---
            Color pixel = mapTexture.GetPixel(texX, texY);
            colorHex = "#" + ColorUtility.ToHtmlStringRGB(pixel);

            string pais = ComprobarPais(colorHex);
            CargarEscudos(pais);
        }

        private string ComprobarPais(string hex)
        {
            return Constants.PaisesPorColor.TryGetValue(hex, out var pais) ? pais : "";
        }

        private void CargarEscudos(string pais)
        {
            listaEquipos.Clear();

            List<Equipo> equipos = EquipoData.ObtenerEquiposPorPais(pais);

            // Crear filas de 10 escudos
            VisualElement fila = null;
            int contador = 0;

            foreach (var equipo in equipos)
            {
                if (equipo.IdEquipo != equipoSeleccionadoId)
                {
                    if (contador % 9 == 0)
                    {
                        fila = new VisualElement();
                        fila.style.flexDirection = FlexDirection.Row;
                        fila.style.flexWrap = Wrap.NoWrap;
                        fila.style.marginBottom = 50;
                        listaEquipos.Add(fila);
                    }

                    var sprite = Resources.Load<Sprite>($"EscudosEquipos/80x80/{equipo.IdEquipo}");
                    var escudo = CrearElementoEscudo(sprite, equipo.IdEquipo);
                    fila.Add(escudo);

                    contador++;
                }
            }
        }

        private VisualElement CrearElementoEscudo(Sprite sprite, int idEquipo)
        {
            var imagen = new VisualElement();
            imagen.style.width = 80;
            imagen.style.height = 80;
            imagen.AddToClassList("escudo"); // Clase base

            if (sprite != null)
            {
                imagen.style.backgroundImage = new StyleBackground(sprite);
            }

            imagen.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                // --- RESALTAR ESCUDO ---
                if (escudoSeleccionado != null)
                    escudoSeleccionado.RemoveFromClassList("escudo-seleccionado"); // desmarcar anterior

                imagen.AddToClassList("escudo-seleccionado"); // marcar el nuevo
                escudoSeleccionado = imagen;

                // --- Código existente para actualizar datos ---
                string nombreEquipo = EquipoData.GetTeamNameById(idEquipo);
                equipo_seleccionado.text = nombreEquipo;

                switch (indiceSeleccionado)
                {
                    case 1:
                        fecha1.text = fechas[0].ToString();
                        nombre_equipo1.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Nombre;
                        estadio1.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Estadio;
                        var sprite1 = Resources.Load<Sprite>($"EscudosEquipos/120x120/{idEquipo}");
                        escudo_equipo1.style.backgroundImage = new StyleBackground(sprite1);
                        rivales[0] = idEquipo;
                        break;
                    case 2:
                        fecha2.text = fechas[1].ToString();
                        nombre_equipo2.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Nombre;
                        estadio2.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Estadio;
                        Sprite sprite2 = Resources.Load<Sprite>($"EscudosEquipos/120x120/{idEquipo}");
                        escudo_equipo2.style.backgroundImage = new StyleBackground(sprite2);
                        rivales[1] = idEquipo;
                        break;
                    case 3:
                        fecha3.text = fechas[2].ToString();
                        nombre_equipo3.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Nombre;
                        estadio3.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Estadio;
                        Sprite sprite3 = Resources.Load<Sprite>($"EscudosEquipos/120x120/{idEquipo}");
                        escudo_equipo3.style.backgroundImage = new StyleBackground(sprite3);
                        rivales[2] = idEquipo;
                        break;
                    case 4:
                        fecha4.text = fechas[3].ToString();
                        nombre_equipo4.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Nombre;
                        estadio4.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Estadio;
                        Sprite sprite4 = Resources.Load<Sprite>($"EscudosEquipos/120x120/{idEquipo}");
                        escudo_equipo4.style.backgroundImage = new StyleBackground(sprite4);
                        rivales[3] = idEquipo;
                        break;
                    case 5:
                        fecha5.text = fechas[4].ToString();
                        nombre_equipo5.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Nombre;
                        estadio5.text = EquipoData.ObtenerDetallesEquipo(idEquipo).Estadio;
                        Sprite sprite5 = Resources.Load<Sprite>($"EscudosEquipos/120x120/{idEquipo}");
                        escudo_equipo5.style.backgroundImage = new StyleBackground(sprite5);
                        rivales[4] = idEquipo;
                        break;
                }
            });

            return imagen;
        }
    }
}