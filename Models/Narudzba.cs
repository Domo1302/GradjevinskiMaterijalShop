using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradjevinskiMaterijali.Models
{
    [Table("Narudzba")]
    public class Narudzba
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Datum { get; set; } = DateTime.Now;

        [Required]
        public decimal UkupnaCijena { get; set; }

        public string? Napomena { get; set; }

        // Navigacijsko svojstvo za stavke narudžbe
        public ICollection<NarudzbaStavka> Stavke { get; set; } = new List<NarudzbaStavka>();
    }
}
