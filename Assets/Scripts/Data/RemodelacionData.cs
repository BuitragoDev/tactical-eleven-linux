#nullable enable

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class RemodelacionData
    {
        // --------------------------------------------------------------- MÉTODO QUE COMPRUEBA SI HAY UNA REMODELACIÓN EN MARCHA
        public static Remodelacion? ComprobarRemodelacion(int equipo)
        {
            Remodelacion? obra = null;

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

                    string query = @"SELECT id_remodelacion, fecha_inicio, fecha_final, tipo_remodelacion, id_equipo
                                     FROM remodelaciones 
                                     WHERE id_equipo = @IdEquipo";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read()) // Si encuentra un registro
                            {
                                obra = new Remodelacion
                                {
                                    IdRemodelacion = reader.GetInt32(reader.GetOrdinal("id_remodelacion")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                                    FechaFinal = reader.GetDateTime(reader.GetOrdinal("fecha_final")),
                                    TipoRemodelacion = reader.GetInt32(reader.GetOrdinal("tipo_remodelacion")),
                                    IdEquipo = reader.GetInt32(reader.GetOrdinal("id_equipo"))
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

            return obra;
        }

        // --------------------------------------------------------------- MÉTODO QUE CREA UNA NUEVA REMODELACIÓN
        public static void CrearNuevaRemodelacion(int equipo, DateTime fecha, int tipo)
        {
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

                    string query = @"INSERT INTO remodelaciones (fecha_inicio, fecha_final, tipo_remodelacion, id_equipo) 
                                     VALUES (@FechaInicio, @FechaFinal, @TipoRemodelacion, @IdEquipo)";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@FechaInicio", FechaData.hoy.ToString("yyyy-MM-dd"));
                        comando.Parameters.AddWithValue("@FechaFinal", fecha.ToString("yyyy-MM-dd"));
                        comando.Parameters.AddWithValue("@TipoRemodelacion", tipo);
                        comando.Parameters.AddWithValue("@IdEquipo", equipo);
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
    }
}