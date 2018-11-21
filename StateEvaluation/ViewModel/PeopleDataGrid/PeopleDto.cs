using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StateEvaluation.ViewModel.PeopleDataGrid
{
    public class PeopleDto: INotifyPropertyChanged
    {

        private string _firstName;
        private string _lastName;
        private string _middleName;
        private object _birthday;
        private string _workposition;
        private string _expedition;
        private string _personNumber;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public string MiddleName
        {
            get => _middleName;
            set
            {
                _middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }

        public object Birthday
        {
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public string Workposition
        {
            get => _workposition;
            set
            {
                _workposition = value;
                OnPropertyChanged("Workposition");
            }
        }

        public string Expedition
        {
            get => _expedition;
            set
            {
                _expedition = value;
                OnPropertyChanged("Expedition");
            }
        }

        public string PersonNumber
        {
            get => _personNumber;
            set
            {
                _personNumber = value;
                OnPropertyChanged("PersonNumber");
            }
        }

        public PeopleDto()
        {
            _birthday = new DatePicker();
        }
    }
}
