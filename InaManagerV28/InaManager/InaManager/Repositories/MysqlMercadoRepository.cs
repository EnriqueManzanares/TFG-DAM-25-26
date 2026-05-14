using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlMercadoRepository : RepositoryBase, IMercadoRepository
    {
        public List<AnuncioModel> ObtenerAnunciosDisponibles()
        {
            var lista = new List<AnuncioModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_ObtenerAnunciosMercado", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new AnuncioModel(
                                Convert.ToInt32(reader["id_anuncio"]),
                                Convert.ToInt32(reader["id_jugador"]),
                                Convert.ToInt32(reader["id_equipo"]),
                                Convert.ToDecimal(reader["precio"]),
                                Convert.ToDateTime(reader["fecha_fin"]),
                                reader["estado"].ToString(),
                                reader["jugador_nombre"].ToString(),
                                reader["jugador_apellido"].ToString(),
                                reader["jugador_posicion"].ToString(),
                                reader["jugador_afinidad"] == DBNull.Value ? "Neutro" : reader["jugador_afinidad"].ToString(),
                                reader["jugador_nivel"] == DBNull.Value ? 1 : Convert.ToInt32(reader["jugador_nivel"]),
                                reader["jugador_clausula"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["jugador_clausula"]),
                                reader["jugador_imagen"] == DBNull.Value ? "" : reader["jugador_imagen"].ToString(),
                                reader["equipo_nombre"].ToString(),
                                reader["equipo_escudo"] == DBNull.Value ? "" : reader["equipo_escudo"].ToString()
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerAnunciosDisponibles: {ex.Message}");
            }
            return lista;
        }
    }
}
