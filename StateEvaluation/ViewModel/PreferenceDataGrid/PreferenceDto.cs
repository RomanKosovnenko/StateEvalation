using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.ViewModel.PreferenceDataGrid
{
    public class PreferenceDto : INotifyPropertyChanged
    {
        private string _id;
        private string _userId;
        private object _testDate;
        private string _color1in3;
        private string _color2in3;
        private string _color3in3;
        private string _color1in12;
        private string _color2in12;
        private string _color3in12;
        private string _color4in12;
        private string _color5in12;
        private string _color6in12;
        private string _color7in12;
        private string _color8in12;
        private string _color9in12;
        private string _color10in12;
        private string _color11in12;
        private string _color12in12;
        private string _color21in3;
        private string _color22in3;
        private string _color23in3;
        private string _color21in12;
        private string _color22in12;
        private string _color23in12;
        private string _color24in12;
        private string _color25in12;
        private string _color26in12;
        private string _color27in12;
        private string _color28in12;
        private string _color29in12;
        private string _color210in12;
        private string _color211in12;
        private string _color212in12;
        private string _preference1Red;
        private string _preference1Yellow;
        private string _preference1Blue;
        private string _preference1Grey;
        private string _preference2Red;
        private string _preference2Yellow;
        private string _preference2Blue;
        private string _preference2Grey;
        private string _colorRelax1;
        private string _colorRelax2;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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
        public object TestDate
        {
            get => _testDate;
            set
            {
                _testDate = value;
                OnPropertyChanged("TestDate");
            }
        }

        public string Color1in3
        {
            get => _color1in3;
            set
            {
                _color1in3 = value;
                OnPropertyChanged("Color1in3");
            }
        }
        public string Color2in3
        {
            get => _color2in3;
            set
            {
                _color2in3 = value;
                OnPropertyChanged("Color2in3");
            }
        }
        public string Color3in3
        {
            get => _color3in3;
            set
            {
                _color3in3 = value;
                OnPropertyChanged("Color3in3");
            }
        }
        public string Color1in12
        {
            get => _color1in12;
            set
            {
                _color1in12 = value;
                OnPropertyChanged("Color1in12");
            }
        }
        public string Color2in12
        {
            get => _color2in12;
            set
            {
                _color2in12 = value;
                OnPropertyChanged("Color2in12");
            }
        }
        public string Color3in12
        {
            get => _color3in12;
            set
            {
                _color3in12 = value;
                OnPropertyChanged("Color3in12");
            }
        }
        public string Color4in12
        {
            get => _color4in12;
            set
            {
                _color4in12 = value;
                OnPropertyChanged("Color4in12");
            }
        }
        public string Color5in12
        {
            get => _color5in12;
            set
            {
                _color5in12 = value;
                OnPropertyChanged("Color5in12");
            }
        }
        public string Color6in12
        {
            get => _color6in12;
            set
            {
                _color6in12 = value;
                OnPropertyChanged("Color6in12");
            }
        }
        public string Color7in12
        {
            get => _color7in12;
            set
            {
                _color7in12 = value;
                OnPropertyChanged("Color7in12");
            }
        }
        public string Color8in12
        {
            get => _color8in12;
            set
            {
                _color8in12 = value;
                OnPropertyChanged("Color8in12");
            }
        }
        public string Color9in12
        {
            get => _color9in12;
            set
            {
                _color9in12 = value;
                OnPropertyChanged("Color9in12");
            }
        }
        public string Color10in12
        {
            get => _color10in12;
            set
            {
                _color10in12 = value;
                OnPropertyChanged("Color10in12");
            }
        }
        public string Color11in12
        {
            get => _color11in12;
            set
            {
                OnPropertyChanged("Color11in12");
                _color11in12 = value;
            }
        }
        public string Color12in12
        {
            get => _color12in12;
            set
            {
                _color12in12 = value;
                OnPropertyChanged("Color12in12");
            }
        }
        public string Color21in3
        {
            get => _color21in3;
            set
            {
                _color21in3 = value;
                OnPropertyChanged("Color21in3");
            }
        }
        public string Color22in3
        {
            get => _color22in3;
            set
            {
                _color22in3 = value;
                OnPropertyChanged("Color22in3");
            }
        }
        public string Color23in3
        {
            get => _color23in3;
            set
            {
                _color23in3 = value;
                OnPropertyChanged("Color23in3");
            }
        }
        public string Color21in12
        {
            get => _color21in12;
            set
            {
                _color21in12 = value;
                OnPropertyChanged("Color21in12");
            }
        }
        public string Color22in12
        {
            get => _color22in12;
            set
            {
                _color22in12 = value;
                OnPropertyChanged("Color22in12");
            }
        }
        public string Color23in12
        {
            get => _color23in12;
            set
            {
                _color23in12 = value;
                OnPropertyChanged("Color23in12");
            }
        }
        public string Color24in12
        {
            get => _color24in12;
            set
            {
                _color24in12 = value;
                OnPropertyChanged("Color24in12");
            }
        }
        public string Color25in12
        {
            get => _color25in12;
            set
            {
                _color25in12 = value;
                OnPropertyChanged("Color25in12");
            }
        }
        public string Color26in12
        {
            get => _color26in12;
            set
            {
                _color26in12 = value;
                OnPropertyChanged("Color26in12");
            }
        }
        public string Color27in12
        {
            get => _color27in12;
            set
            {
                _color27in12 = value;
                OnPropertyChanged("Color27in12");
            }
        }
        public string Color28in12
        {
            get => _color28in12;
            set
            {
                _color28in12 = value;
                OnPropertyChanged("Color28in12");
            }
        }
        public string Color29in12
        {
            get => _color29in12;
            set
            {
                _color29in12 = value;
                OnPropertyChanged("Color29in12");
            }
        }
        public string Color210in12
        {
            get => _color210in12;
            set
            {
                _color210in12 = value;
                OnPropertyChanged("Color210in12");
            }
        }
        public string Color211in12
        {
            get => _color211in12;
            set
            {
                _color211in12 = value;
                OnPropertyChanged("Color211in12");
            }
        }
        public string Color212in12
        {
            get => _color212in12;
            set
            {
                _color212in12 = value;
                OnPropertyChanged("Color212in12");
            }
        }
        public string Preference1Red
        {
            get => _preference1Red;
            set
            {
                _preference1Red = value;
                OnPropertyChanged("Preference1Red");
            }
        }
        public string Preference1Blue
        {
            get => _preference1Blue;
            set
            {
                _preference1Blue = value;
                OnPropertyChanged("Preference1Blue");
            }
        }
        public string Preference1Yellow
        {
            get => _preference1Yellow;
            set
            {
                _preference1Yellow = value;
                OnPropertyChanged("Preference1Yellow");
            }
        }
        public string Preference2Red
        {
            get => _preference2Red;
            set
            {
                _preference2Red = value;
                OnPropertyChanged("Preference2Red");
            }
        }
        public string Preference2Yellow
        {
            get => _preference2Yellow;
            set
            {
                _preference2Yellow = value;
                OnPropertyChanged("Preference2Yellow");
            }
        }
        public string Preference2Blue
        {
            get => _preference2Blue;
            set
            {
                _preference2Blue = value;
                OnPropertyChanged("Preference2Blue");
            }
        }
        public string Preference2Grey
        {
            get => _preference2Grey;
            set
            {
                _preference2Grey = value;
                OnPropertyChanged("Preference2Grey");
            }
        }
        public string ColorRelax1
        {
            get => _colorRelax1;
            set
            {
                _colorRelax1 = value;
                OnPropertyChanged("ColorRelax1");
            }
        }
        public string ColorRelax2
        {
            get => _colorRelax2;
            set
            {
                _colorRelax2 = value;
                OnPropertyChanged("ColorRelax2");
            }
        }
        public string Preference1Grey
        {
            get => _preference1Grey;
            set
            {
                _preference1Grey = value;
                OnPropertyChanged("Preference1Grey");
            }
        }

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
    }
}
