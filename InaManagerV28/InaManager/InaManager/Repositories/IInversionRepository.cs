using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IInversionRepository
    {
        List<ActivoInversionModel> ObtenerActivos();
        List<InversionModel> ObtenerCarteraUsuario(int idUsuario, bool esJugador);
        bool ComprarActivo(int idUsuario, bool esJugador, int idActivo, decimal cantidad, decimal precioCompra, decimal costoTotal);
        bool VenderActivoParcial(int idUsuario, bool esJugador, int idActivo, decimal cantidadVender, decimal precioVenta);
    }
}
