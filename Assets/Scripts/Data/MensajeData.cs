using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class MensajeData
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

        // ----------------------------------------------------------------------------------- MÉTODO QUE CREA UN MENSAJE
        public static void CrearMensaje(Mensaje msg)
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

                    string query = @"INSERT INTO mensajes (fecha, remitente, asunto, contenido, tipo_mensaje, id_equipo, leido, icono)
                                     VALUES (@fecha, @remitente, @asunto, @contenido, @tipo_mensaje, @id_equipo, @leido, @icono)";

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@fecha", msg.Fecha);
                        comando.Parameters.AddWithValue("@remitente", msg.Remitente);
                        comando.Parameters.AddWithValue("@asunto", msg.Asunto);
                        comando.Parameters.AddWithValue("@contenido", msg.Contenido);
                        comando.Parameters.AddWithValue("@tipo_mensaje", msg.TipoMensaje);
                        comando.Parameters.AddWithValue("@id_equipo", msg.IdEquipo.HasValue ? (object)msg.IdEquipo.Value : DBNull.Value);
                        comando.Parameters.AddWithValue("@leido", msg.Leido ? 1 : 0); // Convertir bool a 0 o 1
                        comando.Parameters.AddWithValue("@icono", msg.Icono);
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

        // ---------------------------------------------------------------- MÉTODO QUE DEVUELVE TODOS LOS MENSAJES DE MI MÁNAGER
        public static List<Mensaje> MostrarMisMensajes()
        {
            var dbPath = GetDBPath();

            List<Mensaje> mensajes = new List<Mensaje>();

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
                    comando.CommandText = @"SELECT *
                                            FROM mensajes
                                            ORDER BY leido ASC, fecha";

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mensajes.Add(new Mensaje()
                            {
                                IdMensaje = reader.GetInt32(0), // id_mensaje
                                Fecha = reader.GetDateTime(1),  // fecha
                                Remitente = reader.GetString(2), // remitente
                                Asunto = reader.GetString(3),    // asunto
                                Contenido = reader.GetString(4), // contenido
                                TipoMensaje = reader.GetString(5), // tipo_mensaje
                                IdEquipo = reader.IsDBNull(6) ? null : reader.GetInt32(6), // id_equipo (nullable)
                                Leido = reader.GetBoolean(7), // leido
                                Icono = reader.GetInt32(8)
                            });
                        }
                    }
                }
            }

            return mensajes;
        }

        // ----------------------------------------------------------------------------------- MÉTODO QUE BORRA UN MENSAJE
        public static void BorrarMensaje(int idMensaje)
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

                    string query = @"DELETE FROM mensajes
                                     WHERE id_mensaje = @IdMensaje";

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdMensaje", idMensaje);
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

        // ----------------------------------------------------------------------------------- MÉTODO QUE MARCA UN MENSAJE COMO LEÍDO
        public static void MarcarComoLeido(int idMensaje)
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

                    string query = @"UPDATE mensajes SET leido = 1
                                     WHERE id_mensaje = @IdMensaje";

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdMensaje", idMensaje);
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