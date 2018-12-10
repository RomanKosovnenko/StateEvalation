using StateEvaluation.Enums;
using StateEvaluation.Extensions;
using StateEvaluation.Helpers;
using StateEvaluation.Model;
using StateEvaluation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace StateEvaluation.Providers
{
    public class PeopleBuissnesManager
    {
        private PreferenceDB _preferenceDb = new PreferenceDB();

        public List<ComboBox> UserIdComboBoxes { get; }
        public List<ComboBox> ExpeditionComboBoxes { get; }
        public List<ComboBox> UserNumberComboBoxes { get; }
        public DataGrid PeopleDataGrid { get; }
        public Button UpdatePersonBtn { get; }

        public PeopleBuissnesManager(List<ComboBox> userIdComboBoxes, List<ComboBox> expeditionComboBoxes, List<ComboBox> userNumberComboBoxes, DataGrid peopleDataGrid, Button updatePersonBtn)
        {
            UserIdComboBoxes = userIdComboBoxes;
            ExpeditionComboBoxes = expeditionComboBoxes;
            UserNumberComboBoxes = userNumberComboBoxes;
            PeopleDataGrid = peopleDataGrid;
            UpdatePersonBtn = updatePersonBtn;
        }

        public void CreatePerson(PeopleVM newPerson)
        {
            if (
               string.IsNullOrEmpty(newPerson.FirstName) ||
               string.IsNullOrEmpty(newPerson.LastName) ||
               !DateTime.TryParse(newPerson.Birthday.ToString(), out DateTime birthday) ||
               string.IsNullOrEmpty(newPerson.Workposition) ||
               !int.TryParse(newPerson.Expedition, out int expedition) ||
               !int.TryParse(newPerson.PersonNumber, out int personNumber)
               )
            {
                MessageBox.Show("Error! Try edit fields in form!");
                return;
            }
            People person = GeneratePerson(newPerson);

            if (CheckFildAddedPersonOnLenght(person))
            {
                try
                {
                    _preferenceDb.CreatePerson(person);
                    ClearInputsInternal(newPerson);
                    Refresh();

                    MessageBox.Show("Peson was created");
                }
                catch
                {
                    MessageBox.Show("Oops, error while creating person");
                }
            }
            else
            {
                MessageBox.Show("Oops, error while creating person");
            }
        }

        public void UpdatePerson(PeopleVM editedPerson)
        {
            try
            {
                People person = GeneratePerson(editedPerson, new Guid(editedPerson.Id));
                _preferenceDb.UpdatePerson(person);
                ClearInputsInternal(editedPerson);

                //hide save person button
                ToggleButton(UpdatePersonBtn, Visibility.Hidden);
                Refresh();

                MessageBox.Show("Person was updated");
            }
            catch
            {
                MessageBox.Show("Oops, error while updating");
            }
        }

        public void DeletePerson(string id)
        {
            try
            {
                _preferenceDb.Delete(id);
                Refresh();
            }
            catch
            {
                MessageBox.Show("Oops. You can not delete this record because it has associated entries in other tables.");
            }
        }

        public void PrepareInputForm(PeopleVM personVM, Guid id)
        {
            try
            {
                var person = _preferenceDb.GetPerson(id.ToString());
                SetValueInTabs(person, personVM);

                //show save person button
                ToggleButton(UpdatePersonBtn, Visibility.Visible);
            }
            catch
            {
                MessageBox.Show("Oops, error while binding input filds");
            }
        }

        public void ClearInputs(PeopleVM personVM)
        {
            ClearInputsInternal(personVM);

            //hide save person button
            ToggleButton(UpdatePersonBtn, Visibility.Hidden);
        }

        #region private methods

        private void ToggleButton(Button button, Visibility visibility)
        {
            button.Visibility = visibility;
        }

        private People GeneratePerson(PeopleVM personVM, Guid? id = null)
        {
            People person = new People()
            {
                Firstname = personVM.FirstName,
                Lastname = personVM.LastName,
                Expedition = int.Parse(personVM.Expedition),
                Number = int.Parse(personVM.PersonNumber),
                Id = id ?? Guid.NewGuid(),
                Workposition = personVM.Workposition,
                Birthday = personVM.Birthday.ToString().GetDateFromDateTimeString()
            };
            person.UserId = UserIdBuilder.Build(person.Expedition, person.Number);
            return person;
        }

        private void Refresh()
        {
            RefreshUserIds();
            RefreshExpedition();
            RefreshUsersNumber();
            RefreshPeople();
        }

        private void RefreshPeople()
        {
            PeopleDataGrid.ItemsSource = _preferenceDb.GetPeople();
        }

        private void RefreshUsersNumber()
        {
            foreach (var comboBox in UserNumberComboBoxes)
            {
                comboBox.ItemsSource = _preferenceDb.GetPeopleNumbers();
            }
        }

        private void RefreshExpedition()
        {
            foreach (var comboBox in ExpeditionComboBoxes)
            {
                comboBox.ItemsSource = _preferenceDb.GetExpeditionCodes();
            }
        }

        private void RefreshUserIds()
        {
            foreach(var comboBox in UserIdComboBoxes)
            {
                comboBox.ItemsSource = _preferenceDb.GetUserIds();
            }
        }

        private void ClearInputsInternal(PeopleVM personVM)
        {
            personVM.Id = string.Empty;
            personVM.FirstName = string.Empty;
            personVM.LastName = string.Empty;
            personVM.MiddleName = string.Empty;
            personVM.Birthday = string.Empty;
            personVM.Workposition = string.Empty;
            personVM.Expedition = string.Empty;
            personVM.PersonNumber = string.Empty;
        }

        private bool CheckFildAddedPersonOnLenght(People person)
        {
            if (person.UserId.Length > 11)
            {
                MessageBox.Show("Error! The Number and Expedition in the amount must be no more then 7");
                return BooleanValues.False;
            }
            else if (person.Lastname.Length > 21)
            {
                MessageBox.Show("Error! The Lastname must be no more then 20 symbols");
                return BooleanValues.False;
            }
            else if (person.Firstname.Length > 21)
            {
                MessageBox.Show("Error! The Firstname must be no more then 20 symbols");
                return BooleanValues.False;
            }
            else if (person.Workposition.Length > 21)
            {
                MessageBox.Show("Error! The Workposition must be no more then 20 symbols");
                return BooleanValues.False;
            }

            return BooleanValues.True;
        }

        private void SetValueInTabs(People person, PeopleVM personVM)
        {
            personVM.Id = person.Id.ToString();
            personVM.FirstName = person.Firstname;
            personVM.LastName = person.Lastname;
            personVM.MiddleName = person.Middlename;
            personVM.Birthday = string.IsNullOrEmpty(person.Birthday.Trim()) ? new DateTime() : DateTime.Parse(person.Birthday);
            personVM.Workposition = person.Workposition;
            personVM.Expedition = person.Expedition.ToString().Trim();
            personVM.PersonNumber = person.Number.ToString().Trim();
        }
        #endregion
    }
}
