using StateEvaluation.BussinesLayer.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace StateEvaluation.View.Helpers
{
    class PreferenceSameAsRelax : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string preference = values[0].ToString().Trim();
            int.TryParse(values[1]?.ToString()?.Trim(), out int relax);

            bool selector = false;

            switch (preference)
            {
                case PreferenceColors.Red:
                    selector = IsNumberInRange(relax, 1, 4);
                    break;
                case PreferenceColors.Blue:
                    selector = IsNumberInRange(relax, 5, 8);
                    break;
                case PreferenceColors.Yellow:
                    selector = IsNumberInRange(relax, 9, 12);
                    break;
                case PreferenceColors.Gray:
                    selector = false;
                    break;

                default:
                    return "---"; //DependencyProperty.UnsetValue;
            }
            return selector ? "✓" : "✘";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public bool IsNumberInRange(int x, int f, int t)
        {
            return f <= x && x <= t;
        }
    }
}
