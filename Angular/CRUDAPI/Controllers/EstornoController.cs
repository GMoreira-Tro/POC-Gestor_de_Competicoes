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
    public class EstornoController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly EstornoService _EstornoService;

        public EstornoController(Contexto contexto, EstornoService EstornoService)
        {
            _contexto = contexto;
            _EstornoService = EstornoService;
        }

        // GET: api/Estorno
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estorno>>> GetEstornos()
        {
            return await _contexto.Estornos.ToListAsync();
        }

        // GET: api/Estorno/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estorno>> GetEstorno(long id)
        {
            var Estorno = await _contexto.Estornos.FindAsync(id);

            if (Estorno == null)
            {
                return NotFound();
            }

            return Estorno;
        }

        // POST: api/Estorno
        [HttpPost]
        public async Task<ActionResult<Estorno>> PostEstorno(Estorno Estorno)
        {
            try
            {
                Estorno = await _EstornoService.ValidarEstorno(Estorno);

                _contexto.Estornos.Add(Estorno);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEstorno), new { id = Estorno.Id }, Estorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Estorno/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstorno(long id, Estorno Estorno)
        {
            if (id != Estorno.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(Estorno).State = EntityState.Modified;

            try
            {
                Estorno = await _EstornoService.ValidarEstorno(Estorno);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_EstornoService.EstornoExists(id))
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

        // DELETE: api/Estorno/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstorno(long id)
        {
            var Estorno = await _contexto.Estornos.FindAsync(id);
            if (Estorno == null)
            {
                return NotFound();
            }

            _contexto.Estornos.Remove(Estorno);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
