using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;

namespace CRUDAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompeticaoController : ControllerBase
    {
        private readonly Contexto _contexto;

        public CompeticaoController(Contexto contexto)
        {
            _contexto = contexto;
        }

        // GET: api/Competicao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competicao>>> GetCompeticoes()
        {
            return await _contexto.Competicoes.ToListAsync();
        }

        // GET: api/Competicao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Competicao>> GetCompeticao(int id)
        {
            var competicao = await _contexto.Competicoes.FindAsync(id);

            if (competicao == null)
            {
                return NotFound();
            }

            return competicao;
        }

        // POST: api/Competicao
        [HttpPost]
        public async Task<ActionResult<Competicao>> PostCompeticao(Competicao competicao)
        {
            _contexto.Competicoes.Add(competicao);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompeticao), new { id = competicao.Id }, competicao);
        }

        // PUT: api/Competicao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompeticao(int id, Competicao competicao)
        {
            if (id != competicao.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(competicao).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompeticaoExists(id))
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

        // DELETE: api/Competicao/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Competicao>> DeleteCompeticao(int id)
        {
            var competicao = await _contexto.Competicoes.FindAsync(id);
            if (competicao == null)
            {
                return NotFound();
            }

            _contexto.Competicoes.Remove(competicao);
            await _contexto.SaveChangesAsync();

            return competicao;
        }

        private bool CompeticaoExists(int id)
        {
            return _contexto.Competicoes.Any(e => e.Id == id);
        }
    }
}
