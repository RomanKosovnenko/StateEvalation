using StateEvaluation.Extensions;
using StateEvaluation.Helpers;
using StateEvaluation.Model;
using StateEvaluation.ViewModel.PeopleDataGrid;
using System;
using System.Windows;

namespace StateEvaluation.Providers
{
    public class PeopleDataGridProvider
    {
        private PreferenceDB _preferenceDb = new PreferenceDB();

        public PeopleDataGridProvider() { }

        public string CreatePerson(PeopleDto newPerson)
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
                return string.Empty;
            }
            People person = GetNewPerson(newPerson);

            if (CheckFildAddedPersonOnLenght(person))
            {
                try
                {
                    _preferenceDb.InsertEntityInPeople(person);
                    ClearSelectedInPeople(newPerson);
                    return person.Id.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        private People GetNewPerson(PeopleDto personDto)
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

        private void ClearSelectedInPeople(PeopleDto personDto)
        {
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
            else if (person.Workposition.Length > 21)
            {
                MessageBox.Show("Error! The Workposition must be no more then 20 symbols");
                return false;
            }

            return true;
        }

        public void UpdatePerson(PeopleDto editedPerson, Guid id)
        {
            try
            {
                People person = GetNewPerson(editedPerson);
                person.Id = id;
                _preferenceDb.UpdateTestInPreference(person);
                ClearSelectedInPeople(editedPerson);
            }
            catch { }
        }

        public void PrepareUpdate(PeopleDto personDto, string id)
        {
            var person = _preferenceDb.GetPersonById(id);
            SetValueInTabs(person, personDto);
        }
        private void SetValueInTabs(People person, PeopleDto personDto)
        {
            personDto.FirstName = person.Firstname;
            personDto.LastName = person.Lastname;
            personDto.MiddleName = person.Middlename;
            personDto.Birthday = person.Birthday;
            personDto.Workposition = person.Workposition;
            personDto.Expedition = person.Expedition.ToString().Trim();
            personDto.PersonNumber = person.Number.ToString().Trim();
        }
    }
}
