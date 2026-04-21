using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class PalmaresData
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

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL PALMARÉS DEL MÁNAGER
        public static List<PalmaresManager> MostrarPalmaresManager(int equipo)
        {
            var dbPath = GetDBPath();
            List<PalmaresManager> listadoPalmares = new List<PalmaresManager>();

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
                    comando.CommandText = @"SELECT id_palmares, id_competicion, id_equipo, temporada
                                            FROM palmares_manager
                                            WHERE id_equipo = @IdEquipo
                                            ORDER BY id_palmares DESC";

                    comando.Parameters.AddWithValue("@IdEquipo", equipo);

                    using (SQLiteDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listadoPalmares.Add(new PalmaresManager()
                            {
                                // Usamos el operador de coalescencia nula para evitar la asignación de null
                                IdEquipo = dr["id_equipo"] != DBNull.Value ? Convert.ToInt32(dr["id_equipo"]) : 0,
                                IdCompeticion = dr["id_competicion"] != DBNull.Value ? Convert.ToInt32(dr["id_competicion"]) : 0,
                                Temporada = dr["temporada"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }

            return listadoPalmares;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL PALMARÉS COMPLETO
        public static List<Palmares> MostrarPalmaresCompleto(int competicion)
        {
            var dbPath = GetDBPath();
            List<Palmares> listadoPalmares = new List<Palmares>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();

                string tabla = competicion switch
                {
                    1 => "palmares",
                    4 => "palmaresCopa",
                    5 => "palmaresCopaEuropa1",
                    6 => "palmaresCopaEuropa2",
                    _ => "palmares"
                };

                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = $@"SELECT p.id_equipo, p.titulos, e.nombre AS NombreEquipo
                                            FROM {tabla} p
                                            JOIN equipos e ON e.id_equipo = p.id_equipo
                                            ORDER BY titulos DESC";

                    using (SQLiteDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listadoPalmares.Add(new Palmares()
                            {
                                // Usamos el operador de coalescencia nula para evitar la asignación de null
                                IdEquipo = dr["id_equipo"] != DBNull.Value ? Convert.ToInt32(dr["id_equipo"]) : 0,
                                Titulos = dr["titulos"] != DBNull.Value ? Convert.ToInt32(dr["titulos"]) : 0,
                                NombreEquipo = dr.GetString(dr.GetOrdinal("NombreEquipo"))
                            });
                        }
                    }
                }
            }

            return listadoPalmares;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL HISTORIAL DE LAS FINALES
        public static List<HistorialFinales> MostrarHistorialFinales(int competicion)
        {
            var dbPath = GetDBPath();
            List<HistorialFinales> listadoHistorial = new List<HistorialFinales>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();

                string tabla = competicion switch
                {
                    1 => "historial_finales",
                    4 => "historial_finalesCopa",
                    5 => "historial_finalesCopaEuropa1",
                    6 => "historial_finalesCopaEuropa2",
                    _ => "historial_finales"
                };

                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = $@"SELECT h.id_historial,
                                                h.temporada, 
                                                h.id_equipo_campeon,
                                                h.id_equipo_finalista,
                                                e1.nombre AS equipo_campeon, 
                                                e2.nombre AS equipo_finalista
                                            FROM {tabla} h
                                            JOIN equipos e1 ON h.id_equipo_campeon = e1.id_equipo
                                            JOIN equipos e2 ON h.id_equipo_finalista = e2.id_equipo
                                            ORDER BY h.id_historial DESC";

                    using (SQLiteDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listadoHistorial.Add(new HistorialFinales()
                            {
                                Temporada = dr["temporada"]?.ToString() ?? string.Empty,
                                IdEquipoCampeon = dr["id_equipo_campeon"] != DBNull.Value ? Convert.ToInt32(dr["id_equipo_campeon"]) : 0,
                                IdEquipoFinalista = dr["id_equipo_finalista"] != DBNull.Value ? Convert.ToInt32(dr["id_equipo_finalista"]) : 0,
                                NombreEquipoCampeon = dr["equipo_campeon"]?.ToString() ?? string.Empty,
                                NombreEquipoFinalista = dr["equipo_finalista"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }

            return listadoHistorial;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL PALMARÉS DEL BALÓN DE ORO
        public static List<PalmaresJugador> MostrarPalmaresBalonOroTotal()
        {
            var dbPath = GetDBPath();
            List<PalmaresJugador> listadoPalmares = new List<PalmaresJugador>();

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
                    comando.CommandText = $@"SELECT p.id_jugador, 
                                                p.titulos, 
                                                j.nombre || ' ' || j.apellido AS nombreJugador
                                            FROM palmaresJugadores p
                                            JOIN jugadores j ON j.id_jugador = p.id_jugador
                                            ORDER BY titulos DESC";

                    using (SQLiteDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listadoPalmares.Add(new PalmaresJugador()
                            {
                                // Usamos el operador de coalescencia nula para evitar la asignación de null
                                IdJugador = dr["id_jugador"] != DBNull.Value ? Convert.ToInt32(dr["id_jugador"]) : 0,
                                Titulos = dr["titulos"] != DBNull.Value ? Convert.ToInt32(dr["titulos"]) : 0,
                                NombreJugador = dr.GetString(dr.GetOrdinal("nombreJugador"))
                            });
                        }
                    }
                }
            }

            return listadoPalmares;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL PALMARÉS DEL BOTA DE ORO
        public static List<PalmaresJugador> MostrarPalmaresBotaOroTotal()
        {
            var dbPath = GetDBPath();
            List<PalmaresJugador> listadoPalmares = new List<PalmaresJugador>();

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
                    comando.CommandText = $@"SELECT p.id_jugador, 
                                                    p.titulos, 
                                                    j.nombre || ' ' || j.apellido AS nombreJugador
                                            FROM palmaresGoleadores p
                                            JOIN jugadores j ON j.id_jugador = p.id_jugador
                                            ORDER BY titulos DESC";

                    using (SQLiteDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listadoPalmares.Add(new PalmaresJugador()
                            {
                                // Usamos el operador de coalescencia nula para evitar la asignación de null
                                IdJugador = dr["id_jugador"] != DBNull.Value ? Convert.ToInt32(dr["id_jugador"]) : 0,
                                Titulos = dr["titulos"] != DBNull.Value ? Convert.ToInt32(dr["titulos"]) : 0,
                                NombreJugador = dr.GetString(dr.GetOrdinal("nombreJugador"))
                            });
                        }
                    }
                }
            }

            return listadoPalmares;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL HISTORIAL DEL BALÓN DE ORO
        public static List<HistorialJugador> MostrarPalmaresBalonOro()
        {
            var dbPath = GetDBPath();
            List<HistorialJugador> listadoHistorial = new List<HistorialJugador>();

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
                    comando.CommandText = $@"SELECT 
                                                h.temporada,
                                                h.id_jugador_oro,
                                                h.id_jugador_plata,
                                                h.id_jugador_bronce,
                                                oro.nombre || ' ' || oro.apellido AS jugador_oro,
                                                plata.nombre || ' ' || plata.apellido AS jugador_plata,
                                                bronce.nombre || ' ' || bronce.apellido AS jugador_bronce
                                            FROM 
                                                historial_mejorJugador h
                                            JOIN 
                                                jugadores oro ON h.id_jugador_oro = oro.id_jugador
                                            JOIN 
                                                jugadores plata ON h.id_jugador_plata = plata.id_jugador
                                            JOIN 
                                                jugadores bronce ON h.id_jugador_bronce = bronce.id_jugador
                                            ORDER BY 
                                                h.temporada DESC";

                    using (SQLiteDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listadoHistorial.Add(new HistorialJugador()
                            {
                                Temporada = dr["temporada"]?.ToString() ?? string.Empty,
                                IdJugadorOro = dr["id_jugador_oro"] != DBNull.Value ? Convert.ToInt32(dr["id_jugador_oro"]) : 0,
                                IdJugadorPlata = dr["id_jugador_plata"] != DBNull.Value ? Convert.ToInt32(dr["id_jugador_plata"]) : 0,
                                IdJugadorBronce = dr["id_jugador_bronce"] != DBNull.Value ? Convert.ToInt32(dr["id_jugador_bronce"]) : 0,
                                NombreJugadorOro = dr["jugador_oro"]?.ToString() ?? string.Empty,
                                NombreJugadorPlata = dr["jugador_plata"]?.ToString() ?? string.Empty,
                                NombreJugadorBronce = dr["jugador_bronce"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }

            return listadoHistorial;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL HISTORIAL DEL BALÓN DE ORO
        public static List<HistorialJugador> MostrarPalmaresBotaOro()
        {
            var dbPath = GetDBPath();
            List<HistorialJugador> listadoHistorial = new List<HistorialJugador>();

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
                    comando.CommandText = $@"SELECT 
                                                h.temporada,
                                                h.id_jugador_oro,
                                                h.id_jugador_plata,
                                                h.id_jugador_bronce,
                                                oro.nombre || ' ' || oro.apellido AS jugador_oro,
                                                plata.nombre || ' ' || plata.apellido AS jugador_plata,
                                                bronce.nombre || ' ' || bronce.apellido AS jugador_bronce
                                            FROM 
                                                historial_maximoGoleador h
                                            JOIN 
                                                jugadores oro ON h.id_jugador_oro = oro.id_jugador
                                            JOIN 
                                                jugadores plata ON h.id_jugador_plata = plata.id_jugador
                                            JOIN 
                                                jugadores bronce ON h.id_jugador_bronce = bronce.id_jugador
                                            ORDER BY 
                                                h.temporada DESC";

                    using (SQLiteDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listadoHistorial.Add(new HistorialJugador()
                            {
                                Temporada = dr["temporada"]?.ToString() ?? string.Empty,
                                IdJugadorOro = dr["id_jugador_oro"] != DBNull.Value ? Convert.ToInt32(dr["id_jugador_oro"]) : 0,
                                IdJugadorPlata = dr["id_jugador_plata"] != DBNull.Value ? Convert.ToInt32(dr["id_jugador_plata"]) : 0,
                                IdJugadorBronce = dr["id_jugador_bronce"] != DBNull.Value ? Convert.ToInt32(dr["id_jugador_bronce"]) : 0,
                                NombreJugadorOro = dr["jugador_oro"]?.ToString() ?? string.Empty,
                                NombreJugadorPlata = dr["jugador_plata"]?.ToString() ?? string.Empty,
                                NombreJugadorBronce = dr["jugador_bronce"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }

            return listadoHistorial;
        }
    }
}