using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRUDAPI.Services;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremioController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly PremioService _PremioService;

        public PremioController(Contexto contexto, PremioService PremioService)
        {
            _contexto = contexto;
            _PremioService = PremioService;
        }

        // GET: api/Premio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Premio>>> GetPremios()
        {
            return await _contexto.Premios.ToListAsync();
        }

        // GET: api/Premio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Premio>> GetPremio(long id)
        {
            var Premio = await _contexto.Premios.FindAsync(id);

            if (Premio == null)
            {
                return NotFound();
            }

            return Premio;
        }

        // POST: api/Premio
        [HttpPost]
        public async Task<ActionResult<Premio>> PostPremio(Premio Premio)
        {
            try
            {
                Premio = await _PremioService.ValidarPremio(Premio);

                _contexto.Premios.Add(Premio);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPremio), new { id = Premio.Id }, Premio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Premio/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPremio(long id, Premio Premio)
        {
            if (id != Premio.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(Premio).State = EntityState.Modified;

            try
            {
                Premio = await _PremioService.ValidarPremio(Premio);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_PremioService.PremioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/Premio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePremio(long id)
        {
            var Premio = await _contexto.Premios.FindAsync(id);
            if (Premio == null)
            {
                return NotFound();
            }

            _contexto.Premios.Remove(Premio);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
