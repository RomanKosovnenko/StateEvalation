//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    /// <summary>
    /// Creates a collection of values ready to plot
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChartValues<T> : ObservableCollection<T>, IChartValues
    {
        private Point _min = new Point(double.MaxValue, double.MaxValue);
        private Point _max = new Point(double.MinValue, double.MinValue);
        private ChartPoint[] _points = {};
        private bool _limitsChanged = true;

        public ChartValues()
        {
            CollectionChanged += OnChanged;
        }

        #region Properties

        public IChartSeries Series { get; set; }
       
        /// <summary>
        /// Gets the collection of points displayed in the chart current view
        /// </summary>
        public IEnumerable<ChartPoint> Points
        {
            get
            {
                if (_limitsChanged)
                {
                    _limitsChanged = false;
                    if (Series == null) return Enumerable.Empty<ChartPoint>();

                    var config = (Series.Configuration ?? Series.Collection.Configuration) as SeriesConfiguration<T>;

                    if (config == null) return Enumerable.Empty<ChartPoint>();

                    var q = IndexData(config);

                    if (config.DataOptimization == null)
                    {
                        _points = q.Select(t => new ChartPoint
                        {
                            X = config.XValueMapper(t.Value, t.Key),
                            Y = config.YValueMapper(t.Value, t.Key),
                            Instance = t.Value,
                            Key = t.Key
                        }).ToArray();
                        return _points;
                    }

                    config.DataOptimization.Chart = config.Chart;
                    config.DataOptimization.XMapper = config.XValueMapper;
                    config.DataOptimization.YMapper = config.YValueMapper;
                    var collection = Series == null ? null : Series.Collection;
                    _points = collection == null
                        ? new ChartPoint[] {}
                        : config.DataOptimization.Run(q).ToArray();
                    return _points.DefaultIfEmpty(new ChartPoint());
                }
                return _points;
            }
        }

        /// <summary>
        /// Gets Max X and Y coordinates in chart
        /// </summary>
        public Point MaxChartPoint
        {
            get { return _max; }
        }

        /// <summary>
        /// Gets Min X and Y coordintes in chart
        /// </summary>
        public Point MinChartPoint
        {
            get { return _min; }
        }
        public bool RequiresEvaluation { get; set; }

        #endregion

        #region Public Methods

        public ChartValues<T> AddRange(IEnumerable<T> collection)
        {
            CheckReentrancy();
            foreach (var item in collection) Items.Add(item);
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return this;
        }

        public void GetLimits()
        {
            if (RequiresEvaluation)
            {
                EvaluateLimits();
                _limitsChanged = true;
                RequiresEvaluation = false;
            }
        }

        #endregion

        #region Private Methods

        private void EvaluateLimits()
        {
            var config = (Series.Configuration ?? Series.Collection.Configuration) as SeriesConfiguration<T>;
            if (config == null) return;
            if (config.DataOptimization != null)
            {
                config.DataOptimization.Chart = config.Chart;
                config.DataOptimization.XMapper = config.XValueMapper;
                config.DataOptimization.YMapper = config.YValueMapper;
            }
            
            var q = IndexData(config).ToArray();

            var xs = q.Select(t => config.XValueMapper(t.Value, t.Key)).DefaultIfEmpty(0).ToArray();
            var xMax = xs.Where(x => !double.IsNaN(x)).Max();
            var xMin = xs.Where(x => !double.IsNaN(x)).Min();
            _min.X = xMin;
            _max.X = xMax;

            var ys = q.Select(t => config.YValueMapper(t.Value, t.Key)).DefaultIfEmpty(0).ToArray();
            var yMax = ys.Where(x => !double.IsNaN(x)).Max();
            var yMin = ys.Where(x => !double.IsNaN(x)).Min();

            _min.Y = yMin;
            _max.Y = yMax;
        }

        private void OnChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            RequiresEvaluation = true;
        }

        private IEnumerable<KeyValuePair<int, T>> IndexData(SeriesConfiguration<T> config)
        {
            var f = config.Chart.PivotZoomingAxis == AxisTags.X
                        ? config.XValueMapper
                        : config.YValueMapper;

            var isObservable = typeof (IObservableChartPoint).IsAssignableFrom(typeof (T));

            var i = 0;
            foreach (var t in this)
            {
                if (isObservable)
                {
                    var observable = t as IObservableChartPoint;
                    if (observable != null)
                    {
                        observable.PointChanged -= ObservableOnPointChanged;
                        observable.PointChanged += ObservableOnPointChanged;
                    }
                }
                //I think this config.chart.from and config.chart.to is not anymor useful
                //this is causing an issue with double.nan values.
                //becase double.nan is not in the range chart.From-chart.To
                //for now this is disabled and will be rivewed with Highperformance Release
                //var pulled = f(t, i);
                //if (pulled >= config.Chart.From && pulled <= config.Chart.To)
                yield return new KeyValuePair<int, T>(i, t);
                i++;
            }
        }

        private void ObservableOnPointChanged(object caller)
        {
            RequiresEvaluation = true;
            var mapper = Series.Collection.Chart.ShapesMapper;
            var updatedPoint = mapper.FirstOrDefault(x => x.ChartPoint.Instance == caller);
            if (updatedPoint != null)
            {
                var config = (Series.Configuration ?? Series.Collection.Configuration) as SeriesConfiguration<T>;
                if (config != null)
                {
                    updatedPoint.ChartPoint.X = config.XValueMapper((T) caller, updatedPoint.ChartPoint.Key);
                    updatedPoint.ChartPoint.Y = config.YValueMapper((T) caller, updatedPoint.ChartPoint.Key);

                    //test
                    var mapedPoint = Series.Collection.Chart.ShapesMapper
                        .FirstOrDefault(x => updatedPoint.ChartPoint == x.ChartPoint);
                }
            }
            Series.Collection.Chart.Update(false);
        }

        #endregion

        public override sealed event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }
    }
}