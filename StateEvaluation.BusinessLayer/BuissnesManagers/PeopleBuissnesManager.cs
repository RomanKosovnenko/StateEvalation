using StateEvaluation.BussinesLayer.Extensions;
using StateEvaluation.Common.Helpers;
using StateEvaluation.Common.ViewModel;
using StateEvaluation.Repository.Models;
using StateEvaluation.Repository.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StateEvaluation.BussinesLayer.BuissnesManagers
{
    public class PeopleBuissnesManager
    {
        private DataRepository _dataRepository;

        private IEnumerable<ComboBox> _userIdComboBoxes { get; }
        private IEnumerable<ComboBox> _expeditionComboBoxes { get; }
        private IEnumerable<ComboBox> _userNumberComboBoxes { get; }
        private IEnumerable<ComboBox> _professionsComboBoxes { get; }
        private DataGrid _peopleDataGrid { get; }
        private Button _updatePersonBtn { get; }

        public PeopleBuissnesManager(
            DataRepository dataRepository, 
            IEnumerable<ComboBox> userIdComboBoxes,
            IEnumerable<ComboBox> expeditionComboBoxes,
            IEnumerable<ComboBox> userNumberComboBoxes,
            IEnumerable<ComboBox> professionsComboBoxes,
            DataGrid peopleDataGrid,
            Button updatePersonBtn)
        {
            _dataRepository = dataRepository;
            _userIdComboBoxes = userIdComboBoxes;
            _expeditionComboBoxes = expeditionComboBoxes;
            _userNumberComboBoxes = userNumberComboBoxes;
            _peopleDataGrid = peopleDataGrid;
            _updatePersonBtn = updatePersonBtn;
            _professionsComboBoxes = professionsComboBoxes;
        }

        public Dictionary<string, string> GetUserIdBirthPairs()
        {
           return _dataRepository.GetPeople()
               .Select(item => new { UserId = item.UserId.ToString().Trim(), item.Birthday })
               .ToDictionary(i => i.UserId, i => i.Birthday);
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
                    _dataRepository.CreatePerson(person);
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
            if (
               string.IsNullOrEmpty(editedPerson.FirstName) ||
               string.IsNullOrEmpty(editedPerson.MiddleName) ||
               string.IsNullOrEmpty(editedPerson.LastName) ||
               !DateTime.TryParse(editedPerson.Birthday.ToString(), out DateTime birthday) ||
               string.IsNullOrEmpty(editedPerson.Workposition) ||
               !int.TryParse(editedPerson.Expedition, out int expedition) ||
               !int.TryParse(editedPerson.PersonNumber, out int personNumber)
               )
            {
                MessageBox.Show("Error! Try edit fields in form!");
                return;
            }
            try
            {
                People person = GeneratePerson(editedPerson, new Guid(editedPerson.Id));
                _dataRepository.UpdatePerson(person);
                ClearInputsInternal(editedPerson);

                //hide save person button
                ToggleButton(_updatePersonBtn, Visibility.Hidden);
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
                _dataRepository.Delete(id);
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
                var person = _dataRepository.GetPerson(id.ToString());
                SetValueInTabs(person, personVM);

                //show save person button
                ToggleButton(_updatePersonBtn, Visibility.Visible);
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
            ToggleButton(_updatePersonBtn, Visibility.Hidden);
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
                Middlename = personVM.MiddleName,
                Expedition = int.Parse(personVM.Expedition),
                Number = int.Parse(personVM.PersonNumber),
                Id = id ?? Guid.NewGuid(),
                Workposition = personVM.Workposition,
                Birthday = personVM.Birthday.ToString().GetDateFromDateTimeString()
            };
            person.UserId = UserIdBuilder.Build(int.Parse(personVM.Expedition), int.Parse(personVM.PersonNumber));
            return person;
        }

        private void Refresh()
        {
            RefreshUserIds();
            RefreshExpedition();
            RefreshUsersNumber();
            RefreshPeople();
            RefreshProfessions();
        }

        private void RefreshProfessions()
        {
            foreach (var combobox in _professionsComboBoxes)
            {
                combobox.ItemsSource = _dataRepository.Professions();
            }
        }

        private void RefreshPeople()
        {
            _peopleDataGrid.ItemsSource = _dataRepository.GetPeople();
        }

        private void RefreshUsersNumber()
        {
            foreach (var comboBox in _userNumberComboBoxes)
            {
                comboBox.ItemsSource = _dataRepository.GetPeopleNumbers();
            }
        }

        private void RefreshExpedition()
        {
            foreach (var comboBox in _expeditionComboBoxes)
            {
                comboBox.ItemsSource = _dataRepository.GetExpeditionCodes();
            }
        }

        private void RefreshUserIds()
        {
            foreach(var comboBox in _userIdComboBoxes)
            {
                comboBox.ItemsSource = _dataRepository.GetUserIds();
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
                return false;
            }
            else if (person.Lastname.Length > 21)
            {
                MessageBox.Show("Error! The Lastname must be no more then 20 symbols");
                return false;
            }
            else if (person.Firstname.Length > 21)
            {
                MessageBox.Show("Error! The Firstname must be no more then 20 symbols");
                return false;
            }
            else if (person.Middlename.Length > 21)
            {
                MessageBox.Show("Error! The Middlename must be no more then 20 symbols");
                return false;
            }
            else if (person.Workposition.Length > 21)
            {
                MessageBox.Show("Error! The Workposition must be no more then 20 symbols");
                return false;
            }

            return true;
        }

        private void SetValueInTabs(People person, PeopleVM personVM)
        {
            personVM.Id = person.Id.ToString();
            personVM.FirstName = person.Firstname?.Trim();
            personVM.LastName = person.Lastname?.Trim();
            personVM.MiddleName = person.Middlename?.Trim();
            personVM.Birthday = string.IsNullOrEmpty(person.Birthday.Trim()) ? new DateTime() : DateTime.Parse(person.Birthday);
            personVM.Workposition = person.Workposition?.Trim();
            personVM.Expedition = person.Expedition.ToString().Trim();
            personVM.PersonNumber = person.Number.ToString().Trim();
        }
        #endregion
    }
}
