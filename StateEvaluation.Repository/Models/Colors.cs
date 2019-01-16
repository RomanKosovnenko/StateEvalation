namespace StateEvaluation.Repository.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Colors
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Color { get; set; }

        public int wavelengthmin { get; set; }

        public int wavelengthmax { get; set; }

        public double intensity { get; set; }

        public int ColorNumber { get; set; }
    }
}
