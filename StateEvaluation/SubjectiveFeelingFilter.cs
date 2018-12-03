using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation
{
    public class SubjectiveFeelingFilter: Filter
    {
        public bool GeneralWeakness { get; set; }
        public bool PoorAppetite { get; set; }
        public bool PoorSleep { get; set; }
        public bool BadMood { get; set; }
        public bool HeavyHead { get; set; }
        public bool SlowThink { get; set; }
    }
}
