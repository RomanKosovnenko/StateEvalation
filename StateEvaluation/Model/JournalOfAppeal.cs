using System;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.JournalOfAppeal")]
    public class JournalOfAppeal
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public DateTime Date { get; set; }
        [Column]
        public string Diagnosis { get; set; }
        [Column]
        public string HelpIsProvided { get; set; }
        [Column]
        public int DurationOfTreatment { get; set; }

        [Column(Name = "UserID")]
        public string UserId { get; set; }
    }
}

