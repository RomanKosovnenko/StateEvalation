using System.Data.Linq;
using System.Data.Linq.Mapping;
using StateEvaluation.Repository.Models;
using System.Configuration;

namespace StateEvaluation.Repository.Providers
{
    [Database]
    public partial class DataRepository : DataContext
    {
        public DataRepository() : base(ConfigurationManager.ConnectionStrings["StateEvaluation.Properties.Settings.PreferenceDBConnectionString"].ConnectionString) { }
        public Table<People> People;
        public Table<Preference> Preference;
        public Table<RelaxTable1> RelaxTable1;
        public Table<RelaxTable2> RelaxTable2;
        public Table<Color> Color;
        public Table<SubjectiveFeeling> SubjectiveFeeling;
        public Table<NormalPreference> NormalPreference;
    }
}
