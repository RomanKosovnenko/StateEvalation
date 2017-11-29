using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StateEvaluation.BioColor
{
    class Main
    {
        // BioColor 
        private static Grid _myGrid;
        private static DatePicker _birthDate, _nowDate;

        private const int rangeRed = 23, rangeGreen = 28, rangeBlue = 33;
        private const int HALF_HEX_FF = 128, HEIGHT = 480, ZERO = 0;
        private const int SQUARE_SIZE = 20;
        static int mid = Settings.Default.mid,
               square = Settings.Default.square;
        static double alpha = Settings.Default.alpha;

        static int _intRed = Settings.Default.Int_Red,
            _intGreen = Settings.Default.Int_Green,
            _intBlue = Settings.Default.Int_Blue;

        static string _pathRed, _pathGreen, _pathBlue;
        private static string _initDate;

        public static void InitBioColor(Grid myGrid, DatePicker birthDate, DatePicker nowDate)
        {
            Main._myGrid = myGrid;
            Main._birthDate = birthDate;
            Main._nowDate = nowDate;
            Restore();
        }

        private static void Save()
        {
            Settings.Default.Path_Red = _pathRed;
            Settings.Default.Path_Green = _pathGreen;
            Settings.Default.Path_Blue = _pathBlue;
            Settings.Default.Init_date = _initDate;
            Settings.Default.Save();
        }
        public static void Restore()
        {
            _pathRed = Settings.Default.Path_Red;
            _pathGreen = Settings.Default.Path_Green;
            _pathBlue = Settings.Default.Path_Blue;
            _initDate = Settings.Default.Init_date;
        }
        public static void MakeStep(int step)
        {
            _nowDate.Text = Convert.ToDateTime(_nowDate.Text).AddDays(step).ToString();
            DrawPicture();
        }
        public static void DrawGraphs()
        {
            DrawPicture();
        }

        private static void DrawPicture()
        {
            Save();
            try
            {
                DateTime dt = Convert.ToDateTime(_birthDate.Text);
                DateTime nw = Convert.ToDateTime(_nowDate.Text);
                int delta = (int)((nw.Subtract(dt)).TotalDays);

                int width = 1000 + mid;
                Function.DrawClear(_myGrid);
                Function.GetCanvasImage(_pathRed, _intRed, delta, _myGrid);
                Function.GetCanvasImage(_pathGreen, _intGreen, delta, _myGrid);
                Function.GetCanvasImage(_pathBlue, _intBlue, delta, _myGrid);

                Bitmap r = new Bitmap(_pathRed);
                Bitmap g = new Bitmap(_pathGreen);
                Bitmap b = new Bitmap(_pathBlue);
                Bitmap w = new Bitmap(width, HEIGHT);

                for (int x = 0; x < width; ++x)
                {
                    for (int y = HEIGHT / 2 - 1; y > 0; --y)
                    {
                        int xPixel = (x + delta * square - mid);
                        System.Drawing.Color rColor = r.GetPixel((xPixel + r.Width) % r.Width, y);
                        System.Drawing.Color gColor = g.GetPixel((xPixel + g.Width) % g.Width, y);
                        System.Drawing.Color bColor = b.GetPixel((xPixel + b.Width) % b.Width, y);
                        if (
                            rColor.A < HALF_HEX_FF && gColor.A < HALF_HEX_FF ||
                            rColor.A < HALF_HEX_FF && bColor.A < HALF_HEX_FF ||
                            gColor.A < HALF_HEX_FF && bColor.A < HALF_HEX_FF
                            ) break;
                        w.SetPixel(x, y, Function.ColorMix(rColor, gColor, bColor));

                    }
                    for (int y = HEIGHT / 2 ; y < HEIGHT; ++y)
                    {
                        int xPixel = (x + delta * square - mid);
                        System.Drawing.Color rColor = r.GetPixel((xPixel + r.Width) % r.Width, y);
                        System.Drawing.Color gColor = g.GetPixel((xPixel + g.Width) % g.Width, y);
                        System.Drawing.Color bColor = b.GetPixel((xPixel + b.Width) % b.Width, y);
                        if (
                            rColor.A < HALF_HEX_FF && gColor.A < HALF_HEX_FF ||
                            rColor.A < HALF_HEX_FF && bColor.A < HALF_HEX_FF ||
                            gColor.A < HALF_HEX_FF && bColor.A < HALF_HEX_FF
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
                    Opacity = alpha,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                _myGrid.Children.Add(myCanvas);
            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect Date!");
            }

        }
        public static void Menu()
        {
            SettingsEdit s = new SettingsEdit();
            s.ShowDialog();
            s.Get();
            Restore();
        }
        public static void Generate()
        {
            ImageGenerator.Generate(rangeRed);
            ImageGenerator.Generate(rangeGreen);
            ImageGenerator.Generate(rangeBlue);
            MessageBox.Show("Generated!");
            // Application.Current.Shutdown(); 
            // myGrid.Children.Add();
        }
    }
}
