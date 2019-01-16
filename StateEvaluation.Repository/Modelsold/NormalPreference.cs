using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Repository.Models
{
    [Table(Name = "dbo.NormalPreference")]
    public class NormalPreference
    {
        private string _age;
        private string _order;

        [Column(Name = "ID", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public string Age { get => _age; set => _age = value?.Trim(); }
        [Column(Name = "order")]
        public string Order { get => _order; set => _order = value?.Trim(); }
        [Column]
        public char Pol { get; set; }
    }
}
