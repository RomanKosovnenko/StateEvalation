using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using StateEvaluation.Helpers;

namespace StateEvaluation.ViewModel
{
    public class SubjectiveFeelingVM : BaseVM
    {
        private object _date;
        private string _userId;
        private bool _generalWeaknes;
        private bool _poorAppetite;
        private bool _poorSleep;
        private bool _badMood;
        private bool _heavyHead;
        private bool _slowThink;
        private string _id;

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged("UserId");
            }
        }

        public bool GeneralWeaknes
        {
            get => _generalWeaknes;
            set
            {
                _generalWeaknes = value;
                OnPropertyChanged("GeneralWeaknes");
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

        public object Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }
    }
}
