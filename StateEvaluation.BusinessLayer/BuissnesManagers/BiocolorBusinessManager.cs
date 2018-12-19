using StateEvaluation.BioColor.Providers;
using StateEvaluation.BioColor.Helpers;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;

namespace StateEvaluation.BusinessLayer.BuissnesManagers
{
    public class BiocolorBusinessManager
    {
        private BiocolorSettings _biocolorSettings;
        private ImageGenerator _imageGenerator;
        private BiocolorProvider _biocolorProvider;

        private const int RangeRed = 23;
        private const int RangeGreen = 28;
        private const int RangeBlue = 33;

        public BiocolorBusinessManager(Grid biocolorGrid, DatePicker birthday, DatePicker now, BiocolorSettings biocolorSettings)
        {
            this._biocolorSettings = biocolorSettings;
            _biocolorProvider = new BiocolorProvider(biocolorSettings);
            _imageGenerator = new ImageGenerator(biocolorSettings);

            _biocolorProvider.InitBiocolor(biocolorGrid, birthday, now);
        }

        public void GenerateRgbFromCmyk(BiocolorProvider.ColorRow color)
        {
            byte.TryParse(color.C.Text, out byte C);
            byte.TryParse(color.M.Text, out byte M);
            byte.TryParse(color.Y.Text, out byte Y);
            byte.TryParse(color.K.Text, out byte K);

            byte[] RGB = BioColor.Helpers.ColorConverter.CmykToRgb(C, M, Y, K);
            string rgb = string.Format("{0:X2}{1:X2}{2:X2}", RGB[0], RGB[1], RGB[2]);

            color.RGB.Text = rgb;
            color.Background.Brush = (Brush) new BrushConverter().ConvertFromString("#" + rgb);
        }

        public void SetCmykValues(List<BiocolorProvider.ColorRow> colors)
        {
            foreach (BiocolorProvider.ColorRow color in colors)
            {
                byte[] CMYK = BioColor.Helpers.ColorConverter.RgbToCmyk(color.RGB.Text);
                color.C.Text = CMYK[0].ToString();
                color.M.Text = CMYK[1].ToString();
                color.Y.Text = CMYK[2].ToString();
                color.K.Text = CMYK[3].ToString();
                color.Background.Brush = (Brush)new BrushConverter().ConvertFromString("#" + color.RGB.Text);
            }
        }

        public void ResetColors(List<BiocolorProvider.ColorRow> colors)
        {
            _biocolorSettings.Reset();
            SetDefaultColors(colors);
            SaveColors();
            GenerateImages();
        }

        public void RestoreColors(List<BiocolorProvider.ColorRow> colors)
        {
            SetDefaultColors(colors);
        }

        public void SaveColors(List<BiocolorProvider.ColorRow> colors)
        {
            SetNewColors(colors);
            SaveColors();
            GenerateImages();
        }

        /// <summary>
        /// Generate Emotional, Intellectual, Physical images if files not found
        /// </summary>
        public void GenerateImages()
        {
            _imageGenerator.GenerateImages(RangeRed);
            _imageGenerator.GenerateImages(RangeGreen);
            _imageGenerator.GenerateImages(RangeBlue);
        }

        public void MakeStep(int step)
        {
            try
            {
                _biocolorProvider.MakeStep(step);
            }
            catch (System.IO.FileNotFoundException)
            {
                GenerateImages();
            }
        }

        public void DrawGraphs()
        {
            try
            {
                _biocolorProvider.DrawGraphs();
            }
            catch (System.IO.FileNotFoundException)
            {
                GenerateImages();
            }
        }

        private void SaveColors()
        {
            _biocolorSettings.Save();
        }

        private void SetDefaultColors(List<BiocolorProvider.ColorRow> colors)
        {
            colors[ 0].RGB.Text = _biocolorSettings.P1;
            colors[ 1].RGB.Text = _biocolorSettings.P2;
            colors[ 2].RGB.Text = _biocolorSettings.P3;
            colors[ 3].RGB.Text = _biocolorSettings.P4;
            colors[ 4].RGB.Text = _biocolorSettings.E1;
            colors[ 5].RGB.Text = _biocolorSettings.E2;
            colors[ 6].RGB.Text = _biocolorSettings.E3;
            colors[ 7].RGB.Text = _biocolorSettings.E4;
            colors[ 8].RGB.Text = _biocolorSettings.I1;
            colors[ 9].RGB.Text = _biocolorSettings.I2;
            colors[10].RGB.Text = _biocolorSettings.I3;
            colors[11].RGB.Text = _biocolorSettings.I4;
        }

        private void SetNewColors(List<BiocolorProvider.ColorRow> colors)
        {
            _biocolorSettings.P1 = colors[ 0].RGB.Text;
            _biocolorSettings.P2 = colors[ 1].RGB.Text;
            _biocolorSettings.P3 = colors[ 2].RGB.Text;
            _biocolorSettings.P4 = colors[ 3].RGB.Text;
            _biocolorSettings.E1 = colors[ 4].RGB.Text;
            _biocolorSettings.E2 = colors[ 5].RGB.Text;
            _biocolorSettings.E3 = colors[ 6].RGB.Text;
            _biocolorSettings.E4 = colors[ 7].RGB.Text;
            _biocolorSettings.I1 = colors[ 8].RGB.Text;
            _biocolorSettings.I2 = colors[ 9].RGB.Text;
            _biocolorSettings.I3 = colors[10].RGB.Text;
            _biocolorSettings.I4 = colors[11].RGB.Text;
        }
    }
}
