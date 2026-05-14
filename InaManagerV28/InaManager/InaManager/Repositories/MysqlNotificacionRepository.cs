using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlNotificacionRepository : RepositoryBase, INotificacionRepository
    {
        public void CrearNotificacion(NotificacionModel notificacion)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_CrearNotificacion", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_equipo", notificacion.Id_equipo);
                    command.Parameters.AddWithValue("@tipo", notificacion.Tipo);
                    command.Parameters.AddWithValue("@titulo", notificacion.Titulo);
                    command.Parameters.AddWithValue("@mensaje", notificacion.Mensaje);
                    command.Parameters.AddWithValue("@jugador_nombre", notificacion.Jugador_nombre);
                    command.Parameters.AddWithValue("@jugador_apellido", notificacion.Jugador_apellido);
                    command.Parameters.AddWithValue("@monto", notificacion.Monto);
                    command.Parameters.AddWithValue("@estado", notificacion.Estado);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en CrearNotificacion: {ex.Message}");
            }
        }

        public List<NotificacionModel> ObtenerNotificacionesPorEquipo(int id_equipo)
        {
            var lista = new List<NotificacionModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_ObtenerNotificaciones", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_equipo", id_equipo);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new NotificacionModel(
                                Convert.ToInt32(reader["id_equipo"]),
                                reader["tipo"].ToString(),
                                reader["titulo"].ToString(),
                                reader["mensaje"].ToString(),
                                reader["jugador_nombre"].ToString(),
                                reader["jugador_apellido"].ToString(),
                                Convert.ToDecimal(reader["monto"]),
                                Convert.ToDateTime(reader["fecha_creacion"]),
                                Convert.ToBoolean(reader["leida"]),
                                reader["estado"].ToString()
                            )
                            {
                                Id_notificacion = Convert.ToInt32(reader["id_notificacion"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerNotificacionesPorEquipo: {ex.Message}");
            }
            return lista;
        }

        public void MarcarComoLeida(int id_notificacion)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_MarcarNotificacionLeida", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_notificacion", id_notificacion);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en MarcarComoLeida: {ex.Message}");
            }
        }

        public void EliminarNotificacion(int id_notificacion)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_EliminarNotificacion", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_notificacion", id_notificacion);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en EliminarNotificacion: {ex.Message}");
            }
        }
    }
}
