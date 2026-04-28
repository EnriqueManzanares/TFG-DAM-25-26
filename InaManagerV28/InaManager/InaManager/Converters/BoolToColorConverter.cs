using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace InaManager.Converters // Ajusta el namespace si es necesario
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // La lógica aquí asume que el binding es "IsOccupied" (bool)
            // Pero en el ViewModel usé un truco: si es Portero ("PR"), quiero otro color.
            // Para simplificar, si quieres distinguir al portero visualmente en el XAML, 
            // lo ideal es bindear al "Codigo" o usar un estilo Trigger.

            // Para no complicar el XAML que te di antes, hagamos esto simple:
            // Si está ocupado (True) -> Verde (#FFD700 es dorado, #2E7D32 es verde)
            // Si está vacío (False) -> Transparente

            if (value is bool ocupado && ocupado)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD700")); // Dorado
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}