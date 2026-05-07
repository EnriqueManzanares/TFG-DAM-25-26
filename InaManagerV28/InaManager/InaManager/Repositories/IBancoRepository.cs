using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IBancoRepository
    {
        BancoModel ObtenerCuentaPorEmpleado(int idEmpleado);
        BancoModel ObtenerCuentaPorUsuario(int idUsuario, bool esJugador, string nombrePersona);
        bool ActualizarSaldo(int idEmpleado, decimal nuevoSaldo);
        List<BancoModel> ObtenerTransacciones(int idEmpleado, int cantidadUltimas = 10);
        List<TransaccionModel> ObtenerHistorialTransaccionesUsuario(int idUsuario, bool esJugador);
        bool RealizarTransferencia(int idEmpleadoOrigen, string ibanDestino, decimal monto, string concepto);
    }
}
