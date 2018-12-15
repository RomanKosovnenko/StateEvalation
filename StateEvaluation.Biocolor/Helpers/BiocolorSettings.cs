using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.BioColor.Helpers
{
    public class BiocolorSettings
    {
        public int Int_Red {
            get
            {
                return Biocolor.BiocolorSettings.Default.Int_Red;
            }
        }


        public int Int_Green
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.Int_Green;
            }
        }


        public int Int_Blue
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.Int_Blue;
            }
        }


        public int Mid
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.mid;
            }
        }


        public int Square
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.square;
            }
        }


        public int Topline
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.topline;
            }
        }


        public int Step
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.step;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.step = value;
            }
        }


        public double Alpha
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.alpha;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.alpha = value;
            }
        }


        public string P1
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.p1;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.p1 = value;
            }
        }


        public string P2
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.p2;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.p2 = value;
            }
        }


        public string P3
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.p3;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.p3 = value;
            }
        }


        public string P4
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.p4;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.p4 = value;
            }
        }


        public string E1
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.e1;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.e1 = value;
            }
        }


        public string E2
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.e2;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.e2 = value;
            }
        }


        public string E3
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.e3;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.e3 = value;
            }
        }


        public string E4
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.e4;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.e4 = value;
            }
        }


        public string I1
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.i1;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.i1 = value;
            }
        }

        public string I2
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.i2;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.i2 = value;
            }
        }

        public string I3
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.i3;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.i3 = value;
            }
        }

        public string I4
        {
            get
            {
                return Biocolor.BiocolorSettings.Default.i4;
            }
            set
            {
                Biocolor.BiocolorSettings.Default.i4 = value;
            }
        }
        public void Save()
        {
            Biocolor.BiocolorSettings.Default.Save();
        }
        public void Reset()
        {
            Biocolor.BiocolorSettings.Default.Reset();
        }
    }
}
