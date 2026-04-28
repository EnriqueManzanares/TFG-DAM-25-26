using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InaManager.Models
{
    public class EntrenoModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_entreno;
        private int _id_jugador;
        private int _id_ejercicio;
        private int _id_empleado;
        private DateTime _fecha;
        private string _comentarios;
        private bool _completado;

        // --- PROPIEDADES PÚBLICAS ---
        public int id_entreno
        {
            get { return _id_entreno; }
            set { _id_entreno = value; }
        }

        public int id_jugador
        {
            get { return _id_jugador; }
            set { _id_jugador = value; }
        }

        public int id_ejercicio
        {
            get { return _id_ejercicio; }
            set { _id_ejercicio = value; }
        }

        public int id_empleado
        {
            get { return _id_empleado; }
            set { _id_empleado = value; }
        }

        public DateTime fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public string comentarios
        {
            get { return _comentarios; }
            set { _comentarios = value; }
        }

        public bool completado
        {
            get { return _completado; }
            set { _completado = value; }
        }

        // --- CONSTRUCTORES ---

        // 1. Constructor vacío
        public EntrenoModel()
        {
        }

        // 2. Constructor parametrizado
        public EntrenoModel(int idEntreno, int idJugador, int idEjercicio,
                       int idEmpleado, DateTime fecha, string comentarios, bool completado)
        {
            this.id_entreno = idEntreno;
            this.id_jugador = idJugador;
            this.id_ejercicio = idEjercicio;
            this.id_empleado = idEmpleado;
            this.fecha = fecha;
            this.comentarios = comentarios;
            this.completado = completado;
        }
    }
}