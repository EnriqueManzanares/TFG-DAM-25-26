using System.Windows.Controls;

namespace InaManager.View
{
    // Es MUY importante que ponga "partial class" y herede de "UserControl"
    public partial class EjerciciosView : UserControl
    {
        public EjerciciosView()
        {
            // ¡Esta es la orden mágica que dibuja tu XAML!
            InitializeComponent();
        }
    }
}