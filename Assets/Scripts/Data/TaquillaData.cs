#nullable enable

using System;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class TaquillaData
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

        // ------------------------------------------------------------------- MÉTODO QUE CREA UN REGISTRO PARA LA TAQUILLA DEL EQUIPO
        public static void GenerarTaquilla(int equipo)
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

                    string query = @"INSERT INTO taquilla (id_equipo) VALUES (@IdEquipo)";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);
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

        // ------------------------------------------------------------------- MÉTODO QUE CREA EL PRECIO DE LAS ENTRADAS
        public static void EstablecerPrecioEntradas(int equipo, int precioGeneral, int precioTribuna, int precioVip)
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

                    string query = @"UPDATE taquilla SET 
                                        precio_entrada_general = @PrecioGeneral, 
                                        precio_entrada_tribuna = @PrecioTribuna,
                                        precio_entrada_vip = @PrecioVip
                                     WHERE id_equipo = @IdEquipo";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);
                        comando.Parameters.AddWithValue("@PrecioGeneral", precioGeneral);
                        comando.Parameters.AddWithValue("@PrecioTribuna", precioTribuna);
                        comando.Parameters.AddWithValue("@PrecioVip", precioVip);
                        comando.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // ------------------------------------------------------------------- MÉTODO QUE CREA EL PRECIO DE LOS ABONOS
        public static void EstablecerPrecioAbonos(int equipo, int precioGeneral, int precioTribuna, int precioVip)
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

                    string query = @"UPDATE taquilla SET 
                                        precio_abono_general = @PrecioGeneral, 
                                        precio_abono_tribuna = @PrecioTribuna,
                                        precio_abono_vip = @PrecioVip
                                     WHERE id_equipo = @IdEquipo";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);
                        comando.Parameters.AddWithValue("@PrecioGeneral", precioGeneral);
                        comando.Parameters.AddWithValue("@PrecioTribuna", precioTribuna);
                        comando.Parameters.AddWithValue("@PrecioVip", precioVip);
                        comando.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // --------------------------------------------------------------- MÉTODO QUE COMPRUEBA SI SE HA ESTABLECIDO PRECIO DE ABONOS
        public static bool ComprobarAbono(int equipo)
        {
            bool verificado = false;

            try
            {
                // Usa la base activa (temporal si existe)
                string dbPath = DatabaseManager.GetActiveDatabasePath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var connection = new SQLiteConnection(connString))
                {
                    connection.Open();

                    string query = @"SELECT COUNT(*) FROM taquilla 
                                     WHERE id_equipo = @IdEquipo 
                                        AND (precio_abono_general <> 0
                                             OR precio_abono_tribuna <> 0
                                             OR precio_abono_vip <> 0)";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);

                        // Ejecutar la consulta y obtener el resultado
                        int count = Convert.ToInt32(comando.ExecuteScalar());

                        // Si el resultado es mayor que 0, se encontró un registro
                        if (count > 0)
                        {
                            verificado = true;
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return verificado;
        }

        // --------------------------------------------------------------- MÉTODO QUE DEVUELVE LOS PRECIOS DE LA TAQUILLA
        public static Taquilla? RecuperarPreciosTaquilla(int equipo)
        {
            Taquilla? taquilla = null;

            try
            {
                // Usa la base activa (temporal si existe)
                string dbPath = DatabaseManager.GetActiveDatabasePath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var connection = new SQLiteConnection(connString))
                {
                    connection.Open();

                    string query = @"SELECT id_precio, id_equipo, precio_entrada_general, 
                                            precio_entrada_tribuna, precio_entrada_vip,
                                            precio_abono_general, precio_abono_tribuna, 
                                            precio_abono_vip, abonos_vendidos 
                                     FROM taquilla 
                                     WHERE id_equipo = @IdEquipo";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read()) // Si encuentra un registro
                            {
                                taquilla = new Taquilla
                                {
                                    IdPrecio = reader.GetInt32(reader.GetOrdinal("id_precio")),
                                    IdEquipo = reader.GetInt32(reader.GetOrdinal("id_equipo")),
                                    PrecioEntradaGeneral = reader.IsDBNull(reader.GetOrdinal("precio_entrada_general")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("precio_entrada_general")),
                                    PrecioEntradaTribuna = reader.IsDBNull(reader.GetOrdinal("precio_entrada_tribuna")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("precio_entrada_tribuna")),
                                    PrecioEntradaVip = reader.IsDBNull(reader.GetOrdinal("precio_entrada_vip")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("precio_entrada_vip")),
                                    PrecioAbonoGeneral = reader.IsDBNull(reader.GetOrdinal("precio_abono_general")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("precio_abono_general")),
                                    PrecioAbonoTribuna = reader.IsDBNull(reader.GetOrdinal("precio_abono_tribuna")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("precio_abono_tribuna")),
                                    PrecioAbonoVip = reader.IsDBNull(reader.GetOrdinal("precio_abono_vip")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("precio_abono_vip")),
                                    AbonosVendidos = reader.IsDBNull(reader.GetOrdinal("abonos_vendidos")) ? 0 : reader.GetInt32(reader.GetOrdinal("abonos_vendidos"))
                                };

                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return taquilla;
        }
    }
}