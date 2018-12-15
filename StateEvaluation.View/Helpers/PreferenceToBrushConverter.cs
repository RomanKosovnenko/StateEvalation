using System;
using System.Windows.Data;
using System.Windows.Media;
using StateEvaluation.BioColor.Helpers;

namespace StateEvaluation.View.Helpers
{
    public class PreferenceToBrushConverter : IValueConverter
    {
        BiocolorSettings biocolorSettings;

        public PreferenceToBrushConverter()
        {
            biocolorSettings = new BiocolorSettings();
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value?.ToString()?.Trim();
            string color;
            switch (input)
            {
                case "Красная":
                    color = biocolorSettings.P3; // Brushes.Red;
                    break;
                case "Синяя":
                    color = biocolorSettings.I3; // Brushes.Blue;
                    break;
                case "Желтая":
                    color = biocolorSettings.E3; // Brushes.Yellow;
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
