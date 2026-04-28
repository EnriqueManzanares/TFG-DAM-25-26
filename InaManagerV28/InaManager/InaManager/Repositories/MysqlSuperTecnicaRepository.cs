using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlSuperTecnicaRepository : RepositoryBase, ISuperTecnicaRepository
    {
        // 1. OBTENER TODAS (Global)
        public IEnumerable<SuperTecnicaModel> GetAll()
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
                        lista.Add(MapReaderToModel(reader));
                    }
                }
            }
            return lista;
        }

        // 2. OBTENER POR ID
        public SuperTecnicaModel GetById(int id)
        {
            SuperTecnicaModel tecnica = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerSupertecnicaPorId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = id;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tecnica = MapReaderToModel(reader);
                    }
                }
            }
            return tecnica;
        }

        // 3. OBTENER POR JUGADOR (Para la vista de Equipo)
        public IEnumerable<SuperTecnicaModel> GetByPlayerId(int idJugador)
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
                        lista.Add(MapReaderToModel(reader));
                    }
                }
            }
            return lista;
        }

        // 4. AÑADIR (INSERT)
        public void Add(SuperTecnicaModel model)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_InsertarSupertecnica";
                command.CommandType = CommandType.StoredProcedure;

                AddParameters(command, model);
                command.ExecuteNonQuery();
            }
        }

        // 5. ACTUALIZAR (UPDATE)
        public void Update(SuperTecnicaModel model)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ActualizarSupertecnica";
                command.CommandType = CommandType.StoredProcedure;

                AddParameters(command, model);
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = model.Id_SuperTecnica;

                command.ExecuteNonQuery();
            }
        }

        // 6. BORRAR (DELETE) USANDO STORED PROCEDURE
        public void Delete(int id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Usamos el procedimiento almacenado que creaste
                command.CommandText = "sp_BorrarSupertecnica";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
        }

        // 7. DESVINCULAR DE JUGADOR (Para borrar solo la relación)
        public void UnassignFromPlayer(int idJugador, int idTecnica)
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

        // --- HELPERS (MAPEO Y PARÁMETROS) ---

        private void AddParameters(MySqlCommand command, SuperTecnicaModel model)
        {
            // Mapeo Model -> SQL
            command.Parameters.Add("@p_nombre", MySqlDbType.VarChar).Value = model.Nombre_SuperTecnica;
            command.Parameters.Add("@p_tipo", MySqlDbType.VarChar).Value = model.Tipo_SuperTecnica;
            command.Parameters.Add("@p_afinidad", MySqlDbType.VarChar).Value = model.Afinidad_SuperTecnica;
            command.Parameters.Add("@p_especial", MySqlDbType.VarChar).Value = model.Especialidad ?? (object)DBNull.Value;
            command.Parameters.Add("@p_potencia", MySqlDbType.Int32).Value = model.Potencia;
        }

        private SuperTecnicaModel MapReaderToModel(MySqlDataReader reader)
        {
            // Mapeo SQL -> Model
            // IMPORTANTE: Aquí es donde corregimos el error. Usamos los nombres reales de la BD.
            return new SuperTecnicaModel
            {
                Id_SuperTecnica = Convert.ToInt32(reader["id_tecnica"]),
                Nombre_SuperTecnica = reader["nombre"].ToString(),
                Tipo_SuperTecnica = reader["tipo"].ToString(),
                Afinidad_SuperTecnica = reader["afinidad"].ToString(),
                // En la BD es 'especial', en el modelo 'Especialidad'
                Especialidad = reader["especial"] != DBNull.Value ? reader["especial"].ToString() : "-",
                Potencia = reader["potencia"] != DBNull.Value ? Convert.ToInt32(reader["potencia"]) : 0
            };
        }
        public void AssignToPlayer(int idJugador, int idTecnica)
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

    }
}