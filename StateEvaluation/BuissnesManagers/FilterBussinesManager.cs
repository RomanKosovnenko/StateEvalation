using StateEvaluation.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StateEvaluation.BuissnesManagers
{
    public class FilterBussinesManager
    {
        private FilterProvider filterProvider = new FilterProvider();

        public IEnumerable Filter(DataGrid dataGrid, object filter, int tabControl)
        {
            return filterProvider.Filter(dataGrid, filter, tabControl);
        }

        public void Clear(object filter)
        {
            filterProvider.Clear(filter);
        }
    }
}
