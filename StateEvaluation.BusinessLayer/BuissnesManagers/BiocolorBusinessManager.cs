using StateEvaluation.BioColor.Providers;
using StateEvaluation.BioColor.Helpers;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StateEvaluation.BusinessLayer.BuissnesManagers
{
    public class BiocolorBusinessManager
    {
        BiocolorSettings biocolorSettings;
        ImageGenerator imageGenerator;
        BiocolorProvider biocolorProvider;

        private const int RangeRed = 23;
        private const int RangeGreen = 28;
        private const int RangeBlue = 33;

        public BiocolorBusinessManager(Grid biocolorGrid, DatePicker birthday, DatePicker now, BiocolorSettings biocolorSettings)
        {
            this.biocolorSettings = biocolorSettings;
            biocolorProvider = new BiocolorProvider(biocolorSettings);
            imageGenerator = new ImageGenerator(biocolorSettings);

            biocolorProvider.InitBiocolor(biocolorGrid, birthday, now);
        }
        public void ResetColors(TextBox[] colors)
        {
            biocolorSettings.Reset();
            SetDefaultColors(colors);
            SaveColors();
        }

        public void RestoreColors(TextBox[] colors)
        {
            SetDefaultColors(colors);
        }

        public void SaveColors(TextBox[] colors)
        {
            if (colors.ToList().Any(x => !imageGenerator.HEX.IsMatch(x.Text.ToUpper())))
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
            imageGenerator.GenerateImages(RangeRed);
            imageGenerator.GenerateImages(RangeGreen);
            imageGenerator.GenerateImages(RangeBlue);
        }

        public void MakeStep(int step)
        {
            try
            {
                biocolorProvider.MakeStep(step);
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
                biocolorProvider.DrawGraphs();
            }
            catch (System.IO.FileNotFoundException)
            {
                GenerateImages();
            }
        }

        private void SaveColors()
        {
            biocolorSettings.Save();
        }

        private void SetDefaultColors(TextBox[] colors)
        {
            colors[0].Text = biocolorSettings.I1;
            colors[1].Text = biocolorSettings.I2;
            colors[2].Text = biocolorSettings.I3;
            colors[3].Text = biocolorSettings.I4;
            colors[4].Text = biocolorSettings.E1;
            colors[5].Text = biocolorSettings.E2;
            colors[6].Text = biocolorSettings.E3;
            colors[7].Text = biocolorSettings.E4;
            colors[8].Text = biocolorSettings.P1;
            colors[9].Text = biocolorSettings.P2;
            colors[10].Text = biocolorSettings.P3;
            colors[11].Text = biocolorSettings.P4;
        }

        private void SetNewColors(TextBox[] colors)
        {
            biocolorSettings.I1 = colors[0].Text;
            biocolorSettings.I2 = colors[1].Text;
            biocolorSettings.I3 = colors[2].Text;
            biocolorSettings.I4 = colors[3].Text;
            biocolorSettings.E1 = colors[4].Text;
            biocolorSettings.E2 = colors[5].Text;
            biocolorSettings.E3 = colors[6].Text;
            biocolorSettings.E4 = colors[7].Text;
            biocolorSettings.P1 = colors[8].Text;
            biocolorSettings.P2 = colors[9].Text;
            biocolorSettings.P3 = colors[10].Text;
            biocolorSettings.P4 = colors[11].Text;
        }
    }
}
