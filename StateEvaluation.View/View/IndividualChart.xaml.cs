using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LiveCharts;
using StateEvaluation.Repository.Models;
using StateEvaluation.Repository.Providers;

namespace StateEvaluation.View
{
    /// <summary>
    /// Interaction logic for IndividualChart.xaml
    /// </summary>
    public partial class IndividualChart : Window
    {
        Preference test = new Preference();
        DataRepository DataRepository { get; set; }
        ChartValues<ViewModel> values1 = new ChartValues<ViewModel>();
        ChartValues<ViewModel> values2 = new ChartValues<ViewModel>();
        //public IndividualChart(List<byte> LongSTR, string title )
        public IndividualChart(Preference Item, DataRepository dataRepository)
        {
            DataRepository = dataRepository;
            test = Item;
            InitializeComponent();
            Series = new SeriesCollection { };
            DataContext = this;

            if (values1.Count == 0)
            {
                foreach (var oder in test.Oder1.Split(','))
                {
                    values1.Add(new ViewModel { YValue = GetIntensiveByNumber(byte.Parse(oder)) });
                }
            }
            if (values2.Count == 0)
            {
                foreach (var oder in test.Oder1.Split(','))
                {
                    values2.Add(new ViewModel { YValue = GetWaveByNumber(byte.Parse(oder)) });
                }
            }

            DrawGraph();
            this.Show();
        }

        private double GetIntensiveByNumber(byte v)
        {
            return DataRepository.Color.Single(item => item.ColorNumber == Convert.ToInt32(v)).Intensity;
        }

        public SeriesCollection Series { get; set; }


        private void AnimationImprovementLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //This is only to display animation everytime you change the view
            Chart.Update();
        }
        private int GetWaveByNumber(byte number)
        {
            return DataRepository.Color.Single(item => item.ColorNumber == Convert.ToInt32(number)).WaveLengthMax;
        }

        private void DrawGraph()
        {
            switch (Convert.ToInt32(Graph1.IsChecked ?? true) + Convert.ToInt32(Graph2.IsChecked ?? true) * 2)
            {
                case 0:
                    Series.Clear();
                    break;
                case 1:
                    Series.Clear();
                    Series.Add(
                        new LineSeries
                        {
                            LineSmoothness = 0,
                            Fill = Brushes.Transparent,
                            Foreground = Brushes.Blue,
                            Stroke = Brushes.Blue,
                            Title = test.UserId + "    " + test.Date.Date.ToShortDateString(),
                            Values = values2,
                            //StrokeDashArray = new DoubleCollection { 1 },
                            DataLabels = true
                        }.Setup(new SeriesConfiguration<ViewModel>().Y(vm => vm.YValue))
                        );
                    break;
                case 2:
                    Series.Clear();
                    Series.Add(
                        new LineSeries
                        {
                            LineSmoothness = 0,
                            Fill = Brushes.Transparent,
                            Foreground = Brushes.Red,
                            Stroke = Brushes.Red,
                            ScalesYAt = 1,
                            Title = test.UserId + "    " + test.Date.Date.ToShortDateString(),
                            Values = values1,
                            //StrokeDashArray = new DoubleCollection { 1 },
                            DataLabels = true
                        }.Setup(new SeriesConfiguration<ViewModel>().Y(vm => vm.YValue))
                        );
                    break;
                case 3:
                    Series.Clear();
                    Series.Add(
                        new LineSeries
                        {
                            LineSmoothness = 0,
                            Fill = Brushes.Transparent,
                            Foreground = Brushes.Blue,
                            Stroke = Brushes.Blue,
                            Title = test.UserId + "    " + test.Date.Date.ToShortDateString(),
                            Values = values2,
                            //StrokeDashArray = new DoubleCollection { 1 },
                            DataLabels = true
                        }.Setup(new SeriesConfiguration<ViewModel>().Y(vm => vm.YValue))
                    );
                    Series.Add(
                        new LineSeries
                        {
                            LineSmoothness = 0,
                            Fill = Brushes.Transparent,
                            Foreground = Brushes.Red,
                            Stroke = Brushes.Red,
                            ScalesYAt = 1,
                            Title = test.UserId + "    " + test.Date.Date.ToShortDateString(),
                            Values = values1,
                            //StrokeDashArray = new DoubleCollection { 1 },
                            DataLabels = true
                        }.Setup(new SeriesConfiguration<ViewModel>().Y(vm => vm.YValue))
                    );
                    break;
            }
        }

        private void ChartChanger_Click(object sender, RoutedEventArgs e)
        {
            DrawGraph();
        }
    }

    public class ViewModel : IObservableChartPoint
    {
        private double _yValue;

        public double YValue
        {
            get { return _yValue; }
            set
            {
                _yValue = value;
                if (PointChanged != null) PointChanged.Invoke(this);
            }
        }

        public event Action<object> PointChanged;
    }
}
