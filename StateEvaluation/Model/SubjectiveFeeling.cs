using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.SubjectiveFeelings")]
    public class SubjectiveFeeling
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public DateTime Date { get; set; }
        [Column]
        public bool GeneralWeaknes { get; set; }
        [Column]
        public bool PoorAppetite { get; set; }
        [Column]
        public bool PoorSleep { get; set; }
        [Column]
        public bool BadMood { get; set; }
        [Column]
        public bool HeavyHead { get; set; }
        [Column]
        public bool SlowThink { get; set; }

        [Column(Name = "UserID")]
        public string UserId { get; set; }

        private EntityRef<People> _people = new EntityRef<People>();
        [Association(Name = "FK_SubjectiveFeelings_People", IsForeignKey = true, Storage = "_people", ThisKey = "UserId")]
        public People People
        {
            get { return _people.Entity; }
            set { _people.Entity = value; }
        }
    }
}
