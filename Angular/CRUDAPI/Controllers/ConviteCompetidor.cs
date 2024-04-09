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
    public class ConviteCompetidorController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly ConviteCompetidorService _conviteCompetidorService;

        public ConviteCompetidorController(Contexto contexto, ConviteCompetidorService conviteCompetidorService)
        {
            _contexto = contexto;
            _conviteCompetidorService = conviteCompetidorService;
        }

        // GET: api/ConviteCompetidor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConviteCompetidor>>> GetConvitesCompetidores()
        {
            return await _contexto.ConvitesCompetidores.ToListAsync();
        }

        // GET: api/ConviteCompetidor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConviteCompetidor>> GetConviteCompetidor(long id)
        {
            var conviteCompetidor = await _contexto.ConvitesCompetidores.FindAsync(id);

            if (conviteCompetidor == null)
            {
                return NotFound();
            }

            return conviteCompetidor;
        }

        // POST: api/ConviteCompetidor
        [HttpPost]
        public async Task<ActionResult<ConviteCompetidor>> PostConviteCompetidor(ConviteCompetidor conviteCompetidor)
        {
            try
            {
                conviteCompetidor = await _conviteCompetidorService.ValidarConviteCompetidor(conviteCompetidor);

                _contexto.ConvitesCompetidores.Add(conviteCompetidor);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetConviteCompetidor), new { id = conviteCompetidor }, conviteCompetidor);
            }
             catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/ConviteCompetidor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscricao (long id, ConviteCompetidor conviteCompetidor)
        {
            if (id != conviteCompetidor.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(conviteCompetidor).State = EntityState.Modified;

            try
            {
                conviteCompetidor = await _conviteCompetidorService.ValidarConviteCompetidor(conviteCompetidor);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_conviteCompetidorService.ConviteCompetidorExists(id))
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

        // DELETE: api/ConviteCompetidor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConviteCompetidor(long id)
        {
            var conviteCompetidor = await _contexto.ConvitesCompetidores.FindAsync(id);
            if (conviteCompetidor == null)
            {
                return NotFound();
            }

            _contexto.ConvitesCompetidores.Remove(conviteCompetidor);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
