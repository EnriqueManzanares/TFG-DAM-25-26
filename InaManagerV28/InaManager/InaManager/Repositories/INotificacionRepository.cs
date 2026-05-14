using System.Collections.Generic;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface INotificacionRepository
    {
        void CrearNotificacion(NotificacionModel notificacion);
        List<NotificacionModel> ObtenerNotificacionesPorEquipo(int id_equipo);
        void MarcarComoLeida(int id_notificacion);
        void EliminarNotificacion(int id_notificacion);
    }
}
