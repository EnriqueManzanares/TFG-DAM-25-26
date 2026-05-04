using System.Windows.Input;
using InaManager.Models;
using InaManager.View;

namespace InaManager.ViewModel
{
    public class AppHubViewModel : ViewModelBase
    {
        // Nombre del usuario logueado
        public string NombreUsuario => UserSession.UsuarioActual?.DisplayName ?? "Usuario";

        // --- ALERTA "En Desarrollo" ---
        private string _alertTitle;
        public string AlertTitle { get => _alertTitle; set { _alertTitle = value; OnPropertyChanged(nameof(AlertTitle)); } }

        private string _alertMessage;
        public string AlertMessage { get => _alertMessage; set { _alertMessage = value; OnPropertyChanged(nameof(AlertMessage)); } }

        private string _alertColor;
        public string AlertColor { get => _alertColor; set { _alertColor = value; OnPropertyChanged(nameof(AlertColor)); } }

        private bool _isAlertVisible;
        public bool IsAlertVisible { get => _isAlertVisible; set { _isAlertVisible = value; OnPropertyChanged(nameof(IsAlertVisible)); } }

        // --- COMANDOS ---
        public ICommand AbrirInaManagerCommand { get; }
        public ICommand AbrirBancoCommand { get; }
        public ICommand AbrirMercadoCommand { get; }
        public ICommand CloseAlertCommand { get; }
        public ICommand VolverAlLoginCommand { get; }

        public AppHubViewModel()
        {
            AbrirInaManagerCommand = new ViewModelCommand(ExecuteAbrirInaManager);
            AbrirBancoCommand = new ViewModelCommand(ExecuteAbrirBanco);
            AbrirMercadoCommand = new ViewModelCommand(o => MostrarEnDesarrollo("Mercado de Fichajes"));
            CloseAlertCommand = new ViewModelCommand(o => IsAlertVisible = false);
            VolverAlLoginCommand = new ViewModelCommand(ExecuteVolverAlLogin);
        }

        private void ExecuteAbrirInaManager(object obj)
        {
            var mainView = new MainView();
            mainView.Show();

            // Cerramos la ventana actual del Hub
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
            {
                if (w is AppHubView)
                {
                    w.Close();
                    break;
                }
            }
        }

        private void ExecuteAbrirBanco(object obj)
        {
            var bancoView = new View.BancoView();
            bancoView.Show();

            // Cerramos la ventana actual del Hub
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
            {
                if (w is AppHubView)
                {
                    w.Close();
                    break;
                }
            }
        }

        private void ExecuteVolverAlLogin(object obj)
        {
            // Limpiar sesión
            UserSession.UsuarioActual = null;
            System.Threading.Thread.CurrentPrincipal = null;

            var loginView = new LoginView();
            loginView.Show();

            // Cerrar el Hub
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
            {
                if (w is AppHubView)
                {
                    w.Close();
                    break;
                }
            }
        }

        private void MostrarEnDesarrollo(string nombreApp)
        {
            AlertTitle = "En Desarrollo";
            AlertMessage = $"La aplicación \"{nombreApp}\" está actualmente en desarrollo y estará disponible próximamente.";
            AlertColor = "#E67E22"; // Naranja
            IsAlertVisible = true;
        }
    }
}
