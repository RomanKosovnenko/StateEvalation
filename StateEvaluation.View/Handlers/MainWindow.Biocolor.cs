using StateEvaluation.BioColor.Helpers;
using StateEvaluation.BioColor.Providers;
using StateEvaluation.BusinessLayer.BuissnesManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StateEvaluation
{
    /// <summary>
    /// Interaction logic for Biocolor tab
    /// </summary>
    partial class MainWindow : Window
    {
        private const int STEP = 7;
        public static Dictionary<string, string> userIdBirthPairs;
        private BiocolorSettings biocolorSettings;
        private BiocolorBusinessManager biocolorBusinessManager;
        private BiocolorProvider biocolorProvider;
        private ImageGenerator imageGenerator;
        private List<BiocolorProvider.ColorRow> colors;

        private void Prew_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.MakeStep(-STEP);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.MakeStep(+STEP);
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.GenerateImages();
        }

        private void DrawGraphs_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.DrawGraphs();
        }

        private void WindowRendered_Click(object sender, EventArgs e)
        {
            userIdBirthPairs = peopleBuissnesManager.GetUserIdBirthPairs();
            RestoreColors_Click(sender, null);
        }

        private void CMYK2RGB_Click(object sender, RoutedEventArgs e)
        {
            var color = colors.Find(c => c.Converter == sender);

            byte.TryParse(color.C.Text, out byte C);
            byte.TryParse(color.M.Text, out byte M);
            byte.TryParse(color.Y.Text, out byte Y);
            byte.TryParse(color.K.Text, out byte K);

            byte[] RGB = BioColor.Helpers.ColorConverter.CmykToRgb(C, M, Y, K);
            string rgb = String.Format("{0:X2}{1:X2}{2:X2}", RGB[0], RGB[1], RGB[2]);

            color.RGB.Text = rgb;
            color.Background.Brush = (Brush) new BrushConverter().ConvertFromString("#" + rgb);
        }

        private void RestoreColors_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.RestoreColors(colors);
            SetCmykValues(colors);
        }

        private void ResetColors_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Do you want to RESET colors BiocolorSettings?", "Reset BiocolorSettings", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                biocolorBusinessManager.ResetColors(colors);
                SetCmykValues(colors);
            }
            InitializeComponent();
        }

        private void SetCmykValues(List<BiocolorProvider.ColorRow> colors)
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

        private void SaveColors_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.SaveColors(colors);
            RestoreColors_Click(sender, e);
        }
    }
}
