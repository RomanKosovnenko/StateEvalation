using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace StateEvaluation.Model
{
    [Database]
    public class PreferenceDB : DataContext
    {
        public PreferenceDB() : base("Data Source=mssql1.gear.host;Initial Catalog=PreferenceDB;User ID=preferencedb;Password=Qwe!23;Encrypt=True;TrustServerCertificate=True") { }
        public Table<People> People;
        public Table<Preference> Preference;
        public Table<RelaxTable1> RelaxTable1;
        public Table<RelaxTable2> RelaxTable2;
        public Table<Color> Color;
        public Table<SubjectiveFeeling> SubjectiveFeeling;
        public Table<DemboRubin> DemboRubin;
        public Table<Depresion> Depresion;
        public Table<NormalPreference> NormalPreference;
        public Table<ArterialPressuresInNight> ArterialPressuresInNight;
        public Table<JournalOfAppeal> JournalOfAppeal;
        public Table<Anthropometry> Anthropometry;
        
        public Table<ArterialPressuresInDay> ArterialPressuresInDay;
        public Table<ArterialPressuresGeneral> ArterialPressuresGeneral;
        public Table<CycleErgometry> CycleErgometry;
        public Table<CycleErgometryWithLoad> CycleErgometryWithLoad;
        public Table<BloodTest> BloodTest;

        public People GetPersonById(string id)
        {
            var person = People.Single(item => item.Id.ToString() == id);
            return person;
        }
        public IEnumerable<CycleErgometry> GetAllCycleErgometry()
        {
            var items = CycleErgometry.Select(item => item).OrderBy(item => item.UserId);
            return items;
        }
        public IEnumerable<string> GetAnthropometry()
        {
            var items = Anthropometry.Select(item => item.UserId);
            return items;
        }
        public void InsertInAnthropometry( Anthropometry anthropometry)
        {
            Anthropometry.InsertOnSubmit(anthropometry);
            SubmitChanges();
        }
        public IEnumerable<string> GetATNght()
        {
            var items = BloodTest.Select(item => item.UserId);
            return items;
        }
        public IEnumerable<People> GetAllPeople()
        {
            var items = this.People.Select(item => item).OrderBy(item => item.UserId);
            return items;
        }
        public People GetPerson(string tempUID)
        {
            var items = this.People.Select(item => item).Where(item => item.UserId == tempUID);
            People person = new People();
            person = items.Single();
            return person;
        }
        public void InsertEntityInSubjectiveFeeling(SubjectiveFeeling sf)
        {
            if (sf.Date.ToString() == "" || sf.UserId == null)
            {
                MessageBox.Show("Error! Some fields are empty");
            }
            else
            {
                SubjectiveFeeling.InsertOnSubmit(sf);
                SubmitChanges();
            }
        }
        public void InsertEntityInPeople(People person)
        {
            try
            {
                People.InsertOnSubmit(person);
                SubmitChanges();
            }
            catch(SqlException)
            {
                MessageBox.Show("Error, when inserting in data base! May be such an UserID already exists");
            }
        }
        public IEnumerable<Preference> GetAllTests()
        { //.Where(item => item.UserId.StartsWith("Ex20"))
            var items = this.Preference.Select(item => item).OrderByDescending(item => item.Date);
            return items;
        }
        public IEnumerable<Anthropometry> GetAllAnthropometry()
        {
            var items = Anthropometry.Select(item => item).OrderByDescending(item => item.Date);
            return items;
        }
            public IEnumerable<SubjectiveFeeling> GetAllSubjecriveFeelings()
        {
            var items = this.SubjectiveFeeling.Select(item => item).OrderBy(item => item.Date);
            return items;
        }
        public IEnumerable<string> CodesForFilter()
        {
            var list = CodesForInsert().ToList();
            list.Insert(0, "All");
            return list;
        }
        public IEnumerable<string> CodesForInsert()
        {
            var items = this.People.Select(item => item.UserId).Distinct().OrderByDescending(item => item);
            var list = items.ToList();
            return list;
        }
        public IEnumerable<string> Preferences()
        {
            var items = this.Preference.Select(item => item.Preference1).Distinct().OrderByDescending(item => item);
            var list = items.ToList();
            list.Insert(0, "All");
            return list;
        }
        public IEnumerable<string> Professions()
        {
            var items = this.People.Select(item => item.Workposition).Distinct().OrderByDescending(item => item);
            var list = items.ToList();
            list.Insert(0, "All");
            return list;
        }
        public IEnumerable<string> ExpeditionCodes()
        {
            IEnumerable<string> items = this.People.Select(item => item.Expedition.ToString()).Distinct().OrderByDescending(item => Convert.ToInt32(item));
            List<string> list = items.ToList().ToList();

            list.Insert(0, "All");
            return list;
        }
        
        public IEnumerable<string> PeopleCodes()
        {
            IEnumerable<string> items = this.People.Select(item => item.Number.ToString()).Distinct().OrderBy(item => Convert.ToInt32(item));
            List<string> list = items.ToList().ToList();

            list.Insert(0, "All");
            return list;
        }
        public void UpdateTestInPreference(Preference person)
        {
            var items = this.Preference.Where(item => item.Id == person.Id).Single<Preference>();
            items.Oder1 = person.Oder1;
            items.Oder2 = person.Oder2;
            items.Preference1 = person.Preference1;
            items.Preference2 = person.Preference2;
            items.RelaxTable1 = person.RelaxTable1;
            items.RelaxTable2 = person.RelaxTable2;
            items.ShortOder1 = person.ShortOder1;
            items.ShortOder2 = person.ShortOder2;
            items.Compare = person.Preference1 == person.Preference2 ? "true" : "false";
            SubmitChanges();
        }
        public void UpdateTestInPreference(People people)
        {
            var items = this.People.Where(item => item.Id == people.Id).Single<People>();
            items.Id = people.Id;
            items.Firstname = people.Firstname;
            items.Lastname = people.Lastname;
            items.Birthday = people.Birthday;
            items.Workposition = people.Workposition;
            items.Expedition = people.Expedition;
            items.Number = people.Number;
            items.UserId = people.UserId;
            SubmitChanges();
        }
        public void UpdateTestInPreference(SubjectiveFeeling feeling)
        {
            var items = this.SubjectiveFeeling.Where(item => item.Id == feeling.Id).Single<SubjectiveFeeling>();
            items.GeneralWeaknes = feeling.GeneralWeaknes;
            items.PoorAppetite = feeling.PoorAppetite;
            items.PoorSleep = feeling.PoorSleep;
            items.BadMood = feeling.BadMood;
            items.HeavyHead = feeling.HeavyHead;
            items.SlowThink = feeling.SlowThink;
            items.UserId = feeling.UserId;
            items.Date = feeling.Date;
            SubmitChanges();
        }
        public IEnumerable<string> ShortColorsNumbersList()
        {
            string[] list = { "3", "7", "11" };
            return list;
        }
        public IEnumerable<string> ColorsNumbersList()
        {
            string[] list = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            return list;
        }
        public IEnumerable<string> ShortColorsNumbersList(string x1, string x2)
        {
            string[] list = {x1, x2};
            return list;
        }
        public IEnumerable<string> ShortColorsNumbersList(string x1)
        {
            string[] list = { x1 };
            return list;
        }
    }
}
