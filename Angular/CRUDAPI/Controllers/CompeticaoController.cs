using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using CRUDAPI.Services; // Importe o serviço CompeticaoService

namespace CRUDAPI.Controllers // Corrigi o nome do namespace para "Controllers"
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompeticaoController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly CompeticaoService _competicaoService; // Adicione a injeção de dependência do CompeticaoService

        public CompeticaoController(Contexto contexto, CompeticaoService competicaoService) // Adicione o CompeticaoService ao construtor
        {
            _contexto = contexto;
            _competicaoService = competicaoService;
        }

        // GET: api/Competicao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competicao>>> GetCompeticoes()
        {
            return await _contexto.Competicoes.ToListAsync();
        }

        // GET: api/Competicao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Competicao>> GetCompeticao(long id)
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
            try
            {
                await _competicaoService.ValidarCompeticao(competicao); // Validar a competição antes de adicioná-la

                _contexto.Competicoes.Add(competicao);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCompeticao), new { id = competicao.Id }, competicao);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao salvar a competição: {ex.InnerException?.Message}");
            }
        }

        // PUT: api/Competicao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompeticao(long id, Competicao competicao)
        {
            if (id != competicao.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(competicao).State = EntityState.Modified;

            try
            {
                await _competicaoService.ValidarCompeticao(competicao);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_competicaoService.CompeticaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/Competicao/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Competicao>> DeleteCompeticao(long id)
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
    }
}
