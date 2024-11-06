using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GradjevinskiMaterijali.Data;
using GradjevinskiMaterijali.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace GradjevinskiMaterijali.Controllers
{
    [Route("api/alati")]
    [ApiController]
    public class AlatiController : ControllerBase
    {
        private readonly GradjevinskiMaterijaliContext _context;

        public AlatiController(GradjevinskiMaterijaliContext context)
        {
            _context = context;
        }

        // GET: api/alati
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alat>>> GetAlati()
        {
            return await _context.Alati.ToListAsync();
        }

        // GET: api/alati/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alat>> GetAlat(int id)
        {
            var alat = await _context.Alati.FindAsync(id);
            if (alat == null)
                return NotFound();

            return alat;
        }

        // POST: api/alati
        [HttpPost]
        public async Task<ActionResult<Alat>> PostAlat(Alat alat)
        {
            _context.Alati.Add(alat);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlat), new { id = alat.Id }, alat);
        }

        // PUT: api/alati/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlat(int id, Alat alat)
        {
            if (id != alat.Id)
                return BadRequest();

            _context.Entry(alat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Alati.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/alati/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlat(int id)
        {
            var alat = await _context.Alati.FindAsync(id);
            if (alat == null)
                return NotFound();

            _context.Alati.Remove(alat);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
