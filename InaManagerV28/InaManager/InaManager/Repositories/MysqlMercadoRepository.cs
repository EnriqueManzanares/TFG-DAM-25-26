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

        public bool ComprarJugadorPorPrecio(int id_anuncio, int id_equipo_comprador, decimal precio)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_ComprarJugadorPrecio", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_anuncio", id_anuncio);
                    command.Parameters.AddWithValue("@id_equipo_comprador", id_equipo_comprador);
                    command.Parameters.AddWithValue("@precio", precio);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ComprarJugadorPorPrecio: {ex.Message}");
                return false;
            }
        }

        public bool ComprarJugadorPorClausula(int id_jugador, int id_equipo_comprador, decimal clausula)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_ComprarJugadorClausula", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_jugador", id_jugador);
                    command.Parameters.AddWithValue("@id_equipo_comprador", id_equipo_comprador);
                    command.Parameters.AddWithValue("@clausula", clausula);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ComprarJugadorPorClausula: {ex.Message}");
                return false;
            }
        }

        public void CrearAnuncio(int id_jugador, int id_equipo, decimal precio, int dias)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_CrearAnuncioMercado", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_jugador", id_jugador);
                    command.Parameters.AddWithValue("@id_equipo", id_equipo);
                    command.Parameters.AddWithValue("@precio", precio);
                    command.Parameters.AddWithValue("@dias", dias);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en CrearAnuncio: {ex.Message}");
            }
        }

        public void EliminarAnuncio(int id_anuncio)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_EliminarAnuncio", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_anuncio", id_anuncio);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en EliminarAnuncio: {ex.Message}");
            }
        }
    }
}