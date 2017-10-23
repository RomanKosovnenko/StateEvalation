using StateEvaluationDLL.DataStructures.ReferenceData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StateEvaluationDLL.DataStructures
{
    public class TestColorDescription
    {
        TestColors Name { get; set; }
        Color Color { get; set; }
        TemperamentType Temperament { get; set; }
        String Descripton { get; set; }

        public static String GetColorName(int order)
        {
            return (((TestColors)order).ToString());
        }
    }
}
