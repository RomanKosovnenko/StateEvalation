using System;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StateEvaluation.BioColor
{
    class Function
    {
        static int mid = Settings.Default.mid,
               square = Settings.Default.square,
               topline = Settings.Default.topline,
               step = Settings.Default.step,
               range = 2;
        static double alpha = Settings.Default.alpha,
               screen_W = 1920,
               screen_H = 1080;

        static public void DrawClear(Grid myGrid)
        {
            Line myLine = new Line
            {
                Stroke = System.Windows.Media.Brushes.Black,
                X1 = mid,
                X2 = mid,
                Y1 = topline,
                Y2 = screen_H,
                StrokeThickness = 1
            };
            myGrid.Children.Add(myLine);

        }

        public static void GetCanvasImage(String a, int x, int Delta, Grid myGrid)
        {
            BitmapImage theImage = new BitmapImage();
            var stream = File.OpenRead(a);
            theImage.BeginInit();
            theImage.CacheOption = BitmapCacheOption.OnLoad;
            theImage.StreamSource = stream;
            theImage.EndInit();
            stream.Close();
            stream.Dispose();

            System.Windows.Media.ImageBrush myImageBrush = new System.Windows.Media.ImageBrush(theImage);

            for (int i = -range; i <= range * 3; ++i)
            {
                myGrid.Children.Add(new Canvas
                {
                    Width = theImage.Width,
                    Height = theImage.Height,
                    Background = myImageBrush,
                    Opacity = alpha,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(i * square * x - Delta % x * square + mid, 0, 0, 0)
                });
            }
        }
    }
}
