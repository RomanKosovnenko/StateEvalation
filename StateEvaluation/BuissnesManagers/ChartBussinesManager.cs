using StateEvaluation.Model;
using StateEvaluation.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.BuissnesManagers
{
    public class ChartBussinesManager
    {
        public PreferenceDB _preferenceDB = new PreferenceDB();

        public void BuildChart(IEnumerable<Preference> preferences, DateTime? dateFrom, DateTime? dateTo)
        {
            //var subWindow = new TestsChart(GetUIDs().OrderBy(x => x.Date).ToList(), true, dateFrom != null && dateFrom == dateTo);
        }
    }
}
