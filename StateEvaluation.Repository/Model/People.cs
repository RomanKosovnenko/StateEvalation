using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Repository.Models
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
        public string Middlename { get; set; }
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

        internal object Substring(int v1, int v2)
        {
            throw new NotImplementedException();
        }

        private EntitySet<SubjectiveFeeling> _subjectiveFeelings = new EntitySet<SubjectiveFeeling>();
        [Association(Storage = "_subjectiveFeelings", OtherKey = "UserId")]
        public EntitySet<SubjectiveFeeling> SubjectiveFeelings
        {
            get { return this._subjectiveFeelings; }
            set { this._subjectiveFeelings.Assign(value); }
        }
    }
}
