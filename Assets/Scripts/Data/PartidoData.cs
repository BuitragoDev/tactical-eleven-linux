using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace TacticalEleven.Scripts
{
    public static class PartidoData
    {
        private static string GetDBPath()
        {
            string path = DatabaseManager.GetActiveDatabasePath();
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("No se ha establecido una base de datos activa en DatabaseManager.");
            }
            return path;
        }

        // --------------------------------------------------------------------------------------------- MÉTODO QUE CREA UN PARTIDO DE LIGA
        public static int CrearPartido(int local, int visitante, string fecha, int competicion, int jornada)
        {
            var dbPath = GetDBPath();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return -1;
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();
                comando.CommandText = @"INSERT INTO partidos (fecha, id_equipo_local, id_equipo_visitante, id_competicion, jornada) 
                                        VALUES (@Fecha, @IdEquipoLocal, @IdEquipoVisitante, @Competicion, @Jornada)";
                comando.Parameters.AddWithValue("@Fecha", fecha);
                comando.Parameters.AddWithValue("@IdEquipoLocal", local);
                comando.Parameters.AddWithValue("@IdEquipoVisitante", visitante);
                comando.Parameters.AddWithValue("@Competicion", competicion);
                comando.Parameters.AddWithValue("@Jornada", jornada);

                comando.ExecuteNonQuery();

                // Obtener el ID recién insertado
                comando.CommandText = "SELECT last_insert_rowid();";
                long idPartido = (long)comando.ExecuteScalar();
                return (int)idPartido;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
                return -1;
            }
        }

        // --------------------------------------------------------------------------------------------- MÉTODO QUE CREA UN PARTIDO DE COPA
        public static int CrearPartidoCopa(int local, int visitante, string fecha, int competicion, int ronda, int partidoVuelta)
        {
            var dbPath = GetDBPath();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return -1;
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();
                comando.CommandText = @"INSERT INTO partidos_CopaNacional (partido_vuelta, fecha, id_equipo_local, id_equipo_visitante, id_competicion, id_ronda) 
                                        VALUES (@PartidoVuelta, @Fecha, @IdEquipoLocal, @IdEquipoVisitante, @Competicion, @Ronda)";
                comando.Parameters.AddWithValue("@PartidoVuelta", partidoVuelta);
                comando.Parameters.AddWithValue("@Fecha", fecha);
                comando.Parameters.AddWithValue("@IdEquipoLocal", local);
                comando.Parameters.AddWithValue("@IdEquipoVisitante", visitante);
                comando.Parameters.AddWithValue("@Competicion", competicion);
                comando.Parameters.AddWithValue("@Ronda", ronda);

                comando.ExecuteNonQuery();

                // Obtener el ID recién insertado
                comando.CommandText = "SELECT last_insert_rowid();";
                long idPartido = (long)comando.ExecuteScalar();
                return (int)idPartido;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
                return -1;
            }
        }

        // ---------------------------------------------------------------------------------- MÉTODO QUE CREA UN PARTIDO DE COPA EUROPA 1
        public static int CrearPartidoCopaEuropa(int local, int visitante, string fecha, int competicion, int jornada, int ronda, int partidoVuelta)
        {
            var dbPath = GetDBPath();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return -1;
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();
                comando.CommandText = @"INSERT INTO partidos_copaEuropa1 (partido_vuelta, fecha, id_equipo_local, id_equipo_visitante, id_competicion, jornada, id_ronda, estado) 
                                        VALUES (@PartidoVuelta, @Fecha, @IdEquipoLocal, @IdEquipoVisitante, @Competicion, @Jornada, @Ronda, 'Pendiente')";
                comando.Parameters.AddWithValue("@PartidoVuelta", partidoVuelta);
                comando.Parameters.AddWithValue("@Fecha", fecha);
                comando.Parameters.AddWithValue("@IdEquipoLocal", local);
                comando.Parameters.AddWithValue("@IdEquipoVisitante", visitante);
                comando.Parameters.AddWithValue("@Competicion", competicion);
                comando.Parameters.AddWithValue("@Jornada", jornada);
                comando.Parameters.AddWithValue("@Ronda", ronda);

                comando.ExecuteNonQuery();

                // Obtener el ID recién insertado
                comando.CommandText = "SELECT last_insert_rowid();";
                long idPartido = (long)comando.ExecuteScalar();
                return (int)idPartido;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
                return -1;
            }
        }

        // ---------------------------------------------------------------------------------- MÉTODO QUE CREA UN PARTIDO DE COPA EUROPA 2
        public static int CrearPartidoCopaEuropa2(int local, int visitante, string fecha, int competicion, int jornada, int ronda, int partidoVuelta)
        {
            var dbPath = GetDBPath();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return -1;
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();
                comando.CommandText = @"INSERT INTO partidos_copaEuropa2 (partido_vuelta, fecha, id_equipo_local, id_equipo_visitante, id_competicion, jornada, id_ronda, estado) 
                                        VALUES (@PartidoVuelta, @Fecha, @IdEquipoLocal, @IdEquipoVisitante, @Competicion, @Jornada, @Ronda, 'Pendiente')";
                comando.Parameters.AddWithValue("@PartidoVuelta", partidoVuelta);
                comando.Parameters.AddWithValue("@Fecha", fecha);
                comando.Parameters.AddWithValue("@IdEquipoLocal", local);
                comando.Parameters.AddWithValue("@IdEquipoVisitante", visitante);
                comando.Parameters.AddWithValue("@Competicion", competicion);
                comando.Parameters.AddWithValue("@Jornada", jornada);
                comando.Parameters.AddWithValue("@Ronda", ronda);

                comando.ExecuteNonQuery();

                // Obtener el ID recién insertado
                comando.CommandText = "SELECT last_insert_rowid();";
                long idPartido = (long)comando.ExecuteScalar();
                return (int)idPartido;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
                return -1;
            }
        }

        // --------------------------------------------------------------------------------------------- MÉTODO QUE ELIMINA UN PARTIDO
        public static void EliminarPartidos(List<int> idsPartidos)
        {
            var dbPath = GetDBPath();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                foreach (int id in idsPartidos)
                {
                    using var comando = conexion.CreateCommand();
                    comando.CommandText = @"DELETE FROM partidos WHERE id_partido = @IdPartido";
                    comando.Parameters.AddWithValue("@IdPartido", id);

                    comando.ExecuteNonQuery();

                    comando.CommandText = @"DELETE FROM partidos_copaNacional WHERE id_partido = @IdPartido";
                    comando.Parameters.AddWithValue("@IdPartido", id);

                    comando.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                // Manejo de errores
                Debug.Log($"Error al eliminar los partidos: {ex.Message}");
            }
        }

        // ----------------------------------------------------------------- METODO PARA OBTENER LOS EQUIPOS DE LA LIGA DE FORMA ALEATORIA
        public static List<int> ObtenerEquiposLigaAleatoria(int idCompeticion)
        {
            var dbPath = GetDBPath();

            List<int> equipos = new List<int>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT id_equipo FROM equipos WHERE id_competicion = @IdCompeticion ORDER BY RANDOM()";

                    comando.Parameters.AddWithValue("@IdCompeticion", idCompeticion);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipos.Add(reader.GetInt32(0));
                        }
                    }
                }
            }

            return equipos;
        }

        // --------------------------------------------------------------------------------- METODO PARA CREAR EL CALENDARIO
        public static void GenerarCalendario(int temporadaActual, int idCompeticion)
        {
            List<int> equipos = ObtenerEquiposLigaAleatoria(idCompeticion);
            if (equipos.Count < 2)
            {
                throw new Exception("No hay suficientes equipos para generar un calendario.");
            }

            List<List<Tuple<int, int>>> calendario = GenerarRoundRobin(equipos);

            GuardarCalendario(calendario, temporadaActual, idCompeticion);
        }

        // --------------------------------------------------------------------------------- MÉTODO QUE GENERA EL ROUND ROBIN
        private static List<List<Tuple<int, int>>> GenerarRoundRobin(List<int> equipos)
        {
            int numEquipos = equipos.Count;

            // Si el número de equipos es impar, añadimos un equipo "fantasma"
            bool esImpar = numEquipos % 2 != 0;
            if (esImpar)
            {
                equipos.Add(-1); // ID -1 representa descanso
                numEquipos++;
            }

            List<List<Tuple<int, int>>> jornadasIda = new List<List<Tuple<int, int>>>();

            // Crear una copia de los equipos sin el primero (que se fija)
            List<int> rotables = equipos.Skip(1).ToList();
            int equipoFijo = equipos[0];

            // Generar la primera vuelta (ida)
            for (int i = 0; i < numEquipos - 1; i++)
            {
                List<Tuple<int, int>> jornada = new List<Tuple<int, int>>();

                if (i % 2 == 0)
                    jornada.Add(new Tuple<int, int>(equipoFijo, rotables[0]));
                else
                    jornada.Add(new Tuple<int, int>(rotables[0], equipoFijo));

                for (int j = 1; j < numEquipos / 2; j++)
                {
                    int local = rotables[j];
                    int visitante = rotables[numEquipos - 1 - j];

                    if (i % 2 == 0)
                        jornada.Add(new Tuple<int, int>(local, visitante));
                    else
                        jornada.Add(new Tuple<int, int>(visitante, local));
                }

                jornadasIda.Add(jornada);

                // Rotar
                rotables.Insert(0, rotables[^1]);
                rotables.RemoveAt(rotables.Count - 1);
            }

            // Generar la segunda vuelta (vuelta) invirtiendo local y visitante
            List<List<Tuple<int, int>>> jornadasVuelta = jornadasIda
                .Select(jornada => jornada
                    .Select(partido => new Tuple<int, int>(partido.Item2, partido.Item1)).ToList())
                .ToList();

            // Unir ambas vueltas
            return jornadasIda.Concat(jornadasVuelta).ToList();
        }

        // ------------------------------------------------------------------------ MÉTODO QUE GUARDA EL CALENDARIO EN LA BASE DE DATOS
        private static void GuardarCalendario(List<List<Tuple<int, int>>> calendario, int temporada, int idCompeticion)
        {
            DateTime fechaInicio = ObtenerTercerSabadoDeAgosto(temporada);
            int jornadaNum = 1;

            foreach (var jornada in calendario)
            {
                // Calcula las dos fechas para los partidos de la jornada
                DateTime fechaPrimerDia = fechaInicio.AddDays((jornadaNum - 1) * 7); // Día 1 de la jornada (Sabado)
                DateTime fechaSegundoDia = fechaPrimerDia.AddDays(1); // Día 2 de la jornada (Domingo)

                // Dividir los partidos de la jornada en dos grupos
                for (int i = 0; i < jornada.Count; i++)
                {
                    // Si es el primer grupo de 5 partidos, asignar fechaPrimerDia
                    DateTime fechaPartido = (i < 5) ? fechaPrimerDia : fechaSegundoDia;
                    var partido = jornada[i];

                    // Llamamos a tu método para insertar en la BD
                    CrearPartido(partido.Item1, partido.Item2, fechaPartido.ToString("yyyy-MM-dd"), idCompeticion, jornadaNum);
                }

                jornadaNum++;
            }
        }

        // ----------------------------------------------------------------------- MÉTODO QUE GENERA LOS 1/32 DE FINAL DE COPA
        public static void GenerarTreintaidosavosCopa(List<Equipo> equiposCopa, int temporada, int idCompeticionCopa)
        {
            if (equiposCopa.Count != 64)
                throw new ArgumentException("Deben ser exactamente 64 equipos para los dieciseisavos de final.");

            // Mezclar aleatoriamente
            Random rnd = new Random();
            var equiposMezclados = equiposCopa.OrderBy(e => rnd.Next()).ToList();

            // Fechas de ida y vuelta
            DateTime fechaIda = ObtenerPrimerMiercolesSeptiembre(temporada);
            DateTime fechaVuelta = fechaIda.AddDays(14);

            // Crear los emparejamientos
            for (int i = 0; i < equiposMezclados.Count; i += 2)
            {
                int idLocal = equiposMezclados[i].IdEquipo;
                int idVisitante = equiposMezclados[i + 1].IdEquipo;

                // Partido de ida
                CrearPartidoCopa(idLocal, idVisitante, fechaIda.ToString("yyyy-MM-dd"), idCompeticionCopa, 1, 0);

                // Partido de vuelta (se invierte local/visitante)
                CrearPartidoCopa(idVisitante, idLocal, fechaVuelta.ToString("yyyy-MM-dd"), idCompeticionCopa, 1, 1);
            }
        }

        // ----------------------------------------------------------------------- MÉTODO QUE GENERA EL CALENDARIO DE COPA DE EUROPA 1
        public static void GenerarCalendarioChampions(List<Equipo> equipos, int idCompeticion, DateTime fechaInicio)
        {
            // Jornada 1
            CrearPartidoCopaEuropa(equipos[35].IdEquipo, equipos[7].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[19].IdEquipo, equipos[13].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[12].IdEquipo, equipos[0].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[11].IdEquipo, equipos[24].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[10].IdEquipo, equipos[25].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[22].IdEquipo, equipos[6].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[30].IdEquipo, equipos[33].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[27].IdEquipo, equipos[26].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[20].IdEquipo, equipos[34].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[23].IdEquipo, equipos[9].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[21].IdEquipo, equipos[3].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[14].IdEquipo, equipos[32].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[18].IdEquipo, equipos[5].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[28].IdEquipo, equipos[15].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[16].IdEquipo, equipos[1].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[8].IdEquipo, equipos[2].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[4].IdEquipo, equipos[31].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa(equipos[17].IdEquipo, equipos[29].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);

            // Jornada 2
            CrearPartidoCopaEuropa(equipos[33].IdEquipo, equipos[17].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[25].IdEquipo, equipos[30].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[2].IdEquipo, equipos[14].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[5].IdEquipo, equipos[12].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[9].IdEquipo, equipos[20].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[1].IdEquipo, equipos[35].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[3].IdEquipo, equipos[28].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[13].IdEquipo, equipos[22].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[34].IdEquipo, equipos[21].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[26].IdEquipo, equipos[8].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[32].IdEquipo, equipos[18].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[7].IdEquipo, equipos[11].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[24].IdEquipo, equipos[16].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[0].IdEquipo, equipos[27].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[6].IdEquipo, equipos[10].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[31].IdEquipo, equipos[19].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[29].IdEquipo, equipos[23].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa(equipos[15].IdEquipo, equipos[4].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);

            // Jornada 3
            CrearPartidoCopaEuropa(equipos[12].IdEquipo, equipos[23].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[16].IdEquipo, equipos[28].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[2].IdEquipo, equipos[26].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[7].IdEquipo, equipos[27].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[32].IdEquipo, equipos[34].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[19].IdEquipo, equipos[25].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[14].IdEquipo, equipos[13].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[10].IdEquipo, equipos[9].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[29].IdEquipo, equipos[22].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[8].IdEquipo, equipos[20].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[17].IdEquipo, equipos[5].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[4].IdEquipo, equipos[6].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[35].IdEquipo, equipos[3].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[1].IdEquipo, equipos[11].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[33].IdEquipo, equipos[24].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[21].IdEquipo, equipos[30].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[31].IdEquipo, equipos[0].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa(equipos[15].IdEquipo, equipos[18].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);

            // Jornada 4
            CrearPartidoCopaEuropa(equipos[13].IdEquipo, equipos[32].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[34].IdEquipo, equipos[24].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[27].IdEquipo, equipos[16].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[9].IdEquipo, equipos[29].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[20].IdEquipo, equipos[31].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[0].IdEquipo, equipos[5].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[6].IdEquipo, equipos[19].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[10].IdEquipo, equipos[12].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[22].IdEquipo, equipos[21].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[23].IdEquipo, equipos[7].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[26].IdEquipo, equipos[35].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[30].IdEquipo, equipos[17].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[11].IdEquipo, equipos[15].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[3].IdEquipo, equipos[2].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[18].IdEquipo, equipos[33].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[28].IdEquipo, equipos[1].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[14].IdEquipo, equipos[4].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa(equipos[25].IdEquipo, equipos[8].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);

            // Jornada 5
            CrearPartidoCopaEuropa(equipos[34].IdEquipo, equipos[12].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[30].IdEquipo, equipos[4].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[1].IdEquipo, equipos[17].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[5].IdEquipo, equipos[33].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[11].IdEquipo, equipos[14].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[3].IdEquipo, equipos[31].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[21].IdEquipo, equipos[18].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[22].IdEquipo, equipos[2].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[35].IdEquipo, equipos[8].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[28].IdEquipo, equipos[25].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[29].IdEquipo, equipos[32].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[7].IdEquipo, equipos[19].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[27].IdEquipo, equipos[6].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[20].IdEquipo, equipos[23].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[24].IdEquipo, equipos[9].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[0].IdEquipo, equipos[10].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[16].IdEquipo, equipos[15].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa(equipos[13].IdEquipo, equipos[26].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);

            // Jornada 6
            CrearPartidoCopaEuropa(equipos[32].IdEquipo, equipos[0].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[24].IdEquipo, equipos[20].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[8].IdEquipo, equipos[10].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[5].IdEquipo, equipos[3].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[23].IdEquipo, equipos[22].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[33].IdEquipo, equipos[14].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[26].IdEquipo, equipos[11].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[31].IdEquipo, equipos[7].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[17].IdEquipo, equipos[13].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[4].IdEquipo, equipos[34].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[6].IdEquipo, equipos[29].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[12].IdEquipo, equipos[28].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[2].IdEquipo, equipos[16].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[9].IdEquipo, equipos[1].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[18].IdEquipo, equipos[30].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[19].IdEquipo, equipos[21].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[15].IdEquipo, equipos[27].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa(equipos[25].IdEquipo, equipos[35].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);

            // Jornada 7
            CrearPartidoCopaEuropa(equipos[16].IdEquipo, equipos[7].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[8].IdEquipo, equipos[29].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[4].IdEquipo, equipos[5].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[27].IdEquipo, equipos[9].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[23].IdEquipo, equipos[19].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[28].IdEquipo, equipos[13].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[0].IdEquipo, equipos[6].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[34].IdEquipo, equipos[25].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[15].IdEquipo, equipos[1].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[26].IdEquipo, equipos[17].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[31].IdEquipo, equipos[22].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[12].IdEquipo, equipos[32].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[30].IdEquipo, equipos[3].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[2].IdEquipo, equipos[24].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[20].IdEquipo, equipos[35].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[18].IdEquipo, equipos[11].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[14].IdEquipo, equipos[21].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa(equipos[10].IdEquipo, equipos[33].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);

            // Jornada 8
            CrearPartidoCopaEuropa(equipos[7].IdEquipo, equipos[20].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[5].IdEquipo, equipos[30].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[9].IdEquipo, equipos[26].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[35].IdEquipo, equipos[28].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[1].IdEquipo, equipos[8].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[11].IdEquipo, equipos[34].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[3].IdEquipo, equipos[16].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[33].IdEquipo, equipos[4].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[32].IdEquipo, equipos[2].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[24].IdEquipo, equipos[12].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[19].IdEquipo, equipos[15].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[6].IdEquipo, equipos[18].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[21].IdEquipo, equipos[23].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[13].IdEquipo, equipos[0].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[29].IdEquipo, equipos[31].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[22].IdEquipo, equipos[27].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[17].IdEquipo, equipos[10].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa(equipos[25].IdEquipo, equipos[14].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
        }

        // ----------------------------------------------------------------------- MÉTODO QUE GENERA EL CALENDARIO DE COPA DE EUROPA 2
        public static void GenerarCalendarioChampions2(List<Equipo> equipos, int idCompeticion, DateTime fechaInicio)
        {
            // Jornada 1
            CrearPartidoCopaEuropa2(equipos[35].IdEquipo, equipos[7].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[19].IdEquipo, equipos[13].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[12].IdEquipo, equipos[0].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[11].IdEquipo, equipos[24].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[10].IdEquipo, equipos[25].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[22].IdEquipo, equipos[6].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[30].IdEquipo, equipos[33].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[27].IdEquipo, equipos[26].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[20].IdEquipo, equipos[34].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[23].IdEquipo, equipos[9].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[21].IdEquipo, equipos[3].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[14].IdEquipo, equipos[32].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[18].IdEquipo, equipos[5].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[28].IdEquipo, equipos[15].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[16].IdEquipo, equipos[1].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[8].IdEquipo, equipos[2].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[4].IdEquipo, equipos[31].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);
            CrearPartidoCopaEuropa2(equipos[17].IdEquipo, equipos[29].IdEquipo, fechaInicio.ToString("yyyy-MM-dd"), idCompeticion, 1, 0, 0);

            // Jornada 2
            CrearPartidoCopaEuropa2(equipos[33].IdEquipo, equipos[17].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[25].IdEquipo, equipos[30].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[2].IdEquipo, equipos[14].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[5].IdEquipo, equipos[12].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[9].IdEquipo, equipos[20].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[1].IdEquipo, equipos[35].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[3].IdEquipo, equipos[28].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[13].IdEquipo, equipos[22].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[34].IdEquipo, equipos[21].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[26].IdEquipo, equipos[8].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[32].IdEquipo, equipos[18].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[7].IdEquipo, equipos[11].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[24].IdEquipo, equipos[16].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[0].IdEquipo, equipos[27].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[6].IdEquipo, equipos[10].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[31].IdEquipo, equipos[19].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[29].IdEquipo, equipos[23].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);
            CrearPartidoCopaEuropa2(equipos[15].IdEquipo, equipos[4].IdEquipo, fechaInicio.AddDays(14).ToString("yyyy-MM-dd"), idCompeticion, 2, 0, 0);

            // Jornada 3
            CrearPartidoCopaEuropa2(equipos[12].IdEquipo, equipos[23].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[16].IdEquipo, equipos[28].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[2].IdEquipo, equipos[26].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[7].IdEquipo, equipos[27].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[32].IdEquipo, equipos[34].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[19].IdEquipo, equipos[25].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[14].IdEquipo, equipos[13].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[10].IdEquipo, equipos[9].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[29].IdEquipo, equipos[22].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[8].IdEquipo, equipos[20].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[17].IdEquipo, equipos[5].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[4].IdEquipo, equipos[6].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[35].IdEquipo, equipos[3].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[1].IdEquipo, equipos[11].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[33].IdEquipo, equipos[24].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[21].IdEquipo, equipos[30].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[31].IdEquipo, equipos[0].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);
            CrearPartidoCopaEuropa2(equipos[15].IdEquipo, equipos[18].IdEquipo, fechaInicio.AddDays(28).ToString("yyyy-MM-dd"), idCompeticion, 3, 0, 0);

            // Jornada 4
            CrearPartidoCopaEuropa2(equipos[13].IdEquipo, equipos[32].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[34].IdEquipo, equipos[24].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[27].IdEquipo, equipos[16].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[9].IdEquipo, equipos[29].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[20].IdEquipo, equipos[31].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[0].IdEquipo, equipos[5].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[6].IdEquipo, equipos[19].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[10].IdEquipo, equipos[12].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[22].IdEquipo, equipos[21].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[23].IdEquipo, equipos[7].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[26].IdEquipo, equipos[35].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[30].IdEquipo, equipos[17].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[11].IdEquipo, equipos[15].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[3].IdEquipo, equipos[2].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[18].IdEquipo, equipos[33].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[28].IdEquipo, equipos[1].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[14].IdEquipo, equipos[4].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);
            CrearPartidoCopaEuropa2(equipos[25].IdEquipo, equipos[8].IdEquipo, fechaInicio.AddDays(49).ToString("yyyy-MM-dd"), idCompeticion, 4, 0, 0);

            // Jornada 5
            CrearPartidoCopaEuropa2(equipos[34].IdEquipo, equipos[12].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[30].IdEquipo, equipos[4].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[1].IdEquipo, equipos[17].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[5].IdEquipo, equipos[33].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[11].IdEquipo, equipos[14].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[3].IdEquipo, equipos[31].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[21].IdEquipo, equipos[18].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[22].IdEquipo, equipos[2].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[35].IdEquipo, equipos[8].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[28].IdEquipo, equipos[25].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[29].IdEquipo, equipos[32].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[7].IdEquipo, equipos[19].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[27].IdEquipo, equipos[6].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[20].IdEquipo, equipos[23].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[24].IdEquipo, equipos[9].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[0].IdEquipo, equipos[10].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[16].IdEquipo, equipos[15].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);
            CrearPartidoCopaEuropa2(equipos[13].IdEquipo, equipos[26].IdEquipo, fechaInicio.AddDays(63).ToString("yyyy-MM-dd"), idCompeticion, 5, 0, 0);

            // Jornada 6
            CrearPartidoCopaEuropa2(equipos[32].IdEquipo, equipos[0].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[24].IdEquipo, equipos[20].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[8].IdEquipo, equipos[10].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[5].IdEquipo, equipos[3].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[23].IdEquipo, equipos[22].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[33].IdEquipo, equipos[14].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[26].IdEquipo, equipos[11].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[31].IdEquipo, equipos[7].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[17].IdEquipo, equipos[13].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[4].IdEquipo, equipos[34].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[6].IdEquipo, equipos[29].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[12].IdEquipo, equipos[28].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[2].IdEquipo, equipos[16].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[9].IdEquipo, equipos[1].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[18].IdEquipo, equipos[30].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[19].IdEquipo, equipos[21].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[15].IdEquipo, equipos[27].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);
            CrearPartidoCopaEuropa2(equipos[25].IdEquipo, equipos[35].IdEquipo, fechaInicio.AddDays(84).ToString("yyyy-MM-dd"), idCompeticion, 6, 0, 0);

            // Jornada 7
            CrearPartidoCopaEuropa2(equipos[16].IdEquipo, equipos[7].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[8].IdEquipo, equipos[29].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[4].IdEquipo, equipos[5].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[27].IdEquipo, equipos[9].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[23].IdEquipo, equipos[19].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[28].IdEquipo, equipos[13].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[0].IdEquipo, equipos[6].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[34].IdEquipo, equipos[25].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[15].IdEquipo, equipos[1].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[26].IdEquipo, equipos[17].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[31].IdEquipo, equipos[22].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[12].IdEquipo, equipos[32].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[30].IdEquipo, equipos[3].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[2].IdEquipo, equipos[24].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[20].IdEquipo, equipos[35].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[18].IdEquipo, equipos[11].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[14].IdEquipo, equipos[21].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);
            CrearPartidoCopaEuropa2(equipos[10].IdEquipo, equipos[33].IdEquipo, fechaInicio.AddDays(98).ToString("yyyy-MM-dd"), idCompeticion, 7, 0, 0);

            // Jornada 8
            CrearPartidoCopaEuropa2(equipos[7].IdEquipo, equipos[20].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[5].IdEquipo, equipos[30].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[9].IdEquipo, equipos[26].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[35].IdEquipo, equipos[28].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[1].IdEquipo, equipos[8].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[11].IdEquipo, equipos[34].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[3].IdEquipo, equipos[16].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[33].IdEquipo, equipos[4].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[32].IdEquipo, equipos[2].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[24].IdEquipo, equipos[12].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[19].IdEquipo, equipos[15].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[6].IdEquipo, equipos[18].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[21].IdEquipo, equipos[23].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[13].IdEquipo, equipos[0].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[29].IdEquipo, equipos[31].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[22].IdEquipo, equipos[27].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[17].IdEquipo, equipos[10].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
            CrearPartidoCopaEuropa2(equipos[25].IdEquipo, equipos[14].IdEquipo, fechaInicio.AddDays(119).ToString("yyyy-MM-dd"), idCompeticion, 8, 0, 0);
        }

        // ----------------------------------------------------------------- METODO QUE DEVUELVE EL ÚLTIMO PARTIDO DE MI EQUIPO
        public static Partido ObtenerUltimoPartido(int equipo, DateTime hoy)
        {
            var dbPath = GetDBPath();

            Partido partido = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT fecha, id_equipo_local, id_equipo_visitante, goles_local, goles_visitante, id_competicion, 
                                                CASE WHEN source = 'liga' THEN jornada ELSE 0 END AS jornada,
                                                CASE 
                                                    WHEN source = 'copa' THEN id_ronda 
                                                    WHEN source = 'europa1' THEN id_ronda 
                                                    WHEN source = 'europa2' THEN id_ronda 
                                                    ELSE NULL 
                                                END AS ronda
                                             FROM (
                                                -- Liga
                                                SELECT fecha, id_equipo_local, id_equipo_visitante, goles_local, goles_visitante, id_competicion, 
                                                    jornada, NULL AS id_ronda, 'liga' AS source
                                                FROM partidos
                                                WHERE 
                                                    (id_equipo_local = @IdEquipo OR id_equipo_visitante = @IdEquipo)
                                                    AND DATE(fecha) < DATE(@Hoy)

                                                UNION ALL

                                                -- Copa Nacional
                                                SELECT fecha, id_equipo_local, id_equipo_visitante, goles_local, goles_visitante, id_competicion, 
                                                    0 AS jornada, id_ronda, 'copa' AS source
                                                FROM partidos_copaNacional
                                                WHERE 
                                                    (id_equipo_local = @IdEquipo OR id_equipo_visitante = @IdEquipo)
                                                    AND DATE(fecha) < DATE(@Hoy)

                                                UNION ALL

                                                -- Copa Europa 1
                                                SELECT fecha, id_equipo_local, id_equipo_visitante, goles_local, goles_visitante, id_competicion, 
                                                    0 AS jornada, id_ronda, 'europa1' AS source
                                                FROM partidos_copaEuropa1
                                                WHERE 
                                                    (id_equipo_local = @IdEquipo OR id_equipo_visitante = @IdEquipo)
                                                    AND DATE(fecha) < DATE(@Hoy)

                                                UNION ALL

                                                -- Copa Europa 2
                                                SELECT fecha, id_equipo_local, id_equipo_visitante, goles_local, goles_visitante, id_competicion, 
                                                    0 AS jornada, id_ronda, 'europa2' AS source
                                                FROM partidos_copaEuropa2
                                                WHERE 
                                                    (id_equipo_local = @IdEquipo OR id_equipo_visitante = @IdEquipo)
                                                    AND DATE(fecha) < DATE(@Hoy)
                                             )
                                             ORDER BY fecha DESC
                                             LIMIT 1";

                    comando.Parameters.AddWithValue("@IdEquipo", equipo);
                    comando.Parameters.AddWithValue("@Hoy", hoy.Date);

                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            partido = new Partido
                            {
                                FechaPartido = DateTime.Parse(reader["fecha"]?.ToString() ?? "2000-01-01"),
                                IdEquipoLocal = Convert.ToInt32(reader["id_equipo_local"]),
                                IdEquipoVisitante = Convert.ToInt32(reader["id_equipo_visitante"]),
                                GolesLocal = reader["goles_local"] != DBNull.Value ? Convert.ToInt32(reader["goles_local"]) : 0,
                                GolesVisitante = reader["goles_visitante"] != DBNull.Value ? Convert.ToInt32(reader["goles_visitante"]) : 0,
                                IdCompeticion = reader["id_competicion"] != DBNull.Value ? Convert.ToInt32(reader["id_competicion"]) : 0,
                                Jornada = Convert.ToInt32(reader["jornada"]),
                                Ronda = reader["ronda"] != DBNull.Value ? Convert.ToInt32(reader["ronda"]) : (int?)null
                            };
                        }
                    }
                }
            }

            return partido;
        }

        // ------------------------------------------------------------------ METODO QUE DEVUELVE EL PRÓXIMO PARTIDO DE MI EQUIPO
        public static Partido ObtenerProximoPartido(int equipo, DateTime hoy)
        {
            var dbPath = GetDBPath();

            Partido partido = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT * FROM (SELECT 
                                                    p.id_partido,
                                                    p.fecha, 
                                                    p.jornada,
                                                    NULL AS id_ronda,
                                                    NULL AS partido_vuelta, 
                                                    el.nombre AS nombreEquipoLocal, 
                                                    ev.nombre AS nombreEquipoVisitante, 
                                                    p.id_equipo_local, 
                                                    p.id_equipo_visitante,
                                                    p.goles_local,
                                                    p.goles_visitante,
                                                    p.id_competicion
                                                FROM partidos p
                                                JOIN equipos el ON p.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON p.id_equipo_visitante = ev.id_equipo
                                                WHERE 
                                                    (p.id_equipo_local = @IdEquipo OR p.id_equipo_visitante = @IdEquipo)
                                                    AND DATE(p.fecha) >= DATE(@Hoy)

                                            UNION ALL

                                                SELECT 
                                                    pc.id_partido,
                                                    pc.fecha, 
                                                    NULL AS jornada,
                                                    pc.id_ronda,
                                                    pc.partido_vuelta,
                                                    el.nombre AS nombreEquipoLocal, 
                                                    ev.nombre AS nombreEquipoVisitante, 
                                                    pc.id_equipo_local, 
                                                    pc.id_equipo_visitante,
                                                    pc.goles_local,
                                                    pc.goles_visitante,
                                                    pc.id_competicion
                                                FROM partidos_copaNacional pc
                                                JOIN equipos el ON pc.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON pc.id_equipo_visitante = ev.id_equipo
                                                WHERE 
                                                    (pc.id_equipo_local = @IdEquipo OR pc.id_equipo_visitante = @IdEquipo)
                                                    AND DATE(pc.fecha) >= DATE(@Hoy)

                                            UNION ALL

                                                SELECT 
                                                    pe1.id_partido,
                                                    pe1.fecha, 
                                                    pe1.jornada,
                                                    pe1.id_ronda,
                                                    pe1.partido_vuelta, 
                                                    el.nombre AS nombreEquipoLocal, 
                                                    ev.nombre AS nombreEquipoVisitante, 
                                                    pe1.id_equipo_local, 
                                                    pe1.id_equipo_visitante,
                                                    pe1.goles_local,
                                                    pe1.goles_visitante,
                                                    pe1.id_competicion
                                                FROM partidos_copaEuropa1 pe1
                                                JOIN equipos el ON pe1.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON pe1.id_equipo_visitante = ev.id_equipo
                                                WHERE 
                                                    (pe1.id_equipo_local = @IdEquipo OR pe1.id_equipo_visitante = @IdEquipo)
                                                    AND DATE(pe1.fecha) >= DATE(@Hoy)

                                            UNION ALL

                                                SELECT 
                                                    pe2.id_partido,
                                                    pe2.fecha, 
                                                    pe2.jornada,
                                                    pe2.id_ronda,
                                                    pe2.partido_vuelta, 
                                                    el.nombre AS nombreEquipoLocal, 
                                                    ev.nombre AS nombreEquipoVisitante, 
                                                    pe2.id_equipo_local, 
                                                    pe2.id_equipo_visitante,
                                                    pe2.goles_local,
                                                    pe2.goles_visitante,
                                                    pe2.id_competicion
                                                FROM partidos_copaEuropa2 pe2
                                                JOIN equipos el ON pe2.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON pe2.id_equipo_visitante = ev.id_equipo
                                                WHERE 
                                                    (pe2.id_equipo_local = @IdEquipo OR pe2.id_equipo_visitante = @IdEquipo)
                                                    AND DATE(pe2.fecha) >= DATE(@Hoy)
                                        )
                                        ORDER BY fecha ASC
                                        LIMIT 1";

                    comando.Parameters.AddWithValue("@IdEquipo", equipo);
                    comando.Parameters.AddWithValue("@Hoy", hoy.Date);

                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            partido = new Partido
                            {
                                IdPartido = Convert.ToInt32(reader["id_partido"]),
                                FechaPartido = reader["fecha"] != DBNull.Value && DateTime.TryParse(reader["fecha"].ToString(), out DateTime fecha)
                                                   ? fecha
                                                   : DateTime.Parse("2000-01-01"),
                                Jornada = reader["jornada"] != DBNull.Value ? Convert.ToInt32(reader["jornada"]) : 0,
                                Ronda = reader["id_ronda"] != DBNull.Value ? Convert.ToInt32(reader["id_ronda"]) : 0,
                                PartidoVuelta = reader["partido_vuelta"] != DBNull.Value ? Convert.ToInt32(reader["partido_vuelta"]) : 0,
                                IdEquipoLocal = Convert.ToInt32(reader["id_equipo_local"]),
                                IdEquipoVisitante = Convert.ToInt32(reader["id_equipo_visitante"]),
                                GolesLocal = reader["goles_local"] != DBNull.Value ? Convert.ToInt32(reader["goles_local"]) : 0,
                                GolesVisitante = reader["goles_visitante"] != DBNull.Value ? Convert.ToInt32(reader["goles_visitante"]) : 0,
                                IdCompeticion = reader["id_competicion"] != DBNull.Value ? Convert.ToInt32(reader["id_competicion"]) : 0
                            };
                        }
                    }
                }
            }

            return partido;
        }

        // ------------------------------------------------------------------------ MÉTODO QUE OBTIENE EL TERCER SÁBADO DE AGOSTO 
        public static DateTime ObtenerTercerSabadoDeAgosto(int anio)
        {
            DateTime fecha = new DateTime(anio, 8, 1);
            int sabadosEncontrados = 0;

            while (fecha.Month == 8)
            {
                if (fecha.DayOfWeek == DayOfWeek.Saturday)
                {
                    sabadosEncontrados++;
                    if (sabadosEncontrados == 3)
                    {
                        return fecha;
                    }
                }

                fecha = fecha.AddDays(1);
            }

            throw new Exception("No se encontró el tercer sábado de agosto.");
        }

        // ------------------------------------------------------------------------ MÉTODO QUE OBTIENE EL PRIMER MIÉRCOLES DE SEPTIEMBRE
        public static DateTime ObtenerPrimerMiercolesSeptiembre(int anio)
        {
            DateTime fecha = new DateTime(anio, 9, 1);
            int miercolesEncontrados = 0;

            while (fecha.Month == 9)
            {
                if (fecha.DayOfWeek == DayOfWeek.Wednesday)
                {
                    miercolesEncontrados++;
                    if (miercolesEncontrados == 1)
                    {
                        return fecha;
                    }
                }

                fecha = fecha.AddDays(1);
            }

            throw new Exception("No se encontró el tercer sábado de agosto.");
        }

        // ------------------------------------------------------------------------ MÉTODO QUE OBTIENE EL PRIMER MARTES DE OCTUBRE
        public static DateTime ObtenerPrimerMartesOctubre(int anio)
        {
            DateTime fecha = new DateTime(anio, 10, 1);
            int miercolesEncontrados = 0;

            while (fecha.Month == 10)
            {
                if (fecha.DayOfWeek == DayOfWeek.Tuesday)
                {
                    miercolesEncontrados++;
                    if (miercolesEncontrados == 1)
                    {
                        return fecha;
                    }
                }

                fecha = fecha.AddDays(1);
            }

            throw new Exception("No se encontró el tercer sábado de agosto.");
        }

        // ------------------------------------------------------------------------ MÉTODO QUE OBTIENE EL PRIMER JUEVES DE OCTUBRE
        public static DateTime ObtenerPrimerJuevesOctubre(int anio)
        {
            DateTime fecha = new DateTime(anio, 10, 1);
            int miercolesEncontrados = 0;

            while (fecha.Month == 10)
            {
                if (fecha.DayOfWeek == DayOfWeek.Thursday)
                {
                    miercolesEncontrados++;
                    if (miercolesEncontrados == 1)
                    {
                        return fecha;
                    }
                }

                fecha = fecha.AddDays(1);
            }

            throw new Exception("No se encontró el tercer sábado de agosto.");
        }

        // ----------------------------------------------------------------- METODO QUE DEVUELVE TODOS LOS PARTIDOS DE MI EQUIPO
        public static List<Partido> MostrarMisPartidos(int equipo)
        {
            var dbPath = GetDBPath();

            List<Partido> partidos = new List<Partido>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT 
                                                p.fecha, 
                                                el.nombre AS nombreEquipoLocal, 
                                                ev.nombre AS nombreEquipoVisitante, 
                                                p.id_equipo_local, 
                                                p.id_equipo_visitante,
                                                p.goles_local,
                                                p.goles_visitante,
                                                p.id_competicion
                                            FROM partidos p
                                            JOIN equipos el ON p.id_equipo_local = el.id_equipo
                                            JOIN equipos ev ON p.id_equipo_visitante = ev.id_equipo
                                            WHERE 
                                                (p.id_equipo_local = @IdEquipo OR p.id_equipo_visitante = @IdEquipo)

                                            UNION ALL

                                            SELECT 
                                                pc.fecha, 
                                                el.nombre AS nombreEquipoLocal, 
                                                ev.nombre AS nombreEquipoVisitante, 
                                                pc.id_equipo_local, 
                                                pc.id_equipo_visitante,
                                                pc.goles_local,
                                                pc.goles_visitante,
                                                pc.id_competicion
                                            FROM partidos_copaNacional pc
                                            JOIN equipos el ON pc.id_equipo_local = el.id_equipo
                                            JOIN equipos ev ON pc.id_equipo_visitante = ev.id_equipo
                                            WHERE 
                                                (pc.id_equipo_local = @IdEquipo OR pc.id_equipo_visitante = @IdEquipo)

                                            UNION ALL

                                            SELECT 
                                                pe1.fecha, 
                                                el.nombre AS nombreEquipoLocal, 
                                                ev.nombre AS nombreEquipoVisitante, 
                                                pe1.id_equipo_local, 
                                                pe1.id_equipo_visitante,
                                                pe1.goles_local,
                                                pe1.goles_visitante,
                                                pe1.id_competicion
                                            FROM partidos_copaEuropa1 pe1
                                            JOIN equipos el ON pe1.id_equipo_local = el.id_equipo
                                            JOIN equipos ev ON pe1.id_equipo_visitante = ev.id_equipo
                                            WHERE 
                                                (pe1.id_equipo_local = @IdEquipo OR pe1.id_equipo_visitante = @IdEquipo)

                                            UNION ALL

                                            SELECT 
                                                pe2.fecha, 
                                                el.nombre AS nombreEquipoLocal, 
                                                ev.nombre AS nombreEquipoVisitante, 
                                                pe2.id_equipo_local, 
                                                pe2.id_equipo_visitante,
                                                pe2.goles_local,
                                                pe2.goles_visitante,
                                                pe2.id_competicion
                                            FROM partidos_copaEuropa2 pe2
                                            JOIN equipos el ON pe2.id_equipo_local = el.id_equipo
                                            JOIN equipos ev ON pe2.id_equipo_visitante = ev.id_equipo
                                            WHERE 
                                                (pe2.id_equipo_local = @IdEquipo OR pe2.id_equipo_visitante = @IdEquipo)

                                            ORDER BY fecha";

                    comando.Parameters.AddWithValue("@IdEquipo", equipo);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            partidos.Add(new Partido()
                            {
                                FechaPartido = DateTime.Parse(reader["fecha"]?.ToString() ?? "2000-01-01"),
                                IdEquipoLocal = Convert.ToInt32(reader["id_equipo_local"]),
                                IdEquipoVisitante = Convert.ToInt32(reader["id_equipo_visitante"]),
                                GolesLocal = reader["goles_local"] != DBNull.Value ? Convert.ToInt32(reader["goles_local"]) : 0,
                                GolesVisitante = reader["goles_visitante"] != DBNull.Value ? Convert.ToInt32(reader["goles_visitante"]) : 0,
                                IdCompeticion = reader["id_competicion"] != DBNull.Value ? Convert.ToInt32(reader["id_competicion"]) : 0
                            });
                        }
                    }
                }
            }

            return partidos;
        }

        // ------------------------------------------------------------------------ MÉTODO QUE DEVUELVE LA ÚLTIMA JORNADA DE LIGA JUGADA
        public static int ObtenerUltimaJornadaJugada(int equipo, int competicion)
        {
            var dbPath = GetDBPath();
            int ultimaJornada = 0;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return -1;
            }

            // Según la competición, determinamos tabla, campo y si tiene filtro por equipo
            string tabla = "";
            string campo = "";
            bool filtrarPorEquipo = false;

            switch (competicion)
            {
                case 1: // Liga 1
                case 2: // Liga 2
                    tabla = "partidos";
                    campo = "jornada";
                    filtrarPorEquipo = true;
                    break;

                case 4: // Copa Nacional
                    tabla = "partidos_copaNacional";
                    campo = "id_ronda";
                    filtrarPorEquipo = false;
                    break;

                case 5: // Copa Europa 1
                    tabla = "partidos_copaEuropa1";
                    campo = "jornada";
                    filtrarPorEquipo = false;
                    break;

                case 6: // Copa Europa 2
                    tabla = "partidos_copaEuropa2";
                    campo = "jornada";
                    filtrarPorEquipo = false;
                    break;

                default:
                    Debug.LogError("Competición desconocida en ObtenerUltimaJornadaJugada: " + competicion);
                    return -1;
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();

                // QUERY dinámica según si filtra por equipo o no
                if (filtrarPorEquipo)
                {
                    comando.CommandText = $@"SELECT MAX({campo})
                                            FROM {tabla}
                                            WHERE (id_equipo_local = @IdEquipo OR id_equipo_visitante = @IdEquipo)
                                            AND estado != 'Pendiente'";
                    comando.Parameters.AddWithValue("@IdEquipo", equipo);
                }
                else
                {
                    comando.CommandText = $@"SELECT MAX({campo})
                                            FROM {tabla}
                                            WHERE estado != 'Pendiente'";
                }

                object result = comando.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                    ultimaJornada = Convert.ToInt32(result);
                else
                    ultimaJornada = 0;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al obtener última jornada: {ex.Message}");
                return -1;
            }

            return ultimaJornada;
        }


        // ----------------------------------------------------------------- METODO QUE CARGA LOS PARTIDOS DE UNA JORNADA
        public static List<Partido> CargarPartidos(string tabla, Dictionary<string, object> filtros)
        {
            var dbPath = GetDBPath();
            List<Partido> partidos = new List<Partido>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            try
            {
                using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conexion.Open();

                    using (var comando = conexion.CreateCommand())
                    {
                        // Construcción dinámica del WHERE
                        string where = string.Join(" AND ", filtros.Keys.Select(k => $"{k} = @{k}"));

                        comando.CommandText = $@"SELECT fecha, jornada, id_ronda, partido_vuelta,
                                                    id_equipo_local, id_equipo_visitante,
                                                    goles_local, goles_visitante, estado, id_competicion
                                                FROM {tabla}
                                                WHERE {where}";

                        // Añadir parámetros
                        foreach (var par in filtros)
                            comando.Parameters.AddWithValue("@" + par.Key, par.Value);

                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                partidos.Add(new Partido()
                                {
                                    FechaPartido = DateTime.Parse(reader["fecha"]?.ToString() ?? "2000-01-01"),
                                    IdEquipoLocal = Convert.ToInt32(reader["id_equipo_local"]),
                                    IdEquipoVisitante = Convert.ToInt32(reader["id_equipo_visitante"]),
                                    Estado = reader.GetString(reader.GetOrdinal("estado")),
                                    GolesLocal = reader["goles_local"] != DBNull.Value ? Convert.ToInt32(reader["goles_local"]) : 0,
                                    GolesVisitante = reader["goles_visitante"] != DBNull.Value ? Convert.ToInt32(reader["goles_visitante"]) : 0,
                                    IdCompeticion = reader["id_competicion"] != DBNull.Value ? Convert.ToInt32(reader["id_competicion"]) : 0,
                                    Jornada = reader["jornada"] != DBNull.Value ? Convert.ToInt32(reader["jornada"]) : 0,
                                    Ronda = reader["id_ronda"] != DBNull.Value ? Convert.ToInt32(reader["id_ronda"]) : 0,
                                    PartidoVuelta = reader["partido_vuelta"] != DBNull.Value ? Convert.ToInt32(reader["partido_vuelta"]) : 0
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al cargar partidos: {ex.Message}");
                return null;
            }

            return partidos;
        }

        public static List<Partido> CargarPartidosPorCompeticion(int numero, int competicion, int ronda, int vuelta)
        {
            if (!config.ContainsKey(competicion))
            {
                Debug.LogError("Competición no reconocida.");
                return new List<Partido>();
            }

            var (tabla, filtro, usaVuelta) = config[competicion];
            string dbPath = GetDBPath();

            List<Partido> lista = new List<Partido>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return lista;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();

                using var comando = conexion.CreateCommand();

                if (competicion == 4)
                {
                    comando.CommandText =
                        $@"SELECT fecha, id_ronda, partido_vuelta, id_equipo_local, id_equipo_visitante, goles_local, goles_visitante, estado, id_competicion
                                     FROM partidos_copaNacional
                                     WHERE id_ronda = @Ronda AND partido_vuelta = @Vuelta AND id_competicion = @IdCompeticion";

                    comando.Parameters.AddWithValue("@Ronda", ronda);
                    comando.Parameters.AddWithValue("@Vuelta", vuelta);
                    comando.Parameters.AddWithValue("@IdCompeticion", competicion);
                }
                else
                {
                    // --- Construcción dinámica de la query ---
                    string select = "fecha, id_equipo_local, id_equipo_visitante, goles_local, goles_visitante, estado, id_competicion";

                    if (tabla != "partidos")
                    {
                        select += ", id_ronda, partido_vuelta";
                    }
                    else
                    {
                        select += ", jornada";
                    }

                    comando.CommandText =
                        $"SELECT {select} FROM {tabla} WHERE {filtro} = @Num AND id_competicion = @Comp";

                    comando.Parameters.AddWithValue("@Num", numero);
                    comando.Parameters.AddWithValue("@Comp", competicion);
                }



                using (SQLiteDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Partido p = new Partido()
                        {
                            FechaPartido = DateTime.Parse(reader["fecha"]?.ToString() ?? "2000-01-01"),
                            IdEquipoLocal = Convert.ToInt32(reader["id_equipo_local"]),
                            IdEquipoVisitante = Convert.ToInt32(reader["id_equipo_visitante"]),
                            Estado = reader.GetString(reader.GetOrdinal("estado")),
                            GolesLocal = reader["goles_local"] != DBNull.Value ? Convert.ToInt32(reader["goles_local"]) : 0,
                            GolesVisitante = reader["goles_visitante"] != DBNull.Value ? Convert.ToInt32(reader["goles_visitante"]) : 0,
                            IdCompeticion = Convert.ToInt32(reader["id_competicion"])
                        };

                        if (tabla != "partidos")
                        {
                            p.Ronda = Convert.ToInt32(reader["id_ronda"]);
                            p.PartidoVuelta = Convert.ToInt32(reader["partido_vuelta"]);
                        }
                        else
                        {
                            p.Jornada = numero;
                        }

                        lista.Add(p);
                    }
                }
            }

            return lista;
        }

        private static readonly Dictionary<int, (string tabla, string filtro, bool usaVuelta)> config =
            new Dictionary<int, (string, string, bool)>
            {
                { 1, ("partidos", "jornada", false) },                    // Liga 1
                { 2, ("partidos", "jornada", false) },                    // Liga 2
                { 4, ("partidos_copaNacional", "id_ronda", true) },       // Copa Nacional
                { 5, ("partidos_copaEuropa1", "jornada", true) },         // Europa 1
                { 6, ("partidos_copaEuropa2", "jornada", true) },         // Europa 2
            };

        // ------------------------------------------------------------------------ MÉTODO PARA OBTENER EL NOMBRE DE UNA RONDA DE COPA
        public static string ObtenerNombreRonda(int idRonda)
        {
            var dbPath = GetDBPath();
            string nombre = "";

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();

                comando.CommandText = @"SELECT nombre FROM rondas_copaNacional WHERE id_ronda = @idRonda";
                comando.Parameters.AddWithValue("@idRonda", idRonda);

                using (SQLiteDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        nombre = reader["nombre"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al obtener el nombre de la ronda: {ex.Message}");
            }

            return nombre;
        }

        // ------------------------------------------------------------------ METODO QUE DEVUELVE EL PRÓXIMO PARTIDO DE MI EQUIPO
        public static Partido MostrarProximoPartidoLocal(int equipo, Fecha hoy)
        {
            var dbPath = GetDBPath();

            Partido partido = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT * FROM (
                                                SELECT 
                                                    p.id_partido,
                                                    p.id_equipo_local,
                                                    p.id_equipo_visitante,
                                                    p.fecha,
                                                    p.goles_local,
                                                    p.goles_visitante,
                                                    p.id_competicion,
                                                    p.jornada,
                                                    NULL AS id_ronda,
                                                    el.nombre AS nombre_local, 
                                                    ev.nombre AS nombre_visitante 
                                                FROM partidos p
                                                JOIN equipos el ON p.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON p.id_equipo_visitante = ev.id_equipo
                                                WHERE p.id_equipo_local = @IdEquipo 
                                                AND p.fecha > @Fecha

                                                UNION ALL

                                                SELECT 
                                                    pc.id_partido,
                                                    pc.id_equipo_local,
                                                    pc.id_equipo_visitante,
                                                    pc.fecha,
                                                    pc.goles_local,
                                                    pc.goles_visitante,
                                                    pc.id_competicion,
                                                    NULL AS jornada,
                                                    pc.id_ronda,
                                                    el.nombre AS nombre_local, 
                                                    ev.nombre AS nombre_visitante 
                                                FROM partidos_copaNacional pc
                                                JOIN equipos el ON pc.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON pc.id_equipo_visitante = ev.id_equipo
                                                WHERE pc.id_equipo_local = @IdEquipo 
                                                AND pc.fecha > @Fecha

                                                UNION ALL

                                                SELECT 
                                                    pe1.id_partido,
                                                    pe1.id_equipo_local,
                                                    pe1.id_equipo_visitante,
                                                    pe1.fecha,
                                                    pe1.goles_local,
                                                    pe1.goles_visitante,
                                                    pe1.id_competicion,
                                                    NULL AS jornada,
                                                    pe1.id_ronda,
                                                    el.nombre AS nombre_local, 
                                                    ev.nombre AS nombre_visitante 
                                                FROM partidos_copaEuropa1 pe1
                                                JOIN equipos el ON pe1.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON pe1.id_equipo_visitante = ev.id_equipo
                                                WHERE pe1.id_equipo_local = @IdEquipo 
                                                AND pe1.fecha > @Fecha

                                                UNION ALL

                                                SELECT 
                                                    pe2.id_partido,
                                                    pe2.id_equipo_local,
                                                    pe2.id_equipo_visitante,
                                                    pe2.fecha,
                                                    pe2.goles_local,
                                                    pe2.goles_visitante,
                                                    pe2.id_competicion,
                                                    NULL AS jornada,
                                                    pe2.id_ronda,
                                                    el.nombre AS nombre_local, 
                                                    ev.nombre AS nombre_visitante 
                                                FROM partidos_copaEuropa2 pe2
                                                JOIN equipos el ON pe2.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON pe2.id_equipo_visitante = ev.id_equipo
                                                WHERE pe2.id_equipo_local = @IdEquipo 
                                                AND pe2.fecha > @Fecha
                                            )
                                            ORDER BY fecha ASC
                                            LIMIT 1";

                    comando.Parameters.AddWithValue("@IdEquipo", equipo);
                    DateTime fechaHoy = DateTime.Parse(hoy.Hoy);
                    comando.Parameters.AddWithValue("@Fecha", fechaHoy);

                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            partido = new Partido
                            {
                                FechaPartido = DateTime.Parse(reader["fecha"]?.ToString() ?? "2000-01-01"),
                                IdEquipoLocal = Convert.ToInt32(reader["id_equipo_local"]),
                                IdEquipoVisitante = Convert.ToInt32(reader["id_equipo_visitante"]),
                                GolesLocal = reader["goles_local"] != DBNull.Value ? Convert.ToInt32(reader["goles_local"]) : 0,
                                GolesVisitante = reader["goles_visitante"] != DBNull.Value ? Convert.ToInt32(reader["goles_visitante"]) : 0,
                                IdCompeticion = reader["id_competicion"] != DBNull.Value ? Convert.ToInt32(reader["id_competicion"]) : 0,
                                Jornada = reader["jornada"] != DBNull.Value ? Convert.ToInt32(reader["jornada"]) : 0,
                                NombreEquipoLocal = reader["nombre_local"]?.ToString() ?? "",
                                NombreEquipoVisitante = reader["nombre_visitante"]?.ToString() ?? "",
                                Ronda = reader["id_ronda"] != DBNull.Value ? Convert.ToInt32(reader["id_ronda"]) : 0
                            };
                        }
                    }
                }
            }

            return partido;
        }

        // ----------------------------------------------------------------- MÉTODO QUE DEVUELVE LOS ÚLTIMOS 5 PARTIDOS DE UN EQUIPO
        public static List<Partido> UltimosCincoPartidos(int equipo)
        {
            string dbPath = GetDBPath();
            List<Partido> partidos = new List<Partido>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT * FROM (
                                                SELECT 
                                                    p.fecha, 
                                                    el.nombre AS nombreEquipoLocal, 
                                                    ev.nombre AS nombreEquipoVisitante, 
                                                    p.id_equipo_local, 
                                                    p.id_equipo_visitante,
                                                    p.goles_local,
                                                    p.goles_visitante,
                                                    p.id_competicion
                                                FROM partidos p
                                                JOIN equipos el ON p.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON p.id_equipo_visitante = ev.id_equipo
                                                WHERE p.fecha < @Hoy
                                                AND (p.id_equipo_local = @IdEquipo OR p.id_equipo_visitante = @IdEquipo)

                                                UNION ALL

                                                SELECT 
                                                    pc.fecha, 
                                                    el.nombre AS nombreEquipoLocal, 
                                                    ev.nombre AS nombreEquipoVisitante, 
                                                    pc.id_equipo_local, 
                                                    pc.id_equipo_visitante,
                                                    pc.goles_local,
                                                    pc.goles_visitante,
                                                    pc.id_competicion
                                                FROM partidos_copaNacional pc
                                                JOIN equipos el ON pc.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON pc.id_equipo_visitante = ev.id_equipo
                                                WHERE pc.fecha < @Hoy
                                                AND (pc.id_equipo_local = @IdEquipo OR pc.id_equipo_visitante = @IdEquipo)

                                                UNION ALL

                                                SELECT 
                                                    pe1.fecha, 
                                                    el.nombre AS nombreEquipoLocal, 
                                                    ev.nombre AS nombreEquipoVisitante, 
                                                    pe1.id_equipo_local, 
                                                    pe1.id_equipo_visitante,
                                                    pe1.goles_local,
                                                    pe1.goles_visitante,
                                                    pe1.id_competicion
                                                FROM partidos_copaEuropa1 pe1
                                                JOIN equipos el ON pe1.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON pe1.id_equipo_visitante = ev.id_equipo
                                                WHERE pe1.fecha < @Hoy
                                                AND (pe1.id_equipo_local = @IdEquipo OR pe1.id_equipo_visitante = @IdEquipo)

                                                UNION ALL

                                                SELECT 
                                                    pe2.fecha, 
                                                    el.nombre AS nombreEquipoLocal, 
                                                    ev.nombre AS nombreEquipoVisitante, 
                                                    pe2.id_equipo_local, 
                                                    pe2.id_equipo_visitante,
                                                    pe2.goles_local,
                                                    pe2.goles_visitante,
                                                    pe2.id_competicion
                                                FROM partidos_copaEuropa2 pe2
                                                JOIN equipos el ON pe2.id_equipo_local = el.id_equipo
                                                JOIN equipos ev ON pe2.id_equipo_visitante = ev.id_equipo
                                                WHERE pe2.fecha < @Hoy
                                                AND (pe2.id_equipo_local = @IdEquipo OR pe2.id_equipo_visitante = @IdEquipo)
                                            )
                                            ORDER BY fecha DESC
                                            LIMIT 6";

                    comando.Parameters.AddWithValue("@IdEquipo", equipo);
                    Fecha f = FechaData.ObtenerFechaHoy();
                    comando.Parameters.AddWithValue("@Hoy", f.ToDateTime());

                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            partidos.Add(new Partido()
                            {
                                FechaPartido = DateTime.Parse(reader["fecha"]?.ToString() ?? "2000-01-01"),
                                IdEquipoLocal = Convert.ToInt32(reader["id_equipo_local"]),
                                IdEquipoVisitante = Convert.ToInt32(reader["id_equipo_visitante"]),
                                GolesLocal = reader["goles_local"] != DBNull.Value ? Convert.ToInt32(reader["goles_local"]) : 0,
                                GolesVisitante = reader["goles_visitante"] != DBNull.Value ? Convert.ToInt32(reader["goles_visitante"]) : 0,
                                IdCompeticion = reader["id_competicion"] != DBNull.Value ? Convert.ToInt32(reader["id_competicion"]) : 0
                            });
                        }
                    }
                }
            }

            return partidos;
        }
    }
}