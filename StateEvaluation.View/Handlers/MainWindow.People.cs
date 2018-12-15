using StateEvaluation.Common.ViewModel;
using StateEvaluation.Repository.Models;
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

        /// <summary>
        /// Clear filters for people grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilterPeopleTab(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Clear(PeopleDataGrid, peopleFilter);
        }

        /// <summary>
        /// Filter data int People grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterPeople(object sender, RoutedEventArgs e)
        {
            filterBussinesManager.Filter(PeopleDataGrid, peopleFilter);
        }
    }
}
