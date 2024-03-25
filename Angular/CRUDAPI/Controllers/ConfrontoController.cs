using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using CRUDAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfrontoController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly ConfrontoService _confrontoService;

        public ConfrontoController(Contexto contexto, ConfrontoService confrontoService)
        {
            _contexto = contexto;
            _confrontoService = confrontoService;
        }

        // GET: api/Confronto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Confronto>>> GetConfrontos()
        {
            return await _contexto.Confrontos.ToListAsync();
        }

        // GET: api/Confronto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Confronto>> GetConfronto(long id)
        {
            var confronto = await _contexto.Confrontos.FindAsync(id);

            if (confronto == null)
            {
                return NotFound();
            }

            return confronto;
        }

        // POST: api/Confronto
        [HttpPost]
        public async Task<ActionResult<Confronto>> PostConfronto(Confronto confronto)
        {
            try
            {
                confronto = await _confrontoService.ValidarConfronto(confronto);
                _contexto.Confrontos.Add(confronto);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetConfronto), new { id = confronto.Id }, confronto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao salvar o confronto: {ex.Message}");
            }
        }

        // PUT: api/Confronto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfronto(long id, Confronto confronto)
        {
            if (id != confronto.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(confronto).State = EntityState.Modified;

            try
            {
                confronto = await _confrontoService.ValidarConfronto(confronto);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfrontoExists(id))
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

        // DELETE: api/Confronto/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Confronto>> DeleteConfronto(long id)
        {
            var confronto = await _contexto.Confrontos.FindAsync(id);
            if (confronto == null)
            {
                return NotFound();
            }

            _contexto.Confrontos.Remove(confronto);
            await _contexto.SaveChangesAsync();

            return confronto;
        }

        private bool ConfrontoExists(long id)
        {
            return _contexto.Confrontos.Any(e => e.Id == id);
        }
    }
}
