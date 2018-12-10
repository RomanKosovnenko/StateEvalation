using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Helpers
{
    public static class RangeGenerator
    {
        public static string Generate(string f, string t)
        {
            if (!int.TryParse(f, out int from) || !int.TryParse(t, out int to))
            {
                return "([0-9]+)";
            }
            else
            {
                string re = "(";
                if (from > to)
                {
                    int temp = from;
                    from = to;
                    to = temp;
                };
                for (int i = from; i <= to; ++i)
                {
                    re += i;
                    if (i != to) re += "|";
                }
                re += ")";
                return re;
            }
        }
    }
}
