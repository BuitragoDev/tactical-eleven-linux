using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Data.SQLite;
using System.IO;
using Unity.VisualScripting;
using System;

namespace TacticalEleven.Scripts
{
    public partial class TeamSelection : MonoBehaviour
    {
        [Header("Sound Clips")]
        [SerializeField] private AudioClip clickSFX;

        // UI Elements
        private VisualElement areaEscudos, background, ventanaDetalles, escudo_equipo;
        private Button btnSeguir, btnVolver, btnDetalles, btnCerrar;
        private Label nombreEquipo, presidente, presupuesto, estadio, objetivo;
        private VisualElement division1Logo, division2Logo;
        private VisualElement escudoSeleccionado = null;

        private int equipoSeleccionadoId = -1;

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // Referencias a elementos pantalla principal
            areaEscudos = root.Q<VisualElement>("area-escudos");
            background = root.Q<VisualElement>("background-image");
            ventanaDetalles = root.Q<VisualElement>("ventanaDetalles");
            btnVolver = root.Q<Button>("btnVolver");
            btnSeguir = root.Q<Button>("btnSeguir");
            btnSeguir.SetEnabled(false);
            btnDetalles = root.Q<Button>("btnDetalles");
            division1Logo = root.Q<VisualElement>("division1-logo");
            division2Logo = root.Q<VisualElement>("division2-logo");

            // Referencias a elementos pantalla detalles
            escudo_equipo = root.Q<VisualElement>("escudo-equipo");
            nombreEquipo = root.Q<Label>("nombre-equipo");
            presidente = root.Q<Label>("presidente");
            presupuesto = root.Q<Label>("presupuesto");
            estadio = root.Q<Label>("estadio");
            objetivo = root.Q<Label>("objetivo");
            btnCerrar = root.Q<Button>("btnCerrar");

            ventanaDetalles.style.display = DisplayStyle.None; // Ventana de detalles oculta al principio

            btnVolver.clicked += () =>
            {
                if (PlayerPrefs.HasKey("MyTeam"))
                {
                    PlayerPrefs.DeleteKey("MyTeam");
                }

                SceneLoader.Instance.LoadScene(Constants.MAIN_MENU_SCENE);
            };

            btnSeguir.clicked += () =>
            {
                // Guardo el ID del equipo que he seleccionado en un PlayerPref
                if (equipoSeleccionadoId != -1)
                {
                    PlayerPrefs.SetInt("MyTeam", equipoSeleccionadoId);
                }

                ManagerData.AgregarEquipoSeleccionado(equipoSeleccionadoId);

                SceneLoader.Instance.LoadScene(Constants.PRE_SEASON_SCENE);
            };

            // Ruta de la base de datos
            string dbPath = DatabaseManager.GetActiveDatabasePath();

            // Cargar por defecto los equipos de competición 1
            CargarEscudosPorCompeticion(1);

            // Click en división 1
            division1Logo.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                // Cambiar sprites de los logos
                SetLogoSprite(division1Logo, "LogosCompeticiones/1off");
                SetLogoSprite(division2Logo, "LogosCompeticiones/2");

                division1Logo.SetEnabled(false);
                division2Logo.SetEnabled(true);

                CargarEscudosPorCompeticion(1);
            });

            // Click en división 2
            division2Logo.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                // Cambiar sprites de los logos
                SetLogoSprite(division1Logo, "LogosCompeticiones/1");
                SetLogoSprite(division2Logo, "LogosCompeticiones/2off");

                division1Logo.SetEnabled(true);
                division2Logo.SetEnabled(false);

                CargarEscudosPorCompeticion(2);
            });

            // Click en Ver Detalles
            btnDetalles.clicked += () =>
            {
                ventanaDetalles.style.display = DisplayStyle.Flex;
                background.SetEnabled(false);
                float presupuestoConversion = 0f;
                string symbol = "€";

                Equipo equipo = EquipoData.ObtenerDetallesEquipo(equipoSeleccionadoId);
                if (equipo != null)
                {
                    var sprite = Resources.Load<Sprite>($"EscudosEquipos/{equipo.IdEquipo}");
                    escudo_equipo.style.backgroundImage = new StyleBackground(sprite);

                    string currency = PlayerPrefs.GetString("Currency", string.Empty);
                    if (currency != string.Empty)
                    {
                        switch (currency)
                        {
                            case Constants.EURO_NAME:
                                presupuestoConversion = equipo.Presupuesto * Constants.EURO_VALUE;
                                symbol = Constants.EURO_SYMBOL;
                                break;
                            case Constants.POUND_NAME:
                                presupuestoConversion = equipo.Presupuesto * Constants.POUND_VALUE;
                                symbol = Constants.POUND_SYMBOL;
                                break;
                            case Constants.DOLLAR_NAME:
                                presupuestoConversion = equipo.Presupuesto * Constants.DOLLAR_VALUE;
                                symbol = Constants.DOLLAR_SYMBOL;
                                break;
                        }
                    }
                    else
                    {
                        presupuestoConversion = equipo.Presupuesto;
                    }

                    nombreEquipo.text = equipo.Nombre;
                    presidente.text = equipo.Presidente;
                    presupuesto.text = $"{presupuestoConversion.ToString("N0")} {symbol}";
                    estadio.text = $"{equipo.Estadio} ({equipo.Aforo.ToString("N0")} asientos)";
                    objetivo.text = equipo.Objetivo;
                }
            };

            // Click en Cerrar Detalles
            btnCerrar.clicked += () =>
            {
                ventanaDetalles.style.display = DisplayStyle.None;
                background.SetEnabled(true);

                nombreEquipo.text = "";
                presidente.text = "";
                presupuesto.text = "";
                estadio.text = "";
                objetivo.text = "";
            };
        }

        // Función auxiliar para cambiar el sprite de un logo
        private void SetLogoSprite(VisualElement logo, string spritePath)
        {
            // spritePath debe ser la ruta relativa dentro de Resources, sin extensión
            Sprite sprite = Resources.Load<Sprite>(spritePath);
            if (sprite != null)
            {
                logo.style.backgroundImage = new StyleBackground(sprite);
            }
        }

        private void CargarEscudosPorCompeticion(int idCompeticion)
        {
            areaEscudos.Clear();

            List<Equipo> equipos = EquipoData.ObtenerEquiposPorCompeticion(idCompeticion);

            // Crear filas de 10 escudos
            VisualElement fila = null;
            int contador = 0;

            foreach (var equipo in equipos)
            {
                if (contador % 10 == 0)
                {
                    fila = new VisualElement();
                    fila.style.flexDirection = FlexDirection.Row;
                    fila.style.flexWrap = Wrap.NoWrap;
                    fila.style.marginBottom = 50;
                    areaEscudos.Add(fila);
                }

                var sprite = Resources.Load<Sprite>($"EscudosEquipos/120x120/{equipo.IdEquipo}");
                var escudo = CrearElementoEscudo(sprite, equipo.IdEquipo);
                fila.Add(escudo);

                contador++;
            }
        }

        private VisualElement CrearElementoEscudo(Sprite sprite, int idEquipo)
        {
            var imagen = new VisualElement();
            imagen.style.width = 120;
            imagen.style.height = 120;
            imagen.AddToClassList("escudo"); // Clase USS para background-size, etc.

            if (sprite != null)
            {
                imagen.style.backgroundImage = new StyleBackground(sprite);
            }

            // Callback al pulsar el escudo
            imagen.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);    // Reproducir sonido de click

                // --- RESALTAR ESCUDO ---
                if (escudoSeleccionado != null)
                    escudoSeleccionado.RemoveFromClassList("escudo-seleccionado"); // desmarcar anterior

                imagen.AddToClassList("escudo-seleccionado"); // marcar el nuevo
                escudoSeleccionado = imagen;

                PlayerPrefs.SetInt("miEquipo", idEquipo);   // Guardar el equipo seleccionado en un PlayerPref

                // Guardar el id del equipo seleccionado
                equipoSeleccionadoId = idEquipo;

                // Obtener el nombre del equipo desde la clase EquipoData
                string nombreEquipo = EquipoData.GetTeamNameById(idEquipo);

                // Actualizar el texto del botón
                if (!string.IsNullOrEmpty(nombreEquipo))
                {
                    btnDetalles.text = $"VER DETALLES DEL {nombreEquipo.ToUpper()}";
                }
                else
                {
                    btnDetalles.text = "VER DETALLES";
                }

                btnSeguir.SetEnabled(true);
            });

            return imagen;
        }
    }
}