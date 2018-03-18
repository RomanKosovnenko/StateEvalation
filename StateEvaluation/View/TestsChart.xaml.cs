using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StateEvaluation.Model;

namespace StateEvaluation.View
{
    public partial class TestsChart : Window
    {


        public TestsChart(List<Preference> Tests, bool isPreference1)
        {
            InitializeComponent();

            Sales = new SalesViewModel(Tests, isPreference1);
            DataContext = this;
            YFormatter = x => x + "";
            this.Title = isPreference1 ? "Диаграма преференций 1" : "Диаграма преференций 2";
            this.Show();
        }
        public TestsChart(List<Preference> Tests, bool isPreference1, bool isOneDay): this(Tests, isPreference1)
        {
            if (isOneDay)
            {
                AxisY.MaxValue = 12;
            }
        }

        private List<Preference> MakeTests(List<Preference> Tests)
        {
            List<Preference> tests = new List<Preference>();
            foreach (var item in Tests)
            {
                tests.Add(item);
                //if (!(Months.Contains(_months[item.TestDate.Month] + " " + item.TestDate.Year.ToString())))
                //{
                //    Months.Add(_months[item.TestDate.Month] + " " + item.TestDate.Year.ToString());
                //}
            }
            return tests;
        }

        public SalesViewModel Sales { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private string GetLeedingZero(int x) {
            if(x < 10)
            {
                return "0" + x;
            }
            else
            {
                return "" + x;
            }
        }
        private void SaveImage(object sender, RoutedEventArgs e)
        {
            SaveButton.Visibility = Visibility.Hidden;

            var rtb = new RenderTargetBitmap(
                (int) Width, 
                (int) Height, 
                96d, 
                96d,
                PixelFormats.Pbgra32
                );

            rtb.Render(Chart);

            var currentDate = new DateTime(DateTime.Now.Ticks);
            SaveRTBAsPNG(rtb, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Result_" + currentDate.Year + GetLeedingZero(currentDate.Month) + GetLeedingZero(currentDate.Day) + "_" + GetLeedingZero(currentDate.Hour) + GetLeedingZero(currentDate.Minute) + GetLeedingZero(currentDate.Second) + ".png");

            MessageBox.Show("Chart saved!");
            SaveButton.Visibility = Visibility.Visible;
        }

        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new PngBitmapEncoder(); 
            enc.Frames.Add(BitmapFrame.Create(bmp));
            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
        private void MvvmExample_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.Update();
        }

    }

    public class SalesData
    {
        public int Count { get; set; }
    }

    public class SalesViewModel
    {
        private readonly string[] _months1 =
        {
            "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
        };

        public SeriesCollection SalesmenSeries { get; set; }
        public List<string> AvailableMonthsList = new List<string>();
        public string[] AvailableMonths { get; set; }

        public SalesViewModel(List<Preference> Tests, bool isPreference1)
        {
            //AvailableMonths = _months1;
            foreach (var item in Tests)
            {
                if (!(AvailableMonthsList.Contains(_months1[item.Date.Month - 1] + " " + item.Date.Year.ToString())))
                {
                    AvailableMonthsList.Add(_months1[item.Date.Month - 1] + " " + item.Date.Year.ToString());
                }
            }
            AvailableMonths = new string[AvailableMonthsList.Count];

            for (int t = 0; t < AvailableMonthsList.Count; ++t)
            {
                AvailableMonths[t] = AvailableMonthsList[t];
            }

            SalesmenSeries = new SeriesCollection();
            SalesmenSeries.Add(
                new BarSeries
                {
                    Title = "Синяя",
                    Values = new ChartValues<SalesData>()
                });
            SalesmenSeries.Add(
                new BarSeries
                {
                    Title = "Красная",
                    Values = new ChartValues<SalesData>()
                });
            SalesmenSeries.Add(
                new BarSeries
                {
                    Title = "Желтая",
                    Values = new ChartValues<SalesData>()
                });

            SalesmenSeries.Add(
                new BarSeries
                {
                    Title = "Серая",
                    Values = new ChartValues<SalesData>()
                });
            SalesmenSeries.Setup(new SeriesConfiguration<SalesData>().Y(data => data.Count)); // Setup a default configuration for all series in this collection.
            int i = 0;
            foreach (var month in AvailableMonths)
            {
                foreach (var item in Tests)
                {

                    if (_months1[item.Date.Month - 1] + " " + item.Date.Year.ToString() == month)
                    {
                        string itemPreference = isPreference1 ? item.Preference1 : itemPreference = item.Preference2;
                        switch (itemPreference.TrimEnd((' ')))
                        {
                            case "Синяя":
                                if (SalesmenSeries[0].Values.Count == i)
                                {
                                    SalesmenSeries[0].Values.Add(new SalesData { Count = 1 });
                                    continue;
                                }
                                try
                                {
                                    (SalesmenSeries[0].Values[i] as SalesData).Count += 1;
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    while (SalesmenSeries[0].Values.Count != i + 1)
                                    {
                                        SalesmenSeries[0].Values.Add(new SalesData { Count = 0 });
                                    }
                                    (SalesmenSeries[0].Values[i] as SalesData).Count += 1;
                                }
                                break;
                            case "Красная":
                                if (SalesmenSeries[1].Values.Count == i)
                                {
                                    SalesmenSeries[1].Values.Add(new SalesData { Count = 1 });
                                    continue;
                                }
                                try
                                {
                                    (SalesmenSeries[1].Values[i] as SalesData).Count += 1;
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    while (SalesmenSeries[1].Values.Count != i + 1)
                                    {
                                        SalesmenSeries[1].Values.Add(new SalesData { Count = 0 });
                                    }
                                    (SalesmenSeries[1].Values[i] as SalesData).Count += 1;
                                }
                                break;
                            case "Желтая":
                                if (SalesmenSeries[2].Values.Count == i)
                                {
                                    SalesmenSeries[2].Values.Add(new SalesData { Count = 1 });
                                    continue;
                                }
                                try
                                {
                                    (SalesmenSeries[2].Values[i] as SalesData).Count += 1;
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    while (SalesmenSeries[2].Values.Count != i + 1)
                                    {
                                        SalesmenSeries[2].Values.Add(new SalesData { Count = 0 });
                                    }
                                    (SalesmenSeries[2].Values[i] as SalesData).Count += 1;
                                }
                                break;
                            case "Смешанная":
                                if (SalesmenSeries[3].Values.Count == i)
                                {
                                    SalesmenSeries[3].Values.Add(new SalesData { Count = 1 });
                                    continue;
                                }
                                try
                                {
                                    (SalesmenSeries[3].Values[i] as SalesData).Count += 1;
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    while (SalesmenSeries[3].Values.Count != i + 1)
                                    {
                                        SalesmenSeries[3].Values.Add(new SalesData { Count = 0 });
                                    }
                                    (SalesmenSeries[3].Values[i] as SalesData).Count += 1;
                                }
                                break;
                        }
                    }
                }
                ++i;
            }
        }

    }
}

