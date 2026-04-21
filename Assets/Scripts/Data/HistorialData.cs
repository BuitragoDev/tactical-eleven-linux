using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class HistorialData
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

        // ------------------------------------------------------------------- MÉTODO PARA INSERTAR UN NUEVO HISTORIAL DE MÁNAGER VACÍO
        public static void CrearLineaHistorial(int equipo, string temporada)
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
                using (var connection = new SQLiteConnection(connString))
                {
                    connection.Open();

                    string query = @"INSERT INTO historial_manager (id_equipo, temporada) 
                                     VALUES (@IdEquipo, @Temporada)";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);
                        cmd.Parameters.AddWithValue("@Temporada", temporada);
                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL HISTORIAL DEL MÁNAGER
        public static List<Historial> MostrarHistorialManager()
        {
            var dbPath = GetDBPath();
            List<Historial> listadoHistorial = new List<Historial>();

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
                    comando.CommandText = @"SELECT h.id_historial, h.id_equipo, h.temporada, h.posicionLiga, h.partidosJugados, 
                                                h.partidosGanados, h.partidosEmpatados, h.partidosPerdidos, h.golesMarcados, h.golesRecibidos, h.titulosInternacionales,
                                                e.nombre AS nombre_equipo, h.cDirectiva, h.cFans, h.cJugadores
                                            FROM historial_manager h
                                            JOIN equipos e 
                                                ON h.id_equipo = e.id_equipo
                                            ORDER BY id_historial ASC";

                    using (SQLiteDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listadoHistorial.Add(new Historial()
                            {
                                IdHistorial = dr.GetInt32(0),
                                IdEquipo = dr.GetInt32(1),
                                Temporada = dr.GetString(2),
                                PosicionLiga = dr.IsDBNull(3) ? 0 : dr.GetInt32(3),
                                PartidosJugados = dr.IsDBNull(4) ? 0 : dr.GetInt32(4),
                                PartidosGanados = dr.IsDBNull(5) ? 0 : dr.GetInt32(5),
                                PartidosEmpatados = dr.IsDBNull(6) ? 0 : dr.GetInt32(6),
                                PartidosPerdidos = dr.IsDBNull(7) ? 0 : dr.GetInt32(7),
                                GolesMarcados = dr.IsDBNull(8) ? 0 : dr.GetInt32(8),
                                GolesRecibidos = dr.IsDBNull(9) ? 0 : dr.GetInt32(9),
                                TitulosInternacionales = dr.IsDBNull(10) ? 0 : dr.GetInt32(10),
                                NombreEquipo = dr.IsDBNull(dr.GetOrdinal("nombre_equipo")) ? string.Empty : dr.GetString(dr.GetOrdinal("nombre_equipo")),
                                CDirectiva = dr.IsDBNull(12) ? 0 : dr.GetInt32(12),
                                CFans = dr.IsDBNull(13) ? 0 : dr.GetInt32(13),
                                CJugadores = dr.IsDBNull(14) ? 0 : dr.GetInt32(14)
                            });

                        }
                    }
                }
            }

            return listadoHistorial;
        }
    }
}