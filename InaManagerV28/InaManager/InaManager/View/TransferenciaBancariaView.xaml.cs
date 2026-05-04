using System.Windows;

namespace InaManager.View
{
    public partial class TransferenciaBancariaView : Window
    {
        public TransferenciaBancariaView()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                try
                {
                    DragMove();
                }
                catch { }
            }
        }
    }
}
