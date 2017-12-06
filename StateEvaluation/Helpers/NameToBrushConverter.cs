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
    public class NameToBrushConverter : IValueConverter
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
                    return Brushes.Blue;
                case "5":
                case "6":
                case "7":
                case "8":
                    return Brushes.Yellow;
                case "9":
                case "10":
                case "11":
                case "12":
                    return Brushes.Red;

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
