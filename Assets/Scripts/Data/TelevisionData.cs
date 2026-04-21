#nullable enable

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class TelevisionData
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

        // ---------------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE TELEVISIONES
        public static List<Television> MostrarListaTelevisiones()
        {
            var dbPath = GetDBPath();

            List<Television> televisiones = new List<Television>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT id_cadenatv, nombre, reputacion
                                            FROM cadenastv";

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            televisiones.Add(new Television()
                            {
                                IdTelevision = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Reputacion = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }

            return televisiones;
        }

        // ---------------------------------------------------------- MÉTODO PARA MOSTRAR LOS TELEVISIONES CONTRATADAS
        public static Television? TelevisionesContratadas(int equipo)
        {
            var dbPath = GetDBPath();

            Television? oTelevision = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT 
                                                ca.id_cadenatv, 
                                                ca.nombre, 
                                                ca.reputacion, 
                                                c.cantidad, 
                                                c.mensualidad,
                                                c.duracion_contrato
                                            FROM 
                                                cadenastv AS ca
                                            INNER JOIN 
                                                contratos_cadenastv AS c 
                                            ON 
                                                ca.id_cadenatv = c.id_cadenatv
                                            WHERE 
                                                c.id_equipo = @IdEquipo
                                            LIMIT 1";

                    comando.Parameters.AddWithValue("@IdEquipo", equipo);

                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Crear y agregar cada objeto Finanza a la lista
                            oTelevision = new Television
                            {
                                IdTelevision = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Reputacion = reader.GetInt32(2),
                                Cantidad = reader.GetInt32(3),
                                Mensualidad = reader.GetInt32(4),
                                DuracionContrato = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }

            return oTelevision;
        }

        // ---------------------------------------------------------- MÉTODO QUE DEVUELVE EL NOMBRE DE UNA TELEVISIÓN
        public static string? NombreTelevision(int television)
        {
            var dbPath = GetDBPath();
            string nombre = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT nombre FROM cadenastv WHERE id_cadenatv = @IdCadena";

                    comando.Parameters.AddWithValue("@IdCadena", television);

                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nombre = reader["nombre"].ToString();
                        }
                    }
                }
            }

            return nombre;
        }

        // ----------------------------------------------------------------------------------- MÉTODO QUE AÑADE UN PATROCINADOR
        public static void AnadirUnaTelevision(int television, int cantidad, int mensualidad, int duracion, int equipo)
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

                    string query = @"INSERT INTO contratos_cadenastv (id_cadenatv, id_equipo, cantidad, mensualidad, duracion_contrato) 
                                     VALUES (@IdCadenaTV, @IdEquipo, @Cantidad, @Mensualidad, @Duracion)";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdCadenaTV", television);
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);
                        cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                        cmd.Parameters.AddWithValue("@Mensualidad", mensualidad);
                        cmd.Parameters.AddWithValue("@Duracion", duracion);

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
    }
}