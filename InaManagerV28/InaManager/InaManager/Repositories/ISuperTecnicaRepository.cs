using InaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Repositories
{
    public interface ISuperTecnicaRepository
    {
        void Add(SuperTecnicaModel superTecnica);
        void Update(SuperTecnicaModel superTecnica);
        void Delete(int id);
        IEnumerable<SuperTecnicaModel> GetAll();
        SuperTecnicaModel GetById(int id);
        IEnumerable<SuperTecnicaModel> GetByPlayerId(int idJugador);
        void UnassignFromPlayer(int idJugador, int idTecnica);
        void AssignToPlayer(int idJugador, int idTecnica);
    }
}
