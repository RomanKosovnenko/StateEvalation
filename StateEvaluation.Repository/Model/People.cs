namespace StateEvaluation.Repository.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class People
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public People()
        {
            Preference = new HashSet<Preference>();
            SubjectiveFeelings = new HashSet<SubjectiveFeeling>();
        }

        public Guid Id { get; set; }

        [Key]
        [StringLength(10)]
        [Column("UserID")]
        public string UserId { get; set; }

        [StringLength(20)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(30)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(10)]
        public string Birthday { get; set; }

        public int Expedition { get; set; }

        public int Number { get; set; }

        [StringLength(20)]
        public string Workposition { get; set; }

        [StringLength(30)]
        public string Middlename { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Preference> Preference { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubjectiveFeeling> SubjectiveFeelings { get; set; }
    }
}
