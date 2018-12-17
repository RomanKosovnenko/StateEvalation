using Microsoft.Office.Interop.Excel;
using StateEvaluation.Repository.Providers;
using StateEvaluation.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using TextBox = System.Windows.Controls.TextBox;
using Window = System.Windows.Window;
using StateEvaluation.Repository.Models;
using StateEvaluation.BussinesLayer.BuissnesManagers;
using StateEvaluation.BioColor.Providers;
using StateEvaluation.BioColor.Helpers;
using StateEvaluation.BusinessLayer.BuissnesManagers;

namespace StateEvaluation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const float MAX = 0xFF;
      
        public List<string> PersonCodes = new List<string>();
        public PeopleBuissnesManager peopleBuissnesManager;
        public PreferenceBuissnesManager preferenceBuissnesManager;
        public SubjectiveFeelingBuissnesManager subjectiveFeelingBuissnesManager;
        private PreferenceFilterVM preferenceFilter;
        private PeopleFilterVM peopleFilter;
        private SubjectiveFeelingFilterVM subjectiveFeelingFilter;
        private FilterBussinesManager filterBussinesManager;
        private DataRepository dataRepository;

        private IEnumerable<ComboBox> userIdComboBoxes;
        private IEnumerable<ComboBox> expeditionComboBoxes;
        private IEnumerable<ComboBox> numberOfPeopleComboBoxes;
        private IEnumerable<ComboBox> professionsComboBoxes;
     
        #region ctor
        public MainWindow(DataRepository dataRepository)
        {
            InitializeComponent();
            this.DataContext = this;

            this.dataRepository = dataRepository;

            userIdComboBoxes = new List<ComboBox>()
            {
                UserIdsFilterPeopleCB,
                UserIdsFilterSubjFeelingCB,
                UserIdsFilterPreferenceCB,
                UserIdsInsertPreferenceCB,
                UserIdsInsertSubjFeelCB
            };

            expeditionComboBoxes = new List<ComboBox>()
            {
                ExpeditionFromFilterPeopleCB,
                ExpeditionToFilterPeopleCB,
                ExpeditionFromFilterSubjFeelCB,
                ExpeditionToFilterSubjFeelCB,
                ExpeditionFilterToPreferenceCB,
                ExpeditionFromFilterPreferenceCB
            };

            numberOfPeopleComboBoxes = new List<ComboBox>()
            {
                NumberFromFilterPeopleCB,
                NumberToFilterPeopleCB,
                NumberFromFilterSubjFeelCB,
                NumberToFilterSubjFeelCB,
                NumberFromFilterPreferenceCB,
                NumberToFilterPreferenceCB

            };

            professionsComboBoxes = new List<ComboBox>()
            {
                ProfessionFilterPeopleTab,
                ProfessionPreferenceTab,
                ProfessionsSubjectiveFeelingTab
            };

            peopleFilter = (PeopleFilterVM)Resources["peopleFilterVM"];
            preferenceFilter = (PreferenceFilterVM)Resources["preferenceFilterVM"];
            subjectiveFeelingFilter = (SubjectiveFeelingFilterVM)Resources["subjectiveFeelingFilterVM"];

            peopleBuissnesManager = new PeopleBuissnesManager
            (
                dataRepository,
                userIdComboBoxes,
                expeditionComboBoxes,
                numberOfPeopleComboBoxes,
                professionsComboBoxes,
                PeopleDataGrid,
                UpdatePersonBtn
            );

            biocolorSettings = new BiocolorSettings();
            biocolorProvider = new BiocolorProvider(biocolorSettings);
            imageGenerator = new ImageGenerator(biocolorSettings);

            biocolorProvider.InitBiocolor(BioColorGrid, Date, DateNow);

            biocolorBusinessManager = new BiocolorBusinessManager(BioColorGrid, Date, DateNow, biocolorSettings);
            preferenceBuissnesManager = new PreferenceBuissnesManager(dataRepository, PreferencesDataGrid, UpdatePrefernceBtn);
            filterBussinesManager = new FilterBussinesManager(dataRepository);
            subjectiveFeelingBuissnesManager = new SubjectiveFeelingBuissnesManager(dataRepository, SubjectiveFeelingDataGrid, UpdateSubjectiveFeelingBtn);

            colors = new List<BiocolorProvider.ColorRow> {
                new BiocolorProvider.ColorRow(PhysicalColor1, PhysicalButton1, cPhysicalColor1, mPhysicalColor1, yPhysicalColor1, kPhysicalColor1, PhysicalColorBackground1),
                new BiocolorProvider.ColorRow(PhysicalColor2, PhysicalButton2, cPhysicalColor2, mPhysicalColor2, yPhysicalColor2, kPhysicalColor2, PhysicalColorBackground2),
                new BiocolorProvider.ColorRow(PhysicalColor3, PhysicalButton3, cPhysicalColor3, mPhysicalColor3, yPhysicalColor3, kPhysicalColor3, PhysicalColorBackground3),
                new BiocolorProvider.ColorRow(PhysicalColor4, PhysicalButton4, cPhysicalColor4, mPhysicalColor4, yPhysicalColor4, kPhysicalColor4, PhysicalColorBackground4),
                new BiocolorProvider.ColorRow(EmotionalColor1, EmotionalButton1, cEmotionalColor1, mEmotionalColor1, yEmotionalColor1, kEmotionalColor1, EmotionalColorBackground1),
                new BiocolorProvider.ColorRow(EmotionalColor2, EmotionalButton2, cEmotionalColor2, mEmotionalColor2, yEmotionalColor2, kEmotionalColor2, EmotionalColorBackground2),
                new BiocolorProvider.ColorRow(EmotionalColor3, EmotionalButton3, cEmotionalColor3, mEmotionalColor3, yEmotionalColor3, kEmotionalColor3, EmotionalColorBackground3),
                new BiocolorProvider.ColorRow(EmotionalColor4, EmotionalButton4, cEmotionalColor4, mEmotionalColor4, yEmotionalColor4, kEmotionalColor4, EmotionalColorBackground4),
                new BiocolorProvider.ColorRow(IntellectualColor1, IntellectualButton1, cIntellectualColor1, mIntellectualColor1, yIntellectualColor1, kIntellectualColor1, IntellectualColorBackground1),
                new BiocolorProvider.ColorRow(IntellectualColor2, IntellectualButton2, cIntellectualColor2, mIntellectualColor2, yIntellectualColor2, kIntellectualColor2, IntellectualColorBackground2),
                new BiocolorProvider.ColorRow(IntellectualColor3, IntellectualButton3, cIntellectualColor3, mIntellectualColor3, yIntellectualColor3, kIntellectualColor3, IntellectualColorBackground3),
                new BiocolorProvider.ColorRow(IntellectualColor4, IntellectualButton4, cIntellectualColor4, mIntellectualColor4, yIntellectualColor4, kIntellectualColor4, IntellectualColorBackground4),
            };
        }
        #endregion
    }
}