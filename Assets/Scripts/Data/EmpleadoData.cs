#nullable enable

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class EmpleadoData
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

        // ----------------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE EMPLEADOS DISPONIBLES
        public static List<Empleado> MostrarListaEmpleadosDisponibles(int puesto)
        {
            List<Empleado> empleados = new List<Empleado>();

            string[] posicion = {
                "Segundo Entrenador",
                "Delegado",
                "Director Técnico",
                "Preparador Físico",
                "Psicólogo",
                "Financiero",
                "Equipo Médico",
                "Encargado Campo"
            };
            string nombrePuesto = "";

            if (puesto >= 1 && puesto <= 8)
            {
                nombrePuesto = posicion[puesto - 1];
            }

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

                    string query = @"SELECT id_empleadoDisponible, nombre, puesto, categoria, salario, id_equipo
                                     FROM empleados_disponibles 
                                     WHERE puesto = @Puesto
                                     ORDER BY RANDOM()
                                     LIMIT 5";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Puesto", nombrePuesto);
                        using (SQLiteDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                empleados.Add(new Empleado()
                                {
                                    IdEmpleado = dr.GetInt32(0), // Obtenemos el Id como entero
                                    Nombre = dr.GetString(1), // Nombre es un string
                                    Puesto = dr.GetString(2), // Puesto es un string
                                    Categoria = dr.GetInt32(3), // Categoria es un entero
                                    Salario = dr.GetInt32(4), // Salario es un entero
                                    IdEquipo = dr.IsDBNull(5) ? (int?)null : dr.GetInt32(5)
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

            return empleados;
        }

        // ----------------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE EMPLEADOS CONTRATADOS
        public static List<Empleado> MostrarListaEmpleadosContratados(int equipo)
        {
            List<Empleado> empleados = new List<Empleado>();

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

                    string query = @"SELECT id_empleado, nombre, puesto, categoria, salario, id_equipo
                                     FROM empleados WHERE id_equipo = @Equipo";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Equipo", equipo);
                        using (SQLiteDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                empleados.Add(new Empleado()
                                {
                                    IdEmpleado = dr.GetInt32(0), // Obtenemos el Id como entero
                                    Nombre = dr.GetString(1), // Nombre es un string
                                    Puesto = dr.GetString(2), // Puesto es un string
                                    Categoria = dr.GetInt32(3), // Categoria es un entero
                                    Salario = dr.GetInt32(4), // Salario es un entero
                                    IdEquipo = dr.IsDBNull(5) ? (int?)null : dr.GetInt32(5)
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

            return empleados;
        }

        // ----------------------------------------------------------- MÉTODO QUE DEVUELVE UN EMPLEADO POR SU PUESTO
        public static Empleado? ObtenerEmpleadoPorPuesto(string tarea)
        {
            Empleado? empleado = null;

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

                    string query = @"SELECT * FROM empleados WHERE puesto = @Tarea LIMIT 1";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Tarea", tarea);
                        using (SQLiteDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read()) // Si hay resultados, creamos el objeto
                            {
                                empleado = new Empleado
                                {
                                    IdEmpleado = dr.GetInt32(0),
                                    Nombre = dr.IsDBNull(1) ? null : dr.GetString(1),
                                    Puesto = dr.IsDBNull(2) ? null : dr.GetString(2),
                                    Categoria = dr.GetInt32(3),
                                    Salario = dr.GetInt32(4),
                                    IdEquipo = dr.IsDBNull(5) ? null : dr.GetInt32(5)
                                };
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

            return empleado;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LA CATEGORIA DE UN EMPLEADO
        public static Empleado? MostrarCategoriaEmpleado(string nombre)
        {
            var dbPath = GetDBPath();
            Empleado? empleado = null;

            try
            {
                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"SELECT categoria, puesto
                                     FROM empleados WHERE nombre = @Nombre";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        using (SQLiteDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                empleado = new Empleado()
                                {
                                    Categoria = dr.GetInt32(0),
                                    Puesto = dr.GetString(1),
                                };
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

            return empleado;
        }

        // -------------------------------------------------------------- MÉTODO QUE CAMBIA EL CAMPO CONTRATADO DE 0 A 1
        public static void FicharEmpleado(int equipo, int idEmpleado)
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

                    // Consulta para obtener los datos del empleado desde empleados_disponibles
                    string querySelect = @"SELECT nombre, puesto, categoria, salario, id_equipo
                                           FROM empleados_disponibles
                                           WHERE id_empleadoDisponible = @IdEmpleado";

                    using (SQLiteCommand cmdSelect = new SQLiteCommand(querySelect, conexion))
                    {
                        cmdSelect.Parameters.AddWithValue("@IdEmpleado", idEmpleado);

                        using (SQLiteDataReader dr = cmdSelect.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                // Obtener los datos del empleado
                                string nombre = dr.GetString(0);
                                string puesto = dr.GetString(1);
                                int categoria = dr.GetInt32(2);
                                int salario = dr.GetInt32(3);
                                int? idEquipoDisponible = dr.IsDBNull(4) ? (int?)null : dr.GetInt32(4);

                                // Insertar los datos del empleado en la tabla empleados
                                string queryInsert = @"INSERT INTO empleados (nombre, puesto, categoria, salario, id_equipo)
                                                        VALUES (@Nombre, @Puesto, @Categoria, @Salario, @IdEquipo)";

                                using (SQLiteCommand cmdInsert = new SQLiteCommand(queryInsert, conexion))
                                {
                                    cmdInsert.Parameters.AddWithValue("@Nombre", nombre);
                                    cmdInsert.Parameters.AddWithValue("@Puesto", puesto);
                                    cmdInsert.Parameters.AddWithValue("@Categoria", categoria);
                                    cmdInsert.Parameters.AddWithValue("@Salario", salario);
                                    cmdInsert.Parameters.AddWithValue("@IdEquipo", equipo);

                                    // Ejecutar el insert
                                    cmdInsert.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Empleado no encontrado en empleados_disponibles.");
                            }
                        }

                        conexion.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------------------------- MÉTODO QUE DESPIDE UN EMPLEADO
        public static void DespedirEmpleado(string puesto)
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

                    string query = @"DELETE FROM empleados WHERE puesto = @Puesto";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Puesto", puesto);
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

        // ----------------------------------------------------------- MÉTODO PARA CALCULAR SALARIO TOTAL EMPLEADOS
        public static int SalarioTotalEmpleados(int equipo)
        {
            int total = 0;

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

                    string query = @"SELECT salario
                                     FROM empleados 
                                     WHERE id_equipo = @Equipo";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Equipo", equipo);

                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                total += reader.GetInt32(0);
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

            return total;
        }

        // ------------------------------------------------------------------ MÉTODO QUE DEVUELVE TRUE SI HAY UN EMPLEADO ENCONTRADO
        public static bool EmpleadoEncontrado(string tarea)
        {
            bool encontrado = false;

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

                    string query = @"SELECT COUNT(*) FROM empleados WHERE puesto = @Tarea";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@Tarea", tarea);

                        // Ejecutamos la consulta y obtenemos el número de coincidencias
                        int count = Convert.ToInt32(comando.ExecuteScalar());

                        // Si el count es mayor que 0, significa que encontramos un empleado
                        if (count > 0)
                        {
                            encontrado = true;
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return encontrado;
        }
    }
}