using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using InaManager.Models;
using InaManager.ViewModel;

namespace InaManager.View
{
    public partial class PartidosView : UserControl
    {
        public PartidosView()
        {
            InitializeComponent();
        }

        // HE BORRADO EL MÉTODO 'Calendario_DateChanged' PORQUE YA NO ES NECESARIO.
        // El ViewModel se encarga de todo automáticamente.
    }

    // EL CONVERTIDOR SE QUEDA IGUAL, ES NECESARIO PARA LOS COLORES
    public class DayColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is DateTime date && values[1] is IEnumerable<AgendaItemModel> eventos)
            {
                DateOnly fechaCalendario = DateOnly.FromDateTime(date);
                // Usamos cadenas exactas "Partido" y "Entreno" (o "Entrenamiento") según tu modelo
                bool hayPartido = eventos.Any(e => e.Fecha == fechaCalendario && e.Tipo == "Partido");
                // Asegúrate que en tu BD/Modelo se llame "Entreno" o "Entrenamiento"
                bool hayEntreno = eventos.Any(e => e.Fecha == fechaCalendario && (e.Tipo == "Entreno" || e.Tipo == "Entrenamiento"));

                if (hayPartido && hayEntreno)
                {
                    LinearGradientBrush gradient = new LinearGradientBrush();
                    gradient.StartPoint = new Point(0, 0);
                    gradient.EndPoint = new Point(1, 1);
                    gradient.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#E74C3C"), 0.5));
                    gradient.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#3498DB"), 0.5));
                    return gradient;
                }

                if (hayPartido) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E74C3C"));
                if (hayEntreno) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3498DB"));
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}