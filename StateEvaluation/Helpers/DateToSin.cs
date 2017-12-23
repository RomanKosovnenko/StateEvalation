using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StateEvaluation.Helpers
{
    public class DateToSin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //    DateTime nw = DateTime.Today;
            string userid = value.ToString().Trim();


            MainWindow.people.TryGetValue(userid, out string birthday);

            if (birthday == null || birthday.Trim().Length == 0)
            {
                return "---";
            }

            DateTime nw = DateTime.Now;
            DateTime dt = System.Convert.ToDateTime(birthday);
            double Delta = (int)((nw.Subtract(dt)).TotalDays);

            int.TryParse(parameter.ToString(), out int DAYS);

            double sta = -Math.Sin((0.0 + Delta / DAYS * 360) / 180 * Math.PI);
            double now = -Math.Sin((0.1 + Delta / DAYS * 360) / 180 * Math.PI);
            return String.Format("{0} ({1})", now > sta ? "↗" : "↘", (int)Delta % DAYS);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
