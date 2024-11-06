using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradjevinskiMaterijali.Models
{
    [Table("Materijal")]
    public class Materijal
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

        public string? Tip { get; set; }

        public string? Dobavljac { get; set; }
    }
}
