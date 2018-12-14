using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using StateEvaluation.BioColor;
using StateEvaluation.ViewModel;

namespace StateEvaluation.View.Helpers
{
    public class PreferenceToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value?.ToString()?.Trim();
            string color;
            switch (input)
            {
                case "Красная":
                    color = Settings.Default.p3; // Brushes.Red;
                    break;
                case "Синяя":
                    color = Settings.Default.i3; // Brushes.Blue;
                    break;
                case "Желтая":
                    color = Settings.Default.e3; // Brushes.Yellow;
                    break;
                case "Смешанная":
                    return Brushes.Gray;

                default:
                    return Brushes.White; //DependencyProperty.UnsetValue;
            }
            return new BrushConverter().ConvertFromString("#" + color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
