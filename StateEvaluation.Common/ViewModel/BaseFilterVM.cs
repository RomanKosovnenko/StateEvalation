
using System.Collections.Generic;

namespace StateEvaluation.Common.ViewModel
{
    public class BaseFilterVM : BaseVM
    {
        private List<string> _userIds;
        private object _dateFrom;
        private object _dateTo;
        private List<string> _expeditions;
        private List<string> _people;
        private List<string> _professions;
        
        public List<string> UserIds
        {
            get => _userIds;
            set
            {
                _userIds = value;
                OnPropertyChanged("UserIds");
            }
        }

        public object DateFrom
        {
            get => _dateFrom;
            set
            {
                _dateFrom = value;
                OnPropertyChanged("DateFrom");
            }
        }
        public object DateTo
        {
            get => _dateTo;
            set
            {
                _dateTo = value;
                OnPropertyChanged("DateTo");
            }
        }
        public List<string> Expeditions
        {
            get => _expeditions;
            set
            {
                _expeditions = value;
                OnPropertyChanged("Expeditions");
            }
        }
        public List<string> People
        {
            get => _people;
            set
            {
                _people = value;
                OnPropertyChanged("People");
            }
        }
        public List<string> Professions
        {
            get => _professions;
            set
            {
                _professions = value;
                OnPropertyChanged("Professions");
            }
        }
    }
}
