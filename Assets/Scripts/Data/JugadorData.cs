using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class JugadorData
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

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LOS TITULARES DEL EQUIPO
        public static void CrearAlineacion(int equipo)
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

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"SELECT id_jugador, rol_id,
                                           (velocidad + resistencia + agresividad + calidad + estado_forma + moral) / 6.0 as media
                                     FROM jugadores 
                                     WHERE id_equipo = @IdEquipo
                                     ORDER BY 
                                        CASE 
                                            WHEN rol_id = 1 THEN 1
                                            WHEN rol_id = 4 THEN 2
                                            WHEN rol_id = 2 THEN 3
                                            WHEN rol_id = 3 THEN 4
                                            WHEN rol_id BETWEEN 5 AND 7 THEN 5
                                            WHEN rol_id BETWEEN 8 AND 10 THEN 6
                                            ELSE 7 
                                        END,
                                        media DESC";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            List<(int id_jugador, int rol_id, double media)> jugadores = new();

                            while (reader.Read())
                            {
                                jugadores.Add((
                                    reader.GetInt32(0),
                                    reader.GetInt32(1),
                                    reader.GetDouble(2)
                                ));
                            }

                            if (jugadores.Count == 0) return;

                            // Agrupar por rol
                            var porteros = jugadores.Where(j => j.rol_id == 1).ToList();
                            var centrales = jugadores.Where(j => j.rol_id == 4).ToList();
                            var lateralDer = jugadores.Where(j => j.rol_id == 2).ToList();
                            var lateralIzq = jugadores.Where(j => j.rol_id == 3).ToList();
                            var mediocampistas = jugadores.Where(j => j.rol_id >= 5 && j.rol_id <= 7).ToList();
                            var delanteros = jugadores.Where(j => j.rol_id >= 8 && j.rol_id <= 10).ToList();

                            Dictionary<int, int> posiciones = new();
                            int posicion = 1;

                            if (porteros.Count >= 1) posiciones[posicion++] = porteros[0].id_jugador;
                            if (centrales.Count >= 2) { posiciones[posicion++] = centrales[0].id_jugador; posiciones[posicion++] = centrales[1].id_jugador; }
                            if (lateralDer.Count >= 1) posiciones[posicion++] = lateralDer[0].id_jugador;
                            if (lateralIzq.Count >= 1) posiciones[posicion++] = lateralIzq[0].id_jugador;

                            for (int i = 0; i < Math.Min(4, mediocampistas.Count); i++)
                                posiciones[posicion++] = mediocampistas[i].id_jugador;

                            for (int i = 0; i < Math.Min(2, delanteros.Count); i++)
                                posiciones[posicion++] = delanteros[i].id_jugador;

                            // Si hay menos de 11 jugadores asignados, completar con los mejores disponibles restantes
                            var usados = new HashSet<int>(posiciones.Values);
                            var restantes = jugadores.Where(j => !usados.Contains(j.id_jugador)).ToList();

                            while (posiciones.Count < 11 && restantes.Count > 0)
                            {
                                posiciones[posicion++] = restantes[0].id_jugador;
                                usados.Add(restantes[0].id_jugador);
                                restantes.RemoveAt(0);
                            }

                            // Insertar titulares
                            foreach (var kvp in posiciones)
                            {
                                string insertQuery = "INSERT INTO alineacion (id_jugador, posicion) VALUES (@id_jugador, @posicion)";
                                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conexion))
                                {
                                    insertCmd.Parameters.AddWithValue("@id_jugador", kvp.Value);
                                    insertCmd.Parameters.AddWithValue("@posicion", kvp.Key);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }

                            // Insertar suplentes
                            int pos = 12;
                            foreach (var suplente in jugadores.Where(j => !posiciones.Values.Contains(j.id_jugador)))
                            {
                                string insertQuery = "INSERT INTO alineacion (id_jugador, posicion) VALUES (@id_jugador, @posicion)";
                                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conexion))
                                {
                                    insertCmd.Parameters.AddWithValue("@id_jugador", suplente.id_jugador);
                                    insertCmd.Parameters.AddWithValue("@posicion", pos++);
                                    insertCmd.ExecuteNonQuery();
                                }
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
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LA MEDIA DE EQUIPO
        public static double ObtenerMediaEquipo(int idEquipo)
        {
            var dbPath = GetDBPath();
            double media = 0;

            try
            {
                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                    return 0;
                }

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"SELECT velocidad, resistencia, agresividad, calidad, estado_forma, moral 
                                     FROM jugadores 
                                     WHERE id_equipo = @IdEquipo";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", idEquipo);
                        using (var reader = comando.ExecuteReader())
                        {
                            var medias = new List<double>();

                            while (reader.Read())
                            {
                                int velocidad = reader.GetInt32(0);
                                int resistencia = reader.GetInt32(1);
                                int agresividad = reader.GetInt32(2);
                                int calidad = reader.GetInt32(3);
                                int estadoForma = reader.GetInt32(4);
                                int moral = reader.GetInt32(5);

                                double mediaJugador = (velocidad + resistencia + agresividad + calidad + estadoForma + moral) / 6.0;

                                medias.Add(mediaJugador);
                            }

                            if (medias.Any())
                            {
                                media = medias.Average();
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

            return media;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LOS DATOS DE UN JUGADOR
        public static Jugador MostrarDatosJugador(int id)
        {
            Jugador jugador = null;

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
                                        j.id_jugador,
                                        j.nombre,
                                        j.apellido,
                                        j.peso,
                                        j.altura,
                                        j.nacionalidad,
                                        j.dorsal,
                                        j.fecha_nacimiento,
                                        j.rol_id,
                                        j.rol,
                                        j.velocidad,
                                        j.resistencia,
                                        j.agresividad,
                                        j.calidad,
                                        j.estado_forma,
                                        j.moral,
                                        j.potencial,
                                        j.portero,
                                        j.pase,
                                        j.regate,
                                        j.remate,
                                        j.entradas,
                                        j.tiro,
                                        j.lesion,
                                        j.tipo_lesion,
                                        j.lesion_tratada,
                                        j.valor_mercado,
                                        j.estado_animo, 
                                        j.situacion_mercado,
                                        j.id_equipo,
                                        j.status,
                                        j.proxima_negociacion,
                                        j.ruta_imagen,
                                        c.duracion AS AniosContrato,
                                        c.salario_anual AS SalarioTemporada,
                                        c.clausula_rescision AS ClausulaRescision,
                                        c.bono_por_partidos AS BonoPartidos,
                                        c.bono_por_goles AS BonoGoles,
                                        e.nombre AS NombreEquipo
                                    FROM jugadores j
                                    LEFT JOIN equipos e ON e.id_equipo = j.id_equipo
                                    LEFT JOIN contratos c ON j.id_jugador = c.id_jugador
                                    WHERE j.id_jugador = @IdJugador";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", id);
                        using (SQLiteDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                // Crear un objeto Jugador y asignar los valores de la base de datos
                                jugador = new Jugador
                                {
                                    IdJugador = dr.GetInt32(dr.GetOrdinal("id_jugador")),
                                    Nombre = dr.GetString(dr.GetOrdinal("nombre")),
                                    Apellido = dr.GetString(dr.GetOrdinal("apellido")),
                                    Peso = dr.GetInt32(dr.GetOrdinal("peso")),
                                    Altura = dr.GetInt32(dr.GetOrdinal("altura")),
                                    Nacionalidad = dr.GetString(dr.GetOrdinal("nacionalidad")),
                                    Dorsal = dr.GetInt32(dr.GetOrdinal("dorsal")),
                                    FechaNacimiento = DateTime.Parse(dr.GetString(dr.GetOrdinal("fecha_nacimiento"))),
                                    RolId = dr.GetInt32(dr.GetOrdinal("rol_id")),
                                    Rol = dr.GetString(dr.GetOrdinal("rol")),
                                    Velocidad = dr.GetInt32(dr.GetOrdinal("velocidad")),
                                    Resistencia = dr.GetInt32(dr.GetOrdinal("resistencia")),
                                    Agresividad = dr.GetInt32(dr.GetOrdinal("agresividad")),
                                    Calidad = dr.GetInt32(dr.GetOrdinal("calidad")),
                                    EstadoForma = dr.GetInt32(dr.GetOrdinal("estado_forma")),
                                    Moral = dr.GetInt32(dr.GetOrdinal("moral")),
                                    Potencial = dr.GetInt32(dr.GetOrdinal("potencial")),
                                    Portero = dr.GetInt32(dr.GetOrdinal("portero")),
                                    Pase = dr.GetInt32(dr.GetOrdinal("pase")),
                                    Regate = dr.GetInt32(dr.GetOrdinal("regate")),
                                    Remate = dr.GetInt32(dr.GetOrdinal("remate")),
                                    Entradas = dr.GetInt32(dr.GetOrdinal("entradas")),
                                    Tiro = dr.GetInt32(dr.GetOrdinal("tiro")),
                                    Lesion = dr.GetInt32(dr.GetOrdinal("lesion")),
                                    TipoLesion = dr.IsDBNull(dr.GetOrdinal("tipo_lesion")) ? null : dr.GetString(dr.GetOrdinal("tipo_lesion")),
                                    LesionTratada = dr.GetInt32(dr.GetOrdinal("lesion_tratada")),
                                    ValorMercado = dr.GetInt32(dr.GetOrdinal("valor_mercado")),
                                    EstadoAnimo = dr.GetInt32(dr.GetOrdinal("estado_animo")),
                                    RutaImagen = dr.GetString(dr.GetOrdinal("ruta_imagen")),
                                    NombreEquipo = dr.GetString(dr.GetOrdinal("NombreEquipo")),
                                    SituacionMercado = dr.IsDBNull(dr.GetOrdinal("situacion_mercado")) ? 0 : dr.GetInt32(dr.GetOrdinal("situacion_mercado")),
                                    IdEquipo = dr.GetInt32(dr.GetOrdinal("id_equipo")),
                                    AniosContrato = dr.IsDBNull(dr.GetOrdinal("AniosContrato")) ? null : dr.GetInt32(dr.GetOrdinal("AniosContrato")),
                                    SalarioTemporada = dr.IsDBNull(dr.GetOrdinal("SalarioTemporada")) ? null : dr.GetInt32(dr.GetOrdinal("SalarioTemporada")),
                                    BonusPartido = dr.IsDBNull(dr.GetOrdinal("BonoPartidos")) ? null : dr.GetInt32(dr.GetOrdinal("BonoPartidos")),
                                    BonusGoles = dr.IsDBNull(dr.GetOrdinal("BonoGoles")) ? null : dr.GetInt32(dr.GetOrdinal("BonoGoles")),
                                    ClausulaRescision = dr.IsDBNull(dr.GetOrdinal("ClausulaRescision")) ? null : dr.GetInt32(dr.GetOrdinal("ClausulaRescision")),
                                    Status = dr.GetInt32(dr.GetOrdinal("status")),
                                    ProximaNegociacion = dr.IsDBNull(dr.GetOrdinal("proxima_negociacion")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("proxima_negociacion"))
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

            return jugador;
        }

        // ----------------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE JUGADORES DETALLADA POR EQUIPO
        public static List<Jugador> ListadoJugadoresCompleto(int id)
        {
            List<Jugador> jugadores = new List<Jugador>();

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

                    string query = @"SELECT j.*, c.duracion AS AniosContrato, c.salario_anual AS SalarioTemporada, c.clausula_rescision AS ClausulaRescision
                                     FROM jugadores j
                                     LEFT JOIN contratos c ON j.id_jugador = c.id_jugador
                                     WHERE j.id_equipo = @idEquipo
                                     ORDER BY j.rol_id ASC";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@idEquipo", id);
                        using (SQLiteDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                // Crear un objeto Jugador y asignar los valores de la base de datos
                                Jugador jugador = new Jugador
                                {
                                    IdJugador = dr.GetInt32(dr.GetOrdinal("id_jugador")),
                                    Nombre = dr.GetString(dr.GetOrdinal("nombre")),
                                    Apellido = dr.GetString(dr.GetOrdinal("apellido")),
                                    IdEquipo = dr.GetInt32(dr.GetOrdinal("id_equipo")),
                                    Dorsal = dr.GetInt32(dr.GetOrdinal("dorsal")),
                                    Rol = dr.GetString(dr.GetOrdinal("rol")),
                                    RolId = dr.GetInt32(dr.GetOrdinal("rol_id")),
                                    Velocidad = dr.GetInt32(dr.GetOrdinal("velocidad")),
                                    Resistencia = dr.GetInt32(dr.GetOrdinal("resistencia")),
                                    Agresividad = dr.GetInt32(dr.GetOrdinal("agresividad")),
                                    Calidad = dr.GetInt32(dr.GetOrdinal("calidad")),
                                    EstadoForma = dr.GetInt32(dr.GetOrdinal("estado_forma")),
                                    Moral = dr.GetInt32(dr.GetOrdinal("moral")),
                                    Potencial = dr.GetInt32(dr.GetOrdinal("potencial")),
                                    Portero = dr.GetInt32(dr.GetOrdinal("portero")),
                                    Pase = dr.GetInt32(dr.GetOrdinal("pase")),
                                    Regate = dr.GetInt32(dr.GetOrdinal("regate")),
                                    Remate = dr.GetInt32(dr.GetOrdinal("remate")),
                                    Entradas = dr.GetInt32(dr.GetOrdinal("entradas")),
                                    Tiro = dr.GetInt32(dr.GetOrdinal("tiro")),
                                    FechaNacimiento = DateTime.Parse(dr.GetString(dr.GetOrdinal("fecha_nacimiento"))),
                                    Peso = dr.GetInt32(dr.GetOrdinal("peso")),
                                    Altura = dr.GetInt32(dr.GetOrdinal("altura")),
                                    Lesion = dr.GetInt32(dr.GetOrdinal("lesion")),
                                    TipoLesion = dr.IsDBNull(dr.GetOrdinal("tipo_lesion")) ? null : dr.GetString(dr.GetOrdinal("tipo_lesion")),
                                    LesionTratada = dr.GetInt32(dr.GetOrdinal("lesion_tratada")),
                                    Nacionalidad = dr.GetString(dr.GetOrdinal("nacionalidad")),
                                    Status = dr.GetInt32(dr.GetOrdinal("status")),
                                    Sancionado = dr.GetInt32(dr.GetOrdinal("sancionado")),
                                    Entrenamiento = dr.GetInt32(dr.GetOrdinal("entrenamiento")),
                                    RutaImagen = dr.GetString(dr.GetOrdinal("ruta_imagen")),
                                    ValorMercado = dr.GetInt32(dr.GetOrdinal("valor_mercado")),
                                    AniosContrato = dr.IsDBNull(dr.GetOrdinal("AniosContrato")) ? null : dr.GetInt32(dr.GetOrdinal("AniosContrato")),
                                    SalarioTemporada = dr.IsDBNull(dr.GetOrdinal("SalarioTemporada")) ? null : dr.GetInt32(dr.GetOrdinal("SalarioTemporada")),
                                    ClausulaRescision = dr.IsDBNull(dr.GetOrdinal("ClausulaRescision")) ? null : dr.GetInt32(dr.GetOrdinal("ClausulaRescision"))
                                };

                                // Agregar el jugador a la lista
                                jugadores.Add(jugador);
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

            return jugadores;
        }

        // ------------------------------------------------------------------- MÉTODO QUE REDUCE UNA LESIÓN UN PORCENTAJE
        public static void TratarLesion(int idJugador, int porcentaje)
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

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    // Paso 1: Obtener el valor actual de la lesión
                    int lesionActual = 0;
                    string selectQuery = "SELECT lesion FROM jugadores WHERE id_jugador = @IdJugador";
                    using (SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, conexion))
                    {
                        selectCmd.Parameters.AddWithValue("@IdJugador", idJugador);
                        object result = selectCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            lesionActual = Convert.ToInt32(result);
                    }

                    // Paso 2: Calcular la nueva lesión
                    double factor = (100 - porcentaje) / 100.0;
                    int nuevaLesion = (int)Math.Round(lesionActual * factor);

                    // Paso 3: Actualizar el valor de lesión
                    string updateQuery = "UPDATE jugadores SET lesion = @NuevaLesion WHERE id_jugador = @IdJugador";
                    using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conexion))
                    {
                        updateCmd.Parameters.AddWithValue("@NuevaLesion", nuevaLesion);
                        updateCmd.Parameters.AddWithValue("@IdJugador", idJugador);
                        updateCmd.ExecuteNonQuery();
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // ------------------------------------------------------------------- MÉTODO QUE PONE AL JUGADOR EN TRATAMIENTO POR LESIÓN
        public static void ActivarTratamientoLesion(int jugador, int valor)
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

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    // Consulta SQL para obtener las finanzas del equipo
                    string query = @"UPDATE jugadores SET lesion_tratada = @Valor WHERE id_jugador = @IdJugador";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        // Agregar parámetro para evitar inyección SQL
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        comando.Parameters.AddWithValue("@Valor", valor);
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

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL ENTRENAMIENTO DE UN JUGADOR
        public static int EntrenamientoJugador(int jugador)
        {
            var dbPath = GetDBPath();
            int num = 0;

            try
            {
                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                    return 0;
                }

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"SELECT entrenamiento FROM jugadores WHERE id_jugador = @IdJugador";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        using (var reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                num = reader.GetInt32(0);
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

            return num;
        }

        // ------------------------------------------------------------------- MÉTODO QUE SELECCIONA UN ENTRENAMIENTO PARA UN JUGADOR
        public static void EntrenarJugador(int jugador, int tipo)
        {
            var dbPath = GetDBPath();

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

                    string query = @"UPDATE jugadores SET entrenamiento = @Entrenamiento
                                     WHERE id_jugador = @IdJugador";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        comando.Parameters.AddWithValue("@Entrenamiento", tipo);
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

        // ------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE JUGADORES DETALLADA POR EQUIPO ENTRE DOS POSICIONES
        public static List<Jugador> MostrarAlineacion(int inicio, int final)
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

                    string query = @"SELECT j.*, a.posicion
                                     FROM jugadores j
                                     JOIN alineacion a ON j.id_jugador = a.id_jugador 
                                     WHERE a.posicion >= @inicio AND a.posicion <= @final 
                                     ORDER BY a.posicion";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@inicio", inicio);
                        comando.Parameters.AddWithValue("@final", final);
                        using (SQLiteDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                // Crear un objeto Jugador y asignar los valores de la base de datos
                                Jugador jugador = new Jugador
                                {
                                    IdJugador = dr.GetInt32(dr.GetOrdinal("id_jugador")),
                                    Nombre = dr.GetString(dr.GetOrdinal("nombre")),
                                    Apellido = dr.GetString(dr.GetOrdinal("apellido")),
                                    IdEquipo = dr.GetInt32(dr.GetOrdinal("id_equipo")),
                                    Dorsal = dr.GetInt32(dr.GetOrdinal("dorsal")),
                                    Rol = dr.GetString(dr.GetOrdinal("rol")),
                                    RolId = dr.GetInt32(dr.GetOrdinal("rol_id")),
                                    Velocidad = dr.GetInt32(dr.GetOrdinal("velocidad")),
                                    Resistencia = dr.GetInt32(dr.GetOrdinal("resistencia")),
                                    Agresividad = dr.GetInt32(dr.GetOrdinal("agresividad")),
                                    Calidad = dr.GetInt32(dr.GetOrdinal("calidad")),
                                    EstadoForma = dr.GetInt32(dr.GetOrdinal("estado_forma")),
                                    Moral = dr.GetInt32(dr.GetOrdinal("moral")),
                                    Potencial = dr.GetInt32(dr.GetOrdinal("potencial")),
                                    Portero = dr.GetInt32(dr.GetOrdinal("portero")),
                                    Pase = dr.GetInt32(dr.GetOrdinal("pase")),
                                    Regate = dr.GetInt32(dr.GetOrdinal("regate")),
                                    Remate = dr.GetInt32(dr.GetOrdinal("remate")),
                                    Entradas = dr.GetInt32(dr.GetOrdinal("entradas")),
                                    Tiro = dr.GetInt32(dr.GetOrdinal("tiro")),
                                    FechaNacimiento = DateTime.Parse(dr.GetString(dr.GetOrdinal("fecha_nacimiento"))),
                                    Peso = dr.GetInt32(dr.GetOrdinal("peso")),
                                    Altura = dr.GetInt32(dr.GetOrdinal("altura")),
                                    Lesion = dr.GetInt32(dr.GetOrdinal("lesion")),
                                    TipoLesion = dr.IsDBNull(dr.GetOrdinal("tipo_lesion")) ? null : dr.GetString(dr.GetOrdinal("tipo_lesion")),
                                    LesionTratada = dr.GetInt32(dr.GetOrdinal("lesion_tratada")),
                                    Sancionado = dr.GetInt32(dr.GetOrdinal("sancionado")),
                                    Nacionalidad = dr.GetString(dr.GetOrdinal("nacionalidad")),
                                    Status = dr.GetInt32(dr.GetOrdinal("status")),
                                    PosicionAlineacion = dr.GetInt32(dr.GetOrdinal("posicion")),
                                    RutaImagen = dr.GetString(dr.GetOrdinal("ruta_imagen"))
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

        // ------------------------------------------------------------------- MÉTODO QUE CAMBIA LAS POSICIONES DE DOS JUGADORES
        public static void IntercambioPosicion(int jugador1, int jugador2, int posicion1, int posicion2)
        {
            var dbPath = GetDBPath();

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
                        comando.CommandText = @"UPDATE alineacion 
                                                    SET posicion = @PosicionDos
                                                    WHERE id_jugador = @IdJugadorUno";
                        comando.Parameters.AddWithValue("@IdJugadorUno", jugador1);
                        comando.Parameters.AddWithValue("@PosicionDos", posicion2);
                        comando.ExecuteNonQuery();

                        // Limpiar parámetros para la segunda consulta
                        comando.Parameters.Clear();

                        // Actualizar la posición del segundo jugador
                        comando.CommandText = @"UPDATE alineacion 
                                                    SET posicion = @PosicionUno
                                                    WHERE id_jugador = @IdJugadorDos";
                        comando.Parameters.AddWithValue("@IdJugadorDos", jugador2);
                        comando.Parameters.AddWithValue("@PosicionUno", posicion1);
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

        // ------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE JUGADORES EN MERCADO
        public static List<Jugador> ListadoJugadoresMercado(int equipo, int tipoStart, int tipoEnd)
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

                    string query = @"SELECT j.*, c.duracion AS AniosContrato, c.salario_anual AS SalarioTemporada, c.clausula_rescision AS ClausulaRescision
                                     FROM jugadores j
                                     LEFT JOIN contratos c ON j.id_jugador = c.id_jugador
                                     WHERE j.situacion_mercado BETWEEN @TipoStart AND @TipoEnd";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@Equipo", equipo);
                        comando.Parameters.AddWithValue("@TipoStart", tipoStart);
                        comando.Parameters.AddWithValue("@TipoEnd", tipoEnd);

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
                                    Peso = reader.GetInt32(reader.GetOrdinal("peso")),
                                    Altura = reader.GetInt32(reader.GetOrdinal("altura")),
                                    Nacionalidad = reader.GetString(reader.GetOrdinal("nacionalidad")),
                                    Dorsal = reader.GetInt32(reader.GetOrdinal("dorsal")),
                                    FechaNacimiento = DateTime.Parse(reader.GetString(reader.GetOrdinal("fecha_nacimiento"))),
                                    RolId = reader.GetInt32(reader.GetOrdinal("rol_id")),
                                    Rol = reader.GetString(reader.GetOrdinal("rol")),
                                    Velocidad = reader.GetInt32(reader.GetOrdinal("velocidad")),
                                    Resistencia = reader.GetInt32(reader.GetOrdinal("resistencia")),
                                    Agresividad = reader.GetInt32(reader.GetOrdinal("agresividad")),
                                    Calidad = reader.GetInt32(reader.GetOrdinal("calidad")),
                                    EstadoForma = reader.GetInt32(reader.GetOrdinal("estado_forma")),
                                    Moral = reader.GetInt32(reader.GetOrdinal("moral")),
                                    Potencial = reader.GetInt32(reader.GetOrdinal("potencial")),
                                    Portero = reader.GetInt32(reader.GetOrdinal("portero")),
                                    Pase = reader.GetInt32(reader.GetOrdinal("pase")),
                                    Regate = reader.GetInt32(reader.GetOrdinal("regate")),
                                    Remate = reader.GetInt32(reader.GetOrdinal("remate")),
                                    Entradas = reader.GetInt32(reader.GetOrdinal("entradas")),
                                    Tiro = reader.GetInt32(reader.GetOrdinal("tiro")),
                                    Lesion = reader.GetInt32(reader.GetOrdinal("lesion")),
                                    TipoLesion = reader.IsDBNull(reader.GetOrdinal("tipo_lesion")) ? null : reader.GetString(reader.GetOrdinal("tipo_lesion")),
                                    LesionTratada = reader.GetInt32(reader.GetOrdinal("lesion_tratada")),
                                    ValorMercado = reader.GetInt32(reader.GetOrdinal("valor_mercado")),
                                    EstadoAnimo = reader.GetInt32(reader.GetOrdinal("estado_animo")),
                                    RutaImagen = reader.GetString(reader.GetOrdinal("ruta_imagen")),
                                    SituacionMercado = reader.IsDBNull(reader.GetOrdinal("situacion_mercado")) ? 0 : reader.GetInt32(reader.GetOrdinal("situacion_mercado")),
                                    IdEquipo = reader.GetInt32(reader.GetOrdinal("id_equipo")),
                                    AniosContrato = reader.IsDBNull(reader.GetOrdinal("AniosContrato")) ? null : reader.GetInt32(reader.GetOrdinal("AniosContrato")),
                                    SalarioTemporada = reader.IsDBNull(reader.GetOrdinal("SalarioTemporada")) ? null : reader.GetInt32(reader.GetOrdinal("SalarioTemporada")),
                                    ClausulaRescision = reader.IsDBNull(reader.GetOrdinal("ClausulaRescision")) ? null : reader.GetInt32(reader.GetOrdinal("ClausulaRescision")),
                                    Status = reader.GetInt32(reader.GetOrdinal("status")),
                                    ProximaNegociacion = reader.IsDBNull(reader.GetOrdinal("proxima_negociacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("proxima_negociacion"))
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

        // ------------------------------------------------------------------- MÉTODO QUE DESPIDE UN JUGADOR DEL EQUIPO
        public static void DespedirJugador(int jugador)
        {
            var dbPath = GetDBPath();

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
                        comando.CommandText = @"UPDATE jugadores SET id_equipo = 0 WHERE id_jugador = @IdJugador";
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

        // ------------------------------------------------------------------- MÉTODO QUE ELIMINA UN CONTRATO DE UN JUGADOR
        public static void EliminarContratoJugador(int jugador)
        {
            var dbPath = GetDBPath();

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
                        comando.CommandText = @"DELETE FROM contratos WHERE id_jugador = @IdJugador";
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

        // ------------------------------------------------------------------- MÉTODO QUE PONER UN JUGADOR EN EL MERCADO
        public static void PonerJugadorEnMercado(int jugador, int opcion)
        {
            var dbPath = GetDBPath();

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
                        comando.CommandText = @"UPDATE jugadores SET situacion_mercado = @Opcion WHERE id_jugador = @IdJugador";
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        comando.Parameters.AddWithValue("@Opcion", opcion);
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

        // ------------------------------------------------------------------- MÉTODO QUE QUITA UN JUGADOR DEL MERCADO
        public static void QuitarJugadorDeMercado(int jugador)
        {
            var dbPath = GetDBPath();

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
                        comando.CommandText = @"UPDATE jugadores SET situacion_mercado = 0 WHERE id_jugador = @IdJugador";
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

        // ------------------------------------------------------------------------- MÉTODO QUE CREA UNA NUEVA FECHA DE NEGOCIACIÓN
        public static void NegociacionCancelada(int jugador, int dias)
        {
            DateTime fechaEnfado = FechaData.hoy.AddDays(dias);

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

                    string query = @"UPDATE jugadores SET proxima_negociacion = @FechaEnfado WHERE id_jugador = @IdJugador";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        comando.Parameters.AddWithValue("@FechaEnfado", fechaEnfado.ToString("yyyy-MM-dd"));
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

        // --------------------------------------------------------- MÉTODO QUE DEVUELVE EL SALARIO MEDIO DE LOS JUGADORES CON LA MISMA MEDIA
        public static int SalarioMedioJugadores(int jugador)
        {
            var dbPath = GetDBPath();
            int num = 0;

            try
            {
                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                    return 0;
                }

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"WITH media_objetivo AS (
                                        SELECT 
                                            (velocidad + resistencia + agresividad + calidad + estado_forma + moral) / 6.0 AS media
                                        FROM jugadores
                                        WHERE id_jugador = @IdJugador
                                     )
                                     SELECT 
                                        CAST((CAST(AVG(c.salario_anual) AS FLOAT) + 500) / 1000 AS INTEGER) * 1000 AS salario_medio
                                     FROM jugadores j
                                     JOIN contratos c ON j.id_jugador = c.id_jugador
                                     JOIN media_objetivo mo ON 
                                        ABS(((j.velocidad + j.resistencia + j.agresividad + j.calidad + j.estado_forma + j.moral) / 6.0) - mo.media) < 0.01";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        using (var reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                num = reader.GetInt32(0);
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

            return num;
        }

        // --------------------------------------------------------- MÉTODO QUE DEVUELVE LA CLAUSULA MEDIA DE LOS JUGADORES CON LA MISMA MEDIA
        public static int ClausulaMediaJugadores(int jugador)
        {
            var dbPath = GetDBPath();
            int num = 0;

            try
            {
                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                    return 0;
                }

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"WITH media_objetivo AS (
                                        SELECT 
                                            (velocidad + resistencia + agresividad + calidad + estado_forma + moral) / 6.0 AS media
                                        FROM jugadores
                                        WHERE id_jugador = @IdJugador
                                     )
                                     SELECT 
                                        CAST((CAST(AVG(c.clausula_rescision) AS FLOAT) + 500) / 1000 AS INTEGER) * 1000 AS clausula_media
                                     FROM jugadores j
                                     JOIN contratos c ON j.id_jugador = c.id_jugador
                                     JOIN media_objetivo mo ON 
                                        ABS(((j.velocidad + j.resistencia + j.agresividad + j.calidad + j.estado_forma + j.moral) / 6.0) - mo.media) < 0.01";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        using (var reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                num = reader.GetInt32(0);
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

            return num;
        }

        // ------------------------------------------------------------------------- MÉTODO QUE RENUEVA EL STATUS DE UN JUGADOR
        public static void RenovarStatusJugador(int jugador, int rol)
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

                    string query = @"UPDATE jugadores SET status = @Status WHERE id_jugador = @IdJugador";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        comando.Parameters.AddWithValue("@Status", rol);
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

        // ------------------------------------------------------------------------- MÉTODO QUE RENUEVA EL CONTRATO DE UN JUGADOR
        public static void RenovarContratoJugador(int jugador, int salario, int clausula, int anios, int bonusP, int bonusG)
        {
            FechaData fechaData = new FechaData();
            fechaData.InicializarTemporadaActual();
            DateTime nuevaFecha = new DateTime(FechaData.hoy.Year + anios, 6, 30);

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

                    string query = @"UPDATE contratos SET salario_anual = @Salario, 
                                                          clausula_rescision = @Clausula, 
                                                          duracion = @Anios, 
                                                          bono_por_partidos = @BonusP, 
                                                          bono_por_goles = @BonusG, 
                                                          fecha_inicio = @FechaInicio,
                                                          fecha_fin = @FechaFin
                                     WHERE id_jugador = @IdJugador";

                    using (var comando = new SQLiteCommand(query, connection))
                    {
                        comando.Parameters.AddWithValue("@IdJugador", jugador);
                        comando.Parameters.AddWithValue("@Salario", salario);
                        comando.Parameters.AddWithValue("@Clausula", clausula);
                        comando.Parameters.AddWithValue("@Anios", anios);
                        comando.Parameters.AddWithValue("@FechaInicio", FechaData.hoy.ToString("yyyy-MM-dd"));
                        comando.Parameters.AddWithValue("@FechaFin", nuevaFecha.ToString("yyyy-MM-dd"));
                        comando.Parameters.AddWithValue("@BonusP", bonusP);
                        comando.Parameters.AddWithValue("@BonusG", bonusG);
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