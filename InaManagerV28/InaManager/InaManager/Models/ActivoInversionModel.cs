using System;

namespace InaManager.Models
{
    public class ActivoInversionModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_activo;
        private string _nombre;
        private string _simbolo;
        private decimal _precio_base;
        private decimal _volatilidad;
        private string _icono_emoji;
        private string _descripcion;

        // --- PROPIEDADES PÚBLICAS ---
        public int Id_activo
        {
            get { return _id_activo; }
            set { _id_activo = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Simbolo
        {
            get { return _simbolo; }
            set { _simbolo = value; }
        }

        public decimal PrecioBase
        {
            get { return _precio_base; }
            set { _precio_base = value; }
        }

        public decimal Volatilidad
        {
            get { return _volatilidad; }
            set { _volatilidad = value; }
        }

        public string IconoEmoji
        {
            get { return _icono_emoji; }
            set { _icono_emoji = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        // --- CONSTRUCTORES ---

        // Constructor vacío
        public ActivoInversionModel()
        {
        }

        // Constructor parametrizado
        public ActivoInversionModel(int id_activo, string nombre, string simbolo, decimal precio_base, decimal volatilidad, string icono_emoji, string descripcion)
        {
            this.Id_activo = id_activo;
            this.Nombre = nombre;
            this.Simbolo = simbolo;
            this.PrecioBase = precio_base;
            this.Volatilidad = volatilidad;
            this.IconoEmoji = icono_emoji;
            this.Descripcion = descripcion;
        }
    }
}
