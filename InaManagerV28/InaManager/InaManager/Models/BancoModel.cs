namespace InaManager.Models
{
    public class BancoModel
    {
        public string NombrePropietario { get; set; }
        public string IBAN { get; set; }
        public decimal Saldo { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }
    }
}
