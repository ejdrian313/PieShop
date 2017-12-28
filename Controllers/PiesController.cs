using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PieShop.Data;
using PieShop.Models;

namespace PieShop.Controllers
{
    [Produces("application/json")]
    [Route("api/Pies")]
    public class PiesController : Controller
    {
        private readonly AppDbContext _context;

        public PiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pies
        [HttpGet]
        public IEnumerable<Pie> GetPies()
        {
            return _context.Pies;
        }

        // GET: api/Pies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pie = await _context.Pies.SingleOrDefaultAsync(m => m.PieId == id);

            if (pie == null)
            {
                return NotFound();
            }

            return Ok(pie);
        }

        // PUT: api/Pies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPie([FromRoute] int id, [FromBody] Pie pie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pie.PieId)
            {
                return BadRequest();
            }

            _context.Entry(pie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PieExists(id))
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

        // POST: api/Pies
        [HttpPost]
        public async Task<IActionResult> PostPie([FromBody] Pie pie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pies.Add(pie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPie", new { id = pie.PieId }, pie);
        }

        // DELETE: api/Pies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pie = await _context.Pies.SingleOrDefaultAsync(m => m.PieId == id);
            if (pie == null)
            {
                return NotFound();
            }

            _context.Pies.Remove(pie);
            await _context.SaveChangesAsync();

            return Ok(pie);
        }

        private bool PieExists(int id)
        {
            return _context.Pies.Any(e => e.PieId == id);
        }
    }
}