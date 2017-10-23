﻿//The MIT License(MIT)

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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiveCharts.Components;
using LiveCharts.Tooltip;
using LiveCharts.TypeConverters;
using LiveCharts.Viewers;

namespace LiveCharts.CoreComponents
{
    public abstract class Chart : UserControl
    {
        public event Action<Chart> Plot;
        public event Action<ChartPoint> DataClick;

        internal Canvas DrawMargin;
        internal int ColorStartIndex;
        internal bool RequiresScale;
        internal List<DeleteBufferItem> EraseSerieBuffer = new List<DeleteBufferItem>();
        internal bool SeriesInitialized;
        internal double From = double.MinValue;
        internal double To = double.MaxValue;
        internal AxisTags PivotZoomingAxis = AxisTags.None;
        internal bool SupportsMultipleSeries = true;

        protected ShapeHoverBehavior ShapeHoverBehavior;
        protected bool AlphaLabel;
        protected readonly DispatcherTimer TooltipTimer;
        protected double DefaultFillOpacity = 0.35;

        private static readonly Random Randomizer;
        private readonly DispatcherTimer _resizeTimer;
        private readonly DispatcherTimer _seriesValuesChanged;
        internal readonly DispatcherTimer SeriesChanged;
        private Point _panOrigin;
        private bool _isDragging;
        private int _colorIndexer;
        private FrameworkElement _dataTooltip;
        private bool _isZooming = false;

        static Chart()
        {
            Colors = new List<Color>
            {
                Color.FromRgb(33, 149, 242),
                Color.FromRgb(243, 67, 54),
                Color.FromRgb(254, 192, 7),
                Color.FromRgb(96, 125, 138),
                Color.FromRgb(155, 39, 175),
                Color.FromRgb(0, 149, 135),
                Color.FromRgb(76, 174, 80),
                Color.FromRgb(121, 85, 72),
                Color.FromRgb(157, 157, 157),
                Color.FromRgb(232, 30, 99),
                Color.FromRgb(63, 81, 180),
                Color.FromRgb(0, 187, 211),
                Color.FromRgb(254, 234, 59),
                Color.FromRgb(254, 87, 34)
            };
            Randomizer = new Random();
        }

        protected Chart()
        {
            var b = new Border();
            Canvas = new Canvas();
            b.Child = Canvas;
            Content = b;

            DrawMargin = new Canvas { ClipToBounds = true };
            Canvas.SetLeft(DrawMargin, 0d);
            Canvas.SetTop(DrawMargin, 0d);

            SetValue(MinHeightProperty, 125d);
            SetValue(MinWidthProperty, 125d);

            SetValue(AxisYProperty, new List<Axis>());
            SetValue(AxisXProperty, new List<Axis>());

            SetValue(LegendProperty, new ChartLegend());
            
            CursorX = new ChartCursor(this, AxisTags.X);
            CursorY = new ChartCursor(this, AxisTags.Y);

            if (RandomizeStartingColor) ColorStartIndex = Randomizer.Next(0, Colors.Count - 1);
            
            AnimatesNewPoints = false;
            AnimationsSpeed = TimeSpan.FromMilliseconds(500);

            var defaultConfig = new SeriesConfiguration<double>().Y(x => x);
            SetCurrentValue(SeriesProperty, new SeriesCollection(defaultConfig));

            DataTooltip = new DefaultIndexedTooltip();
            Shapes = new List<FrameworkElement>();
            ShapesMapper = new List<ShapeMap>();

            PointHoverColor = System.Windows.Media.Colors.White; 

            Background = Brushes.Transparent;

            SizeChanged += Chart_OnsizeChanged;
            MouseWheel += MouseWheelOnRoll;
            MouseLeftButtonDown += MouseDownForPan;
            MouseLeftButtonUp += MouseUpForPan;
            MouseMove += MouseMoveForPan;

            _resizeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _resizeTimer.Tick += (sender, e) =>
            {
                _resizeTimer.Stop();
                Update(false);
            };

            TooltipTimer = new DispatcherTimer();
            TooltipTimer.Tick += TooltipTimerOnTick;
            SetValue(TooltipTimeoutProperty, TimeSpan.FromMilliseconds(800));

            _seriesValuesChanged = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
            _seriesValuesChanged.Tick += UpdateModifiedDataSeries;

            SeriesChanged = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
            SeriesChanged.Tick += (sender, args) =>
            {
                PrepareCanvas();
                UpdateSeries(sender, args);
            };
        }

        #region StaticProperties
        /// <summary>
        /// Gets or sets an area for unit testing, since normally for unit testing you don´t have an UI and LiveCharts refuses to draw a chart if there is no a valid UI area, then you need to set this property to run unit test, width and hegiht must be grather than 15px, this property must be null if you are not unit testing.
        /// </summary>
        public static Rect? MockedArea { get; set; }

        /// <summary>
        /// Gets or sets the default series colors.
        /// </summary>
        public static List<Color> Colors { get; set; }

        /// <summary>
        /// Gets or sets if a each new instance of a chart should initialize with a random color index
        /// </summary>
        public static bool RandomizeStartingColor { get; set; }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty AxisYProperty = DependencyProperty.Register(
            "AxisY", typeof (List<Axis>), typeof (Chart), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets vertical axis
        /// </summary>
        public List<Axis> AxisY
        {
            get { return (List<Axis>) GetValue(AxisYProperty); }
            set { SetValue(AxisYProperty, value); }
        }

        public static readonly DependencyProperty AxisXProperty = DependencyProperty.Register(
            "AxisX", typeof (List<Axis>), typeof (Chart), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets horizontal axis
        /// </summary>
        public List<Axis> AxisX
        {
            get { return (List<Axis>) GetValue(AxisXProperty); }
            set { SetValue(AxisXProperty, value); }
        }

        
        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
            "Zoom", typeof (ZoomingOptions), typeof (Chart), new PropertyMetadata(default(ZoomingOptions)));
        /// <summary>
        /// Gets or sets chart zoom behavior
        /// </summary>
        public ZoomingOptions Zoom
        {
            get { return (ZoomingOptions) GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        public static readonly DependencyProperty LegendProperty = DependencyProperty.Register(
            "Legend", typeof (ChartLegend), typeof (Chart), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets legend location, a legend is a small control that displays series titles and its colors.
        /// </summary>
        public ChartLegend Legend
        {
            get { return (ChartLegend) GetValue(LegendProperty); }
            set { SetValue(LegendProperty, value); }
        }

        public static readonly DependencyProperty LegendLocationProperty = DependencyProperty.Register(
            "LegendLocation", typeof (LegendLocation), typeof (Chart), new PropertyMetadata(LegendLocation.None));
        /// <summary>
        /// Gets or sets where legend is located
        /// </summary>
        public LegendLocation LegendLocation
        {
            get { return (LegendLocation) GetValue(LegendLocationProperty); }
            set { SetValue(LegendLocationProperty, value); }
        }

        public static readonly DependencyProperty InvertProperty = DependencyProperty.Register(
            "Invert", typeof (bool), typeof (Chart), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets if series in this chart should be inverted, even this is a dependency property, it is only to support bidings, this property won't invert the chart when it changes, if you need so then call Chart.Redraw() mathod after you cahnge this property.
        /// </summary>
        public bool Invert
        {
            get { return (bool) GetValue(InvertProperty); }
            set { SetValue(InvertProperty, value); }
        }

        public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register(
            "Hoverable", typeof (bool), typeof (Chart), new PropertyMetadata(true));

        public bool Hoverable
        {
            get { return (bool) GetValue(HoverableProperty); }
            set { SetValue(HoverableProperty, value); }
        }

        public static readonly DependencyProperty PointHoverColorProperty = DependencyProperty.Register(
            "PointHoverColor", typeof (Color), typeof (Chart));
        
        /// <summary>
        /// Gets or sets data point color when mouse is over it. Todo: this seems to be in a wrong place, this property does not works for bar, stacked bar or pie chart.
        /// </summary>
        public Color PointHoverColor
        {
            get { return (Color) GetValue(PointHoverColorProperty); }
            set { SetValue(PointHoverColorProperty, value); }
        }

        public static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register(
            "DisableAnimations", typeof (bool), typeof (Chart));

        /// <summary>
        /// Indicates weather to show animation or not.
        /// </summary>
        public bool DisableAnimations
        {
            get { return (bool) GetValue(DisableAnimationsProperty); }
            set { SetValue(DisableAnimationsProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            "Series", typeof (SeriesCollection), typeof (Chart),
            new PropertyMetadata(null, SeriesChangedCallback ));
        /// <summary>
        /// Gets or sets chart series collection to plot.
        /// </summary>
        public SeriesCollection Series
        {
            get { return (SeriesCollection)  GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public static readonly DependencyProperty TooltipTimeoutProperty = DependencyProperty.Register(
            "TooltipTimeout", typeof (TimeSpan), typeof (Chart), new PropertyMetadata(default(TimeSpan), (o, args) =>
            {
                var chart = o as Chart;
                if (chart == null) return;
                chart.TooltipTimer.Interval = chart.TooltipTimeout;
            }));
        [TypeConverter(typeof(TooltipTimeoutConverter))]
        public TimeSpan TooltipTimeout
        {
            get { return (TimeSpan) GetValue(TooltipTimeoutProperty); }
            set { SetValue(TooltipTimeoutProperty, value); }
        }


        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            "AnimationsSpeed", typeof (TimeSpan), typeof (Chart), new PropertyMetadata(default(TimeSpan)));
        [TypeConverter(typeof(TooltipTimeoutConverter))]
        public TimeSpan AnimationsSpeed
        {
            get { return (TimeSpan) GetValue(AnimationsSpeedProperty); }
            set { SetValue(AnimationsSpeedProperty, value); }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets current DataTooltip
        /// </summary>
        public FrameworkElement DataTooltip
        {
            get { return _dataTooltip; }
            set
            {
                _dataTooltip = value;
                if (value == null) return;
                Panel.SetZIndex(DataTooltip, int.MaxValue);
                Canvas.SetLeft(DataTooltip, 0);
                Canvas.SetTop(DataTooltip, 0);
            }
        }
        /// <summary>
        /// Gets chart canvas
        /// </summary>
        public Canvas Canvas { get; internal set; }

        public ChartCursor CursorX { get; set; }
        public ChartCursor CursorY { get; set; }

        /// <summary>
        /// Gets chart point offset
        /// </summary>
        [Obsolete]
        internal double XOffset { get; set; }
        /// <summary>
        /// Gets charts point offset
        /// </summary>
        [Obsolete]
        internal double YOffset { get; set; }
        /// <summary>
        /// Gets current set of shapes added to canvas by LiveCharts
        /// </summary>
        [Obsolete]
        public List<FrameworkElement> Shapes { get; internal set; }
        /// <summary>
        /// Gets collection of shapes that fires tooltip on hover
        /// </summary>
        [Obsolete]
        public List<ShapeMap> ShapesMapper { get; internal set; }
        #endregion

        #region ProtectedProperties
        protected bool AnimatesNewPoints { get; set; }

        internal bool HasValidSeriesAndValues
        {
            get { return Series.Any(x => x.Values != null && x.Values.Count > 1); }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Forces redraw.
        /// </summary>
        /// <param name="ereaseAll"></param>
        public void Update(bool ereaseAll = true)
        {
            if (SeriesChanged == null) return;
            SeriesChanged.Stop();
            SeriesChanged.Start();
            PrepareCanvas(ereaseAll);
        }

        /// <summary>
        /// Forces chart redraw without waiting for changes. if this method is not used correctly thi might cause chart to plot multiple times in short periods of time.
        /// </summary>
        public void UnsafeUpdate()
        {
            PrepareCanvas();
            UpdateSeries(null, null);
        }

        /// <summary>
        /// Zooms a unit in according to a pivot, the unit is determined by LiveCharts depending on chart scale, values range and zooming mode.
        /// </summary>
        /// <param name="pivot"></param>
        public void ZoomIn(Point pivot)
        {
            if (DataTooltip != null) DataTooltip.Visibility = Visibility.Hidden;

            if (_isZooming) return;

            _isZooming = true;
            var t = new DispatcherTimer {Interval = DisableAnimations ? TimeSpan.Zero : AnimationsSpeed};
            t.Tick += (sender, args) =>
            {
                _isZooming = false;
                t.Stop();
            };
            t.Start();

            var dataPivot = new Point(FromDrawMargin(pivot.X, AxisTags.X), FromDrawMargin(pivot.Y, AxisTags.Y));

            if (Zoom == ZoomingOptions.X || Zoom == ZoomingOptions.XY)
            {
                foreach (var xi in AxisX)
                {
                    var max = xi.MaxValue ?? xi.MaxLimit;
                    var min = xi.MinValue ?? xi.MinLimit;
                    var l = max - min;
                    var rMin = (dataPivot.X - min) / l;
                    var rMax = 1 - rMin;

                    xi.MinValue = min + rMin * xi.S;
                    xi.MaxValue = max - rMax * xi.S;
                }
            }
            else
            {
                foreach (var xi in AxisX)
                {
                    xi.MinValue = null;
                    xi.MaxValue = null;
                }
            }

            if (Zoom == ZoomingOptions.Y || Zoom == ZoomingOptions.XY)
            {
                foreach (var yi in AxisY)
                {
                    var max = yi.MaxValue ?? yi.MaxLimit;
                    var min = yi.MinValue ?? yi.MinLimit;
                    var l = max - min;
                    var rMin = (dataPivot.Y - min) / l;
                    var rMax = 1 - rMin;

                    yi.MinValue = min + rMin * yi.S;
                    yi.MaxValue = max - rMax * yi.S;
                }
            }
            else
            {
                foreach (var yi in AxisY)
                {
                    yi.MinValue = null;
                    yi.MaxValue = null;
                }
            }

            foreach (var series in Series) series.Values.RequiresEvaluation = true;

            UnsafeUpdate();
        }

        /// <summary>
        /// Zooms a unit in according to a pivot, the unit is determined by LiveCharts depending on chart scale, values range and zooming mode.
        /// </summary>
        /// <param name="pivot"></param>
        public void ZoomOut(Point pivot)
        {
            if (DataTooltip != null) DataTooltip.Visibility = Visibility.Hidden;

            if (_isZooming) return;

            _isZooming = true;
            var t = new DispatcherTimer { Interval = DisableAnimations ? TimeSpan.Zero : AnimationsSpeed };
            t.Tick += (sender, args) =>
            {
                _isZooming = false;
                t.Stop();
            };
            t.Start();

            var dataPivot = new Point(FromDrawMargin(pivot.X, AxisTags.X), FromDrawMargin(pivot.Y, AxisTags.Y));

            if (Zoom == ZoomingOptions.X || Zoom == ZoomingOptions.XY)
            {
                foreach (var xi in AxisX)
                {
                    var max = xi.MaxValue ?? xi.MaxLimit;
                    var min = xi.MinValue ?? xi.MinLimit;
                    var l = max - min;
                    var rMin = (dataPivot.X - min) / l;
                    var rMax = 1 - rMin;

                    xi.MinValue = min - rMin * xi.S;
                    xi.MaxValue = max + rMax * xi.S;
                }
            }

            if (Zoom == ZoomingOptions.Y || Zoom == ZoomingOptions.XY)
            {
                foreach (var yi in AxisY)
                {
                    var max = yi.MaxValue ?? yi.MaxLimit;
                    var min = yi.MinValue ?? yi.MinLimit;
                    var l = max - min;
                    var rMin = (dataPivot.Y - min) / l;
                    var rMax = 1 - rMin;

                    yi.MinValue = min - rMin * yi.S;
                    yi.MaxValue = max + rMax * yi.S;
                }
            }

            foreach (var series in Series)
                series.Values.RequiresEvaluation = true;

            UnsafeUpdate();
        }

        /// <summary>
        /// Clears zoom
        /// </summary>
        public void ClearZoom()
        {
            foreach (var xi in AxisX)
            {
                xi.MinValue = null;
                xi.MaxValue = null;
            }

            foreach (var yi in AxisY)
            {
                yi.MinValue = null;
                yi.MaxValue = null;
            }

            UnsafeUpdate();
        }

        /// <summary>
        /// Scales a graph value to screen pixels according to an axis.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="source"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public double ToPlotArea(double value, AxisTags source, int axis = 0)
        {
            return Methods.ToPlotArea(value, source, this, axis);
        }

        /// <summary>
        /// Scales a graph value to screen.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public Point ToPlotArea(Point value, int axis = 0)
        {
            return new Point(ToPlotArea(value.X, AxisTags.X, axis), ToPlotArea(value.Y, AxisTags.Y, axis));
        }

        /// <summary>
        /// Scales a graph point to screen.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public double FromPlotArea(double value, AxisTags axis)
        {
            return Methods.FromPlotArea(value, axis, this);
        }

        /// <summary>
        /// Scales from screen pixels graph value according to an axis.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public double FromDrawMargin(double value, AxisTags axis)
        {
            return Methods.FromDrawMargin(value, axis, this);
        }

        /// <summary>
        /// Scales from screen pixels graph value according to an axis.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public double ToDrawMargin(double value, AxisTags axis, int index = 0)
        {
            return Methods.ToDrawMargin(value, axis, this, index);
        }

        public Point ToDrawMargin(Point point)
        {
            return new Point(ToDrawMargin(point.X, AxisTags.X), ToDrawMargin(point.Y, AxisTags.Y));
        }

        #endregion

        #region ProtectedMethods

        protected void ValidateAxes()
        {
            if (AxisX.Count == 0)
                SetValue(AxisXProperty, new List<Axis> {DefaultAxes.DefaultAxis});
            if (AxisY.Count == 0)
                SetValue(AxisYProperty, new List<Axis> {DefaultAxes.DefaultAxis});
        }


        protected Point GetLabelSize(Axis axis, string value)
        {
            if (!axis.ShowLabels || value == null) return new Point(0, 0);
            var uiLabelSize = new FormattedText(value, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight, axis.FontStretch),
                axis.FontSize, Brushes.Black);
            return new Point(uiLabelSize.Width, uiLabelSize.Height);
        }

        protected virtual void PrepareAxes()
        {
            InitializeComponents();

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                xi.MaxLimit = xi.MaxValue ??
                              Series.Where(series => series.Values != null && series.ScalesXAt == index)
                                  .Select(series => series.Values.MaxChartPoint.X).DefaultIfEmpty(0).Max();
                xi.MinLimit = xi.MinValue ??
                              Series.Where(series => series.Values != null && series.ScalesXAt == index)
                                  .Select(series => series.Values.MinChartPoint.X).DefaultIfEmpty(0).Min();
            }

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];
                yi.MaxLimit = yi.MaxValue ??
                              Series.Where(series => series.Values != null && series.ScalesYAt == index)
                                  .Select(series => series.Values.MaxChartPoint.Y).DefaultIfEmpty(0).Max();
                yi.MinLimit = yi.MinValue ??
                              Series.Where(series => series.Values != null && series.ScalesYAt == index)
                                  .Select(series => series.Values.MinChartPoint.Y).DefaultIfEmpty(0).Min();
            }

            PivotZoomingAxis = Invert ? AxisTags.Y : AxisTags.X;
        }
        #endregion

        #region Virtual Methods
        protected virtual void CalculateComponentsAndMargin()
        {
            if (DrawMargin.ActualHeight < 10 || DrawMargin.ActualWidth < 10) return;

            Canvas.SetLeft(DrawMargin, 0);
            Canvas.SetTop(DrawMargin, 0);
            DrawMargin.Width = ActualWidth;
            DrawMargin.Height = ActualHeight;

            PlaceLegend();

            //left, bot, botMerged, left, right margins
            double t = Canvas.GetTop(DrawMargin),
                b = 0d,
                bm = 0d,
                l = Canvas.GetLeft(DrawMargin),
                r = 0d;

            foreach (var yi in AxisY)
            {
                if (yi.TitleLabel.Parent == null)
                {
                    yi.TitleLabel.RenderTransform = new RotateTransform(-90);
                    Canvas.Children.Add(yi.TitleLabel);
                }
                yi.TitleLabel.UpdateLayout();
                var titleSize = string.IsNullOrWhiteSpace(yi.Title)
                    ? new Size()
                    : yi.TitleLabel.RenderSize;
                var biggest = yi.PreparePlotArea(AxisTags.Y, this);
                var x = Canvas.GetLeft(DrawMargin);
                var merged = yi.IsMerged ? 0 : biggest.Width + 2;
                if (yi.Position == AxisPosition.LeftBottom)
                {
                    Canvas.SetLeft(yi.TitleLabel, x);
                    yi.LabelsReference = x + titleSize.Height + merged ;
                    Canvas.SetLeft(DrawMargin,
                        Canvas.GetLeft(DrawMargin) + titleSize.Height + merged);
                    DrawMargin.Width -= (titleSize.Height + merged);
                }
                else
                {
                    Canvas.SetLeft(yi.TitleLabel, x + DrawMargin.Width - titleSize.Height);
                    yi.LabelsReference = x + DrawMargin.Width - titleSize.Height - merged;
                    DrawMargin.Width -= (titleSize.Height + merged);
                }

                var top = yi.IsMerged ? 0 : biggest.Height*.5;
                if (t < top) t = top;

                var bot = yi.IsMerged ? 0 : biggest.Height*.5;
                if (b < bot) b = bot;

                if (yi.IsMerged && bm < biggest.Height)
                    bm = biggest.Height;
            }

            if (t > 0)
            {
                Canvas.SetTop(DrawMargin, t);
                DrawMargin.Height -= t;
            }
            if (b > 0 && !AxisX.Any())
                DrawMargin.Height = DrawMargin.Height - b;

            foreach (var xi in AxisX)
            {
                if (xi.TitleLabel.Parent == null) Canvas.Children.Add(xi.TitleLabel);

                xi.TitleLabel.UpdateLayout();
                var titleSize = string.IsNullOrWhiteSpace(xi.Title)
                    ? new Size()
                    : xi.TitleLabel.RenderSize;
                var biggest = xi.PreparePlotArea(AxisTags.X, this);
                var top = Canvas.GetTop(DrawMargin);
                var merged = xi.IsMerged ? 0 : biggest.Height;
                if (xi.Position == AxisPosition.LeftBottom)
                {
                    Canvas.SetTop(xi.TitleLabel, top + DrawMargin.Height - titleSize.Height);
                    xi.LabelsReference = top + b - (xi.IsMerged ? bm : 0) +
                        (DrawMargin.Height - (titleSize.Height + merged + b)) -
                        (xi.IsMerged ? b : 0);
                    DrawMargin.Height -= (titleSize.Height + merged + b);
                }
                else
                {
                    Canvas.SetTop(xi.TitleLabel, top - t);
                    xi.LabelsReference = (top - t) + titleSize.Height + (xi.IsMerged ? bm : 0);
                    Canvas.SetTop(DrawMargin,
                        Canvas.GetTop(DrawMargin) + titleSize.Height + merged);
                    DrawMargin.Height -= (titleSize.Height + merged);
                }

                var left = xi.IsMerged ? 0 : biggest.Width * .5;
                if (l < left) l = left;

                var right = xi.IsMerged ? 0 : biggest.Width * .5;
                if (r < right) r = right;
            }

            if (Canvas.GetLeft(DrawMargin) < l)
            {
                var cor = l - Canvas.GetLeft(DrawMargin);
                Canvas.SetLeft(DrawMargin, l);
                DrawMargin.Width -= cor;
                foreach (var yi in AxisY.Where(x => x.Position == AxisPosition.LeftBottom))
                {
                    Canvas.SetLeft(yi.TitleLabel, Canvas.GetLeft(yi.TitleLabel) + cor);
                    yi.LabelsReference += cor;
                }
            }
            var rp = ActualWidth - Canvas.GetLeft(DrawMargin) - DrawMargin.Width;
            if (r > rp)
            {
                var cor = r - rp;
                DrawMargin.Width -= cor;
                foreach (var yi in AxisY.Where(x => x.Position == AxisPosition.RightTop))
                {
                    Canvas.SetLeft(yi.TitleLabel, Canvas.GetLeft(yi.TitleLabel) - cor);
                    yi.LabelsReference -= cor;
                }
            }

            var isUp = this is IUnitaryPoints;

            for (var index = 0; index < AxisY.Count; index++)
            {
                var yi = AxisY[index];
                if (isUp && Invert)
                    yi.UnitWidth = Methods.GetUnitWidth(AxisTags.Y, this, index);
                yi.UpdateSeparations(AxisTags.Y, this, index);
                Canvas.SetTop(yi.TitleLabel,
                    Canvas.GetTop(DrawMargin) + DrawMargin.Height*.5 + yi.TitleLabel.ActualWidth*.5);
            }

            for (var index = 0; index < AxisX.Count; index++)
            {
                var xi = AxisX[index];
                if (isUp && !Invert)
                    xi.UnitWidth = Methods.GetUnitWidth(AxisTags.X, this, index);
                xi.UpdateSeparations(AxisTags.X, this, index);
                Canvas.SetLeft(xi.TitleLabel,
                    Canvas.GetLeft(DrawMargin) + DrawMargin.Width*.5 - xi.TitleLabel.ActualWidth*.5);
            }

            //drawing ceros.
            //if (Max.Y >= 0 &&AxisY.MinLimit <= 0 && AxisX.IsEnabled)
            //{
            //    var l = new Line
            //    {
            //        Stroke = new SolidColorBrush {Color = AxisX.Color},
            //        StrokeThickness = AxisX.StrokeThickness,
            //        X1 = ToPlotArea(Min.X, AxisTags.X),
            //        Y1 = ToPlotArea(0, AxisTags.Y),
            //        X2 = ToPlotArea(Max.X, AxisTags.X),
            //        Y2 = ToPlotArea(0, AxisTags.Y)
            //    };
            //    Canvas.Children.Add(l);
            //    Shapes.Add(l);
            //}
            //if (Max.X >= 0 &&AxisX.MinLimit <= 0 && AxisY.IsEnabled)
            //{
            //    var l = new Line
            //    {
            //        Stroke = new SolidColorBrush {Color = AxisY.Color},
            //        StrokeThickness = AxisY.StrokeThickness,
            //        X1 = ToPlotArea(0, AxisTags.X),
            //        Y1 = ToPlotArea(Min.Y, AxisTags.Y),
            //        X2 = ToPlotArea(0, AxisTags.X),
            //        Y2 = ToPlotArea(Max.Y, AxisTags.Y)
            //    };
            //    Canvas.Children.Add(l);
            //    Shapes.Add(l);
            //}
        }

        protected virtual void LoadLegend()
        {
            if (Legend.Parent == null)
                Canvas.Children.Add(Legend);

            Legend.Series = Series.Select(x => new SeriesStandin
            {
                Fill = x.Fill,
                Stroke = x.Stroke,
                Title = x.Title
            });
            Legend.Orientation = LegendLocation == LegendLocation.Bottom || LegendLocation == LegendLocation.Top
                ? Orientation.Horizontal
                : Orientation.Vertical;
        }

        protected internal virtual void DataMouseEnter(object sender, MouseEventArgs e)
        {
            if (DataTooltip == null || !Hoverable) return;

            DataTooltip.Visibility = Visibility.Visible;
            TooltipTimer.Stop();

            var senderShape = ShapesMapper.FirstOrDefault(s => Equals(s.HoverShape, sender));
            if (senderShape == null) return;

            var targetAxis = Invert ? senderShape.Series.ScalesYAt : senderShape.Series.ScalesXAt;

            var sibilings = Invert
                ? ShapesMapper.Where(s => Math.Abs(s.ChartPoint.Y - senderShape.ChartPoint.Y) < AxisY[targetAxis].S*.01)
                    .ToList()
                : ShapesMapper.Where(s => Math.Abs(s.ChartPoint.X - senderShape.ChartPoint.X) < AxisX[targetAxis].S*.01)
                    .ToList();

            var first = sibilings.Count > 0 ? sibilings[0] : null;
            var vx = first != null ? (Invert ? first.ChartPoint.Y : first.ChartPoint.X) : 0;

            foreach (var sibiling in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    sibiling.Shape.Stroke = sibiling.Series.Stroke;
                    sibiling.Shape.Fill = new SolidColorBrush {Color = PointHoverColor};
                }
                else sibiling.Shape.Opacity = .8;
                sibiling.Active = true;
            }

            var indexedToolTip = DataTooltip as IndexedTooltip;
            if (indexedToolTip != null)
            {
                var fh = (Invert ? AxisY[targetAxis] : AxisX[targetAxis]).GetFormatter();
                var fs = (Invert ? AxisX[targetAxis] : AxisY[targetAxis]).GetFormatter();

                indexedToolTip.Header = fh(vx);
                indexedToolTip.Data = sibilings.Select(x => new IndexedTooltipData
                {
                    Index = Series.IndexOf(x.Series),
                    Series = x.Series,
                    Stroke = x.Series.Stroke,
                    Fill = x.Series.Fill,
                    Point = x.ChartPoint,
                    Value = fs(Invert ? x.ChartPoint.X : x.ChartPoint.Y)
                }).ToArray();
            }

            var p = GetToolTipPosition(senderShape, sibilings);

            DataTooltip.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                To = p.X, Duration = TimeSpan.FromMilliseconds(200)
            });
            DataTooltip.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                To = p.Y, Duration = TimeSpan.FromMilliseconds(200)
            });
        }

        protected internal virtual void DataMouseLeave(object sender, MouseEventArgs e)
        {
            if (!Hoverable) return;

            var s = sender as Shape;
            if (s == null) return;

            var shape = ShapesMapper.FirstOrDefault(x => Equals(x.HoverShape, s));
            if (shape == null) return;

            var sibilings = ShapesMapper.Where(x => x.Active).ToList();

            foreach (var p in sibilings)
            {
                if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
                {
                    p.Shape.Fill = p.Series.Stroke;
                    p.Shape.Stroke = new SolidColorBrush {Color = PointHoverColor};
                }
                else
                {
                    p.Shape.Opacity = 1;
                }
            }
            TooltipTimer.Stop();
            TooltipTimer.Start();
        }

        internal virtual void DataMouseDown(object sender, MouseEventArgs e)
        {
            var shape = ShapesMapper.FirstOrDefault(s => Equals(s.HoverShape, sender));
            if (shape == null) return;
            OnDataClick(shape.ChartPoint);
        }

        protected virtual void OnDataClick(ChartPoint chartPoint)
        {
            if (DataClick != null) DataClick.Invoke(chartPoint);
        }

        protected virtual Point GetToolTipPosition(ShapeMap sender, List<ShapeMap> sibilings)
        {
            DataTooltip.UpdateLayout();
            DataTooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            var targetAxis = Invert ? sender.Series.ScalesYAt : sender.Series.ScalesXAt;

            var x = sender.ChartPoint.X > (AxisX[targetAxis].MinLimit + AxisX[targetAxis].MaxLimit)/2
                ? sender.ChartPoint.Location.X - 10 - DataTooltip.DesiredSize.Width
                : sender.ChartPoint.Location.X + 10;

            x += Canvas.GetLeft(DrawMargin);

            var y = sibilings.Select(s => s.ChartPoint.Location.Y).DefaultIfEmpty(0).Sum()/sibilings.Count;
            y = y + DataTooltip.DesiredSize.Height > ActualHeight ? y - (y + DataTooltip.DesiredSize.Height - ActualHeight) - 5 : y;
            return new Point(x, y);
        }

        #endregion

        #region Internal Methods

        internal void OnDataSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _seriesValuesChanged.Stop();
            _seriesValuesChanged.Start();
        }

        #endregion

        #region Private Methods

        private void PlaceLegend()
        {
            LoadLegend();

            if (LegendLocation != LegendLocation.None)
            {
                Legend.UpdateLayout();
                Legend.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            switch (LegendLocation)
            {
                case LegendLocation.None:
                    Legend.Visibility = Visibility.Hidden;
                    break;
                case LegendLocation.Top:
                    var top = new Point(ActualWidth*.5 - Legend.DesiredSize.Width*.5, 0);
                    var y = Canvas.GetTop(DrawMargin);
                    Canvas.SetTop(DrawMargin, y + top.Y + Legend.DesiredSize.Height + 10);
                    DrawMargin.Height -= Legend.DesiredSize.Height -10;
                    Canvas.SetTop(Legend, top.Y);
                    Canvas.SetLeft(Legend, top.X);
                    break;
                case LegendLocation.Bottom:
                    var bot = new Point(ActualWidth*.5 - Legend.DesiredSize.Width*.5,
                        ActualHeight - Legend.DesiredSize.Height);
                    DrawMargin.Height -= Legend.DesiredSize.Height;
                    Canvas.SetTop(Legend, Canvas.ActualHeight - Legend.DesiredSize.Height);
                    Canvas.SetLeft(Legend, bot.X);
                    break;
                case LegendLocation.Left:
                    Canvas.SetLeft(DrawMargin, Canvas.GetLeft(DrawMargin) + Legend.DesiredSize.Width);
                    Canvas.SetTop(Legend, Canvas.ActualHeight*.5 - Legend.DesiredSize.Height*.5);
                    Canvas.SetLeft(Legend, 0);
                    break;
                case LegendLocation.Right:
                    DrawMargin.Width -= Legend.DesiredSize.Width + 10;
                    Canvas.SetTop(Legend, Canvas.ActualHeight*.5 - Legend.DesiredSize.Height*.5);
                    Canvas.SetLeft(Legend, ActualWidth - Legend.DesiredSize.Width);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void PrepareCanvas(bool ereaseAll = false)
        {
            if (Series == null) return;
            if (!SeriesInitialized) InitializeSeries(this);

            foreach (var xi in AxisX.Where(xi => xi.Parent == null))
                Canvas.Children.Add(xi);
            foreach (var yi in AxisY.Where(yi => yi.Parent == null))
                Canvas.Children.Add(yi);
            
            if (DrawMargin.Parent == null)
            {
                Canvas.Children.Add(DrawMargin);
                Panel.SetZIndex(DrawMargin, 1);
            }

            foreach (var series in Series)
            {
                var p = series.Parent as Canvas;
                if (p != null) p.Children.Remove(series);
                DrawMargin.Children.Add(series);
                EraseSerieBuffer.Add(new DeleteBufferItem {Series = series, Force = ereaseAll});
                series.RequiresAnimation = ereaseAll;
                series.RequiresPlot = true;
            }

            SetPlotArea();

            RequiresScale = true;
        }

        internal void InitializeComponents()
        {
            Series.Chart = this;
            Series.Configuration.Chart = this;
            if (DataTooltip.Parent == null)
            {
                DataTooltip.Visibility = Visibility.Hidden;
                Canvas.Children.Add(DataTooltip);
            }
            foreach (var series in Series)
            {
                series.Collection = Series;
                if (series.Values == null) continue;
                if (series.Configuration != null) series.Configuration.Chart = this;
                series.Values.Series = series;
                series.Values.GetLimits();
            }
        }

        private void Chart_OnsizeChanged(object sender, SizeChangedEventArgs e)
        {
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        private static void SeriesChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs eventArgs)
        {
            var chart = o as Chart;

            if (chart == null || chart.Series == null || chart.Series.Count == 0) return;
            if (chart.Series.Any(x => x == null)) return;

            if (chart.Series.Count > 0)
            {
                //we need to kill everything in the canvas if the seriesCollection changes
                chart.RemoveAllVisualElements();

                chart.PrepareAxes();

                //Todo: Initialize chart size correctly, this works but fo course is not correct.
                //This means: if chart is initialized then update the chart.
                if (chart.ActualHeight > 10)
                {
                    chart.InitializeSeries(chart);
                    chart.InitializeComponents();
                    chart.Update();
                }
            }
        }

        private void InitializeSeries(Chart chart)
        {
#if DEBUG
            Trace.WriteLine("Chart was initialized (" + DateTime.Now.ToLongTimeString() + ")");
#endif
            chart.SeriesInitialized = true;
            foreach (var series in chart.Series)
            {
                var index = _colorIndexer++;
                series.Chart = chart;
                series.Collection = Series;
                series.Stroke = series.Stroke ?? new SolidColorBrush(Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))]);
                series.Fill = series.Fill ?? new SolidColorBrush(Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))])
                {
                    Opacity = DefaultFillOpacity
                };
                series.RequiresPlot = true;
                series.RequiresAnimation = true;
                var observable = series.Values as INotifyCollectionChanged;
                if (observable != null)
                    observable.CollectionChanged += chart.OnDataSeriesChanged;
            }

            chart.Update();
            var anim = new DoubleAnimation
            {
                From = 0, To = 1, Duration = TimeSpan.FromMilliseconds(1000)
            };
            if (!chart.DisableAnimations) chart.Canvas.BeginAnimation(OpacityProperty, anim);

            chart.Series.CollectionChanged += (sender, args) =>
            {
                chart.SeriesChanged.Stop();
                chart.SeriesChanged.Start();

                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    chart.RemoveAllVisualElements();
                }

                if (args.OldItems != null)
                    foreach (var series in args.OldItems.Cast<Series>())
                        chart.EraseSerieBuffer.Add(new DeleteBufferItem {Series = series, Force = true});

                var newElements = args.NewItems != null ? args.NewItems.Cast<Series>() : new List<Series>();

                chart.RequiresScale = true;
                foreach (var series in chart.Series.Where(x => !newElements.Contains(x)))
                {
                    chart.EraseSerieBuffer.Add(new DeleteBufferItem {Series = series});
                    series.RequiresPlot = true;
                }

                if (args.NewItems != null)
                    foreach (var series in newElements)
                    {
                        var index = _colorIndexer++;
                        series.Chart = chart;
                        series.Collection = Series;
                        series.Stroke = series.Stroke ?? new SolidColorBrush(Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))]);
                        series.Fill = series.Fill ?? new SolidColorBrush(Colors[(int) (index - Colors.Count*Math.Truncate(index/(decimal) Colors.Count))])
                        {
                            Opacity = DefaultFillOpacity
                        };
                        series.RequiresPlot = true;
                        series.RequiresAnimation = true;
                        var observable = series.Values as INotifyCollectionChanged;
                        if (observable != null)
                            observable.CollectionChanged += chart.OnDataSeriesChanged;
#if DEBUG
                        if (observable == null) Trace.WriteLine("series do not implements INotifyCollectionChanged");
#endif
                    }
            };
        }

        private void RemoveAllVisualElements()
        {
            foreach (var yi in AxisY) yi.Reset();
            foreach (var xi in AxisX) xi.Reset();
            DrawMargin.Children.Clear();
            Canvas.Children.Clear();
            Shapes.Clear();
            ShapesMapper.Clear();
        }

        private void UpdateSeries(object sender, EventArgs e)
        {
            SeriesChanged.Stop();

            EreaseSeries();

            ValidateAxes();

            if (Series == null || Series.Count == 0) return;

            foreach (var shape in Shapes) Canvas.Children.Remove(shape);
            
            Shapes = new List<FrameworkElement>();

            if (RequiresScale)
            {
                PrepareAxes();
                RequiresScale = false;
            }

            foreach (var series in Series.Where(x => x.RequiresPlot))
            {
                if (series.Values != null && series.Values.Count > 0 )
                    series.Plot(series.RequiresAnimation);
                series.RequiresPlot = false;
                series.RequiresAnimation = false;
            }

            if (Plot != null) Plot(this);
#if DEBUG
            Trace.WriteLine("Series Updated (" + DateTime.Now.ToLongTimeString() + ")");
            if (DrawMargin != null) Trace.WriteLine("Draw Margin Objects \t" + DrawMargin.Children.Count);
            Trace.WriteLine("Canvas Children \t\t" + Canvas.Children.Count);
            Trace.WriteLine("Chart Shapes \t\t\t" + Shapes.Count);
            Trace.WriteLine("Shapes Mapper Items \t" + ShapesMapper.Count);
#endif
        }

        private void EreaseSeries()
        {
            foreach (var deleteItem in EraseSerieBuffer) deleteItem.Series.Erase(deleteItem.Force);
            EraseSerieBuffer.Clear();
        }

        private void UpdateModifiedDataSeries(object sender, EventArgs e)
        {
#if DEBUG
            Trace.WriteLine("Primary Values Updated (" + DateTime.Now.ToLongTimeString() + ")");
#endif
            _seriesValuesChanged.Stop();
            PrepareAxes();
            foreach (var serie in Series)
            {
                serie.Erase();
                serie.Plot(AnimatesNewPoints);
            }
        }

        private void SetPlotArea()
        {
            var w = MockedArea != null ? MockedArea.Value.Width : ActualWidth;
            var h = MockedArea != null ? MockedArea.Value.Height : ActualHeight;
            Canvas.Width = w;
            Canvas.Height = h;
            DrawMargin.Width = w;
            DrawMargin.Height = h;
        }

        private void MouseWheelOnRoll(object sender, MouseWheelEventArgs e)
        {
            if (PivotZoomingAxis == AxisTags.None) return;
            e.Handled = true;
            if (e.Delta > 0) ZoomIn(e.GetPosition(this));
            else ZoomOut(e.GetPosition(this));
        }

        private void MouseDownForPan(object sender, MouseEventArgs e)
        {
            if (PivotZoomingAxis == AxisTags.None) return;
            var p = e.GetPosition(this);
            _panOrigin = new Point(FromDrawMargin(p.X, AxisTags.X), FromDrawMargin(p.Y, AxisTags.Y));
            _isDragging = true;
        }

        private void MouseMoveForPan(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            //var p = e.GetPosition(this);
            //var movePoint = new Point(FromDrawMargin(p.X, AxisTags.X), FromDrawMargin(p.Y, AxisTags.Y));
            //var dif = _panOrigin - movePoint;

            //var maxX = AxisX.MaxValue ??AxisX.Max;
            //var minX = AxisX.MinValue ??AxisX.Min;
            //var maxY = AxisY.MaxValue ??AxisY.Max;
            //var minY = AxisY.MinValue ??AxisY.Min;

            //var dx =dif.X;
            //var dy = dif.Y;

            //AxisX.MaxValue = maxX + dx;
            //AxisX.MinValue = minX - dx;

            //foreach (var series in Series) series.Values.RequiresEvaluation = true;

            //ForceRedrawNow();

            //_panOrigin = movePoint;
        }

        private void MouseUpForPan(object sender, MouseEventArgs e)
        {
            if (PivotZoomingAxis == AxisTags.None) return;

            var p = e.GetPosition(this);
            var movePoint = new Point(FromDrawMargin(p.X, AxisTags.X), FromDrawMargin(p.Y, AxisTags.Y));
            var dif = _panOrigin - movePoint;
            var dx = dif.X;
            var dy = dif.Y;

            if (Zoom == ZoomingOptions.X || Zoom == ZoomingOptions.XY)
            {
                foreach (var xi in AxisX)
                {
                    var maxX = xi.MaxValue ?? xi.MaxLimit;
                    var minX = xi.MinValue ?? xi.MinLimit;
                    xi.MaxValue = maxX + dx;
                    xi.MinValue = minX + dx;
                }
            }

            if (Zoom == ZoomingOptions.Y || Zoom == ZoomingOptions.XY)
            {
                foreach (var yi in AxisY)
                {
                    var maxY = yi.MaxValue ?? yi.MaxLimit;
                    var minY = yi.MinValue ?? yi.MinLimit;

                    yi.MaxValue = maxY + dy;
                    yi.MinValue = minY + dy;
                }
            }

            foreach (var series in Series) series.Values.RequiresEvaluation = true;

            UnsafeUpdate();
            _isDragging = false;
        }

        private void TooltipTimerOnTick(object sender, EventArgs e)
        {
            DataTooltip.Visibility = Visibility.Hidden;
            TooltipTimer.Stop();
        }

        #endregion
    }
}