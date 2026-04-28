using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using InaManager.Models;
using System.Diagnostics;

namespace InaManager.Repositories
{
    public class MysqlEntrenoRepository : RepositoryBase, IEntrenoRepository
    {
        // =========================================================
        // MÉTODOS DE LECTURA (AGENDA)
        // =========================================================

        public List<AgendaItemModel> ObtenerEntrenosAgenda()
        {
            return EjecutarConsultaAgenda(null);
        }

        public List<AgendaItemModel> ObtenerEntrenosAgendaPorJugador(int idJugador)
        {
            return EjecutarConsultaAgenda(idJugador);
        }

        private List<AgendaItemModel> EjecutarConsultaAgenda(int? idJugadorFiltro)
        {
            var lista = new List<AgendaItemModel>();
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new MySqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    // Volvemos a usar el procedimiento almacenado actualizado en la BD
                    cmd.CommandText = "sp_ObtenerEntrenosAgenda";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_id_jugador", idJugadorFiltro.HasValue ? (object)idJugadorFiltro.Value : DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new AgendaItemModel
                            {
                                Id = Convert.ToInt32(reader["id_entreno"]),
                                Fecha = DateOnly.FromDateTime(Convert.ToDateTime(reader["fecha"])),
                                Titulo = "Entrenamiento",
                                Subtitulo = reader["nombre_ejercicio"].ToString(),
                                Tipo = "Entrenamiento",
                                Color = "#3498DB", // Azul

                                NombreEjercicio = reader["nombre_ejercicio"].ToString(),
                                NombreJugador = reader["nombre_jugador"].ToString(),
                                NombreEmpleado = reader["nombre_empleado"].ToString(),
                                Comentarios = reader["comentarios"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Mantenemos el throw para que los errores salten en la UI (Alerta Roja)
                throw;
            }
            return lista;
        }

        // =========================================================
        // MÉTODOS DE SELECTORES (COMBOBOX)
        // =========================================================

        public List<SelectorSimple> ObtenerSelectorJugadores()
        {
            return ExecuteSelectorQuerySp("sp_ObtenerSelectorJugadores");
        }

        public List<SelectorSimple> ObtenerSelectorEjercicios()
        {
            return ExecuteSelectorQuerySp("sp_ObtenerSelectorEjercicios");
        }

        public List<SelectorSimple> ObtenerSelectorEmpleados()
        {
            return ExecuteSelectorQuerySp("sp_ObtenerSelectorEmpleados");
        }

        private List<SelectorSimple> ExecuteSelectorQuerySp(string spName)
        {
            var lista = new List<SelectorSimple>();
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new SelectorSimple
                        {
                            Id = Convert.ToInt32(reader[0]),
                            Nombre = reader[1].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        // =========================================================
        // MÉTODOS DE ESCRITURA
        // =========================================================

        public void InsertarEntrenoCompleto(int idJugador, int idEjercicio, int idEmpleado, DateTime fecha, string comentarios)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "sp_InsertarEntreno";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_id_jugador", idJugador);
                cmd.Parameters.AddWithValue("@p_id_ejercicio", idEjercicio);
                cmd.Parameters.AddWithValue("@p_id_empleado", idEmpleado <= 0 ? (object)DBNull.Value : idEmpleado);
                cmd.Parameters.AddWithValue("@p_fecha", fecha);
                cmd.Parameters.AddWithValue("@p_comentarios", comentarios ?? "");

                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int idEntreno)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand("sp_EliminarEntreno", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                cmd.Parameters.AddWithValue("@p_id", idEntreno);
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarComentarios(int idEntreno, string comentarios)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "sp_ActualizarComentariosEntreno";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_id", idEntreno);
                cmd.Parameters.AddWithValue("@p_comentarios", comentarios ?? "");

                cmd.ExecuteNonQuery();
            }
        }
    }
}