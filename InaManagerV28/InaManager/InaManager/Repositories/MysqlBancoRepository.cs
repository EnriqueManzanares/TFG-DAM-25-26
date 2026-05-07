using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using InaManager.Models;

namespace InaManager.Repositories
{
    public class MysqlBancoRepository : RepositoryBase, IBancoRepository
    {
        public BancoModel ObtenerCuentaPorEmpleado(int idEmpleado)
        {
            var bancoModel = new BancoModel();
            
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand(@"
                    SELECT c.iban, c.saldo_actual, e.nombre, e.apellido
                    FROM Cuentas_Bancarias c
                    INNER JOIN Empleados e ON c.id_empleado = e.id_empleado
                    WHERE c.id_empleado = @id_emp", connection))
                {
                    command.Parameters.AddWithValue("@id_emp", idEmpleado);
                    
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string iban = reader["iban"].ToString();
                            bancoModel.IBAN = iban;
                            bancoModel.Saldo = Convert.ToDecimal(reader["saldo_actual"]);
                            bancoModel.NombrePropietario = $"{reader["nombre"]} {reader["apellido"]}";
                            // Extraer últimos 7 dígitos del IBAN como número de cuenta
                            bancoModel.NumeroCuenta = iban.Length >= 7 ? iban.Substring(iban.Length - 7) : iban;
                            bancoModel.TipoCuenta = "Cuenta Corriente";
                        }
                        else
                        {
                            // Si no existe, retornar modelo vacío/por defecto
                            bancoModel.NombrePropietario = "Usuario Desconocido";
                            bancoModel.IBAN = "N/A";
                            bancoModel.Saldo = 0m;
                            bancoModel.NumeroCuenta = "N/A";
                            bancoModel.TipoCuenta = "Cuenta Corriente";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerCuentaPorEmpleado: {ex.Message}");
                bancoModel.NombrePropietario = "Error al cargar";
                bancoModel.IBAN = "Error";
                bancoModel.Saldo = 0m;
            }

            return bancoModel;
        }

        public BancoModel ObtenerCuentaPorUsuario(int idUsuario, bool esJugador, string nombrePersona)
        {
            var bancoModel = new BancoModel();
            
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_ObtenerCuentaPorUsuario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", idUsuario);
                    command.Parameters.AddWithValue("p_es_jugador", esJugador);
                    
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string iban = reader["iban"].ToString();
                            bancoModel.IBAN = iban;
                            bancoModel.Saldo = Convert.ToDecimal(reader["saldo_actual"]);
                            bancoModel.NombrePropietario = nombrePersona;
                            bancoModel.NumeroCuenta = iban.Length >= 7 ? iban.Substring(iban.Length - 7) : iban;
                            bancoModel.TipoCuenta = "Cuenta Corriente";
                        }
                        else
                        {
                            bancoModel.NombrePropietario = nombrePersona;
                            bancoModel.IBAN = "N/A";
                            bancoModel.Saldo = 0m;
                            bancoModel.NumeroCuenta = "N/A";
                            bancoModel.TipoCuenta = "Cuenta Corriente";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerCuentaPorUsuario: {ex.Message}");
                bancoModel.NombrePropietario = "Error al cargar";
                bancoModel.IBAN = "Error";
                bancoModel.Saldo = 0m;
            }

            return bancoModel;
        }

        public bool ActualizarSaldo(int idEmpleado, decimal nuevoSaldo)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(
                        "UPDATE Cuentas_Bancarias SET saldo_actual = @saldo WHERE id_empleado = @id_emp", connection))
                    {
                        command.Parameters.AddWithValue("@saldo", nuevoSaldo);
                        command.Parameters.AddWithValue("@id_emp", idEmpleado);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ActualizarSaldo: {ex.Message}");
                return false;
            }
        }

        public List<BancoModel> ObtenerTransacciones(int idEmpleado, int cantidadUltimas = 10)
        {
            var transacciones = new List<BancoModel>();
            
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand(@"
                    SELECT t.monto, t.tipo, t.concepto, t.fecha_operacion
                    FROM Transacciones t
                    INNER JOIN Cuentas_Bancarias c ON t.id_cuenta = c.id_cuenta
                    WHERE c.id_empleado = @id_emp
                    ORDER BY t.fecha_operacion DESC
                    LIMIT @cantidad", connection))
                {
                    command.Parameters.AddWithValue("@id_emp", idEmpleado);
                    command.Parameters.AddWithValue("@cantidad", cantidadUltimas);
                    
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var transaccion = new BancoModel
                            {
                                Saldo = Convert.ToDecimal(reader["monto"]),
                                TipoCuenta = reader["tipo"].ToString(),
                                NumeroCuenta = reader["concepto"].ToString()
                            };
                            transacciones.Add(transaccion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerTransacciones: {ex.Message}");
            }

            return transacciones;
        }

        public List<TransaccionModel> ObtenerHistorialTransaccionesUsuario(int idUsuario, bool esJugador)
        {
            var transacciones = new List<TransaccionModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("sp_ObtenerHistorialTransaccionesUsuario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", idUsuario);
                    command.Parameters.AddWithValue("p_es_jugador", esJugador);
                    
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var transaccion = new TransaccionModel
                            {
                                Monto = Convert.ToDecimal(reader["monto_final"]),
                                Tipo = reader["tipo"].ToString(),
                                Concepto = reader["concepto"].ToString(),
                                Fecha_operacion = Convert.ToDateTime(reader["fecha_operacion"])
                            };
                            transacciones.Add(transaccion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerHistorialTransaccionesUsuario: {ex.Message}");
            }
            return transacciones;
        }

        public bool RealizarTransferencia(int idEmpleadoOrigen, string ibanDestino, decimal monto, string concepto)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    // Obtener la cuenta del origen
                    int idCuentaOrigen = ObtenerIdCuenta(connection, idEmpleadoOrigen);
                    if (idCuentaOrigen <= 0)
                    {
                        throw new Exception("No se encontró la cuenta del origen");
                    }

                    // Obtener la cuenta del destino por IBAN
                    int idCuentaDestino = ObtenerIdCuentaPorIban(connection, ibanDestino);
                    if (idCuentaDestino <= 0)
                    {
                        throw new Exception("El IBAN de destino no existe");
                    }

                    // Verificar saldo suficiente
                    decimal saldoOrigen = ObtenerSaldoCuenta(connection, idCuentaOrigen);
                    if (saldoOrigen < monto)
                    {
                        throw new Exception("Saldo insuficiente");
                    }

                    // Iniciar transacción
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Restar del origen
                            var cmdRestar = new MySqlCommand(
                                "UPDATE Cuentas_Bancarias SET saldo_actual = saldo_actual - @monto WHERE id_cuenta = @id_cuenta",
                                connection, transaction);
                            cmdRestar.Parameters.AddWithValue("@monto", monto);
                            cmdRestar.Parameters.AddWithValue("@id_cuenta", idCuentaOrigen);
                            cmdRestar.ExecuteNonQuery();

                            // Agregar al destino
                            var cmdAgregar = new MySqlCommand(
                                "UPDATE Cuentas_Bancarias SET saldo_actual = saldo_actual + @monto WHERE id_cuenta = @id_cuenta",
                                connection, transaction);
                            cmdAgregar.Parameters.AddWithValue("@monto", monto);
                            cmdAgregar.Parameters.AddWithValue("@id_cuenta", idCuentaDestino);
                            cmdAgregar.ExecuteNonQuery();

                            // Registrar transacción de origen
                            var cmdTransaccionOrigen = new MySqlCommand(
                                @"INSERT INTO Transacciones (id_cuenta, monto, tipo, concepto, fecha_operacion) 
                                  VALUES (@id_cuenta, @monto, 'Transferencia', @concepto, NOW())",
                                connection, transaction);
                            cmdTransaccionOrigen.Parameters.AddWithValue("@id_cuenta", idCuentaOrigen);
                            cmdTransaccionOrigen.Parameters.AddWithValue("@monto", -monto);
                            cmdTransaccionOrigen.Parameters.AddWithValue("@concepto", $"{concepto} a {ibanDestino}");
                            cmdTransaccionOrigen.ExecuteNonQuery();

                            // Registrar transacción de destino
                            var cmdTransaccionDestino = new MySqlCommand(
                                @"INSERT INTO Transacciones (id_cuenta, monto, tipo, concepto, fecha_operacion) 
                                  VALUES (@id_cuenta, @monto, 'Transferencia', @concepto, NOW())",
                                connection, transaction);
                            cmdTransaccionDestino.Parameters.AddWithValue("@id_cuenta", idCuentaDestino);
                            cmdTransaccionDestino.Parameters.AddWithValue("@monto", monto);
                            cmdTransaccionDestino.Parameters.AddWithValue("@concepto", concepto);
                            cmdTransaccionDestino.ExecuteNonQuery();

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en RealizarTransferencia: {ex.Message}");
                return false;
            }
        }

        private int ObtenerIdCuenta(MySqlConnection connection, int idEmpleado)
        {
            using (var command = new MySqlCommand(
                "SELECT id_cuenta FROM Cuentas_Bancarias WHERE id_empleado = @id_emp LIMIT 1",
                connection))
            {
                command.Parameters.AddWithValue("@id_emp", idEmpleado);
                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private int ObtenerIdCuentaPorIban(MySqlConnection connection, string iban)
        {
            using (var command = new MySqlCommand(
                "SELECT id_cuenta FROM Cuentas_Bancarias WHERE iban = @iban LIMIT 1",
                connection))
            {
                command.Parameters.AddWithValue("@iban", iban);
                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private decimal ObtenerSaldoCuenta(MySqlConnection connection, int idCuenta)
        {
            using (var command = new MySqlCommand(
                "SELECT saldo_actual FROM Cuentas_Bancarias WHERE id_cuenta = @id_cuenta",
                connection))
            {
                command.Parameters.AddWithValue("@id_cuenta", idCuenta);
                var result = command.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }
    }
}
