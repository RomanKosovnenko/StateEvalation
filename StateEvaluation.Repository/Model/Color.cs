using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Repository.Models
{
    [Table(Name = "dbo.Colors")]
    public class Color
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column(Name = "Color")]
        public string Color1 { get; set; }
        [Column(Name = "wavelengthmin")]
        public int WaveLengthMin { get; set; }
        [Column(Name = "wavelengthmax")]
        public int WaveLengthMax { get; set; }
        [Column(Name = "intensity")]
        public double Intensity { get; set; }
        [Column]
        public int ColorNumber { get; set; }
    }
}
