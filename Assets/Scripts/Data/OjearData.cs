using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class OjearData
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

        // -------------------------------------------------------------------------- MÉTODO QUE DICE SI UN JUGADOR YA HA SIDO OJEADO
        public static bool ComprobarJugadorOjeado(int jugador)
        {
            bool ojeado = false;
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

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

                    string query = @"SELECT COUNT(*)  
                                     FROM ojear_jugadores
                                     WHERE id_jugador = @IdJugador";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);

                        // Ejecutar la consulta y obtener el resultado
                        int count = Convert.ToInt32(comando.ExecuteScalar());

                        // Si el resultado es mayor que 0, se encontró un registro
                        if (count > 0)
                        {
                            ojeado = true;
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return ojeado;
        }

        // ---------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DETALLADA DE LOS JUGADORES OJEADOS
        public static List<Jugador> ListadoJugadoresOjeados()
        {
            List<Jugador> listaJugadores = new List<Jugador>();

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

                    string query = @"SELECT j.*, o.fecha_informe, c.salario_anual, c.clausula_rescision, c.duracion
                                     FROM jugadores j
                                     INNER JOIN contratos c ON j.id_jugador = c.id_jugador
                                     INNER JOIN ojear_jugadores o ON j.id_jugador = o.id_jugador
                                     ORDER BY o.fecha_informe ASC";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Crear un objeto Jugador y asignar los valores de la base de datos
                                Jugador jugador = new Jugador
                                {
                                    IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador")),
                                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                    Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                                    IdEquipo = reader.GetInt32(reader.GetOrdinal("id_equipo")),
                                    RolId = reader.GetInt32(reader.GetOrdinal("rol_id")),
                                    Velocidad = reader.GetInt32(reader.GetOrdinal("velocidad")),
                                    Resistencia = reader.GetInt32(reader.GetOrdinal("resistencia")),
                                    Agresividad = reader.GetInt32(reader.GetOrdinal("agresividad")),
                                    Calidad = reader.GetInt32(reader.GetOrdinal("calidad")),
                                    EstadoForma = reader.GetInt32(reader.GetOrdinal("estado_forma")),
                                    Moral = reader.GetInt32(reader.GetOrdinal("moral")),
                                    FechaNacimiento = DateTime.Parse(reader.GetString(reader.GetOrdinal("fecha_nacimiento"))),
                                    AniosContrato = reader.IsDBNull(reader.GetOrdinal("duracion")) ? null : reader.GetInt32(reader.GetOrdinal("duracion")),
                                    SalarioTemporada = reader.IsDBNull(reader.GetOrdinal("salario_anual")) ? null : reader.GetInt32(reader.GetOrdinal("salario_anual")),
                                    ClausulaRescision = reader.IsDBNull(reader.GetOrdinal("clausula_rescision")) ? null : reader.GetInt32(reader.GetOrdinal("clausula_rescision")),
                                    Status = reader.GetInt32(reader.GetOrdinal("status")),
                                    FechaInforme = reader.GetString(reader.GetOrdinal("fecha_informe")),
                                    RutaImagen = reader.GetString(reader.GetOrdinal("ruta_imagen"))
                                };

                                // Agregar el jugador a la lista
                                listaJugadores.Add(jugador);
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

            return listaJugadores;
        }

        // ------------------------------------------------------------------- MÉTODO QUE PONE UN JUGADOR EN CARTERA
        public static void PonerJugadorCartera(int jugador, int dias)
        {
            var dbPath = GetDBPath();
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

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

                    using (SQLiteCommand comando = conexion.CreateCommand())
                    {
                        // Actualizar la posición del primer jugador
                        comando.CommandText = @"INSERT INTO ojear_jugadores (id_jugador, fecha_agregado, fecha_informe) 
                                                VALUES (@IdJugador, @FechaAgregado, @FechaInforme)";
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        comando.Parameters.AddWithValue("@FechaAgregado", FechaData.hoy.ToString("yyyy-MM-dd"));
                        comando.Parameters.AddWithValue("@FechaInforme", FechaData.hoy.AddDays(dias).ToString("yyyy-MM-dd"));
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

        // ------------------------------------------------------------------- MÉTODO QUE QUITA UN JUGADOR DE LA CARTERA
        public static void QuitarJugadorCartera(int jugador)
        {
            var dbPath = GetDBPath();
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

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

                    using (SQLiteCommand comando = conexion.CreateCommand())
                    {
                        // Actualizar la posición del primer jugador
                        comando.CommandText = @"DELETE FROM ojear_jugadores WHERE id_jugador = @IdJugador";
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
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

        // -------------------------------------------------------------------------- MÉTODO QUE DICE SI UN JUGADOR YA ESTÁ EN CARTERA
        public static bool ComprobarSiJugadorEnCartera(int jugador)
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

                    string query = @"SELECT COUNT(*) 
                                     FROM ojear_jugadores
                                     WHERE id_jugador = @IdJugador";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);

                        // Ejecutamos la consulta y obtenemos el número de coincidencias
                        int count = Convert.ToInt32(comando.ExecuteScalar());

                        // Si count > 0, significa que el jugador ya está siendo ojeado
                        encontrado = (count > 0);
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