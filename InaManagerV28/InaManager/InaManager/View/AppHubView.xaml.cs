using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace InaManager.View
{
    public partial class AppHubView : Window
    {
        public AppHubView()
        {
            InitializeComponent();
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
            if (WindowsMenuPopup != null)
            {
                WindowsMenuPopup.IsOpen = !WindowsMenuPopup.IsOpen;
            }
        }
    }
}
