using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models; // Importe os modelos necessários

namespace CRUDAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricaoController : ControllerBase
    {
        private readonly Contexto _contexto;

        public InscricaoController(Contexto contexto)
        {
            _contexto = contexto;
        }

        // GET: api/Inscricao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoes()
        {
            return await _contexto.Inscricoes.ToListAsync();
        }

        // GET: api/Inscricao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inscricao>> GetInscricao(int id)
        {
            var inscricao = await _contexto.Inscricoes.FindAsync(id);

            if (inscricao == null)
            {
                return NotFound();
            }

            return inscricao;
        }

        // POST: api/Inscricao
        [HttpPost]
        public async Task<ActionResult<Inscricao>> PostInscricao(Inscricao inscricao)
        {
            _contexto.Inscricoes.Add(inscricao);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInscricao), new { id = inscricao.Id }, inscricao);
        }

        // PUT: api/Inscricao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscricao(int id, Inscricao inscricao)
        {
            if (id != inscricao.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(inscricao).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscricaoExists(id))
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

        // DELETE: api/Inscricao/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inscricao>> DeleteInscricao(int id)
        {
            var inscricao = await _contexto.Inscricoes.FindAsync(id);
            if (inscricao == null)
            {
                return NotFound();
            }

            _contexto.Inscricoes.Remove(inscricao);
            await _contexto.SaveChangesAsync();

            return inscricao;
        }

        private bool InscricaoExists(int id)
        {
            return _contexto.Inscricoes.Any(e => e.Id == id);
        }
    }
}
