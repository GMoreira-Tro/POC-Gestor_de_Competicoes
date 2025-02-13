using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using CRUDAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUDAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetidorController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly CompetidorService _competidorService;

        public CompetidorController(Contexto contexto, CompetidorService competidorService)
        {
            _contexto = contexto;
            _competidorService = competidorService;
        }

        // GET: api/Competidor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competidor>>> GetCompetidores()
        {
            return await _contexto.Competidores.ToListAsync();
        }

        // GET: api/Competidor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Competidor>> GetCompetidor(long id)
        {
            var competidor = await _contexto.Competidores.FindAsync(id);

            if (competidor == null)
            {
                return NotFound();
            }

            return competidor;
        }

        // PUT: api/Competidor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompetidor(long id, Competidor competidor)
        {
            if (id != competidor.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(competidor).State = EntityState.Modified;

            try
            {
                competidor = await _competidorService.ValidarCompetidor(competidor);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_competidorService.CompetidorExists(id))
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

        // POST: api/Competidor
        [HttpPost]
        public async Task<ActionResult<Competidor>> PostCompetidor(Competidor competidor)
        {
            try
            {
                competidor = await _competidorService.ValidarCompetidor(competidor);

                _contexto.Competidores.Add(competidor);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCompetidor), new { id = competidor.Id }, competidor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Competidor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetidor(long id)
        {
            var competidor = await _contexto.Competidores.FindAsync(id);
            if (competidor == null)
            {
                return NotFound();
            }

            _contexto.Competidores.Remove(competidor);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("buscar-do-usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<Competidor>>> BuscarDoUsuario(long userId)
        {
            return await _contexto.Competidores.Where(competidor => competidor.CriadorId == userId).ToListAsync();
        }

        [HttpPost("{id}/upload-imagem")]
        public async Task<IActionResult> UploadImagem(long id, IFormFile imagem)
        {
            var competidor = await _contexto.Competidores.FindAsync(id);
            if (competidor == null) return NotFound("Competidor não encontrada");

            if (imagem == null || imagem.Length == 0)
                return BadRequest("Nenhuma imagem foi enviada");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}_{imagem.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }

            competidor.ImagemUrl = $"/uploads/{fileName}";
            _contexto.Competidores.Update(competidor);
            await _contexto.SaveChangesAsync();

            return Ok(new { imagemUrl = competidor.ImagemUrl });
        }
    }
}
