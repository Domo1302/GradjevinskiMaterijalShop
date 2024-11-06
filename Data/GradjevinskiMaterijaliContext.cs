using GradjevinskiMaterijali.Models;
using Microsoft.EntityFrameworkCore;

namespace GradjevinskiMaterijali.Data
{
    public class GradjevinskiMaterijaliContext : DbContext
    {
        public GradjevinskiMaterijaliContext(DbContextOptions<GradjevinskiMaterijaliContext> options)
            : base(options) { }

        public DbSet<Alat> Alati { get; set; }
        public DbSet<Materijal> Materijali { get; set; }
        public DbSet<Narudzba> Narudzbe { get; set; }
        public DbSet<NarudzbaStavka> NarudzbaStavke { get; set; }
    }
}
