using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using CRUDAPI.Services;
using Newtonsoft.Json; // Importe o serviço de inscrições

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

        // GET: api/Inscricao/Categoria/{categoriaId}
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoesPorCategoria(long categoriaId)
        {
            var inscricoes = await _contexto.Inscricoes
                .Where(i => i.CategoriaId == categoriaId)
                .ToListAsync();

            return inscricoes;
        }

        public class EnviarInscricaoRequest
        {
            public long[] InscricoesIds { get; set; } = [];
            public string EmailOrganizador { get; set; } = "";
        }

        [HttpPost("enviar-inscricoes")]
        public async Task<ActionResult> EnviarInscricoesParaOrganizador([FromBody] EnviarInscricaoRequest request)
        {
            await _inscricaoService.EnviarInscricoesParaOrganizador(request.InscricoesIds, request.EmailOrganizador);
            return Ok();
        }

        [HttpGet("buscar-do-usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoesDoUsuario(long userId)
        {
            var inscricoes = await _contexto.Inscricoes
                .Where(i => _contexto.Competidores
                    .Where(c => c.CriadorId == userId)
                    .Select(c => c.Id)
                    .Contains(i.CompetidorId))
                .ToListAsync();

            return inscricoes;
        }

        [HttpGet("buscar-de-competicao/{competicaoId}")]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoesDeCompeticao(long competicaoId)
        {
            var inscricoes = await _contexto.Inscricoes
                .Where(i => _contexto.Categorias
                    .Where(c => c.CompeticaoId == competicaoId)
                    .Select(c => c.Id)
                    .Contains(i.CategoriaId))
                .ToListAsync();

            return inscricoes;
        }

        [HttpGet("info/{inscricaoId}")]
        public async Task<ActionResult<InfoInscricao>> GetInfoInscricao(long inscricaoId)
        {
            var inscricao = await _contexto.Inscricoes.FirstAsync(inscricao => inscricao.Id == inscricaoId);
            var categoria = await _contexto.Categorias.FirstAsync(categoria => categoria.Id == inscricao.CategoriaId);
            var competicao = await _contexto.Competicoes.FirstAsync(competicao => categoria.CompeticaoId == competicao.Id);
            var competidor = await _contexto.Competidores.FirstAsync(competidor => competidor.Id == inscricao.CompetidorId);

            var infoInscricao = new InfoInscricao
            {
                TituloCompeticao = competicao.Titulo,
                NomeCategoria = categoria.Nome,
                NomeCompetidor = competidor.Nome,
                EmailCompetidor = competidor.Email
            };

            return infoInscricao;
        }

        public class InfoInscricao
        {
            public string TituloCompeticao { get; set; } = "";
            public string NomeCategoria { get; set; } = "";
            public string NomeCompetidor { get; set; } = "";
            public string EmailCompetidor { get; set; } = "";
        }
    }
}
