using Microsoft.Office.Interop.Excel;
using StateEvaluation.Model;
using StateEvaluation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        public static Dictionary<string, string> people;
        private PreferenceDB _preferenceDb = new PreferenceDB();
        public List<string> PersonCodes = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            BioColor.Main.InitBioColor(BioColorGraph, Date, DateNow);
        }

        private void AddFeelingPeople(object sender, RoutedEventArgs e)
        {
            if (!DateTime.TryParse((PersonAddFormGrid.FindName("selectedDateInSubjectiveFeeling") as DatePicker).Text, out DateTime obj1)||
                string.IsNullOrEmpty((PersonAddFormGrid.FindName("selectedUIDInSubjectiveFeeling") as ComboBox).Text))
            {
                MessageBox.Show("Error! Try edit fields in form!");
                return;
            }
            SubjectiveFeeling sf = GetNewSubjectiveFeeling();
            _preferenceDb.InsertEntityInSubjectiveFeeling(sf);
            RefreshSubjectiveFeelDataGrid();
            ClearSelected();
            RefreshUIDInTabs();
        }
        private void RefreshSubjectiveFeelDataGrid()
        {
            SubjectiveFeelDataGrid.ItemsSource = _preferenceDb.GetAllSubjecriveFeelings();
        }
        /// <summary>
        /// Получение из интерфейса новых данных для субъективных ощущений
        /// </summary>
        /// <returns> Заданные пользователем субъективные ощущения </returns>
        private SubjectiveFeeling GetNewSubjectiveFeeling()
        {
            string userId = selectedUIDInSubjectiveFeeling.SelectedItem?.ToString();

            SubjectiveFeeling subjectiveFeeling = new SubjectiveFeeling()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = selectedDateInSubjectiveFeeling.SelectedDate.GetValueOrDefault(),
                GeneralWeaknes = markGeneralWeakness.IsChecked.Value,
                PoorAppetite = markBadAppetite.IsChecked.Value,
                PoorSleep = markBadDream.IsChecked.Value,
                BadMood = markBadMood.IsChecked.Value,
                HeavyHead = markHeavyHead.IsChecked.Value,
                SlowThink = markSlowThink.IsChecked.Value
            };
            return subjectiveFeeling;
        }
        private void ClearSelected()
        {
            markGeneralWeakness.IsChecked = false;
            markBadAppetite.IsChecked = false;
            markBadDream.IsChecked = false;
            markBadMood.IsChecked = false;
            markHeavyHead.IsChecked = false;
            markSlowThink.IsChecked = false;
            selectedUIDInSubjectiveFeeling.SelectedIndex = -1;
            selectedDateInSubjectiveFeeling.Text = "";
        }
        
        private void SaveTestCommand(object sender, RoutedEventArgs e)
        {
            var Cin3s = new List<string> { C1in3, C2in3, C3in3 };
            var C2in3s = new List<string> { C21in3, C22in3, C23in3 };
            var Cin12s = new List<string> { C1in12, C2in12, C3in12, C4in12, C5in12, C6in12, C7in12, C8in12, C9in12, C10in12, C11in12, C12in12 };
            var C2in12s = new List<string> { C21in12, C22in12, C23in12, C24in12, C25in12, C26in12, C27in12, C28in12, C29in12, C210in12, C211in12, C212in12 };
            var Pref1 = new List<RadioButton> { RedStat, YellowStat, BlueStat, GrayStat };
            var Pref2 = new List<RadioButton> { Red2Stat, Yellow2Stat, Blue2Stat, Gray2Stat };
            var Prefs = new List<string> { "Красная", "Желтая", "Синяя", "Смешанная" };

            var inputs = new List<string> { SelectedCode };

            if (new List<List<string>> { inputs, Cin3s, C2in3s, Cin12s, C2in12s }.Any(x => x.Any(y => y == null) || x.Count != x.Distinct().Count())
                || new List<List<RadioButton>> { Pref1, Pref2 }.Any(x => x.All(y => y.IsChecked != true))
                || TestDate == null)
            {
                MessageBox.Show("Not all fields is filled!");
            }
            else
            {
                MessageBox.Show(SelectedCode);

                Preference preference = new Preference()
                {
                    Id = Guid.NewGuid(),
                    UserId = SelectedCode,
                    Date = (DateTime)TestDate,
                    FavoriteColor = 0,
                    ShortOder1 = String.Join(",", Cin3s),
                    Oder1 = String.Join(",", Cin12s),
                    Preference1 = Prefs[Pref1.Select(x => x.IsChecked).ToList().IndexOf(true)],
                    ShortOder2 = String.Join(",", C2in3s),
                    Oder2 = String.Join(",", C2in12s),
                    Preference2 = Prefs[Pref2.Select(x => x.IsChecked).ToList().IndexOf(true)],
                    Compare = (Pref1.Select(x => x.IsChecked).ToList().IndexOf(true) == Pref2.Select(x => x.IsChecked).ToList().IndexOf(true)).ToString().ToLower(),
                    RelaxTable1 = CRelax1,
                    RelaxTable2 = CRelax2
                };
                PreferenceDB _preferenceDbNew = new PreferenceDB();
                _preferenceDbNew.Preference.InsertOnSubmit(preference);
                _preferenceDbNew.SubmitChanges();
                TestsDataGrid.ItemsSource = _preferenceDbNew.GetAllTests();
                ClearInputs();
            }
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
            People person = GetNewPeople();
            _preferenceDb.InsertEntityInPeople(person);
            RefreshPersonDataGrid();
            ClearSelectedInPeople();
            RefreshUIDInTabs();
        }
        private void RefreshUIDInTabs()
        {
            UID.ItemsSource = _preferenceDb.CodesForFilter();
            selectedUIDInSubjectiveFeeling.ItemsSource = _preferenceDb.CodesForInsert();
            selectedUIDInTests.ItemsSource = _preferenceDb.CodesForInsert();
        }
        private void ClearSelectedInPeople()
        {
            (PersonAddFormGrid.FindName("FirstnameTextbox") as TextBox).Text = string.Empty;
            (PersonAddFormGrid.FindName("LastnameTextbox") as TextBox).Text = string.Empty;
            (PersonAddFormGrid.FindName("BirthdayDatePicker") as DatePicker).Text = string.Empty;
            (PersonAddFormGrid.FindName("ProfessionTextbox") as TextBox).Text = string.Empty;
            (PersonAddFormGrid.FindName("ExpeditionTextbox") as TextBox).Text = string.Empty;
            (PersonAddFormGrid.FindName("NumberTextbox") as TextBox).Text = string.Empty;
        }
        private void RefreshPersonDataGrid()
        {
            PersonDataGrid.ItemsSource = _preferenceDb.GetAllPeople();
        }
        private People GetNewPeople()
        {
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
            return person;
        }
        private void RemovePersonBtn_Click(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            People person = _preferenceDb.People.Single(c => c == ((Button)(e.Source)).BindingGroup.Items[0]);
            _preferenceDb.People.DeleteOnSubmit(person);
            _preferenceDb.SubmitChanges();
            PersonDataGrid.ItemsSource = _preferenceDb.GetAllPeople();
            RefreshUIDInTabs();
        }

        private void RemovePreferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            Preference pref = _preferenceDb.Preference.Single(c => c == ((Button)(e.Source)).BindingGroup.Items[0]);
            _preferenceDb.Preference.DeleteOnSubmit(pref);
            _preferenceDb.SubmitChanges();
            TestsDataGrid.ItemsSource = _preferenceDb.GetAllTests();
            RefreshUIDInTabs();
        }

        private void RemoveFeelingsBtn_Click(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            SubjectiveFeeling feelings = _preferenceDb.SubjectiveFeeling.Single(c => c == ((Button)(e.Source)).BindingGroup.Items[0]);
            _preferenceDb.SubjectiveFeeling.DeleteOnSubmit(feelings);
            _preferenceDb.SubmitChanges();
            SubjectiveFeelDataGrid.ItemsSource = _preferenceDb.GetAllSubjecriveFeelings();
            RefreshUIDInTabs();
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

        private void BildChartOnPreference1(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            var subWindow = new TestsChart(GetUIDs().OrderBy(x => x.Date).ToList(),true);
        }
        private void BildChartOnPreference2(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            var subWindow = new TestsChart(GetUIDs().OrderBy(x => x.Date).ToList(),false);
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

        private void WindowRendered(object sender, EventArgs e)
        {
            people = _preferenceDb
                .GetAllPeople()
                .Select(item => new { UserId = item.UserId.ToString().Trim(), item.Birthday })
                .ToDictionary(i => i.UserId, i => i.Birthday);
            ClearFilters();
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
                if (from > to)
                {
                    int temp = from;
                    from = to;
                    to = temp;
                };
                for (int i = from; i <= to; ++i)
                {
                    re += i;
                    if (i != to) re += "|";
                }
                re += ")";
                return re;
            }
        }
        private void ClearInputs()
        {
            selectedUIDInTests.SelectedIndex = -1;
            dateTimeForTest.Text = "";
            selectorC1in3    .SelectedIndex = -1 ;
            selectorC2in3    .SelectedIndex = -1 ;
            selectorC3in3    .SelectedIndex = -1 ;
            selectorC21in3   .SelectedIndex = -1 ;
            selectorC22in3   .SelectedIndex = -1 ;
            selectorC23in3   .SelectedIndex = -1 ;
            selectorC1in12   .SelectedIndex = -1 ;
            selectorC2in12   .SelectedIndex = -1 ;
            selectorC3in12   .SelectedIndex = -1 ;
            selectorC4in12   .SelectedIndex = -1 ;
            selectorC5in12   .SelectedIndex = -1 ;
            selectorC6in12   .SelectedIndex = -1 ;
            selectorC7in12   .SelectedIndex = -1 ;
            selectorC8in12   .SelectedIndex = -1 ;
            selectorC9in12   .SelectedIndex = -1 ;
            selectorC10in12  .SelectedIndex = -1 ;
            selectorC11in12  .SelectedIndex = -1 ;
            selectorC12in12  .SelectedIndex = -1 ;
            selectorC21in12  .SelectedIndex = -1 ;
            selectorC22in12  .SelectedIndex = -1 ;
            selectorC23in12  .SelectedIndex = -1 ;
            selectorC24in12  .SelectedIndex = -1 ;
            selectorC25in12  .SelectedIndex = -1 ;
            selectorC26in12  .SelectedIndex = -1 ;
            selectorC27in12  .SelectedIndex = -1 ;
            selectorC28in12  .SelectedIndex = -1 ;
            selectorC29in12  .SelectedIndex = -1 ;
            selectorC210in12 .SelectedIndex = -1 ;
            selectorC211in12 .SelectedIndex = -1 ;
            selectorC212in12 .SelectedIndex = -1 ;
            selectorRelax1   .SelectedIndex = -1 ;
            selectorRelax2   .SelectedIndex = -1 ;
            RedStat     .IsChecked = false;
            YellowStat  .IsChecked = false;
            BlueStat    .IsChecked = false;
            GrayStat    .IsChecked = false;
            Red2Stat    .IsChecked = false;
            Yellow2Stat .IsChecked = false;
            Blue2Stat   .IsChecked = false;
            Gray2Stat   .IsChecked = false;
        }
        private void ClearFilters()
        {
            Pref.SelectedIndex = 0;
            ExFrom.SelectedIndex = 0;
            ExTo.SelectedIndex = 0;
            PeopleFrom.SelectedIndex = 0;
            PeopleTo.SelectedIndex = 0;
            UID.SelectedIndex = 0;
            DateFrom.Text = "";
            DateTo.Text = "";
            PreferenceFilter1.Text = "";
            PreferenceFilter2.Text = "";
            PreferenceFilter3.Text = "";
            PreferenceFilter4.Text = "";
            PreferenceFilter5.Text = "";
            PreferenceFilter6.Text = "";
            PreferenceFilter7.Text = "";
            PreferenceFilter8.Text = "";
            PreferenceFilter9.Text = "";
            PreferenceFilter10.Text = "";
            PreferenceFilter11.Text = "";
            PreferenceFilter12.Text = "";

            generalWeakness.IsChecked = false;
            badAppetite.IsChecked = false;
            badDream.IsChecked = false;
            badMood.IsChecked = false;
            heavyHead.IsChecked = false;
            slowThink.IsChecked = false;
        }
        
        private void ClearTestAdds(object sender, RoutedEventArgs e)
        {
            ClearInputs();
        }
        private void ClearUIDs(object sender, RoutedEventArgs e)
        {
            TestsDataGrid.ItemsSource = _preferenceDb.GetAllTests();
            SubjectiveFeelDataGrid.ItemsSource = _preferenceDb.GetAllSubjecriveFeelings();
            PersonDataGrid.ItemsSource = _preferenceDb.GetAllPeople();
            ClearFilters();
        }

        private bool CompareOrder(string order, params string[] prefs)
        {
            var orders = order.Split(',');
            if (orders.Length != prefs.Length)
            {
                throw new IndexOutOfRangeException("Order lenght is not the same as prefs lengts");
            }

            var comparer = new bool[orders.Length];
            for (int i = 0; i < comparer.Length; ++i)
            {
                comparer[i] = prefs[i] == "" || prefs[i] == orders[i];
            }
            return comparer.All(x => x);
        }

        private void FilterUIDs(object sender, RoutedEventArgs e)
        {
            TestsDataGrid.ItemsSource = GetUIDs();
            PersonDataGrid.ItemsSource = GetPeople();
            SubjectiveFeelDataGrid.ItemsSource = GetSubjectiveFeel();
        }
        
        private IEnumerable<Preference> GetUIDs()
        {

            string id = UID.SelectedItem?.ToString();
            string exfrom = ExFrom.SelectedItem?.ToString()?.Trim();
            string exto = ExTo.SelectedItem?.ToString()?.Trim();
            string peoplefrom = PeopleFrom.SelectedItem?.ToString()?.Trim();
            string peopleto = PeopleTo.SelectedItem?.ToString()?.Trim();
            string preference = Pref.SelectedItem?.ToString();
            DateTime datefrom = DateFrom.SelectedDate.GetValueOrDefault();
            DateTime dateto = DateTo.SelectedDate.GetValueOrDefault();

            Regex re = new Regex(id == "All" ? "Ex" + GenerateRange(exfrom, exto) + "#" + GenerateRange(peoplefrom, peopleto) : id);

            return _preferenceDb.GetAllTests()
                .Where(item =>
/* UserID     */   (re.IsMatch(item.UserId)) &&
/* DatePicker */   ((datefrom.Ticks == 0 || item.Date >= datefrom) && (item.Date <= dateto || dateto.Ticks == 0)) &&
/* ShortOder  */   (CompareOrder(item.ShortOder1, PreferenceShortFilter1.Text, PreferenceShortFilter2.Text, PreferenceShortFilter3.Text)) &&
/* Oder       */   (CompareOrder(item.Oder1, PreferenceFilter1.Text, PreferenceFilter2.Text, PreferenceFilter3.Text, PreferenceFilter4.Text, PreferenceFilter5.Text, PreferenceFilter6.Text, PreferenceFilter7.Text, PreferenceFilter8.Text, PreferenceFilter9.Text, PreferenceFilter10.Text, PreferenceFilter11.Text, PreferenceFilter12.Text)) &&
/* Preference */   (item.Preference1 == preference || preference == "All")
                );
        }
        private IEnumerable<People> GetPeople()
        {

            string id = UID.SelectedItem?.ToString();
            string exfrom = ExFrom.SelectedItem?.ToString()?.Trim();
            string exto = ExTo.SelectedItem?.ToString()?.Trim();
            string peoplefrom = PeopleFrom.SelectedItem?.ToString()?.Trim();
            string peopleto = PeopleTo.SelectedItem?.ToString()?.Trim();
            string preference = Pref.SelectedItem?.ToString();
            DateTime datefrom = DateFrom.SelectedDate.GetValueOrDefault();
            DateTime dateto = DateTo.SelectedDate.GetValueOrDefault();

            Regex re = new Regex(id == "All" ? "Ex" + GenerateRange(exfrom, exto) + "#" + GenerateRange(peoplefrom, peopleto) : id);

            return _preferenceDb.GetAllPeople()
                .Where(item =>
    /* UserID     */   (re.IsMatch(item.UserId))
                );
        }
        private IEnumerable<SubjectiveFeeling> GetSubjectiveFeel()
        {
            string id = UID.SelectedItem?.ToString();
            string exfrom = ExFrom.SelectedItem?.ToString()?.Trim();
            string exto = ExTo.SelectedItem?.ToString()?.Trim();
            string peoplefrom = PeopleFrom.SelectedItem?.ToString()?.Trim();
            string peopleto = PeopleTo.SelectedItem?.ToString()?.Trim();
            string preference = Pref.SelectedItem?.ToString();
            DateTime datefrom = DateFrom.SelectedDate.GetValueOrDefault();
            DateTime dateto = DateTo.SelectedDate.GetValueOrDefault();

            bool gWeakness = generalWeakness.IsChecked.Value;
            bool bAppetite = badAppetite.IsChecked.Value;
            bool bDream    = badDream.IsChecked.Value;
            bool bMood     = badMood.IsChecked.Value;
            bool hHead     = heavyHead.IsChecked.Value;
            bool sThink    = slowThink.IsChecked.Value;

            Regex re = new Regex(id == "All" ? "Ex" + GenerateRange(exfrom, exto) + "#" + GenerateRange(peoplefrom, peopleto) : id);

            return _preferenceDb.GetAllSubjecriveFeelings()
                .Where(item =>
    /* UserID     */   (re.IsMatch(item.UserId)) &&
                       (item.GeneralWeaknes == gWeakness) &&
                       (item.PoorAppetite == bAppetite) &&
                       (item.PoorSleep == bDream) &&
                       (item.BadMood == bMood) &&
                       (item.HeavyHead == hHead) &&
                       (item.SlowThink == sThink)
                );
        }

        public string SelectedCode { get; set; }
        public DateTime? TestDate { get; set; }

        public string C1in3 { get; set; }
        public string C2in3 { get; set; }
        public string C3in3 { get; set; }

        public string C21in3 { get; set; }
        public string C22in3 { get; set; }
        public string C23in3 { get; set; }

        public string C1in12 { get; set; }
        public string C2in12 { get; set; }
        public string C3in12 { get; set; }
        public string C4in12 { get; set; }
        public string C5in12 { get; set; }
        public string C6in12 { get; set; }
        public string C7in12 { get; set; }
        public string C8in12 { get; set; }
        public string C9in12 { get; set; }
        public string C10in12 { get; set; }
        public string C11in12 { get; set; }
        public string C12in12 { get; set; }

        public string C21in12 { get; set; }
        public string C22in12 { get; set; }
        public string C23in12 { get; set; }
        public string C24in12 { get; set; }
        public string C25in12 { get; set; }
        public string C26in12 { get; set; }
        public string C27in12 { get; set; }
        public string C28in12 { get; set; }
        public string C29in12 { get; set; }
        public string C210in12 { get; set; }
        public string C211in12 { get; set; }
        public string C212in12 { get; set; }

        public int? CRelax1 { get; set; }
        public int? CRelax2 { get; set; }


        /* private IEnumerable<string> ExeptValue()
{
    string ShortColorsNumbersList1_Bio1 = ShortColorsNumbersList1_Biocolor1.SelectedItem?.ToString();
    MessageBox.Show(ShortColorsNumbersList1_Bio1);
    return "";   
}*/
    }
}


