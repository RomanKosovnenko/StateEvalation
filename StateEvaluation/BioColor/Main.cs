using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StateEvaluation.BioColor
{
    internal static class Main
    {
        // BioColor 
        private static Grid _myGrid;
        private static DatePicker _birthDate, _nowDate;

        private const int RangeRed = 23, RangeGreen = 28, RangeBlue = 33;
        private const int HalfHexFf = 128, Height = 480, Zero = 0;
        private static readonly int Mid = Settings.Default.mid,
               Square = Settings.Default.square;
        private static readonly double Alpha = Settings.Default.alpha;

        private static readonly int IntRed = Settings.Default.Int_Red,
            IntGreen = Settings.Default.Int_Green,
            IntBlue = Settings.Default.Int_Blue;
        private static int[] days = new int[] { 23, 28, 33 };
        private static string[] paths = new string[days.Length];
        public static void InitBioColor(Grid myGrid, DatePicker birthDate, DatePicker nowDate)
        {
            _myGrid = myGrid;
            _birthDate = birthDate;
            _nowDate = nowDate;
            for (int i = 0; i < days.Length; ++i) {
                paths[i] = System.IO.Directory.GetCurrentDirectory() + "/../../BioColor/template/Image_" + days[i] + ".png";
            }
        }
        public static void MakeStep(int step)
        {
            _nowDate.Text = Convert.ToDateTime(_nowDate.Text).AddDays(step).ToString(CultureInfo.CurrentCulture);
            DrawPicture();
        }
        public static void DrawGraphs()
        {
            DrawPicture();
        }

        private static void DrawPicture()
        {
            try
            {
                DateTime dt = Convert.ToDateTime(_birthDate.Text);
                DateTime nw = Convert.ToDateTime(_nowDate.Text);
                if (dt > nw)
                {
                    MessageBox.Show("Birthday greater than date!");
                    return;
                }
                int delta = (int)((nw.Subtract(dt)).TotalDays);

                int width = (int)(SystemParameters.PrimaryScreenWidth) + Mid;
                Bitmap r;
                Bitmap g;
                Bitmap b;
                Bitmap w;
                _myGrid.Children.Clear();
                Function.GetCanvasImage(paths[0], IntRed, delta, _myGrid);
                Function.GetCanvasImage(paths[1], IntGreen, delta, _myGrid);
                Function.GetCanvasImage(paths[2], IntBlue, delta, _myGrid);

                using (var fs = new System.IO.FileStream(paths[0], System.IO.FileMode.Open))
                {
                    r = (Bitmap)new Bitmap(fs).Clone();
                }
                using (var fs = new System.IO.FileStream(paths[1], System.IO.FileMode.Open))
                {
                    g = (Bitmap)new Bitmap(fs).Clone();
                }
                using (var fs = new System.IO.FileStream(paths[2], System.IO.FileMode.Open))
                {
                    b = (Bitmap)new Bitmap(fs).Clone();
                }
                    
                w = new Bitmap(width, Height);

                for (int x = Zero; x < width; ++x)
                {
                    for (int y = Height / 2 - 1; Zero < y; --y)
                    {
                        int xPixel = (x + delta * Square - Mid);
                        System.Drawing.Color rColor = r.GetPixel((xPixel + r.Width) % r.Width, y);
                        System.Drawing.Color gColor = g.GetPixel((xPixel + g.Width) % g.Width, y);
                        System.Drawing.Color bColor = b.GetPixel((xPixel + b.Width) % b.Width, y);
                        if (
                            rColor.A < HalfHexFf && gColor.A < HalfHexFf ||
                            rColor.A < HalfHexFf && bColor.A < HalfHexFf ||
                            gColor.A < HalfHexFf && bColor.A < HalfHexFf
                            ) break;
                        w.SetPixel(x, y, Function.ColorMix(rColor, gColor, bColor));

                    }
                    for (int y = Height / 2; y < Height; ++y)
                    {
                        int xPixel = (x + delta * Square - Mid);
                        System.Drawing.Color rColor = r.GetPixel((xPixel + r.Width) % r.Width, y);
                        System.Drawing.Color gColor = g.GetPixel((xPixel + g.Width) % g.Width, y);
                        System.Drawing.Color bColor = b.GetPixel((xPixel + b.Width) % b.Width, y);
                        if (
                            rColor.A < HalfHexFf && gColor.A < HalfHexFf ||
                            rColor.A < HalfHexFf && bColor.A < HalfHexFf ||
                            gColor.A < HalfHexFf && bColor.A < HalfHexFf
                            ) break;
                        w.SetPixel(x, y, Function.ColorMix(rColor, gColor, bColor));

                    }
                }
                MemoryStream memoryStream = new MemoryStream();
                w.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                BitmapImage theImage = new BitmapImage();
                {
                    theImage.BeginInit();
                    theImage.CacheOption = BitmapCacheOption.OnLoad;
                    theImage.StreamSource = memoryStream;
                    theImage.EndInit();
                }

                ImageBrush myImageBrush = new ImageBrush(theImage);
                Canvas myCanvas = new Canvas
                {
                    Width = theImage.Width,
                    Height = theImage.Height,
                    Background = myImageBrush,
                    Opacity = Alpha,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                _myGrid.Children.Add(myCanvas);
                Function.DrawClear(_myGrid);

            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect Date!");
            }

        }
        public static void Menu()
        {
            Colors c = new Colors();
            c.ShowDialog();
            c.Save();

            ImageGenerator.Generate(23);
            ImageGenerator.Generate(28);
            ImageGenerator.Generate(33);
        }
        public static void Generate()
        {
            ImageGenerator.Generate(RangeRed);
            ImageGenerator.Generate(RangeGreen);
            ImageGenerator.Generate(RangeBlue);
        }
    }
}
