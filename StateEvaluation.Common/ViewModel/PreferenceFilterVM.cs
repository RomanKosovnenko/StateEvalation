
namespace StateEvaluation.Common.ViewModel
{
    public class PreferenceFilterVM : BaseFilterVM
    {
        private string _color1in3Filter;
        private string _color2in3Filter;
        private string _color3in3Filter;
        private string _color1in12Filter;
        private string _color2in12Filter;
        private string _color3in12Filter;
        private string _color4in12Filter;
        private string _color5in12Filter;
        private string _color6in12Filter;
        private string _color7in12Filter;
        private string _color8in12Filter;
        private string _color9in12Filter;
        private string _color10in12Filter;
        private string _color11in12Filter;
        private string _color12in12Filter;
        private string _preferenceFilter;

        public string Color1in3Filter
        {
            get => _color1in3Filter;
            set
            {
                _color1in3Filter = value;
                OnPropertyChanged("Color1in3Filter");
            }
        }
        public string Color2in3Filter
        {
            get => _color2in3Filter;
            set
            {
                _color2in3Filter = value;
                OnPropertyChanged("Color2in3Filter");
            }
        }
        public string Color3in3Filter
        {
            get => _color3in3Filter;
            set
            {
                _color3in3Filter = value;
                OnPropertyChanged("Color3in3Filter");
            }
        }
        public string Color1in12Filter
        {
            get => _color1in12Filter;
            set
            {
                _color1in12Filter = value;
                OnPropertyChanged("Color1in12Filter");
            }
        }
        public string Color2in12Filter
        {
            get => _color2in12Filter;
            set
            {
                _color2in12Filter = value;
                OnPropertyChanged("Color2in12Filter");
            }
        }
        public string Color3in12Filter
        {
            get => _color3in12Filter;
            set
            {
                _color3in12Filter = value;
                OnPropertyChanged("Color3in12Filter");
            }
        }
        public string Color4in12Filter
        {
            get => _color4in12Filter;
            set
            {
                _color4in12Filter = value;
                OnPropertyChanged("Color4in12Filter");
            }
        }
        public string Color5in12Filter
        {
            get => _color5in12Filter;
            set
            {
                _color5in12Filter = value;
                OnPropertyChanged("Color5in12Filter");
            }
        }
        public string Color6in12Filter
        { 
            get => _color6in12Filter;
            set
            {
                _color6in12Filter = value;
                OnPropertyChanged("Color6in12Filter");
            }
        }
        public string Color7in12Filter
        {
            get => _color7in12Filter;
            set
            {
                _color7in12Filter = value;
                OnPropertyChanged("Color7in12Filter");
            }
        }
        public string Color8in12Filter
        {
            get => _color8in12Filter;
            set
            {
                _color8in12Filter = value;
                OnPropertyChanged("Color8in12Filter");
            }
        }
        public string Color9in12Filter
        {
            get => _color9in12Filter;
            set
            {
                _color9in12Filter = value;
                OnPropertyChanged("Color9in12Filter");
            }
        }
        public string Color10in12Filter
        {
            get => _color10in12Filter;
            set
            {
                _color10in12Filter = value;
                OnPropertyChanged("Color10in12Filter");
            }
        }
        public string Color11in12Filter
        {
            get => _color11in12Filter;
            set
            {
                _color11in12Filter = value;
                OnPropertyChanged("Color11in12Filter");
            }
        }
        public string Color12in12Filter
        {
            get => _color12in12Filter;
            set
            {
                _color12in12Filter = value;
                OnPropertyChanged("Color12in12Filter");
            }
        }
        public string PreferenceFilter
        {
            get => _preferenceFilter;
            set
            {
                _preferenceFilter = value;
                OnPropertyChanged("UserId");
            }
        }
    }
}
