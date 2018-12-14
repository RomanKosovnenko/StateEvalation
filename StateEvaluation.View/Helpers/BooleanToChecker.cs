using System;
using System.Windows.Data;

namespace StateEvaluation.View.Helpers
{
    public class BooleanToChecker : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "---";
            }
            return value.ToString().Trim() == "true" ? "✓" : "✘";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
