using System.Windows;
using InaManager.ViewModel;

namespace InaManager.View
{
    public partial class CrearAnuncioWindow : Window
    {
        public CrearAnuncioWindow()
        {
            InitializeComponent();
            this.DataContext = new CrearAnuncioViewModel();
        }
    }
}
