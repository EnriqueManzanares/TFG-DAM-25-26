using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Net;
using MySql.Data.MySqlClient;
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlEmpleadoRepository : RepositoryBase, IEmpleadoRepository
    {
        // 1. VALIDAR EMPLEADO (LOGIN)
        public bool ValidateEmployee(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ValidarEmpleado";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_username", MySqlDbType.VarChar).Value = credential.UserName;
                command.Parameters.Add("@p_password", MySqlDbType.VarChar).Value = credential.Password;
                
                // Parámetro de salida
                command.Parameters.Add(new MySqlParameter("@p_es_valido", MySqlDbType.Bit));
                command.Parameters["@p_es_valido"].Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();
                validUser = Convert.ToBoolean(command.Parameters["@p_es_valido"].Value);
            }
            return validUser;
        }

        // 2. OBTENER TODOS
        public IEnumerable<EmpleadoModel> GetAllEmployees()
        {
            var lista = new List<EmpleadoModel>();
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "sp_ObtenerTodosEmpleados";
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Usamos tu mapeador base para los datos del empleado
                        var empleado = MapReaderToEmpleado(reader);

                        // Si el empleado tiene una formación (ID > 0)
                        if (reader["id_formacion_activa"] != DBNull.Value && Convert.ToInt32(reader["id_formacion_activa"]) > 0)
                        {
                            var formacion = new FormacionModel
                            {
                                Id_formacion = Convert.ToInt32(reader["id_formacion_activa"]),
                                Nombre = reader["nombre_formacion"]?.ToString() ?? "Sin Nombre",
                                // IMPORTANTE: Inicializamos la lista con 11 nulos para que el índice exista
                                Posiciones = new List<string>(new string[11])
                            };

                            for (int i = 1; i <= 11; i++)
                            {
                                string columnaSlot = $"slot_{i}";
                                // Verificamos que la columna existe y no es nula
                                string valorPosicion = reader[columnaSlot] != DBNull.Value
                                                       ? reader[columnaSlot].ToString()
                                                       : "";

                                // Ahora SetSlot no fallará porque ya hay 11 espacios creados arriba
                                formacion.SetSlot(i - 1, valorPosicion);
                            }

                            empleado.FormacionTactica = formacion;
                        }

                        lista.Add(empleado);
                    }
                }
            }
            return lista;
        }

        // 3. OBTENER POR ID
        public EmpleadoModel GetEmployeeById(int id)
        {
            EmpleadoModel empleado = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerEmpleadoPorId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = id;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        empleado = MapReaderToEmpleado(reader);
                    }
                }
            }
            return empleado;
        }

        // 4. OBTENER POR USERNAME
        public EmpleadoModel GetEmployeeByUsername(string username)
        {
            EmpleadoModel empleado = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerEmpleadoPorUsername";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_username", MySqlDbType.VarChar).Value = username;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        empleado = MapReaderToEmpleado(reader);
                    }
                }
            }
            return empleado;
        }

        // 5. AÑADIR EMPLEADO
        public void AddEmployee(EmpleadoModel empleado)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_InsertarEmpleado";
                command.CommandType = CommandType.StoredProcedure;

                AddParameters(command, empleado);
                command.ExecuteNonQuery();
            }
        }

        // 6. ACTUALIZAR EMPLEADO
        public void UpdateEmployee(EmpleadoModel empleado)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "sp_ActualizarEmpleado";
                command.CommandType = CommandType.StoredProcedure;

                // Este método ya añade @p_nombre, @p_apellido... y AHORA también @p_titular
                AddParameters(command, empleado);

                // AQUÍ YA NO PONGAS EL PARÁMETRO @p_titular (estaba duplicado)

                // Solo el ID, que no suele estar en AddParameters
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = empleado.Id_empleado;

                command.ExecuteNonQuery();
            }
        }

        // 7. BORRAR EMPLEADO
        public void DeleteEmployee(int id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "sp_EliminarEmpleado";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_id", id);
                command.ExecuteNonQuery();
            }
        }

        // 8. OBTENER JUGADORES (DE UN EMPLEADO)
        public IEnumerable<JugadorModel> GetJugadoresPorEmpleado(int idEmpleado)
        {
            var lista = new List<JugadorModel>();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerJugadoresPorEmpleado";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = idEmpleado;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var jugador = new JugadorModel();

                        jugador.Id_jugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                        jugador.Nombre = reader.GetString(reader.GetOrdinal("nombre"));
                        jugador.Apellido = reader.GetString(reader.GetOrdinal("apellido"));
                        jugador.Email = !reader.IsDBNull(reader.GetOrdinal("email")) ? reader.GetString(reader.GetOrdinal("email")) : "";
                        jugador.Posicion = reader.GetString(reader.GetOrdinal("posicion"));

                        jugador.Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? 0 : reader.GetInt32(reader.GetOrdinal("telefono"));
                        jugador.Url_imagen = reader.IsDBNull(reader.GetOrdinal("url_imagen")) ? "/Images/Jugadores/default.png" : reader.GetString(reader.GetOrdinal("url_imagen"));

                        lista.Add(jugador);
                    }
                }
            }
            return lista;
        }

        // 9. ASIGNAR JUGADOR (RESTAURADO)
        public void AsignarJugador(int idEmpleado, int idJugador)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_AsignarJugadorEmpleado";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_id_empleado", MySqlDbType.Int32).Value = idEmpleado;
                command.Parameters.Add("@p_id_jugador", MySqlDbType.Int32).Value = idJugador;

                command.ExecuteNonQuery();
            }
        }

        // 10. DESASIGNAR JUGADOR (RESTAURADO)
        public void DesasignarJugador(int idEmpleado, int idJugador)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_DesasignarJugadorEmpleado";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_id_empleado", MySqlDbType.Int32).Value = idEmpleado;
                command.Parameters.Add("@p_id_jugador", MySqlDbType.Int32).Value = idJugador;

                command.ExecuteNonQuery();
            }
        }

        // 11. ASIGNAR TÁCTICA (CORREGIDO PARA USAR TU SP)
        public void AsignarTactica(string username, string rol, int idFormacion)
        {
            // NOTA: Aunque recibimos 'rol' para cumplir la interfaz, tu SP 'sp_AsignarFormacionActiva'
            // solo necesita el username y el idFormacion, así que usamos esos dos.

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Llamamos al Stored Procedure que tienes en tu base de datos
                command.CommandText = "sp_AsignarFormacionActiva";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_username", MySqlDbType.VarChar).Value = username;
                command.Parameters.Add("@p_id_formacion", MySqlDbType.Int32).Value = idFormacion;

                command.ExecuteNonQuery();
            }
        }

        // Método antiguo de ayuda (opcional, pero lo dejo por si acaso lo usabas en otro lado)
        public void SetActiveFormation(string username, int idFormacion)
        {
            AsignarTactica(username, "", idFormacion);
        }

        // --- MÉTODOS PRIVADOS ---

        private EmpleadoModel MapReaderToEmpleado(MySqlDataReader reader)
        {
            return new EmpleadoModel
            {
                Id_empleado = Convert.ToInt32(reader["id_empleado"]),
                Nombre = reader["nombre"].ToString(),
                Apellido = reader["apellido"].ToString(),
                Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : "",
                Username = reader["username"] != DBNull.Value ? reader["username"].ToString() : "",
                Password = reader["password"] != DBNull.Value ? reader["password"].ToString() : "",
                Telefono = reader["telefono"] != DBNull.Value ? Convert.ToInt32(reader["telefono"]) : 0,
                Puesto = reader["puesto"].ToString(),
                Especialidad = reader["especialidad"] != DBNull.Value ? reader["especialidad"].ToString() : "",
                Años_experiencia = reader["años_experiencia"] != DBNull.Value ? Convert.ToInt32(reader["años_experiencia"]) : 0,
                Salario = reader["salario"] != DBNull.Value ? Convert.ToDecimal(reader["salario"]) : 0,
                Url_imagen = reader["url_imagen"] != DBNull.Value ? reader["url_imagen"].ToString() : "\\Images\\Empleados\\default.png",
                Entrenador_titular = reader["entrenador_titular"] != DBNull.Value && Convert.ToBoolean(reader["entrenador_titular"]),
                // Mapeo seguro para evitar error si es nulo
                Id_formacion_activa = reader["id_formacion_activa"] != DBNull.Value
                                      ? Convert.ToInt32(reader["id_formacion_activa"])
                                      : 0
            };
        }

        private void AddParameters(MySqlCommand command, EmpleadoModel emp)
        {
            // SE HA AÑADIDO EL PREFIJO "p_" A TODOS LOS PARÁMETROS
            command.Parameters.Add("@p_nombre", MySqlDbType.VarChar).Value = emp.Nombre;
            command.Parameters.Add("@p_apellido", MySqlDbType.VarChar).Value = emp.Apellido;
            command.Parameters.Add("@p_email", MySqlDbType.VarChar).Value = emp.Email;
            command.Parameters.Add("@p_username", MySqlDbType.VarChar).Value = emp.Username;
            command.Parameters.Add("@p_password", MySqlDbType.VarChar).Value = emp.Password;
            command.Parameters.Add("@p_telefono", MySqlDbType.Int32).Value = emp.Telefono;
            command.Parameters.Add("@p_puesto", MySqlDbType.VarChar).Value = emp.Puesto;
            command.Parameters.Add("@p_especialidad", MySqlDbType.VarChar).Value = emp.Especialidad;
            command.Parameters.Add("@p_experiencia", MySqlDbType.Int32).Value = emp.Años_experiencia;
            command.Parameters.Add("@p_salario", MySqlDbType.Decimal).Value = emp.Salario;

            // Nuevo parámetro para el booleano de entrenador titular
            command.Parameters.Add("@p_titular", MySqlDbType.Bit).Value = emp.Entrenador_titular;

            string rutaImagen = string.IsNullOrWhiteSpace(emp.Url_imagen)
                                ? "Images/Empleados/default.png"
                                : emp.Url_imagen;
            command.Parameters.Add("@p_url_imagen", MySqlDbType.VarChar).Value = rutaImagen;

            // Manejo de nulo para la FK
            if (emp.Id_formacion_activa == 0)
                command.Parameters.Add("@p_id_formacion", MySqlDbType.Int32).Value = DBNull.Value;
            else
                command.Parameters.Add("@p_id_formacion", MySqlDbType.Int32).Value = emp.Id_formacion_activa;
        }

        public EmpleadoModel GetEntrenadorTitular()
        {
            EmpleadoModel entrenador = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // Buscamos al empleado que sea titular, sin importar si su puesto es 'Entrenador', 'Manager', etc.
                command.CommandText = "sp_ObtenerEntrenadorTitular";
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        entrenador = new EmpleadoModel
                        {
                            Id_empleado = (int)reader["id_empleado"],
                            Id_formacion_activa = reader["id_formacion_activa"] != DBNull.Value ? (int)reader["id_formacion_activa"] : 0,
                            FormacionTactica = new FormacionModel()
                        };

                        if (entrenador.Id_formacion_activa > 0)
                        {
                            entrenador.FormacionTactica.Nombre = reader["nombre_formacion"].ToString();
                            for (int i = 1; i <= 11; i++)
                            {
                                entrenador.FormacionTactica.SetSlot(i - 1, reader[$"slot_{i}"].ToString());
                            }
                        }
                    }
                }
            }
            return entrenador;
        }

        public void ActualizarSituacionJugador(int idJugador, string nuevaPosicion, bool esTitular, bool estaConvocado)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Actualizamos Posicion, Titularidad y Convocatoria
                command.CommandText = "sp_ActualizarEstadoJugador";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_posicion", nuevaPosicion);
                command.Parameters.AddWithValue("@p_titular", esTitular);
                command.Parameters.AddWithValue("@p_convocado", estaConvocado);
                command.Parameters.AddWithValue("@p_id", idJugador);

                command.ExecuteNonQuery();
            }
        }

    }
}