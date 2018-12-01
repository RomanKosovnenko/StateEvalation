using StateEvaluation.Enums;
using StateEvaluation.Extensions;
using StateEvaluation.Helpers;
using StateEvaluation.Model;
using StateEvaluation.ViewModel;
using System;
using System.Collections.Generic;
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
            }
            People person = GetNewPerson(newPerson);

            if (CheckFildAddedPersonOnLenght(person))
            {
                try
                {
                    _preferenceDb.CreatePerson(person);
                    ClearSelectedInPeople(newPerson);
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
                People person = GetNewPerson(editedPerson);
                person.Id = new Guid(editedPerson.Id);
                _preferenceDb.UpdatePerson(person);
                ClearSelectedInPeople(editedPerson);

                //hide save person button
                UpdatePersonBtn.Visibility = Visibility.Hidden;
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

        public void PrepareInputForm(PeopleVM personDto, Guid id)
        {
            try
            {
                var person = _preferenceDb.GetPerson(id.ToString());
                SetValueInTabs(person, personDto);

                //show save person button
                UpdatePersonBtn.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("Oops, error while binding input filds");
            }
        }

        #region private methods
        private People GetNewPerson(PeopleVM personDto)
        {
            People person = new People()
            {
                Firstname = personDto.FirstName,
                Lastname = personDto.LastName,
                Expedition = int.Parse(personDto.Expedition),
                Number = int.Parse(personDto.PersonNumber),
                Id = Guid.NewGuid(),
                Workposition = personDto.Workposition,
                Birthday = personDto.Birthday.ToString().GetDateFromDateTimeString()
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

        private void ClearSelectedInPeople(PeopleVM personDto)
        {
            personDto.Id = string.Empty;
            personDto.FirstName = string.Empty;
            personDto.LastName = string.Empty;
            personDto.MiddleName = string.Empty;
            personDto.Birthday = string.Empty;
            personDto.Workposition = string.Empty;
            personDto.Expedition = string.Empty;
            personDto.PersonNumber = string.Empty;
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

        private void SetValueInTabs(People person, PeopleVM personDto)
        {
            personDto.Id = person.Id.ToString();
            personDto.FirstName = person.Firstname;
            personDto.LastName = person.Lastname;
            personDto.MiddleName = person.Middlename;
            personDto.Birthday = DateTime.Parse(person.Birthday);
            personDto.Workposition = person.Workposition;
            personDto.Expedition = person.Expedition.ToString().Trim();
            personDto.PersonNumber = person.Number.ToString().Trim();
        }
        #endregion
    }
}
