using System;

namespace InaManager.Models
{
    public class EquipoModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_equipo;
        private string _nombre_equipo;
        private int _fk_director;
        private string _url_escudo;

        // Datos extraídos del JOIN
        private string _nombre_director;
        private string _apellido_director;

        // --- PROPIEDADES PÚBLICAS ---
        public int Id_equipo
        {
            get { return _id_equipo; }
            set { _id_equipo = value; }
        }

        public string Nombre_equipo
        {
            get { return _nombre_equipo; }
            set { _nombre_equipo = value; }
        }

        public int Fk_director
        {
            get { return _fk_director; }
            set { _fk_director = value; }
        }

        public string Url_escudo
        {
            get { return _url_escudo; }
            set { _url_escudo = value; }
        }

        public string Nombre_director
        {
            get { return _nombre_director; }
            set { _nombre_director = value; }
        }

        public string Apellido_director
        {
            get { return _apellido_director; }
            set { _apellido_director = value; }
        }

        // --- CONSTRUCTORES ---

        // Constructor vacío
        public EquipoModel()
        {
        }

        // Constructor parametrizado
        public EquipoModel(int id_equipo, string nombre_equipo, int fk_director, string url_escudo, string nombre_director = "", string apellido_director = "")
        {
            this.Id_equipo = id_equipo;
            this.Nombre_equipo = nombre_equipo;
            this.Fk_director = fk_director;
            this.Url_escudo = url_escudo;
            this.Nombre_director = nombre_director;
            this.Apellido_director = apellido_director;
        }
    }
}
