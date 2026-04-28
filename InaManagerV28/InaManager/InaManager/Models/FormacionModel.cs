using System;
using System.Collections.Generic;

namespace InaManager.Models
{
    public class FormacionModel
    {
        public int Id_formacion { get; set; }
        public string Nombre { get; set; }

        // Usamos una lista para manejar los 11 slots más fácilmente en código,
        // aunque en BD sean columnas separadas.
        public List<string> Posiciones { get; set; } = new List<string>();

        public FormacionModel() { }

        public FormacionModel(int id, string nombre, List<string> posiciones)
        {
            Id_formacion = id;
            Nombre = nombre;
            Posiciones = posiciones;
            
        }
        public void SetSlot(int index, string posicion)
        {
            if (index >= 0 && index < Posiciones.Count)
            {
                Posiciones[index] = posicion;
            }
        }
    }
}