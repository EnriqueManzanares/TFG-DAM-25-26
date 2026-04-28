using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IPartidoRepository
    {
        // Usamos nombres propios para evitar conflictos
        List<PartidoModel> ObtenerListaPartidos();
        void RegistrarNuevoPartido(PartidoModel partido);
        void ActualizarDatosPartido(PartidoModel partido);
        void EliminarPartidoPorId(int id);
    }
}