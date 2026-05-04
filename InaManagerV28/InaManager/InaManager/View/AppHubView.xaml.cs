using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace InaManager.View
{
    public partial class AppHubView : Window
    {
        private Popup _windowsMenuPopup;

        public AppHubView()
        {
            InitializeComponent();
            // Crear instancia del Popup desde resources
            _windowsMenuPopup = this.Resources["WindowsMenuPopup"] as Popup;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_CerrarSesion(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as dynamic;
            if (viewModel?.VolverAlLoginCommand != null)
            {
                viewModel.VolverAlLoginCommand.Execute(null);
            }
        }

        private void WindowsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_windowsMenuPopup != null)
            {
                _windowsMenuPopup.PlacementTarget = sender as UIElement;
                _windowsMenuPopup.IsOpen = !_windowsMenuPopup.IsOpen;
            }
        }
    }
}
