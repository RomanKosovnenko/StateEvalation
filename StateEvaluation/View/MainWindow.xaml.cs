using Microsoft.Office.Interop.Excel;
using StateEvaluation.BioColor;
using StateEvaluation.Model;
using StateEvaluation.View;
using StateEvaluation.ViewModel;
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
using StateEvaluation.Providers;

namespace StateEvaluation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const float MAX = 0xFF;
        public static Dictionary<string, string> people;
        private PreferenceDB _preferenceDb = new PreferenceDB();
        public List<string> PersonCodes = new List<string>();
        public PeopleBuissnesManager peopleBuissnesManager;
        public PreferenceBuissnesManager preferenceBuissnesManager;
        public SubjectiveFeelingBuissnesManager subjectiveFeelingBuissnesManager;

        #region ctor
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            BioColor.Main.InitBioColor(BioColorGraph, Date, DateNow);

            peopleBuissnesManager = new PeopleBuissnesManager
                (
                    new List<ComboBox>() { UserIdsFilter, UserIdsPreferenceInsert, UserIdsSubFelingInsert },
                    new List<ComboBox>() { ExpeditionFromFilter, ExpeditionToFilter },
                    new List<ComboBox>() { PeopleFromFilter, PeopleToFilter },
                    PersonDataGrid, UpdatePersonBtn
                );

            preferenceBuissnesManager = new PreferenceBuissnesManager(PreferencesDataGrid, UpdatePrefernceBtn);

            subjectiveFeelingBuissnesManager = new SubjectiveFeelingBuissnesManager(SubjectiveFeelingDataGrid, UpdateSubjectiveFeelingBtn);
        }
        #endregion

        #region Button handlers for People
        /// <summary>
        /// Create new person from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void CreatePerson(object sender, RoutedEventArgs e)
        {
            var newPerson = (PeopleVM)Resources["peopleVM"];
            peopleBuissnesManager.CreatePerson(newPerson);
        }

        /// <summary>
        /// Update person from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void UpdatePerson(object sender, RoutedEventArgs e)
        {
            var editedPerson = (PeopleVM)Resources["peopleVM"];
            peopleBuissnesManager.UpdatePerson(editedPerson);
        }

        /// <summary>
        /// Bind person data into input fields 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void BindPersonInForm(object sender, RoutedEventArgs e)
        {
            var butonContext = ((Button)sender).DataContext;
            var personId = ((People)butonContext).Id;

            var personDto = (PeopleVM)Resources["peopleVM"];

            peopleBuissnesManager.PrepareInputForm(personDto, personId);
        }

        /// <summary>
        /// Remove person
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void RemovePerson(object sender, RoutedEventArgs e)
        {
            var personDto = (PeopleVM)Resources["peopleVM"];
            var dialogResult = MessageBox.Show("Sure", "Remove item", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                peopleBuissnesManager.DeletePerson(personDto.Id);
            }
        }
        #endregion

        #region Button handlers for Preference
        /// <summary>
        /// Create test of preference from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void CreatePreferense(object sender, RoutedEventArgs e)
        {
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            preferenceBuissnesManager.Create(preferenceVM);
        }

        /// <summary>
        /// Bind test of preference into input fields
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void BindPreferenceInForm(object sender, RoutedEventArgs e)
        {
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            var butonContext = ((Button)sender).DataContext;
            var preferenceId = ((Preference)butonContext).Id;

            preferenceBuissnesManager.PrepareInputForm(preferenceVM, preferenceId);
        }

        /// <summary>
        /// Update test of preference from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void UpdatePreference(object sender, RoutedEventArgs e)
        {
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            preferenceBuissnesManager.UpdatePreference(preferenceVM);
        }

        /// <summary>
        /// Remove test of preference 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void RemovePreference(object sender, RoutedEventArgs e)
        {
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            var dialogResult = MessageBox.Show("Sure", "Remove item", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                preferenceBuissnesManager.RemovePreference(preferenceVM.Id);
            }
        }

        /// <summary>
        /// Reset all inputs fields
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void ResetPreferenceInputs(object sender, RoutedEventArgs e)
        {
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            preferenceBuissnesManager.ClearInputs(preferenceVM);
        }
        #endregion

        #region Button handler for SubjectiveFeelng
        /// <summary>
        /// Create new record of subjective feeling from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void AddFeelingPeople(object sender, RoutedEventArgs e)
        {
            var subjectiveFeeling = (SubjectiveFeelingVM)this.Resources["subjectiveFeelingVM"];
            subjectiveFeelingBuissnesManager.Create(subjectiveFeeling);
        }

        /// <summary>
        /// Bind record of subjective feeling into input fields
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void EditFeelingsBtn_Click(object sender, RoutedEventArgs e)
        {
            var butonContext = ((Button)sender).DataContext;
            var subjectiveFeelingId = ((SubjectiveFeeling)butonContext).Id;

            var subjectiveFeeling = (SubjectiveFeelingVM)Resources["subjectiveFeelingVM"];

           subjectiveFeelingBuissnesManager.PrepareInputForm(subjectiveFeeling, subjectiveFeelingId);
        }

        /// <summary>
        /// Remove record of subjective feeling
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void RemoveFeelingsBtn_Click(object sender, RoutedEventArgs e)
        {
            var butonContext = ((Button)sender).DataContext;
            var subjectiveFeelingId = ((SubjectiveFeeling)butonContext).Id;

            var dialogResult = MessageBox.Show("Sure", "Remove item", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                subjectiveFeelingBuissnesManager.Remove(subjectiveFeelingId);
            }
        }

        /// <summary>
        /// Update record of subjective feeling from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void UpdateSubjectiveFeeling(object sender, RoutedEventArgs e)
        {
            var butonContext = ((Button)sender).DataContext;
            var subjectiveFeelingId = ((SubjectiveFeelingVM)butonContext).Id;

            var subjectiveFeelingMV = (SubjectiveFeelingVM)Resources["subjectiveFeelingVM"];

            subjectiveFeelingBuissnesManager.Update(subjectiveFeelingMV, Guid.Parse(subjectiveFeelingId));
        }
        #endregion

        #region focus

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
        #endregion

        #region charts
        private void BildChartOnPreference1(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            var subWindow = new TestsChart(GetUIDs().OrderBy(x => x.Date).ToList(), true, DateFrom.SelectedDate != null && DateFrom.Text == DateTo.Text);
        }
        private void BildChartOnPreference2(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            var subWindow = new TestsChart(GetUIDs().OrderBy(x => x.Date).ToList(), false, DateFrom.SelectedDate != null && DateFrom.Text == DateTo.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PreferenceDB _preferenceDb = new PreferenceDB();
            Preference pref = _preferenceDb.Preference.Single(c => c == ((Button)(e.Source)).BindingGroup.Items[0]);
            var subWindow = new IndividualChart(pref, _preferenceDb);
        }
        #endregion

        #region import
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
        #endregion

        private const int STEP = 7;
        private void Prew(object sender, RoutedEventArgs e) => BioColor.Main.MakeStep(-STEP);
        private void Next(object sender, RoutedEventArgs e) => BioColor.Main.MakeStep(+STEP);
        private void Menu(object sender, RoutedEventArgs e) => BioColor.Main.Menu();
        private void Generate(object sender, RoutedEventArgs e) => BioColor.Main.Generate();
        private void DrawGraphs(object sender, RoutedEventArgs e) => BioColor.Main.DrawGraphs();

        private void WindowRendered(object sender, EventArgs e)
        {
            people = _preferenceDb
                .GetPeople()
                .Select(item => new { UserId = item.UserId.ToString().Trim(), item.Birthday })
                .ToDictionary(i => i.UserId, i => i.Birthday);
            ClearFilters();
            RestoreColors();
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

        private void ClearFilters()
        {
            ComdoBoxProfession.SelectedIndex = 0;
            PreferenceFilter.SelectedIndex = 0;
            ExpeditionFromFilter.SelectedIndex = 0;
            ExpeditionToFilter.SelectedIndex = 0;
            PeopleFromFilter.SelectedIndex = 0;
            PeopleToFilter.SelectedIndex = 0;
            UserIdsFilter.SelectedIndex = 0;
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

        private void ClearUIDs(object sender, RoutedEventArgs e)
        {
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    PersonDataGrid.ItemsSource = _preferenceDb.GetPeople();
                    break;
                case 1:
                    PreferencesDataGrid.ItemsSource = _preferenceDb.GetPreferences();
                    break;
                case 2:
                    SubjectiveFeelingDataGrid.ItemsSource = _preferenceDb.GetSubjecriveFeelings();
                    break;
            }
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
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    PersonDataGrid.ItemsSource = GetPeople();
                    break;
                case 1:
                    PreferencesDataGrid.ItemsSource = GetUIDs();
                    break;
                case 2:
                    SubjectiveFeelingDataGrid.ItemsSource = GetSubjectiveFeel();
                    break;
            }
        }

        private IEnumerable<Preference> GetUIDs()
        {

            string id = UserIdsFilter.SelectedItem?.ToString();
            string exfrom = ExpeditionFromFilter.SelectedItem?.ToString()?.Trim();
            string exto = ExpeditionToFilter.SelectedItem?.ToString()?.Trim();
            string peoplefrom = PeopleFromFilter.SelectedItem?.ToString()?.Trim();
            string peopleto = PeopleToFilter.SelectedItem?.ToString()?.Trim();
            string preference = PreferenceFilter.SelectedItem?.ToString();
            DateTime datefrom = DateFrom.SelectedDate.GetValueOrDefault();
            DateTime dateto = DateTo.SelectedDate.GetValueOrDefault();
            string profession = ComdoBoxProfession.SelectedItem?.ToString();

            Regex re = new Regex(id == "All" ? "Ex" + GenerateRange(exfrom, exto) + "#" + GenerateRange(peoplefrom, peopleto) : id);

            var people = _preferenceDb.GetPeople().Where(item => (item.Workposition == profession || profession == "All"));
            List<string> listOfPeople = new List<string>();
            foreach (var item in people.ToList())
            {
                listOfPeople.Add(item.UserId.ToString());
            }
            return _preferenceDb.GetPreferences()
                .Where(item =>
/* UserID     */   (re.IsMatch(item.UserId)) &&
/* DatePicker */   ((datefrom.Ticks == 0 || item.Date >= datefrom) && (item.Date <= dateto || dateto.Ticks == 0)) &&
/* ShortOder  */   (CompareOrder(item.ShortOder1, PreferenceShortFilter1.Text, PreferenceShortFilter2.Text, PreferenceShortFilter3.Text)) &&
/* Oder       */   (CompareOrder(item.Oder1, PreferenceFilter1.Text, PreferenceFilter2.Text, PreferenceFilter3.Text, PreferenceFilter4.Text, PreferenceFilter5.Text, PreferenceFilter6.Text, PreferenceFilter7.Text, PreferenceFilter8.Text, PreferenceFilter9.Text, PreferenceFilter10.Text, PreferenceFilter11.Text, PreferenceFilter12.Text)) &&
/* Preference */   (item.Preference1 == preference || preference == "All") &&
                   (listOfPeople.Contains(item.UserId))
                );
        }
        private IEnumerable<People> GetPeople()
        {
            string id = UserIdsFilter.SelectedItem?.ToString();
            string exfrom = ExpeditionFromFilter.SelectedItem?.ToString()?.Trim();
            string exto = ExpeditionToFilter.SelectedItem?.ToString()?.Trim();
            string peoplefrom = PeopleFromFilter.SelectedItem?.ToString()?.Trim();
            string peopleto = PeopleToFilter.SelectedItem?.ToString()?.Trim();
            string preference = PreferenceFilter.SelectedItem?.ToString();
            DateTime datefrom = DateFrom.SelectedDate.GetValueOrDefault();
            DateTime dateto = DateTo.SelectedDate.GetValueOrDefault();
            string profession = ComdoBoxProfession.SelectedItem?.ToString();

            Regex re = new Regex(id == "All" ? "Ex" + GenerateRange(exfrom, exto) + "#" + GenerateRange(peoplefrom, peopleto) : id);

            return _preferenceDb.GetPeople()
                .Where(item =>
    /* UserID     */   (re.IsMatch(item.UserId)) &&
                       (item.Workposition == profession || profession == "All")
                );
        }
       
        private IEnumerable<SubjectiveFeeling> GetSubjectiveFeel()
        {
            string id = UserIdsFilter.SelectedItem?.ToString();
            string exfrom = ExpeditionFromFilter.SelectedItem?.ToString()?.Trim();
            string exto = ExpeditionToFilter.SelectedItem?.ToString()?.Trim();
            string peoplefrom = PeopleFromFilter.SelectedItem?.ToString()?.Trim();
            string peopleto = PeopleToFilter.SelectedItem?.ToString()?.Trim();
            string preference = PreferenceFilter.SelectedItem?.ToString();
            DateTime datefrom = DateFrom.SelectedDate.GetValueOrDefault();
            DateTime dateto = DateTo.SelectedDate.GetValueOrDefault();
            string profession = ComdoBoxProfession.SelectedItem?.ToString();

            bool gWeakness = generalWeakness.IsChecked.Value;
            bool bAppetite = badAppetite.IsChecked.Value;
            bool bDream = badDream.IsChecked.Value;
            bool bMood = badMood.IsChecked.Value;
            bool hHead = heavyHead.IsChecked.Value;
            bool sThink = slowThink.IsChecked.Value;

            var people = _preferenceDb.GetPeople().Where(item => (item.Workposition == profession || profession == "All"));
            List<string> listOfPeople = new List<string>();
            foreach (var item in people.ToList())
            {
                listOfPeople.Add(item.UserId.ToString());
            }
            Regex re = new Regex(id == "All" ? "Ex" + GenerateRange(exfrom, exto) + "#" + GenerateRange(peoplefrom, peopleto) : id);

            return _preferenceDb.GetSubjecriveFeelings()
                .Where(item =>
    /* UserID     */   (re.IsMatch(item.UserId)) &&
                       (datefrom.Ticks == 0 || item.Date >= datefrom) &&
                       (item.Date <= dateto || dateto.Ticks == 0) &&
                       (item.GeneralWeaknes == gWeakness) &&
                       (item.PoorAppetite == bAppetite) &&
                       (item.PoorSleep == bDream) &&
                       (item.BadMood == bMood) &&
                       (item.HeavyHead == hHead) &&
                       (item.SlowThink == sThink) &&
                       (listOfPeople.Contains(item.UserId))
                );
        }
        
        private void SetFilterVisibility(object sender, SelectionChangedEventArgs e)
        {
            if (Filter != null)
            {
                Filter.Visibility = tabControl.SelectedIndex > 4 ? Visibility.Hidden : Visibility.Visible;
            }
        }
        #region biocolor
        private void CMYK2RGB(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name;
            string id = name.Substring(name.Length - 1);
            string factor = name.Substring(0, 1);

            int.TryParse((FindName("c" + factor + "c" + id) as TextBox).Text.ToString(), out int C);
            int.TryParse((FindName("m" + factor + "c" + id) as TextBox).Text.ToString(), out int M);
            int.TryParse((FindName("y" + factor + "c" + id) as TextBox).Text.ToString(), out int Y);
            int.TryParse((FindName("k" + factor + "c" + id) as TextBox).Text.ToString(), out int K);

            if (new List<int> { C, M, Y, K }.Any(x => x < 0 || x > ImageGenerator.MAX))
            {
                MessageBox.Show("Invalid color");
                return;
            }

            int[] RGB = ImageGenerator.CmykToRgb(C, M, Y, K);
            string rgb = String.Format("{0:X2}{1:X2}{2:X2}", RGB[0], RGB[1], RGB[2]);
            (FindName(factor + "c" + id) as TextBox).Text = rgb;
            if (ImageGenerator.HEX.IsMatch(rgb))
            {
                (FindName(factor + "cb" + id) as GeometryDrawing).Brush = (Brush)new BrushConverter().ConvertFromString("#" + rgb);
            }
        }

        private void RestoreColors(object sender, RoutedEventArgs e)
        {
            RestoreColors();
        }
        private void ResetColors(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Do you want to RESET colors settings?", "Reset Settings", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                Settings.Default.Reset();
                RestoreColors();
                SaveColors();
            }
        }
        internal void RestoreColors()
        {
            InitializeComponent(); ;
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
            foreach (var element in new TextBox[] { ic1, ic2, ic3, ic4, ec1, ec2, ec3, ec4, pc1, pc2, pc3, pc4 })
            {
                var name = element.Name;
                var factor = name.Substring(0, 1);
                var id = name.Substring(name.Length - 1);

                int[] CMYK = ImageGenerator.RgbToCmyk(element.Text);

                (FindName("c" + factor + "c" + id) as TextBox).Text = CMYK[0].ToString();
                (FindName("m" + factor + "c" + id) as TextBox).Text = CMYK[1].ToString();
                (FindName("y" + factor + "c" + id) as TextBox).Text = CMYK[2].ToString();
                (FindName("k" + factor + "c" + id) as TextBox).Text = CMYK[3].ToString();

                (FindName(factor + "cb" + id) as GeometryDrawing).Brush = (Brush)new BrushConverter().ConvertFromString("#" + element.Text);

            }
        }

        private void SaveColors(object sender, RoutedEventArgs e)
        {
            SaveColors();
        }
        internal void SaveColors()
        {
            if (new List<TextBox> { ic1, ic2, ic3, ic4, ec1, ec2, ec3, ec4, pc1, pc2, pc3, pc4 }.Any(x => !ImageGenerator.HEX.IsMatch(x.Text.ToUpper())))
            {
                MessageBox.Show("Invalid color!");
                return;
            }
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
            Settings.Default.Save();

            RestoreColors();
            ImageGenerator.Generate(23);
            ImageGenerator.Generate(28);
            ImageGenerator.Generate(33);
        }

    }
    #endregion
}