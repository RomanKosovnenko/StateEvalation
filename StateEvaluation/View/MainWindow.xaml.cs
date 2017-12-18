using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Excel;
using StateEvaluation.Model;
using StateEvaluation.View;
using StateEvaluation.ViewModel;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;
using Window = System.Windows.Window;

namespace StateEvaluation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PreferenceDB _preferenceDb = new PreferenceDB();
        public List<string> PersonCodes = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            PersonCodes = _preferenceDb.People.Select(s => s.UserId).ToList();
            comboBox.ItemsSource = PersonCodes;
            BioColor.Main.InitBioColor(BioColorGraph, Date, DateNow);
        }

        private void AddPersonBtn_Click(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrEmpty((PersonAddFormGrid.FindName("FirstnameTextbox") as TextBox).Text) ||
                string.IsNullOrEmpty((PersonAddFormGrid.FindName("LastnameTextbox") as TextBox).Text) ||
                !DateTime.TryParse((PersonAddFormGrid.FindName("BirthdayDatePicker") as DatePicker).Text, out DateTime obj1) ||
                string.IsNullOrEmpty((PersonAddFormGrid.FindName("ProfessionTextbox") as TextBox).Text) ||
                !int.TryParse((PersonAddFormGrid.FindName("ExpeditionTextbox") as TextBox).Text, out int obj2) ||
                !int.TryParse((PersonAddFormGrid.FindName("NumberTextbox") as TextBox).Text, out int obj3)
                )
            {
                MessageBox.Show("Error! Try edit fields in form!");
                return;
            }
            People person = new People()
            {
                Firstname = (PersonAddFormGrid.FindName("FirstnameTextbox") as TextBox).Text,
                Lastname = (PersonAddFormGrid.FindName("LastnameTextbox") as TextBox).Text,
                Expedition = int.Parse((PersonAddFormGrid.FindName("ExpeditionTextbox") as TextBox).Text),
                Number = int.Parse((PersonAddFormGrid.FindName("NumberTextbox") as TextBox).Text),
                Id = Guid.NewGuid(),
                Workposition = (PersonAddFormGrid.FindName("LastnameTextbox") as TextBox).Text,
                Birthday = (PersonAddFormGrid.FindName("BirthdayDatePicker") as DatePicker).Text
            };
            person.UserId = $"EX{person.Expedition}#{person.Number}";
            PreferenceDB _preferenceDb = new PreferenceDB();
            _preferenceDb.People.InsertOnSubmit(person);
            _preferenceDb.SubmitChanges();
            PersonDataGrid.ItemsSource = _preferenceDb.GetAllPeople();
            (PersonAddFormGrid.FindName("FirstnameTextbox") as TextBox).Text = string.Empty;
            (PersonAddFormGrid.FindName("LastnameTextbox") as TextBox).Text = string.Empty;
            (PersonAddFormGrid.FindName("BirthdayDatePicker") as DatePicker).Text = string.Empty;
            (PersonAddFormGrid.FindName("ProfessionTextbox") as TextBox).Text = string.Empty;
            (PersonAddFormGrid.FindName("ExpeditionTextbox") as TextBox).Text = string.Empty;
            (PersonAddFormGrid.FindName("NumberTextbox") as TextBox).Text = string.Empty;
        }

        private void RemovePersonBtn_Click(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            People person = _preferenceDb.People.Single(c => c == ((Button)(e.Source)).BindingGroup.Items[0]);
            _preferenceDb.People.DeleteOnSubmit(person);
            _preferenceDb.SubmitChanges();
            PersonDataGrid.ItemsSource = _preferenceDb.GetAllPeople();
        }

        private void RemovePreferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            Preference pref = _preferenceDb.Preference.Single(c => c == ((Button)(e.Source)).BindingGroup.Items[0]);
            _preferenceDb.Preference.DeleteOnSubmit(pref);
            _preferenceDb.SubmitChanges();
            TestsDataGrid.ItemsSource = _preferenceDb.GetAllTests();
        }

        private void RemoveFeelingsBtn_Click(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            SubjectiveFeeling feelings = _preferenceDb.SubjectiveFeeling.Single(c => c == ((Button)(e.Source)).BindingGroup.Items[0]);
            _preferenceDb.SubjectiveFeeling.DeleteOnSubmit(feelings);
            _preferenceDb.SubmitChanges();
            SubjectiveFeelDataGrid.ItemsSource = _preferenceDb.GetAllSubjecriveFeelings();
        }

        private void NumberTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (!int.TryParse(textbox.Text, out int text))
            {
                textbox.BorderBrush = Brushes.Red;
            }
            else
            {
                textbox.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFABADB3"));
            }
        }

        private void Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (string.IsNullOrEmpty(textbox.Text))
            {
                textbox.BorderBrush = Brushes.Red;
            }
            else
            {
                textbox.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFABADB3"));
            }
        }

        private void DatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            DatePicker textbox = (DatePicker)sender;
            if (string.IsNullOrEmpty(textbox.Text) || !DateTime.TryParse(textbox.Text, out DateTime date))
            {
                textbox.BorderBrush = Brushes.Red;
            }
            else
            {
                textbox.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFABADB3"));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            Preference pref = _preferenceDb.Preference.Single(c => c == ((Button)(e.Source)).BindingGroup.Items[0]);
            var subWindow = new IndividualChart(pref, _preferenceDb);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            var subWindow = new TestsChart(_preferenceDb.Preference.Select(x => x).OrderBy(x => x.Date).ToList());
        }

        private void AddData_OnClick(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();

            string filePath = System.IO.Directory.GetCurrentDirectory() + "\\" + "21.xlsx";


            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook wb = ExcelApp.Workbooks.Open(filePath, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            Microsoft.Office.Interop.Excel.Worksheet sh = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets["Кольорова преференція початок"];

            Range excelRange = sh.UsedRange;

            int i0 = 7, j0 = 45;

            Range xlRng = sh.UsedRange;
            Object arr = xlRng.Value;


            #region MyRegion

            List<List<Byte>> lessSequences1 = new List<List<byte>>();
            List<List<Byte>> Sequences1 = new List<List<byte>>();
            List<string> pref1 = new List<string>();
            List<List<Byte>> lessSequences2 = new List<List<byte>>();
            List<List<Byte>> Sequences2 = new List<List<byte>>();
            List<string> pref2 = new List<string>();
            List<string> number = new List<string>();
            List<string> dates = new List<string>();
            List<string> favcolor = new List<string>();
            List<string> tabl1 = new List<string>();
            List<string> tabl2 = new List<string>();
            List<string> fornames = new List<string>();
            for (int i = i0; i < i0 + 12; ++i)
            {
                fornames.Add(xlRng[i, 2].Value2);
            }
            PreferenceDB _preferenceDb = new PreferenceDB();
            for (int i = i0; i < xlRng.Rows.Count; ++i)
            {
                tabl1.Add(xlRng[i, 44].Value2.ToString());
                tabl2.Add(xlRng[i, 45].Value2.ToString());
                favcolor.Add(xlRng[i, 10].Value2.ToString());
                dates.Add(xlRng[i, 3].Text.ToString());
                number.Add(xlRng[i, 1].Value2.ToString());
                //dates.Add(((DateTime)xlRng[i, 3]).Date.ToShortDateString());
                lessSequences1.Add(new List<byte>() { (byte)xlRng[i, 12].Value2, (byte)xlRng[i, 13].Value2, (byte)xlRng[i, 14].Value2 });
                Sequences1.Add(new List<byte>() { (byte)xlRng[i, 16].Value2, (byte)xlRng[i, 17].Value2, (byte)xlRng[i, 18].Value2, (byte)xlRng[i, 19].Value2,
                    (byte)xlRng[i, 20].Value2, (byte)xlRng[i, 21].Value2, (byte)xlRng[i, 22].Value2, (byte)xlRng[i, 23].Value2,
                    (byte)xlRng[i, 24].Value2, (byte)xlRng[i, 25].Value2, (byte)xlRng[i, 26].Value2, (byte)xlRng[i, 27].Value2});
                if (xlRng[i, 28].Value2 == null)
                {
                    lessSequences2.Add(null);
                    Sequences2.Add(null);
                    continue;
                }
                lessSequences2.Add(new List<byte>() { (byte)xlRng[i, 28].Value2, (byte)xlRng[i, 29].Value2, (byte)xlRng[i, 30].Value2 });
                Sequences2.Add(new List<byte>() { (byte)xlRng[i, 32].Value2, (byte)xlRng[i, 33].Value2, (byte)xlRng[i, 34].Value2, (byte)xlRng[i, 35].Value2,
                    (byte)xlRng[i, 36].Value2, (byte)xlRng[i, 37].Value2, (byte)xlRng[i, 38].Value2, (byte)xlRng[i, 39].Value2,
                    (byte)xlRng[i, 40].Value2, (byte)xlRng[i, 41].Value2, (byte)xlRng[i, 42].Value2, (byte)xlRng[i, 43].Value2});
            }
            List<StateEvaluationDLL.DataStructures.Preference> prefRes1 = new List<StateEvaluationDLL.DataStructures.Preference>();
            List<StateEvaluationDLL.DataStructures.Preference> prefRes2 = new List<StateEvaluationDLL.DataStructures.Preference>();
            for (var i = 0; i < Sequences1.Count; i++)
            {
                prefRes1.Add(new StateEvaluationDLL.DataStructures.Preference(lessSequences1[i], Sequences1[i]));
            }
            for (var i = 0; i < Sequences2.Count; i++)
            {
                if (Sequences2[i] == null)
                {
                    prefRes2.Add(null);
                    continue;
                }
                prefRes2.Add(new StateEvaluationDLL.DataStructures.Preference(lessSequences2[i], Sequences2[i]));
            }

            foreach (var preference in prefRes1)
            {
                pref1.Add(preference.Type.ToString());
            }
            foreach (var preference in prefRes2)
            {
                if (preference == null)
                {
                    pref2.Add(null);
                    continue;
                }
                pref2.Add(preference.Type.ToString());
            }
            List<Preference> prefinTable = new List<Preference>();
            for (var index = 0; index < pref1.Count; index++)
            {
                var preference = pref1[index];
                string shorder1 = string.Empty;
                string shorder2 = string.Empty;
                string order1 = string.Empty;
                string order2 = string.Empty;
                foreach (var od in lessSequences1[index])
                {
                    shorder1 += od + ",";
                }
                shorder1 = shorder1.Remove(shorder1.Length - 1);
                if (lessSequences2[index] != null)
                {
                    foreach (var od in lessSequences2[index])
                    {
                        if (od == null) break;
                        shorder2 += od + ",";
                    }
                    shorder2 = shorder2.Remove(shorder2.Length - 1);
                }
                foreach (var od in Sequences1[index])
                {
                    order1 += od + ",";
                }
                order1 = order1.Remove(order1.Length - 1);
                if (Sequences2[index] != null)
                {
                    foreach (var od in Sequences2[index])
                    {
                        if (od == null) break;
                        order2 += od + ",";
                    }
                    order2 = order2.Remove(order2.Length - 1);
                }
                prefinTable.Add(new Preference()
                {
                    Date = DateTime.Parse(dates[index]),
                    //FavoriteColor = int.Parse(favcolor[index]),
                    Id = Guid.NewGuid(),
                    Preference1 = "Золотая",
                    Oder1 = order1,
                    Oder2 = order2,
                    ShortOder1 = shorder1,
                    ShortOder2 = shorder2,
                    Preference2 = pref2[index] ?? "null",
                    Compare = pref1[index] == (pref2[index] ?? "") ? "true" : "false",
                    UserId = $"Ex21#{number[index]}"
                });
            }

            foreach (var pref in prefinTable)
            {
                _preferenceDb.Preference.InsertOnSubmit(pref);
                _preferenceDb.SubmitChanges();
            }
            #endregion

            foreach (object s in (Array)excelRange)
            {
                Console.WriteLine(s);
            }

            for (int i = 2; i <= excelRange.Count + 1; i++)
            {
                string values = sh.Cells[i, 2].ToString();
            }
        }

        private const int STEP = 7;
        private void Prew(object sender, RoutedEventArgs e) => BioColor.Main.MakeStep(-STEP);
        private void Next(object sender, RoutedEventArgs e) => BioColor.Main.MakeStep(+STEP);
        private void Menu(object sender, RoutedEventArgs e) => BioColor.Main.Menu();
        private void Generate(object sender, RoutedEventArgs e) => BioColor.Main.Generate();
        private void DrawGraphs(object sender, RoutedEventArgs e) => BioColor.Main.DrawGraphs();

        private void TestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private string GenerateRange(string f, string t)
        {
            int from = 0;
            int to = 0;
            if (!int.TryParse(f, out from) || !int.TryParse(t, out to))
            {
                return "([0-9]+)";
            }
            else
            {
                string re = "(";
                if (from > to) {
                    int temp = from;
                    from = to;
                    to = temp;
                };
                for(int i = from; i <= to; ++i)
                {
                    re += i;
                    if (i != to) re += "|";
                }
                re += ")";
                return re;
            }
        }
        private void FilterUIDs(object sender, SelectionChangedEventArgs e)
        {

            string id = UID.SelectedItem?.ToString();
            string exfrom = ExFrom.SelectedItem?.ToString()?.Trim();
            string exto = ExTo.SelectedItem?.ToString()?.Trim();
            string peoplefrom = PeopleFrom.SelectedItem?.ToString()?.Trim();
            string peopleto = PeopleTo.SelectedItem?.ToString()?.Trim();
            DateTime datefrom = DateFrom.SelectedDate.GetValueOrDefault();
            DateTime dateto = DateTo.SelectedDate.GetValueOrDefault();

            if (id == "All" || id == null)
            {
                var re = new System.Text.RegularExpressions.Regex("Ex" + GenerateRange(exfrom, exto) + "#" + GenerateRange(peoplefrom, peopleto));
                TestsDataGrid.ItemsSource = _preferenceDb.GetAllTests()
                    .Where(item => re.IsMatch(item.UserId)).Where(item => (datefrom.Ticks == 0 || item.Date >= datefrom) && (item.Date <= dateto || dateto.Ticks == 0));
            }
            else
            {
                TestsDataGrid.ItemsSource = _preferenceDb.GetAllTests()
                       .Where(item => item.UserId == id);
            }
        }
    }
}
