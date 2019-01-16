using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Repository.Models
{
    [Table(Name = "dbo.RelaxTable2")]
    public class RelaxTable2
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column]
        public int Number { get; set; }
    }
}
