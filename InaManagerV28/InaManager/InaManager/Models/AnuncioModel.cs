using System;

namespace InaManager.Models
{
    public class AnuncioModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_anuncio;
        private int _id_jugador;
        private int _id_equipo;
        private decimal _precio;
        private DateTime _fecha_fin;
        private string _estado;

        // --- PROPIEDADES PÚBLICAS ---
        public int Id_anuncio
        {
            get { return _id_anuncio; }
            set { _id_anuncio = value; }
        }

        public int Id_jugador
        {
            get { return _id_jugador; }
            set { _id_jugador = value; }
        }

        public int Id_equipo
        {
            get { return _id_equipo; }
            set { _id_equipo = value; }
        }

        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

        public DateTime Fecha_fin
        {
            get { return _fecha_fin; }
            set { _fecha_fin = value; }
        }

        public string Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        // --- CONSTRUCTORES ---

        // Constructor vacío
        public AnuncioModel()
        {
        }

        // Constructor parametrizado
        public AnuncioModel(int id_anuncio, int id_jugador, int id_equipo, decimal precio, DateTime fecha_fin, string estado)
        {
            this.Id_anuncio = id_anuncio;
            this.Id_jugador = id_jugador;
            this.Id_equipo = id_equipo;
            this.Precio = precio;
            this.Fecha_fin = fecha_fin;
            this.Estado = estado;
        }
    }
}
