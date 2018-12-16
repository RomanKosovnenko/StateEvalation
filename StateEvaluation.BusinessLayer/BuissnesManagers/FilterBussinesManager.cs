using StateEvaluation.BussinesLayer.Providers;
using StateEvaluation.Repository.Providers;
using System.Collections;
using System.Windows.Controls;

namespace StateEvaluation.BussinesLayer.BuissnesManagers
{
    public class FilterBussinesManager
    {
        private FilterProvider _filterProvider;

        public FilterBussinesManager(DataRepository dataRepository)
        {
            _filterProvider = new FilterProvider(dataRepository);
        }

        public IEnumerable Filter(DataGrid dataGrid, object filter)
        {
            var filteredData = _filterProvider.Filter(filter);
            dataGrid.ItemsSource = filteredData;
            return filteredData;
        }

        public void Clear(DataGrid dataGrid, object filter)
        {
            _filterProvider.Clear(filter);
            var filteredData = _filterProvider.Filter(filter);
            dataGrid.ItemsSource = filteredData;
        }
    }
}
