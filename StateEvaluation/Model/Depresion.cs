using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Model
{
    [Table(Name = "dbo.Depresion")]
    public class Depresion
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public DateTime Date { get; set; }
        [Column]
        public int Results { get; set; }

        [Column(Name = "UserID")] private string UserId;
        private EntityRef<People> _people = new EntityRef<People>();

        [Association(Name = "FK_Depresion_People", IsForeignKey = true, Storage = "_people", ThisKey = "UserId")]
        public People People
        {
            get => _people.Entity;
            set => _people.Entity = value;
        }
    }
}
