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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Cache;
using LiveCharts.CoreComponents;
using LiveCharts.TypeConverters;

namespace LiveCharts
{
    public class Axis : FrameworkElement
    {
        public Axis()
        {
            CleanFactor = 3;

            SetValue(SeparatorProperty, new Separator
            {
                IsEnabled = true,
                Color = Color.FromRgb(242, 242, 242),
                StrokeThickness = 1
            });

            TitleLabel = new TextBlock();

            BindingOperations.SetBinding(TitleLabel, TextBlock.TextProperty,
                new Binding {Path = new PropertyPath(TitleProperty), Source = this});
            BindingOperations.SetBinding(TitleLabel, TextBlock.FontFamilyProperty,
                new Binding {Path = new PropertyPath(FontFamilyProperty), Source = this});
            BindingOperations.SetBinding(TitleLabel, TextBlock.FontWeightProperty,
                new Binding { Path = new PropertyPath(FontWeightProperty), Source = this });
            BindingOperations.SetBinding(TitleLabel, TextBlock.FontStyleProperty,
                new Binding { Path = new PropertyPath(FontStyleProperty), Source = this });
            BindingOperations.SetBinding(TitleLabel, TextBlock.FontStretchProperty,
                new Binding { Path = new PropertyPath(FontStretchProperty), Source = this });
            BindingOperations.SetBinding(TitleLabel, TextBlock.ForegroundProperty,
                new Binding { Path = new PropertyPath(ForegroundProperty), Source = this });
        }

        #region Public Properties

        public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register(
            "Labels", typeof (IList<string>), typeof (Axis), new PropertyMetadata(default(IList<string>)));

        /// <summary>
        /// Gets or sets axis labels, labels property stores the array to map for each index and value, for example if axis value is 0 then label will be labels[0], when value 1 then labels[1], value 2 then labels[2], ..., value n labels[n], use this property instead of a formatter when there is no conversion between value and label for example names, if you are ploting sales vs salesman name.
        /// </summary>
        [TypeConverter(typeof (StringCollectionConverter))]
        public IList<string> Labels
        {
            get { return (IList<string>) GetValue(LabelsProperty); }
            set { SetValue(LabelsProperty, value); }
        }

        public static readonly DependencyProperty LabelFormatterProperty =
            DependencyProperty.Register("LabelFormatter", typeof (Func<double, string>), typeof (Axis),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the function to convet a value to label, for example when you need to display your chart as curency ($1.00) or as degrees (10°), if Labels property is not null then formatter is ignored, and label will be pulled from Labels prop.
        /// </summary>
        public Func<double, string> LabelFormatter
        {
            get { return (Func<double, string>) GetValue(LabelFormatterProperty); }
            set { SetValue(LabelFormatterProperty, value); }
        }

        public static readonly DependencyProperty SeparatorProperty = DependencyProperty.Register(
            "Separator", typeof (Separator), typeof (Axis), new PropertyMetadata(default(Separator)));

        /// <summary>
        /// Get or sets configuration for parallel lines to axis.
        /// </summary>
        public Separator Separator
        {
            get { return (Separator) GetValue(SeparatorProperty); }
            set { SetValue(SeparatorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof (Color), typeof (Axis),
                new PropertyMetadata(Color.FromRgb(242, 242, 242)));

        /// <summary>
        /// Gets or sets axis color, axis means only the zero value, if you need to highlight where zero is. to change separators color, see Axis.Separator
        /// </summary>
        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof (double), typeof (Axis), new PropertyMetadata(1d));

        /// <summary>
        /// Gets or sets axis thickness.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty ShowLabelsProperty =
            DependencyProperty.Register("ShowLabels", typeof (bool), typeof (Axis), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets if labels are visible.
        /// </summary>
        public bool ShowLabels
        {
            get { return (bool) GetValue(ShowLabelsProperty); }
            set { SetValue(ShowLabelsProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof (double?), typeof (Axis), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets chart max value, set it to null to make this property Auto, default value is null
        /// </summary>
        public double? MaxValue
        {
            get { return (double?) GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof (double?), typeof (Axis), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets chart min value, set it to null to make this property Auto, default value is null
        /// </summary>
        public double? MinValue
        {
            get { return (double?) GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (string), typeof (Axis), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets axis title
        /// </summary>
        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            "AnimationsSpeed", typeof (TimeSpan?), typeof (Axis), new PropertyMetadata(default(TimeSpan?)));

        public TimeSpan? AnimationsSpeed
        {
            get { return (TimeSpan?) GetValue(AnimationsSpeedProperty); }
            set { SetValue(AnimationsSpeedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the axis position
        /// </summary>
        public AxisPosition Position { get; set; }

        /// <summary>
        /// Gets or sets if the axis labels should me placed inside the chart.
        /// </summary>
        public bool IsMerged { get; set; }

        public bool DisableAnimations { get; set; }
        #endregion

        #region Font Properties

        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof (FontFamily), typeof (Axis),
                new PropertyMetadata(new FontFamily("Calibri")));

        /// <summary>
        /// Gets or sets labels font family, font to use for labels in this axis
        /// </summary>
        public FontFamily FontFamily
        {
            get { return (FontFamily) GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof (double), typeof (Axis), new PropertyMetadata(11.0));

        /// <summary>
        /// Gets or sets labels font size
        /// </summary>
        public double FontSize
        {
            get { return (double) GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register("FontWeight", typeof (FontWeight), typeof (Axis),
                new PropertyMetadata(FontWeights.Normal));

        /// <summary>
        /// Gets or sets labels font weight
        /// </summary>
        public FontWeight FontWeight
        {
            get { return (FontWeight) GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public static readonly DependencyProperty FontStyleProperty =
            DependencyProperty.Register("FontStyle", typeof (FontStyle), typeof (Axis),
                new PropertyMetadata(FontStyles.Normal));

        /// <summary>
        /// Gets or sets labels font style
        /// </summary>
        public FontStyle FontStyle
        {
            get { return (FontStyle) GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public static readonly DependencyProperty FontStretchProperty =
            DependencyProperty.Register("FontStretch", typeof (FontStretch), typeof (Axis),
                new PropertyMetadata(FontStretches.Normal));

        /// <summary>
        /// Gets or sets labels font strech
        /// </summary>
        public FontStretch FontStretch
        {
            get { return (FontStretch) GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof (Brush), typeof (Axis),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(150, 150, 150))));

        /// <summary>
        /// Gets or sets labels text color.
        /// </summary>
        public Brush Foreground
        {
            get { return (Brush) GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        #endregion

        internal TextBlock TitleLabel;
        internal double? LastAxisMax;
        internal double? LastAxisMin;
        internal Rect LastPlotArea;
        internal double LabelsReference;
        internal Dictionary<double, AxisSeparationCache> Separations = new Dictionary<double, AxisSeparationCache>();
        internal double UnitWidth;

        /// <summary>
        /// Factor used to calculate label separations. default is 3. increase it to make it 'cleaner'
        /// initialSeparations = Graph.Heigth / (label.Height * cleanFactor)
        /// </summary>
        internal int CleanFactor { get; set; }

        internal bool IgnoresLastLabel { get; set; }

        internal void Reset()
        {
            Separations.Clear();
        }

        internal TextBlock BindATextBlock(int rotate)
        {
            var tb = new TextBlock();

            tb.SetBinding(TextBlock.FontFamilyProperty,
                new Binding {Path = new PropertyPath(FontFamilyProperty), Source = this});
            tb.SetBinding(FontSizeProperty,
                new Binding { Path = new PropertyPath(FontSizeProperty), Source = this });
            tb.SetBinding(TextBlock.FontStretchProperty,
                new Binding { Path = new PropertyPath(FontStretchProperty), Source = this });
            tb.SetBinding(TextBlock.FontStyleProperty,
                new Binding { Path = new PropertyPath(FontStyleProperty), Source = this });
            tb.SetBinding(TextBlock.FontWeightProperty,
                new Binding { Path = new PropertyPath(FontWeightProperty), Source = this });
            tb.SetBinding(TextBlock.ForegroundProperty,
                 new Binding { Path = new PropertyPath(ForegroundProperty), Source = this });

            return tb;
        }

        internal double MaxLimit { get; set; }
        internal double MinLimit { get; set; }
        internal double S { get; set; }

        internal bool HasValidRange
        {
            get { return Math.Abs(MaxLimit - MinLimit) > S*.01; }
        }

        internal void CalculateSeparator(Chart chart, AxisTags source)
        {
            if (Separator.Step != null)
            {
                S = Separator.Step ?? 1;
                return;
            }

            var range = MaxLimit - MinLimit;
            range = range <= 0 ? 1 : range;

            var ft = new FormattedText(
                "A label",
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight,
                    FontStretch), FontSize, Brushes.Black);

            var separations = source == AxisTags.Y
                ? Math.Round(chart.DrawMargin.Height/((ft.Height)*CleanFactor), 0)
                : Math.Round(chart.DrawMargin.Width/((ft.Width)*CleanFactor), 0);

            separations = separations < 2 ? 2 : separations;

            var minimum = range/separations;
            var magnitude = Math.Pow(10, Math.Floor(Math.Log(minimum)/Math.Log(10)));
            var residual = minimum/magnitude;
            double tick;
            if (residual > 5)
                tick = 10*magnitude;
            else if (residual > 2)
                tick = 5*magnitude;
            else if (residual > 1)
                tick = 2*magnitude;
            else
                tick = magnitude;

            S = tick;

            if (Labels != null) S = S < 1 ? 1 : S;
        }

        internal Size GetLabelSize(double value)
        {
            if (!ShowLabels) return new Size(0, 0);

            var fomattedValue = Labels == null
                ? (LabelFormatter == null
                    ? MinLimit.ToString(CultureInfo.InvariantCulture)
                    : LabelFormatter(value))
                : (Labels.Count > value && value >= 0
                    ? Labels[(int) value]
                    : "");

            var labelSize =
                new FormattedText(fomattedValue, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    FontSize, Brushes.Black);

            return new Size(labelSize.Width, labelSize.Height);
        }

        internal double FromPreviousAxisState(double value, AxisTags direction, Chart chart)
        {
            //y = m * (x - x1) + y1

            if (LastAxisMax == null) return 0;

            var p1 = new Point();
            var p2 = new Point();

            if (direction == AxisTags.Y)
            {
                p1.X = LastAxisMax ?? 0;
                p1.Y = LastPlotArea.Y;

                p2.X = LastAxisMin ?? 0;
                p2.Y = LastPlotArea.Y + LastPlotArea.Height;
            }
            else
            {
                p1.X = LastAxisMax ?? 0;
                p1.Y = LastPlotArea.Width + LastPlotArea.X;

                p2.X = LastAxisMin ?? 0;
                p2.Y = LastPlotArea.X;
            }

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);
            return m * (value - p1.X) + p1.Y;
        }

        internal Size PreparePlotArea(AxisTags direction, Chart chart)
        {
            if (!HasValidRange || !ShowLabels) return new Size();
            if (chart.DrawMargin.Width < 5 || chart.DrawMargin.Height < 5) return new Size();

            CalculateSeparator(chart, direction);

            var f = GetFormatter();

            var biggest = new Size(0, 0);
            var tolerance = S/10;

            var uwc = 0;

            if (chart is IUnitaryPoints)
            {
                //if (direction == AxisTags.Y && chart.Invert) uwc = 1; //this is not necessary.
                if (direction == AxisTags.X && !chart.Invert) uwc = 1;
            }

            for (var i = MinLimit; i <= MaxLimit - uwc; i += S)
            {
                AxisSeparationCache axisSeparationCache;

                var key = Math.Round(i/tolerance)*tolerance;
                if (!Separations.TryGetValue(key, out axisSeparationCache))
                {
                    axisSeparationCache = new AxisSeparationCache
                    {
                        TextBlock = BindATextBlock(0),
                        Line = new Line
                        {
                            Stroke = new SolidColorBrush(Separator.Color),
                            StrokeThickness = Separator.StrokeThickness
                        },
                        IsNew = true
                    };
                    Panel.SetZIndex(axisSeparationCache.TextBlock, -1);
                    Panel.SetZIndex(axisSeparationCache.Line, -1);
                    chart.Canvas.Children.Add(axisSeparationCache.TextBlock);
                    chart.Canvas.Children.Add(axisSeparationCache.Line);
                    Separations[key] = axisSeparationCache;
                }
                else
                {
                    axisSeparationCache.IsNew = false;
                }

                axisSeparationCache.Key = key;
                axisSeparationCache.Value = i;
                axisSeparationCache.IsActive = true;
                axisSeparationCache.TextBlock.Text = f(i);
                axisSeparationCache.TextBlock.UpdateLayout();

                biggest.Width = axisSeparationCache.TextBlock.ActualWidth > biggest.Width
                    ? axisSeparationCache.TextBlock.ActualWidth
                    : biggest.Width;
                biggest.Height = axisSeparationCache.TextBlock.ActualHeight > biggest.Height
                    ? axisSeparationCache.TextBlock.ActualHeight
                    : biggest.Height;

                if (LastAxisMax == null)
                {
                    axisSeparationCache.State = SeparationState.InitialAdd;
                    continue;
                }

                axisSeparationCache.State = SeparationState.Keep;
            }
#if DEBUG
            Trace.WriteLine("Axis.Separations: " + Separations.Count);
#endif
            return biggest;
        }

        internal Func<double, string> GetFormatter()
        {
            return x => Labels == null
                ? (LabelFormatter == null
                    ? x.ToString(CultureInfo.InvariantCulture)
                    : LabelFormatter(x))
                : (Labels.Count > x && x >= 0
                    ? Labels[(int) x]
                    : "");
        }

        internal void UpdateSeparations(AxisTags direction, Chart chart, int axisPosition)
        {
            foreach (var separation in Separations.Values.ToArray())
            {
                if (!separation.IsActive)
                {
                    separation.State = SeparationState.Remove;
                    Separations.Remove(separation.Key);
                }
                separation.Place(chart, direction, axisPosition, this);
                separation.IsActive = false;
            }

            LastAxisMax = MaxLimit;
            LastAxisMin = MinLimit;
            LastPlotArea = new Rect(Canvas.GetLeft(chart.DrawMargin), Canvas.GetTop(chart.DrawMargin),
                chart.DrawMargin.Width, chart.DrawMargin.Height);
        }
    }
}