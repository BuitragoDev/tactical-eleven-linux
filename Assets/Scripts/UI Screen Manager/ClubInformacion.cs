using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class ClubInformacion
    {
        private Equipo miEquipo;
        private Manager miManager;
        Jugador jugador;
        Estadistica estadistica;

        private VisualElement root;
        private VisualElement escudoEquipo, valoracionEquipo, logoCampeonato, valoracionEntrenador, equipacionUno, equipacionDos, escudoRival,
                              fotoMaximoGoleador, fotoMaximoAsistente, fotoMaximoMVP;
        private Label nombreEquipo, mediaEquipo, nombreEntrenador, estadio, aforo, maximoGoleadorNombre, maximoGoleadorPosicion, maximoGoleadorCantidad,
                      maximoAsistenteNombre, maximoAsistentePosicion, maximoAsistenteCantidad,
                      maximoMVPNombre, maximoMVPPosicion, maximoMVPCantidad;

        public ClubInformacion(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;

            // Referencias a objetos de la UI
            escudoEquipo = root.Q<VisualElement>("escudo-equipo");
            valoracionEquipo = root.Q<VisualElement>("estrellas-contenedor");
            logoCampeonato = root.Q<VisualElement>("logo-competicion");
            valoracionEntrenador = root.Q<VisualElement>("valoracion-entrenador");
            equipacionUno = root.Q<VisualElement>("equipacion-uno");
            equipacionDos = root.Q<VisualElement>("equipacion-dos");
            escudoRival = root.Q<VisualElement>("escudo-rival");
            fotoMaximoGoleador = root.Q<VisualElement>("maximo-goleador");
            fotoMaximoAsistente = root.Q<VisualElement>("maximo-asistente");
            fotoMaximoMVP = root.Q<VisualElement>("maximo-mvp");
            nombreEquipo = root.Q<Label>("nombre-equipo");
            mediaEquipo = root.Q<Label>("media-equipo");
            nombreEntrenador = root.Q<Label>("nombre-entrenador");
            estadio = root.Q<Label>("estadio-nombre");
            aforo = root.Q<Label>("estadio-aforo");
            maximoGoleadorNombre = root.Q<Label>("maximo-goleador-nombre");
            maximoGoleadorPosicion = root.Q<Label>("maximo-goleador-posicion");
            maximoGoleadorCantidad = root.Q<Label>("maximo-goleador-goles");
            maximoAsistenteNombre = root.Q<Label>("maximo-asistente-nombre");
            maximoAsistentePosicion = root.Q<Label>("maximo-asistente-posicion");
            maximoAsistenteCantidad = root.Q<Label>("maximo-asistente-asistencias");
            maximoMVPNombre = root.Q<Label>("maximo-mvp-nombre");
            maximoMVPPosicion = root.Q<Label>("maximo-mvp-posicion");
            maximoMVPCantidad = root.Q<Label>("maximo-mvp-mvps");

            // Carga de datos
            // Escudo equipo
            Sprite miEquipoSprite = Resources.Load<Sprite>($"EscudosEquipos/120x120/{miEquipo.IdEquipo}");
            if (miEquipoSprite != null)
                escudoEquipo.style.backgroundImage = new StyleBackground(miEquipoSprite);

            // Nombre equipo
            nombreEquipo.text = miEquipo.Nombre;

            // Valoración equipo
            MostrarEstrellas(EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Reputacion, valoracionEquipo);

            // Media equipo
            double mediaEquipoValue = JugadorData.ObtenerMediaEquipo(miEquipo.IdEquipo);
            mediaEquipo.text = Math.Round(mediaEquipoValue).ToString();

            if (mediaEquipoValue > 85)
            {
                mediaEquipo.style.color = (Color)new Color32(0x1E, 0x72, 0x3C, 0xFF);  // verde oscuro
            }
            else if (mediaEquipoValue >= 75)
            {
                mediaEquipo.style.color = (Color)new Color32(0x3A, 0x8E, 0x42, 0xFF);  // verde medio
            }
            else if (mediaEquipoValue >= 65)
            {
                mediaEquipo.style.color = (Color)new Color32(0xC6, 0x76, 0x17, 0xFF);  // naranja oscuro
            }
            else
            {
                mediaEquipo.style.color = (Color)new Color32(0xA3, 0x1E, 0x1E, 0xFF);  // rojo oscuro
            }

            // Logo campeonato
            Sprite logoCampeonatoSprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/{miEquipo.IdCompeticion}");
            if (logoCampeonatoSprite != null)
                logoCampeonato.style.backgroundImage = new StyleBackground(logoCampeonatoSprite);

            // Nombre entrenador
            nombreEntrenador.text = $"{ManagerData.MostrarManager().Nombre} {ManagerData.MostrarManager().Apellido}";

            // Valoración entrenador
            MostrarEstrellas(ManagerData.MostrarManager().Reputacion, valoracionEntrenador);

            // Estadio
            estadio.text = miEquipo.Estadio;
            aforo.text = "(" + miEquipo.Aforo.ToString("N0") + " asientos)";

            // Equipaciones
            Sprite equipacionUnoSprite = Resources.Load<Sprite>($"Kits/{miEquipo.IdEquipo}local");
            if (equipacionUnoSprite != null)
                equipacionUno.style.backgroundImage = new StyleBackground(equipacionUnoSprite);

            Sprite equipacionDosSprite = Resources.Load<Sprite>($"Kits/{miEquipo.IdEquipo}visitante");
            if (equipacionDosSprite != null)
                equipacionDos.style.backgroundImage = new StyleBackground(equipacionDosSprite);

            // Equipo rival
            Sprite rivalSprite = Resources.Load<Sprite>($"EscudosEquipos/120x120/{miEquipo.Rival}");
            if (rivalSprite != null)
                escudoRival.style.backgroundImage = new StyleBackground(rivalSprite);

            // Máximo Goleador
            estadistica = EstadisticaJugadorData.MostrarJugadorConMasGoles(miEquipo.IdEquipo);
            jugador = JugadorData.MostrarDatosJugador(estadistica.IdJugador);

            Sprite maximoGoleadorSprite = Resources.Load<Sprite>($"Jugadores/{jugador.IdJugador}");
            if (maximoGoleadorSprite != null)
                fotoMaximoGoleador.style.backgroundImage = new StyleBackground(maximoGoleadorSprite);

            maximoGoleadorNombre.text = jugador.NombreCompleto;
            maximoGoleadorPosicion.text = jugador.Rol;
            maximoGoleadorCantidad.text = estadistica.Goles.ToString();

            // Máximo Asistente
            estadistica = EstadisticaJugadorData.MostrarJugadorConMasAsistencias(miEquipo.IdEquipo);
            jugador = JugadorData.MostrarDatosJugador(estadistica.IdJugador);

            Sprite maximoAsistenteSprite = Resources.Load<Sprite>($"Jugadores/{jugador.IdJugador}");
            if (maximoAsistenteSprite != null)
                fotoMaximoAsistente.style.backgroundImage = new StyleBackground(maximoAsistenteSprite);

            maximoAsistenteNombre.text = jugador.NombreCompleto;
            maximoAsistentePosicion.text = jugador.Rol;
            maximoAsistenteCantidad.text = estadistica.Asistencias.ToString();

            // Máximo MVP
            estadistica = EstadisticaJugadorData.MostrarJugadorConMasMvp(miEquipo.IdEquipo);
            jugador = JugadorData.MostrarDatosJugador(estadistica.IdJugador);

            Sprite maximoMVPSprite = Resources.Load<Sprite>($"Jugadores/{jugador.IdJugador}");
            if (maximoMVPSprite != null)
                fotoMaximoMVP.style.backgroundImage = new StyleBackground(maximoMVPSprite);

            maximoMVPNombre.text = jugador.NombreCompleto;
            maximoMVPPosicion.text = jugador.Rol;
            maximoMVPCantidad.text = estadistica.MVP.ToString();
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

            int numeroEstrellas = reputacion switch
            {
                100 => 5,
                >= 90 => 4,
                >= 70 => 3,
                >= 50 => 2,
                >= 25 => 1,
                _ => 0
            };

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
    }
}