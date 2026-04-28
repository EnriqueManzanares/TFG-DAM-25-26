using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlPartidoRepository : RepositoryBase, IPartidoRepository
    {
        private const string NOMBRE_MI_EQUIPO = "Royal Academy";

        // =========================================================
        // MÉTODOS DE LECTURA
        // =========================================================

        // Método original (Mantenido por compatibilidad)
        public List<PartidoModel> ObtenerListaPartidos()
        {
            var lista = new List<PartidoModel>();
            using (var connection = GetConnection())
            using (var command = new MySqlCommand("sp_ObtenerListaPartidos", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new PartidoModel();
                        p.Id_Partido = Convert.ToInt32(reader["id_partido"]);
                        p.Fecha_Partido = DateOnly.FromDateTime(Convert.ToDateTime(reader["fecha"]));
                        p.Equipo_Local = NOMBRE_MI_EQUIPO;
                        p.Equipo_Visitante = reader["rival"].ToString();
                        p.Goles_Local = Convert.ToInt32(reader["goles_local"]);
                        p.Goles_Visitante = Convert.ToInt32(reader["goles_rival"]);
                        lista.Add(p);
                    }
                }
            }
            return lista;
        }

        // NUEVO MÉTODO: Para la Agenda (con datos extra para el desplegable)
        public List<AgendaItemModel> ObtenerAgendaPartidos()
        {
            var lista = new List<AgendaItemModel>();
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand("sp_ObtenerAgendaPartidos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new AgendaItemModel
                        {
                            Id = Convert.ToInt32(reader["id_partido"]),
                            Fecha = DateOnly.FromDateTime(Convert.ToDateTime(reader["fecha"])),
                            Titulo = "VS " + reader["rival"].ToString(),
                            Subtitulo = reader["competicion"].ToString(),
                            Tipo = "Partido",
                            Color = "#E74C3C", // Rojo

                            // Datos específicos para el desplegable
                            Competicion = reader["competicion"].ToString(),
                            GolesLocal = Convert.ToInt32(reader["goles_local"]),
                            GolesVisitante = Convert.ToInt32(reader["goles_rival"])
                        });
                    }
                }
            }
            return lista;
        }

        // =========================================================
        // MÉTODOS DE ESCRITURA
        // =========================================================

        public void RegistrarNuevoPartido(PartidoModel partido)
        {
            // Redirigimos al método completo para no duplicar lógica
            InsertarPartidoCompleto(partido.Fecha_Partido.ToDateTime(TimeOnly.MinValue), partido.Equipo_Visitante, "Amistoso");
        }

        public void InsertarPartidoCompleto(DateTime fecha, string rival, string competicion)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                // Insertamos con goles a 0 por defecto
                cmd.CommandText = "sp_InsertarPartido";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_fecha", fecha);
                cmd.Parameters.AddWithValue("@p_rival", rival);
                cmd.Parameters.AddWithValue("@p_competicion", competicion);
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarDatosPartido(PartidoModel partido)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                string rival = partido.Equipo_Visitante == NOMBRE_MI_EQUIPO ? partido.Equipo_Local : partido.Equipo_Visitante;

                cmd.CommandText = "sp_ActualizarDatosPartido";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_fecha", partido.Fecha_Partido.ToDateTime(TimeOnly.MinValue));
                cmd.Parameters.AddWithValue("@p_rival", rival);
                cmd.Parameters.AddWithValue("@p_id", partido.Id_Partido);
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarPartidoPorId(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand("sp_EliminarPartido", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                cmd.Parameters.AddWithValue("@p_id", id);
                cmd.ExecuteNonQuery();
            }
        }
        public void ActualizarResultado(int idPartido, int golesLocal, int golesVisitante, string competicion)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "sp_ActualizarResultadoPartido";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@p_gl", golesLocal);
                cmd.Parameters.AddWithValue("@p_gv", golesVisitante);
                cmd.Parameters.AddWithValue("@p_comp", competicion);
                cmd.Parameters.AddWithValue("@p_id", idPartido);

                cmd.ExecuteNonQuery();
            }
        }
    }
}