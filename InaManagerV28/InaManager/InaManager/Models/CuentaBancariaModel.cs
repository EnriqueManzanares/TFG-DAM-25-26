using System;

namespace InaManager.Models
{
    public class CuentaBancariaModel
    {
        // --- CAMPOS PRIVADOS ---
        private int _id_cuenta;
        private string _iban;
        private int? _id_jugador;
        private int? _id_empleado;
        private decimal _saldo_actual;
        private string _moneda;

        // --- PROPIEDADES PÚBLICAS ---
        public int Id_cuenta
        {
            get { return _id_cuenta; }
            set { _id_cuenta = value; }
        }

        public string Iban
        {
            get { return _iban; }
            set { _iban = value; }
        }

        public int? Id_jugador
        {
            get { return _id_jugador; }
            set { _id_jugador = value; }
        }

        public int? Id_empleado
        {
            get { return _id_empleado; }
            set { _id_empleado = value; }
        }

        public decimal Saldo_actual
        {
            get { return _saldo_actual; }
            set { _saldo_actual = value; }
        }

        public string Moneda
        {
            get { return _moneda; }
            set { _moneda = value; }
        }

        // --- CONSTRUCTORES ---

        // Constructor vacío
        public CuentaBancariaModel()
        {
        }

        // Constructor parametrizado
        public CuentaBancariaModel(int id_cuenta, string iban, int? id_jugador, int? id_empleado, decimal saldo_actual, string moneda)
        {
            this.Id_cuenta = id_cuenta;
            this.Iban = iban;
            this.Id_jugador = id_jugador;
            this.Id_empleado = id_empleado;
            this.Saldo_actual = saldo_actual;
            this.Moneda = moneda;
        }
    }
}
