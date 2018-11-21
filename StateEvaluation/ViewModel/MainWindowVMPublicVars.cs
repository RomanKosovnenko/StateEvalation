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
    public partial class MainWindowVM : INotifyPropertyChanged
    {
        public string SelectedUser { get; set; }
        public bool GeneralWeaknes { get; set; }
        public bool PoorAppetite { get; set; }
        public bool PoorSleep { get; set; }
        private bool badMood;
        public bool BadMood {
            get { return badMood; }
            set
            {
                badMood = value;
                OnPropertyChanged("BadMood");
            }
        }
        public bool HeavyHead { get; set; }
        public bool SlowThink { get; set; }
        private object fealingsDate = new DatePicker();
        public object FealingsDate {
            get { return fealingsDate; }
            set
            {
                fealingsDate = value;
                OnPropertyChanged("FealingsDate");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
