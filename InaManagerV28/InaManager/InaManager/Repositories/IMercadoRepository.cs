using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IMercadoRepository
    {
        List<AnuncioModel> ObtenerAnunciosDisponibles();
        bool ComprarJugadorPorPrecio(int id_anuncio, int id_equipo_comprador, decimal precio);
        bool ComprarJugadorPorClausula(int id_jugador, int id_equipo_comprador, decimal clausula);
        void CrearAnuncio(int id_jugador, int id_equipo, decimal precio, int dias);
        void EliminarAnuncio(int id_anuncio);
    }
}
