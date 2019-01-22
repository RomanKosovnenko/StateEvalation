namespace StateEvaluation.Repository.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Colors")]
    public partial class Color
    {
        [Column("ID")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Colors { get; set; }

        [Column("wavelengthmin")]
        public int Wavelengthmin { get; set; }

        [Column("wavelengthmax")]
        public int Wavelengthmax { get; set; }

        [Column("intensity")]
        public double Intensity { get; set; }

        public int ColorNumber { get; set; }
    }
}
