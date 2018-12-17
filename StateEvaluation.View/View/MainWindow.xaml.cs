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
     
        #region ctor
        public MainWindow(DataRepository dataRepository)
        {
            InitializeComponent();
            this.DataContext = this;

            this.dataRepository = dataRepository;

            biocolorSettings = new BiocolorSettings();

            biocolorProvider = new BiocolorProvider(biocolorSettings);
            imageGenerator = new ImageGenerator(biocolorSettings);

            biocolorProvider.InitBiocolor(BioColorGrid, Date, DateNow);

            filterBussinesManager = new FilterBussinesManager(dataRepository);

            peopleFilter = (PeopleFilterVM)Resources["peopleFilterVM"];
            preferenceFilter = (PreferenceFilterVM)Resources["preferenceFilterVM"];
            subjectiveFeelingFilter = (SubjectiveFeelingFilterVM)Resources["subjectiveFeelingFilterVM"];


            peopleBuissnesManager = new PeopleBuissnesManager
                (
                    dataRepository,
                    new List<ComboBox>() { UserIdsFilterPeopleCB, UserIdsFilterSubjFeelingCB, UserIdsFilterPreferenceCB, UserIdsInsertPreferenceCB, UserIdsInsertSubjFeelCB },
                    new List<ComboBox>() { ExpeditionFromFilterPeopleCB, ExpeditionToFilterPeopleCB, ExpeditionFromFilterSubjFeelCB, ExpeditionToFilterSubjFeelCB, ExpeditionFilterToPreferenceCB, ExpeditionFromFilterPreferenceCB },
                    new List<ComboBox>() { NumberFromFilterPeopleCB, NumberToFilterPeopleCB, NumberFromFilterSubjFeelCB, NumberToFilterSubjFeelCB, NumberFromFilterPreferenceCB, NumberToFilterPreferenceCB },
                    PeopleDataGrid, UpdatePersonBtn
                );

            biocolorBusinessManager = new BiocolorBusinessManager(BioColorGrid, Date, DateNow, biocolorSettings);

            preferenceBuissnesManager = new PreferenceBuissnesManager(dataRepository, PreferencesDataGrid, UpdatePrefernceBtn);

            subjectiveFeelingBuissnesManager = new SubjectiveFeelingBuissnesManager(dataRepository, SubjectiveFeelingDataGrid, UpdateSubjectiveFeelingBtn);
            
            colors = new TextBox[] {
                IntellectualColor1,
                IntellectualColor2,
                IntellectualColor3,
                IntellectualColor4,
                EmotionalColor1,
                EmotionalColor2,
                EmotionalColor3,
                EmotionalColor4,
                PhysicalColor1,
                PhysicalColor2,
                PhysicalColor3,
                PhysicalColor4
            };
        }
        #endregion
    }
}