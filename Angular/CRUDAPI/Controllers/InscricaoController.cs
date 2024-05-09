using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using CRUDAPI.Services; // Importe o serviço de inscrições

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscricaoController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly InscricaoService _inscricaoService; // Adicionando o serviço de inscrições

        public InscricaoController(Contexto contexto, InscricaoService inscricaoService)
        {
            _contexto = contexto;
            _inscricaoService = inscricaoService;
        }

        // GET: api/Inscricao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoes()
        {
            return await _contexto.Inscricoes.ToListAsync();
        }

        // GET: api/Inscricao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inscricao>> GetInscricao(long id)
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
            try
            {
                // Valida os campos obrigatórios da inscrição
                inscricao.Posição = 0;
                inscricao = await _inscricaoService.ValidarInscricao(inscricao);

                _contexto.Inscricoes.Add(inscricao);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInscricao), new { id = inscricao.Id }, inscricao);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Inscricao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscricao(long id, Inscricao inscricao)
        {
            if (id != inscricao.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(inscricao).State = EntityState.Modified;

            try
            {
                inscricao = await _inscricaoService.ValidarInscricao(inscricao);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_inscricaoService.InscricaoExists(id))
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

        // DELETE: api/Inscricao/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inscricao>> DeleteInscricao(long id)
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
    }
}
