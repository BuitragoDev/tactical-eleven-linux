using System;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class DatabaseManager
    {
        // ------------------------------------------------------------- RUTAS PRINCIPALES
        public static string OriginalDbPath => Path.Combine(Application.streamingAssetsPath, Constants.DATABASE_NAME);
        public static string TempFolder => Path.Combine(Application.streamingAssetsPath, "Temp");
        public static string TempDbPath => Path.Combine(TempFolder, "tempGame.db");

        private static string activeDatabasePath;

        // ------------------------------------------------------------- CREAR BD TEMPORAL
        public static void CreateTempDatabase()
        {
            try
            {
                if (!Directory.Exists(TempFolder))
                    Directory.CreateDirectory(TempFolder);

                if (File.Exists(TempDbPath))
                    File.Delete(TempDbPath);

                File.Copy(OriginalDbPath, TempDbPath, true);
                Debug.Log($"Base de datos temporal creada en: {TempDbPath}");

                SetActiveDatabase(TempDbPath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear base de datos temporal: {ex.Message}");
            }
        }

        // ------------------------------------------------------------- ELIMINAR BD TEMPORAL
        public static void DeleteTempDatabase()
        {
            try
            {
                if (File.Exists(TempDbPath))
                {
                    File.Delete(TempDbPath);
                    Debug.Log("Base de datos temporal eliminada.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al eliminar base de datos temporal: {ex.Message}");
            }
        }

        // ------------------------------------------------------------- GUARDAR PARTIDA
        public static string SaveCurrentGame()
        {
            try
            {
                string currentDbPath = GetActiveDatabasePath();
                if (string.IsNullOrEmpty(currentDbPath) || !File.Exists(currentDbPath))
                {
                    Debug.LogError("No existe base de datos activa para guardar.");
                    return null;
                }

                string savesPath = Path.Combine(Application.persistentDataPath, "SavedGames");
                Directory.CreateDirectory(savesPath);

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string saveFileName = $"save_{timestamp}.db";
                string savePath = Path.Combine(savesPath, saveFileName);

                File.Copy(currentDbPath, savePath, true);
                Debug.Log($"Partida guardada en: {savePath}");

                return savePath;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar partida: {ex.Message}");
                return null;
            }
        }

        // ------------------------------------------------------------- ESTABLECER BD ACTIVA
        public static void SetActiveDatabase(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Debug.LogError($"No se puede establecer la base de datos activa. Ruta inválida: {path}");
                return;
            }

            activeDatabasePath = path;
            PlayerPrefs.SetString("ActiveDatabasePath", path);
            PlayerPrefs.Save();

            Debug.Log($"Base de datos activa establecida en: {path}");
        }

        // ------------------------------------------------------------- OBTENER BD ACTIVA
        public static string GetActiveDatabasePath()
        {
            // Si ya tenemos una ruta activa en memoria
            if (!string.IsNullOrEmpty(activeDatabasePath))
                return activeDatabasePath;

            // Intentar recuperar la última usada
            string savedPath = PlayerPrefs.GetString("ActiveDatabasePath", string.Empty);
            if (!string.IsNullOrEmpty(savedPath) && File.Exists(savedPath))
            {
                activeDatabasePath = savedPath;
                return activeDatabasePath;
            }

            // Si existe una base temporal, usarla
            if (File.Exists(TempDbPath))
                return TempDbPath;

            // Si no, usar la base original
            return OriginalDbPath;
        }

        // ------------------------------------------------------------- BORRAR TODO (UTILIDAD)
        public static void ResetActiveDatabase()
        {
            activeDatabasePath = null;
            PlayerPrefs.DeleteKey("ActiveDatabasePath");
            PlayerPrefs.Save();
            Debug.Log("Base de datos activa restablecida al estado original.");
        }
    }
}