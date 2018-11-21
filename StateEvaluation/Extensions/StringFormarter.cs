using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Extensions
{
    public static class StringFormarter
    {
        public static string GetDateFromDateTimeString(this string dateTimeString)
        {
            return dateTimeString.Split(' ')[0];
        }
    }
}
