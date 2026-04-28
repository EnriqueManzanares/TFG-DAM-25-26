using System;
using System.Data;
using MySql.Data.MySqlClient; // Asegúrate de tener este using
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlPlantillaRepository : RepositoryBase, IPlantillaRepository
    {
        // 1. MÉTODO OPTIMIZADO: Úsalo cuando arrastres en el campo
        // Es mucho más seguro porque solo toca la columna 'posicion'
        public void ActualizarPosicion(int idJugador, string nuevaPosicion)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "sp_ActualizarPosicionJugador";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@p_posicion", MySqlDbType.VarChar).Value = nuevaPosicion;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = idJugador;

                command.ExecuteNonQuery();
            }
        }

        // 2. MÉTODO COMPLETO: El que tenías, corregido y aislado
        public void ActualizarDatosJugador(JugadorModel jugador)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "sp_ActualizarDatosBasicosJugador";
                command.CommandType = CommandType.StoredProcedure;

                // Uso de AddWithValue o Add con tipo explícito (más seguro)
                command.Parameters.Add("@p_nombre", MySqlDbType.VarChar).Value = jugador.Nombre ?? (object)DBNull.Value;
                command.Parameters.Add("@p_apellido", MySqlDbType.VarChar).Value = jugador.Apellido ?? (object)DBNull.Value;
                command.Parameters.Add("@p_apodo", MySqlDbType.VarChar).Value = jugador.Apodo ?? (object)DBNull.Value;
                command.Parameters.Add("@p_email", MySqlDbType.VarChar).Value = jugador.Email ?? (object)DBNull.Value;
                command.Parameters.Add("@p_dorsal", MySqlDbType.Int32).Value = jugador.Dorsal;
                command.Parameters.Add("@p_posicion", MySqlDbType.VarChar).Value = jugador.Posicion ?? (object)DBNull.Value;
                command.Parameters.Add("@p_afinidad", MySqlDbType.VarChar).Value = jugador.Afinidad ?? (object)DBNull.Value;
                command.Parameters.Add("@p_id", MySqlDbType.Int32).Value = jugador.Id_jugador;

                command.ExecuteNonQuery();
            }
        }
        public void ActualizarSituacionJugador(int idJugador, string nuevaPosicion, bool esTitular, bool estaConvocado)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                // Actualizamos Posicion, Titularidad y Convocatoria
                command.CommandText = "sp_ActualizarEstadoJugador";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_posicion", nuevaPosicion);
                command.Parameters.AddWithValue("@p_titular", esTitular);
                command.Parameters.AddWithValue("@p_convocado", estaConvocado);
                command.Parameters.AddWithValue("@p_id", idJugador);

                command.ExecuteNonQuery();
            }
        }
    }
}