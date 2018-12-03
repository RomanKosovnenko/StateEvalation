using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation
{
    public class Filter
    {
        public string UserId { get; set; }
        public string ExpeditionFrom { get; set; }
        public string ExpeditionTo { get; set; }
        public string PeopleFrom { get; set; }
        public string PeopleTo { get; set; }
        public string Preference { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Profession { get; set; }
    }
}
