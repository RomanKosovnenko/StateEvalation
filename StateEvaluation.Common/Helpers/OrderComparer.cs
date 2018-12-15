using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Common.Helpers
{
    public static class OrderComparer
    {
        public static bool Compare(string order, params string[] prefs)
        {
            var orders = order.Split(',');
            if (orders.Length != prefs.Length)
            {
                throw new IndexOutOfRangeException("Order lenght is not the same as prefs lengts");
            }

            var comparer = new bool[orders.Length];
            for (int i = 0; i < comparer.Length; ++i)
            {
                comparer[i] = string.IsNullOrEmpty(prefs[i]) || prefs[i] == orders[i];
            }
            return comparer.All(x => x);
        }
    }
}
