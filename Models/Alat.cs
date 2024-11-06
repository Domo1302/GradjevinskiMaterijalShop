using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradjevinskiMaterijali.Models
{
    [Table("Alat")]
    public class Alat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Naziv { get; set; }

        [Required]
        public string SKU { get; set; }

        [Required]
        public int Kolicina { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Cijena { get; set; }

        public string? Marka { get; set; }

        public int? GarancijaGodina { get; set; }
    }
}
