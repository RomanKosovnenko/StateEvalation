using StateEvaluation.Biocolor;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace StateEvaluation.BioColor.Helpers
{
    public class NameToMultiBrushConverter : IValueConverter
    {
        BiocolorSettings biocolorSettings;

        public NameToMultiBrushConverter()
        {
            biocolorSettings = new BiocolorSettings();
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value?.ToString();
            string color;
            switch (input)
            {
                case "1":
                    color = biocolorSettings.I1; // Brushes.Violet;
                    break;
                case "2":
                    color = biocolorSettings.I2; // Brushes.DarkBlue;
                    break;
                case "3":
                    color = biocolorSettings.I3; // Brushes.Blue;
                    break;
                case "4":
                    color = biocolorSettings.I4; // Brushes.Cyan;
                    break;
                case "5":
                    color = biocolorSettings.E1; // Brushes.Green;
                    break;
                case "6":
                    color = biocolorSettings.E2; // Brushes.LightGreen;
                    break;
                case "7":
                    color = biocolorSettings.E3; // Brushes.Yellow;
                    break;
                case "8":
                    color = biocolorSettings.E4; // Brushes.Orange;
                    break;
                case "9":
                    color = biocolorSettings.P1; // Brushes.DarkOrange;
                    break;
                case "10":
                    color = biocolorSettings.P2; // Brushes.OrangeRed;
                    break;
                case "11":
                    color = biocolorSettings.P3; // Brushes.Red;
                    break;
                case "12":
                    color = biocolorSettings.P4; // Brushes.Magenta;
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
