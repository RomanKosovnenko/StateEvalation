namespace StateEvaluation.Repository.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NormalPreference")]
    public partial class NormalPreference
    {
        public Guid ID { get; set; }

        public int Age { get; set; }

        [Required]
        [StringLength(27)]
        public string order { get; set; }

        [Required]
        [StringLength(1)]
        public string Pol { get; set; }
    }
}
