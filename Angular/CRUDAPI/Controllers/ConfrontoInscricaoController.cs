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
    public class ConfrontoInscricaoController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly ConfrontoInscricaoService _confrontoInscricaoService;

        public ConfrontoInscricaoController(Contexto contexto, ConfrontoInscricaoService confrontoInscricaoService)
        {
            _contexto = contexto;
            _confrontoInscricaoService = confrontoInscricaoService;
        }

        // GET: api/ConfrontoInscricao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConfrontoInscricao>>> GetConfrontoInscricoes()
        {
            return await _contexto.ConfrontoInscricoes.ToListAsync();
        }

        // GET: api/ConfrontoInscricao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConfrontoInscricao>> GetConfrontoInscricao(long id)
        {
            var confrontoInscricao = await _contexto.ConfrontoInscricoes.FindAsync(id);

            if (confrontoInscricao == null)
            {
                return NotFound();
            }

            return confrontoInscricao;
        }

        // POST: api/ConfrontoInscricao
        [HttpPost]
        public async Task<ActionResult<ConfrontoInscricao>> PostConfrontoInscricao(ConfrontoInscricao confrontoInscricao)
        {
            try
            {
                confrontoInscricao = await _confrontoInscricaoService.ValidarConfrontoInscricao(confrontoInscricao);

                _contexto.ConfrontoInscricoes.Add(confrontoInscricao);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetConfrontoInscricao), new { id = confrontoInscricao }, confrontoInscricao);
            }
             catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/ConfrontoInscricao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscricao (long id, ConfrontoInscricao confrontoInscricao)
        {
            if (id != confrontoInscricao.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(confrontoInscricao).State = EntityState.Modified;

            try
            {
                confrontoInscricao = await _confrontoInscricaoService.ValidarConfrontoInscricao(confrontoInscricao);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_confrontoInscricaoService.ConfrontoInscricaoExists(id))
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

        // DELETE: api/ConfrontoInscricao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfrontoInscricao(long id)
        {
            var confrontoInscricao = await _contexto.ConfrontoInscricoes.FindAsync(id);
            if (confrontoInscricao == null)
            {
                return NotFound();
            }

            _contexto.ConfrontoInscricoes.Remove(confrontoInscricao);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
