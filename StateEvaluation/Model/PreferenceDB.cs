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
            try
            {
                SubjectiveFeeling.InsertOnSubmit(sf);
                SubmitChanges();
            }
            catch (SqlException)
            {
                MessageBox.Show("Error!Try edit fields in form with less date!");
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
                MessageBox.Show("Error!Try edit fields in form with less date!");
            }
        }
        public IEnumerable<Preference> GetAllTests()
        { //.Where(item => item.UserId.StartsWith("Ex20"))
            var items = this.Preference.Select(item => item).OrderByDescending(item => item.Date);
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
        public IEnumerable<string> ExpeditionCodes()
        {
            var regex = new System.Text.RegularExpressions.Regex(@"Ex([0-9]+)#([0-9]+)");
            var items = this.People.Select(item => item.UserId).Distinct().OrderByDescending(item => item).Select(x => regex.Match(x.ToString()).Groups[1].Value);
            var list = items.ToList().Distinct().ToList();

            list.Insert(0, "All");
            return list;
        }
        
        public IEnumerable<string> PeopleCodes()
        {
            var regex = new System.Text.RegularExpressions.Regex(@"Ex([0-9]+)#([0-9]+)");
            var items = this.People.Select(item => item.UserId).OrderByDescending(item => item).Select(x => regex.Match(x.ToString()).Groups[2].Value);
            var list = items.ToList().Distinct().ToList();

            list.Insert(0, "All");
            return list;
        }
        public IEnumerable<string> ShortColorsNumbersList()
        {
            string[] list = { "3", "7", "11" };
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
