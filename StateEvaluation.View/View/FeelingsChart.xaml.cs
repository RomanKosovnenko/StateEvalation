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

namespace StateEvaluation.View
{
    /// <summary>
    /// Interaction logic for FeelingsChart.xaml
    /// </summary>
    public partial class FeelingsChart : Window
    {
    //    StateEvaluationDBEntities DataBase = new StateEvaluationDBEntities();
    //    public SeriesCollection Series { get; set; }
    //    private readonly string[] _months1 =
    //    {
    //        "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
    //    };
    //    public string[] AvailableMonths { get; set; }

    //    public FeelingsChart(List<SubjectiveFeelingsTest> Tests)
    //    {
            
    //        List<string> AvailableMonthsList = new List<string>();
            
    //        InitializeComponent();

    //        foreach (var item in Tests)
    //        {
    //            if (!(AvailableMonthsList.Contains(_months1[item.Date.Month - 1] + " " + item.Date.Year.ToString())))
    //            {
    //                AvailableMonthsList.Add(_months1[item.Date.Month - 1] + " " + item.Date.Year.ToString());
    //            }
    //        }
    //        AvailableMonths = new string[AvailableMonthsList.Count];

    //        for (int t = 0; t < AvailableMonthsList.Count; ++t)
    //        {
    //            AvailableMonths[t] = AvailableMonthsList[t];
    //        }

    //        Series = new SeriesCollection();
    //        Series.Add(
    //            new LineSeries
    //            {
    //                LineSmoothness = 0,
    //                Fill = Brushes.Transparent,
    //                Foreground = Brushes.Blue,
    //                Stroke = Brushes.Blue,
    //                Title = "Общая слабость",
    //                Values = new ChartValues<ViewModelFeelings>()
    //            }.Setup(new SeriesConfiguration<ViewModelFeelings>().Y(vm => vm.YValue)));
    //        Series.Add(
    //            new LineSeries
    //            {
    //                LineSmoothness = 0,
    //                Fill = Brushes.Transparent,
    //                Foreground = Brushes.Black,
    //                Stroke = Brushes.Black,
    //                Title = "Плохой аппетит",
    //                Values = new ChartValues<ViewModelFeelings>()
    //            }.Setup(new SeriesConfiguration<ViewModelFeelings>().Y(vm => vm.YValue)));
    //        Series.Add(
    //            new LineSeries
    //            {
    //                LineSmoothness = 0,
    //                Fill = Brushes.Transparent,
    //                Foreground = Brushes.DarkOrange,
    //                Stroke = Brushes.DarkOrange,
    //                Title = "Плохой сон",
    //                Values = new ChartValues<ViewModelFeelings>()
    //            }.Setup(new SeriesConfiguration<ViewModelFeelings>().Y(vm => vm.YValue)));

    //        Series.Add(
    //            new LineSeries
    //            {
    //                LineSmoothness = 0,
    //                Fill = Brushes.Transparent,
    //                Foreground = Brushes.Gray,
    //                Stroke = Brushes.Gray,
    //                Title = "Плохое настроение",
    //                Values = new ChartValues<ViewModelFeelings>()
    //            }.Setup(new SeriesConfiguration<ViewModelFeelings>().Y(vm => vm.YValue)));
    //        Series.Add(
    //            new LineSeries
    //            {
    //                LineSmoothness = 0,
    //                Fill = Brushes.Transparent,
    //                Foreground = Brushes.Green,
    //                Stroke = Brushes.Green,
    //                Title = "Тяжесть в голове",
    //                Values = new ChartValues<ViewModelFeelings>()
    //            }.Setup(new SeriesConfiguration<ViewModelFeelings>().Y(vm => vm.YValue)));
    //        Series.Add(
    //            new LineSeries
    //            {
    //                LineSmoothness = 0,
    //                Fill = Brushes.Transparent,
    //                Foreground = Brushes.Navy,
    //                Stroke = Brushes.Navy,
    //                Title = "Замедленное мышление",
    //                Values = new ChartValues<ViewModelFeelings>()
    //            }.Setup(new SeriesConfiguration<ViewModelFeelings>().Y(vm => vm.YValue)));

    //        int i = 0;
    //        foreach (var month in AvailableMonths)
    //        {
    //            foreach (var item in Tests)
    //            {

    //                if (_months1[item.Date.Month - 1] + " " + item.Date.Year.ToString() == month)
    //                {
    //                    if (item.GeneralWeaknes == true)
    //                    {
    //                        if (Series[0].Values.Count == i)
    //                        {
    //                            Series[0].Values.Add(new ViewModelFeelings { YValue = 1 });
    //                            continue;
    //                        }
    //                        try
    //                        {
    //                            (Series[0].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                        catch (ArgumentOutOfRangeException)
    //                        {
    //                            while (Series[0].Values.Count != i + 1)
    //                            {
    //                                Series[0].Values.Add(new ViewModelFeelings { YValue = 0 });
    //                            }
    //                            (Series[0].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                    }


    //                    if (item.PoorAppetite == true)
    //                    {
    //                        if (Series[1].Values.Count == i)
    //                        {
    //                            Series[1].Values.Add(new ViewModelFeelings { YValue = 1 });
    //                            continue;
    //                        }
    //                        try
    //                        {
    //                            (Series[1].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                        catch (ArgumentOutOfRangeException)
    //                        {
    //                            while (Series[1].Values.Count != i + 1)
    //                            {
    //                                Series[1].Values.Add(new ViewModelFeelings { YValue = 0 });
    //                            }
    //                            (Series[1].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                    }
    //                    if (item.PoorSleep == true)
    //                    {
    //                        if (Series[2].Values.Count == i)
    //                        {
    //                            Series[2].Values.Add(new ViewModelFeelings { YValue = 1 });
    //                            continue;
    //                        }
    //                        try
    //                        {
    //                            (Series[2].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                        catch (ArgumentOutOfRangeException)
    //                        {
    //                            while (Series[2].Values.Count != i + 1)
    //                            {
    //                                Series[2].Values.Add(new ViewModelFeelings { YValue = 0 });
    //                            }
    //                            (Series[2].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                    }
    //                    if (item.BadMood == true)
    //                    {
    //                        if (Series[3].Values.Count == i)
    //                        {
    //                            Series[3].Values.Add(new ViewModelFeelings { YValue = 1 });
    //                            continue;
    //                        }
    //                        try
    //                        {
    //                            (Series[3].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                        catch (ArgumentOutOfRangeException)
    //                        {
    //                            while (Series[3].Values.Count != i + 1)
    //                            {
    //                                Series[3].Values.Add(new ViewModelFeelings { YValue = 0 });
    //                            }
    //                            (Series[3].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                    }
    //                    if (item.HeavyHead == true)
    //                    {
    //                        if (Series[4].Values.Count == i)
    //                        {
    //                            Series[4].Values.Add(new ViewModelFeelings { YValue = 1 });
    //                            continue;
    //                        }
    //                        try
    //                        {
    //                            (Series[4].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                        catch (ArgumentOutOfRangeException)
    //                        {
    //                            while (Series[4].Values.Count != i + 1)
    //                            {
    //                                Series[4].Values.Add(new ViewModelFeelings { YValue = 0 });
    //                            }
    //                            (Series[4].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                    }
    //                    if (item.SlowThink == true)
    //                    {
    //                        if (Series[5].Values.Count == i)
    //                        {
    //                            Series[5].Values.Add(new ViewModelFeelings { YValue = 1 });
    //                            continue;
    //                        }
    //                        try
    //                        {
    //                            (Series[5].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                        catch (ArgumentOutOfRangeException)
    //                        {
    //                            while (Series[5].Values.Count != i + 1)
    //                            {
    //                                Series[5].Values.Add(new ViewModelFeelings { YValue = 0 });
    //                            }
    //                            (Series[5].Values[i] as ViewModelFeelings).YValue += 1;
    //                        }
    //                    }
    //                }
    //            }
    //            ++i;
    //        }

    //        DataContext = this;
    //        this.Show();
    //    }
    //}

    //public class ViewModelFeelings : IObservableChartPoint
    //{
    //    private double _yValue;

    //    public double YValue
    //    {
    //        get { return _yValue; }
    //        set
    //        {
    //            _yValue = value;
    //            if (PointChanged != null) PointChanged.Invoke(this);
    //        }
    //    }

    //    public event Action<object> PointChanged;
    }
}
