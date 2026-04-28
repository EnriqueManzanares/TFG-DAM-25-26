using System;
using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IEntrenoRepository
    {
        List<AgendaItemModel> ObtenerEntrenosAgenda();
        List<AgendaItemModel> ObtenerEntrenosAgendaPorJugador(int idJugador);


        List<SelectorSimple> ObtenerSelectorJugadores();
        List<SelectorSimple> ObtenerSelectorEjercicios();
        List<SelectorSimple> ObtenerSelectorEmpleados();

        void InsertarEntrenoCompleto(int idJugador, int idEjercicio, int idEmpleado, DateTime fecha, string comentarios);
        void Eliminar(int idEntreno);
        void ActualizarComentarios(int idEntreno, string comentarios);
    }
}