using System;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.BloodTest")]
    public class BloodTest
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public DateTime Date { get; set; }
        [Column]
        public int Hemoglobin { get; set; }
        [Column]
        public string RedBloodCells { get; set; }
        [Column]
        public string Hematocrit { get; set; }
        [Column]
        public string ColorIndicator { get; set; }
        [Column]
        public string WhiteBloodCells { get; set; }

        [Column(Name = "UserID")] public string UserId { get; set; }
    }
} 

