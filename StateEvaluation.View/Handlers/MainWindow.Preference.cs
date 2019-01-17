using StateEvaluation.Common.Constants;
using StateEvaluation.Common.ViewModel;
using StateEvaluation.Repository.Models;
using StateEvaluation.View;
using System;
using System.Collections.Generic;
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
        private List<ComboBox> biocolor1ShortOrder;
        private List<ComboBox> biocolor1LongOrder;
        private List<ComboBox> biocolor2ShortOrder;
        private List<ComboBox> biocolor2LongOrder;
        private Preference currentPreference;

        /// <summary>
        /// Create test of preference from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void CreatePreferense_Click(object sender, RoutedEventArgs e)
        {
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            preferenceBuissnesManager.Create(preferenceVM);
            FilterPreference_Click(sender, e);
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
            SetLoaderVisibility(Visibility.Visible);
            var butonContext = ((Button)sender).DataContext;
            var preferenceId = ((Preference)butonContext).Id;
            var dialogResult = MessageBox.Show(MessageBoxConstants.DeleteSure, MessageBoxConstants.DeleteSureTitle, MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                preferenceBuissnesManager.RemovePreference(preferenceId.ToString());
            }
            SetLoaderVisibility(Visibility.Hidden);
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
            RegenerateComboBoxItems(biocolor1ShortOrder);
            RegenerateComboBoxItems(biocolor1LongOrder);
            RegenerateComboBoxItems(biocolor2ShortOrder);
            RegenerateComboBoxItems(biocolor2LongOrder);
        }

        /// <summary>
        /// Clear filter for Preference grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilterPreferenceTab_Click(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Clear(PreferencesDataGrid, preferenceFilter, new System.Collections.Generic.List<ListBox> {
                UserIdsFilterPreferencesTab,
                ExpeditionFilterPreferencesTab,
                NumberFilterPreferencesTab,
                ProfessionFilterPreferencesTab,
                PreferenceFilterPreferenceTab
            });
        }

        /// <summary>
        /// Filter data in Preference grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterPreference_Click(object sender, RoutedEventArgs e)
        {
            if (preferenceFilter.DateFrom != null && preferenceFilter.DateTo != null && (DateTime)preferenceFilter.DateFrom > (DateTime)preferenceFilter.DateTo)
            {
                MessageBox.Show(MessageBoxConstants.WrongDateFields);
            }
            else
            {
                filterBussinesManager.Filter(PreferencesDataGrid, preferenceFilter);
            }
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
            if (preferenceBuissnesManager.IsExistsPreference(currentPreference))
            {
                var subWindow = new IndividualChart(currentPreference, dataRepository);
            }
            else
            {
                throw new System.Exception();
            }
        }
        private void PreferencesDataGrid_Selected(object sender, RoutedEventArgs e)
        {
            currentPreference = (Preference)((DataGrid)(sender)).SelectedItem;
        }


        private void UserIdsFilterPreferencesTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            preferenceFilter.SetUserState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void ExpeditionFilterPreferencesTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            preferenceFilter.SetExpeditionState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void NumberFilterPreferencesTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            preferenceFilter.SetPeopleState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void ProfessionFilterPreferencesTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            preferenceFilter.SetProfessionsState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void PreferenceFilterPreferencesTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            preferenceFilter.SetPreferenceState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }

        private void RegenerateComboBoxItems(List<ComboBox> comboBoxes)
        {
            List<string> strings = new List<string> { };
            List<string> fullOrder;
            if(comboBoxes.Count() == 3)
            {
                fullOrder = Common.Enums.PreferenceValues.ShortColorsNumbersList.ToList();
            } else if(comboBoxes.Count() == 12)
            {
                fullOrder = Common.Enums.PreferenceValues.ColorsNumbersList.ToList();
            } else
            {
                fullOrder = new List<string> { };
            }

            foreach (ComboBox combo in comboBoxes)
            {
                string value = combo.SelectedValue?.ToString();
                if (value != string.Empty && value != null)
                {
                    strings.Add(value);
                }
            }

            foreach (ComboBox combo in comboBoxes)
            {
                combo.SelectionChanged -= BioColor_SelectionChanged;
                var selected = combo.SelectedItem;
                combo.Items.Clear();
                combo.Items.Add(string.Empty);
                foreach (var value in fullOrder)
                {
                    if (selected != null && value == selected.ToString())
                    {
                        combo.Items.Add(selected);
                    }
                    if (!strings.Contains(value))
                    {
                        combo.Items.Add(value);
                    }
                }
                combo.SelectedItem = selected;
                combo.SelectionChanged += BioColor_SelectionChanged;
            }
        }

        private void ExpandImage(object sender, RoutedEventArgs e)
        {
            HideImage(sender, e);

            var source = (sender as Button).Tag;
            if (source != null)
            {
                BigImage.Visibility = Visibility.Visible;
                System.Windows.Media.Imaging.BitmapImage bimage = new System.Windows.Media.Imaging.BitmapImage();
                bimage.BeginInit();
                bimage.UriSource = new System.Uri(source.ToString(), System.UriKind.Relative);
                bimage.EndInit();
                BigImage.Source = bimage;
            }
        }

        private void HideImage(object sender, RoutedEventArgs e)
        {
            BigImage.Visibility = Visibility.Hidden;
            BigImage.Source = null;
        }

        private void BioColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var combo = (sender as ComboBox);
            List<ComboBox> list;
            int biocolor;
            switch (combo.Tag)
            {
                case "shortOrder1":
                    list = biocolor1ShortOrder;
                    biocolor = 1;
                    break;
                case "longOrder1":
                    list = biocolor1LongOrder;
                    biocolor = 1;
                    break;
                case "shortOrder2":
                    list = biocolor2ShortOrder;
                    biocolor = 2;
                    break;
                case "longOrder2":
                    list = biocolor2LongOrder;
                    biocolor = 2;
                    break;
                default:
                    return;
            }
            var preferenceVM = (PreferenceVM)Resources["preferenceVM"];
            preferenceBuissnesManager.GeneratePreference(preferenceVM, biocolor);
            RegenerateComboBoxItems(list);
        }
    }
}