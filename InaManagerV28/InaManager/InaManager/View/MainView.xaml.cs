using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace InaManager.View
{
    public partial class MainView : Window
    {

        private MediaPlayer _sonidoHover;
        private MediaPlayer _sonidoClick;
        public MainView()
        {
            InitializeComponent();

            // Preparamos el sonido Hover (pasar por encima)
            _sonidoHover = new MediaPlayer();
            _sonidoHover.Open(new Uri("Audio/SonidoJober.mp3", UriKind.Relative));
            _sonidoHover.Volume = 0.5;

            // Preparamos el sonido Click (seleccionar)
            _sonidoClick = new MediaPlayer();
            _sonidoClick.Open(new Uri("Audio/SonidoSeleccionar.mp3", UriKind.Relative));
            _sonidoClick.Volume = 0.8;
        }
        // Método que se ejecutará al pasar el ratón
        private void ReproducirSonidoHover(object sender, MouseEventArgs e)
        {
            _sonidoHover.Position = TimeSpan.Zero; // Vuelve al inicio rápido
            _sonidoHover.Play();
        }

        // Método que se ejecutará al hacer clic
        private void ReproducirSonidoClick(object sender, RoutedEventArgs e)
        {
            _sonidoClick.Position = TimeSpan.Zero; // Vuelve al inicio rápido
            _sonidoClick.Play();
        }
        // Permite mover la ventana arrastrando con el mouse
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // Botón Minimizar
        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Botón Cerrar
        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}