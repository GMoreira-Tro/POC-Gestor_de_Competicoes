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

        public NotificacaoController(Contexto contexto, NotificacaoService NotificacaoService)
        {
            _contexto = contexto;
            _NotificacaoService = NotificacaoService;
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
    }
}
