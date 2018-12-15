using System;
using System.Windows.Data;
using System.Windows.Media;
using StateEvaluation.Biocolor;

namespace StateEvaluation.BioColor.Helpers
{
    public class NameToBrushConverter : IValueConverter
    {
        BiocolorSettings biocolorSettings;

        public NameToBrushConverter()
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
                case "2":
                case "3":
                case "4":
                    color = biocolorSettings.I3; // Brushes.Blue;
                    break;
                case "5":
                case "6":
                case "7":
                case "8":
                    color = biocolorSettings.E3; // Brushes.Yellow;
                    break;
                case "9":
                case "10":
                case "11":
                case "12":
                    color = biocolorSettings.P3; // Brushes.Red;
                    break;

                default:
                    return Brushes.Black; //DependencyProperty.UnsetValue;
            }
            return new BrushConverter().ConvertFromString("#" + color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
