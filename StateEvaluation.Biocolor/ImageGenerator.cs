using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace StateEvaluation.BioColor
{
    public class ImageGenerator
    {
        public Regex HEX;
        private readonly int SquareSize;
        private Graphics _graphics;
        private string[] _colors;
        public float MAX;
        public int MAX_HEX;

        public ImageGenerator()
        {
            HEX = new Regex(@"^([0-9A-F]{2})([0-9A-F]{2})([0-9A-F]{2})$");
            SquareSize = Settings.Default.square;
            MAX = 0xFF;
            MAX_HEX = 0xFF;
        }
        /// <summary>
        /// Draw square on template
        /// </summary>
        /// <param name="point">top left coordinate</param>
        /// <param name="color">Color of square</param>
        /// <param name="reverse">Top/Bottom square</param>
        private void DrawSquare(Point point, string color, bool reverse = false)
        {
            _graphics.FillRectangle(
                new SolidBrush(ColorTranslator.FromHtml(color)),
                new Rectangle
                {
                    Y = point.Y - (reverse ? SquareSize : 0),
                    X = point.X,
                    Width = SquareSize,
                    Height = SquareSize
                }
            );
        }
        /// <summary>
        /// Draw column of squares on template
        /// </summary>
        /// <param name="point">Top left coordinate of column</param>
        /// <param name="startIndex">Index of color of first square in column</param>
        /// <param name="count">Count of squares in column</param>
        /// <param name="reverse">Top/Bottom column</param>
        private void DrawColumn(Point point, int startIndex, int count, bool reverse = false)
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
        /// <summary>
        /// Draw triangle from columns on template
        /// </summary>
        /// <param name="point">Top left coordinate of top square</param>
        /// <param name="startIndex">Index of color of top square in triangle</param>
        /// <param name="reverse">Top/Bottom triangle</param>
        private void DrawTriangle(Point point, int startIndex, bool reverse = false)
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
        /// <summary>
        /// Load Colors from User settings
        /// </summary>
        private void LoadColorsFromSettings() {
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
        /// <summary>
        /// Generates Images for 23, 28, 33 days from templates
        /// </summary>
        /// <param name="width">Count of days</param>
        public void GenerateImages(int width)
        {
            LoadColorsFromSettings();
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

            string s = System.IO.Directory.GetCurrentDirectory() + "/../../BioColor/Assets/Template/base_" + width + ".png";
            Bitmap baseImage = new Bitmap(s);
            for (int i = 0; i < image.Width; ++i)
                for (int j = 0; j < image.Height; ++j)
                {
                    if (baseImage.GetPixel(i, j).A == 0) image.SetPixel(i, j, alpha);
                }
            image.Save(System.IO.Directory.GetCurrentDirectory() + "/../../BioColor/Assets/Template/Image_" + width + ".png");

        }
    }
}
