using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GradjevinskiMaterijali.Data;
using GradjevinskiMaterijali.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace GradjevinskiMaterijali.Controllers
{
    [Route("api/materijali")]
    [ApiController]
    public class MaterijaliController : ControllerBase
    {
        private readonly GradjevinskiMaterijaliContext _context;

        public MaterijaliController(GradjevinskiMaterijaliContext context)
        {
            _context = context;
        }

        // GET: api/materijali
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materijal>>> GetMaterijali()
        {
            return await _context.Materijali.ToListAsync();
        }

        // GET: api/materijali/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Materijal>> GetMaterijal(int id)
        {
            var materijal = await _context.Materijali.FindAsync(id);
            if (materijal == null)
                return NotFound();

            return materijal;
        }

        // POST: api/materijali
        [HttpPost]
        public async Task<ActionResult<Materijal>> PostMaterijal(Materijal materijal)
        {
            _context.Materijali.Add(materijal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaterijal), new { id = materijal.Id }, materijal);
        }

        // PUT: api/materijali/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterijal(int id, Materijal materijal)
        {
            if (id != materijal.Id)
                return BadRequest();

            _context.Entry(materijal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Materijali.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/materijali/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterijal(int id)
        {
            var materijal = await _context.Materijali.FindAsync(id);
            if (materijal == null)
                return NotFound();

            _context.Materijali.Remove(materijal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
