using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StateEvaluation.BioColor.Providers
{
    public class BiocolorProvider
    {
        private Grid _myGrid;
        private DatePicker _birthDate;
        private DatePicker _nowDate;

        private const int HalfHexFf = 128;
        private const int Height = 480;

        private const int Screen_W = 1920;
        private const int Screen_H = 1080;

        private const int Zero = 0;

        private const int Range = 2;

        private readonly int Mid;
        private readonly int Square;
        private double Alpha;

        private readonly int IntRed;
        private readonly int IntGreen;
        private readonly int IntBlue;
        private readonly int Topline;
        private int[] days;
        private readonly string[] paths;
        
        private Helpers.BiocolorSettings biocolorSettings;

        public BiocolorProvider(Helpers.BiocolorSettings biocolorSettings)
        {
            this.biocolorSettings = biocolorSettings;

            Mid = biocolorSettings.Mid;
            Square = biocolorSettings.Square;
            Alpha = biocolorSettings.Alpha;
            IntRed = biocolorSettings.Int_Red;
            IntGreen = biocolorSettings.Int_Green;
            IntBlue = biocolorSettings.Int_Blue;
            Topline = biocolorSettings.Topline;
            days = new int[] { 23, 28, 33 };
            paths = new string[days.Length];
            
            biocolorSettings = new Helpers.BiocolorSettings();
        }

        public void InitBiocolor(Grid myGrid, DatePicker birthDate, DatePicker nowDate)
        {
            _myGrid = myGrid;
            _birthDate = birthDate;
            _nowDate = nowDate;
            for (int i = 0; i < days.Length; ++i) {
                paths[i] = Directory.GetCurrentDirectory() + "../../../Assets/Templates/Image_" + days[i] + ".png";
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

        /// <summary>
        /// Draw Pictures for selected Birthday and for selected Date
        /// Generates three canvases and adds it together
        /// </summary>
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
                GenerateCanvasImage(paths[0], IntRed, delta, _myGrid);
                GenerateCanvasImage(paths[1], IntGreen, delta, _myGrid);
                GenerateCanvasImage(paths[2], IntBlue, delta, _myGrid);
                
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
                        w.SetPixel(x, y, Helpers.ColorConverter.Mix(rColor, gColor, bColor));

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
                        w.SetPixel(x, y, Helpers.ColorConverter.Mix(rColor, gColor, bColor));

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
                DrawTodayLine(_myGrid);

            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect Date!");
            }
        }

        /// <summary>
        /// Draw vertical line for today
        /// </summary>
        /// <param name="myGrid"></param>
        public void DrawTodayLine(Grid myGrid)
        {
            Line myLine = new Line
            {
                Stroke = System.Windows.Media.Brushes.Black,
                X1 = Mid,
                X2 = Mid,
                Y1 = Topline,
                Y2 = Screen_H,
                StrokeThickness = 1
            };
            myGrid.Children.Add(myLine);
        }

        /// <summary>
        /// Generates canvas image from given file for some range
        /// </summary>
        /// <param name="path">Path to image file</param>
        /// <param name="daysCount">Count of days of given image</param>
        /// <param name="Delta">Difference between Birthdate and Given date in days</param>
        /// <param name="myGrid"></param>
        public void GenerateCanvasImage(string path, int daysCount, int Delta, Grid myGrid)
        {
            BitmapImage theImage = new BitmapImage();
            var stream = File.OpenRead(path);
            theImage.BeginInit();
            theImage.CacheOption = BitmapCacheOption.OnLoad;
            theImage.StreamSource = stream;
            theImage.EndInit();
            stream.Close();
            stream.Dispose();

            ImageBrush myImageBrush = new ImageBrush(theImage);

            for (int i = -Range; i <= Range * 3; ++i)
            {
                myGrid.Children.Add(new Canvas
                {
                    Width = theImage.Width,
                    Height = theImage.Height,
                    Background = myImageBrush,
                    Opacity = Alpha,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(i * Square * daysCount - Delta % daysCount * Square + Mid, 0, 0, 0)
                });
            }
        }
    }
}
