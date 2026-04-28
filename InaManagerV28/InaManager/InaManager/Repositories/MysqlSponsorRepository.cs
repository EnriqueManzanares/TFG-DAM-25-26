using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlSponsorRepository : RepositoryBase, ISponsorRepository
    {
        // LEER TODOS
        public IEnumerable<SponsorModel> GetAll()
        {
            var lista = new List<SponsorModel>();
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ObtenerSponsors";
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new SponsorModel
                        {
                            Id_Sponsor = Convert.ToInt32(reader["id_sponsor"]),
                            Nombre_Empresa = reader["nombre_empresa"].ToString(),
                            Sector = reader["sector"].ToString(),
                            Aporte_Economico = Convert.ToDecimal(reader["aporte_economico"]),
                            Url_Logo = reader["url_logo"] != DBNull.Value ? reader["url_logo"].ToString() : "/Images/Sponsors/default_logo.png",
                            Fecha_Inicio = reader["fecha_inicio"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_inicio"]) : DateTime.Now,
                            Fecha_Fin = reader["fecha_fin"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_fin"]) : DateTime.Now
                        });
                    }
                }
            }
            return lista;
        }

        // AÑADIR
        public void Add(SponsorModel sponsor)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_InsertarSponsor";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_nombre_empresa", MySqlDbType.VarChar).Value = sponsor.Nombre_Empresa;
                command.Parameters.Add("@p_sector", MySqlDbType.VarChar).Value = sponsor.Sector;
                command.Parameters.Add("@p_aporte", MySqlDbType.Decimal).Value = sponsor.Aporte_Economico;
                command.Parameters.Add("@p_url", MySqlDbType.VarChar).Value = sponsor.Url_Logo;
                command.Parameters.Add("@p_fecha_inicio", MySqlDbType.Date).Value = sponsor.Fecha_Inicio;
                command.Parameters.Add("@p_fecha_fin", MySqlDbType.Date).Value = sponsor.Fecha_Fin;

                command.ExecuteNonQuery();
            }
        }

        // EDITAR
        public void Edit(SponsorModel sponsor)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ActualizarSponsor";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_nombre_empresa", MySqlDbType.VarChar).Value = sponsor.Nombre_Empresa;
                command.Parameters.Add("@p_sector", MySqlDbType.VarChar).Value = sponsor.Sector;
                command.Parameters.Add("@p_aporte", MySqlDbType.Decimal).Value = sponsor.Aporte_Economico;
                command.Parameters.Add("@p_url", MySqlDbType.VarChar).Value = sponsor.Url_Logo;
                command.Parameters.Add("@p_fecha_inicio", MySqlDbType.Date).Value = sponsor.Fecha_Inicio;
                command.Parameters.Add("@p_fecha_fin", MySqlDbType.Date).Value = sponsor.Fecha_Fin;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = sponsor.Id_Sponsor;

                command.ExecuteNonQuery();
            }
        }

        // BORRAR
        public void Remove(int id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_EliminarSponsor";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = id;
                command.ExecuteNonQuery();
            }
        }
    }
}