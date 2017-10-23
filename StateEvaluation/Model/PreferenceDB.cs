using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace StateEvaluation.Model
{
    [Database]
    public class PreferenceDB : DataContext
    {
        public PreferenceDB() : base("Data Source=roman-kosovnenko-sql1.database.windows.net;Initial Catalog=PreferenceDB;User ID=roma.kosovnenko;Password=80962275494Kr18;Encrypt=True;TrustServerCertificate=False") { }
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
        {
            var items = this.Preference.Select(item => item).OrderBy(item => item.Date);
            return items;
        }

        public IEnumerable<SubjectiveFeeling> GetAllSubjecriveFeelings()
        {
            var items = this.SubjectiveFeeling.Select(item => item).OrderBy(item => item.Date);
            return items;
        }
    }
}
