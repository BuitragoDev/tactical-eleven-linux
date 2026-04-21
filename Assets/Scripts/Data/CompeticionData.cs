using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class CompeticionData
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

        // ------------------------------------------------------------------------------ MÉTODO QUE MUESTRA EL NOMBRE DE UNA COMPETICIÓN
        public static string MostrarNombreCompeticion(int competicion)
        {
            var dbPath = GetDBPath();

            string nombreComp = "";

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return "";
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();
                comando.CommandText = @"SELECT nombre, id_competicion
                                        FROM competiciones
                                        WHERE id_competicion = @IdCompeticion";

                comando.Parameters.AddWithValue("@IdCompeticion", competicion);

                using (SQLiteDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read()) // Si encuentra un registro
                    {
                        nombreComp = reader["nombre"]?.ToString() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
                return "";
            }

            return nombreComp;
        }
    }
}