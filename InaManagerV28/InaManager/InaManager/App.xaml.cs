using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using InaManager.View;

namespace InaManager
{
    public partial class App : Application
    {
        public static MediaPlayer MusicaFondo = new MediaPlayer();

        // 1. ESTE MÉTODO ES IMPOSIBLE QUE SE IGNORE. Se ejecuta el 100% de las veces al abrir la app.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                string ruta = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", "MusicaMenu.mp3"));

                // Comprobamos si el archivo existe
                if (File.Exists(ruta))
                {
                    MusicaFondo.Open(new Uri(ruta, UriKind.Absolute));
                    MusicaFondo.Volume = 0.3; // Volumen al 30%

                    MusicaFondo.MediaEnded += (senderMedia, argsMedia) =>
                    {
                        MusicaFondo.Position = TimeSpan.Zero;
                        MusicaFondo.Play();
                    };

                    MusicaFondo.Play(); // ¡Que suene la música!
                }
                else
                {
                    // Si te salta esto, es que el archivo MP3 no está en la carpeta correcta
                    MessageBox.Show("No se ha encontrado la música en:\n" + ruta, "Aviso de Audio");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el audio: " + ex.Message);
            }
        }

        // =========================================================================
        // 2. EL CÓDIGO ZOMBI DE TU COMPAÑERO (Lo dejamos por si lo está usando)
        // =========================================================================
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();

            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    var mainView = new MainView();
                    mainView.Show();
                    loginView.Close();
                }
            };
        }
    }
}