﻿using System.Windows.Controls;

namespace StateEvaluation.ViewModel
{
    public class PeopleVM: BaseVM
    {
        private string _id;
        private string _firstName;
        private string _lastName;
        private string _middleName;
        private object _birthday;
        private string _workposition;
        private string _expedition;
        private string _personNumber;

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
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

        public PeopleVM()
        {
            _birthday = new DatePicker();
        }
    }
}
