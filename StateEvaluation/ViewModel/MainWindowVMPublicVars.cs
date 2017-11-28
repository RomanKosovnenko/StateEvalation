using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  StateEvaluation.Helpers;

namespace StateEvaluation.ViewModel
{
    public partial class MainWindowVM : INotifyPropertyChanged
    {
        
        public static int ColorsCount = 4;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private List<byte> _ShortColorsNumbers;
        public List<byte> ShortColorsNumbers
        {
            get { return _ShortColorsNumbers; }
            set { _ShortColorsNumbers = value; PropertyChanged(this, new PropertyChangedEventArgs("ShortColorsNumbers")); }
        }
}
}
