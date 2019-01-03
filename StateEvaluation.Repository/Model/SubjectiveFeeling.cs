using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Repository.Models
{
    [Table(Name = "dbo.SubjectiveFeelings")]
    public class SubjectiveFeeling
    {
        [Column(Name = "ID", DbType = "uniqueidentifier", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "UserID")]
        public string UserId { get; set; }
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

        private EntityRef<People> _people = new EntityRef<People>();
        [Association(Name = "FK_SubjectiveFeelings_People", IsForeignKey = true, Storage = "_people", ThisKey = "UserId", OtherKey = "UserId")]
        public People People
        {
            get { return _people.Entity; }
            set
            {
                //https://msdn.microsoft.com/ru-ru/library/bb386989(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-3
                People previousValue = this._people.Entity;
                if (((previousValue != value) || (this._people.HasLoadedOrAssignedValue == false)))
                {
                    if ((previousValue != null))
                    {
                        this._people.Entity = null;
                        previousValue.SubjectiveFeelings.Remove(this);
                    }
                    this._people.Entity = value;
                    if ((value != null))
                    {
                        value.SubjectiveFeelings.Add(this);
                        this.Id = value.Id;
                    }
                    else
                    {
                        this.Id = default(Guid);
                    }
                }
            }
        }
    }
}
