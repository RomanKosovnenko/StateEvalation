using System;
using System.Drawing;

namespace StateEvaluation.BioColor
{
    class ImageGenerator
    {
        private static int squareSize = Settings.Default.square;
        private static Graphics graphics;
        private static String[] colors = new String[] {
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
        private static void DrawSquare(Point upperLeft, String color, bool reverse = false)
        {
            graphics.FillRectangle(
                new SolidBrush(ColorTranslator.FromHtml(color)),
                new Rectangle
                {
                    Y = upperLeft.Y - (reverse ? squareSize : 0),
                    X = upperLeft.X,
                    Width = squareSize,
                    Height = squareSize
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
                    Y = reverse ? point.Y - i * squareSize : point.Y + i * squareSize
                }, colors[(startIndex + i + colors.Length) % colors.Length], reverse);
            }
        }
        private static void DrawTriangle(Point point, int startIndex, bool reverse = false)
        {
            for (int j = 0; j < 12; ++j)
            {
                DrawColumn(new Point
                {
                    X = point.X + j * squareSize,
                    Y = reverse ? point.Y - j * squareSize : point.Y + j * squareSize
                }, startIndex, 12 - j, reverse);
                if (j == 0) continue;
                DrawColumn(new Point
                {
                    X = point.X - j * squareSize,
                    Y = reverse ? point.Y - j * squareSize : point.Y + j * squareSize
                }, startIndex, 12 - j, reverse);
            }
        }
        public static void Generate(int width)
        {
            Bitmap image = new Bitmap(width * squareSize, 24 * squareSize);
            graphics = Graphics.FromImage(image);


            Point topPoint = new Point((int)((width / 4.0 * 3 - 0.5) * squareSize), 0);
            Point bottomPoint = new Point((int)((width / 4.0 * 1 - 0.5) * squareSize), image.Height);
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
