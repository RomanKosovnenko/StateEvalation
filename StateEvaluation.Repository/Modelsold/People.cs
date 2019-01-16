using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Repository.Models
{
    [Table(Name = "dbo.People")]
    public class People
    {
        private string _userId;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _birthday;
        private string _workposition;

        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "UserID")]
        public string UserId { get => _userId; set => _userId = value?.Trim(); }
        [Column]
        public string Firstname { get => _firstName; set => _firstName = value?.Trim(); }
        [Column]
        public string Middlename { get => _middleName; set => _middleName = value?.Trim(); }
        [Column]
        public string Lastname { get => _lastName; set => _lastName = value?.Trim(); }
        [Column]
        public string Birthday { get => _birthday; set => _birthday = value?.Trim(); }
        [Column]
        public string Workposition { get => _workposition; set => _workposition = value?.Trim(); }
        [Column]
        public int Expedition { get; set; }
        [Column]
        public int Number { get; set; }

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
