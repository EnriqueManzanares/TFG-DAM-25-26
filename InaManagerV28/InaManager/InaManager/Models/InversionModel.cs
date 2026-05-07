using System;

namespace InaManager.Models
{
    public class InversionModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_inversion;
        private int _id_cuenta;
        private int _id_activo;
        private decimal _cantidad;
        private decimal _precio_compra;
        private DateTime _fecha_compra;

        // Datos del activo (denormalizados desde el JOIN en el SP)
        private string _nombre_activo;
        private string _simbolo_activo;
        private decimal _precio_base;
        private decimal _volatilidad;
        private string _icono_emoji;

        // --- PROPIEDADES PÚBLICAS ---
        public int Id_inversion
        {
            get { return _id_inversion; }
            set { _id_inversion = value; }
        }

        public int Id_cuenta
        {
            get { return _id_cuenta; }
            set { _id_cuenta = value; }
        }

        public int Id_activo
        {
            get { return _id_activo; }
            set { _id_activo = value; }
        }

        public decimal Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public decimal PrecioCompra
        {
            get { return _precio_compra; }
            set { _precio_compra = value; }
        }

        public DateTime FechaCompra
        {
            get { return _fecha_compra; }
            set { _fecha_compra = value; }
        }

        public string NombreActivo
        {
            get { return _nombre_activo; }
            set { _nombre_activo = value; }
        }

        public string SimboloActivo
        {
            get { return _simbolo_activo; }
            set { _simbolo_activo = value; }
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

        // --- CONSTRUCTORES ---

        // Constructor vacío
        public InversionModel()
        {
        }

        // Constructor parametrizado
        public InversionModel(int id_inversion, int id_cuenta, int id_activo, decimal cantidad,
            decimal precio_compra, DateTime fecha_compra, string nombre_activo,
            string simbolo_activo, decimal precio_base, decimal volatilidad, string icono_emoji)
        {
            this.Id_inversion = id_inversion;
            this.Id_cuenta = id_cuenta;
            this.Id_activo = id_activo;
            this.Cantidad = cantidad;
            this.PrecioCompra = precio_compra;
            this.FechaCompra = fecha_compra;
            this.NombreActivo = nombre_activo;
            this.SimboloActivo = simbolo_activo;
            this.PrecioBase = precio_base;
            this.Volatilidad = volatilidad;
            this.IconoEmoji = icono_emoji;
        }
    }
}
