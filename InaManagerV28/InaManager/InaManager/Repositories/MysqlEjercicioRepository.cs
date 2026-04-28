using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using InaManager.Models;

namespace InaManager.Repositories
{
    // Heredamos de RepositoryBase, igual que hace MysqlEmpleadoRepository
    public class MysqlEjercicioRepository : RepositoryBase , IEjercicioRepository
    {
        // 1. OBTENER TODOS LOS EJERCICIOS
        public List<EjercicioModel> ObtenerTodos()
        {
            var lista = new List<EjercicioModel>();

            // Usamos GetConnection() en lugar de connectionString manual
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerEjercicios";
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new EjercicioModel
                        {
                            id_ejercicio = Convert.ToInt32(reader["id_ejercicio"]),
                            nombre = reader["nombre"].ToString(),
                            categoria = reader["categoria"].ToString(),
                            descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : ""
                        });
                    }
                }
            }
            return lista;
        }

        // 2. AÑADIR NUEVO EJERCICIO
        public void Insertar(EjercicioModel ejercicio)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_InsertarEjercicio";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_nombre", MySqlDbType.VarChar).Value = ejercicio.nombre;
                command.Parameters.Add("@p_categoria", MySqlDbType.VarChar).Value = ejercicio.categoria;
                command.Parameters.Add("@p_descripcion", MySqlDbType.Text).Value = ejercicio.descripcion;

                command.ExecuteNonQuery();
            }
        }

        // 3. ACTUALIZAR EJERCICIO
        public void Actualizar(EjercicioModel ejercicio)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ActualizarEjercicio";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = ejercicio.id_ejercicio;
                command.Parameters.Add("@p_nombre", MySqlDbType.VarChar).Value = ejercicio.nombre;
                command.Parameters.Add("@p_categoria", MySqlDbType.VarChar).Value = ejercicio.categoria;
                command.Parameters.Add("@p_descripcion", MySqlDbType.Text).Value = ejercicio.descripcion;

                command.ExecuteNonQuery();
            }
        }

        // 4. ELIMINAR EJERCICIO
        public void Eliminar(int idEjercicio)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_EliminarEjercicio";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = idEjercicio;

                command.ExecuteNonQuery();
            }
        }
    }
}