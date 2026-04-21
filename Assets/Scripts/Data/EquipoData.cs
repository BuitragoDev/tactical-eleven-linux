using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class EquipoData
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

        // --------------------------------------------------------------------- MÉTODO QUE DEVUELVE EL NOMBRE DEL EQUIPO SEGÚN SU ID_EQUIPO
        public static string GetTeamNameById(int idEquipo)
        {
            var dbPath = GetDBPath();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return "";
            }

            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT nombre FROM equipos WHERE id_equipo=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", idEquipo);
                    var result = command.ExecuteScalar();
                    if (result != null)
                        return result.ToString();
                }
            }

            return "";
        }

        // ------------------------------------------------------------------------- MÉTODO QUE DEVUELVE LOS DETALLES DE UN EQUIPO POR SU ID
        public static Equipo ObtenerDetallesEquipo(int idEquipo)
        {
            var dbPath = GetDBPath();

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
                                                id_equipo, nombre, nombre_corto, presidente, presupuesto, ciudad, estadio, 
                                                aforo, reputacion, objetivo, rival, id_competicion, competicion_europea, 
                                                ruta_imagen, ruta_imagen120, ruta_imagen80, ruta_imagen64, ruta_imagen32, 
                                                ruta_estadio_interior, ruta_estadio_exterior, ruta_kit_local, ruta_kit_visitante
                                            FROM equipos 
                                            WHERE id_equipo = @idEquipo";

                    comando.Parameters.AddWithValue("@idEquipo", idEquipo);

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Equipo
                            {
                                IdEquipo = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                NombreCorto = reader.GetString(2),
                                Presidente = reader.GetString(3),
                                Presupuesto = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                Ciudad = reader.GetString(5),
                                Estadio = reader.GetString(6),
                                Aforo = reader.GetInt32(7),
                                Reputacion = reader.GetInt32(8),
                                Objetivo = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Rival = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                                IdCompeticion = reader.GetInt32(11),
                                CompeticionEuropea = reader.IsDBNull(12) ? 0 : reader.GetInt32(12),
                                RutaImagen = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                RutaImagen120 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                RutaImagen80 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                                RutaImagen64 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                                RutaImagen32 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                                RutaEstadioInterior = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                RutaEstadioExterior = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                RutaKitLocal = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                RutaKitVisitante = reader.IsDBNull(21) ? string.Empty : reader.GetString(21)
                            };
                        }
                    }
                }
            }

            return null;
        }

        // --------------------------------------------------------------------- MÉTODO QUE MUESTRA UNA LISTA DE LOS EQUIPOS POR COMPETICIÓN 
        public static List<Equipo> ObtenerEquiposPorCompeticion(int idCompeticion)
        {
            var dbPath = GetDBPath();

            List<Equipo> equipos = new List<Equipo>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return equipos;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT 
                                                e.id_equipo,
                                                e.nombre,
                                                e.nombre_corto,
                                                e.presidente,
                                                e.ciudad,
                                                e.estadio,
                                                e.objetivo,
                                                e.aforo,
                                                e.reputacion,
                                                e.rival,
                                                e.competicion_europea,
                                                t.nombre || ' ' || t.apellido AS entrenador,
                                                t.reputacion AS reputacion_entrenador,
                                                e.ruta_imagen, e.ruta_imagen120, e.ruta_imagen80, e.ruta_imagen64, e.ruta_imagen32, 
                                                e.ruta_estadio_interior, e.ruta_estadio_exterior, 
                                                e.ruta_kit_local, e.ruta_kit_visitante
                                            FROM
                                                equipos e
                                            LEFT JOIN
                                                entrenadores t
                                            ON
                                                e.id_equipo = t.id_equipo
                                            WHERE
                                                e.id_competicion = @IdCompeticion";
                    comando.Parameters.AddWithValue("@IdCompeticion", idCompeticion);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipos.Add(new Equipo
                            {
                                IdEquipo = reader["id_equipo"] != DBNull.Value ? Convert.ToInt32(reader["id_equipo"]) : 0,
                                Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                                NombreCorto = reader["nombre_corto"]?.ToString() ?? string.Empty,
                                Presidente = reader["presidente"]?.ToString() ?? string.Empty,
                                Ciudad = reader["ciudad"]?.ToString() ?? string.Empty,
                                Estadio = reader["estadio"]?.ToString() ?? string.Empty,
                                Aforo = int.TryParse(reader["aforo"]?.ToString(), out int capacidad) ? capacidad : 0,
                                Reputacion = int.TryParse(reader["reputacion"]?.ToString(), out int reputacion) ? reputacion : 0,
                                Objetivo = reader["objetivo"]?.ToString() ?? string.Empty,
                                Rival = int.TryParse(reader["rival"]?.ToString(), out int rival) ? rival : 0,
                                CompeticionEuropea = int.TryParse(reader["competicion_europea"]?.ToString(), out int competicionEuropea) ? competicionEuropea : 0,
                                Entrenador = reader["entrenador"] as string ?? "Sin asignar",
                                ReputacionEntrenador = reader["reputacion_entrenador"] != DBNull.Value ? Convert.ToInt32(reader["reputacion_entrenador"]) : 0,
                                RutaImagen = reader["ruta_imagen"]?.ToString() ?? string.Empty,
                                RutaImagen120 = reader["ruta_imagen120"]?.ToString() ?? string.Empty,
                                RutaImagen80 = reader["ruta_imagen80"]?.ToString() ?? string.Empty,
                                RutaImagen64 = reader["ruta_imagen64"]?.ToString() ?? string.Empty,
                                RutaImagen32 = reader["ruta_imagen32"]?.ToString() ?? string.Empty,
                                RutaEstadioInterior = reader["ruta_estadio_interior"]?.ToString() ?? string.Empty,
                                RutaEstadioExterior = reader["ruta_estadio_exterior"]?.ToString() ?? string.Empty,
                                RutaKitLocal = reader["ruta_kit_local"]?.ToString() ?? string.Empty,
                                RutaKitVisitante = reader["ruta_kit_visitante"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }

            return equipos;
        }

        // ------------------------------------------------------------- MÉTODO QUE DEVUELVE LOS DETALLES DE LOS EQUIPOS DE UN PAÍS
        public static List<Equipo> ObtenerEquiposPorPais(string pais)
        {
            var dbPath = GetDBPath();

            List<Equipo> oEquipo = new List<Equipo>();

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
                    comando.CommandText = @"SELECT id_equipo, nombre, nombre_corto, presidente, presupuesto, pais, ciudad, estadio, objetivo,
                                                aforo, reputacion, rival, id_competicion, competicion_europea,
                                                ruta_imagen, ruta_imagen120, ruta_imagen80, ruta_imagen64,
                                                ruta_imagen32, ruta_estadio_interior, ruta_estadio_exterior,
                                                ruta_kit_local, ruta_kit_visitante
                                            FROM equipos
                                            WHERE pais = @Pais";

                    comando.Parameters.AddWithValue("@Pais", pais);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oEquipo.Add(new Equipo()
                            {
                                IdEquipo = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                NombreCorto = reader.GetString(2),
                                Presidente = reader.GetString(3),
                                Presupuesto = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                Pais = reader.GetString(5),
                                Ciudad = reader.GetString(6),
                                Estadio = reader.GetString(7),
                                Objetivo = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                Aforo = reader.GetInt32(9),
                                Reputacion = reader.GetInt32(10),
                                Rival = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                                IdCompeticion = reader.GetInt32(12),
                                CompeticionEuropea = reader.IsDBNull(13) ? 0 : reader.GetInt32(13),
                                RutaImagen = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                RutaImagen120 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                                RutaImagen80 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                                RutaImagen64 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                                RutaImagen32 = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                RutaEstadioInterior = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                RutaEstadioExterior = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                RutaKitLocal = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                                RutaKitVisitante = reader.IsDBNull(22) ? string.Empty : reader.GetString(22)
                            });
                        }
                    }
                }
            }

            return oEquipo;
        }

        // ------------------------------------------------------------ MÉTODO QUE DEVUELVE LOS OBJETIVOS DE UN EQUIPO POR COMPETICIÓN
        public static List<Equipo> GetObjetivosEquiposPorCompeticion(int idCompeticion)
        {
            var dbPath = GetDBPath();

            List<Equipo> equipos = new List<Equipo>();

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
                                                e.id_equipo,
                                                e.nombre,
                                                e.objetivo,
                                                en.nombre AS entrenador_nombre,
                                                en.apellido AS entrenador_apellido,
                                                en.reputacion AS reputacion_entrenador
                                            FROM equipos e
                                            LEFT JOIN entrenadores en ON e.id_equipo = en.id_equipo
                                            WHERE e.id_competicion = @idCompeticion
                                            ORDER BY e.reputacion DESC";

                    comando.Parameters.AddWithValue("@idCompeticion", idCompeticion);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipos.Add(new Equipo()
                            {
                                IdEquipo = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Objetivo = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                Entrenador = reader.IsDBNull(3) ? "—" : $"{reader.GetString(3)} {reader.GetString(4)}",
                                ReputacionEntrenador = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                            });
                        }
                    }
                }
            }

            return equipos;
        }

        // ---------------------------------------------------------- MÉTODO QUE DEVUELVE UNA LISTA DE EQUIPOS QUE JUEGAN EN EUROPA 1
        public static List<Equipo> EquiposJueganEuropa1(int competicion)
        {
            var dbPath = GetDBPath();

            List<Equipo> equipos = new List<Equipo>();

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
                    comando.CommandText = @"SELECT e.*, en.nombre AS nombre_entrenador, en.reputacion AS reputacion_entrenador
                                            FROM equipos e
                                            LEFT JOIN entrenadores en ON e.id_equipo = en.id_equipo
                                            WHERE e.id_competicion = @idCompeticion AND e.competicion_europea = 5";

                    comando.Parameters.AddWithValue("@idCompeticion", competicion);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipos.Add(new Equipo()
                            {
                                IdEquipo = reader["id_equipo"] != DBNull.Value ? Convert.ToInt32(reader["id_equipo"]) : 0,
                                Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                                NombreCorto = reader["nombre_corto"]?.ToString() ?? string.Empty,
                                Presidente = reader["presidente"]?.ToString() ?? string.Empty,
                                Ciudad = reader["ciudad"]?.ToString() ?? string.Empty,
                                Estadio = reader["estadio"]?.ToString() ?? string.Empty,
                                Aforo = int.TryParse(reader["aforo"]?.ToString(), out int capacidad) ? capacidad : 0,
                                Reputacion = int.TryParse(reader["reputacion"]?.ToString(), out int reputacion) ? reputacion : 0,
                                Objetivo = reader["objetivo"]?.ToString() ?? string.Empty,
                                Rival = int.TryParse(reader["rival"]?.ToString(), out int rival) ? rival : 0,
                                CompeticionEuropea = int.TryParse(reader["competicion_europea"]?.ToString(), out int competicionEuropea) ? competicionEuropea : 0,
                                Entrenador = reader["nombre_entrenador"] as string ?? "Sin asignar",
                                ReputacionEntrenador = reader["reputacion_entrenador"] != DBNull.Value ? Convert.ToInt32(reader["reputacion_entrenador"]) : 0,
                                RutaImagen = reader["ruta_imagen"]?.ToString() ?? string.Empty,
                                RutaImagen120 = reader["ruta_imagen120"]?.ToString() ?? string.Empty,
                                RutaImagen80 = reader["ruta_imagen80"]?.ToString() ?? string.Empty,
                                RutaImagen64 = reader["ruta_imagen64"]?.ToString() ?? string.Empty,
                                RutaImagen32 = reader["ruta_imagen32"]?.ToString() ?? string.Empty,
                                RutaEstadioInterior = reader["ruta_estadio_interior"]?.ToString() ?? string.Empty,
                                RutaEstadioExterior = reader["ruta_estadio_exterior"]?.ToString() ?? string.Empty,
                                RutaKitLocal = reader["ruta_kit_local"]?.ToString() ?? string.Empty,
                                RutaKitVisitante = reader["ruta_kit_visitante"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }

            return equipos;
        }

        // ---------------------------------------------------------- MÉTODO QUE DEVUELVE UNA LISTA DE EQUIPOS QUE JUEGAN EN EUROPA 2
        public static List<Equipo> EquiposJueganEuropa2(int competicion)
        {
            var dbPath = GetDBPath();

            List<Equipo> equipos = new List<Equipo>();

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
                    comando.CommandText = @"SELECT e.*, en.nombre AS nombre_entrenador, en.reputacion AS reputacion_entrenador
                                            FROM equipos e
                                            LEFT JOIN entrenadores en ON e.id_equipo = en.id_equipo
                                            WHERE e.id_competicion = @idCompeticion AND e.competicion_europea = 6";

                    comando.Parameters.AddWithValue("@idCompeticion", competicion);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipos.Add(new Equipo()
                            {
                                IdEquipo = reader["id_equipo"] != DBNull.Value ? Convert.ToInt32(reader["id_equipo"]) : 0,
                                Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                                NombreCorto = reader["nombre_corto"]?.ToString() ?? string.Empty,
                                Presidente = reader["presidente"]?.ToString() ?? string.Empty,
                                Ciudad = reader["ciudad"]?.ToString() ?? string.Empty,
                                Estadio = reader["estadio"]?.ToString() ?? string.Empty,
                                Aforo = int.TryParse(reader["aforo"]?.ToString(), out int capacidad) ? capacidad : 0,
                                Reputacion = int.TryParse(reader["reputacion"]?.ToString(), out int reputacion) ? reputacion : 0,
                                Objetivo = reader["objetivo"]?.ToString() ?? string.Empty,
                                Rival = int.TryParse(reader["rival"]?.ToString(), out int rival) ? rival : 0,
                                CompeticionEuropea = int.TryParse(reader["competicion_europea"]?.ToString(), out int competicionEuropea) ? competicionEuropea : 0,
                                Entrenador = reader["nombre_entrenador"] as string ?? "Sin asignar",
                                ReputacionEntrenador = reader["reputacion_entrenador"] != DBNull.Value ? Convert.ToInt32(reader["reputacion_entrenador"]) : 0,
                                RutaImagen = reader["ruta_imagen"]?.ToString() ?? string.Empty,
                                RutaImagen120 = reader["ruta_imagen120"]?.ToString() ?? string.Empty,
                                RutaImagen80 = reader["ruta_imagen80"]?.ToString() ?? string.Empty,
                                RutaImagen64 = reader["ruta_imagen64"]?.ToString() ?? string.Empty,
                                RutaImagen32 = reader["ruta_imagen32"]?.ToString() ?? string.Empty,
                                RutaEstadioInterior = reader["ruta_estadio_interior"]?.ToString() ?? string.Empty,
                                RutaEstadioExterior = reader["ruta_estadio_exterior"]?.ToString() ?? string.Empty,
                                RutaKitLocal = reader["ruta_kit_local"]?.ToString() ?? string.Empty,
                                RutaKitVisitante = reader["ruta_kit_visitante"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }

            return equipos;
        }

        // -------------------------------------------------------------------------- MÉTODO QUE RESTA UNA CANTIDAD AL PRESUPUESTO
        public static void RestarCantidadAPresupuesto(int equipo, int cantidad)
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

                    string query = @"UPDATE equipos SET presupuesto = presupuesto - @Cantidad WHERE id_equipo = @IdEquipo";

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);
                        comando.Parameters.AddWithValue("@Cantidad", cantidad);
                        comando.ExecuteNonQuery();
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------------- MÉTODO QUE SUMA UNA CANTIDAD AL PRESUPUESTO
        public static void SumarCantidadAPresupuesto(int equipo, int cantidad)
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

                    string query = @"UPDATE equipos SET presupuesto = presupuesto + @Cantidad WHERE id_equipo = @IdEquipo";

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);
                        comando.Parameters.AddWithValue("@Cantidad", cantidad);
                        comando.ExecuteNonQuery();
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }
    }
}