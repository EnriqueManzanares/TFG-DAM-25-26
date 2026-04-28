using System;

namespace InaManager.Models
{
    public class EventoCalendario
    {
        public DateTime Fecha { get; set; }
        public string Titulo { get; set; }      // Ejemplo: "VS Raimon" o "Tiro Fantasma"
        public string Tipo { get; set; }        // "PARTIDO" o "ENTRENO"
        public string ColorHex { get; set; }    // "#D32F2F" (Rojo) o "#1976D2" (Azul)
        public string Hora { get; set; }
    }

    // Clase auxiliar para representar un día en la cuadrícula
    public class DiaCalendario
    {
        public int NumeroDia { get; set; }
        public bool EsDiaValido { get; set; } // Para ocultar los huecos vacíos
        public DateTime FechaCompleta { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<EventoCalendario> Eventos { get; set; }
            = new System.Collections.ObjectModel.ObservableCollection<EventoCalendario>();
    }
}