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
using System.Linq;
using System.Windows;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public class StackedBarChart : Chart, IStackedBar, ILine
    {
        public StackedBarChart()
        {
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
            IndexTotals = new Dictionary<int, StackedBarHelper>();
            MaxColumnWidth = 40;
            DefaultFillOpacity = 0.75;
            LineSmoothness = 0.8;

            throw new Exception("StackedBar Charts are currently disabled");
        }

        #region Properties
        /// <summary>
        /// Gets or sets maximum column width, default is 60
        /// </summary>
        public double MaxColumnWidth { get; set; }
        /// <summary>
        /// Gets or sets Line Smoothness
        /// </summary>
        public double LineSmoothness { get; set; }
        /// <summary>
        /// Gets a dictinary that groups every bar proportion
        /// </summary> 
        public Dictionary<int, StackedBarHelper> IndexTotals { get; internal set; }

        internal new bool HasValidSeriesAndValues
        {
            get { return Series.Any(x => x.Values != null && x.Values.Count > 0); }
        }
        #endregion

        private Point GetMax()
        {
            var s = Series.FirstOrDefault();
            if (s == null) return new Point(0,0);

            //var p = Invert
            //    ? new Point(s.Values.Points.Select(
            //        (t, i) => Series.OfType<StackedBarSeries>().Sum(serie => serie.Values.Points.Any()
            //            ? (serie.Values.Points as IList<ChartPoint>)[i].X
            //            : double.MinValue))
            //        .Concat(new[] {double.MinValue}).Max(),
            //        Series.Select(x => x.Values.MaxChartPoint.Y).DefaultIfEmpty(0).Max() )
            //    : new Point(Series.Select(x => x.Values.MaxChartPoint.X).DefaultIfEmpty(0).Max() +1,
            //        s.Values.Points.Select(
            //            (t, i) => Series.OfType<StackedBarSeries>().Sum(serie => serie.Values.Points.Any()
            //                ? (serie.Values.Points as IList<ChartPoint>)[i].Y
            //                : double.MinValue))
            //            .Concat(new[] {double.MinValue}).Max());


            ////correction for lineSeries
            //if (Invert)
            //{
            //    var maxLineSeries =
            //    Series.OfType<LineSeries>()
            //        .Select(series => series.Values.Points.Select(x => x.X).DefaultIfEmpty(0).Max())
            //        .DefaultIfEmpty(0)
            //        .Max();
            //    p.X = p.X > maxLineSeries ? p.X : maxLineSeries;
            //    p.X = AxisX.MaxValue ?? p.X;
            //}
            //else
            //{
            //    var maxLineSeries =
            //    Series.OfType<LineSeries>()
            //        .Select(series => series.Values.Points.Select(x => x.Y).DefaultIfEmpty(0).Max())
            //        .DefaultIfEmpty(0)
            //        .Max();
            //    p.Y = p.Y > maxLineSeries ? p.Y : maxLineSeries;
            //    p.Y = AxisY.MaxValue ?? p.Y;
            //}

            //return p;

            return new Point();
        }

        private Point GetMin()
        {
            //var s = Series.FirstOrDefault();
            //if (s==null) return new Point(0,0);
            //var p = Invert
            //    ? new Point(
            //        s.Values.Points.Select(
            //            (t, i) => Series.OfType<StackedBarSeries>().Sum(serie => serie.Values.Points.Any()
            //                ? (serie.Values.Points as IList<ChartPoint>)[i].X
            //                : double.MinValue))
            //            .Concat(new[] { double.MaxValue }).Min(),
            //        Series.Select(x => x.Values.MinChartPoint.Y).DefaultIfEmpty(0).Min() -1)
            //    : new Point(
            //        Series.Select(x => x.Values.MinChartPoint.X).DefaultIfEmpty(0).Min(),
            //        s.Values.Points.Select(
            //            (t, i) => Series.OfType<StackedBarSeries>().Sum(serie => serie.Values.Points.Any()
            //                ? (serie.Values.Points as IList<ChartPoint>)[i].Y
            //                : double.MinValue))
            //            .Concat(new[] {double.MaxValue}).Min());

            ////correction for lineSeries
            //if (Invert)
            //{
            //    var minLineSeries =
            //    Series.OfType<LineSeries>()
            //        .Select(series => series.Values.Points.Select(x => x.X).DefaultIfEmpty(0).Min())
            //        .DefaultIfEmpty(0)
            //        .Min();
            //    p.X = p.X < minLineSeries ? p.X : minLineSeries;

            //    p.X = AxisX.MinValue ?? p.X;
            //}
            //else
            //{
            //    var minLineSeries =
            //    Series.OfType<LineSeries>()
            //        .Select(series => series.Values.Points.Select(x => x.Y).DefaultIfEmpty(0).Min())
            //        .DefaultIfEmpty(0)
            //        .Min();
            //    p.Y = p.Y < minLineSeries ? p.Y : minLineSeries;

            //    p.Y = AxisY.MinValue ?? p.Y;
            //}
            //return p;
            return new Point();
        }

        protected override void PrepareAxes()
        {
            //InitializeComponents();

            //if (!HasValidSeriesAndValues) return;

            //if (Invert)
            //    foreach (var xi in AxisX) xi.MinValue = 0;
            //else
            //    foreach (var yi in AxisY) yi.MinValue = 0;

            //var stackedSeries = Series.OfType<StackedBarSeries>().ToList();

            ////All series must have the same number of items.
            ////This chart should be kinf of buggy when adding mutiple axes.
            ////ToDo: Review stacked bar chart and multiple axes
            ////since the chart has only a StackedBarHelper,
            ////then the escale of all axis mut be shared by this class,
            ////one possible solution is to add a StackedBarHelper for each axis.
            ////For now it is not safe to use multiple axis with stackedbar chart
            ////unless the other axis do not contain StackedBarSeries.
            //var fSerie = stackedSeries.FirstOrDefault();
            //if (fSerie == null) return;
            //for (var i = 0; i < fSerie.Values.Count; i++)
            //{
            //    var helper = new StackedBarHelper();
            //    var sum = 0d;
            //    for (int index = 0; index < stackedSeries.Count; index++)
            //    {
            //        var serie = stackedSeries[index];
            //        var p = (serie.Values.Points as IList<ChartPoint>)[i];
            //        var value = serie.Values.Points.Any()
            //            ? (Invert ? p.X : p.Y)
            //            : double.MinValue;
            //        helper.Stacked[index] = new StackedItem
            //        {
            //            Value = value,
            //            Stacked = sum
            //        };
            //        sum += value;
            //    }
            //    helper.Total = sum;
            //    IndexTotals[i] = helper;
            //}

            ////InitializeComponents();

            //var s = Series.FirstOrDefault();

            //var p = Invert
            //    ? new Point(s.Values.Points.Select(
            //        (t, i) => Series.OfType<StackedBarSeries>().Sum(serie => serie.Values.Points.Any()
            //            ? (serie.Values.Points as IList<ChartPoint>)[i].X
            //            : double.MinValue))
            //        .Concat(new[] { double.MinValue }).Max(),
            //        Series.Select(x => x.Values.MaxChartPoint.Y).DefaultIfEmpty(0).Max())
            //    : new Point(Series.Select(x => x.Values.MaxChartPoint.X).DefaultIfEmpty(0).Max() + 1,
            //        s.Values.Points.Select(
            //            (t, i) => Series.OfType<StackedBarSeries>().Sum(serie => serie.Values.Points.Any()
            //                ? (serie.Values.Points as IList<ChartPoint>)[i].Y
            //                : double.MinValue))
            //            .Concat(new[] { double.MinValue }).Max());


            ////correction for lineSeries
            //if (Invert)
            //{
            //    var maxLineSeries =
            //    Series.OfType<LineSeries>()
            //        .Select(series => series.Values.Points.Select(x => x.X).DefaultIfEmpty(0).Max())
            //        .DefaultIfEmpty(0)
            //        .Max();
            //    p.X = p.X > maxLineSeries ? p.X : maxLineSeries;
            //    p.X = AxisX.MaxValue ?? p.X;
            //}
            //else
            //{
            //    var maxLineSeries =
            //    Series.OfType<LineSeries>()
            //        .Select(series => series.Values.Points.Select(x => x.Y).DefaultIfEmpty(0).Max())
            //        .DefaultIfEmpty(0)
            //        .Max();
            //    p.Y = p.Y > maxLineSeries ? p.Y : maxLineSeries;
            //    p.Y = AxisY.MaxValue ?? p.Y;
            //}

            //for (var index = 0; index < AxisX.Count; index++)
            //{
            //    var xi = AxisX[index];
            //    xi.MaxLimit = xi.MaxValue ??
            //                  Series.Where(series => series.Values != null && series.ScalesXAt == index)
            //                      .Select(series => series.Values.MaxChartPoint.X).DefaultIfEmpty(0).Max();
            //    xi.MinLimit = xi.MinValue ??
            //                  Series.Where(series => series.Values != null && series.ScalesXAt == index)
            //                      .Select(series => series.Values.MinChartPoint.X).DefaultIfEmpty(0).Min();
            //}

            //for (var index = 0; index < AxisY.Count; index++)
            //{
            //    var yi = AxisY[index];
            //    yi.MaxLimit = yi.MaxValue ??
            //                  Series.Where(series => series.Values != null && series.ScalesYAt == index)
            //                      .Select(series => series.Values.MaxChartPoint.Y).DefaultIfEmpty(0).Max();
            //    yi.MinLimit = yi.MinValue ??
            //                  Series.Where(series => series.Values != null && series.ScalesYAt == index)
            //                      .Select(series => series.Values.MinChartPoint.Y).DefaultIfEmpty(0).Min();
            //}

            foreach (var xi in AxisX)
            {
                if (!Invert) xi.MaxLimit += 1;
                xi.CalculateSeparator(this, AxisTags.X);
                if (!Invert) continue;
                if (xi.MaxValue == null) xi.MaxLimit = (Math.Round(xi.MaxLimit / xi.S) + 1) * xi.S;
                if (xi.MinValue == null) xi.MinLimit = (Math.Truncate(xi.MinLimit / xi.S) - 1) * xi.S;
            }

            foreach (var yi in AxisY)
            {
                if (Invert) yi.MinLimit -= 1;
                yi.CalculateSeparator(this, AxisTags.X);
                if (Invert) continue;
                if (yi.MaxValue == null) yi.MaxLimit = (Math.Round(yi.MaxLimit / yi.S) + 1) * yi.S;
                if (yi.MinValue == null) yi.MinLimit = (Math.Truncate(yi.MinLimit / yi.S) - 1) * yi.S;
            }

            CalculateComponentsAndMargin();
        }

        protected override Point GetToolTipPosition(ShapeMap sender, List<ShapeMap> sibilings)
        {
            //DataTooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //var unitW = ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;
            //var overflow = unitW - MaxColumnWidth > 0 ? unitW - MaxColumnWidth : 0;
            //unitW = unitW > MaxColumnWidth ? MaxColumnWidth : unitW;

            //var x = sender.ChartPoint.X + 1 > (Min.X +AxisX.MaxLimit) / 2
            //    ? ToPlotArea(sender.ChartPoint.X, AxisTags.X) + overflow * .5 - DataTooltip.DesiredSize.Width
            //    : ToPlotArea(sender.ChartPoint.X, AxisTags.X) + unitW + overflow * .5;
            //var y = ToPlotArea(sibilings.Where(s => s.Series is StackedBarSeries).Select(s => s.ChartPoint.Y).DefaultIfEmpty(0).Sum()*0.5, AxisTags.Y);
            //y = y + DataTooltip.DesiredSize.Height > ActualHeight
            //    ? y - (y + DataTooltip.DesiredSize.Height - ActualHeight) - 5
            //    : y;

            //return new Point(x, y);
            return new Point();
        }

        protected override void CalculateComponentsAndMargin()
        {
   //         if (Invert)
   //         {
   //         } //AxisY.IgnoresLastLabel = true;
   //         else AxisX.IgnoresLastLabel = true;

   //         if (Invert) ConfigureYAsIndexed();
   //         else ConfigureXAsIndexed();

   //         S = GetS();

   //         Canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

   //         var lastLabelX = Math.Truncate((Max.X -AxisX.MinLimit) /AxisX.S) *AxisX.S;
   //         var longestYLabelSize = GetLongestLabelSize(AxisY, AxisTags.Y);
   //         var fistXLabelSize = GetLabelSize(AxisX,AxisX.MinLimit);
   //         var lastXLabelSize = GetLabelSize(AxisX, lastLabelX);

   //         const int padding = 5;

   //         var unitW = Invert
   //              ? ToPlotArea(Max.Y - 1, AxisTags.Y) - PlotArea.Y + 5
   //              : ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;
   //         unitW = unitW > MaxColumnWidth * 3 ? MaxColumnWidth * 3 : unitW;

   //         if (Invert) YOffset = unitW / 2;
   //         else XOffset = unitW / 2;

   //         PlotArea.X = padding * 2 +
   //                      (fistXLabelSize.X * 0.5 - XOffset > longestYLabelSize.X
   //                          ? fistXLabelSize.X * 0.5 - XOffset
   //                          : longestYLabelSize.X);
   //         PlotArea.Y = longestYLabelSize.Y * .5 + padding;
   //         PlotArea.Height = Math.Max(0, Canvas.DesiredSize.Height - (padding * 2 + fistXLabelSize.Y) - PlotArea.Y);
   //         PlotArea.Width = Math.Max(0,  Canvas.DesiredSize.Width - PlotArea.X - padding);
   //         var distanceToEnd = PlotArea.Width - (ToPlotArea(Max.X, AxisTags.X) - ToPlotArea(1, AxisTags.X));
   //         distanceToEnd -= XOffset + padding;
	  //      var change = lastXLabelSize.X * .5 - distanceToEnd > 0 ? lastXLabelSize.X * .5 - distanceToEnd : 0;
			//if (change <= PlotArea.Width)
	  //          PlotArea.Width -= change;

   //         //calculate it again to get a better result
   //         unitW = Invert
   //             ? ToPlotArea(Max.Y - 1, AxisTags.Y) - PlotArea.Y + 5
   //             : ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;
   //         unitW = unitW > MaxColumnWidth * 3 ? MaxColumnWidth * 3 : unitW;

   //         if (Invert) YOffset = unitW / 2;
   //         else XOffset = unitW / 2;

            base.CalculateComponentsAndMargin();
        }
    }

    public class StackedBarHelper
    {
        public StackedBarHelper()
        {
            Stacked = new Dictionary<int, StackedItem>();
        }
        public double Total { get; set; }
        public Dictionary<int, StackedItem> Stacked { get; set; }
    }

    public class StackedItem
    {
        public double Value { get; set; }
        public double Stacked { get; set; }
    }
}
