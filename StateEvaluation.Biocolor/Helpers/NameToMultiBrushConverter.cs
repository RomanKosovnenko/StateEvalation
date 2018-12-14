using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using StateEvaluation.BioColor;
using StateEvaluation.Common;

namespace StateEvaluation.Biocolor.Helpers
{
    public class NameToMultiBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value?.ToString();
            string color;
            switch (input)
            {
                case "1":
                    color = Settings.Default.i1; // Brushes.Violet;
                    break;
                case "2":
                    color = Settings.Default.i2; // Brushes.DarkBlue;
                    break;
                case "3":
                    color = Settings.Default.i3; // Brushes.Blue;
                    break;
                case "4":
                    color = Settings.Default.i4; // Brushes.Cyan;
                    break;
                case "5":
                    color = Settings.Default.e1; // Brushes.Green;
                    break;
                case "6":
                    color = Settings.Default.e2; // Brushes.LightGreen;
                    break;
                case "7":
                    color = Settings.Default.e3; // Brushes.Yellow;
                    break;
                case "8":
                    color = Settings.Default.e4; // Brushes.Orange;
                    break;
                case "9":
                    color = Settings.Default.p1; // Brushes.DarkOrange;
                    break;
                case "10":
                    color = Settings.Default.p2; // Brushes.OrangeRed;
                    break;
                case "11":
                    color = Settings.Default.p3; // Brushes.Red;
                    break;
                case "12":
                    color = Settings.Default.p4; // Brushes.Magenta;
                    break;

                default:
                    return Brushes.Black; //DependencyProperty.UnsetValue;
            }
            return new BrushConverter().ConvertFromString("#"+color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
