using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.RelaxTable1")]
    public class RelaxTable1
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column]
        public int Number { get; set; }

        //private EntitySet<Preference> _preferences1 = new EntitySet<Preference>();
        //[Association(Name = "FK_Preference_RelaxTable1", Storage = "_preferences1", OtherKey = "relaxTable1", ThisKey = "Id")]
        //public ICollection<Preference> Preferences1
        //{
        //    get => _preferences1;
        //    set => _preferences1.Assign(value);
        //}

        //private EntitySet<Preference> _preferences2 = new EntitySet<Preference>();
        //[Association(Name = "FK_Preference_RelaxTable2", Storage = "_preferences2", OtherKey = "relaxTable2", ThisKey = "Id")]
        //public ICollection<Preference> Preferences2
        //{
        //    get => _preferences2;
        //    set => _preferences2.Assign(value);
        //}
    }
}
