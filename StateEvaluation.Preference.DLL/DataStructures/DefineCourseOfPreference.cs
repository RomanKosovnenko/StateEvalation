using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluationDLL.DataStructures
{
    public class DefineCourseOfPreference
    {
        public DefineCourseOfPreference(int countColors, int[] arrayColors)
        {
            int coursByHour = 0, courseCounterclockwise = 0, difference = 0, undefined = 0;
            for (int i = 0; i < countColors - 1; ++i)
            {
                difference = arrayColors[i + 1] - arrayColors[i];
                if (((difference > 0) && (difference < countColors / 2)) ||
                    ((difference < 0) && (difference > countColors / 2)))
                    coursByHour++;
                else if (difference == countColors / 2)
                    undefined++;
                else
                    courseCounterclockwise++;
            }
        }
    }
}
