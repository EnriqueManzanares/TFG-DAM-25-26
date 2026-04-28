using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InaManager.Models
{
    // Implementamos INotify para que el desplegable funcione al instante
    public class AgendaItemModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; } // Competición o Categoría Ejercicio
        public string Tipo { get; set; } // "Partido" o "Entrenamiento"
        public string Color { get; set; }

        // --- DATOS ESPECÍFICOS PARA EL DESPLEGABLE ---
        // Partido
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
        public string Competicion { get; set; }

        // Entrenamiento
        public string NombreJugador { get; set; }
        public string NombreEjercicio { get; set; }
        public string NombreEmpleado { get; set; }
        public string Comentarios { get; set; }

        // --- ESTADO VISUAL ---
        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // Modelo simple para llenar los ComboBoxes (Jugadores, Ejercicios, Empleados)
    public class SelectorSimple
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}