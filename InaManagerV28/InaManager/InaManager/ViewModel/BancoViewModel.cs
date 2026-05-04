using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class BancoViewModel : ViewModelBase
    {
        private BancoModel _bancoDatos;
        private readonly IBancoRepository _bancoRepository;

        public BancoModel BancoDatos
        {
            get => _bancoDatos;
            set { _bancoDatos = value; OnPropertyChanged(nameof(BancoDatos)); }
        }

        public string NombreUsuario => UserSession.UsuarioActual?.DisplayName ?? "Usuario";

        public ICommand VolverAlHubCommand { get; }
        public ICommand TransferirDineroCommand { get; }

        public BancoViewModel()
        {
            _bancoRepository = new MysqlBancoRepository();
            VolverAlHubCommand = new ViewModelCommand(ExecuteVolverAlHub);
            TransferirDineroCommand = new ViewModelCommand(ExecuteTransferirDinero);
            
            // Cargar datos del banco desde la base de datos
            CargarDatosDelBanco();
        }

        private void CargarDatosDelBanco()
        {
            try
            {
                // Obtener el ID del usuario actual de la sesión
                int idEmpleado = UserSession.IdUsuario;
                
                if (idEmpleado > 0)
                {
                    // Cargar datos de la base de datos
                    BancoDatos = _bancoRepository.ObtenerCuentaPorEmpleado(idEmpleado);
                }
                else
                {
                    // Si no hay usuario en sesión, mostrar datos vacíos
                    BancoDatos = new BancoModel
                    {
                        NombrePropietario = "Usuario Desconocido",
                        IBAN = "N/A",
                        Saldo = 0m,
                        NumeroCuenta = "N/A",
                        TipoCuenta = "Cuenta Corriente"
                    };
                }
            }
            catch
            {
                // Si hay error, mostrar datos por defecto
                BancoDatos = new BancoModel
                {
                    NombrePropietario = "Error al cargar datos",
                    IBAN = "Error",
                    Saldo = 0m,
                    NumeroCuenta = "Error",
                    TipoCuenta = "Cuenta Corriente"
                };
            }
        }

        private void ExecuteTransferirDinero(object obj)
        {
            // Abrir ventana de transferencia
            var transferenciaView = new View.TransferenciaBancariaView();
            transferenciaView.Show();
        }

        private void ExecuteVolverAlHub(object obj)
        {
            // Abrimos el Hub nuevamente
            var hubView = new View.AppHubView();
            hubView.Show();

            // Cerramos la ventana actual del Banco
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
            {
                if (w.GetType().Name == "BancoView")
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}
