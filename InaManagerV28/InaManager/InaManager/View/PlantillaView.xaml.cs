using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using InaManager.Models;
using InaManager.ViewModel;

namespace InaManager.View
{
    public partial class PlantillaView : UserControl
    {
        public PlantillaView()
        {
            InitializeComponent();
            // Refrescar datos cada vez que la vista se cargue en pantalla
            this.Loaded += (s, e) => {
                if (this.DataContext is PlantillaViewModel vm)
                {
                    vm.CargarDatos();
                }
            };
        }

        private void Jugador_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;

            JugadorModel jugadorArrastrado = null;

            if (frameworkElement is DataGridRow row)
            {
                jugadorArrastrado = row.DataContext as JugadorModel;
            }
            else if (frameworkElement.DataContext is JugadorModel j)
            {
                jugadorArrastrado = j;
            }
            else if (frameworkElement.DataContext is CeldaEsquema celda && celda.Jugador != null)
            {
                jugadorArrastrado = celda.Jugador;
            }

            if (jugadorArrastrado != null)
            {
                DragDrop.DoDragDrop(frameworkElement, jugadorArrastrado, DragDropEffects.Move);
                e.Handled = true;
            }
        }

        private void Celda_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(JugadorModel)))
            {
                var jugador = (JugadorModel)e.Data.GetData(typeof(JugadorModel));
                var frameworkElement = sender as FrameworkElement;
                var celdaDestino = frameworkElement?.DataContext as CeldaEsquema;

                if (celdaDestino != null && this.DataContext is PlantillaViewModel vm)
                {
                    vm.MoverJugador(jugador, celdaDestino);
                }
            }
        }

        private void Reservas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(JugadorModel)))
            {
                var jugador = (JugadorModel)e.Data.GetData(typeof(JugadorModel));
                if (this.DataContext is PlantillaViewModel vm)
                {
                    vm.MoverJugador(jugador, "Reservas");
                }
            }
        }

        private void Celda_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(JugadorModel)))
            {
                e.Effects = DragDropEffects.None;
            }
        }
        private void Mister_Click(object sender, MouseButtonEventArgs e)
        {
            var vm = (PlantillaViewModel)this.DataContext;
            vm.MostrarSelectorMister = true;
        }

        private void SelectorMister_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (PlantillaViewModel)this.DataContext;
            if (e.AddedItems.Count > 0)
            {
                vm.SeleccionarNuevoMister(e.AddedItems[0] as EmpleadoModel);
            }
        }
    }
}