using System;
using System.Windows;
using System.Windows.Input;
using WpfAppPS.Properties;
using WpfAppPS;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace BioColor
{
    public partial class MainWindow : Window
    {
        int    mid     = Settings.Default.mid    , 
               square  = Settings.Default.square , 
               topline = Settings.Default.topline, 
               step    = Settings.Default.step   ;
        double alpha   = Settings.Default.alpha  ;

        int int_red   = Settings.Default.Int_Red  , 
            int_green = Settings.Default.Int_Green, 
            int_blue  = Settings.Default.Int_Blue ;

        string path_red, path_green, path_blue;
        string initdate;
        int range;
        int a = 0;
        public MainWindow()
        { 
            
            InitializeComponent();
            Restore();
        }
        private void GO()
        {
            Save();
            try
            {
                DateTime dt = Convert.ToDateTime(Date.Text);
                DateTime nw = Convert.ToDateTime(Date_Now.Text);
                int Delta = (int)((nw.Subtract(dt)).TotalDays);

                int width = 1000 + mid;
                Function.DrawClear(myGrid);
                System.Drawing.Bitmap r = new System.Drawing.Bitmap(path_red);
                System.Drawing.Bitmap g = new System.Drawing.Bitmap(path_green);
                System.Drawing.Bitmap b = new System.Drawing.Bitmap(path_blue);
                System.Drawing.Bitmap w = new System.Drawing.Bitmap(width, 480);

                for (int x = 0; x < width; ++x) {
                    for (int y = 0; y < 480; ++y) {
                        w.SetPixel(x, y, 
                            Function.ColorMix(
                                r.GetPixel((x+Delta*square-mid) % r.Width, y), 
                                g.GetPixel((x+Delta*square-mid) % g.Width, y), 
                                b.GetPixel((x+Delta*square-mid) % b.Width, y)
                            )
                        );
                    }
                }
                w.Save("D:\\result"+(a)+".png", System.Drawing.Imaging.ImageFormat.Png);
                //    Function.doMix(new String[] { path_red, path_green, path_blue});
                if (!true) {
                    Function.Add(path_red, int_red, Delta, myGrid);
                    Function.Add(path_green, int_green, Delta, myGrid);
                    Function.Add(path_blue, int_blue, Delta, myGrid);
                } else {
                    BitmapImage theImage = new BitmapImage
                        (new Uri("D:\\result"+(a++) +".png"));
                    ImageBrush myImageBrush = new ImageBrush(theImage);
                    Canvas myCanvas = new Canvas();
                    myCanvas.Width = theImage.Width;
                    myCanvas.Height = theImage.Height;
                    myCanvas.Background = myImageBrush;
                    myCanvas.Opacity = alpha;
                    myCanvas.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    myGrid.Children.Add(myCanvas);
                    
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect Date!");
            }

        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                GO();
            }
        }
        private void Prew(object sender, RoutedEventArgs e)
        { 
            DateTime nw = Convert.ToDateTime(Date_Now.Text).AddDays(-step);
            Date_Now.Text = nw.ToString();
            GO();
        }
        private void Next(object sender, RoutedEventArgs e)
        {
            DateTime nw = Convert.ToDateTime(Date_Now.Text).AddDays(step);
            Date_Now.Text = nw.ToString();
            GO();
        }
        private void Menu(object sender, RoutedEventArgs e)
        {
            Settings_Edit s = new Settings_Edit();
            s.ShowDialog();
            s.Get();
            Restore();
            GO();
        }
        public void Save()
        {
            initdate = Date.Text;
            Settings.Default.Range      = range     ;
            Settings.Default.Path_Red   = path_red  ;
            Settings.Default.Path_Green = path_green;
            Settings.Default.Path_Blue  = path_blue ;
            Settings.Default.Init_date  = initdate  ;
            Settings.Default.Save();
        }
        public void Restore()
        {
            range      = Settings.Default.Range     ;
            path_red   = Settings.Default.Path_Red  ;
            path_green = Settings.Default.Path_Green;
            path_blue  = Settings.Default.Path_Blue ;
            initdate   = Settings.Default.Init_date;
            Date.Text  = initdate;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GO();
        }
    }
}
