using System;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StateEvaluation.BioColor
{
    public class Function
    {
        private readonly int mid = Settings.Default.mid;
        private readonly int square = Settings.Default.square;
        private readonly int topline = Settings.Default.topline;
        private readonly int step = Settings.Default.step;
        private readonly int range = 2;
        private readonly double alpha = Settings.Default.alpha;
        private readonly double screen_W = 1920;
        private readonly double screen_H = 1080;

        private BiocolorProvider biocolorManager;
        private ImageGenerator imageGenerator;

        public Function()
        {
            biocolorManager = new BiocolorProvider();
            imageGenerator = new ImageGenerator();
        }

        internal Color ColorMix(Color c1, Color c2, Color c3)
        {

            const int DIVIDER_3 = 3;
            const int DIVIDER_2 = 2;
            const int SPLACER = 128;

            int[] C1 = imageGenerator.RgbToCmyk(c1);
            int[] C2 = imageGenerator.RgbToCmyk(c2);
            int[] C3 = imageGenerator.RgbToCmyk(c3);

            if (c1.A > SPLACER && c2.A > SPLACER && c3.A > SPLACER)
            {
                int[] RGB = imageGenerator.CmykToRgb(
                    (C1[0] + C2[0] + C3[0]) / DIVIDER_3,
                    (C1[1] + C2[1] + C3[1]) / DIVIDER_3,
                    (C1[2] + C2[2] + C3[2]) / DIVIDER_3,
                    (C1[3] + C2[3] + C3[3]) / DIVIDER_3
                );
                return Color.FromArgb(RGB[0], RGB[1], RGB[2]);
            }
            else if (c1.A > SPLACER && c2.A > SPLACER ||
                    c2.A > SPLACER && c3.A > SPLACER ||
                    c3.A > SPLACER && c1.A > SPLACER)
            {
                int[] RGB = imageGenerator.CmykToRgb(
                    (C1[0] + C2[0] + C3[0]) / DIVIDER_2,
                    (C1[1] + C2[1] + C3[1]) / DIVIDER_2,
                    (C1[2] + C2[2] + C3[2]) / DIVIDER_2,
                    (C1[3] + C2[3] + C3[3]) / DIVIDER_2
                );
                return Color.FromArgb(RGB[0], RGB[1], RGB[2]);
            }
            else
            {
                if (c1.A > SPLACER) return c1;
                else if (c2.A > SPLACER) return c2;
                else if (c3.A > SPLACER) return c3;
                else return Color.FromArgb(0, 0, 0, 0);
            }
        }


        public void DrawClear(Grid myGrid)
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

        public void GetCanvasImage(String a, int x, int Delta, Grid myGrid)
        {
            try
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
            catch (FileNotFoundException)
            {
                biocolorManager.Generate();
            }
        }

    }
}
