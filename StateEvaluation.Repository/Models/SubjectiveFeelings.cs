namespace StateEvaluation.Repository.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SubjectiveFeelings
    {
        [Column(TypeName = "ID")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "UserID")]
        public string UserId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public bool GeneralWeaknes { get; set; }

        public bool PoorAppetite { get; set; }

        public bool PoorSleep { get; set; }

        public bool BadMood { get; set; }

        public bool HeavyHead { get; set; }

        public bool SlowThink { get; set; }

        public virtual People People { get; set; }
    }
}
