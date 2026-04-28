using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace InaManager.Converters
{
    public class BoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = false;
            if (value is bool)
            {
                bValue = (bool)value;
            }

            // Si pasamos el parámetro "Inverse", invertimos la lógica
            if (parameter != null && parameter.ToString() == "Inverse")
            {
                return bValue ? Visibility.Collapsed : Visibility.Visible;
            }

            return bValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                bool isVisible = visibility == Visibility.Visible;
                if (parameter != null && parameter.ToString() == "Inverse")
                    return !isVisible;
                return isVisible;
            }
            return false;
        }
    }
}