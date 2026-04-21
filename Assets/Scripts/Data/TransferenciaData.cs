using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class TransferenciaData
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

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE TRASPASOS Y CESIONES
        public static List<Transferencia> ListarTraspasos()
        {
            List<Transferencia> transferencias = new List<Transferencia>();

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

                    string query = @"SELECT * FROM transferencias ORDER BY fecha_traspaso ASC";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transferencias.Add(new Transferencia()
                                {
                                    // Usamos el operador de coalescencia nula para evitar la asignación de null
                                    IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador")),
                                    IdEquipoOrigen = reader.GetInt32(reader.GetOrdinal("id_equipo_origen")),
                                    IdEquipoDestino = reader.GetInt32(reader.GetOrdinal("id_equipo_destino")),
                                    TipoFichaje = reader.GetInt32(reader.GetOrdinal("tipo_fichaje")),
                                    FechaOferta = reader["fecha_oferta"]?.ToString() ?? string.Empty,
                                    FechaTraspaso = reader["fecha_traspaso"]?.ToString() ?? string.Empty,
                                    RespuestaEquipo = reader.IsDBNull(reader.GetOrdinal("respuesta_equipo"))
                                                        ? (int?)null
                                                        : reader.GetInt32(reader.GetOrdinal("respuesta_equipo")),
                                    RespuestaJugador = reader.IsDBNull(reader.GetOrdinal("respuesta_jugador"))
                                                        ? (int?)null
                                                        : reader.GetInt32(reader.GetOrdinal("respuesta_jugador")),
                                    MontoOferta = reader.GetInt32(reader.GetOrdinal("monto_oferta")),
                                    SalarioAnual = reader.GetInt32(reader.GetOrdinal("salario_anual")),
                                    ClausulaRescision = reader.GetInt32(reader.GetOrdinal("clausula_rescision")),
                                    Duracion = reader.GetInt32(reader.GetOrdinal("duracion")),
                                    BonoPorGoles = reader.GetInt32(reader.GetOrdinal("bono_por_goles")),
                                    BonoPorPartidos = reader.GetInt32(reader.GetOrdinal("bono_por_partidos")),
                                });
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

            return transferencias;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE OFERTAS SIN FINALIZAR
        public static List<Transferencia> ListarOfertasSinFinalizar()
        {
            List<Transferencia> transferencias = new List<Transferencia>();

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

                    string query = @"SELECT * FROM ofertas WHERE respuesta_jugador = 0";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transferencias.Add(new Transferencia()
                                {
                                    // Usamos el operador de coalescencia nula para evitar la asignación de null
                                    IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador")),
                                    IdEquipoOrigen = reader.GetInt32(reader.GetOrdinal("id_equipo_origen")),
                                    IdEquipoDestino = reader.GetInt32(reader.GetOrdinal("id_equipo_destino")),
                                    TipoFichaje = reader.GetInt32(reader.GetOrdinal("tipo_fichaje")),
                                    FechaOferta = reader["fecha_oferta"]?.ToString() ?? string.Empty,
                                    FechaTraspaso = reader["fecha_traspaso"]?.ToString() ?? string.Empty,
                                    RespuestaEquipo = reader.IsDBNull(reader.GetOrdinal("respuesta_equipo"))
                                                        ? (int?)null
                                                        : reader.GetInt32(reader.GetOrdinal("respuesta_equipo")),
                                    RespuestaJugador = reader.IsDBNull(reader.GetOrdinal("respuesta_jugador"))
                                                        ? (int?)null
                                                        : reader.GetInt32(reader.GetOrdinal("respuesta_jugador")),
                                    MontoOferta = reader.GetInt32(reader.GetOrdinal("monto_oferta")),
                                    SalarioAnual = reader.GetInt32(reader.GetOrdinal("salario_anual")),
                                    ClausulaRescision = reader.GetInt32(reader.GetOrdinal("clausula_rescision")),
                                    Duracion = reader.GetInt32(reader.GetOrdinal("duracion")),
                                    BonoPorGoles = reader.GetInt32(reader.GetOrdinal("bono_por_goles")),
                                    BonoPorPartidos = reader.GetInt32(reader.GetOrdinal("bono_por_partidos")),
                                });
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

            return transferencias;
        }

        // ----------------------------------------------------------------------------------- MÉTODO QUE BORRA UNA OFERTA POR UN JUGADOR
        public static void BorrarOferta(int jugador)
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

                    string query = @"DELETE FROM ofertas WHERE id_jugador = @IdJugador";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdJugador", jugador);
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

        // ------------------------------------------------------------------- MÉTODO QUE EVALUA UNA OFERTA POR UN JUGADOR
        public static Transferencia EvaluarOfertaEquipo(int idJugador, int idEquipoComprador, int montoOferta, int tipoFichaje)
        {
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            Transferencia oferta = new Transferencia();

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

                    string query = @"SELECT j.valor_mercado, j.status, j.situacion_mercado, j.moral, j.estado_animo, 
                                            c.fecha_fin, c.clausula_rescision, c.id_equipo AS equipoActual,
                                            e.presupuesto AS presupuestoEquipoVendedor, e.rival,
                                            (SELECT presupuesto FROM equipos WHERE id_equipo = @idEquipoComprador) AS presupuestoEquipoComprador
                                     FROM jugadores j 
                                     JOIN contratos c ON j.id_jugador = c.id_jugador 
                                     JOIN equipos e ON e.id_equipo = c.id_equipo
                                     WHERE j.id_jugador = @idJugador";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@idJugador", idJugador);
                        comando.Parameters.AddWithValue("@idEquipoComprador", idEquipoComprador);

                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oferta = new Transferencia()
                                {
                                    IdJugador = idJugador,
                                    IdEquipoOrigen = reader.GetInt32(reader.GetOrdinal("equipoActual")),
                                    IdEquipoDestino = idEquipoComprador,
                                    ClausulaRescision = reader.GetInt32(reader.GetOrdinal("clausula_rescision")),
                                    ValorMercado = reader.GetInt32(reader.GetOrdinal("valor_mercado")),
                                    SituacionMercado = reader.GetInt32(reader.GetOrdinal("situacion_mercado")),
                                    Moral = reader.GetInt32(reader.GetOrdinal("moral")),
                                    EstadoAnimo = reader.GetInt32(reader.GetOrdinal("estado_animo")),
                                    FinContrato = reader.GetString(reader.GetOrdinal("fecha_fin")),
                                    PresupuestoComprador = reader.GetInt32(reader.GetOrdinal("presupuestoEquipoComprador")),
                                    PresupuestoVendedor = reader.GetInt32(reader.GetOrdinal("presupuestoEquipoVendedor")),
                                    Rival = reader.GetInt32(reader.GetOrdinal("rival")),
                                    MontoOferta = montoOferta,
                                    TipoFichaje = tipoFichaje,
                                    Status = reader.GetInt32(reader.GetOrdinal("status")),
                                    FechaOferta = FechaData.hoy.ToString("yyyy-MM-dd")
                                };
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

            return oferta;
        }

        // ----------------------------------------- MÉTODO QUE COMPRUEBA SI EL JUGADOR YA TIENE UNA OFERTA ACEPTADA POR EL EQUIPO
        public static bool ComprobarOfertaActiva(int idJugador, int idEquipoComprador, int idEquipoVendedor)
        {
            bool ofertaAceptada = false;

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

                    string query = @"SELECT * 
                                     FROM ofertas 
                                     WHERE id_jugador = @idJugador 
                                        AND id_equipo_origen = @idEquipoVendedor 
                                        AND id_equipo_destino = @idEquipoComprador";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@idJugador", idJugador);
                        comando.Parameters.AddWithValue("@idEquipoComprador", idEquipoComprador);
                        comando.Parameters.AddWithValue("@idEquipoVendedor", idEquipoVendedor);

                        // Ejecutamos la consulta y obtenemos el número de coincidencias
                        int count = Convert.ToInt32(comando.ExecuteScalar());
                        ofertaAceptada = (count > 0);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return ofertaAceptada;
        }

        // ----------------------------------------------------------------------------------- MÉTODO QUE REGISTRA UNA OFERTA POR UN JUGADOR
        public static void RegistrarOferta(Transferencia oferta)
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

                    string query = @"INSERT INTO ofertas (
                                        id_jugador, id_equipo_origen, id_equipo_destino, tipo_fichaje, 
                                        monto_oferta, fecha_oferta, fecha_traspaso, respuesta_equipo, respuesta_jugador,
                                        salario_anual, clausula_rescision, duracion, bono_por_goles, bono_por_partidos
                                     ) 
                                     VALUES (
                                        @idJugador, @idEquipoOrigen, @idEquipoDestino, @tipoFichaje, 
                                        @montoOferta, @fechaOferta, @fechaTraspaso, @RespuestaEquipo, @RespuestaJugador,
                                        @salario, @clausula, @duracion, @bonoGoles, @bonoPartidos
                                     )";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@idJugador", oferta.IdJugador);
                        comando.Parameters.AddWithValue("@idEquipoOrigen", oferta.IdEquipoOrigen);
                        comando.Parameters.AddWithValue("@idEquipoDestino", oferta.IdEquipoDestino);
                        comando.Parameters.AddWithValue("@tipoFichaje", oferta.TipoFichaje);
                        comando.Parameters.AddWithValue("@montoOferta", oferta.MontoOferta);
                        comando.Parameters.AddWithValue("@fechaOferta", oferta.FechaOferta);
                        comando.Parameters.AddWithValue("@fechaTraspaso", oferta.FechaTraspaso);
                        comando.Parameters.AddWithValue("@RespuestaEquipo", oferta.RespuestaEquipo);
                        comando.Parameters.AddWithValue("@RespuestaJugador", oferta.RespuestaJugador);
                        comando.Parameters.AddWithValue("@salario", oferta.SalarioAnual);
                        comando.Parameters.AddWithValue("@clausula", oferta.ClausulaRescision);
                        comando.Parameters.AddWithValue("@duracion", oferta.Duracion);
                        comando.Parameters.AddWithValue("@bonoGoles", oferta.BonoPorGoles);
                        comando.Parameters.AddWithValue("@bonoPartidos", oferta.BonoPorPartidos);
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

        // ----------------------------------------------------------------------------------- MÉTODO QUE REGISTRA UNA OFERTA POR UN JUGADOR
        public static void ActualizarOferta(Transferencia oferta)
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

                    string query = @"UPDATE ofertas 
                                     SET monto_oferta = @montoOferta, 
                                         fecha_oferta = @fechaOferta, 
                                         fecha_traspaso = @fechaTraspaso,
                                         salario_anual = @salario,
                                         clausula_rescision = @clausula,
                                         duracion = @duracion,
                                         bono_por_goles = @bonoGoles,
                                         bono_por_partidos = @bonoPartidos,
                                         respuesta_equipo = @RespuestaEquipo,
                                         respuesta_jugador = @RespuestaJugador
                                     WHERE id_jugador = @idJugador 
                                       AND id_equipo_origen = @idEquipoOrigen 
                                       AND id_equipo_destino = @idEquipoDestino 
                                       AND tipo_fichaje = @tipoFichaje";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@idJugador", oferta.IdJugador);
                        comando.Parameters.AddWithValue("@idEquipoOrigen", oferta.IdEquipoOrigen);
                        comando.Parameters.AddWithValue("@idEquipoDestino", oferta.IdEquipoDestino);
                        comando.Parameters.AddWithValue("@tipoFichaje", oferta.TipoFichaje);
                        comando.Parameters.AddWithValue("@montoOferta", oferta.MontoOferta);
                        comando.Parameters.AddWithValue("@fechaOferta", oferta.FechaOferta);
                        comando.Parameters.AddWithValue("@fechaTraspaso", oferta.FechaTraspaso);
                        comando.Parameters.AddWithValue("@RespuestaEquipo", oferta.RespuestaEquipo);
                        comando.Parameters.AddWithValue("@RespuestaJugador", oferta.RespuestaJugador);
                        comando.Parameters.AddWithValue("@salario", oferta.SalarioAnual);
                        comando.Parameters.AddWithValue("@clausula", oferta.ClausulaRescision);
                        comando.Parameters.AddWithValue("@duracion", oferta.Duracion);
                        comando.Parameters.AddWithValue("@bonoGoles", oferta.BonoPorGoles);
                        comando.Parameters.AddWithValue("@bonoPartidos", oferta.BonoPorPartidos);
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

        // ------------------------------------------------------------------------- MÉTODO QUE REGISTRA UNA TRANSFERENCIA POR UN JUGADOR
        public static void RegistrarTransferencia(Transferencia transfer)
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

                    string query = @"INSERT INTO transferencias (
                                        id_jugador, id_equipo_origen, id_equipo_destino, tipo_fichaje, 
                                        monto_oferta, fecha_oferta, fecha_traspaso, respuesta_equipo, respuesta_jugador,
                                        salario_anual, clausula_rescision, duracion, bono_por_goles, bono_por_partidos
                                     ) 
                                     VALUES (
                                        @idJugador, @idEquipoOrigen, @idEquipoDestino, @tipoFichaje, 
                                        @montoOferta, @fechaOferta, @fechaTraspaso, @RespuestaEquipo, @RespuestaJugador,
                                        @salario, @clausula, @duracion, @bonoGoles, @bonoPartidos
                                     )";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@idJugador", transfer.IdJugador);
                        comando.Parameters.AddWithValue("@idEquipoOrigen", transfer.IdEquipoOrigen);
                        comando.Parameters.AddWithValue("@idEquipoDestino", transfer.IdEquipoDestino);
                        comando.Parameters.AddWithValue("@tipoFichaje", transfer.TipoFichaje);
                        comando.Parameters.AddWithValue("@montoOferta", transfer.MontoOferta);
                        comando.Parameters.AddWithValue("@fechaOferta", transfer.FechaOferta);
                        comando.Parameters.AddWithValue("@fechaTraspaso", transfer.FechaTraspaso);
                        comando.Parameters.AddWithValue("@RespuestaEquipo", transfer.RespuestaEquipo);
                        comando.Parameters.AddWithValue("@RespuestaJugador", transfer.RespuestaJugador);
                        comando.Parameters.AddWithValue("@salario", transfer.SalarioAnual);
                        comando.Parameters.AddWithValue("@clausula", transfer.ClausulaRescision);
                        comando.Parameters.AddWithValue("@duracion", transfer.Duracion);
                        comando.Parameters.AddWithValue("@bonoGoles", transfer.BonoPorGoles);
                        comando.Parameters.AddWithValue("@bonoPartidos", transfer.BonoPorPartidos);
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

        // ----------------------------------------- MÉTODO QUE COMPRUEBA SI EL EQUIPO YA HA ACEPTADO O RECHAZADO UNA OFERTA POR EL JUGADOR
        public static int ComprobarRespuestaEquipo(int idJugador, int idEquipoComprador, int idEquipoVendedor)
        {
            int? respuestaEquipo = null;

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

                    string query = @"SELECT respuesta_equipo 
                                     FROM ofertas 
                                     WHERE id_jugador = @idJugador 
                                        AND id_equipo_origen = @idEquipoVendedor 
                                        AND id_equipo_destino = @idEquipoComprador
                                        AND tipo_fichaje = 1";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@idJugador", idJugador);
                        comando.Parameters.AddWithValue("@idEquipoComprador", idEquipoComprador);
                        comando.Parameters.AddWithValue("@idEquipoVendedor", idEquipoVendedor);

                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                respuestaEquipo = reader.GetInt32(0);
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

            return respuestaEquipo ?? -1;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LOS DETALLES DE LA OFERTA DE UN JUGADOR
        public static Transferencia MostrarDetallesOferta(int jugador)
        {
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();

            Transferencia transferencia = null;

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

                    string query = @"SELECT * FROM ofertas WHERE id_jugador = @IdJugador ORDER BY fecha_oferta DESC LIMIT 1";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);

                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                transferencia = new Transferencia()
                                {
                                    IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador")),
                                    IdEquipoOrigen = reader.GetInt32(reader.GetOrdinal("id_equipo_origen")),
                                    IdEquipoDestino = reader.GetInt32(reader.GetOrdinal("id_equipo_destino")),
                                    TipoFichaje = reader.GetInt32(reader.GetOrdinal("tipo_fichaje")),
                                    FechaOferta = reader["fecha_oferta"]?.ToString() ?? string.Empty,
                                    FechaTraspaso = reader["fecha_traspaso"]?.ToString() ?? string.Empty,
                                    RespuestaEquipo = reader.IsDBNull(reader.GetOrdinal("respuesta_equipo"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("respuesta_equipo")),
                                    RespuestaJugador = reader.IsDBNull(reader.GetOrdinal("respuesta_jugador"))
                                        ? (int?)null
                                        : reader.GetInt32(reader.GetOrdinal("respuesta_jugador")),
                                    MontoOferta = reader.GetInt32(reader.GetOrdinal("monto_oferta")),
                                    SalarioAnual = reader.GetInt32(reader.GetOrdinal("salario_anual")),
                                    ClausulaRescision = reader.GetInt32(reader.GetOrdinal("clausula_rescision")),
                                    Duracion = reader.GetInt32(reader.GetOrdinal("duracion")),
                                    BonoPorGoles = reader.GetInt32(reader.GetOrdinal("bono_por_goles")),
                                    BonoPorPartidos = reader.GetInt32(reader.GetOrdinal("bono_por_partidos")),
                                };
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

            return transferencia;
        }
    }
}