using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class EstadisticaJugadorData
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

        // ------------------------------------------------------------- MÉTODO QUE INSERTA UNA FILA DE ESTADÍSTICA POR CADA JUGADOR DE LIGA 
        public static void InsertarEstadisticasJugadores()
        {
            try
            {
                // Usa la base activa (temporal si existe)
                string dbPath = DatabaseManager.GetActiveDatabasePath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                    return;
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Obtener todos los id_jugador de la tabla jugadores
                    string query = "SELECT id_jugador FROM jugadores WHERE id_jugador < 5000";
                    List<int> listaJugadores = new List<int>();

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaJugadores.Add(reader.GetInt32(0));
                        }
                    }

                    // Insertar una fila en estadisticas_jugadores por cada jugador
                    string insertQuery = @"INSERT INTO estadisticas_jugadores (id_jugador)
                                           VALUES (@IdJugador)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, conexion))
                    {
                        insertCommand.Parameters.Add(new SQLiteParameter("@IdJugador"));

                        foreach (int idJugador in listaJugadores)
                        {
                            insertCommand.Parameters["@IdJugador"].Value = idJugador;
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // ------------------------------------------------------ MÉTODO QUE INSERTA UNA FILA DE ESTADÍSTICA POR CADA JUGADOR DE EQUIPO EUROPEO
        public static void InsertarEstadisticasJugadoresEuropa()
        {
            try
            {
                // Usa la base activa (temporal si existe)
                string dbPath = DatabaseManager.GetActiveDatabasePath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                    return;
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT j.id_jugador
                                     FROM jugadores j
                                     INNER JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE j.id_jugador >= 5000 OR e.competicion_europea != 0";
                    List<int> listaJugadores = new List<int>();

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaJugadores.Add(reader.GetInt32(0));
                        }
                    }

                    // Insertar una fila en estadisticas_jugadores por cada jugador
                    string insertQuery = @"INSERT INTO estadisticas_jugadores_europa (id_jugador)
                                           VALUES (@IdJugador)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, conexion))
                    {
                        insertCommand.Parameters.Add(new SQLiteParameter("@IdJugador"));

                        foreach (int idJugador in listaJugadores)
                        {
                            insertCommand.Parameters["@IdJugador"].Value = idJugador;
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS GOLES
        public static Estadistica MostrarJugadorConMasGoles(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                Goles = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.goles, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.goles DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.Goles = reader.GetInt32(reader.GetOrdinal("goles"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS ASISTENCIAS
        public static Estadistica MostrarJugadorConMasAsistencias(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                Asistencias = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.asistencias, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.asistencias DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.Asistencias = reader.GetInt32(reader.GetOrdinal("asistencias"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS MVP
        public static Estadistica MostrarJugadorConMasMvp(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                MVP = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.mvp, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.mvp DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.MVP = reader.GetInt32(reader.GetOrdinal("mvp"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS TARJETAS AMARILLAS
        public static Estadistica MostrarJugadorConMasTarjetasAmarillas(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                TarjetasAmarillas = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.tarjetasAmarillas, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.tarjetasAmarillas DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.TarjetasAmarillas = reader.GetInt32(reader.GetOrdinal("tarjetasAmarillas"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS TARJETAS ROJAS
        public static Estadistica MostrarJugadorConMasTarjetasRojas(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                TarjetasRojas = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.tarjetasRojas, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.tarjetasRojas DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.TarjetasRojas = reader.GetInt32(reader.GetOrdinal("tarjetasRojas"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE LAS ESTADÍSTICAS DE UN EQUIPO
        public static List<Estadistica> MostrarEstadisticasEquipo(int idEquipo)
        {
            List<Estadistica> listaEstadistica = new List<Estadistica>();

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        j.id_jugador,
                                        j.nombre,
                                        j.apellido,
                                        j.dorsal,
                                        j.nacionalidad,
                                        j.rol_id,
                                        e.partidosJugados,
                                        e.goles,
                                        e.asistencias,
                                        e.tarjetasAmarillas,
                                        e.tarjetasRojas,
                                        e.mvp
                                    FROM jugadores j
                                    LEFT JOIN estadisticas_jugadores e ON j.id_jugador = e.id_jugador
                                    WHERE j.id_equipo = @idEquipo";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idEquipo", idEquipo);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            listaEstadistica.Clear();  // Asegura que la lista esté vacía antes de llenarla

                            while (reader.Read())
                            {
                                listaEstadistica.Add(new Estadistica
                                {
                                    IdJugador = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2),
                                    Dorsal = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                    Nacionalidad = reader.GetString(4),
                                    RolId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                                    PartidosJugados = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                                    Goles = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                    Asistencias = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                                    TarjetasAmarillas = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                                    TarjetasRojas = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                                    MVP = reader.IsDBNull(11) ? 0 : reader.GetInt32(11)
                                });
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return listaEstadistica;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE LAS ESTADÍSTICAS DE UN EQUIPO
        public static List<Estadistica> MostrarEstadisticasTotales(int filtro, int competicion)
        {
            var lista = new List<Estadistica>();

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                    return lista;
                }

                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                // 1) Selección de columna para ordenar
                string ordenColumna = filtro switch
                {
                    1 => "e.goles",
                    2 => "e.asistencias",
                    3 => "e.tarjetasAmarillas",
                    4 => "e.tarjetasRojas",
                    5 => "e.mvp",
                    _ => "e.goles"
                };

                // 2) Selección de tabla según competición
                string tablaEstadisticas = (competicion <= 2)
                    ? "estadisticas_jugadores"
                    : "estadisticas_jugadores_europa";

                // 3) Query limpia y legible
                string query = $@"SELECT 
                                        j.id_jugador, j.nombre, j.apellido, j.dorsal, j.nacionalidad, j.rol_id,
                                        e.partidosJugados, e.goles, e.asistencias, e.tarjetasAmarillas, 
                                        e.tarjetasRojas, e.mvp,
                                        j.id_equipo
                                FROM jugadores j
                                LEFT JOIN equipos eq ON eq.id_equipo = j.id_equipo
                                LEFT JOIN {tablaEstadisticas} e ON j.id_jugador = e.id_jugador
                                WHERE eq.id_competicion = @Comp OR eq.competicion_europea = @Comp
                                ORDER BY {ordenColumna} DESC
                                LIMIT 25";

                using var cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Comp", competicion);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Estadistica
                    {
                        IdJugador = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Apellido = reader.GetString(2),
                        Dorsal = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Nacionalidad = reader.GetString(4),
                        RolId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        PartidosJugados = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                        Goles = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                        Asistencias = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                        TarjetasAmarillas = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                        TarjetasRojas = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                        MVP = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),   // ← Corregido
                        IdEquipo = reader.GetInt32(12)
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al leer estadísticas: {ex.Message}");
            }

            return lista;
        }

        // -------------------------------------------------------------- MÉTODO PARA MOSTRAR LAS ESTADÍSTICAS DE UN JUGADOR
        public static Estadistica MostrarEstadisticasJugador(int id)
        {
            Estadistica total = new Estadistica();

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                // Tablas a consultar
                string[] tablas = { "estadisticas_jugadores", "estadisticas_jugadores_europa" };

                foreach (string tabla in tablas)
                {
                    Estadistica parcial = LeerEstadisticasDesdeTabla(id, tabla, conexion);

                    // Sumar si existen datos
                    if (parcial != null && parcial.IdJugador != 0)
                    {
                        total.PartidosJugados += parcial.PartidosJugados;
                        total.Goles += parcial.Goles;
                        total.Asistencias += parcial.Asistencias;
                        total.TarjetasAmarillas += parcial.TarjetasAmarillas;
                        total.TarjetasRojas += parcial.TarjetasRojas;
                        total.MVP += parcial.MVP;

                        // Siempre devolver el id
                        total.IdJugador = id;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al leer estadísticas: {ex.Message}");
            }

            return total;
        }

        // Método auxiliar reutilizable
        private static Estadistica LeerEstadisticasDesdeTabla(int id, string tabla, SQLiteConnection conexion)
        {
            try
            {
                string query = $@"SELECT 
                                    id_jugador,
                                    partidosJugados,
                                    goles,
                                    asistencias,
                                    tarjetasAmarillas,
                                    tarjetasRojas,
                                    mvp
                                FROM {tabla}
                                WHERE id_jugador = @idJugador";

                using var cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.AddWithValue("@idJugador", id);

                using SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Estadistica
                    {
                        IdJugador = reader.GetInt32(0),
                        PartidosJugados = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                        Goles = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                        Asistencias = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        TarjetasAmarillas = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        TarjetasRojas = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        MVP = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error leyendo tabla {tabla}: {ex.Message}");
            }

            return null;
        }
    }
}
