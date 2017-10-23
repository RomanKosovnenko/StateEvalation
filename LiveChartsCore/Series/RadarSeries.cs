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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiveCharts.CoreComponents;

namespace LiveCharts
{
    public class RadarSeries : Series
    {
        public RadarSeries()
        {
            StrokeThickness = 2.5;
            PointRadius = 4;
        }

        public double PointRadius { get; set; }

        public override void Plot(bool animate = true)
        {
            if (Visibility != Visibility.Visible) return;
            var chart = Chart as RadarChart;
            if (chart == null) return;
            var alpha = 360/CurrentXAxis.MaxLimit;

            var pf = new PathFigure();
            var segments = new PathSegmentCollection();
            var l = 0d;

            Point? p2 = null;

            if (!Values.Points.Any()) return;
            var lastPoint = Values.Points.Last();
            var fisrtPoint = Values.Points.First();
            foreach (var point in Values.Points)
            {
                var r1 = point != lastPoint
                    ? chart.ToChartRadius(point.Y)
                    : chart.ToChartRadius(fisrtPoint.Y);
                if (point == fisrtPoint)
                    pf.StartPoint = new Point(
                        chart.ActualWidth/2 + Math.Sin(alpha*point.X*(Math.PI/180))*r1,
                        chart.ActualHeight/2 - Math.Cos(alpha*point.X*(Math.PI/180))*r1);
                else
                    segments.Add(new LineSegment
                    {
                        Point = new Point
                        {
                            X = chart.ActualWidth/2 + Math.Sin(alpha*point.X*(Math.PI/180))*r1,
                            Y = chart.ActualHeight/2 - Math.Cos(alpha*point.X*(Math.PI/180))*r1
                        }
                    });

                var p1 = new Point(chart.ActualWidth/2 + Math.Sin(alpha*point.X*(Math.PI/180))*r1,
                    chart.ActualHeight/2 - Math.Cos(alpha*point.X*(Math.PI/180))*r1);
                if (p2 != null)
                {
                    l += Math.Sqrt(
                        Math.Pow(Math.Abs(p1.X - p2.Value.X), 2) +
                        Math.Pow(Math.Abs(p1.Y - p2.Value.Y), 2)
                        );
                }
                p2 = p1;

                if (point == lastPoint) continue;

                var r = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    Width = 40,
                    Height = 40
                };
                var e = new Ellipse
                {
                    Width = PointRadius*2,
                    Height = PointRadius*2,
                    Fill = Stroke,
                    Stroke = new SolidColorBrush {Color = Chart.PointHoverColor},
                    StrokeThickness = 2
                };

                r.MouseEnter += chart.DataMouseEnter;
                r.MouseLeave += chart.DataMouseLeave;
                r.MouseDown += chart.DataMouseDown;
                chart.Canvas.Children.Add(r);
                //Shapes.Add(r);
                chart.ShapesMapper.Add(new ShapeMap
                {
                    Series = this,
                    HoverShape = r,
                    ChartPoint = point,
                    Shape = e
                });

                //Shapes.Add(e);
                chart.Canvas.Children.Add(e);
                Panel.SetZIndex(r, int.MaxValue);

                Canvas.SetLeft(e, p1.X - e.Width/2);
                Canvas.SetTop(e, p1.Y - e.Height/2);
                Panel.SetZIndex(e, 2);

                Canvas.SetLeft(r, p1.X - r.Width/2);
                Canvas.SetTop(r, p1.Y - r.Height/2);
                Panel.SetZIndex(r, int.MaxValue);

                if (!chart.DisableAnimations && animate)
                {
                    var topAnim = new DoubleAnimation
                    {
                        From = chart.ActualHeight/2,
                        To = p1.Y - e.Height/2,
                        Duration = TimeSpan.FromMilliseconds(300)
                    };
                    e.BeginAnimation(Canvas.TopProperty, topAnim);
                    var leftAnim = new DoubleAnimation
                    {
                        From = chart.ActualWidth/2,
                        To = p1.X - e.Width/2,
                        Duration = TimeSpan.FromMilliseconds(300)
                    };
                    e.BeginAnimation(Canvas.LeftProperty, leftAnim);
                }

            }

            pf.Segments = segments;
            var g = new PathGeometry
            {
                Figures = new PathFigureCollection(new List<PathFigure>
                {
                    pf
                })
            };

            var path = new Path
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                Data = g,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                Fill = Fill,
                StrokeDashOffset = l,
                StrokeDashArray = new DoubleCollection { l, l }
            };

            var draw = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromSeconds(0),
                KeyFrames = new DoubleKeyFrameCollection
                {
                    new SplineDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.FromMilliseconds(1),
                        Value = l
                    },
                    new SplineDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.FromMilliseconds(750),
                        Value = 0
                    }
                }
            };
            Storyboard.SetTarget(draw, path);
            Storyboard.SetTargetProperty(draw, new PropertyPath(Shape.StrokeDashOffsetProperty));
            var sbDraw = new Storyboard();
            sbDraw.Children.Add(draw);
            var animated = false;
            if (!chart.DisableAnimations)
            {
                if (animate)
                {
                    sbDraw.Begin();
                    animated = true;
                }
            }
            if (!animated) path.StrokeDashOffset = 0;

            chart.Canvas.Children.Add(path);
            //Shapes.Add(path);
        }
    }
}
