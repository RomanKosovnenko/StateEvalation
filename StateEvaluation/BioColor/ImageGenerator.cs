using System.Drawing;

namespace StateEvaluation.BioColor
{
    internal static class ImageGenerator
    {
        private static readonly int SquareSize = Settings.Default.square;
        private static Graphics _graphics;
        private static string[] _colors;
        /*
new String[] {
"#FF2317F5",
"#FF0F2BEB",
"#FF1855F7",
"#FF3DDEFB",
"#FF31F715",
"#FF7EFE07",
"#FFF4FE09",
"#FFFBBC0C",
"#FFF96F0A",
"#FFFE280C",
"#FFFA0006",
"#FFF603EB"
};
        */
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
                case 23: startIndex = 8; break;
                case 28: startIndex = 4; break;
                case 33: startIndex = 0; break;

            }

            DrawTriangle(topPoint, startIndex);
            DrawTriangle(bottomPoint, startIndex, true);

            Color alpha = Color.FromArgb(0, 255, 255, 255);

            Bitmap baseImage = new Bitmap("D:/base_" + width + ".png");
            for (int i = 0; i < image.Width; ++i)
                for (int j = 0; j < image.Height; ++j)
                {
                    if (baseImage.GetPixel(i, j).A == 0) image.SetPixel(i, j, alpha);
                }
            image.Save("D:/Image_" + width + ".png");

        }
    }
}
