using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppPS.Properties;

namespace WpfAppPS
{
    /// <summary>
    /// Interaction logic for Settings_Edit.xaml
    /// </summary>
    public partial class Settings_Edit : Window
    {
        public Settings_Edit()
        {
            InitializeComponent();
            pr.Text = Settings.Default.Path_Red;
            pg.Text = Settings.Default.Path_Green;
            pb.Text = Settings.Default.Path_Blue;
            prange.Text = Settings.Default.Range.ToString();
        }
        private string FilePicker()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.InitialDirectory = @"D:\";
            dlg.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                return dlg.FileName;
            }
            else return "D:/Error.png";
        }

        private void Br_click(object sender, RoutedEventArgs e)
        {
            pr.Text = FilePicker();
        }
        private void Bg_click(object sender, RoutedEventArgs e)
        {
            pg.Text = FilePicker();
        }
        private void Bb_click(object sender, RoutedEventArgs e)
        {
            pb.Text = FilePicker();
        }
        public void Get() {
            Settings.Default.Path_Red   = pr.Text;
            Settings.Default.Path_Green = pg.Text;
            Settings.Default.Path_Blue  = pb.Text;

            try
            {
                Settings.Default.Range = Int32.Parse(prange.Text);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Incorrect Range!");
            }
            Settings.Default.Save();
        }
    }
}
