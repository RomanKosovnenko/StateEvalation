using StateEvaluation.Common.Constants;
using StateEvaluation.Common.ViewModel;
using StateEvaluation.Repository.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StateEvaluation
{
    /// <summary>
    /// Interaction logic for SubjectiveFeeling tab
    /// </summary>
    partial class MainWindow : Window
    {
        /// <summary>
        /// Create new record of subjective feeling from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void CreateSubjectiveFeeling_Click(object sender, RoutedEventArgs e)
        {
            var subjectiveFeeling = (SubjectiveFeelingVM)this.Resources["subjectiveFeelingVM"];
            subjectiveFeelingBuissnesManager.Create(subjectiveFeeling);
            FilterSubjectiveFeeling_Click(sender, e);
        }

        /// <summary>
        /// Bind record of subjective feeling into input fields
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void BindSubjectiveFeelingInForm_Click(object sender, RoutedEventArgs e)
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
        private void DeleteSubjectiveFeeling_Click(object sender, RoutedEventArgs e)
        {
            SetLoaderVisibility(Visibility.Visible);
            var butonContext = ((Button)sender).DataContext;
            var subjectiveFeelingId = ((SubjectiveFeeling)butonContext).Id;

            var dialogResult = MessageBox.Show(MessageBoxConstants.DeleteSure, MessageBoxConstants.DeleteSureTitle, MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                subjectiveFeelingBuissnesManager.Remove(subjectiveFeelingId);
            }
            SetLoaderVisibility(Visibility.Hidden);
        }

        /// <summary>
        /// Update record of subjective feeling from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void UpdateSubjectiveFeeling_Click(object sender, RoutedEventArgs e)
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
        private void ClearSubjectiveFeelingInputs_Click(object sender, RoutedEventArgs e)
        {
            var subjectiveFeelingVM = (SubjectiveFeelingVM)Resources["subjectiveFeelingVM"];
            subjectiveFeelingBuissnesManager.ClearInputs(subjectiveFeelingVM);
        }

        /// <summary>
        /// Clear filters for SubjectiveFeeling grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilterSubjFeelTab_Click(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Clear(SubjectiveFeelingDataGrid, subjectiveFeelingFilter, new System.Collections.Generic.List<ListBox> {
                UserIdsFilterFeelingsTab,
                ExpeditionFilterFeelingsTab,
                NumberFilterFeelingsTab,
                ProfessionFilterFeelingsTab
            });
        }

        /// <summary>
        /// Filter data in SubjectiveFeeling grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterSubjectiveFeeling_Click(object sender, RoutedEventArgs e)
        {
            if (subjectiveFeelingFilter.DateFrom != null && subjectiveFeelingFilter.DateTo != null && (DateTime)subjectiveFeelingFilter.DateFrom > (DateTime)subjectiveFeelingFilter.DateTo)
            {
                MessageBox.Show(MessageBoxConstants.WrongDateFields);
            }
            else
            {
                filterBussinesManager.Filter(SubjectiveFeelingDataGrid, subjectiveFeelingFilter);
            }
        }

        private void UserIdsFilterFeelingsTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            subjectiveFeelingFilter.SetUserState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void ExpeditionFilterFeelingsTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            subjectiveFeelingFilter.SetExpeditionState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void NumberFilterFeelingsTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            subjectiveFeelingFilter.SetPeopleState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void ProfessionFilterFeelingsTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            subjectiveFeelingFilter.SetProfessionsState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }

        private void CheckBox_Selected(object sender, RoutedEventArgs e)
        {
            subjectiveFeelingFilter.IsFeeling = true;
        }

        private void FeelingsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            SetLoaderVisibility(Visibility.Visible);
            var currentFeeling = (SubjectiveFeeling)((DataGrid)(sender)).SelectedItem;
            try
            {
                var changedProperty = e.Column.SortMemberPath.ToString();
                var currentValue = currentFeeling.GetType().GetProperty(changedProperty).GetValue(currentFeeling).ToString();
                string newValue;
                if (e.EditingElement is CheckBox)
                {
                    newValue = ((CheckBox)e.EditingElement).IsChecked.ToString();
                }
                else
                {
                    newValue = ((TextBox)e.EditingElement).Text.ToString();
                }

                if (currentValue == newValue)
                {
                    SetLoaderVisibility(Visibility.Hidden);
                    return;
                }

                bool updated = false;
                var dialogResult = MessageBox.Show(MessageBoxConstants.UpdateSure, MessageBoxConstants.UpdateSureTitle, MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    updated = subjectiveFeelingBuissnesManager.TryUpdateFeeling(currentFeeling, changedProperty, newValue);
                }
                if (!updated)
                {
                    if (e.EditingElement is CheckBox)
                    {
                        ((CheckBox)e.EditingElement).IsChecked = bool.Parse(currentValue);
                    }
                    else if (e.EditingElement is TextBox)
                    {
                        ((TextBox)e.EditingElement).Text = currentValue;
                    }
                }

            }
            catch
            {
                MessageBox.Show(MessageBoxConstants.ErrorUpdating);
            }
            SetLoaderVisibility(Visibility.Hidden);
        }
    }
}
