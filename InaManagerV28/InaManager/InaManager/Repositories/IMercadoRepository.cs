using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IMercadoRepository
    {
        List<AnuncioModel> ObtenerAnunciosDisponibles();
    }
}
