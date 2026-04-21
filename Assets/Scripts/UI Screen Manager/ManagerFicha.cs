using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Globalization;

namespace TacticalEleven.Scripts
{
    public class ManagerFicha
    {
        private AudioClip clickSFX;
        private Equipo miEquipo;
        private Manager miManager;

        private VisualElement root, rankingContenedor, fotoManager, bandera, valoracionManagerContenedor;
        private Label confianzaDirectivaValue, confianzaFansValue, confianzaJugadoresValue,
                      confianzaDirectivaTexto, confianzaFansTexto, confianzaJugadoresTexto,
                      jugados, victorias, empates, derrotas, porcentaje,
                      nombreManager, fechaNacimientoManager, nacionalidadManager, rankingManager;

        public ManagerFicha(VisualElement rootInstance, Equipo equipo, Manager manager)
        {
            root = rootInstance;
            miEquipo = equipo;
            miManager = manager;
            clickSFX = Resources.Load<AudioClip>("Audios/click");

            // Referencias a objetos de la UI
            rankingContenedor = root.Q<VisualElement>("ranking-container");
            fotoManager = root.Q<VisualElement>("foto-manager");
            bandera = root.Q<VisualElement>("bandera");
            valoracionManagerContenedor = root.Q<VisualElement>("manager-valoracion-container");
            confianzaDirectivaValue = root.Q<Label>("lblConfianzaDirectiva-value");
            confianzaFansValue = root.Q<Label>("lblConfianzaFans-value");
            confianzaJugadoresValue = root.Q<Label>("lblConfianzaJugadores-value");
            confianzaDirectivaTexto = root.Q<Label>("lblConfianzaDirectiva-texto");
            confianzaFansTexto = root.Q<Label>("lblConfianzaFans-texto");
            confianzaJugadoresTexto = root.Q<Label>("lblConfianzaJugadores-texto");
            jugados = root.Q<Label>("lblJugados");
            victorias = root.Q<Label>("lblVictorias");
            empates = root.Q<Label>("lblEmpates");
            derrotas = root.Q<Label>("lblDerrotas");
            porcentaje = root.Q<Label>("lblPorcentaje");
            nombreManager = root.Q<Label>("manager-nombre");
            fechaNacimientoManager = root.Q<Label>("manager-fechaN");
            nacionalidadManager = root.Q<Label>("manager-nacionalidad");
            rankingManager = root.Q<Label>("manager-ranking");

            Manager oManager = ManagerData.MostrarManager();
            CargarFichaManager(oManager);
            CargarRelaciones(oManager);
            CargarRanking(oManager);
        }

        private void CargarRelaciones(Manager oManager)
        {
            AsignarConfianza(oManager.CDirectiva, confianzaDirectivaValue, confianzaDirectivaTexto, new[]
            {
                "La directiva está completamente impresionada con tu gestión. Tu capacidad para dirigir al equipo y tomar decisiones clave ha fortalecido su confianza y apoyo de manera notable.",
                "La directiva está muy contenta con tu rendimiento. Aunque aún hay pequeños aspectos a mejorar, confían en que seguirás guiando al equipo hacia el éxito con un liderazgo firme.",
                "La relación con la directiva es estable, pero existen ciertas preocupaciones. A veces se cuestionan algunas decisiones, pero hay una disposición a seguir adelante y ver cómo evolucionan las cosas.",
                "La directiva ha comenzado a perder confianza en tu liderazgo. Si no mejoras los resultados pronto, podrían reconsiderar tu posición, lo que pondría en riesgo tu continuidad al mando del equipo.",
                "La relación con la directiva es extremadamente tensa. La falta de resultados y las decisiones cuestionadas han erosionado casi por completo su confianza, y tu futuro en el equipo está seriamente comprometido."
            });

            AsignarConfianza(oManager.CFans, confianzaFansValue, confianzaFansTexto, new[]
            {
                "Los fans están absolutamente encantados con tu gestión. Cada victoria, cada decisión que tomas es celebrada con entusiasmo. Tienes su total apoyo, y la conexión con ellos es más fuerte que nunca.",
                "Los fans están muy satisfechos con tu rendimiento. Hay una gran admiración por lo que has hecho hasta ahora, aunque algunos aún esperan un poco más de consistencia. En general, te respaldan completamente.",
                "La relación con los fans es moderada. Muchos aprecian tus esfuerzos, pero otros empiezan a mostrar algo de desconfianza debido a los altibajos en los resultados. Aún hay tiempo para ganar más apoyo si logras mejorar.",
                "La relación con los fans se ha deteriorado en las últimas semanas. Están empezando a frustrarse con la falta de victorias y algunos de tus movimientos. La presión está aumentando y necesitarás trabajar más para recuperar su apoyo.",
                "Los fans están completamente decepcionados con tu gestión. Los resultados no son los esperados y tu liderazgo está siendo severamente criticado. La relación está en su punto más bajo y será un desafío revertir esta situación."
            });

            AsignarConfianza(oManager.CJugadores, confianzaJugadoresValue, confianzaJugadoresTexto, new[]
            {
                "Los jugadores están totalmente a tu favor. La moral del equipo está por las nubes y todos confían plenamente en tus decisiones. El vestuario está unido y motivado, listos para alcanzar grandes logros bajo tu dirección.",
                "La relación con los jugadores es muy positiva. Confían en tus decisiones y saben que los llevas por el buen camino. Aunque algunos todavía tienen dudas menores, el equipo en general está muy comprometido con tus objetivos.",
                "La confianza de los jugadores es moderada. La mayoría sigue tu liderazgo, pero algunos empiezan a mostrar señales de desconfianza debido a ciertas decisiones tácticas. El equipo sigue unido, pero hay aspectos que necesitan mejorar para evitar que crezca la incertidumbre.",
                "La moral del equipo está baja. Los jugadores están empezando a dudar de tus métodos y algunos cuestionan tus decisiones. Si no logras mejorar los resultados y darles más razones para confiar en ti, la situación podría empeorar.",
                "La relación con los jugadores es crítica. La confianza en tu liderazgo es prácticamente nula y muchos ya están perdiendo la fe en tus decisiones. El vestuario está dividido y, sin cambios inmediatos, podrías enfrentar una rebelión dentro del equipo."
            });
        }

        private void AsignarConfianza(int valor, Label valorLabel, Label textoLabel, string[] textos)
        {
            valorLabel.text = valor.ToString();
            valorLabel.style.color = DeterminarColor(valor);

            if (valor >= 90) textoLabel.text = textos[0];
            else if (valor >= 70) textoLabel.text = textos[1];
            else if (valor >= 50) textoLabel.text = textos[2];
            else if (valor >= 30) textoLabel.text = textos[3];
            else textoLabel.text = textos[4];
        }


        private void CargarFichaManager(Manager oManager)
        {
            // Obtener el nombre
            nombreManager.text = $"{oManager.Nombre} {oManager.Apellido}";

            // Obtener la nacionalidad.
            nacionalidadManager.text = oManager.Nacionalidad;
            var banderaSprite = Resources.Load<Sprite>($"Banderas/{Constants.ObtenerCodigoBanderas(oManager.Nacionalidad)}");
            if (banderaSprite != null)
                bandera.style.backgroundImage = new StyleBackground(banderaSprite);

            // Obtener la fecha de nacimiento y Edad
            CalcularEdad();

            // Obtener Ranking
            string miNombre = ManagerData.MostrarManager().Nombre + " " + ManagerData.MostrarManager().Apellido;
            int miPosicion = -1;
            List<Entrenador> listado = EntrenadorData.ObtenerRankingEntrenadores();
            foreach (var entrenador in listado)
            {
                if (entrenador.NombreCompleto == miNombre)
                {
                    miPosicion = entrenador.Posicion;
                    break; // Salimos del bucle una vez encontrado
                }
            }
            rankingManager.text = "Ranking Mánager: " + miPosicion + "º";

            // Obtener Valoración de Mánager
            MostrarEstrellas(oManager.Reputacion, valoracionManagerContenedor);

            // Obtener Resumen de partidos
            jugados.text = oManager.PartidosJugados.ToString();
            victorias.text = oManager.PartidosGanados.ToString();
            empates.text = oManager.PartidosEmpatados.ToString();
            derrotas.text = oManager.PartidosPerdidos.ToString();
            if (oManager.PartidosJugados != 0)
            {
                porcentaje.text = ((oManager.PartidosGanados * 100) / oManager.PartidosJugados).ToString() + " %";
            }
            else
            {
                porcentaje.text = "0 %";
            }
        }

        private bool CalcularEdad()
        {
            Fecha fechaHoyDB = FechaData.ObtenerFechaHoy();

            if (fechaHoyDB == null || string.IsNullOrEmpty(fechaHoyDB.Hoy))
            {
                Debug.LogError("No se pudo obtener la fecha de hoy desde la base de datos.");
                return false;
            }

            DateTime fechaHoy;

            // Intentamos parsear con formatos comunes que usas
            string[] formatos = { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" };

            if (!DateTime.TryParseExact(fechaHoyDB.Hoy, formatos,
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out fechaHoy))
            {
                Debug.LogError("No se pudo convertir la fecha de hoy: " + fechaHoyDB.Hoy);
                return false;
            }

            string fechaNacimientoStr = ManagerData.MostrarManager().FechaNacimiento;

            if (DateTime.TryParseExact(fechaNacimientoStr, formatos,
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime fechaNacimiento))
            {
                int edad = fechaHoy.Year - fechaNacimiento.Year;

                if (fechaNacimiento > fechaHoy.AddYears(-edad))
                    edad--;

                fechaNacimientoManager.text =
                    fechaNacimiento.ToString("dd/MM/yyyy") + $" ({edad} años)";
            }
            else
            {
                Debug.LogError("Fecha de nacimiento inválida: " + fechaNacimientoStr);
            }

            return true;
        }

        private void CargarRanking(Manager oManager)
        {
            List<Entrenador> entrenadores = EntrenadorData.ObtenerRankingEntrenadores();
            string miNombre = $"{oManager.Nombre} {oManager.Apellido}";

            rankingContenedor.Clear();

            // PORCENTAJES DE COLUMNAS
            float col1 = 10f;    // POS
            float col2 = 35f;    // ENTRENADOR
            float col3 = 35f;    // EQUIPO
            float col4 = 16f;    // PUNTOS

            // FILAS
            int index = 0;
            foreach (var item in entrenadores)
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
                    ? new Color32(255, 255, 255, 255)     // blanco
                    : new Color32(242, 242, 242, 255);    // gris suave

                // Aplicar color final
                fila.style.backgroundColor = new StyleColor(filaColor);

                if (miNombre.Equals(item.NombreCompleto))
                {
                    // 1) POS
                    fila.Add(CreateCell(item.Posicion.ToString(), col1, Color.black, TextAnchor.MiddleCenter, true));

                    // 2) ENTRENADOR
                    fila.Add(CreateCell(item.NombreCompleto, col2, Color.black, TextAnchor.MiddleLeft, true));

                    // 3) EQUIPO
                    fila.Add(CreateCell(item.NombreEquipo, col3, Color.black, TextAnchor.MiddleLeft, true));

                    // 4) PUNTOS
                    fila.Add(CreateCell(item.Puntos.ToString(), col4, Color.black, TextAnchor.MiddleLeft, true));
                }
                else
                {
                    // 1) POS
                    fila.Add(CreateCell(item.Posicion.ToString(), col1, Color.black, TextAnchor.MiddleCenter, false));

                    // 2) ENTRENADOR
                    fila.Add(CreateCell(item.NombreCompleto, col2, Color.black, TextAnchor.MiddleLeft, false));

                    // 3) EQUIPO
                    fila.Add(CreateCell(item.NombreEquipo, col3, Color.black, TextAnchor.MiddleLeft, false));

                    // 4) PUNTOS
                    fila.Add(CreateCell(item.Puntos.ToString(), col4, Color.black, TextAnchor.MiddleLeft, false));
                }

                rankingContenedor.Add(fila);
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

            label.style.fontSize = 16;

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

        private Color DeterminarColor(int valoracion)
        {
            if (valoracion >= 80)
                return new Color32(0x1E, 0x72, 0x3C, 0xFF);   // Verde 
            else if (valoracion >= 50)
                return new Color32(0xC6, 0x76, 0x17, 0xFF);   // Naranja 
            else
                return new Color32(0xA3, 0x1E, 0x1E, 0xFF);   // Rojo
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