using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class CompeticionesResultados
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;
        int competicionActiva;

        // Jornadas y Rondas
        private int jornadaActual;
        private const int jornadaMin = 1;
        private int jornadaMax = 38;
        private int rondaActual = 1;
        private int vueltaActual = 0;
        private const int rondaMin = 1;
        private const int rondaMax = 6;
        private const int vueltaMin = 0;
        private const int vueltaMax = 1;

        private VisualElement root, resultadosContainer, competicion1, competicion3, competicion4, competicion5,
                              btnMenos, btnMas, colIzq, colDer;
        private Label tituloVentana, jornada;

        public CompeticionesResultados(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            var scrollView = root.Q<ScrollView>("resultados-container");
            scrollView.contentContainer.style.flexDirection = FlexDirection.Row;
            colIzq = root.Q<VisualElement>("ColumnaIzq");
            colDer = root.Q<VisualElement>("ColumnaDer");
            competicion1 = root.Q<VisualElement>("liga1-logo");
            competicion3 = root.Q<VisualElement>("copa-logo");
            competicion4 = root.Q<VisualElement>("europa1-logo");
            competicion5 = root.Q<VisualElement>("europa2-logo");
            btnMenos = root.Q<VisualElement>("btnMenos");
            btnMas = root.Q<VisualElement>("btnMas");
            tituloVentana = root.Q<Label>("titulo-ventana");
            jornada = root.Q<Label>("jornada");

            competicionActiva = miEquipo.IdCompeticion;

            // Iconos de las competiciones
            Sprite competicion1Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/{miEquipo.IdCompeticion}");
            if (competicion1Sprite != null)
                competicion1.style.backgroundImage = new StyleBackground(competicion1Sprite);

            Sprite competicion3Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/4");
            if (competicion3Sprite != null)
                competicion3.style.backgroundImage = new StyleBackground(competicion3Sprite);

            Sprite competicion4Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/5");
            if (competicion4Sprite != null)
                competicion4.style.backgroundImage = new StyleBackground(competicion4Sprite);

            Sprite competicion5Sprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/6");
            if (competicion5Sprite != null)
                competicion5.style.backgroundImage = new StyleBackground(competicion5Sprite);

            // Eventos de los botones de las Competiciones
            competicion1.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 1;
                GestionarJornadaMaxima(competicionActiva);
                CargarPartidos();
                GestionarFlechas();
            });

            competicion3.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 4;
                if (vueltaActual == 0)
                {
                    jornada.text = $"{PartidoData.ObtenerNombreRonda(rondaActual)} (Ida)";
                }
                else
                {
                    jornada.text = $"{PartidoData.ObtenerNombreRonda(rondaActual)} (Vuelta)";
                }

                CargarPartidos();
                GestionarFlechas();
            });

            competicion4.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 5;
                GestionarJornadaMaxima(competicionActiva);
                CargarPartidos();
                GestionarFlechas();
            });

            competicion5.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                competicionActiva = 6;
                GestionarJornadaMaxima(competicionActiva);
                CargarPartidos();
                GestionarFlechas();
            });

            // Eventos de las Flechas
            btnMenos.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                if (competicionActiva != 4)
                {
                    if (jornadaActual > jornadaMin)
                    {
                        jornadaActual--;
                        List<Partido> partidosJornada = PartidoData.CargarPartidosPorCompeticion(jornadaActual, competicionActiva, 0, 0);
                        MostrarPartidos(partidosJornada);
                        jornada.text = $"Jornada {jornadaActual}";
                        GestionarFlechas();
                    }
                }
                else
                {
                    // Primero intenta retroceder la vuelta
                    if (vueltaActual > vueltaMin)
                    {
                        vueltaActual--;
                    }
                    else
                    {
                        // Si ya estabas en la vuelta mínima, retrocede la ronda y pon la vuelta al máximo
                        if (rondaActual > rondaMin)
                        {
                            rondaActual--;
                            vueltaActual = vueltaMax;
                        }
                        else
                        {
                            return;
                        }
                    }

                    // Cargar los partidos correspondientes a la nueva ronda/vuelta
                    List<Partido> partidosRonda = PartidoData.CargarPartidosPorCompeticion(jornadaActual, competicionActiva, rondaActual, vueltaActual);
                    if (vueltaActual == 0)
                    {
                        jornada.text = $"{PartidoData.ObtenerNombreRonda(rondaActual)} (Ida)";
                    }
                    else
                    {
                        jornada.text = $"{PartidoData.ObtenerNombreRonda(rondaActual)} (Vuelta)";
                    }
                    MostrarPartidos(partidosRonda);
                    GestionarFlechas();
                }
            });

            btnMas.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);

                if (competicionActiva != 4)
                {
                    if (jornadaActual < jornadaMax)
                    {
                        jornadaActual++;
                        List<Partido> partidosJornada = PartidoData.CargarPartidosPorCompeticion(jornadaActual, competicionActiva, 0, 0);
                        MostrarPartidos(partidosJornada);
                        jornada.text = $"Jornada {jornadaActual}";
                        GestionarFlechas();
                    }
                }
                else
                {
                    // Si ya estás en la Final (ronda 6) y en la ida (0), no avanzar más
                    if (rondaActual == 6 && vueltaActual == 0)
                        return;

                    // Primero intenta avanzar la vuelta
                    if (vueltaActual < vueltaMax)
                    {
                        vueltaActual++;
                    }
                    else
                    {
                        // Si ya estaba en la vuelta máxima, resetea la vuelta y avanza la ronda
                        if (rondaActual < rondaMax)
                        {
                            rondaActual++;
                            vueltaActual = vueltaMin;
                        }
                        else
                        {
                            return;
                        }
                    }

                    // Cargar los partidos correspondientes a la nueva ronda/vuelta
                    List<Partido> partidosRonda = PartidoData.CargarPartidosPorCompeticion(jornadaActual, competicionActiva, rondaActual, vueltaActual);
                    if (vueltaActual == 0)
                    {
                        jornada.text = $"{PartidoData.ObtenerNombreRonda(rondaActual)} (Ida)";
                    }
                    else
                    {
                        jornada.text = $"{PartidoData.ObtenerNombreRonda(rondaActual)} (Vuelta)";
                    }
                    MostrarPartidos(partidosRonda);
                    GestionarFlechas();
                }
            });

            CargarPartidos();
        }

        private void CargarPartidos()
        {
            ActualizarTituloVentana(competicionActiva);

            jornadaActual = Math.Max(
                1,
                PartidoData.ObtenerUltimaJornadaJugada(miEquipo.IdEquipo, competicionActiva)
            );

            List<Partido> partidosJornada =
                PartidoData.CargarPartidosPorCompeticion(jornadaActual, competicionActiva, rondaActual, vueltaActual);

            MostrarPartidos(partidosJornada);

            if (competicionActiva != 4)
            {
                jornada.text = $"Jornada {jornadaActual}";
            }
            else
            {
                if (vueltaActual == 0)
                {
                    jornada.text = $"{PartidoData.ObtenerNombreRonda(rondaActual)} (Ida)";
                }
                else
                {
                    jornada.text = $"{PartidoData.ObtenerNombreRonda(rondaActual)} (Vuelta)";
                }
            }
        }


        private void MostrarPartidos(List<Partido> partidos)
        {
            colIzq.Clear();
            colDer.Clear();

            if (partidos == null || partidos.Count == 0)
                return;

            int mitad = Mathf.CeilToInt(partidos.Count / 2f);

            for (int i = 0; i < partidos.Count; i++)
            {
                var fila = CrearFilaPartido(partidos[i]);

                if (i < mitad)
                    colIzq.Add(fila);
                else
                    colDer.Add(fila);
            }
        }

        private VisualElement CrearColumna()
        {
            var col = new VisualElement();
            col.style.flexDirection = FlexDirection.Column;
            col.style.width = Length.Percent(48);
            col.style.paddingLeft = 10;
            col.style.paddingRight = 10;
            return col;
        }

        private VisualElement CrearFilaPartido(Partido p)
        {
            var fila = new VisualElement();
            fila.style.flexDirection = FlexDirection.Row;
            fila.style.width = Length.Percent(100);
            fila.style.minHeight = 50;
            fila.style.maxHeight = 50;

            fila.style.alignItems = Align.Center;

            Color bg = new Color32(245, 245, 245, 255);
            fila.style.backgroundColor = bg;

            // ESCUDO LOCAL
            fila.Add(CrearEscudo(p.IdEquipoLocal));

            // NOMBRE LOCAL
            if (p.IdEquipoLocal == miEquipo.IdEquipo)
                fila.Add(CrearTexto(EquipoData.ObtenerDetallesEquipo(p.IdEquipoLocal).Nombre, 35, TextAnchor.MiddleLeft, true));
            else
                fila.Add(CrearTexto(EquipoData.ObtenerDetallesEquipo(p.IdEquipoLocal).Nombre, 35, TextAnchor.MiddleLeft, false));

            // GOLES LOCAL
            fila.Add(CrearTexto(p.GolesLocal.ToString(), 5, TextAnchor.MiddleCenter, true));

            // ESCUDO VISITANTE
            fila.Add(CrearEscudo(p.IdEquipoVisitante));

            // NOMBRE VISITANTE
            if (p.IdEquipoVisitante == miEquipo.IdEquipo)
                fila.Add(CrearTexto(EquipoData.ObtenerDetallesEquipo(p.IdEquipoVisitante).Nombre, 30, TextAnchor.MiddleLeft, true));
            else
                fila.Add(CrearTexto(EquipoData.ObtenerDetallesEquipo(p.IdEquipoVisitante).Nombre, 30, TextAnchor.MiddleLeft, false));

            // GOLES VISITANTE
            fila.Add(CrearTexto(p.GolesVisitante.ToString(), 5, TextAnchor.MiddleCenter, true));

            return fila;
        }

        private VisualElement CrearEscudo(int idEquipo)
        {
            var escudo = new VisualElement();
            escudo.style.width = Length.Percent(10);
            escudo.style.height = 32;

            var sprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{idEquipo}");
            if (sprite != null)
            {
                escudo.style.backgroundImage = new StyleBackground(sprite);
                escudo.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            }

            return escudo;
        }

        private VisualElement CrearTexto(string contenido, float widthPercent, TextAnchor anchor, bool negrita)
        {
            // Fuente Poppins-Bold
            var fontPath = negrita
                ? "Fonts/Poppins-SemiBold SDF"
                : "Fonts/Poppins-Regular SDF";


            var container = new VisualElement();
            container.style.width = Length.Percent(widthPercent);
            container.style.flexDirection = FlexDirection.Row;
            container.style.alignItems = Align.Center;

            var label = new Label(contenido);
            label.style.unityTextAlign = anchor;
            label.style.fontSize = 18;
            var fontAsset = Resources.Load<UnityEngine.TextCore.Text.FontAsset>(fontPath);
            label.style.unityFontDefinition = new StyleFontDefinition(fontAsset);

            container.Add(label);
            return container;
        }

        private void ActualizarTituloVentana(int comp)
        {
            tituloVentana.text = $"RESULTADOS {CompeticionData.MostrarNombreCompeticion(comp).ToUpper()}";
        }

        private void GestionarFlechas()
        {
            if (competicionActiva == 4)
            {
                if (rondaActual == 6 && vueltaActual == 0)
                {
                    btnMenos.style.visibility = Visibility.Visible;
                    btnMas.style.visibility = Visibility.Hidden;
                }
                else if (rondaActual == 1 && vueltaActual == 0)
                {
                    btnMenos.style.visibility = Visibility.Hidden;
                    btnMas.style.visibility = Visibility.Visible;
                }
                else
                {
                    btnMenos.style.visibility = Visibility.Visible;
                    btnMas.style.visibility = Visibility.Visible;
                }
            }
            else
            {
                if (jornadaActual <= 1)
                {
                    btnMenos.style.visibility = Visibility.Hidden;
                    btnMas.style.visibility = Visibility.Visible;
                }
                else
                {
                    if (jornadaActual >= jornadaMax)
                    {
                        btnMenos.style.visibility = Visibility.Visible;
                        btnMas.style.visibility = Visibility.Hidden;
                    }
                    else
                    {
                        btnMenos.style.visibility = Visibility.Visible;
                        btnMas.style.visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void GestionarJornadaMaxima(int comp)
        {
            if (comp < 3)
            {
                jornadaMax = 38;
            }
            else
            {
                jornadaMax = 12;
            }
        }
    }
}