using System.Windows;
using System.Windows.Input;

namespace InaManager.View
{
    public partial class MercadoWindow : Window
    {
        public MercadoWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnVolverHub_Click(object sender, RoutedEventArgs e)
        {
            var hub = new AppHubView();
            hub.Show();
            this.Close();
        }
    }
}
