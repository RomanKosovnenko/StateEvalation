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
    public class NameToMultiBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value?.ToString();
            switch (input)
            {
                case "1":
                    return Brushes.Violet;
                case "2":
                    return Brushes.DarkBlue;
                case "3":
                    return Brushes.Blue;
                case "4":
                    return Brushes.Cyan;
                case "5":
                    return Brushes.Green;
                case "6":
                    return Brushes.LightGreen;
                case "7":
                    return Brushes.Yellow;
                case "8":
                    return Brushes.Orange;
                case "9":
                    return Brushes.DarkOrange;
                case "10":
                    return Brushes.OrangeRed;
                case "11":
                    return Brushes.Red;
                case "12":
                    return Brushes.Magenta;

                default:
                    return Brushes.Black; //DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
