using UnityEngine;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System;

namespace TacticalEleven.Scripts
{
    public static class ManagerData
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

        // ----------------------------------------------------------------------------------- MÉTODO QUE GUARDA UN MANAGER
        public static void GuardarManagerEnDB(string nombre, string apellido, string nacionalidad, string fechaNacimiento)
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

                    string query = @"INSERT INTO managers (nombre, apellido, nacionalidad, fechaNacimiento)
                                     VALUES (@nombre, @apellido, @nacionalidad, @fechaNacimiento);";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@apellido", apellido);
                        cmd.Parameters.AddWithValue("@nacionalidad", nacionalidad);
                        cmd.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                Debug.Log($"Manager '{nombre} {apellido}' guardado correctamente en la base de datos.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // ------------------------------------------------------------------------- MÉTODO QUE MUESTRA LOS DATOS DEL MÁNAGER
        public static Manager MostrarManager()
        {
            Manager coach = null; // Inicializamos la variable como null

            try
            {
                // Usa la base activa (temporal si existe)
                string dbPath = DatabaseManager.GetActiveDatabasePath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                    return null;
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var connection = new SQLiteConnection(connString))
                {
                    connection.Open();

                    string query = @"SELECT * FROM managers WHERE id_manager = 1;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                coach = new Manager
                                {
                                    IdManager = reader.GetInt32(reader.GetOrdinal("id_manager")),
                                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                    Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                                    Nacionalidad = reader.GetString(reader.GetOrdinal("nacionalidad")),
                                    FechaNacimiento = reader.GetString(reader.GetOrdinal("fechaNacimiento")),
                                    IdEquipo = reader.IsDBNull(reader.GetOrdinal("id_equipo")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_equipo")),
                                    CDirectiva = reader.IsDBNull(reader.GetOrdinal("cDirectiva")) ? 0 : reader.GetInt32(reader.GetOrdinal("cDirectiva")),
                                    CFans = reader.IsDBNull(reader.GetOrdinal("cFans")) ? 0 : reader.GetInt32(reader.GetOrdinal("cFans")),
                                    CJugadores = reader.IsDBNull(reader.GetOrdinal("cJugadores")) ? 0 : reader.GetInt32(reader.GetOrdinal("cJugadores")),
                                    Reputacion = reader.IsDBNull(reader.GetOrdinal("reputacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("reputacion")),
                                    PartidosJugados = reader.IsDBNull(reader.GetOrdinal("partidosJugados")) ? 0 : reader.GetInt32(reader.GetOrdinal("partidosJugados")),
                                    PartidosGanados = reader.IsDBNull(reader.GetOrdinal("partidosGanados")) ? 0 : reader.GetInt32(reader.GetOrdinal("partidosGanados")),
                                    PartidosEmpatados = reader.IsDBNull(reader.GetOrdinal("partidosEmpatados")) ? 0 : reader.GetInt32(reader.GetOrdinal("partidosEmpatados")),
                                    PartidosPerdidos = reader.IsDBNull(reader.GetOrdinal("partidosPerdidos")) ? 0 : reader.GetInt32(reader.GetOrdinal("partidosPerdidos")),
                                    Puntos = reader.IsDBNull(reader.GetOrdinal("puntos")) ? 0 : reader.GetInt32(reader.GetOrdinal("puntos")),
                                    Tactica = reader.GetString(reader.GetOrdinal("tactica")),
                                    Despedido = reader.GetInt32(reader.GetOrdinal("despedido")),
                                    RutaImagen = reader["ruta_imagen"]?.ToString() ?? string.Empty,
                                    PrimeraTemporada = reader.GetInt32(reader.GetOrdinal("primera_temporada"))
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

            return coach;
        }

        // ----------------------------------------------------------------------------------- MÉTODO QUE ELIMINA UN MANAGER
        public static void EliminarManager()
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

                    string query = @"DELETE FROM managers WHERE id_manager = 1;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al eliminar de la base de datos: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------------- MÉTODO QUE AÑADE UN EQUIPO A UN MÁNAGER
        public static void AgregarEquipoSeleccionado(int idEquipo)
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

                    string query = @"UPDATE managers SET id_equipo = @IdEquipo WHERE id_manager = 1;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", idEquipo);
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

        // ------------------------------------------------------------------ MÉTODO QUE ACTUALIZA LA CONFIANZA EN EL MÁNAGER
        public static void ActualizarConfianza(int directiva, int fans, int jugadores)
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

                    string query = @"UPDATE managers SET " +
                                   "cDirectiva = MIN(MAX(cDirectiva + @Directiva, 0), 100), " +
                                   "cFans = MIN(MAX(cFans + @Fans, 0), 100), " +
                                   "cJugadores = MIN(MAX(cJugadores + @Jugadores, 0), 100) " +
                                   "WHERE id_manager = 1;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Directiva", directiva); // Parámetro para el cDirectiva
                        cmd.Parameters.AddWithValue("@Fans", fans); // Parámetro para el cFans
                        cmd.Parameters.AddWithValue("@Jugadores", jugadores); // Parámetro para el cJugadores
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

        // ------------------------------------------------------------------- MÉTODO QUE ACTUALIZA LA REPUTACIÓN DEL MÁNAGER
        public static void ActualizarReputacion(int valor)
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

                    string query = @"UPDATE managers SET " +
                                   "reputacion = MAX(reputacion + @Reputacion, 0) " +
                                   "WHERE id_manager = 1";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Reputacion", valor);
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

        // ------------------------------------------------------------------ MÉTODO QUE ACTUALIZA LOS PARTIDOS DE UN MÁNAGER
        public static void ActualizarResultadoManager(int idManager, int jugados, int ganados, int empatados, int perdidos, int puntos)
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

                    string query = @"UPDATE managers SET " +
                                   "partidosJugados = partidosJugados + @PartidosJugados, " +
                                   "partidosGanados = partidosGanados + @PartidosGanados, " +
                                   "partidosEmpatados = partidosEmpatados + @PartidosEmpatados, " +
                                   "partidosPerdidos = partidosPerdidos + @PartidosPerdidos, " +
                                   "puntos = puntos + @Puntos " +
                                   "WHERE id_manager = @IdManager;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@PartidosJugados", jugados);
                        cmd.Parameters.AddWithValue("@PartidosGanados", ganados);
                        cmd.Parameters.AddWithValue("@PartidosEmpatados", empatados);
                        cmd.Parameters.AddWithValue("@PartidosPerdidos", perdidos);
                        cmd.Parameters.AddWithValue("@Puntos", puntos);
                        cmd.Parameters.AddWithValue("@IdManager", idManager); // Parámetro para el idManager
                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al actualizar el mánager: {ex.Message}");
            }
        }

        // ------------------------------------------------------------- MÉTODO QUE ACTUALIZA LA TABLA HISTORIAL_MANAGER_TEMP
        public static void ActualizarManagerTemporal(Historial historial)
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

                    string query = @"UPDATE historial_manager_temp SET " +
                                   "partidosJugados = partidosJugados + @PartidosJugados, " +
                                   "partidosGanados = partidosGanados + @PartidosGanados, " +
                                   "partidosEmpatados = partidosEmpatados + @PartidosEmpatados, " +
                                   "partidosPerdidos = partidosPerdidos + @PartidosPerdidos, " +
                                   "golesMarcados = golesMarcados + @GolesMarcados, " +
                                   "golesRecibidos = golesRecibidos + @GolesRecibidos " +
                                   "WHERE id_historial = @IdHistorial;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdHistorial", 1);
                        cmd.Parameters.AddWithValue("@PartidosJugados", historial.PartidosJugados);
                        cmd.Parameters.AddWithValue("@PartidosGanados", historial.PartidosGanados);
                        cmd.Parameters.AddWithValue("@PartidosEmpatados", historial.PartidosEmpatados);
                        cmd.Parameters.AddWithValue("@PartidosPerdidos", historial.PartidosPerdidos);
                        cmd.Parameters.AddWithValue("@GolesMarcados", historial.GolesMarcados);
                        cmd.Parameters.AddWithValue("@GolesRecibidos", historial.GolesRecibidos);
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

        // ------------------------------------------------------------------------------------ MÉTODO QUE DESPIDE UN MANAGER
        public static void DespedirManager(int idManager)
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

                    string query = @"UPDATE managers SET despedido = 1 WHERE id_manager = @IdManager;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdManager", idManager);
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

        // -------------------------------------------------------------- MÉTODO QUE CAMBIA EL ESTADO DE LA PRIMERA TEMPORADA
        public static void ModificarPrimeraTemporada(int valor)
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

                    string query = @"UPDATE managers SET primera_temporada = 1 WHERE id_manager = @Valor;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Valor", valor);
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

        // ----------------------------------------------------------------------------------- MÉTODO QUE CAMBIA LA TÁCTICA DEL MÁNAGER
        public static void CambiarTactica(string tactica)
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

                    string query = @"UPDATE managers SET tactica = @Tactica";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@Tactica", tactica);
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