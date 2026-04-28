using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace InaManager.Converters
{
    public class NullToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNull = (value == null);

            // Lógica normal: Si es nulo -> Collapsed (Oculto)
            // Si pasamos "Inverse": Si es nulo -> Visible
            if (parameter != null && parameter.ToString() == "Inverse")
            {
                return isNull ? Visibility.Visible : Visibility.Collapsed;
            }

            return isNull ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}