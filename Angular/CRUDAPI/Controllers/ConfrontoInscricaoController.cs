using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfrontoInscricaoController : ControllerBase
    {
        private readonly Contexto _contexto;

        public ConfrontoInscricaoController(Contexto contexto)
        {
            _contexto = contexto;
        }

        // GET: api/ConfrontoInscricao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConfrontoInscricao>>> GetConfrontoInscricoes()
        {
            return await _contexto.ConfrontoInscricoes.ToListAsync();
        }

        // GET: api/ConfrontoInscricao/5
        [HttpGet("{confrontoId}/{inscricaoId}")]
        public async Task<ActionResult<ConfrontoInscricao>> GetConfrontoInscricao(long confrontoId, long inscricaoId)
        {
            var confrontoInscricao = await _contexto.ConfrontoInscricoes.FindAsync(confrontoId, inscricaoId);

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
            _contexto.ConfrontoInscricoes.Add(confrontoInscricao);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConfrontoInscricao), new { confrontoId = confrontoInscricao.ConfrontoId, inscricaoId = confrontoInscricao.InscricaoId }, confrontoInscricao);
        }

        // DELETE: api/ConfrontoInscricao/5
        [HttpDelete("{confrontoId}/{inscricaoId}")]
        public async Task<IActionResult> DeleteConfrontoInscricao(long confrontoId, long inscricaoId)
        {
            var confrontoInscricao = await _contexto.ConfrontoInscricoes.FindAsync(confrontoId, inscricaoId);
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
