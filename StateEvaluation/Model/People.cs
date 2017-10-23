using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.People")]
    public class People
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "UserID")]
        public string UserId { get; set; }
        [Column]
        public string Firstname { get; set; }
        [Column]
        public string Lastname { get; set; }
        [Column]
        public string Birthday { get; set; }
        [Column]
        public int Expedition { get; set; }
        [Column]
        public int Number { get; set; }
        [Column]
        public string Workposition { get; set; }
    }
}
