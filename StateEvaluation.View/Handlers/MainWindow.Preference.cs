using StateEvaluation.Common.Constants;
using StateEvaluation.Common.ViewModel;
using StateEvaluation.Repository.Models;
using StateEvaluation.View;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StateEvaluation
{
    /// <summary>
    /// Interaction logic for Preference tab
    /// </summary>
    partial class MainWindow : Window
    {
        /// <summary>
        /// Create test of preference from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void CreatePreferense_Click(object sender, RoutedEventArgs e)
        {
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            preferenceBuissnesManager.Create(preferenceVM);
        }

        /// <summary>
        /// Bind test of preference into input fields
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void BindPreferenceInForm_Click(object sender, RoutedEventArgs e)
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
        private void UpdatePreference_Click(object sender, RoutedEventArgs e)
        {
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            preferenceBuissnesManager.UpdatePreference(preferenceVM);
        }

        /// <summary>
        /// Remove test of preference 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void RemovePreference_Click(object sender, RoutedEventArgs e)
        {
            var butonContext = ((Button)sender).DataContext;
            var preferenceId = ((Preference)butonContext).Id;
            var dialogResult = MessageBox.Show(MessageBoxConstants.DeleteSure, MessageBoxConstants.DeleteSureTitle, MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                preferenceBuissnesManager.RemovePreference(preferenceId.ToString());
            }
        }

        /// <summary>
        /// Reset all inputs fields
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void ClearPreferenceInputs_Click(object sender, RoutedEventArgs e)
        {
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            preferenceBuissnesManager.ClearInputs(preferenceVM);
        }

        /// <summary>
        /// Clear filter for Preference grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilterPreferenceTab_Click(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Clear(PreferencesDataGrid, preferenceFilter);
        }

        /// <summary>
        /// Filter data in Preference grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterPreference_Click(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter);
        }

        /// <summary>
        /// Build chart by first preference
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BildChartOnPreference1_Click(object sender, RoutedEventArgs e)
        {
            var preferences = filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter).Cast<Preference>().OrderBy(x => x.Date).ToList();
            var subWindow = new TestsChart(preferences, true, preferenceFilter?.DateFrom != null && preferenceFilter?.DateFrom == preferenceFilter?.DateTo);
        }

        /// <summary>
        /// Build chart by second preference
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BildChartOnPreference2_Click(object sender, RoutedEventArgs e)
        {
            var preferences = filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter).Cast<Preference>().OrderBy(x => x.Date).ToList();
            var subWindow = new TestsChart(preferences, false, preferenceFilter?.DateFrom != null && preferenceFilter?.DateFrom == preferenceFilter?.DateTo);
        }

        /// <summary>
        /// Build induvidual charts for person
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildInduvidualChart_Click(object sender, RoutedEventArgs e)
        {
            var currentPreference = (Preference)((Button)(e.Source)).BindingGroup.Items[0];
            if (preferenceBuissnesManager.IsExistsPreference(currentPreference))
            {
                var subWindow = new IndividualChart(currentPreference, dataRepository);
            }
            else
            {
                throw new System.Exception();
            }
        }
    }
}