using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace TacticalEleven.Scripts
{
    public class PortadaManager
    {
        // UI Elements
        private VisualElement root;
        private VisualElement lugarUltimoPartido, lugarProximoPartido, ultimoPartidoEscudoLocal, ultimoPartidoEscudoVisitante,
                              proximoPartidoEscudoLocal, proximoPartidoEscudoVisitante, logoCompeticion, escudoEquipo, objetivoResultado,
                              clasificacionContainer, maximoGoleador, maximoAsistente, maximoTA, maximoTR, maximoMVP;
        private Label lblUltimoPartido, lblProximoPartido, lblUltimoPartidoEquipoLocal, lblUltimoPartidoEquipoVisitante, lblUltimoPartidoGolesLocal, lblUltimoPartidoGolesVisitante,
                      lblProximoPartidoEquipos, lblProximoPartidoEstadio, confianzaDirectiva, confianzaFans, confianzaJugadores,
                      lblObjetivoTemporada, lblnombreCompeticion, lblRachaValue, lblMediaEquipoValue, lblEstadio, lblAforoEstadio,
                      golesValue, asistenciasValue, taValue, trValue, mvpValue, maximoGoleadorNombre, maximoGoleadorPJ, maximoAsistenteNombre,
                      maximoAsistentePJ, maximoTANombre, maximoTAPJ, maximoTRNombre, maximoTRPJ, maximoMVPNombre, maximoMVPPJ;
        private Label fechaProximoPartido;

        Fecha f = FechaData.ObtenerFechaHoy();

        public PortadaManager(VisualElement rootInstance, Equipo miEquipo, Manager miManager)
        {
            root = rootInstance;

            // Referencias a elementos internos
            lblUltimoPartido = root.Q<Label>("lastGame-label");
            lugarUltimoPartido = root.Q<VisualElement>("lugar-ultimoPartido-icon");
            ultimoPartidoEscudoLocal = root.Q<VisualElement>("ultimoPartido-escudo-local");
            ultimoPartidoEscudoVisitante = root.Q<VisualElement>("ultimoPartido-escudo-visitante");
            lblUltimoPartidoEquipoLocal = root.Q<Label>("ultimoPartido-nombreEquipo-local");
            lblUltimoPartidoEquipoVisitante = root.Q<Label>("ultimoPartido-nombreEquipo-visitante");
            lblUltimoPartidoGolesLocal = root.Q<Label>("ultimoPartido-goles-local");
            lblUltimoPartidoGolesVisitante = root.Q<Label>("ultimoPartido-goles-visitante");

            lblProximoPartido = root.Q<Label>("nextGame-label");
            lugarProximoPartido = root.Q<VisualElement>("lugar-proximoPartido-icon");
            proximoPartidoEscudoLocal = root.Q<VisualElement>("proximoPartido-escudo-local");
            proximoPartidoEscudoVisitante = root.Q<VisualElement>("proximoPartido-escudo-visitante");
            fechaProximoPartido = root.Q<Label>("fecha-proximoPartido");
            lblProximoPartidoEquipos = root.Q<Label>("proximoPartido-equipos");
            lblProximoPartidoEstadio = root.Q<Label>("proximoPartido-estadio");

            clasificacionContainer = root.Q<VisualElement>("clasificacionTabla-container");

            logoCompeticion = root.Q<VisualElement>("logo-competicion");
            escudoEquipo = root.Q<VisualElement>("escudo-equipo-media");
            objetivoResultado = root.Q<VisualElement>("objetivo-resultado");
            lblObjetivoTemporada = root.Q<Label>("objetivo-temporada");
            lblnombreCompeticion = root.Q<Label>("nombre-competicion");
            lblRachaValue = root.Q<Label>("racha-value");
            lblMediaEquipoValue = root.Q<Label>("mediaEquipo-value");
            lblEstadio = root.Q<Label>("nombre-estadio");
            lblAforoEstadio = root.Q<Label>("aforo-estadio");

            maximoGoleador = root.Q<VisualElement>("maximo-goleador");
            maximoAsistente = root.Q<VisualElement>("maximo-asistente");
            maximoTA = root.Q<VisualElement>("maximo-ta");
            maximoTR = root.Q<VisualElement>("maximo-tr");
            maximoMVP = root.Q<VisualElement>("maximo-mvp");
            golesValue = root.Q<Label>("goles-value");
            asistenciasValue = root.Q<Label>("asistencias-value");
            taValue = root.Q<Label>("ta-value");
            trValue = root.Q<Label>("tr-value");
            mvpValue = root.Q<Label>("mvp-value");
            maximoGoleadorNombre = root.Q<Label>("maximo-goleador-nombre");
            maximoGoleadorPJ = root.Q<Label>("maximo-goleador-pj");
            maximoAsistenteNombre = root.Q<Label>("maximo-asistente-nombre");
            maximoAsistentePJ = root.Q<Label>("maximo-asistente-pj");
            maximoTANombre = root.Q<Label>("maximo-ta-nombre");
            maximoTAPJ = root.Q<Label>("maximo-ta-pj");
            maximoTRNombre = root.Q<Label>("maximo-tr-nombre");
            maximoTRPJ = root.Q<Label>("maximo-tr-pj");
            maximoMVPNombre = root.Q<Label>("maximo-mvp-nombre");
            maximoMVPPJ = root.Q<Label>("maximo-mvp-pj");

            confianzaDirectiva = root.Q<Label>("confianza-directiva");
            confianzaFans = root.Q<Label>("confianza-fans");
            confianzaJugadores = root.Q<Label>("confianza-jugadores");

            // Inicializar datos de la pantalla Portada
            MostrarUltimoPartido(miEquipo);
            MostrarProximoPartido(miEquipo);
            CargarClasificacion(miEquipo);
            CargarEstadisticasJugadores(miEquipo);
            CargarEstadisticasEquipo(miEquipo);
            CargarRelaciones();
        }

        private void MostrarUltimoPartido(Equipo miEquipo)
        {
            Partido ultimoPartido = PartidoData.ObtenerUltimoPartido(miEquipo.IdEquipo, f.ToDateTime());

            if (ultimoPartido != null)
            {
                // Comprobar si soy local o visitante
                if (ultimoPartido.IdEquipoLocal == miEquipo.IdEquipo)
                {
                    lblUltimoPartido.text = $"ÚLTIMO PARTIDO ({CompeticionData.MostrarNombreCompeticion(ultimoPartido.IdCompeticion)})";
                    var lugarUltimoPartidoSprite = Resources.Load<Sprite>($"Icons/casa_icon");
                    if (lugarUltimoPartidoSprite != null)
                        lugarUltimoPartido.style.backgroundImage = new StyleBackground(lugarUltimoPartidoSprite);
                }
                else
                {
                    lblUltimoPartido.text = $"ÚLTIMO PARTIDO ({CompeticionData.MostrarNombreCompeticion(ultimoPartido.IdCompeticion)})";
                    var lugarUltimoPartidoSprite = Resources.Load<Sprite>($"Icons/avion-icon");
                    if (lugarUltimoPartidoSprite != null)
                        lugarUltimoPartido.style.backgroundImage = new StyleBackground(lugarUltimoPartidoSprite);
                }

                // Escudo equipo local
                var escudoLocal = Resources.Load<Sprite>($"EscudosEquipos/32x32/{ultimoPartido.IdEquipoLocal}");
                if (escudoLocal != null)
                    ultimoPartidoEscudoLocal.style.backgroundImage = new StyleBackground(escudoLocal);

                // Escudo equipo visitante
                var escudoVisitante = Resources.Load<Sprite>($"EscudosEquipos/32x32/{ultimoPartido.IdEquipoVisitante}");
                if (escudoVisitante != null)
                    ultimoPartidoEscudoVisitante.style.backgroundImage = new StyleBackground(escudoVisitante);

                // Nombres de equipo y goles
                lblUltimoPartidoEquipoLocal.text = EquipoData.ObtenerDetallesEquipo(ultimoPartido.IdEquipoLocal).Nombre;
                lblUltimoPartidoEquipoVisitante.text = EquipoData.ObtenerDetallesEquipo(ultimoPartido.IdEquipoVisitante).Nombre;
                lblUltimoPartidoGolesLocal.text = ultimoPartido.GolesLocal.ToString();
                lblUltimoPartidoGolesVisitante.text = ultimoPartido.GolesVisitante.ToString();
            }
        }

        private void MostrarProximoPartido(Equipo miEquipo)
        {
            Partido proximoPartido = PartidoData.ObtenerProximoPartido(miEquipo.IdEquipo, f.ToDateTime());

            // Comprobar si soy local o visitante
            if (proximoPartido.IdEquipoLocal == miEquipo.IdEquipo)
            {
                lblProximoPartido.text = $"PRÓXIMO PARTIDO ({CompeticionData.MostrarNombreCompeticion(proximoPartido.IdCompeticion)})";
                var lugarProximoPartidoSprite = Resources.Load<Sprite>($"Icons/casa_icon");
                if (lugarProximoPartidoSprite != null)
                    lugarProximoPartido.style.backgroundImage = new StyleBackground(lugarProximoPartidoSprite);
            }
            else
            {
                lblProximoPartido.text = $"PRÓXIMO PARTIDO ({CompeticionData.MostrarNombreCompeticion(proximoPartido.IdCompeticion)})";
                var lugarProximoPartidoSprite = Resources.Load<Sprite>($"Icons/avion-icon");
                if (lugarProximoPartidoSprite != null)
                    lugarProximoPartido.style.backgroundImage = new StyleBackground(lugarProximoPartidoSprite);
            }

            // Escudo equipo local
            var escudoLocal = Resources.Load<Sprite>($"EscudosEquipos/64x64/{proximoPartido.IdEquipoLocal}");
            if (escudoLocal != null)
                proximoPartidoEscudoLocal.style.backgroundImage = new StyleBackground(escudoLocal);

            // Escudo equipo visitante
            var escudoVisitante = Resources.Load<Sprite>($"EscudosEquipos/64x64/{proximoPartido.IdEquipoVisitante}");
            if (escudoVisitante != null)
                proximoPartidoEscudoVisitante.style.backgroundImage = new StyleBackground(escudoVisitante);

            // Nombre Equipos
            lblProximoPartidoEquipos.text = (EquipoData.ObtenerDetallesEquipo(proximoPartido?.IdEquipoLocal ?? 0)?.Nombre?.ToUpper() ?? "SIN EQUIPO") + " - " +
                                            (EquipoData.ObtenerDetallesEquipo(proximoPartido?.IdEquipoVisitante ?? 0)?.Nombre?.ToUpper() ?? "SIN EQUIPO");

            // Nombre Eestadio
            lblProximoPartidoEstadio.text = EquipoData.ObtenerDetallesEquipo(proximoPartido?.IdEquipoLocal ?? 0)?.Estadio ?? "Sin estadio";

            // Fecha del próximo partido
            fechaProximoPartido.text = proximoPartido != null && proximoPartido.FechaPartido != DateTime.MinValue
                                            ? proximoPartido.FechaPartido.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"))
                                            : "Fecha no disponible"; ;
        }

        private void CargarClasificacion(Equipo miEquipo)
        {
            clasificacionContainer.Clear();

            var cl = ClasificacionData.MostrarClasificacion(miEquipo.IdCompeticion);

            // PORCENTAJES DE COLUMNAS
            float col1 = 4f;    // columna vacía / indicador clasificación
            float col2 = 12f;   // POS
            float col3 = 12f;   // ESCUDO
            float col4 = 60f;   // EQUIPO
            float col5 = 12f;   // PTS

            // COLORES
            Color headerBg = new Color32(56, 78, 63, 255);
            Color headerText = Color.white;

            // CABECERA
            var header = new VisualElement();
            header.style.flexDirection = FlexDirection.Row;
            header.style.backgroundColor = new StyleColor(headerBg);
            header.style.minHeight = 30;
            header.style.maxHeight = 30;
            header.style.width = Length.Percent(100);
            header.style.alignItems = Align.Center;
            header.style.justifyContent = Justify.FlexStart;
            header.style.unityFontStyleAndWeight = FontStyle.Bold;

            // Columna vacía de cabecera
            var headerEmpty = new VisualElement();
            headerEmpty.style.width = Length.Percent(col1);
            headerEmpty.style.height = 30;
            header.Add(headerEmpty);

            header.Add(CreateCell("POS", col2, headerText, TextAnchor.MiddleCenter, true));

            // Columna ESCUDO de cabecera
            var headerEscudo = new VisualElement();
            headerEscudo.style.width = Length.Percent(col3);
            headerEscudo.style.height = 30;
            header.Add(headerEscudo);

            header.Add(CreateCell("EQUIPO", col4, headerText, TextAnchor.MiddleLeft, true));
            header.Add(CreateCell("PTS", col5, headerText, TextAnchor.MiddleCenter, true));

            clasificacionContainer.Add(header);

            // FILAS
            int index = 0;
            foreach (var item in cl)
            {
                var fila = new VisualElement();
                fila.style.flexDirection = FlexDirection.Row;
                fila.style.width = Length.Percent(100);
                fila.style.minHeight = 29;
                fila.style.maxHeight = 30;
                fila.style.alignItems = Align.Center;

                // Color de fondo de fila alternante (para todas las columnas)
                // Color estándar alterno
                Color filaColor = (index % 2 == 0)
                    ? new Color32(255, 255, 255, 255)     // blanco
                    : new Color32(242, 242, 242, 255);    // gris suave

                // Si esta fila es de mi equipo → color verde
                if (item.IdEquipo == miEquipo.IdEquipo)
                {
                    filaColor = new Color32(197, 232, 202, 255); // #C5E8CA verde
                }

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                // 1) Primera columna coloreada según posición (encima del fondo alternante)
                var colEmpty = new VisualElement();
                colEmpty.style.width = Length.Percent(col1);
                colEmpty.style.height = 30;
                colEmpty.style.display = DisplayStyle.Flex;

                if (miEquipo.IdCompeticion == 1)
                {
                    if (index >= 0 && index <= 3)           // filas 1 a 4 -> SteelBlue
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(70, 130, 180, 255));
                    else if (index >= 4 && index <= 5)      // filas 5 y 6 -> DarkGreen
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(0, 100, 0, 255));
                    else if (index >= 17 && index <= 19)    // filas 18 a 20 -> DarkRed
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(139, 0, 0, 255));
                }
                else
                {
                    if (index >= 0 && index <= 2)               // filas 1 a 3 -> SteelBlue
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(70, 130, 180, 255));
                    else if (index >= 17 && index <= 19)    // filas 18 a 20 -> DarkRed
                        colEmpty.style.backgroundColor = new StyleColor(new Color32(139, 0, 0, 255));
                }

                fila.Add(colEmpty);

                // 2) Posición
                fila.Add(CreateCell(item.Posicion.ToString(), col2, Color.black, TextAnchor.MiddleCenter, false));

                // 3) Escudo
                var escudo = new VisualElement();
                escudo.style.width = Length.Percent(col3);
                escudo.style.height = 32;
                var sprite = Resources.Load<Sprite>($"EscudosEquipos/32x32/{item.IdEquipo}");
                if (sprite != null)
                {
                    escudo.style.backgroundImage = new StyleBackground(sprite);
                    escudo.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                }
                fila.Add(escudo);

                // 4) Nombre del equipo
                fila.Add(CreateCell(item.NombreEquipo, col4, Color.black, TextAnchor.MiddleLeft, false));

                // 5) Puntos
                fila.Add(CreateCell(item.Puntos.ToString(), col5, Color.black, TextAnchor.MiddleCenter, false));

                clasificacionContainer.Add(fila);
                index++;
            }
        }

        private VisualElement CreateCell(string texto, float anchoPercent, Color color, TextAnchor alineacion, bool esHeader)
        {
            var label = new Label(texto);

            label.style.width = Length.Percent(anchoPercent);
            label.style.color = color;
            label.style.unityTextAlign = alineacion;

            // Fuente Poppins-Bold
            var fontPath = esHeader
                ? "Fonts/Poppins-SemiBold SDF"
                : "Fonts/Poppins-Regular SDF";

            var fontAsset = Resources.Load<UnityEngine.TextCore.Text.FontAsset>(fontPath);
            label.style.unityFontDefinition = new StyleFontDefinition(fontAsset);

            // Flex para que no se comprima ni expanda
            label.style.display = DisplayStyle.Flex;
            label.style.flexDirection = FlexDirection.Row;
            label.style.alignItems = Align.Center;
            label.style.flexShrink = 0;
            label.style.flexGrow = 0;

            // Padding según alineación
            label.style.paddingLeft = (alineacion == TextAnchor.MiddleLeft) ? 6 : 0;
            label.style.paddingRight = (alineacion == TextAnchor.MiddleLeft) ? 0 : 6;

            return label;
        }

        private void CargarEstadisticasJugadores(Equipo miEquipo)
        {
            // Goles
            Estadistica estadisticaGoleador = EstadisticaJugadorData.MostrarJugadorConMasGoles(miEquipo.IdEquipo);
            Jugador oMaximoGoleador = JugadorData.MostrarDatosJugador(estadisticaGoleador.IdJugador);
            var maximoGoleadorSprite = Resources.Load<Sprite>($"Jugadores/{oMaximoGoleador.IdJugador}");
            if (maximoGoleadorSprite != null)
                maximoGoleador.style.backgroundImage = new StyleBackground(maximoGoleadorSprite);
            maximoGoleadorNombre.text = oMaximoGoleador.NombreCompleto;
            maximoGoleadorPJ.text = $"Partidos jugados: {estadisticaGoleador.PartidosJugados.ToString()}";
            golesValue.text = estadisticaGoleador.Goles.ToString();

            // Asistencias
            Estadistica estadisticaAsistencias = EstadisticaJugadorData.MostrarJugadorConMasAsistencias(miEquipo.IdEquipo);
            Jugador oMaximoAsistente = JugadorData.MostrarDatosJugador(estadisticaAsistencias.IdJugador);
            var maximoAsistenteSprite = Resources.Load<Sprite>($"Jugadores/{oMaximoAsistente.IdJugador}");
            if (maximoAsistenteSprite != null)
                maximoAsistente.style.backgroundImage = new StyleBackground(maximoAsistenteSprite);
            maximoAsistenteNombre.text = oMaximoAsistente.NombreCompleto;
            maximoAsistentePJ.text = $"Partidos jugados: {estadisticaAsistencias.PartidosJugados.ToString()}";
            asistenciasValue.text = estadisticaAsistencias.Asistencias.ToString();

            // Tarjetas Amarillas
            Estadistica estadisticaTA = EstadisticaJugadorData.MostrarJugadorConMasTarjetasAmarillas(miEquipo.IdEquipo);
            Jugador oMaximoTA = JugadorData.MostrarDatosJugador(estadisticaTA.IdJugador);
            var maximoTASprite = Resources.Load<Sprite>($"Jugadores/{oMaximoTA.IdJugador}");
            if (maximoTASprite != null)
                maximoTA.style.backgroundImage = new StyleBackground(maximoTASprite);
            maximoTANombre.text = oMaximoTA.NombreCompleto;
            maximoTAPJ.text = $"Partidos jugados: {estadisticaTA.PartidosJugados.ToString()}";
            taValue.text = estadisticaTA.TarjetasAmarillas.ToString();

            // Tarjetas Rojas
            Estadistica estadisticaTR = EstadisticaJugadorData.MostrarJugadorConMasTarjetasRojas(miEquipo.IdEquipo);
            Jugador oMaximoTR = JugadorData.MostrarDatosJugador(estadisticaTR.IdJugador);
            var maximoTRSprite = Resources.Load<Sprite>($"Jugadores/{oMaximoTR.IdJugador}");
            if (maximoTRSprite != null)
                maximoTR.style.backgroundImage = new StyleBackground(maximoTRSprite);
            maximoTRNombre.text = oMaximoTR.NombreCompleto;
            maximoTRPJ.text = $"Partidos jugados: {estadisticaTR.PartidosJugados.ToString()}";
            trValue.text = estadisticaTR.TarjetasRojas.ToString();

            // MVP
            Estadistica estadisticaMVP = EstadisticaJugadorData.MostrarJugadorConMasMvp(miEquipo.IdEquipo);
            Jugador oMaximoMVP = JugadorData.MostrarDatosJugador(estadisticaMVP.IdJugador);
            var maximoMVPSprite = Resources.Load<Sprite>($"Jugadores/{oMaximoMVP.IdJugador}");
            if (maximoMVPSprite != null)
                maximoMVP.style.backgroundImage = new StyleBackground(maximoMVPSprite);
            maximoMVPNombre.text = oMaximoMVP.NombreCompleto;
            maximoMVPPJ.text = $"Partidos jugados: {estadisticaMVP.PartidosJugados.ToString()}";
            mvpValue.text = estadisticaMVP.MVP.ToString();
        }

        private void CargarEstadisticasEquipo(Equipo miEquipo)
        {
            var logoCompeticionSprite = Resources.Load<Sprite>($"LogosCompeticiones/80x80/{miEquipo.IdCompeticion}");
            if (logoCompeticionSprite != null)
                logoCompeticion.style.backgroundImage = new StyleBackground(logoCompeticionSprite);

            var escudoEquipoSprite = Resources.Load<Sprite>($"EscudosEquipos/64x64/{miEquipo.IdEquipo}");
            if (escudoEquipoSprite != null)
                escudoEquipo.style.backgroundImage = new StyleBackground(escudoEquipoSprite);

            //Objetivo
            // 1) Obtener clasificación y posición
            var cl = ClasificacionData.MostrarClasificacion(miEquipo.IdCompeticion);
            int posicion = cl.FindIndex(e => e.IdEquipo == miEquipo.IdEquipo) + 1;

            // 2) Diccionario de objetivos → posición mínima para cumplirlos
            var limites = new Dictionary<string, int>();

            if (miEquipo.IdCompeticion == 1)
            {
                limites["Campeón"] = 4;
                limites["Zona Tranquila"] = 14;
                limites["Descenso"] = 16;
            }
            else
            {
                limites["Ascenso"] = 4;
                limites["Zona Tranquila"] = 14;
                limites["Descenso"] = 16;
            }

            // 3) Comprobar si cumple objetivo
            string objetivo = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Objetivo;

            Sprite verificadoSprite = Resources.Load<Sprite>("Icons/verificado");
            Sprite rechazadoSprite = Resources.Load<Sprite>("Icons/rechazado");

            bool cumpleObjetivo = limites.ContainsKey(objetivo) && posicion <= limites[objetivo];

            // 4) Aplicar icono
            objetivoResultado.style.backgroundImage =
                new StyleBackground(cumpleObjetivo ? verificadoSprite : rechazadoSprite);


            lblObjetivoTemporada.text = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Objetivo;
            lblnombreCompeticion.text = CompeticionData.MostrarNombreCompeticion(EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).IdCompeticion);

            int racha = ClasificacionData.MostrarClasificacionPorEquipo(miEquipo.IdEquipo, miEquipo.IdCompeticion).Racha;
            lblRachaValue.text = racha.ToString();

            double mediaEquipo = JugadorData.ObtenerMediaEquipo(miEquipo.IdEquipo);
            lblMediaEquipoValue.text = Math.Round(mediaEquipo).ToString();

            if (mediaEquipo > 85)
            {
                lblMediaEquipoValue.style.color = (Color)new Color32(0x1E, 0x72, 0x3C, 0xFF);  // verde oscuro
            }
            else if (mediaEquipo >= 75)
            {
                lblMediaEquipoValue.style.color = (Color)new Color32(0x3A, 0x8E, 0x42, 0xFF);  // verde medio
            }
            else if (mediaEquipo >= 65)
            {
                lblMediaEquipoValue.style.color = (Color)new Color32(0xC6, 0x76, 0x17, 0xFF);  // naranja oscuro
            }
            else
            {
                lblMediaEquipoValue.style.color = (Color)new Color32(0xA3, 0x1E, 0x1E, 0xFF);  // rojo oscuro
            }

            lblEstadio.text = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Estadio;
            lblAforoEstadio.text = EquipoData.ObtenerDetallesEquipo(miEquipo.IdEquipo).Aforo.ToString("N0") + " espectadores";
        }

        private void CargarRelaciones()
        {
            int cDirectiva = ManagerData.MostrarManager().CDirectiva;
            int cFans = ManagerData.MostrarManager().CFans;
            int cJugadores = ManagerData.MostrarManager().CJugadores;

            // Asignar texto y color
            confianzaDirectiva.text = cDirectiva.ToString();
            confianzaDirectiva.style.color = DeterminarColor(cDirectiva);

            confianzaFans.text = cFans.ToString();
            confianzaFans.style.color = DeterminarColor(cFans);

            confianzaJugadores.text = cJugadores.ToString();
            confianzaJugadores.style.color = DeterminarColor(cJugadores);
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