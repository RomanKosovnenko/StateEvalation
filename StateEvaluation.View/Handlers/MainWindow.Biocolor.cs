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
        private TextBox[] colors;
        private BiocolorBusinessManager biocolorBusinessManager;
        private BiocolorProvider biocolorProvider;
        private ImageGenerator imageGenerator;

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
            string name = (sender as Button).Name;
            string id = name.Substring(name.Length - 1);
            string factor = name.Substring(0, 1);

            int.TryParse((FindName("c" + factor + "c" + id) as TextBox).Text.ToString(), out int C);
            int.TryParse((FindName("m" + factor + "c" + id) as TextBox).Text.ToString(), out int M);
            int.TryParse((FindName("y" + factor + "c" + id) as TextBox).Text.ToString(), out int Y);
            int.TryParse((FindName("k" + factor + "c" + id) as TextBox).Text.ToString(), out int K);

            if (new List<int> { C, M, Y, K }.Any(x => x < 0 || x > BioColor.Helpers.ColorConverter.MAX))
            {
                MessageBox.Show("Invalid color");
                return;
            }

            int[] RGB = BioColor.Helpers.ColorConverter.CmykToRgb(C, M, Y, K);
            string rgb = String.Format("{0:X2}{1:X2}{2:X2}", RGB[0], RGB[1], RGB[2]);
            (FindName(factor + "c" + id) as TextBox).Text = rgb;
            if (imageGenerator.HEX.IsMatch(rgb))
            {
                (FindName(factor + "cb" + id) as GeometryDrawing).Brush = (Brush)new BrushConverter().ConvertFromString("#" + rgb);
            }
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

        private void SetCmykValues(TextBox[] colors)
        {
            foreach (var element in colors)
            {
                var name = element.Name;
                var factor = name.Substring(0, 1);
                var id = name.Substring(name.Length - 1);

                int[] CMYK = BioColor.Helpers.ColorConverter.RgbToCmyk(element.Text);

                (FindName("c" + factor + "c" + id) as TextBox).Text = CMYK[0].ToString();
                (FindName("m" + factor + "c" + id) as TextBox).Text = CMYK[1].ToString();
                (FindName("y" + factor + "c" + id) as TextBox).Text = CMYK[2].ToString();
                (FindName("k" + factor + "c" + id) as TextBox).Text = CMYK[3].ToString();

                (FindName(factor + "cb" + id) as GeometryDrawing).Brush = (Brush)new BrushConverter().ConvertFromString("#" + element.Text);
            }
        }

        private void SaveColors_Click(object sender, RoutedEventArgs e)
        {
            biocolorBusinessManager.SaveColors(colors);
            RestoreColors_Click(sender, e);
        }
    }
}
