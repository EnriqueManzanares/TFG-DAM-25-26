using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using InaManager.Models;

namespace InaManager.Repositories
{
    // Modelo extendido interno para traer datos del entrenador
    // Puedes poner esto en tu carpeta Models si prefieres
    public class FormacionConEntrenadorModel : FormacionModel
    {
        public string EntrenadorNombre { get; set; }
        public string EntrenadorApellido { get; set; }
        public string EntrenadorFoto { get; set; }
        public string NombreCompletoEntrenador => $"{EntrenadorNombre} {EntrenadorApellido}";
    }

    public class MysqlFormacionRepository : RepositoryBase, IFormacionRepository
    {
        // --- 1. OBTENER TODAS (Con SP) ---
        public IEnumerable<FormacionModel> GetAll()
        {
            var lista = new List<FormacionConEntrenadorModel>();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerFormaciones"; // Nombre del SP
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var formacion = new FormacionConEntrenadorModel();
                        // Mapeo base
                        formacion.Id_formacion = reader.GetInt32("id_formacion");
                        formacion.Nombre = reader.GetString("nombre");

                        // Slots
                        for (int i = 1; i <= 11; i++)
                            formacion.Posiciones.Add(reader.GetString($"slot_{i}"));

                        // Datos Extra del Entrenador (del JOIN en el SP)
                        if (!reader.IsDBNull(reader.GetOrdinal("entrenador_nombre")))
                        {
                            formacion.EntrenadorNombre = reader.GetString("entrenador_nombre");
                            formacion.EntrenadorApellido = reader.GetString("entrenador_apellido");
                            formacion.EntrenadorFoto = reader.GetString("entrenador_foto");
                        }
                        else
                        {
                            formacion.EntrenadorNombre = "Sin Asignar";
                            formacion.EntrenadorApellido = "";
                            formacion.EntrenadorFoto = "/Images/Empleados/default.png";
                        }

                        lista.Add(formacion);
                    }
                }
            }
            return lista;
        }

        // --- 2. OBTENER POR ID (SQL normal o podrías hacer otro SP) ---
        public FormacionModel GetById(int id)
        {
            FormacionModel formacion = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerFormacionPorId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = id;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        formacion = new FormacionModel();
                        formacion.Id_formacion = reader.GetInt32("id_formacion");
                        formacion.Nombre = reader.GetString("nombre");
                        for (int i = 1; i <= 11; i++)
                            formacion.Posiciones.Add(reader.GetString($"slot_{i}"));
                    }
                }
            }
            return formacion;
        }

        // --- 3. AÑADIR NUEVA (Con SP) ---
        public void Add(FormacionModel formacion)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_CrearFormacion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_nombre", MySqlDbType.VarChar).Value = formacion.Nombre;

                for (int i = 0; i < 11; i++)
                {
                    string val = (i < formacion.Posiciones.Count) ? formacion.Posiciones[i] : "DC";
                    command.Parameters.Add($"@p_s{i + 1}", MySqlDbType.VarChar).Value = val;
                }

                command.ExecuteNonQuery();
            }
        }

        // --- 4. ACTUALIZAR (Con SP) ---
        public void Update(FormacionModel formacion)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ActualizarFormacion";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = formacion.Id_formacion;
                command.Parameters.Add("@p_nombre", MySqlDbType.VarChar).Value = formacion.Nombre;

                for (int i = 0; i < 11; i++)
                {
                    string val = (i < formacion.Posiciones.Count) ? formacion.Posiciones[i] : "DC";
                    command.Parameters.Add($"@p_s{i + 1}", MySqlDbType.VarChar).Value = val;
                }

                command.ExecuteNonQuery();
            }
        }

        // --- 5. BORRAR (Con SP) ---
        public void Delete(int id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_BorrarFormacion";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = id;
                command.ExecuteNonQuery();
            }
        }

        // --- EXTRA ---
        public IEnumerable<string> GetNombresFormaciones()
        {
            // Implementación igual que tenías
            var lista = new List<string>();
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerNombresFormaciones";
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) lista.Add(reader.GetString(0));
                }
            }
            return lista;
        }
    }
}