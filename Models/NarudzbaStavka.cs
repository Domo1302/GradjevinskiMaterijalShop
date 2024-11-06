using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GradjevinskiMaterijali.Models
{
    [Table("NarudzbaStavka")]
    public class NarudzbaStavka
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Kolicina { get; set; }

        [Required]
        public decimal JedinicnaCijena { get; set; }

        [Required]
        public int ProizvodId { get; set; }

        [Required]
        public string ProizvodTip { get; set; } // "Alat" ili "Materijal"

        // Navigacijsko svojstvo za Narudzba
        public int NarudzbaId { get; set; }

        [ForeignKey("NarudzbaId")]
        [JsonIgnore]
        public Narudzba? Narudzba { get; set; }
    }
}
