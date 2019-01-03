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
        int ChartType = 1;
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
            ChartValues<ViewModel> values = new ChartValues<ViewModel>();
            foreach (var oder in test.Oder1.Split(','))
            {
                values.Add(new ViewModel { YValue = GetWaveByNumber(byte.Parse(oder)) });
            }
            Series = new SeriesCollection
            {
                new LineSeries
                {
                    LineSmoothness = 0,

                    Fill = Brushes.Transparent,
                    Foreground = Brushes.Blue,
                    Stroke =Brushes.Blue,
                    Title = Item.UserId + "    " + Item.Date.Date.ToShortDateString(),
                    Values = values,
                    //StrokeDashArray = new DoubleCollection { 1 },
                    DataLabels = true
                }.Setup(new SeriesConfiguration<ViewModel>().Y(vm => vm.YValue))

        };

            DataContext = this;
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

        private void ChartChanger_Click(object sender, RoutedEventArgs e)
        {
            
            switch (ChartType)
            {
                case 1:
                    Series.Clear();
                    if (values1.Count == 0)
                    {
                        foreach (var oder in test.Oder1.Split(','))
                        {
                            values1.Add(new ViewModel {YValue = GetIntensiveByNumber(byte.Parse(oder))});
                        }
                    }
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
                    ChartType = 2;
                    break;
                case 2:
                    Series.Clear();
                    if (values2.Count == 0)
                    {
                        foreach (var oder in test.Oder1.Split(','))
                        {
                            values2.Add(new ViewModel {YValue = GetWaveByNumber(byte.Parse(oder))});
                        }
                    }
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
                    ChartType = 3;
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
                    ChartType = 1;
                    break;
            }
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
