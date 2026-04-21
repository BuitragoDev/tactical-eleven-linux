#nullable enable

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class FinanzaData
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

        // ----------------------------------------------------------------------------------- MÉTODO QUE CREA UN INGRESO
        public static void CrearIngreso(Finanza finanza)
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

                    string query = @"INSERT INTO finanzas (id_equipo, temporada, id_concepto, tipo, cantidad, fecha)
                                     VALUES (@IdEquipo, @Temporada, @IdConcepto, @Tipo, @Cantidad, @Fecha)";

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", finanza.IdEquipo);
                        comando.Parameters.AddWithValue("@Temporada", finanza.Temporada);
                        comando.Parameters.AddWithValue("@IdConcepto", finanza.IdConcepto);
                        comando.Parameters.AddWithValue("@Tipo", finanza.Tipo);
                        comando.Parameters.AddWithValue("@Cantidad", finanza.Cantidad);
                        comando.Parameters.AddWithValue("@Fecha", finanza.Fecha.ToString("yyyy-MM-dd"));
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

        // ----------------------------------------------------------------------------------- MÉTODO QUE CREA UN GASTO
        public static void CrearGasto(Finanza finanza)
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

                    string query = @"INSERT INTO finanzas (id_equipo, temporada, id_concepto, tipo, cantidad, fecha)
                                     VALUES (@IdEquipo, @Temporada, @IdConcepto, @Tipo, @Cantidad, @Fecha)";

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", finanza.IdEquipo);
                        comando.Parameters.AddWithValue("@Temporada", finanza.Temporada);
                        comando.Parameters.AddWithValue("@IdConcepto", finanza.IdConcepto);
                        comando.Parameters.AddWithValue("@Tipo", finanza.Tipo);
                        comando.Parameters.AddWithValue("@Cantidad", finanza.Cantidad);
                        comando.Parameters.AddWithValue("@Fecha", finanza.Fecha.ToString("yyyy-MM-dd"));
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

        // ----------------------------------------------------------- MÉTODO PARA MOSTRAR LAS FINANZAS DE UN EQUIPO
        public static List<Finanza> MostrarFinanzasEquipo(int temporada)
        {
            List<Finanza> finanzas = new List<Finanza>();

            try
            {
                var dbPath = GetDBPath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"SELECT id_finanza, id_equipo, temporada, id_concepto, tipo, cantidad, fecha
                                     FROM finanzas
                                     WHERE temporada = @Temporada";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Temporada", temporada);

                        using (SQLiteDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                // Crear y agregar cada objeto Finanza a la lista
                                finanzas.Add(new Finanza()
                                {
                                    IdFinanza = dr.GetInt32(0),
                                    IdEquipo = dr.GetInt32(1),
                                    Temporada = dr.GetString(2),
                                    IdConcepto = dr.GetInt32(3),
                                    Tipo = dr.GetInt32(4),
                                    Cantidad = dr.GetDouble(5),
                                    Fecha = DateTime.Parse(dr.GetString(6))
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

            return finanzas;
        }
    }
}