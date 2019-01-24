using StateEvaluation.BussinesLayer.Extensions;
using StateEvaluation.Common.Constants;
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
        private DataRepository _dataRepository = new DataRepository();

        private IEnumerable<ListBox> _userIdListBoxes { get; }
        private IEnumerable<ComboBox> _userIdComboBoxes { get; }
        private IEnumerable<ListBox> _expeditionComboBoxes { get; }
        private IEnumerable<ListBox> _userNumberComboBoxes { get; }
        private IEnumerable<ListBox> _professionsComboBoxes { get; }
        private DataGrid _peopleDataGrid { get; }
        private Button _updatePersonBtn { get; }

        public PeopleBuissnesManager(
            IEnumerable<ListBox> userIdListBoxes,
            IEnumerable<ComboBox> userIdComboBoxes,
            IEnumerable<ListBox> expeditionListBoxes,
            IEnumerable<ListBox> userNumberListBoxes,
            IEnumerable<ListBox> professionsListBoxes,
            DataGrid peopleDataGrid,
            Button updatePersonBtn)
        {
            _userIdListBoxes = userIdListBoxes;
            _userIdComboBoxes = userIdComboBoxes;
            _expeditionComboBoxes = expeditionListBoxes;
            _userNumberComboBoxes = userNumberListBoxes;
            _peopleDataGrid = peopleDataGrid;
            _updatePersonBtn = updatePersonBtn;
            _professionsComboBoxes = professionsListBoxes;
        }

        public Dictionary<string, string> GetUserIdBirthPairs()
        {
           return _dataRepository.GetPeople()
               .Select(item => new { UserId = item.UserId.ToString(), item.Birthday })
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
                MessageBox.Show(MessageBoxConstants.WrongFields);
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

                    MessageBox.Show(MessageBoxConstants.PersonCreated);
                }
                catch
                {
                    MessageBox.Show(MessageBoxConstants.ErrorPersonCreate);
                }
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
                MessageBox.Show(MessageBoxConstants.WrongFields);
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

                MessageBox.Show(MessageBoxConstants.PersonUpdated);
            }
            catch
            {
                MessageBox.Show(MessageBoxConstants.ErrorUpdating);
            }
        }

        public bool TryUpdatePerson(People person, string changedProperty, string newValue)
        {
            if (
               string.IsNullOrEmpty(person.Firstname) ||
               string.IsNullOrEmpty(person.Middlename) ||
               string.IsNullOrEmpty(person.Lastname) ||
               !DateTime.TryParse(person.Birthday.ToString(), out DateTime birthday) ||
               string.IsNullOrEmpty(person.Workposition)
               )
            {
                MessageBox.Show(MessageBoxConstants.WrongFields);
                return false;
            }
            try
            {

                if (changedProperty == "Birthday")
                {
                    if (DateTime.TryParse(newValue.ToString(), out DateTime result))
                    {
                        newValue = result.ToShortDateString();
                    }
                    else
                    {
                        MessageBox.Show(MessageBoxConstants.WrongFields);
                        return false;
                    }
                }
                if (changedProperty == "Expedition" || changedProperty == "Number")
                {
                    if (int.TryParse(newValue, out int result))
                    {
                        person.GetType().GetProperty(changedProperty).SetValue(person, result);
                    }
                    else
                    {
                        MessageBox.Show(MessageBoxConstants.WrongFields);
                        return false;
                    }
                    // UserId cannot be changed due to DataBase references
                    // person.UserId = "Ex" + person.Expedition + "#" + person.Number;
                }
                else
                {
                    person.GetType().GetProperty(changedProperty).SetValue(person, newValue);
                }
  
                _dataRepository.UpdatePerson(person);
                Refresh();

                MessageBox.Show(MessageBoxConstants.PersonUpdated);
                return true;
            }
            catch
            {
                MessageBox.Show(MessageBoxConstants.ErrorUpdating);
                return false;
            }
        }

        public void DeletePerson(string id)
        {
            try
            {
                var person = _dataRepository.GetPerson(id);
                var relatedPrefrences = _dataRepository.Preference.Where(_ => _.UserId == person.UserId);
                var relatedSubjFeeling = _dataRepository.SubjectiveFeelings.Where(_ => _.UserId == person.UserId);

                if(!(relatedPrefrences.Any() || relatedSubjFeeling.Any()))
                {
                    _dataRepository.DeletePerson(id);
                    Refresh();
                    MessageBox.Show(MessageBoxConstants.PersonDeleted);
                    return;
                }
            }
            catch
            {
                MessageBox.Show(MessageBoxConstants.ErrorPersonDelete);
                return;
            }
            MessageBox.Show(MessageBoxConstants.ErrorPersonDelete);
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
                MessageBox.Show(MessageBoxConstants.ErrorBinding);
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
                combobox.ItemsSource = _dataRepository.GetProfessionsFilter();
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
                comboBox.ItemsSource = _dataRepository.GetPeopleNumbersFilter();
            }
        }

        private void RefreshExpedition()
        {
            foreach (var comboBox in _expeditionComboBoxes)
            {
                comboBox.ItemsSource = _dataRepository.GetExpeditionCodesFilter();
            }
        }

        private void RefreshUserIds()
        {
            foreach(var comboBox in _userIdComboBoxes)
            {
                comboBox.ItemsSource = _dataRepository.GetUserIdsFilter();
            }
            foreach(var listBox in _userIdListBoxes)
            {
                listBox.ItemsSource = _dataRepository.GetUserIdsFilter();
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
            if (person.UserId.Length > 10)
            {
                MessageBox.Show(MessageBoxConstants.ErrorPersonUserID);
                return false;
            }
            else if (person.Lastname.Length > 20)
            {
                MessageBox.Show(MessageBoxConstants.ErrorPersonLastname);
                return false;
            }
            else if (person.Firstname.Length > 20)
            {
                MessageBox.Show(MessageBoxConstants.ErrorPersonFirstname);
                return false;
            }
            else if (person.Middlename.Length > 20)
            {
                MessageBox.Show(MessageBoxConstants.ErrorPersonMiddlename);
                return false;
            }
            else if (person.Workposition.Length > 20)
            {
                MessageBox.Show(MessageBoxConstants.ErrorPersonWorkposition);
                return false;
            }

            return true;
        }

        private void SetValueInTabs(People person, PeopleVM personVM)
        {
            personVM.Id = person.Id.ToString();
            personVM.FirstName = person.Firstname;
            personVM.LastName = person.Lastname;
            personVM.MiddleName = person.Middlename;
            personVM.Birthday = string.IsNullOrEmpty(person.Birthday) ? new DateTime() : DateTime.Parse(person.Birthday);
            personVM.Workposition = person.Workposition;
            personVM.Expedition = person.Expedition.ToString();
            personVM.PersonNumber = person.Number.ToString();
        }
        #endregion
    }
}
