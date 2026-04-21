using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class EntrenadorData
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

        // ------------------------------------------------------------------------- MÉTODO QUE DEVUELVE LOS DATOS DE UN ENTRENADOR
        public static Entrenador MostrarEntrenador(int id)
        {
            var dbPath = GetDBPath();
            Entrenador entrenador = null;

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
                    comando.CommandText = @"SELECT id_entrenador, nombre, apellido, tactica_favorita, puntos, reputacion, id_equipo,
                                                   nombre || ' ' || apellido AS nombre_completo, ruta_imagen
                                            FROM entrenadores
                                            WHERE id_equipo = @idEquipo";

                    comando.Parameters.AddWithValue("@idEquipo", id);

                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Crear un nuevo objeto Jugador con los datos del jugador con mayor media
                            entrenador = new Entrenador
                            {
                                IdEntrenador = reader.GetInt32(reader.GetOrdinal("id_entrenador")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                                NombreCompleto = reader.GetString(reader.GetOrdinal("nombre_completo")),
                                TacticaFavorita = reader.GetString(reader.GetOrdinal("tactica_favorita")),
                                Puntos = reader.GetInt32(reader.GetOrdinal("puntos")),
                                Reputacion = reader.GetInt32(reader.GetOrdinal("reputacion")),
                                IdEquipo = reader.GetInt32(reader.GetOrdinal("id_equipo")),
                                RutaImagen = reader.GetString(reader.GetOrdinal("ruta_imagen"))
                            };
                        }
                    }
                }
            }

            return entrenador;
        }

        // ----------------------------------------------------------- MÉTODO PARA OBTENER EL RANKING DE ENTRENADORES
        public static List<Entrenador> ObtenerRankingEntrenadores()
        {
            List<Entrenador> rankingEntrenadores = new List<Entrenador>();

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

                    string query = @"SELECT 
                                        ROW_NUMBER() OVER (ORDER BY e.puntos DESC) AS posicion, 
                                        e.nombre || ' ' || e.apellido AS nombre_completo, 
                                        eq.nombre AS nombre_equipo, 
                                        e.reputacion, 
                                        e.puntos
                                     FROM 
                                        (SELECT id_entrenador, nombre, apellido, reputacion, puntos, id_equipo
                                        FROM entrenadores
                                        UNION
                                        SELECT id_manager AS id_entrenador, nombre, apellido, reputacion, puntos, id_equipo
                                        FROM managers
                                        WHERE id_manager = @miManager) e
                                     JOIN equipos eq ON e.id_equipo = eq.id_equipo
                                     GROUP BY e.nombre, e.apellido, e.id_equipo
                                     ORDER BY e.puntos DESC";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@miManager", 1);
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            // Leer los resultados y añadirlos a la lista
                            while (reader.Read())
                            {
                                Entrenador entrenador = new Entrenador
                                {
                                    Posicion = reader.GetInt32(0),
                                    NombreCompleto = reader.GetString(1),
                                    NombreEquipo = reader.GetString(2),
                                    Reputacion = reader.GetInt32(3),
                                    Puntos = reader.GetInt32(4)
                                };
                                rankingEntrenadores.Add(entrenador);
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

            return rankingEntrenadores;
        }
    }
}