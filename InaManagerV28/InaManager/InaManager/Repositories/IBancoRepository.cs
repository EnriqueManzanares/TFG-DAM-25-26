using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IBancoRepository
    {
        BancoModel ObtenerCuentaPorEmpleado(int idEmpleado);
        bool ActualizarSaldo(int idEmpleado, decimal nuevoSaldo);
        List<BancoModel> ObtenerTransacciones(int idEmpleado, int cantidadUltimas = 10);
        bool RealizarTransferencia(int idEmpleadoOrigen, string ibanDestino, decimal monto, string concepto);
    }
}
