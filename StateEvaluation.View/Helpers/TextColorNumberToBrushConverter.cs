using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace StateEvaluation.View.Helpers
{
    public class TextColorNumberToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value?.ToString();
            switch (input)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "9":
                case "10":
                case "11":
                case "12":
                    return Brushes.White;
                case "5":
                case "6":
                case "7":
                case "8":
                    return Brushes.Black;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
