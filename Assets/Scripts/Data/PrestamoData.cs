#nullable enable

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{

    public class PrestamoData
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

        // ---------------------------------------------------------- MÉTODO PARA MOSTRAR LOS PRÉSTAMOS PEDIDOS
        public static List<Prestamo> MostrarPrestamos(int equipo)
        {
            List<Prestamo> prestamos = new List<Prestamo>();

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

                    string query = @"SELECT id_prestamo, orden_prestamo, fecha, capital, capital_restante, semanas, semanas_restantes, tasa_interes, pago_semanal, id_equipo
                                     FROM prestamos
                                     WHERE id_equipo = @IdEquipo";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Crear y agregar cada objeto Prestamo a la lista
                                prestamos.Add(new Prestamo()
                                {
                                    IdPrestamo = reader.GetInt32(0),
                                    Orden = reader.GetInt32(1),
                                    Fecha = reader.GetString(2),
                                    Capital = reader.GetInt32(3),
                                    CapitalRestante = reader.GetInt32(4),
                                    Semanas = reader.GetInt32(5),
                                    SemanasRestantes = reader.GetInt32(6),
                                    TasaInteres = reader.GetInt32(7),
                                    PagoSemanal = reader.GetInt32(8),
                                    IdEquipo = reader.GetInt32(9)
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

            return prestamos;
        }

        // ---------------------------------------------------------- MÉTODO PARA MOSTRAR EL ORDEN DE LOS PRÉSTAMOS
        public static List<int> OrdenPrestamos(int equipo)
        {
            List<int> orden = new List<int>();

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

                    string query = @"SELECT orden_prestamo
                                     FROM prestamos
                                     WHERE id_equipo = @IdEquipo
                                     ORDER BY orden_prestamo ASC";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (SQLiteDataReader reader = comando.ExecuteReader())  // Lee los resultados de la consulta
                        {
                            while (reader.Read())  // Recorre los resultados de la consulta
                            {
                                // Añadir cada valor de orden_prestamo a la lista
                                orden.Add(reader.GetInt32(0));  // Obtén el valor de la columna 'orden_prestamo'
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

            return orden;
        }

        // ---------------------------------------------------------- MÉTODO PARA CREAR UN PRÉSTAMO
        public static void AnadirPrestamo(Prestamo prestamo)
        {
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

                    string query = @"INSERT INTO prestamos (orden_prestamo, fecha, capital, capital_restante, semanas, semanas_restantes, tasa_interes, pago_semanal, id_equipo) 
                                     VALUES (@Orden, @Fecha, @Capital, @CapitalRestante, @Semanas, @SemanasRestantes, @TasaInteres, @PagoSemanal, @IdEquipo)";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Orden", prestamo.Orden);
                        comando.Parameters.AddWithValue("@Fecha", prestamo.Fecha);
                        comando.Parameters.AddWithValue("@Capital", prestamo.Capital);
                        comando.Parameters.AddWithValue("@CapitalRestante", prestamo.CapitalRestante);
                        comando.Parameters.AddWithValue("@Semanas", prestamo.Semanas);
                        comando.Parameters.AddWithValue("@SemanasRestantes", prestamo.SemanasRestantes);
                        comando.Parameters.AddWithValue("@TasaInteres", prestamo.TasaInteres);
                        comando.Parameters.AddWithValue("@PagoSemanal", prestamo.PagoSemanal);
                        comando.Parameters.AddWithValue("@IdEquipo", prestamo.IdEquipo);

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

        // ---------------------------------------------------------- MÉTODO PARA ELIMINAR UN PRÉSTAMO
        public static void EliminarPrestamo(int equipo, int orden)
        {
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

                    string query = @"DELETE FROM prestamos 
                                     WHERE id_equipo = @IdEquipo AND orden_prestamo = @Orden";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Orden", orden);
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);

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

        // ---------------------------------------------------------- MÉTODO PARA RESTAR UNA SEMANA A UN PRÉSTAMO
        public static void RestarSemana(int orden)
        {
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

                    string query = @"UPDATE prestamos SET semanas_restantes = semanas_restantes - 1 WHERE orden_prestamo = @Orden";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Orden", orden);
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