using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using StateEvaluation.Repository.Models;
using StateEvaluation.Common.Enums;
using StateEvaluation.Common.Constants;
using System.Configuration;

namespace StateEvaluation.Repository.Providers
{
    [Database]
    public class DataRepository : DataContext
    {
        public DataRepository() : base(ConfigurationManager.ConnectionStrings["StateEvaluation.Properties.Settings.PreferenceDBConnectionString"].ConnectionString) { }
        public Table<People> People;
        public Table<Preference> Preference;
        public Table<RelaxTable1> RelaxTable1;
        public Table<RelaxTable2> RelaxTable2;
        public Table<Color> Color;
        public Table<SubjectiveFeeling> SubjectiveFeeling;
        public Table<NormalPreference> NormalPreference;

        public void InsertPreference(Preference preference)
        {
            Preference.InsertOnSubmit(preference);
            SubmitChanges();
        }

        #region Preferences
        public Preference GeneratePreference(Guid id)
        {
            var preference = Preference.Single(item => item.Id == id);
            return preference;
        }

        public IEnumerable<Preference> GetPreferences()
        {
            var items = this.Preference.Select(item => item).OrderByDescending(item => item.Date);
            return items;
        }

        public void DeletePreference(string id)
        {
            var preference = Preference.Single(item => item.Id.ToString() == id);
            Preference.DeleteOnSubmit(preference);
            SubmitChanges();
        }

        public void UpdatePreference(Preference person)
        {
            var items = Preference.Single(item => item.Id == person.Id);
            items.Oder1 = person.Oder1;
            items.Oder2 = person.Oder2;
            items.Preference1 = person.Preference1;
            items.Preference2 = person.Preference2;
            items.RelaxTable1 = person.RelaxTable1;
            items.RelaxTable2 = person.RelaxTable2;
            items.ShortOder1 = person.ShortOder1;
            items.ShortOder2 = person.ShortOder2;
            items.Compare = person.Preference1 == person.Preference2 ? StringBooleanValues.True : StringBooleanValues.False;
            SubmitChanges();
        }

        public Preference GetPreference(Preference preference)
        {
            return Preference.Single(item => item == preference);
        }

        public IEnumerable<Preference> GetPreferences(Func<Preference, bool> query)
        {
            var preferences = this.Preference.Where(query);
            return preferences;
        }

        public IEnumerable<string> GetShortColorsNumbersList()
        {
            return PreferenceValues.ShortColorsNumbersList;
        }

        public IEnumerable<string> GetColorsNumbersList()
        {
            return PreferenceValues.ColorsNumbersList;
        }

        #endregion

        #region People
        public People GetPerson(string id)
        {
            var person = People.Single(item => item.Id.ToString() == id);
            return person;
        }
        public IEnumerable<People> GetPeople()
        {
            var people = this.People.Select(item => item).OrderBy(item => item.UserId);
            return people;
        }
        public People GetPersonByUserId(string userId)
        {
            var person = this.People.Select(item => item).Where(item => item.UserId == userId).Single();
            return person;
        }

        public void UpdatePerson(People person)
        {
            var items = this.People.Where(item => item.Id == person.Id).Single<People>();
            items.Id = person.Id;
            items.Firstname = person.Firstname;
            items.Lastname = person.Lastname;
            items.Middlename = person.Middlename;
            items.Birthday = person.Birthday;
            items.Workposition = person.Workposition;
            items.Expedition = person.Expedition;
            items.Number = person.Number;
            items.UserId = person.UserId;
            SubmitChanges();
        }

        public void CreatePerson(People person)
        {
            People.InsertOnSubmit(person);
            SubmitChanges();
        }

        public void Delete(string id)
        {
            var person = People.Single(item => item.Id.ToString() == id);
            People.DeleteOnSubmit(person);
            SubmitChanges();
        }

        public IEnumerable<string> UserIds()
        {
            var items = this.People.Select(item => item.UserId).Distinct().OrderByDescending(item => item);
            var list = items.ToList();
            return list;
        }

        public IEnumerable<People> GetPeople(Func<People, bool> query)
        {
            var person = this.People.Where(query);
            return person;
        }

        //refactor!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
        public IEnumerable<string> GetUserIds()
        {
            var list = UserIds().ToList();
            list.Insert(0, FilterConstants.All);
            return list;
        }

        public IEnumerable<string> Professions()
        {
            var items = this.People.Select(item => item.Workposition).Distinct().OrderByDescending(item => item);
            var list = items.ToList();
            list.Insert(0, FilterConstants.All);
            return list;
        }

        public IEnumerable<string> GetExpeditionCodes()
        {
            IEnumerable<string> items = this.People.Select(item => item.Expedition.ToString()).Distinct().OrderByDescending(item => Convert.ToInt32(item));
            List<string> list = items.ToList().ToList();

            list.Insert(0, FilterConstants.All);
            return list;
        }

        public IEnumerable<string> GetPeopleNumbers()
        {
            IEnumerable<string> items = this.People.Select(item => item.Number.ToString()).Distinct().OrderBy(item => Convert.ToInt32(item));
            List<string> list = items.ToList().ToList();

            list.Insert(0, FilterConstants.All);
            return list;
        }
        #endregion

        #region Subjective feeling
        public void RemoveSubjectiveFeeling(Guid subjectiveFeelingId)
        {
            var subjectiveFeeling = SubjectiveFeeling.Single(item => item.Id == subjectiveFeelingId);
            SubjectiveFeeling.DeleteOnSubmit(subjectiveFeeling);
            SubmitChanges();
        }

        public SubjectiveFeeling GetSubjectiveFeeling(Guid id)
        {
            var subjectiveFeeling = SubjectiveFeeling.Single(item => item.Id == id);
            return subjectiveFeeling;
        }

        public void CreateSubjectiveFeeling(SubjectiveFeeling sf)
        {
            SubjectiveFeeling.InsertOnSubmit(sf);
            SubmitChanges();
        }

        public void UpdateSubjectiveFeeling(SubjectiveFeeling newSubjectiveFeeling)
        {
            var subjectiveFeeling = SubjectiveFeeling.Single(item => item.Id == newSubjectiveFeeling.Id);
            subjectiveFeeling.BadMood = newSubjectiveFeeling.BadMood;
            subjectiveFeeling.Date = newSubjectiveFeeling.Date;
            subjectiveFeeling.GeneralWeaknes = newSubjectiveFeeling.GeneralWeaknes;
            subjectiveFeeling.HeavyHead = newSubjectiveFeeling.HeavyHead;
            subjectiveFeeling.PoorAppetite = newSubjectiveFeeling.PoorAppetite;
            subjectiveFeeling.SlowThink = newSubjectiveFeeling.SlowThink;
            subjectiveFeeling.UserId = newSubjectiveFeeling.UserId;
        }
        public IEnumerable<SubjectiveFeeling> GetSubjecriveFeelings()
        {
            var items = this.SubjectiveFeeling.Select(item => item).OrderBy(item => item.Date);
            return items;
        }

        public IEnumerable<SubjectiveFeeling> GetSubjecriveFeelings(Func<SubjectiveFeeling, bool> query)
        {
            var items = SubjectiveFeeling.Where(query);
            return items;
        }
        #endregion
    }
}
