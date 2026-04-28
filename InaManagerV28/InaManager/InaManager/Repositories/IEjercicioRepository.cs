using InaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Repositories
{
    public interface IEjercicioRepository
    {
        public List<EjercicioModel> ObtenerTodos();

        void Insertar(EjercicioModel ejercicio);

        void Actualizar(EjercicioModel ejercicio);


        void Eliminar(int idEjercicio);
    }
}
