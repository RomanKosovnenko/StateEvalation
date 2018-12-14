using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using StateEvaluation.ViewModel;
using System.Windows.Controls;
using System.Windows;

namespace StateEvaluation.View.Helpers
{
    public class ButtonToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                MessageBox.Show(values[0].ToString());
                bool? masterVisible = null;
                bool? slaveVisible = null;
                if (masterVisible == true && slaveVisible == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                };
            }
            catch
            {
                return Visibility.Hidden;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}