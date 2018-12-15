using StateEvaluation.Common.ViewModel;
using StateEvaluation.Repository.Models;
using StateEvaluation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StateEvaluation
{
    partial class MainWindow : Window
    {
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

        /// <summary>
        /// Clear filter for Preference grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilterPreferenceTab(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Clear(PreferencesDataGrid, preferenceFilter);
        }

        /// <summary>
        /// Filter data in Preference grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterPreference(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter);
        }

        /// <summary>
        /// Build chart by first preference
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BildChartOnPreference1(object sender, RoutedEventArgs e)
        {
            var preferences = ((List<Preference>)filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter))
                .OrderBy(x => x.Date).ToList();
            var subWindow = new TestsChart(preferences, true, preferenceFilter?.DateFrom != null && preferenceFilter?.DateFrom == preferenceFilter?.DateTo);
        }

        /// <summary>
        /// Build chart by second preference
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BildChartOnPreference2(object sender, RoutedEventArgs e)
        {
            var preferences = ((List<Preference>)filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter))
                .OrderBy(x => x.Date).ToList();
            var subWindow = new TestsChart(preferences, false, preferenceFilter?.DateFrom != null && preferenceFilter?.DateFrom == preferenceFilter?.DateTo);
        }

        /// <summary>
        /// Build induvidual charts for person
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentPreference = (Preference)((Button)(e.Source)).BindingGroup.Items[0];
            Preference pref = preferenceBuissnesManager.GetPreference(currentPreference);
            var subWindow = new IndividualChart(pref);
        }
    }
}