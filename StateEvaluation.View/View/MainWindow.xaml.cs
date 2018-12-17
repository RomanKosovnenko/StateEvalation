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
            colors = new TextBox[] { ic1, ic2, ic3, ic4, ec1, ec2, ec3, ec4, pc1, pc2, pc3, pc4 };

            biocolorProvider.InitBiocolor(BioColorGrid, Date, DateNow);

            biocolorBusinessManager = new BiocolorBusinessManager(BioColorGrid, Date, DateNow, biocolorSettings);
            preferenceBuissnesManager = new PreferenceBuissnesManager(dataRepository, PreferencesDataGrid, UpdatePrefernceBtn);
            filterBussinesManager = new FilterBussinesManager(dataRepository);
            subjectiveFeelingBuissnesManager = new SubjectiveFeelingBuissnesManager(dataRepository, SubjectiveFeelingDataGrid, UpdateSubjectiveFeelingBtn);
        }
        #endregion
    }
}