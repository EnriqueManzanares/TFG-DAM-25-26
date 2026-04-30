using System;

namespace InaManager.Models
{
    public class TransaccionModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_transaccion;
        private int? _id_cuenta_origen;
        private int? _id_cuenta_destino;
        private decimal _monto;
        private string _tipo;
        private string _concepto;
        private DateTime _fecha_operacion;
        private int? _id_jugador_relacionado;

        // --- PROPIEDADES PÚBLICAS ---
        public int Id_transaccion
        {
            get { return _id_transaccion; }
            set { _id_transaccion = value; }
        }

        public int? Id_cuenta_origen
        {
            get { return _id_cuenta_origen; }
            set { _id_cuenta_origen = value; }
        }

        public int? Id_cuenta_destino
        {
            get { return _id_cuenta_destino; }
            set { _id_cuenta_destino = value; }
        }

        public decimal Monto
        {
            get { return _monto; }
            set { _monto = value; }
        }

        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public string Concepto
        {
            get { return _concepto; }
            set { _concepto = value; }
        }

        public DateTime Fecha_operacion
        {
            get { return _fecha_operacion; }
            set { _fecha_operacion = value; }
        }

        public int? Id_jugador_relacionado
        {
            get { return _id_jugador_relacionado; }
            set { _id_jugador_relacionado = value; }
        }

        // --- CONSTRUCTORES ---

        // Constructor vacío
        public TransaccionModel()
        {
        }

        // Constructor parametrizado
        public TransaccionModel(int id_transaccion, int? id_cuenta_origen, int? id_cuenta_destino, decimal monto, string tipo, string concepto, DateTime fecha_operacion, int? id_jugador_relacionado)
        {
            this.Id_transaccion = id_transaccion;
            this.Id_cuenta_origen = id_cuenta_origen;
            this.Id_cuenta_destino = id_cuenta_destino;
            this.Monto = monto;
            this.Tipo = tipo;
            this.Concepto = concepto;
            this.Fecha_operacion = fecha_operacion;
            this.Id_jugador_relacionado = id_jugador_relacionado;
        }
    }
}
