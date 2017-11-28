using System;
using System.Windows;
using System.Windows.Controls;

namespace StateEvaluation.BioColor
{
    class Main
    {   
        // BioColor 
        const int squareSize = 20, STEP = 7;
        static int mid = Settings.Default.mid,
               square = Settings.Default.square,
               topline = Settings.Default.topline,
               step = Settings.Default.step;
        static double alpha = Settings.Default.alpha;

        static int int_red = Settings.Default.Int_Red,
            int_green = Settings.Default.Int_Green,
            int_blue = Settings.Default.Int_Blue;

        static string path_red, path_green, path_blue;
        static string initdate;

        public static void Save()
        {
            Settings.Default.Path_Red = path_red;
            Settings.Default.Path_Green = path_green;
            Settings.Default.Path_Blue = path_blue;
            Settings.Default.Init_date = initdate;
            Settings.Default.Save();
        }
        public static void Restore()
        {
            path_red = Settings.Default.Path_Red;
            path_green = Settings.Default.Path_Green;
            path_blue = Settings.Default.Path_Blue;
            initdate = Settings.Default.Init_date;
        }
        public static void MakeStep(DatePicker DateNow, int step)
        {
            DateNow.Text = Convert.ToDateTime(DateNow.Text).AddDays(step).ToString();
            GO();
        }
        public static void DrawGraphs(object sender, RoutedEventArgs e)
        {
            GO();
        }
        public static void GO()
        {
            MessageBox.Show("GO");
        }
        public static void Menu()
        {
            SettingsEdit s = new SettingsEdit();
            s.ShowDialog();
            s.Get();
            Restore();
        }
        public static void Generate(object sender, RoutedEventArgs e)
        {
            ImageGenerator.Generate(23);
            ImageGenerator.Generate(28);
            ImageGenerator.Generate(33);
            MessageBox.Show("Generated!");
            // Application.Current.Shutdown(); 
            // myGrid.Children.Add();
        }
    }
}
