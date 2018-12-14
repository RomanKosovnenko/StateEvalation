using StateEvaluation.Common;
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
    public class BiocolorProvider
    {
        private Grid _myGrid;
        private DatePicker _birthDate;
        private DatePicker _nowDate;

        private const int RangeRed = 23;
        private const int RangeGreen = 28;
        private const int RangeBlue = 33;
        private const int HalfHexFf = 128;
        private const int Height = 480;
        private const int Zero = 0;
        private readonly int Mid;
        private readonly int Square;
        private double Alpha;

        private readonly int IntRed;
        private readonly int IntGreen;
        private readonly int IntBlue;
        private int[] days;
        private readonly string[] paths;

        private Function function;
        private ImageGenerator imageGenerator;

        public BiocolorProvider()
        {
            Mid = Settings.Default.mid;
            Square = Settings.Default.square;
            Alpha = Settings.Default.alpha;
            IntRed = Settings.Default.Int_Red;
            IntGreen = Settings.Default.Int_Green;
            IntBlue = Settings.Default.Int_Blue;
            days = new int[] { 23, 28, 33 };
            paths = new string[days.Length];

            function = new Function();
            imageGenerator = new ImageGenerator();
        }

        public void InitBioColor(Grid myGrid, DatePicker birthDate, DatePicker nowDate)
        {
            _myGrid = myGrid;
            _birthDate = birthDate;
            _nowDate = nowDate;
            for (int i = 0; i < days.Length; ++i) {
                paths[i] = System.IO.Directory.GetCurrentDirectory() + "/../../BioColor/template/Image_" + days[i] + ".png";
            }
        }
        public void MakeStep(int step)
        {
            _nowDate.Text = Convert.ToDateTime(_nowDate.Text).AddDays(step).ToString(CultureInfo.CurrentCulture);
            DrawPicture();
        }
        public void DrawGraphs()
        {
            DrawPicture();
        }

        private void DrawPicture()
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
                function.GetCanvasImage(paths[0], IntRed, delta, _myGrid);
                function.GetCanvasImage(paths[1], IntGreen, delta, _myGrid);
                function.GetCanvasImage(paths[2], IntBlue, delta, _myGrid);

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
                        w.SetPixel(x, y, function.ColorMix(rColor, gColor, bColor));

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
                        w.SetPixel(x, y, function.ColorMix(rColor, gColor, bColor));

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
                function.DrawClear(_myGrid);

            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect Date!");
            }

        }
        public void Menu()
        {
            Colors c = new Colors();
            c.ShowDialog();
            c.Save();

            imageGenerator.Generate(23);
            imageGenerator.Generate(28);
            imageGenerator.Generate(33);
        }
        public void Generate()
        {
            imageGenerator.Generate(RangeRed);
            imageGenerator.Generate(RangeGreen);
            imageGenerator.Generate(RangeBlue);
        }
    }
}
