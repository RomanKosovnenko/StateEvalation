using System;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.CycleErgometryWithLoad")]
    public class CycleErgometryWithLoad
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public int ArterialPressuresUpper { get; set; }
        [Column]
        public int ArterialPressuresBottom { get; set; }
        [Column]
        public int HeartRate { get; set; }
        [Column]
        public int Load { get; set; }
        [Column]
        public int LoadTime { get; set; }

        [Column(Name = "UserID")]
        public string UserId { get; set; }
    }
}

