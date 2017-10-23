using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.NormalPreference")]
    public class NormalPreference
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public string Age { get; set; }
        [Column(Name = "order")]
        public string Order { get; set; }
        [Column]
        public char Pol { get; set; }
    }
}
