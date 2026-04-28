using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IFormacionRepository
    {
        IEnumerable<FormacionModel> GetAll();
        FormacionModel GetById(int id);
        void Add(FormacionModel formacion);
        void Update(FormacionModel formacion);
        void Delete(int id);

        // Método extra útil: Obtener lista de nombres de formaciones para un ComboBox
        IEnumerable<string> GetNombresFormaciones();
    }
}