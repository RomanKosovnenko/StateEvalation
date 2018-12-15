
namespace StateEvaluation.Common.ViewModel
{
    public class BaseFilterVM : BaseVM
    {
        private string _userId;
        private object _dateFrom;
        private object _dateTo;
        private string _expeditionFrom;
        private string _expeditionTo;
        private string _peopleFrom;
        private string _peopleTo;
        private string _profession;
        
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged("UserId");
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
        public string ExpeditionFrom
        {
            get => _expeditionFrom;
            set
            {
                _expeditionFrom = value;
                OnPropertyChanged("ExpeditionFrom");
            }
        }
        public string ExpeditionTo
        {
            get => _expeditionTo;
            set
            {
                _expeditionTo = value;
                OnPropertyChanged("ExpeditionTo");
            }
        }
        public string PeopleFrom
        {
            get => _peopleFrom;
            set
            {
                _peopleFrom = value;
                OnPropertyChanged("PeopleFrom");
            }
        }
        public string PeopleTo
        {
            get => _peopleTo;
            set
            {
                _peopleTo = value;
                OnPropertyChanged("PeopleTo");
            }
        }
        public string Profession
        {
            get => _profession;
            set
            {
                _profession = value;
                OnPropertyChanged("Profession");
            }
        }
    }
}
