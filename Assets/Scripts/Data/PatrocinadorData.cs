#nullable enable

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{

    public class PatrocinadorData
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

        // ---------------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE PATROCINADORES
        public static List<Patrocinador> MostrarListaPatrocinadores()
        {
            var dbPath = GetDBPath();

            List<Patrocinador> patrocinadores = new List<Patrocinador>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT id_patrocinador, nombre, reputacion
                                            FROM patrocinadores";

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patrocinadores.Add(new Patrocinador()
                            {
                                IdPatrocinador = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Reputacion = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }

            return patrocinadores;
        }

        // ---------------------------------------------------------- MÉTODO PARA MOSTRAR LOS PATROCINADORES CONTRATADOS
        public static Patrocinador? PatrocinadoresContratados(int equipo)
        {
            var dbPath = GetDBPath();

            Patrocinador? oPatrocinador = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT p.id_patrocinador, p.nombre, p.reputacion, c.cantidad, c.mensualidad, c.duracion_contrato
                                            FROM patrocinadores p
                                            INNER JOIN contratos_patrocinador c ON p.id_patrocinador = c.id_patrocinador
                                            WHERE c.id_equipo = @IdEquipo
                                            LIMIT 1";

                    comando.Parameters.AddWithValue("@IdEquipo", equipo);

                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Crear y agregar cada objeto Finanza a la lista
                            oPatrocinador = new Patrocinador
                            {
                                IdPatrocinador = reader.GetInt32(0),
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

            return oPatrocinador;
        }

        // ---------------------------------------------------------- MÉTODO QUE DEVUELVE EL NOMBRE DE UN PATROCINADOR
        public static string? NombrePatrocinador(int patrocinador)
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
                    comando.CommandText = @"SELECT nombre FROM patrocinadores WHERE id_patrocinador = @IdPatrocinador";

                    comando.Parameters.AddWithValue("@IdPatrocinador", patrocinador);

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
        public static void AnadirUnPatrocinador(int patrocinador, int cantidad, int mensualidad, int duracion, int equipo)
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

                    string query = @"INSERT INTO contratos_patrocinador (id_patrocinador, id_equipo, cantidad, mensualidad, duracion_contrato) 
                                     VALUES (@IdPatrocinador, @IdEquipo, @Cantidad, @Mensualidad, @Duracion)";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdPatrocinador", patrocinador);
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