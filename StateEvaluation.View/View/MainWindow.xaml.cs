using Microsoft.Office.Interop.Excel;
using StateEvaluation.Repository.Providers;
using StateEvaluation.View;
using StateEvaluation.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;
using Window = System.Windows.Window;
using StateEvaluation.Repository.Models;
using StateEvaluation.BussinesLayer.BuissnesManagers;
using StateEvaluation.BioColor;
using StateEvaluation.Common;

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
        private PreferenceFilterVM preferenceFilter;
        private PeopleFilterVM peopleFilter;
        private SubjectiveFeelingFilterVM subjectiveFeelingFilter;
        private FilterBussinesManager filterBussinesManager;
        private BiocolorProvider biocolorProvider;
        private ImageGenerator imageGenerator;

        #region ctor
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            biocolorProvider = new BiocolorProvider();
            imageGenerator = new ImageGenerator();

            biocolorProvider.InitBioColor(BioColorGraph, Date, DateNow);

            filterBussinesManager = new FilterBussinesManager();

            peopleFilter = (PeopleFilterVM)Resources["peopleFilterVM"];
            preferenceFilter = (PreferenceFilterVM)Resources["preferenceFilterVM"];
            subjectiveFeelingFilter = (SubjectiveFeelingFilterVM)Resources["subjectiveFeelingFilterVM"];


            peopleBuissnesManager = new PeopleBuissnesManager
                (
                    new List<ComboBox>() { UserIdsFilterPeopleCB, UserIdsFilterSubjFeelingCB, UserIdsFilterPreferenceCB, UserIdsInsertPreferenceCB, UserIdsInsertSubjFeelCB },
                    new List<ComboBox>() { ExpeditionFromFilterPeopleCB, ExpeditionToFilterPeopleCB, ExpeditionFromFilterSubjFeelCB, ExpeditionToFilterSubjFeelCB, ExpeditionFilterToPreferenceCB, ExpeditionFromFilterPreferenceCB },
                    new List<ComboBox>() { NumberFromFilterPeopleCB, NumberToFilterPeopleCB, NumberFromFilterSubjFeelCB, NumberToFilterSubjFeelCB, NumberFromFilterPreferenceCB, NumberToFilterPreferenceCB },
                    PeopleDataGrid, UpdatePersonBtn
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
        /// Clear input fields from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void ClearInputs(object sender, RoutedEventArgs e)
        {
            var person = (PeopleVM)Resources["peopleVM"];
            peopleBuissnesManager.ClearInputs(person);
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

            var personVM = (PeopleVM)Resources["peopleVM"];

            peopleBuissnesManager.PrepareInputForm(personVM, personId);
        }

        /// <summary>
        /// Remove person
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void RemovePerson(object sender, RoutedEventArgs e)
        {
            var butonContext = ((Button)sender).DataContext;
            var personId = ((People)butonContext).Id;

            var dialogResult = MessageBox.Show("Sure", "Remove item", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                peopleBuissnesManager.DeletePerson(personId.ToString());
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
        private void ClearPreferenceInputs(object sender, RoutedEventArgs e)
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
        private void CreateSubjectiveFeeling(object sender, RoutedEventArgs e)
        {
            var subjectiveFeeling = (SubjectiveFeelingVM)this.Resources["subjectiveFeelingVM"];
            subjectiveFeelingBuissnesManager.Create(subjectiveFeeling);
        }

        /// <summary>
        /// Bind record of subjective feeling into input fields
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void BindSubjectiveFeelingInForm(object sender, RoutedEventArgs e)
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
        private void DeleteSSubjectiveFeeling(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Reset all inputs fields
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void ClearSubjectiveFeelingInputs(object sender, RoutedEventArgs e)
        {
            var subjectiveFeelingVM = (SubjectiveFeelingVM)Resources["subjectiveFeelingVM"];
            subjectiveFeelingBuissnesManager.ClearInputs(subjectiveFeelingVM);
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
           // var preferences = ((List<Preference>)filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter))
           //     .OrderBy(x => x.Date).ToList();
           // var subWindow = new TestsChart(preferences, true, preferenceFilter?.DateFrom != null && preferenceFilter?.DateFrom == preferenceFilter?.DateTo);
        }
        private void BildChartOnPreference2(object sender, RoutedEventArgs e)
        {
           // var preferences = ((List<Preference>)filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter))
           //     .OrderBy(x => x.Date).ToList();
           // var subWindow = new TestsChart(preferences, false, preferenceFilter?.DateFrom != null && preferenceFilter?.DateFrom == preferenceFilter?.DateTo);
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
        private void Prew(object sender, RoutedEventArgs e) => biocolorProvider.MakeStep(-STEP);
        private void Next(object sender, RoutedEventArgs e) => biocolorProvider.MakeStep(+STEP);
        private void Menu(object sender, RoutedEventArgs e) => biocolorProvider.Menu();
        private void Generate(object sender, RoutedEventArgs e) => biocolorProvider.Generate();
        private void DrawGraphs(object sender, RoutedEventArgs e) => biocolorProvider.DrawGraphs();

        private void WindowRendered(object sender, EventArgs e)
        {
            people = _preferenceDb
                .GetPeople()
                .Select(item => new { UserId = item.UserId.ToString().Trim(), item.Birthday })
                .ToDictionary(i => i.UserId, i => i.Birthday);
            RestoreColors();
        }

        #region filters
        private void ClearFilterPeopleTab(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Clear(PeopleDataGrid, peopleFilter);
        }

        private void ClearFilterPreferenceTab(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Clear(PreferencesDataGrid, preferenceFilter);
        }

        private void ClearFilterSubjFeelTab(object sender, RoutedEventArgs e)
        {
           filterBussinesManager.Clear(SubjectiveFeelingDataGrid, subjectiveFeelingFilter);
        }

        private void FilterPeople(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Filter(PeopleDataGrid, peopleFilter);
        }
        private void FilterPreference(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter);
        }
        private void FilterSubjectiveFeeling(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Filter(SubjectiveFeelingDataGrid, subjectiveFeelingFilter);
        }
        #endregion

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

            if (new List<int> { C, M, Y, K }.Any(x => x < 0 || x > imageGenerator.MAX))
            {
                MessageBox.Show("Invalid color");
                return;
            }

            int[] RGB = imageGenerator.CmykToRgb(C, M, Y, K);
            string rgb = String.Format("{0:X2}{1:X2}{2:X2}", RGB[0], RGB[1], RGB[2]);
            (FindName(factor + "c" + id) as TextBox).Text = rgb;
            if (imageGenerator.HEX.IsMatch(rgb))
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

                int[] CMYK = imageGenerator.RgbToCmyk(element.Text);

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
            if (new List<TextBox> { ic1, ic2, ic3, ic4, ec1, ec2, ec3, ec4, pc1, pc2, pc3, pc4 }.Any(x => !imageGenerator.HEX.IsMatch(x.Text.ToUpper())))
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
            imageGenerator.Generate(23);
            imageGenerator.Generate(28);
            imageGenerator.Generate(33);
        }

    }
    #endregion
}