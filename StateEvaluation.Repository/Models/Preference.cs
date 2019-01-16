namespace StateEvaluation.Repository.Models
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Preference")]
    public partial class Preference
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(10)]
        public string UserID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public int FavoriteColor { get; set; }

        [Required]
        [StringLength(6)]
        public string ShortOder1 { get; set; }

        [Required]
        [StringLength(26)]
        public string Oder1 { get; set; }

        [Required]
        [StringLength(10)]
        public string Preference1 { get; set; }

        [StringLength(6)]
        public string ShortOder2 { get; set; }

        [StringLength(26)]
        public string Oder2 { get; set; }

        [StringLength(10)]
        public string Preference2 { get; set; }

        [StringLength(10)]
        public string Compare { get; set; }

        public int? RelaxTable1 { get; set; }

        public int? RelaxTable2 { get; set; }

        public virtual People People { get; set; }

        public virtual RelaxTable1 RelaxTable11 { get; set; }

        public virtual RelaxTable2 RelaxTable21 { get; set; }
    }
}
