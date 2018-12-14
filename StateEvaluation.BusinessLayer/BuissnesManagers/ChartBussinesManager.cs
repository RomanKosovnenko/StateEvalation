using StateEvaluation.Repository.Providers;
using StateEvaluation.Repository.Models;
using System;
using System.Collections.Generic;

namespace StateEvaluation.BussinesLayer.BuissnesManagers
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
