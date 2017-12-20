using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using StateEvaluation.ViewModel;

namespace StateEvaluation.Helpers
{
    public class PreferenceToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value?.ToString()?.Trim();
            switch (input)
            {
                case "Красная":
                    return Brushes.Red;
                case "Синяя":
                    return Brushes.Blue;
                case "Желтая":
                    return Brushes.Yellow;
                case "Смешанная":
                    return Brushes.Gray;

                default:
                    return Brushes.White; //DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
