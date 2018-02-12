using System;
using System.Data.Linq.Mapping;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.Preference")]
    public class Preference
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public DateTime Date { get; set; }
        [Column]
        public int FavoriteColor { get; set; }
        [Column]
        public string ShortOder1 { get; set; }
        [Column]
        public string Oder1 { get; set; }
        [Column]
        public string Preference1 { get; set; }
        [Column]
        public string ShortOder2 { get; set; }
        [Column]
        public string Oder2 { get; set; }
        [Column]
        public string Preference2 { get; set; }
        [Column]
        public string Compare { get; set; }
        [Column]
        public int? RelaxTable1 { get; set; }
        [Column]
        public int? RelaxTable2 { get; set; }

        [Column(Name = "UserID")] public string UserId { get; set; }
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
