using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Models
{
    public class EjercicioModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_ejercicio;
        private string _nombre;
        private string _categoria;
        private string _descripcion;

        // --- PROPIEDADES PÚBLICAS ---
        public int id_ejercicio
        {
            get { return _id_ejercicio; }
            set { _id_ejercicio = value; }
        }

        public string nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string categoria
        {
            get { return _categoria; }
            set { _categoria = value; }
        }

        public string descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        // --- CONSTRUCTORES ---

        // 1. Constructor vacío
        public EjercicioModel()
        {
        }

        // 2. Constructor parametrizado
        public EjercicioModel(int id, string nombre, string categoria, string descripcion)
        {
            this.id_ejercicio = id;
            this.nombre = nombre;
            this.categoria = categoria;
            this.descripcion = descripcion;
        }
    }
}