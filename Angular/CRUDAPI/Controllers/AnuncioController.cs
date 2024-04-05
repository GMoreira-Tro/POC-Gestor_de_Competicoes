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
    public class AnuncioController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly AnuncioService _AnuncioService;

        public AnuncioController(Contexto contexto, AnuncioService AnuncioService)
        {
            _contexto = contexto;
            _AnuncioService = AnuncioService;
        }

        // GET: api/Anuncio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Anuncio>>> GetAnuncios()
        {
            return await _contexto.Anuncios.ToListAsync();
        }

        // GET: api/Anuncio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Anuncio>> GetAnuncio(long id)
        {
            var Anuncio = await _contexto.Anuncios.FindAsync(id);

            if (Anuncio == null)
            {
                return NotFound();
            }

            return Anuncio;
        }

        // POST: api/Anuncio
        [HttpPost]
        public async Task<ActionResult<Anuncio>> PostAnuncio(Anuncio Anuncio)
        {
            try
            {
                Anuncio = await _AnuncioService.ValidarAnuncio(Anuncio);

                _contexto.Anuncios.Add(Anuncio);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAnuncio), new { id = Anuncio.Id }, Anuncio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Anuncio/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnuncio(long id, Anuncio Anuncio)
        {
            if (id != Anuncio.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(Anuncio).State = EntityState.Modified;

            try
            {
                Anuncio = await _AnuncioService.ValidarAnuncio(Anuncio);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_AnuncioService.AnuncioExists(id))
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

        // DELETE: api/Anuncio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnuncio(long id)
        {
            var Anuncio = await _contexto.Anuncios.FindAsync(id);
            if (Anuncio == null)
            {
                return NotFound();
            }

            _contexto.Anuncios.Remove(Anuncio);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
