using System;

namespace InaManager.Models
{
    public class NotificacionModel
    {
        public int Id_notificacion { get; set; }
        public int Id_equipo { get; set; }
        public string Tipo { get; set; } // "Compra", "Venta", "Oferta", etc.
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Jugador_nombre { get; set; }
        public string Jugador_apellido { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha_creacion { get; set; }
        public bool Leida { get; set; }
        public string Estado { get; set; } // "Pendiente", "Aceptada", "Rechazada"

        public NotificacionModel() { }

        public NotificacionModel(
            int id_equipo, string tipo, string titulo, string mensaje,
            string jugador_nombre, string jugador_apellido, decimal monto,
            DateTime fecha_creacion = default, bool leida = false, string estado = "Pendiente")
        {
            Id_equipo = id_equipo;
            Tipo = tipo;
            Titulo = titulo;
            Mensaje = mensaje;
            Jugador_nombre = jugador_nombre;
            Jugador_apellido = jugador_apellido;
            Monto = monto;
            Fecha_creacion = fecha_creacion == default ? DateTime.Now : fecha_creacion;
            Leida = leida;
            Estado = estado;
        }

        public string NombreJugadorCompleto => $"{Jugador_nombre} {Jugador_apellido}";
        public string MontoFormato => Monto.ToString("N0") + " €";
    }
}
