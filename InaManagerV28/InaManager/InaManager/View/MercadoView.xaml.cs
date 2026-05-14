using System.Windows.Controls;

namespace InaManager.View
{
    public partial class MercadoView : UserControl
    {
        public MercadoView()
        {
            InitializeComponent();
        }

        private void BtnActualizar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is InaManager.ViewModel.MercadoViewModel vm)
                vm.CargarDatos();
        }
    }
}
