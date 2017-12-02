using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using StateEvaluation.ViewModel;

namespace StateEvaluation.Helpers
{
    public class NameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value?.ToString();

            try
            {
                if (input?.Trim().Length != 0)
                {
                    var uri = new Uri("../../RelaxTables/Relax3/" + input + ".png", UriKind.Relative);
                    var bitmap = new BitmapImage(uri);
                    var brush = new ImageBrush(bitmap);
                    return brush;
                }
                else
                {
                    return Brushes.Black;
                }
            }
            catch
            {
                return Brushes.Black;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}