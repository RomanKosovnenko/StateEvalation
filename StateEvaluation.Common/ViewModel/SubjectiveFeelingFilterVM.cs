
namespace StateEvaluation.Common.ViewModel
{
    public class SubjectiveFeelingFilterVM : BaseFilterVM
    {
        private bool _generalWeaknes;
        private bool _poorAppetite;
        private bool _poorSleep;
        private bool _badMood;
        private bool _heavyHead;
        private bool _slowThink;
        private bool _isFeeling;

        public bool GeneralWeakness
        {
            get => _generalWeaknes;
            set
            {
                _generalWeaknes = value;
                OnPropertyChanged("GeneralWeakness");
            }
        }
        public bool PoorAppetite
        {
            get => _poorAppetite;
            set
            {
                _poorAppetite = value;
                OnPropertyChanged("PoorAppetite");
            }
        }
        public bool PoorSleep
        {
            get => _poorSleep;
            set
            {
                _poorSleep = value;
                OnPropertyChanged("PoorSleep");
            }
        }
        public bool BadMood
        {
            get => _badMood;
            set
            {
                _badMood = value;
                OnPropertyChanged("BadMood");
            }
        }
        public bool HeavyHead
        {
            get => _heavyHead;
            set
            {
                _heavyHead = value;
                OnPropertyChanged("HeavyHead");
            }
        }
        public bool SlowThink
        {
            get => _slowThink;
            set
            {
                _slowThink = value;
                OnPropertyChanged("SlowThink");
            }
        }
        public bool IsFeeling
        {
            get => _isFeeling;
            set
            {
                _isFeeling = value;
                OnPropertyChanged("IsFeeling");
            }
        }
    }
}
