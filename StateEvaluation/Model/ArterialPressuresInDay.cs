using System;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.ArterialPressuresInDay")]
    public class ArterialPressuresInDay
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public DateTime Date { get; set; }
        [Column]
        public int ArterialPressuresUpper { get; set; }
        [Column]
        public int ArterialPressuresBottom { get; set; }
        [Column]
        public int HeartRate { get; set; }
        [Column]
        public int Temperature { get; set; }
        [Column]
        public DateTime Time { get; set; }

        [Column(Name = "UserID")]
        public string UserId { get; set; }
    }
}

