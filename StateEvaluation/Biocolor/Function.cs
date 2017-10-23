using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppPS.Properties;

namespace WpfAppPS
{
    class Function
    {

        static int mid = Settings.Default.mid,
               square = Settings.Default.square,
               topline = Settings.Default.topline,
               step = Settings.Default.step,
               range = Settings.Default.Range;
        static double alpha = Settings.Default.alpha;
        static public int ColorMix(int a, int b, int c)
        {
            return (a + b + c) / 3;
        }
        internal static Color ColorMix(Color c1, Color c2, Color c3)
        {
            int x;
            if (c1.A > 128 && c2.A > 128 && c3.A > 128) x = 3;
            else if (c1.A > 128 && c2.A > 128 || c2.A > 128 && c3.A > 128 || c3.A > 128 && c1.A > 128) x = 3;
            else x = 1;

            byte a = (byte)((c1.A + c2.A + c3.A)  );
            byte r = (byte)((c1.R/x + c2.R/x + c3.R/x)  );
            byte g = (byte)((c1.G/x + c2.G/x + c3.G/x)  );
            byte b = (byte)((c1.B/x + c2.B/x + c3.B/x)  );
            return System.Drawing.Color.FromArgb( r, g, b);
        }
        

        static public void DrawClear(Grid myGrid)
        {
            System.Windows.Shapes.Rectangle Re = new System.Windows.Shapes.Rectangle();
            Re.Fill = System.Windows.Media.Brushes.White;
            Re.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Re.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Thickness margin = Re.Margin;
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Black;
            myLine.X1 = mid;
            myLine.X2 = mid;
            myLine.Y1 = topline;
            myLine.Y2 = SystemParameters.FullPrimaryScreenHeight;
            myLine.StrokeThickness = 1;
            margin.Top = topline;
            Re.Margin = margin;
            Re.Width = SystemParameters.FullPrimaryScreenWidth;
            Re.Height = SystemParameters.FullPrimaryScreenHeight;
            myGrid.Children.Add(Re);
            myGrid.Children.Add(myLine);

        }

        static public void Add(String a, int x, int Delta, Grid myGrid)
        {

            BitmapImage theImage = new BitmapImage
                (new Uri(a, UriKind.Relative));

            System.Windows.Media.ImageBrush myImageBrush = new System.Windows.Media.ImageBrush(theImage);
            try
            {

                for (int i = -range; i <= range; ++i)
                {
                    Canvas myCanvas = new Canvas();
                    myCanvas.Width = theImage.Width;
                    myCanvas.Height = theImage.Height;
                    myCanvas.Background = myImageBrush;
                    myCanvas.Opacity = alpha;
                    myCanvas.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    myCanvas.Margin = new Thickness(i * square * x - Delta % x * square + mid, 0, 0, 0);
                    myGrid.Children.Add(myCanvas);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                System.Windows.MessageBox.Show("Error_" + x + "_File!");
            }
        }
        static public void doMix(string[] imageArray)
        {
            try
            {
                System.Drawing.Bitmap[] bmSrcs = new System.Drawing.Bitmap[imageArray.Length];
                // read all files into memory
                for (int b = 0; b < bmSrcs.Length; b++)
                {
                    bmSrcs[b] = new System.Drawing.Bitmap(imageArray[b]);
                }
                System.Drawing.Bitmap bmDst = new System.Drawing.Bitmap(bmSrcs[0].Width, bmSrcs[0].Height);
                for (int y = 0; y < bmSrcs[0].Height; y++)
                {
                    for (int x = 0; x < bmSrcs[0].Width; x++)
                    {
                        int a = 0, r = 0, g = 0, b = 0, iCount = 0;
                        foreach (System.Drawing.Bitmap bmSrc in bmSrcs)
                        {
                            System.Drawing.Color colSrc = bmSrc.GetPixel(x, y);
                            // check alpha (transparency): ignore transparent pixels
                            if (colSrc.A > 0)
                            {
                                a += colSrc.A;
                                r = Math.Max(r, colSrc.R);
                                g = Math.Max(g, colSrc.G);
                                b = Math.Max(b, colSrc.B);
                                iCount++;
                            }
                        }
                        System.Drawing.Color colDst = System.Drawing.Color.FromArgb(iCount > 1 ? (int)Math.Round((double)a / iCount) : a, r, g, b);
                        bmDst.SetPixel(x, y, colDst);
                    }
                }
                bmDst.Save("D:\\result.png", System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

    }
}
