using StateEvaluation.Common.Constants;
using StateEvaluation.Common.ViewModel;
using StateEvaluation.Repository.Models;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace StateEvaluation
{
    /// <summary>
    /// Interaction logic for People tab
    /// </summary>
    partial class MainWindow : Window
    {
        /// <summary>
        /// Create new person from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void CreatePerson_Click(object sender, RoutedEventArgs e)
        {
            var newPerson = (PeopleVM)Resources["peopleVM"];
            peopleBuissnesManager.CreatePerson(newPerson);
            FilterPeople_Click(sender, e);
        }

        /// <summary>
        /// Update person from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void UpdatePerson_Click(object sender, RoutedEventArgs e)
        {
            var editedPerson = (PeopleVM)Resources["peopleVM"];
            peopleBuissnesManager.UpdatePerson(editedPerson);
        }

        /// <summary>
        /// Clear input fields from interface
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void ClearInputs_Click(object sender, RoutedEventArgs e)
        {
            var person = (PeopleVM)Resources["peopleVM"];
            peopleBuissnesManager.ClearInputs(person);
        }

        /// <summary>
        /// Bind person data into input fields 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event arguments</param>
        private void BindPersonInForm_Click(object sender, RoutedEventArgs e)
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
        private void RemovePerson_Click(object sender, RoutedEventArgs e)
        {
            SetLoaderVisibility(Visibility.Visible);
            var butonContext = ((Button)sender).DataContext;
            var personId = ((People)butonContext).Id;

            var dialogResult = MessageBox.Show(MessageBoxConstants.DeleteSure, MessageBoxConstants.DeleteSureTitle, MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                peopleBuissnesManager.DeletePerson(personId.ToString());
            }
            SetLoaderVisibility(Visibility.Hidden);
        }

        /// <summary>
        /// Clear filters for people grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilterPeopleTab_Click(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Clear(PeopleDataGrid, peopleFilter, new System.Collections.Generic.List<ListBox> {
                UserIdsFilterPeopleTab,
                ExpeditionFilterPeopleTab,
                NumberFilterPeopleTab,
                ProfessionFilterPeopleTab
            });
        }

        /// <summary>
        /// Filter data int People grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterPeople_Click(object sender, RoutedEventArgs e)
        {
            SetLoaderVisibility(Visibility.Visible);
            if (peopleFilter.DateFrom != null && peopleFilter.DateTo != null && (DateTime)peopleFilter.DateFrom > (DateTime)peopleFilter.DateTo)
            {
                MessageBox.Show(MessageBoxConstants.WrongDateFields);
            }
            else
            {
                filterBussinesManager.Filter(PeopleDataGrid, peopleFilter);
            }
            SetLoaderVisibility(Visibility.Hidden);
        }

        private void UserIdsFilterPeopleTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            peopleFilter.SetUserState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void ExpeditionFilterPeopleTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            peopleFilter.SetExpeditionState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void NumberFilterPeopleTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            peopleFilter.SetPeopleState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }
        private void ProfessionFilterPeopleTab_Selected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            peopleFilter.SetProfessionsState(checkBox.Content.ToString(), (bool)checkBox.IsChecked);
        }

        /// <summary>
        /// Quick edit person by field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PeopleDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var currentPeople = (People)((DataGrid)(sender)).SelectedItem;
            var changedProperty = e.Column.SortMemberPath.ToString();
            var newValue = ((TextBox)e.EditingElement).Text.ToString();
            peopleBuissnesManager.UpdatePerson(currentPeople, changedProperty, newValue);
        }
    }
}
