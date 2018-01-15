using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace StateEvaluation.BioColor
{
    internal static class ImageGenerator
    {
        public static Regex HEX = new Regex(@"([0-9A-F]{2})([0-9A-F]{2})([0-9A-F]{2})");
        private static readonly int SquareSize = Settings.Default.square;
        private static Graphics _graphics;
        private static string[] _colors;
        private const float MAX = 255;
        public static int[] RgbToCmyk(string RGB)
        {
            var match = HEX.Match(RGB.ToUpper());
            try
            {
                int R = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
                int G = int.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
                int B = int.Parse(match.Groups[3].Value, NumberStyles.HexNumber);
                return RgbToCmyk(R, G, B);
            }
            catch (System.FormatException) {
                return new int[] { 0, 0, 0, 0};
            }
        }
        public static int[] RgbToCmyk(Color color)
        {
            return RgbToCmyk(color.R, color.G, color.B);
        }
        public static int[] RgbToCmyk(int R, int G, int B)
        {
            int C, M, Y, K;
            if (R == 0 && G == 0 && B == 0)
            {
                C = M = Y = 0;
                K = 1;
            }
            else
            {
                K = (int)(MAX - System.Math.Max(R, System.Math.Max(G, B)));
                // K = 0;
                C = (int)(MAX * (MAX - R - K) / (MAX - K));
                M = (int)(MAX * (MAX - G - K) / (MAX - K));
                Y = (int)(MAX * (MAX - B - K) / (MAX - K));
            }
            return new int[] { C, M, Y, K };
        }
        public static int[] CmykToRgb(int C, int M, int Y, int K)
        {
            int R = (int)((MAX - C) * (MAX - K) / MAX);
            int G = (int)((MAX - M) * (MAX - K) / MAX);
            int B = (int)((MAX - Y) * (MAX - K) / MAX);
            return new int[] { R, G, B };
        }
        private static void DrawSquare(Point upperLeft, string color, bool reverse = false)
        {
            _graphics.FillRectangle(
                new SolidBrush(ColorTranslator.FromHtml(color)),
                new Rectangle
                {
                    Y = upperLeft.Y - (reverse ? SquareSize : 0),
                    X = upperLeft.X,
                    Width = SquareSize,
                    Height = SquareSize
                }
            );
        }
        private static void DrawColumn(Point point, int startIndex, int count, bool reverse = false)
        {
            for (int i = -2; i < 12 && i < count; ++i)
            {
                DrawSquare(new Point
                {
                    X = point.X,
                    Y = reverse ? point.Y - i * SquareSize : point.Y + i * SquareSize
                }, _colors[(startIndex + i + _colors.Length) % _colors.Length], reverse);
            }
        }
        private static void DrawTriangle(Point point, int startIndex, bool reverse = false)
        {
            for (int j = 0; j < 12; ++j)
            {
                DrawColumn(new Point
                {
                    X = point.X + j * SquareSize,
                    Y = reverse ? point.Y - j * SquareSize : point.Y + j * SquareSize
                }, startIndex, 12 - j, reverse);
                if (j == 0) continue;
                DrawColumn(new Point
                {
                    X = point.X - j * SquareSize,
                    Y = reverse ? point.Y - j * SquareSize : point.Y + j * SquareSize
                }, startIndex, 12 - j, reverse);
            }
        }
        private static void RestoreColors() {
            _colors = new[] {
                "#" + Settings.Default.i1,
                "#" + Settings.Default.i2,
                "#" + Settings.Default.i3,
                "#" + Settings.Default.i4,
                "#" + Settings.Default.e1,
                "#" + Settings.Default.e2,
                "#" + Settings.Default.e3,
                "#" + Settings.Default.e4,
                "#" + Settings.Default.p1,
                "#" + Settings.Default.p2,
                "#" + Settings.Default.p3,
                "#" + Settings.Default.p4
            };
        }
        public static void Generate(int width)
        {
            RestoreColors();
            Bitmap image = new Bitmap(width * SquareSize, 24 * SquareSize);
			_graphics = Graphics.FromImage(image);


            Point topPoint = new Point((int)((width / 4.0 * 3 - 0.5) * SquareSize), 0);
            Point bottomPoint = new Point((int)((width / 4.0 * 1 - 0.5) * SquareSize), image.Height);
            int startIndex = 0;

            switch (width)
            {
                case 23: startIndex = 8 +2 ; break;
                case 28: startIndex = 4 +2 ; break;
                case 33: startIndex = 0 +2 ; break;

            }

            DrawTriangle(topPoint, startIndex);
            DrawTriangle(bottomPoint, startIndex, true);

            Color alpha = Color.FromArgb(0, 255, 255, 255);

            string s = System.IO.Directory.GetCurrentDirectory() + "/../../BioColor/template/base_" + width + ".png";
            Bitmap baseImage = new Bitmap(s);
            for (int i = 0; i < image.Width; ++i)
                for (int j = 0; j < image.Height; ++j)
                {
                    if (baseImage.GetPixel(i, j).A == 0) image.SetPixel(i, j, alpha);
                }
            image.Save(System.IO.Directory.GetCurrentDirectory() + "/../../BioColor/template/Image_" + width + ".png");

        }
    }
}
