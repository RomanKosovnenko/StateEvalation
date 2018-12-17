using System.Drawing;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using StateEvaluation.Common.Constants;

namespace StateEvaluation.BioColor.Helpers
{
    public class ImageGenerator
    {
        private Graphics _graphics;
        private string[] _colors;
        private Helpers.BiocolorSettings biocolorSettings;
        public float MAX;
        public int MAX_HEX;
        public Regex HEX;

        public ImageGenerator(Helpers.BiocolorSettings biocolorSettings)
        {
            this.biocolorSettings = biocolorSettings;
            HEX = new Regex(@"^([0-9A-F]{2})([0-9A-F]{2})([0-9A-F]{2})$");
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
                    Y = point.Y - (reverse ? biocolorSettings.Square : 0),
                    X = point.X,
                    Width = biocolorSettings.Square,
                    Height = biocolorSettings.Square
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
                    Y = reverse ? point.Y - i *  biocolorSettings.Square : point.Y + i *  biocolorSettings.Square
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
                    X = point.X + j *  biocolorSettings.Square,
                    Y = reverse ? point.Y - j *  biocolorSettings.Square : point.Y + j *  biocolorSettings.Square
                }, startIndex, 12 - j, reverse);
                if (j == 0) continue;
                DrawColumn(new Point
                {
                    X = point.X - j *  biocolorSettings.Square,
                    Y = reverse ? point.Y - j *  biocolorSettings.Square : point.Y + j *  biocolorSettings.Square
                }, startIndex, 12 - j, reverse);
            }
        }

        /// <summary>
        /// Load Colors from User BiocolorSettings
        /// </summary>
        private void LoadColorsFromBiocolorSettings() {
            _colors = new[] {
                "#" + biocolorSettings.I1,
                "#" + biocolorSettings.I2,
                "#" + biocolorSettings.I3,
                "#" + biocolorSettings.I4,
                "#" + biocolorSettings.E1,
                "#" + biocolorSettings.E2,
                "#" + biocolorSettings.E3,
                "#" + biocolorSettings.E4,
                "#" + biocolorSettings.P1,
                "#" + biocolorSettings.P2,
                "#" + biocolorSettings.P3,
                "#" + biocolorSettings.P4
            };
        }

        /// <summary>
        /// Generates Images for 23, 28, 33 days from templates
        /// </summary>
        /// <param name="width">Count of days</param>
        public void GenerateImages(int width)
        {
            LoadColorsFromBiocolorSettings();
            Bitmap image = new Bitmap(width *  biocolorSettings.Square, 24 *  biocolorSettings.Square);
			_graphics = Graphics.FromImage(image);


            Point topPoint = new Point((int)((width / 4.0 * 3 - 0.5) *  biocolorSettings.Square), 0);
            Point bottomPoint = new Point((int)((width / 4.0 * 1 - 0.5) *  biocolorSettings.Square), image.Height);
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

            string s = System.IO.Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["imagePath"] + ConfigurationManager.AppSettings["baseImagePath"] + width + ImageGeneratorConstants.ImageExtension;
            Bitmap baseImage = new Bitmap(s);
            for (int i = 0; i < image.Width; ++i)
                for (int j = 0; j < image.Height; ++j)
                {
                    if (baseImage.GetPixel(i, j).A == 0) image.SetPixel(i, j, alpha);
                }
            image.Save(System.IO.Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["imagePath"] + ConfigurationManager.AppSettings["generatedImagePath"] + width + ImageGeneratorConstants.ImageExtension);

        }
    }
}
