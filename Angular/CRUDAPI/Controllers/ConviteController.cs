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
    public class ConviteController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly ConviteService _ConviteService;

        public ConviteController(Contexto contexto, ConviteService ConviteService)
        {
            _contexto = contexto;
            _ConviteService = ConviteService;
        }

        // GET: api/Convite
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Convite>>> GetConvites()
        {
            return await _contexto.Convites.ToListAsync();
        }

        // GET: api/Convite/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Convite>> GetConvite(long id)
        {
            var Convite = await _contexto.Convites.FindAsync(id);

            if (Convite == null)
            {
                return NotFound();
            }

            return Convite;
        }

        // POST: api/Convite
        [HttpPost]
        public async Task<ActionResult<Convite>> PostConvite(Convite Convite)
        {
            try
            {
                Convite = await _ConviteService.ValidarConvite(Convite);

                _contexto.Convites.Add(Convite);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetConvite), new { id = Convite.Id }, Convite);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Convite/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConvite(long id, Convite Convite)
        {
            if (id != Convite.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(Convite).State = EntityState.Modified;

            try
            {
                Convite = await _ConviteService.ValidarConvite(Convite);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_ConviteService.ConviteExists(id))
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

        // DELETE: api/Convite/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConvite(long id)
        {
            var Convite = await _contexto.Convites.FindAsync(id);
            if (Convite == null)
            {
                return NotFound();
            }

            _contexto.Convites.Remove(Convite);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
