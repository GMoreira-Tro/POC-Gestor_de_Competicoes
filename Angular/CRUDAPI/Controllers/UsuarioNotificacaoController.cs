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
    public class UsuarioNotificacaoController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly UsuarioNotificacaoService _UsuarioNotificacaoService;

        public UsuarioNotificacaoController(Contexto contexto, UsuarioNotificacaoService UsuarioNotificacaoService)
        {
            _contexto = contexto;
            _UsuarioNotificacaoService = UsuarioNotificacaoService;
        }

        // GET: api/UsuarioNotificacao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioNotificacao>>> GetUsuarioNotificacoes()
        {
            return await _contexto.UsuarioNotificacoes.ToListAsync();
        }

        // GET: api/UsuarioNotificacao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioNotificacao>> GetUsuarioNotificacao(long id)
        {
            var UsuarioNotificacao = await _contexto.UsuarioNotificacoes.FindAsync(id);

            if (UsuarioNotificacao == null)
            {
                return NotFound();
            }

            return UsuarioNotificacao;
        }

        // POST: api/UsuarioNotificacao
        [HttpPost]
        public async Task<ActionResult<UsuarioNotificacao>> PostUsuarioNotificacao(UsuarioNotificacao usuarioNotificacao)
        {
            try
            {
                usuarioNotificacao = await _UsuarioNotificacaoService.ValidarUsuarioNotificacao(usuarioNotificacao);

                _contexto.UsuarioNotificacoes.Add(usuarioNotificacao);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuarioNotificacao), new { id = usuarioNotificacao.Id }, usuarioNotificacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/UsuarioNotificacao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarioNotificacao(long id, UsuarioNotificacao usuarioNotificacao)
        {
            if (id != usuarioNotificacao.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(usuarioNotificacao).State = EntityState.Modified;

            try
            {
                usuarioNotificacao = await _UsuarioNotificacaoService.ValidarUsuarioNotificacao(usuarioNotificacao);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_UsuarioNotificacaoService.UsuarioNotificacaoExists(id))
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

        // DELETE: api/UsuarioNotificacao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioNotificacao(long id)
        {
            var UsuarioNotificacao = await _contexto.UsuarioNotificacoes.FindAsync(id);
            if (UsuarioNotificacao == null)
            {
                return NotFound();
            }

            _contexto.UsuarioNotificacoes.Remove(UsuarioNotificacao);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
