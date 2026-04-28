using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface ISponsorRepository
    {
        // LEER TODOS
        IEnumerable<SponsorModel> GetAll();

        // AÑADIR
        void Add(SponsorModel sponsor);

        // EDITAR
        void Edit(SponsorModel sponsor);

        // BORRAR
        void Remove(int id);
    }
}