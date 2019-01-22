using StateEvaluation.BussinesLayer.Providers;
using StateEvaluation.Repository.Providers;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace StateEvaluation.BussinesLayer.BuissnesManagers
{
    public class FilterBussinesManager
    {
        private FilterProvider _filterProvider;
        private DataRepository _dataRepository = new DataRepository();

        public FilterBussinesManager()
        {
            _filterProvider = new FilterProvider(_dataRepository);
        }

        public IEnumerable Filter(DataGrid dataGrid, object filter)
        {
            var filteredData = _filterProvider.Filter(filter);
            dataGrid.ItemsSource = filteredData;
            return filteredData;
        }

        public void Clear(DataGrid dataGrid, object filter, List<ListBox> listBoxes)
        {
            _filterProvider.Clear(filter, listBoxes);
            var filteredData = _filterProvider.Filter(filter);
            dataGrid.ItemsSource = filteredData;
        }
    }
}
