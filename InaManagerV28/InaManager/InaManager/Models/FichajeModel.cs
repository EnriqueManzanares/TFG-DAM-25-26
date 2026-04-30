using System;

namespace InaManager.Models
{
    public class FichajeModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_fichaje;
        private int _fk_jugador;
        private int? _fk_equipo_anterior;
        private int _fk_equipo_destino;
        private int? _fk_transaccion;
        private decimal _precio_final;
        private DateTime _fecha_fichaje;
        private DateTime _fecha_incorporacion;
        private string _estado_fichaje;

        // Datos extraídos del JOIN
        private string _nombre_jugador;
        private string _apellido_jugador;
        private string _equipo_origen;
        private string _equipo_destino;

        // --- PROPIEDADES PÚBLICAS ---
        public int Id_fichaje
        {
            get { return _id_fichaje; }
            set { _id_fichaje = value; }
        }

        public int Fk_jugador
        {
            get { return _fk_jugador; }
            set { _fk_jugador = value; }
        }

        public int? Fk_equipo_anterior
        {
            get { return _fk_equipo_anterior; }
            set { _fk_equipo_anterior = value; }
        }

        public int Fk_equipo_destino
        {
            get { return _fk_equipo_destino; }
            set { _fk_equipo_destino = value; }
        }

        public int? Fk_transaccion
        {
            get { return _fk_transaccion; }
            set { _fk_transaccion = value; }
        }

        public decimal Precio_final
        {
            get { return _precio_final; }
            set { _precio_final = value; }
        }

        public DateTime Fecha_fichaje
        {
            get { return _fecha_fichaje; }
            set { _fecha_fichaje = value; }
        }

        public DateTime Fecha_incorporacion
        {
            get { return _fecha_incorporacion; }
            set { _fecha_incorporacion = value; }
        }

        public string Estado_fichaje
        {
            get { return _estado_fichaje; }
            set { _estado_fichaje = value; }
        }

        public string Nombre_jugador
        {
            get { return _nombre_jugador; }
            set { _nombre_jugador = value; }
        }

        public string Apellido_jugador
        {
            get { return _apellido_jugador; }
            set { _apellido_jugador = value; }
        }

        public string Equipo_origen
        {
            get { return _equipo_origen; }
            set { _equipo_origen = value; }
        }

        public string Equipo_destino
        {
            get { return _equipo_destino; }
            set { _equipo_destino = value; }
        }

        // --- CONSTRUCTORES ---

        // Constructor vacío
        public FichajeModel()
        {
        }

        // Constructor parametrizado
        public FichajeModel(int id_fichaje, int fk_jugador, int? fk_equipo_anterior, int fk_equipo_destino, int? fk_transaccion, decimal precio_final, DateTime fecha_fichaje, DateTime fecha_incorporacion, string estado_fichaje, string nombre_jugador = "", string apellido_jugador = "", string equipo_origen = "", string equipo_destino = "")
        {
            this.Id_fichaje = id_fichaje;
            this.Fk_jugador = fk_jugador;
            this.Fk_equipo_anterior = fk_equipo_anterior;
            this.Fk_equipo_destino = fk_equipo_destino;
            this.Fk_transaccion = fk_transaccion;
            this.Precio_final = precio_final;
            this.Fecha_fichaje = fecha_fichaje;
            this.Fecha_incorporacion = fecha_incorporacion;
            this.Estado_fichaje = estado_fichaje;
            
            this.Nombre_jugador = nombre_jugador;
            this.Apellido_jugador = apellido_jugador;
            this.Equipo_origen = equipo_origen;
            this.Equipo_destino = equipo_destino;
        }
    }
}
