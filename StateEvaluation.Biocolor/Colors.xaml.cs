using StateEvaluation.Common;
using System.Windows;

namespace StateEvaluation.BioColor
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for Colors.xaml
    /// </summary>
    public partial class Colors
    {

        public Colors()
        {
            InitializeComponent();
            ic1.Text = Settings.Default.i1;
            ic2.Text = Settings.Default.i2;
            ic3.Text = Settings.Default.i3;
            ic4.Text = Settings.Default.i4;
            ec1.Text = Settings.Default.e1;
            ec2.Text = Settings.Default.e2;
            ec3.Text = Settings.Default.e3;
            ec4.Text = Settings.Default.e4;
            pc1.Text = Settings.Default.p1;
            pc2.Text = Settings.Default.p2;
            pc3.Text = Settings.Default.p3;
            pc4.Text = Settings.Default.p4;
        }

        internal void Save()
        {
            Settings.Default.i1 = ic1.Text;
            Settings.Default.i2 = ic2.Text;
            Settings.Default.i3 = ic3.Text;
            Settings.Default.i4 = ic4.Text;
            Settings.Default.e1 = ec1.Text;
            Settings.Default.e2 = ec2.Text;
            Settings.Default.e3 = ec3.Text;
            Settings.Default.e4 = ec4.Text;
            Settings.Default.p1 = pc1.Text;
            Settings.Default.p2 = pc2.Text;
            Settings.Default.p3 = pc3.Text;
            Settings.Default.p4 = pc4.Text;
        }
    }
}