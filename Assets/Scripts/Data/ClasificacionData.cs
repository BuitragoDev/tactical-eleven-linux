#nullable enable

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class ClasificacionData
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

        // ------------------------------------------------------------------ MÉTODO QUE RELLENA LA CLASIFICACIÓN DE LIGA 1
        public static void RellenarClasificacionLiga1(int competicion)
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

                    // Consulta para obtener los ID de los equipos de la competición
                    string query = @"SELECT id_equipo FROM equipos WHERE id_competicion = @competicion";
                    List<int> equipos = new List<int>();

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@competicion", competicion);
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                equipos.Add(reader.GetInt32(0));
                            }
                        }
                    }

                    // Eliminar clasificación anterior del manager
                    string queryDelete = @"DELETE FROM clasificacion";
                    using (SQLiteCommand deleteCommand = new SQLiteCommand(queryDelete, conexion))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    // Insertar cada equipo en la tabla clasificacion
                    string queryInsert = @"INSERT INTO clasificacion (id_equipo) VALUES (@idEquipo)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(queryInsert, conexion))
                    {
                        foreach (int idEquipo in equipos)
                        {
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@idEquipo", idEquipo);
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

        // ------------------------------------------------------------------ MÉTODO QUE RELLENA LA CLASIFICACIÓN DE LIGA 2
        public static void RellenarClasificacionLiga2(int competicion)
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

                    // Consulta para obtener los ID de los equipos de la competición
                    string query = @"SELECT id_equipo FROM equipos WHERE id_competicion = @competicion";
                    List<int> equipos = new List<int>();

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@competicion", competicion);
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                equipos.Add(reader.GetInt32(0));
                            }
                        }
                    }

                    // Eliminar clasificación anterior del manager
                    string queryDelete = @"DELETE FROM clasificacion2";
                    using (SQLiteCommand deleteCommand = new SQLiteCommand(queryDelete, conexion))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    // Insertar cada equipo en la tabla clasificacion
                    string queryInsert = @"INSERT INTO clasificacion2 (id_equipo) VALUES (@idEquipo)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(queryInsert, conexion))
                    {
                        foreach (int idEquipo in equipos)
                        {
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@idEquipo", idEquipo);
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

        // ------------------------------------------------------------------ MÉTODO QUE RELLENA LA CLASIFICACIÓN DE EUROPA 1
        public static void RellenarClasificacionEuropa1(List<Equipo> equiposEuropa1)
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

                    // Insertar cada equipo en la tabla clasificacion
                    string query = @"INSERT INTO clasificacion_europa1 (id_equipo) VALUES (@idEquipo)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(query, conexion))
                    {
                        foreach (Equipo equipo in equiposEuropa1)
                        {
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@idEquipo", equipo.IdEquipo);
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

        // ------------------------------------------------------------------ MÉTODO QUE RELLENA LA CLASIFICACIÓN DE EUROPA 1
        public static void RellenarClasificacionEuropa2(List<Equipo> equiposEuropa2)
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

                    // Insertar cada equipo en la tabla clasificacion
                    string query = @"INSERT INTO clasificacion_europa2 (id_equipo) VALUES (@idEquipo)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(query, conexion))
                    {
                        foreach (Equipo equipo in equiposEuropa2)
                        {
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@idEquipo", equipo.IdEquipo);
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

        // ------------------------------------------------------------------ MÉTODO QUE CREA EL OBJETO DE LA CLASIFICACIÓN DE UN EQUIPO
        public static Clasificacion? MostrarClasificacionPorEquipo(int equipo, int competicion)
        {
            var dbPath = GetDBPath();

            Clasificacion? clasificacionEquipo = null; // Cambiado a null para detectar si no se encuentra

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();
                comando.CommandText = null;
                if (competicion == 1)
                {
                    comando.CommandText = @"SELECT ROW_NUMBER() OVER (ORDER BY c.puntos DESC) AS Posicion,
                                                c.id_equipo AS IdEquipo,
                                                c.jugados AS Jugados,
                                                c.ganados AS Ganados,
                                                c.empatados AS Empatados,
                                                c.perdidos AS Perdidos,
                                                c.puntos AS Puntos,
                                                c.local_victorias AS LocalVictorias,
                                                c.local_derrotas AS LocalDerrotas,
                                                c.visitante_victorias AS VisitanteVictorias,
                                                c.visitante_derrotas AS VisitanteDerrotas,
                                                c.goles_favor AS PuntosFavor,
                                                c.goles_contra AS PuntosContra,
                                                c.racha AS Racha,
                                                e.nombre AS NombreEquipo
                                            FROM clasificacion c
                                            INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                            WHERE c.id_equipo = @IdEquipo
                                            ORDER BY c.puntos DESC";
                }
                else if (competicion == 5)
                {
                    comando.CommandText = @"SELECT ROW_NUMBER() OVER (ORDER BY c.puntos DESC) AS Posicion,
                                                c.id_equipo AS IdEquipo,
                                                c.jugados AS Jugados,
                                                c.ganados AS Ganados,
                                                c.empatados AS Empatados,
                                                c.perdidos AS Perdidos,
                                                c.puntos AS Puntos,
                                                c.local_victorias AS LocalVictorias,
                                                c.local_derrotas AS LocalDerrotas,
                                                c.visitante_victorias AS VisitanteVictorias,
                                                c.visitante_derrotas AS VisitanteDerrotas,
                                                c.goles_favor AS PuntosFavor,
                                                c.goles_contra AS PuntosContra,
                                                c.racha AS Racha,
                                                e.nombre AS NombreEquipo
                                            FROM clasificacion_europa1 c
                                            INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                            WHERE c.id_equipo = @IdEquipo
                                            ORDER BY c.puntos DESC";
                }
                else if (competicion == 6)
                {
                    comando.CommandText = @"SELECT ROW_NUMBER() OVER (ORDER BY c.puntos DESC) AS Posicion,
                                                c.id_equipo AS IdEquipo,
                                                c.jugados AS Jugados,
                                                c.ganados AS Ganados,
                                                c.empatados AS Empatados,
                                                c.perdidos AS Perdidos,
                                                c.puntos AS Puntos,
                                                c.local_victorias AS LocalVictorias,
                                                c.local_derrotas AS LocalDerrotas,
                                                c.visitante_victorias AS VisitanteVictorias,
                                                c.visitante_derrotas AS VisitanteDerrotas,
                                                c.goles_favor AS PuntosFavor,
                                                c.goles_contra AS PuntosContra,
                                                c.racha AS Racha,
                                                e.nombre AS NombreEquipo
                                            FROM clasificacion_europa2 c
                                            INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                            WHERE c.id_equipo = @IdEquipo
                                            ORDER BY c.puntos DESC";
                }
                else
                {
                    comando.CommandText = @"SELECT ROW_NUMBER() OVER (ORDER BY c.puntos DESC) AS Posicion,
                                                c.id_equipo AS IdEquipo,
                                                c.jugados AS Jugados,
                                                c.ganados AS Ganados,
                                                c.empatados AS Empatados,
                                                c.perdidos AS Perdidos,
                                                c.puntos AS Puntos,
                                                c.local_victorias AS LocalVictorias,
                                                c.local_derrotas AS LocalDerrotas,
                                                c.visitante_victorias AS VisitanteVictorias,
                                                c.visitante_derrotas AS VisitanteDerrotas,
                                                c.goles_favor AS PuntosFavor,
                                                c.goles_contra AS PuntosContra,
                                                c.racha AS Racha,
                                                e.nombre AS NombreEquipo
                                            FROM clasificacion2 c
                                            INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                            WHERE c.id_equipo = @IdEquipo
                                            ORDER BY c.puntos DESC";
                }
                comando.Parameters.AddWithValue("@IdEquipo", equipo);

                using (SQLiteDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read()) // Si encuentra un registro
                    {
                        clasificacionEquipo = new Clasificacion
                        {
                            Posicion = reader.GetInt32(0),
                            IdEquipo = reader.GetInt32(1),
                            Jugados = reader.GetInt32(2),
                            Ganados = reader.GetInt32(3),
                            Empatados = reader.GetInt32(4),
                            Perdidos = reader.GetInt32(5),
                            Puntos = reader.GetInt32(6),
                            LocalVictorias = reader.GetInt32(7),
                            LocalDerrotas = reader.GetInt32(8),
                            VisitanteVictorias = reader.GetInt32(9),
                            VisitanteDerrotas = reader.GetInt32(10),
                            GolesFavor = reader.GetInt32(11),
                            GolesContra = reader.GetInt32(12),
                            Racha = reader.GetInt32(13),
                            NombreEquipo = reader.GetString(14)
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
                return null;
            }

            return clasificacionEquipo;
        }

        // --------------------------------------------------------------------- MÉTODO QUE MUESTRA LA CLASIFICACIÓN DE LA DIVISIÓN 1
        public static List<Clasificacion> MostrarClasificacion(int competicion)
        {
            var dbPath = GetDBPath();
            List<Clasificacion> clasificaciones = new List<Clasificacion>();
            string tablaClasificacion;

            // Determina la tabla según el valor de la competición
            if (competicion == 1)
                tablaClasificacion = "clasificacion";
            else if (competicion == 2)
                tablaClasificacion = "clasificacion2";
            else if (competicion == 5)
                tablaClasificacion = "clasificacion_europa1";
            else if (competicion == 6)
                tablaClasificacion = "clasificacion_europa2";
            else
                throw new ArgumentException("Competición no válida");

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();
                comando.CommandText = $@"SELECT ROW_NUMBER() OVER (ORDER BY c.puntos DESC, (c.goles_favor - c.goles_contra) DESC) AS Posicion,
                                               c.id_equipo AS IdEquipo,
                                               c.jugados AS Jugados,
                                               c.ganados AS Ganados,
                                               c.empatados AS Empatados,
                                               c.perdidos AS Perdidos,
                                               c.puntos AS Puntos,
                                               c.local_victorias AS LocalVictorias,
                                               c.local_derrotas AS LocalDerrotas,
                                               c.visitante_victorias AS VisitanteVictorias,
                                               c.visitante_derrotas AS VisitanteDerrotas,
                                               c.goles_favor AS GolesFavor,
                                               c.goles_contra AS GolesContra,
                                               c.racha AS Racha,
                                               e.nombre AS NombreEquipo
                                        FROM {tablaClasificacion} c
                                        INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                        ORDER BY c.puntos DESC, (c.goles_favor - c.goles_contra) DESC";

                using (SQLiteDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clasificaciones.Add(new Clasificacion
                        {
                            Posicion = reader.GetInt32(0),
                            IdEquipo = reader.GetInt32(1),
                            Jugados = reader.GetInt32(2),
                            Ganados = reader.GetInt32(3),
                            Empatados = reader.GetInt32(4),
                            Perdidos = reader.GetInt32(5),
                            Puntos = reader.GetInt32(6),
                            LocalVictorias = reader.GetInt32(7),
                            LocalDerrotas = reader.GetInt32(8),
                            VisitanteVictorias = reader.GetInt32(9),
                            VisitanteDerrotas = reader.GetInt32(10),
                            GolesFavor = reader.GetInt32(11),
                            GolesContra = reader.GetInt32(12),
                            Racha = reader.GetInt32(13),
                            NombreEquipo = reader.GetString(14)
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
            }

            return clasificaciones;
        }

        // --------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL EQUIPO CON MÁS GOLES A FAVOR
        public static Clasificacion MostrarMejorAtaque(int competicion)
        {
            var dbPath = GetDBPath();
            if (!File.Exists(dbPath))
                Debug.LogError($"No se encontró la base de datos en {dbPath}");

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                // Elegimos la tabla según la competición
                string tabla = competicion switch
                {
                    1 => "clasificacion",
                    5 => "clasificacion_europa1",
                    6 => "clasificacion_europa2",
                    _ => "clasificacion2"
                };

                string query = $@"SELECT 
                                    c.id_equipo AS IdEquipo,
                                    c.jugados AS Jugados,
                                    c.ganados AS Ganados,
                                    c.empatados AS Empatados,
                                    c.perdidos AS Perdidos,
                                    c.puntos AS Puntos,
                                    c.local_victorias AS LocalVictorias,
                                    c.local_derrotas AS LocalDerrotas,
                                    c.visitante_victorias AS VisitanteVictorias,
                                    c.visitante_derrotas AS VisitanteDerrotas,
                                    c.goles_favor AS PuntosFavor,
                                    c.goles_contra AS PuntosContra,
                                    c.racha AS Racha,
                                    e.nombre AS NombreEquipo
                                FROM {tabla} c
                                INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                ORDER BY c.goles_favor DESC
                                LIMIT 1";

                using var comando = new SQLiteCommand(query, conexion);

                using SQLiteDataReader reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    return new Clasificacion
                    {
                        IdEquipo = reader.GetInt32(0),
                        Jugados = reader.GetInt32(1),
                        Ganados = reader.GetInt32(2),
                        Empatados = reader.GetInt32(3),
                        Perdidos = reader.GetInt32(4),
                        Puntos = reader.GetInt32(5),
                        LocalVictorias = reader.GetInt32(6),
                        LocalDerrotas = reader.GetInt32(7),
                        VisitanteVictorias = reader.GetInt32(8),
                        VisitanteDerrotas = reader.GetInt32(9),
                        GolesFavor = reader.GetInt32(10),
                        GolesContra = reader.GetInt32(11),
                        Racha = reader.GetInt32(12),
                        NombreEquipo = reader.GetString(13)
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al obtener mejor ataque: {ex.Message}");
            }

            return null;
        }

        // --------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL EQUIPO CON MENOS GOLES EN CONTRA
        public static Clasificacion MostrarMejorDefensa(int competicion)
        {
            var dbPath = GetDBPath();
            Clasificacion clasificacionEquipo = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                // Elegimos la tabla según la competición
                string tabla = competicion switch
                {
                    1 => "clasificacion",
                    5 => "clasificacion_europa1",
                    6 => "clasificacion_europa2",
                    _ => "clasificacion2"
                };

                string query = $@"SELECT 
                                    c.id_equipo AS IdEquipo,
                                    c.jugados AS Jugados,
                                    c.ganados AS Ganados,
                                    c.empatados AS Empatados,
                                    c.perdidos AS Perdidos,
                                    c.puntos AS Puntos,
                                    c.local_victorias AS LocalVictorias,
                                    c.local_derrotas AS LocalDerrotas,
                                    c.visitante_victorias AS VisitanteVictorias,
                                    c.visitante_derrotas AS VisitanteDerrotas,
                                    c.goles_favor AS PuntosFavor,
                                    c.goles_contra AS PuntosContra,
                                    c.racha AS Racha,
                                    e.nombre AS NombreEquipo
                                FROM {tabla} c
                                INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                ORDER BY c.goles_contra ASC
                                LIMIT 1";

                using var comando = new SQLiteCommand(query, conexion);

                using SQLiteDataReader reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    return new Clasificacion
                    {
                        IdEquipo = reader.GetInt32(0),
                        Jugados = reader.GetInt32(1),
                        Ganados = reader.GetInt32(2),
                        Empatados = reader.GetInt32(3),
                        Perdidos = reader.GetInt32(4),
                        Puntos = reader.GetInt32(5),
                        LocalVictorias = reader.GetInt32(6),
                        LocalDerrotas = reader.GetInt32(7),
                        VisitanteVictorias = reader.GetInt32(8),
                        VisitanteDerrotas = reader.GetInt32(9),
                        GolesFavor = reader.GetInt32(10),
                        GolesContra = reader.GetInt32(11),
                        Racha = reader.GetInt32(12),
                        NombreEquipo = reader.GetString(13)
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
            }

            return clasificacionEquipo;
        }

        // --------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL EQUIPO CON MEJOR RACHA
        public static Clasificacion MostrarMejorRacha(int competicion)
        {
            var dbPath = GetDBPath();
            Clasificacion clasificacionEquipo = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                // Elegimos la tabla según la competición
                string tabla = competicion switch
                {
                    1 => "clasificacion",
                    5 => "clasificacion_europa1",
                    6 => "clasificacion_europa2",
                    _ => "clasificacion2"
                };

                string query = $@"SELECT 
                                    c.id_equipo AS IdEquipo,
                                    c.jugados AS Jugados,
                                    c.ganados AS Ganados,
                                    c.empatados AS Empatados,
                                    c.perdidos AS Perdidos,
                                    c.puntos AS Puntos,
                                    c.local_victorias AS LocalVictorias,
                                    c.local_derrotas AS LocalDerrotas,
                                    c.visitante_victorias AS VisitanteVictorias,
                                    c.visitante_derrotas AS VisitanteDerrotas,
                                    c.goles_favor AS PuntosFavor,
                                    c.goles_contra AS PuntosContra,
                                    c.racha AS Racha,
                                    e.nombre AS NombreEquipo
                                FROM {tabla} c
                                INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                ORDER BY c.racha DESC
                                LIMIT 1";

                using var comando = new SQLiteCommand(query, conexion);

                using SQLiteDataReader reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    return new Clasificacion
                    {
                        IdEquipo = reader.GetInt32(0),
                        Jugados = reader.GetInt32(1),
                        Ganados = reader.GetInt32(2),
                        Empatados = reader.GetInt32(3),
                        Perdidos = reader.GetInt32(4),
                        Puntos = reader.GetInt32(5),
                        LocalVictorias = reader.GetInt32(6),
                        LocalDerrotas = reader.GetInt32(7),
                        VisitanteVictorias = reader.GetInt32(8),
                        VisitanteDerrotas = reader.GetInt32(9),
                        GolesFavor = reader.GetInt32(10),
                        GolesContra = reader.GetInt32(11),
                        Racha = reader.GetInt32(12),
                        NombreEquipo = reader.GetString(13)
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
            }

            return clasificacionEquipo;
        }

        // --------------------------------------------------------- MÉTODO PARA MOSTRAR EL EQUIPO CON MÁS VICTORIAS COMO LOCAL
        public static Clasificacion MostrarMejorEquipoLocal(int competicion)
        {
            var dbPath = GetDBPath();
            Clasificacion clasificacionEquipo = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                // Elegimos la tabla según la competición
                string tabla = competicion switch
                {
                    1 => "clasificacion",
                    5 => "clasificacion_europa1",
                    6 => "clasificacion_europa2",
                    _ => "clasificacion2"
                };

                string query = $@"SELECT 
                                    c.id_equipo AS IdEquipo,
                                    c.jugados AS Jugados,
                                    c.ganados AS Ganados,
                                    c.empatados AS Empatados,
                                    c.perdidos AS Perdidos,
                                    c.puntos AS Puntos,
                                    c.local_victorias AS LocalVictorias,
                                    c.local_derrotas AS LocalDerrotas,
                                    c.visitante_victorias AS VisitanteVictorias,
                                    c.visitante_derrotas AS VisitanteDerrotas,
                                    c.goles_favor AS PuntosFavor,
                                    c.goles_contra AS PuntosContra,
                                    c.racha AS Racha,
                                    e.nombre AS NombreEquipo
                                FROM {tabla} c
                                INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                ORDER BY c.local_victorias DESC
                                LIMIT 1";

                using var comando = new SQLiteCommand(query, conexion);

                using SQLiteDataReader reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    return new Clasificacion
                    {
                        IdEquipo = reader.GetInt32(0),
                        Jugados = reader.GetInt32(1),
                        Ganados = reader.GetInt32(2),
                        Empatados = reader.GetInt32(3),
                        Perdidos = reader.GetInt32(4),
                        Puntos = reader.GetInt32(5),
                        LocalVictorias = reader.GetInt32(6),
                        LocalDerrotas = reader.GetInt32(7),
                        VisitanteVictorias = reader.GetInt32(8),
                        VisitanteDerrotas = reader.GetInt32(9),
                        GolesFavor = reader.GetInt32(10),
                        GolesContra = reader.GetInt32(11),
                        Racha = reader.GetInt32(12),
                        NombreEquipo = reader.GetString(13)
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
            }

            return clasificacionEquipo;
        }

        // --------------------------------------------------------- MÉTODO PARA MOSTRAR EL EQUIPO CON MÁS VICTORIAS COMO VISITANTE
        public static Clasificacion MostrarMejorEquipoVisitante(int competicion)
        {
            var dbPath = GetDBPath();
            Clasificacion clasificacionEquipo = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                // Elegimos la tabla según la competición
                string tabla = competicion switch
                {
                    1 => "clasificacion",
                    5 => "clasificacion_europa1",
                    6 => "clasificacion_europa2",
                    _ => "clasificacion2"
                };

                string query = $@"SELECT 
                                    c.id_equipo AS IdEquipo,
                                    c.jugados AS Jugados,
                                    c.ganados AS Ganados,
                                    c.empatados AS Empatados,
                                    c.perdidos AS Perdidos,
                                    c.puntos AS Puntos,
                                    c.local_victorias AS LocalVictorias,
                                    c.local_derrotas AS LocalDerrotas,
                                    c.visitante_victorias AS VisitanteVictorias,
                                    c.visitante_derrotas AS VisitanteDerrotas,
                                    c.goles_favor AS PuntosFavor,
                                    c.goles_contra AS PuntosContra,
                                    c.racha AS Racha,
                                    e.nombre AS NombreEquipo
                                FROM {tabla} c
                                INNER JOIN equipos e ON c.id_equipo = e.id_equipo
                                ORDER BY c.visitante_victorias DESC
                                LIMIT 1";

                using var comando = new SQLiteCommand(query, conexion);

                using SQLiteDataReader reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    return new Clasificacion
                    {
                        IdEquipo = reader.GetInt32(0),
                        Jugados = reader.GetInt32(1),
                        Ganados = reader.GetInt32(2),
                        Empatados = reader.GetInt32(3),
                        Perdidos = reader.GetInt32(4),
                        Puntos = reader.GetInt32(5),
                        LocalVictorias = reader.GetInt32(6),
                        LocalDerrotas = reader.GetInt32(7),
                        VisitanteVictorias = reader.GetInt32(8),
                        VisitanteDerrotas = reader.GetInt32(9),
                        GolesFavor = reader.GetInt32(10),
                        GolesContra = reader.GetInt32(11),
                        Racha = reader.GetInt32(12),
                        NombreEquipo = reader.GetString(13)
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
            }

            return clasificacionEquipo;
        }
    }
}