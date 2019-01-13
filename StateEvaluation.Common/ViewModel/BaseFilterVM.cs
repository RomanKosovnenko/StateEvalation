
using System.Collections.Generic;

namespace StateEvaluation.Common.ViewModel
{
    public class BaseFilterVM : BaseVM
    {
        protected List<string> _userIds;
        private object _dateFrom;
        private object _dateTo;
        protected List<string> _expeditions;
        protected List<string> _people;
        protected List<string> _professions;

        public BaseFilterVM()
        {
            _userIds = new List<string>();
            _expeditions = new List<string>();
            _people = new List<string>();
            _professions = new List<string>();
        }

        public List<string> UserIds { get => _userIds; set => _userIds = value; }
        public void SetUserState(string userId, bool state)
        {
            userId = userId.Trim();
            if (_userIds.Contains(userId) && !state)
            {
                _userIds.Remove(userId);
            }
            else if (!_userIds.Contains(userId) && state)
            {
                _userIds.Add(userId);
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

        public List<string> Expeditions { get => _expeditions; set => _expeditions = value; }
        public void SetExpeditionState(string expedition, bool state)
        {
            expedition = expedition.Trim();
            if (_expeditions.Contains(expedition) && !state)
            {
                _expeditions.Remove(expedition);
            }
            else if (!_expeditions.Contains(expedition) && state)
            {
                _expeditions.Add(expedition);
            }
        }

        public List<string> People { get => _people; set => _people = value; }
        public void SetPeopleState(string people, bool state)
        {
            people = people.Trim();
            if (_people.Contains(people) && !state)
            {
                _people.Remove(people);
            }
            else if (!_people.Contains(people) && state)
            {
                _people.Add(people);
            }
        }

        public List<string> Professions { get => _professions; set => _professions = value; }

        public void SetProfessionsState(string profession, bool state)
        {
            profession = profession.Trim();
            if (_professions.Contains(profession) && !state)
            {
                _professions.Remove(profession);
            }
            else if (!_professions.Contains(profession) && state)
            {
                _professions.Add(profession);
            }
        }
    }
}
