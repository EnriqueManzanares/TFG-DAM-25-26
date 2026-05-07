using System.Collections.ObjectModel;
using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    public class BancoViewModel : ViewModelBase
    {
        private BancoModel _bancoDatos;
        private readonly IBancoRepository _bancoRepository;
        private string _activeTab = "MiCuenta";

        // ── Datos bancarios ──────────────────────────────────────────────
        public BancoModel BancoDatos
        {
            get => _bancoDatos;
            set { _bancoDatos = value; OnPropertyChanged(nameof(BancoDatos)); }
        }

        private ObservableCollection<TransaccionModel> _historialTransacciones;
        public ObservableCollection<TransaccionModel> HistorialTransacciones
        {
            get => _historialTransacciones;
            set { _historialTransacciones = value; OnPropertyChanged(nameof(HistorialTransacciones)); }
        }

        private bool _hasHistorial;
        public bool HasHistorial
        {
            get => _hasHistorial;
            set { _hasHistorial = value; OnPropertyChanged(nameof(HasHistorial)); }
        }

        // ── Sub-ViewModels ────────────────────────────────────────────────
        public TransferenciaBancariaViewModel TransferenciaVM { get; }
        public InversionesViewModel InversionesVM { get; }

        // ── Tab activa ───────────────────────────────────────────────────
        public string ActiveTab
        {
            get => _activeTab;
            set
            {
                _activeTab = value;
                OnPropertyChanged(nameof(ActiveTab));
                OnPropertyChanged(nameof(IsTabMiCuenta));
                OnPropertyChanged(nameof(IsTabTransaccion));
                OnPropertyChanged(nameof(IsTabInvertir));
                OnPropertyChanged(nameof(IsTabHistorial));
            }
        }

        public bool IsTabMiCuenta   => ActiveTab == "MiCuenta";
        public bool IsTabTransaccion => ActiveTab == "Transaccion";
        public bool IsTabInvertir   => ActiveTab == "Invertir";
        public bool IsTabHistorial  => ActiveTab == "Historial";

        // ── Comandos ─────────────────────────────────────────────────────
        public ICommand VolverAlHubCommand     { get; }
        public ICommand TabMiCuentaCommand     { get; }
        public ICommand TabTransaccionCommand  { get; }
        public ICommand TabInvertirCommand     { get; }
        public ICommand TabHistorialCommand    { get; }

        public BancoViewModel()
        {
            _bancoRepository = new MysqlBancoRepository();

            VolverAlHubCommand    = new ViewModelCommand(ExecuteVolverAlHub);
            TabMiCuentaCommand    = new ViewModelCommand(_ => ActiveTab = "MiCuenta");
            TabTransaccionCommand = new ViewModelCommand(_ => { ActiveTab = "Transaccion"; TransferenciaVM.CargarSaldoActual(); });
            TabInvertirCommand    = new ViewModelCommand(_ => { ActiveTab = "Invertir"; InversionesVM.CargarDatos(); });
            TabHistorialCommand   = new ViewModelCommand(_ => ActiveTab = "Historial");

            // Crear TransferenciaVM y enlazar callback de vuelta
            TransferenciaVM = new TransferenciaBancariaViewModel
            {
                OnCerrar = () => { ActiveTab = "MiCuenta"; CargarDatosDelBanco(); }
            };

            // Crear InversionesVM y refrescar saldo tras compra/venta
            InversionesVM = new InversionesViewModel
            {
                OnInversionRealizada = () => CargarDatosDelBanco()
            };

            CargarDatosDelBanco();
        }

        private void CargarDatosDelBanco()
        {
            try
            {
                int id = UserSession.IdUsuario;
                bool esJugador = UserSession.Rol == "Jugador";
                string nombrePersona = UserSession.UsuarioActual?.DisplayName ?? "Usuario Desconocido";

                if (id > 0)
                {
                    BancoDatos = _bancoRepository.ObtenerCuentaPorUsuario(id, esJugador, nombrePersona);
                    var historial = _bancoRepository.ObtenerHistorialTransaccionesUsuario(id, esJugador);
                    HistorialTransacciones = new ObservableCollection<TransaccionModel>(historial);
                    HasHistorial = HistorialTransacciones.Count > 0;
                }
                else
                {
                    BancoDatos = new BancoModel { NombrePropietario = "Usuario Desconocido", IBAN = "N/A", Saldo = 0m, NumeroCuenta = "N/A", TipoCuenta = "Cuenta Corriente" };
                    HistorialTransacciones = new ObservableCollection<TransaccionModel>();
                    HasHistorial = false;
                }
            }
            catch
            {
                BancoDatos = new BancoModel { NombrePropietario = "Error al cargar", IBAN = "Error", Saldo = 0m, NumeroCuenta = "Error", TipoCuenta = "Cuenta Corriente" };
                HistorialTransacciones = new ObservableCollection<TransaccionModel>();
                HasHistorial = false;
            }
        }

        private void ExecuteVolverAlHub(object obj)
        {
            var hubView = new View.AppHubView();
            hubView.Show();
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
            {
                if (w.GetType().Name == "BancoView") { w.Close(); break; }
            }
        }
    }
}
