using StateEvaluation.BioColor.Providers;
using StateEvaluation.BioColor.Helpers;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        public void ResetColors(TextBox[] colors)
        {
            _biocolorSettings.Reset();
            SetDefaultColors(colors);
            SaveColors();
        }

        public void RestoreColors(TextBox[] colors)
        {
            SetDefaultColors(colors);
        }

        public void SaveColors(TextBox[] colors)
        {
            if (colors.ToList().Any(x => !_imageGenerator.HEX.IsMatch(x.Text.ToUpper())))
            {
                MessageBox.Show("Invalid color!");
                return;
            }

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

        private void SetDefaultColors(TextBox[] colors)
        {
            colors[0].Text = _biocolorSettings.I1;
            colors[1].Text = _biocolorSettings.I2;
            colors[2].Text = _biocolorSettings.I3;
            colors[3].Text = _biocolorSettings.I4;
            colors[4].Text = _biocolorSettings.E1;
            colors[5].Text = _biocolorSettings.E2;
            colors[6].Text = _biocolorSettings.E3;
            colors[7].Text = _biocolorSettings.E4;
            colors[8].Text = _biocolorSettings.P1;
            colors[9].Text = _biocolorSettings.P2;
            colors[10].Text = _biocolorSettings.P3;
            colors[11].Text = _biocolorSettings.P4;
        }

        private void SetNewColors(TextBox[] colors)
        {
            _biocolorSettings.I1 = colors[0].Text;
            _biocolorSettings.I2 = colors[1].Text;
            _biocolorSettings.I3 = colors[2].Text;
            _biocolorSettings.I4 = colors[3].Text;
            _biocolorSettings.E1 = colors[4].Text;
            _biocolorSettings.E2 = colors[5].Text;
            _biocolorSettings.E3 = colors[6].Text;
            _biocolorSettings.E4 = colors[7].Text;
            _biocolorSettings.P1 = colors[8].Text;
            _biocolorSettings.P2 = colors[9].Text;
            _biocolorSettings.P3 = colors[10].Text;
            _biocolorSettings.P4 = colors[11].Text;
        }
    }
}
