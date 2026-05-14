using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using MySql.Data.MySqlClient;
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlJugadorRepository : RepositoryBase, IJugadorRepository
    {
        // ---------------------------------------------------------
        // MÉTODOS DE LECTURA Y VALIDACIÓN (EXISTENTES)
        // ---------------------------------------------------------

        public IEnumerable<JugadorModel> GetAll()
        {
            var jugadorList = new List<JugadorModel>();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "sp_ObtenerTodosJugadores";
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var jugador = new JugadorModel
                        {
                            Id_jugador = Convert.ToInt32(reader["id_jugador"]),
                            Nombre = reader["nombre"].ToString(),
                            Apellido = reader["apellido"].ToString(),

                            Apodo = reader["apodo"] != DBNull.Value ? reader["apodo"].ToString() : "",
                            Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : "",
                            Dorsal = Convert.ToInt32(reader["dorsal"]),
                            Posicion = reader["posicion"].ToString(),
                            Afinidad = reader["afinidad"].ToString(),

                            // --- ¡AQUÍ FALTABAN ESTAS LÍNEAS! ---
                            // Asegúrate de que los nombres "es_titular" y "esta_convocado" 
                            // coinciden EXACTAMENTE con los de tu tabla en MySQL.
                            Es_titular = reader["es_titular"] != DBNull.Value && Convert.ToBoolean(reader["es_titular"]),
                            Esta_convocado = reader["esta_convocado"] != DBNull.Value && Convert.ToBoolean(reader["esta_convocado"]),
                            // ------------------------------------

                            Url_imagen = reader["url_imagen"] != DBNull.Value ? reader["url_imagen"].ToString() : "/Images/Players/default.png",
                            UrlImagenResponsable = reader["foto_mister"] != DBNull.Value ? reader["foto_mister"].ToString() : "/Images/Staff/default_coach.png",
                            Id_equipo = reader["id_equipo"] != DBNull.Value ? Convert.ToInt32(reader["id_equipo"]) : 0,
                            Clausula_rescision = reader["clausula_rescision"] != DBNull.Value ? Convert.ToDecimal(reader["clausula_rescision"]) : 0m,
                            Esta_disponible = reader["esta_disponible"] != DBNull.Value && Convert.ToBoolean(reader["esta_disponible"]),
                            Sueldo = reader["sueldo"] != DBNull.Value ? Convert.ToDecimal(reader["sueldo"]) : 0m
                        };

                        jugadorList.Add(jugador);
                    }
                }
            }
            return jugadorList;
        }

        public bool ValidateJugador(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ValidarJugador";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_username", MySqlDbType.VarChar).Value = credential.UserName;
                command.Parameters.Add("@p_password", MySqlDbType.VarChar).Value = credential.Password;
                
                command.Parameters.Add(new MySqlParameter("@p_es_valido", MySqlDbType.Bit));
                command.Parameters["@p_es_valido"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                validUser = Convert.ToBoolean(command.Parameters["@p_es_valido"].Value);
            }
            return validUser;
        }

        public JugadorModel GetJugadorByUsername(string username)
        {
            JugadorModel jugador = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerJugadorPorUsername";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_username", MySqlDbType.VarChar).Value = username;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Usamos el método de mapeo específico para jugadores
                        jugador = MapReaderToJugador(reader);
                    }
                }
            }
            return jugador;
        }

        private JugadorModel MapReaderToJugador(MySqlDataReader reader)
        {
            return new JugadorModel()
            {
                // Ajusta estos nombres a los de tus columnas en MySQL
                Id_jugador = reader.IsDBNull(reader.GetOrdinal("Id_jugador")) ? 0 : reader.GetInt32("Id_Jugador"),
                Username = reader.IsDBNull(reader.GetOrdinal("username")) ? string.Empty : reader.GetString("username"),
                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString("Nombre"),
                Apellido = reader.IsDBNull(reader.GetOrdinal("Apellido")) ? string.Empty : reader.GetString("Apellido"),
                Dorsal = reader.IsDBNull(reader.GetOrdinal("Dorsal")) ? 0 : reader.GetInt32("Dorsal"),
                Posicion = reader.IsDBNull(reader.GetOrdinal("Posicion")) ? string.Empty : reader.GetString("Posicion"),
                Afinidad = reader.IsDBNull(reader.GetOrdinal("Afinidad")) ? string.Empty : reader.GetString("Afinidad"),

                // Importante para que salga la foto en el menú
                Url_imagen = reader.IsDBNull(reader.GetOrdinal("Url_imagen")) ? null : reader.GetString("Url_imagen"),
                Id_equipo = reader.IsDBNull(reader.GetOrdinal("id_equipo")) ? 0 : reader.GetInt32("id_equipo"),
                Clausula_rescision = reader.IsDBNull(reader.GetOrdinal("clausula_rescision")) ? 0m : reader.GetDecimal("clausula_rescision"),
                Esta_disponible = !reader.IsDBNull(reader.GetOrdinal("esta_disponible")) && reader.GetBoolean("esta_disponible"),
                Sueldo = reader.IsDBNull(reader.GetOrdinal("sueldo")) ? 0m : reader.GetDecimal("sueldo")
            };
        }

        public void UpdateJugador(JugadorModel jugador)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ActualizarJugador";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_nombre", MySqlDbType.VarChar).Value = jugador.Nombre;
                command.Parameters.Add("@p_apellido", MySqlDbType.VarChar).Value = jugador.Apellido;
                command.Parameters.Add("@p_apodo", MySqlDbType.VarChar).Value = jugador.Apodo;
                command.Parameters.Add("@p_email", MySqlDbType.VarChar).Value = jugador.Email;
                command.Parameters.Add("@p_dorsal", MySqlDbType.Int32).Value = jugador.Dorsal;
                command.Parameters.Add("@p_posicion", MySqlDbType.VarChar).Value = jugador.Posicion;
                command.Parameters.Add("@p_afinidad", MySqlDbType.VarChar).Value = jugador.Afinidad;
                command.Parameters.Add("@p_titular", MySqlDbType.Bit).Value = jugador.Es_titular; 
                command.Parameters.Add("@p_convocado", MySqlDbType.Bit).Value = jugador.Esta_convocado; 
                command.Parameters.Add("@p_sueldo", MySqlDbType.Decimal).Value = jugador.Sueldo; 
                command.Parameters.Add("@p_clausula", MySqlDbType.Decimal).Value = jugador.Clausula_rescision;
                command.Parameters.Add("@p_disponible", MySqlDbType.Bit).Value = jugador.Esta_disponible;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = jugador.Id_jugador;

                command.ExecuteNonQuery();
            }
        }

        public void AddJugador(JugadorModel jugador)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "sp_InsertarJugador";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_nombre", MySqlDbType.VarChar).Value = jugador.Nombre;
                command.Parameters.Add("@p_apellido", MySqlDbType.VarChar).Value = jugador.Apellido;
                command.Parameters.Add("@p_apodo", MySqlDbType.VarChar).Value = jugador.Apodo;
                command.Parameters.Add("@p_username", MySqlDbType.VarChar).Value = jugador.Username;
                command.Parameters.Add("@p_password", MySqlDbType.VarChar).Value = jugador.Password;
                command.Parameters.Add("@p_email", MySqlDbType.VarChar).Value = jugador.Email;
                command.Parameters.Add("@p_dorsal", MySqlDbType.Int32).Value = jugador.Dorsal;
                command.Parameters.Add("@p_posicion", MySqlDbType.VarChar).Value = jugador.Posicion;
                command.Parameters.Add("@p_afinidad", MySqlDbType.VarChar).Value = jugador.Afinidad;
                command.Parameters.Add("@p_url", MySqlDbType.VarChar).Value = jugador.Url_imagen ?? "/Images/Players/default.png";
                command.Parameters.Add("@p_sueldo", MySqlDbType.Decimal).Value = jugador.Sueldo;

                command.ExecuteNonQuery();
            }
        }

        // ---------------------------------------------------------
        // MÉTODOS DE SUPERTÉCNICAS (EXISTENTES)
        // ---------------------------------------------------------

        public List<SuperTecnicaModel> GetTecnicasDeJugador(int idJugador)
        {
            var lista = new List<SuperTecnicaModel>();
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerTecnicasDeJugador";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = idJugador;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new SuperTecnicaModel
                        {
                            Id_SuperTecnica = Convert.ToInt32(reader["id_tecnica"]),
                            Nombre_SuperTecnica = reader["nombre"].ToString(),
                            Tipo_SuperTecnica = reader["tipo"].ToString(),
                            Afinidad_SuperTecnica = reader["afinidad"].ToString(),
                            Potencia = Convert.ToInt32(reader["potencia"])
                        });
                    }
                }
            }
            return lista;
        }

        public List<SuperTecnicaModel> GetCatalogoCompleto()
        {
            var lista = new List<SuperTecnicaModel>();
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerTodasSupertecnicas";
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new SuperTecnicaModel
                        {
                            Id_SuperTecnica = Convert.ToInt32(reader["id_tecnica"]),
                            Nombre_SuperTecnica = reader["nombre"].ToString(),
                            Tipo_SuperTecnica = reader["tipo"].ToString(),
                            Afinidad_SuperTecnica = reader["afinidad"].ToString(),
                            Potencia = reader["potencia"] != DBNull.Value ? Convert.ToInt32(reader["potencia"]) : 0
                        });
                    }
                }
            }
            return lista;
        }

        public void AsignarTecnica(int idJugador, int idTecnica)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_AsignarTecnicaJugador";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id_jugador", MySqlDbType.Int32).Value = idJugador;
                command.Parameters.Add("@p_id_tecnica", MySqlDbType.Int32).Value = idTecnica;
                command.ExecuteNonQuery();
            }
        }

        public void RetirarTecnica(int idJugador, int idTecnica)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_RetirarTecnicaJugador";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id_jugador", MySqlDbType.Int32).Value = idJugador;
                command.Parameters.Add("@p_id_tecnica", MySqlDbType.Int32).Value = idTecnica;
                command.ExecuteNonQuery();
            }
        }

        // ---------------------------------------------------------
        // NUEVOS MÉTODOS PARA GESTIÓN DE PLANTILLA
        // ---------------------------------------------------------

        public void IntercambiarJugadores(int idJugador1, int idJugador2)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Usamos el Stored Procedure que definimos anteriormente para consistencia transaccional
                command.CommandText = "sp_IntercambiarJugadores";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_id1", idJugador1);
                command.Parameters.AddWithValue("@p_id2", idJugador2);

                command.ExecuteNonQuery();
            }
        }

        public void ActualizarEstadoJugador(int idJugador, string nuevaPosicion, bool esTitular, bool estaConvocado)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Usamos el Stored Procedure para actualizar todo de golpe
                command.CommandText = "sp_ActualizarEstadoJugador";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_id", idJugador);
                command.Parameters.AddWithValue("@p_posicion", nuevaPosicion);
                command.Parameters.AddWithValue("@p_titular", esTitular);
                command.Parameters.AddWithValue("@p_convocado", estaConvocado);

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<JugadorModel> GetByTeam()
        {
            var jugadores = new List<JugadorModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "sp_ObtenerJugadoresPorEquipo";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_equipo", UserSession.Id_Equipo);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var jugador = new JugadorModel(
                                Convert.ToInt32(reader["id_jugador"]),
                                reader["nombre"].ToString(),
                                reader["apellido"].ToString(),
                                reader["apodo"].ToString(),
                                reader["afinidad"].ToString(),
                                reader["posicion"].ToString(),
                                Convert.ToInt32(reader["dorsal"]),
                                Convert.ToBoolean(reader["es_titular"]),
                                Convert.ToBoolean(reader["esta_convocado"]),
                                reader["url_imagen"].ToString(),
                                Convert.ToInt32(reader["nivel"]),
                                Convert.ToInt32(reader["id_responsable"]),
                                reader["url_imagen_responsable"].ToString(),
                                Convert.ToInt32(reader["id_equipo"]),
                                Convert.ToDecimal(reader["clausula_rescision"]),
                                Convert.ToBoolean(reader["esta_disponible"]),
                                Convert.ToDecimal(reader["sueldo"])
                            );
                            jugadores.Add(jugador);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en GetByTeam: {ex.Message}");
            }
            return jugadores;
        }

        public void DeleteJugador(int idJugador)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_EliminarJugador";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = idJugador;
                command.ExecuteNonQuery();
            }
        }
    }
}