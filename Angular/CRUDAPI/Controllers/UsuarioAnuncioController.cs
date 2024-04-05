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
    public class UsuarioAnuncioController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly UsuarioAnuncioService _UsuarioAnuncioService;

        public UsuarioAnuncioController(Contexto contexto, UsuarioAnuncioService UsuarioAnuncioService)
        {
            _contexto = contexto;
            _UsuarioAnuncioService = UsuarioAnuncioService;
        }

        // GET: api/UsuarioAnuncio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioAnuncio>>> GetUsuarioAnuncios()
        {
            return await _contexto.UsuarioAnuncios.ToListAsync();
        }

        // GET: api/UsuarioAnuncio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioAnuncio>> GetUsuarioAnuncio(long id)
        {
            var UsuarioAnuncio = await _contexto.UsuarioAnuncios.FindAsync(id);

            if (UsuarioAnuncio == null)
            {
                return NotFound();
            }

            return UsuarioAnuncio;
        }

        // POST: api/UsuarioAnuncio
        [HttpPost]
        public async Task<ActionResult<UsuarioAnuncio>> PostUsuarioAnuncio(UsuarioAnuncio usuarioAnuncio)
        {
            try
            {
                usuarioAnuncio = await _UsuarioAnuncioService.ValidarUsuarioAnuncio(usuarioAnuncio);

                _contexto.UsuarioAnuncios.Add(usuarioAnuncio);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuarioAnuncio), new { id = usuarioAnuncio.Id }, usuarioAnuncio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/UsuarioAnuncio/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarioAnuncio(long id, UsuarioAnuncio usuarioAnuncio)
        {
            if (id != usuarioAnuncio.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(usuarioAnuncio).State = EntityState.Modified;

            try
            {
                usuarioAnuncio = await _UsuarioAnuncioService.ValidarUsuarioAnuncio(usuarioAnuncio);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_UsuarioAnuncioService.UsuarioAnuncioExists(id))
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

        // DELETE: api/UsuarioAnuncio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarioAnuncio(long id)
        {
            var UsuarioAnuncio = await _contexto.UsuarioAnuncios.FindAsync(id);
            if (UsuarioAnuncio == null)
            {
                return NotFound();
            }

            _contexto.UsuarioAnuncios.Remove(UsuarioAnuncio);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
