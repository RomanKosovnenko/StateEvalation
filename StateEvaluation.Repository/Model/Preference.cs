using System;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Repository.Models
{
    [Table(Name = "dbo.Preference")]
    public class Preference
    {
        private string _userId;
        private string _shortOrder1;
        private string _shortOrder2;
        private string _order1;
        private string _order2;
        private string _preference1;
        private string _preference2;
        private string _compare;

        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public DateTime Date { get; set; }
        [Column]
        public int FavoriteColor { get; set; }
        [Column]
        public string ShortOder1 { get => _shortOrder1; set => _shortOrder1 = value?.Trim(); }
        [Column]
        public string Oder1 { get => _order1; set => _order1 = value?.Trim(); }
        [Column]
        public string Preference1 { get => _preference1; set => _preference1 = value?.Trim(); }
        [Column]
        public string ShortOder2 { get => _shortOrder2; set => _shortOrder2 = value?.Trim(); }
        [Column]
        public string Oder2 { get => _order2; set => _order2 = value?.Trim(); }
        [Column]
        public string Preference2 { get => _preference2; set => _preference2 = value?.Trim(); }
        [Column]
        public string Compare { get => _compare; set => _compare = value?.Trim(); }
        [Column]
        public int? RelaxTable1 { get; set; }
        [Column]
        public int? RelaxTable2 { get; set; }

        [Column(Name = "UserID")]
        public string UserId { get => _userId; set => _userId = value?.Trim(); }
    //    private EntityRef<People> _people = new EntityRef<People>();

    //    [Association(Name = "FK_Preferences_People", IsForeignKey = true, Storage = "_people", ThisKey = "UserId")]
    //    public People People
    //    {
    //        get => _people.Entity;
    //        set => _people.Entity = value;
    //    }

    //    [Column(Name = "RelaxTable1")] private int? relaxTable1;
    //    private EntityRef<RelaxTable1> _RelaxTable1 = new EntityRef<RelaxTable1>();

    //    [Association(Name = "FK_Preference_RelaxTable1", IsForeignKey = true, Storage = "_RelaxTable1", ThisKey = "relaxTable1")]
    //    public RelaxTable1 RelaxTable1
    //    {
    //        get => _RelaxTable1.Entity;
    //        set => _RelaxTable1.Entity = value;
    //    }

    //    [Column(Name = "relaxTable2")] private int? relaxTable2;
    //    private EntityRef<RelaxTable1> _RelaxTable2 = new EntityRef<RelaxTable1>();

    //    [Association(Name = "FK_Preference_RelaxTable2", IsForeignKey = true, Storage = "_RelaxTable2", ThisKey = "relaxTable2")]
    //    public RelaxTable1 RelaxTable2
    //    {
    //        get => _RelaxTable2.Entity;
    //        set => _RelaxTable2.Entity = value;
    //    }
    }
}
