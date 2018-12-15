using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Common.Helpers
{
    public static class UserIdBuilder
    {
        public static string Build(int expedition, int number)
        {
            return $"Ex{expedition}#{number}";
        }
    }
}
