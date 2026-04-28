using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IPlantillaRepository
    {
        // Opción A: Ligera y rápida, solo para el Drag & Drop
        void ActualizarPosicion(int idJugador, string nuevaPosicion);

        // Opción B: Tu función completa corregida, por si editas datos personales
        void ActualizarDatosJugador(JugadorModel jugador);

        void ActualizarSituacionJugador(int idJugador, string nuevaPosicion, bool esTitular, bool estaConvocado);
    }
}