using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GradjevinskiMaterijali.Data;
using GradjevinskiMaterijali.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace GradjevinskiMaterijali.Controllers
{
    [Route("api/narudzbe")]
    [ApiController]
    public class NarudzbaController : ControllerBase
    {
        private readonly GradjevinskiMaterijaliContext _context;

        public NarudzbaController(GradjevinskiMaterijaliContext context)
        {
            _context = context;
        }

        // GET: api/narudzbe
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Narudzba>>> GetNarudzbe()
        {
            return await _context.Narudzbe.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Narudzba>> GetNarudzba(int id)
        {
            var narudzba = await _context.Narudzbe
                .Include(n => n.Stavke)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (narudzba == null)
            {
                return NotFound();
            }

            return narudzba;
        }

        [HttpPost]
        public async Task<ActionResult<Narudzba>> PostNarudzba(Narudzba narudzba)
        {
            // Automatski izračunajte ukupnu cijenu ako nije postavljena
            if (narudzba.Stavke != null && narudzba.Stavke.Any())
            {
                narudzba.UkupnaCijena = narudzba.Stavke.Sum(s => s.Kolicina * s.JedinicnaCijena);
            }

            _context.Narudzbe.Add(narudzba);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNarudzba), new { id = narudzba.Id }, narudzba);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutNarudzba(int id, Narudzba narudzba)
        {
            if (id != narudzba.Id)
            {
                return BadRequest();
            }

            // Ažurirajte osnovne podatke narudžbe
            _context.Entry(narudzba).State = EntityState.Modified;

            // Upravljanje stavkama narudžbe
            foreach (var stavka in narudzba.Stavke)
            {
                if (stavka.Id == 0)
                {
                    // Nova stavka
                    _context.NarudzbaStavke.Add(stavka);
                }
                else
                {
                    // Ažuriraj postojeću stavku
                    _context.Entry(stavka).State = EntityState.Modified;
                }
            }

            // Opcionalno: Obrada brisanja stavki koje više nisu u narudžbi
            var postojeceStavke = _context.NarudzbaStavke.Where(s => s.NarudzbaId == id).ToList();
            foreach (var postojecaStavka in postojeceStavke)
            {
                if (!narudzba.Stavke.Any(s => s.Id == postojecaStavka.Id))
                {
                    _context.NarudzbaStavke.Remove(postojecaStavka);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Narudzbe.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNarudzba(int id)
        {
            var narudzba = await _context.Narudzbe
                .Include(n => n.Stavke)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (narudzba == null)
            {
                return NotFound();
            }

            _context.NarudzbaStavke.RemoveRange(narudzba.Stavke);
            _context.Narudzbe.Remove(narudzba);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
