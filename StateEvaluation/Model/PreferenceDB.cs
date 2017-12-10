using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

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
        public IEnumerable<string> Codes()
        {
            var items = this.Preference.Select(item => item.UserId).Distinct().OrderByDescending(item => item);
            return items;
        }
        public IEnumerable<Preference> FilterCodes(string s)
        {
            var items = this.Preference.Select(item => item).Where(item => item.UserId.StartsWith("Ex20")).OrderByDescending(item => item);
            return items;
        }

    }
}
