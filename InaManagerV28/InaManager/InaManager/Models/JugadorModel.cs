using System;

namespace InaManager.Models
{
    public class JugadorModel
    {
        // Campos privados
        private int _id_jugador;
        private string _nombre;
        private string _apellido;
        private string _apodo;
        private string _email;
        private string _username;
        private string _password;
        private int _telefono;
        private string _afinidad;
        private string _posicion;
        private int _dorsal;
        private bool _es_titular;
        private bool _esta_convocado;
        private string _url_imagen;
        private int _nivel;
        private int _id_responsable;
        private string _urlImagenResponsable;
        private bool debe_cambiar_pass;

        // Propiedades con bloque de código tradicional
        public int Id_jugador
        {
            get { return _id_jugador; }
            set { _id_jugador = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Apellido
        {
            get { return _apellido; }
            set { _apellido = value; }
        }
        public string Apodo
        {
            get { return _apodo; }
            set { _apodo = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public bool Debe_cambiar_pass
        {
            get { return debe_cambiar_pass; }
            set { debe_cambiar_pass = value; }
        }
        public int Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }

        public string Afinidad
        {
            get { return _afinidad; }
            set { _afinidad = value; }
        }

        public string Posicion
        {
            get { return _posicion; }
            set { _posicion = value; }
        }

        public int Dorsal
        {
            get { return _dorsal; }
            set { _dorsal = value; }
        }

        public bool Es_titular
        {
            get { return _es_titular; }
            set { _es_titular = value; }
        }

        public bool Esta_convocado
        {
            get { return _esta_convocado; }
            set { _esta_convocado = value; }
        }

        public string Url_imagen
        {
            get { return _url_imagen; }
            set { _url_imagen = value; }
        }

        public int Nivel
        {
            get { return _nivel; }
            set { _nivel = value; }
        }

        public int Id_responsable
        {
            get { return _id_responsable; }
            set { _id_responsable = value; }
        }
        public string UrlImagenResponsable
        {
            get { return _urlImagenResponsable; }
            set { _urlImagenResponsable = value; }
        }


        // --- CONSTRUCTORES ---

        // Constructor vacío (necesario para serialización y WPF)
        public JugadorModel()
        {
        }

        // Constructor parametrizado (para cargar desde las bases de datos)
        public JugadorModel(int id, string nombre, string apellido, string apodo, string afinidad,
                       string posicion, int dorsal, bool esTitular, bool estaConvocado,
                       string urlImagen, int nivel, int idResponsable, string urlImagenResponsable)
        {
            this.Id_jugador = id;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Apodo = apodo;
            this.Afinidad = afinidad;
            this.Posicion = posicion;
            this.Dorsal = dorsal;
            this.Es_titular = esTitular;
            this.Esta_convocado = estaConvocado;
            this.Url_imagen = urlImagen;
            this.Nivel = nivel;
            this.Id_responsable = idResponsable;
            this.UrlImagenResponsable = urlImagenResponsable;
        }
    }
}