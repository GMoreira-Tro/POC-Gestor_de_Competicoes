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
    public class NotificacaoController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly NotificacaoService _NotificacaoService;
        private readonly EmailService _emailService;

        public NotificacaoController(Contexto contexto, NotificacaoService NotificacaoService, EmailService emailService)
        {
            _contexto = contexto;
            _NotificacaoService = NotificacaoService;
            _emailService = emailService;
        }

        // GET: api/Notificacao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notificacao>>> GetNotificacaos()
        {
            return await _contexto.Notificacoes.ToListAsync();
        }

        // GET: api/Notificacao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notificacao>> GetNotificacao(long id)
        {
            var Notificacao = await _contexto.Notificacoes.FindAsync(id);

            if (Notificacao == null)
            {
                return NotFound();
            }

            return Notificacao;
        }

        // POST: api/Notificacao
        [HttpPost]
        public async Task<ActionResult<Notificacao>> PostNotificacao(Notificacao Notificacao)
        {
            try
            {
                Notificacao = await _NotificacaoService.ValidarNotificacao(Notificacao);

                _contexto.Notificacoes.Add(Notificacao);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetNotificacao), new { id = Notificacao.Id }, Notificacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Notificacao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotificacao(long id, Notificacao Notificacao)
        {
            if (id != Notificacao.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(Notificacao).State = EntityState.Modified;

            try
            {
                Notificacao = await _NotificacaoService.ValidarNotificacao(Notificacao);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_NotificacaoService.NotificacaoExists(id))
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

        // DELETE: api/Notificacao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificacao(long id)
        {
            var Notificacao = await _contexto.Notificacoes.FindAsync(id);
            if (Notificacao == null)
            {
                return NotFound();
            }

            _contexto.Notificacoes.Remove(Notificacao);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("buscar-do-usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<Notificacao>>> GetNotificacoesPeloUsuario(long userId)
        {
            return await _contexto.Notificacoes.Where(notificacao => notificacao.NotificadoId == userId).ToListAsync();
        }

        [HttpPost("inscricao/{inscricaoId}")]
        public async Task<IActionResult> CadastrarNotificacaoDeInscricao(long inscricaoId)
        {
            try
            {
                var inscricao = await _contexto.Inscricoes.FindAsync(inscricaoId);
                if (inscricao == null)
                {
                    return NotFound("Inscrição não encontrada");
                }

                var categoria = await _contexto.Categorias.FindAsync(inscricao.CategoriaId);
                if (categoria == null)
                {
                    return NotFound("Categoria não encontrada");
                }

                var competicao = await _contexto.Competicoes.FindAsync(categoria.CompeticaoId);
                if (competicao == null)
                {
                    return NotFound("Competição não encontrada");
                }
                var competidor = await _contexto.Competidores.FindAsync(inscricao.CompetidorId);
                if (competidor == null)
                {
                    return NotFound("Competidor não encontrado");
                }

                var usuario = await _contexto.Usuarios.FindAsync(competidor.CriadorId);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado");
                }

                var notificacao = new Notificacao();
                if (inscricao.Status == InscricaoStatus.APagar)
                {
                    notificacao = new Notificacao
                    {
                        NotificadoId = competidor.CriadorId,
                        Titulo = $"Inscrição em {competicao.Titulo} de {competidor.Nome} aceita!",
                        Descricao = $"Sua inscrição no evento {competicao.Titulo} " +
                        $"do competidor {competidor.Nome} " +
                        $"foi aceita! Aguardando pagamento.",
                        DataPublicacao = DateTime.Now,
                        Link = $"http://localhost:4200/pagar-inscricao/{inscricaoId}",
                        TipoAnuncio = "Inscrição"
                    };

                    if (!string.IsNullOrEmpty(usuario.Email))
                    {
                        var subject = "Inscrição Aceita!";
                        var body = $"Olá {usuario.Nome},\n\n" +
                                   $"Sua inscrição no evento {competicao.Titulo} do competidor {competidor.Nome} foi aceita! " +
                                   $"Aguardando pagamento. Acesse o link para realizar o pagamento: " +
                                   $"http://localhost:4200/pagar-inscricao/{inscricaoId}\n\n" +
                                   "Atenciosamente,\nEquipe ConectaCompetição";

                        await _emailService.SendEmailAsync(usuario.Email, subject, body);
                    }
                }
                else
                {
                    notificacao = new Notificacao
                    {
                        NotificadoId = competidor.CriadorId,
                        Titulo = $"Inscrição em {competicao.Titulo} de {competidor.Nome} recusada!",
                        Descricao = $"Sua inscrição no evento {competicao.Titulo} " +
                        $"do competidor {competidor.Nome} " +
                        $"foi recusada!",
                        DataPublicacao = DateTime.Now,
                        Link = $"http://localhost:4200/pagar-inscricao/{inscricaoId}",
                        TipoAnuncio = "Inscrição"
                    };

                    if (!string.IsNullOrEmpty(usuario.Email))
                    {
                        var subject = "Inscrição Recusada!";
                        var body = $"Olá {usuario.Nome},\n\n" +
                                   $"Sua inscrição no evento {competicao.Titulo} do competidor {competidor.Nome} foi recusada! " +
                                   "Atenciosamente,\nEquipe ConectaCompetição";

                        await _emailService.SendEmailAsync(usuario.Email, subject, body);
                    }
                }

                _contexto.Notificacoes.Add(notificacao);
                await _contexto.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
