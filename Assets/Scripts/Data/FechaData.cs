using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class FechaData
    {
        #region "Variables GLOBALES"
        public static int temporadaActual;
        public static DateTime hoy;
        #endregion

        private static string GetDBPath()
        {
            string path = DatabaseManager.GetActiveDatabasePath();
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("No se ha establecido una base de datos activa en DatabaseManager.");
            }
            return path;
        }

        // ---------------------------------------------------------------------------- MÉTODO PARA INICIALIZAR TEMPORADA ACTUAL
        public void InicializarTemporadaActual()
        {
            Fecha fechaHoy = ObtenerFechaHoy();

            if (fechaHoy != null)
            {
                temporadaActual = fechaHoy.Anio; // Asignar el valor del año
                hoy = DateTime.Parse(fechaHoy.Hoy);
            }
            else
            {
                temporadaActual = 2026;
                hoy = DateTime.Parse("2026-07-15");
            }
        }

        // ----------------------------------------------------------------------- Método que devuelve la fecha de hoy
        public static Fecha ObtenerFechaHoy()
        {
            var dbPath = GetDBPath();

            Fecha fecha = null;

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            try
            {
                using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand("SELECT id_fecha, hoy, anio FROM fechas WHERE id_fecha = 1", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int idFecha = reader.GetInt32(0);
                            string hoy = reader.GetString(1);
                            int anio = reader.GetInt32(2);

                            fecha = new Fecha(idFecha, hoy, anio);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al obtener la fecha de hoy: {ex.Message}");
            }

            return fecha;
        }
    }
}