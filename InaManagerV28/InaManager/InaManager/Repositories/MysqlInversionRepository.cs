using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlInversionRepository : RepositoryBase, IInversionRepository
    {
        public List<ActivoInversionModel> ObtenerActivos()
        {
            var lista = new List<ActivoInversionModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_ObtenerActivos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ActivoInversionModel(
                                Convert.ToInt32(reader["id_activo"]),
                                reader["nombre"].ToString(),
                                reader["simbolo"].ToString(),
                                Convert.ToDecimal(reader["precio_base"]),
                                Convert.ToDecimal(reader["volatilidad"]),
                                reader["icono_emoji"].ToString(),
                                reader["descripcion"] == DBNull.Value ? "" : reader["descripcion"].ToString()
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerActivos: {ex.Message}");
            }
            return lista;
        }

        public List<InversionModel> ObtenerCarteraUsuario(int idUsuario, bool esJugador)
        {
            var lista = new List<InversionModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_ObtenerCarteraUsuario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", idUsuario);
                    command.Parameters.AddWithValue("p_es_jugador", esJugador);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new InversionModel(
                                Convert.ToInt32(reader["id_inversion"]),
                                Convert.ToInt32(reader["id_cuenta"]),
                                Convert.ToInt32(reader["id_activo"]),
                                Convert.ToDecimal(reader["cantidad"]),
                                Convert.ToDecimal(reader["precio_compra"]),
                                Convert.ToDateTime(reader["fecha_compra"]),
                                reader["nombre_activo"].ToString(),
                                reader["simbolo_activo"].ToString(),
                                Convert.ToDecimal(reader["precio_base"]),
                                Convert.ToDecimal(reader["volatilidad"]),
                                reader["icono_emoji"].ToString()
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerCarteraUsuario: {ex.Message}");
            }
            return lista;
        }

        public bool ComprarActivo(int idUsuario, bool esJugador, int idActivo, decimal cantidad, decimal precioCompra, decimal costoTotal)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_ComprarActivo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", idUsuario);
                    command.Parameters.AddWithValue("p_es_jugador", esJugador);
                    command.Parameters.AddWithValue("p_id_activo", idActivo);
                    command.Parameters.AddWithValue("p_cantidad", cantidad);
                    command.Parameters.AddWithValue("p_precio_compra", precioCompra);
                    command.Parameters.AddWithValue("p_costo_total", costoTotal);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ComprarActivo: {ex.Message}");
                return false;
            }
        }

        public bool VenderActivoParcial(int idUsuario, bool esJugador, int idActivo, decimal cantidadVender, decimal precioVenta)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_VenderActivoParcial", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario",     idUsuario);
                    command.Parameters.AddWithValue("p_es_jugador",     esJugador);
                    command.Parameters.AddWithValue("p_id_activo",      idActivo);
                    command.Parameters.AddWithValue("p_cantidad_vender", cantidadVender);
                    command.Parameters.AddWithValue("p_precio_venta",   precioVenta);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en VenderActivoParcial: {ex.Message}");
                return false;
            }
        }
    }
}
