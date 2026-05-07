using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InaManager.View;
using System.Windows;

namespace InaManager.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public ICommand VolverAlLoginCommand { get; }

        public HomeViewModel()
        {
            VolverAlLoginCommand = new ViewModelCommand(ExecuteVolverAlLogin);
        }

        private void ExecuteVolverAlLogin(object obj)
        {
            // Limpiar sesión
            Models.UserSession.UsuarioActual = null;

            var loginView = new LoginView();
            loginView.Show();

            // Cerrar la ventana principal (MainView)
            foreach (Window w in Application.Current.Windows)
            {
                if (w is MainView)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}