using System;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.Anthropometry")]
    public class Anthropometry
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public DateTime Date { get; set; }
        [Column]
        public int Growth { get; set; }
        [Column]
        public int Weight { get; set; }
        [Column]
        public int FatContentIn_kg { get; set; }
        [Column]
        public int FatContentIn_per { get; set; }
        [Column]
        public int VitalCapacityOfTheLungsIn_liters { get; set; }

        [Column(Name = "UserID")]
        public string UserId { get; set; }
    }
}

