using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChaveamentoController : ControllerBase
    {
        private readonly Contexto _contexto;

        public ChaveamentoController(Contexto contexto)
        {
            _contexto = contexto;
        }

        // GET: api/Chaveamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chaveamento>>> GetChaveamentos()
        {
            return await _contexto.Chaveamentos.ToListAsync();
        }

        // GET: api/Chaveamento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chaveamento>> GetChaveamento(long id)
        {
            var chaveamento = await _contexto.Chaveamentos
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chaveamento == null)
            {
                return NotFound();
            }

            return chaveamento;
        }

        // POST: api/Chaveamento
        [HttpPost]
        public async Task<ActionResult<Chaveamento>> PostChaveamento(Chaveamento chaveamento)
        {
            try
            {
                _contexto.Chaveamentos.Add(chaveamento);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetChaveamento), new { id = chaveamento.Id }, chaveamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Chaveamento/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChaveamento(long id, Chaveamento chaveamento)
        {
            if (id != chaveamento.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(chaveamento).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChaveamentoExists(id))
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

        // DELETE: api/Chaveamento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChaveamento(long id)
        {
            var chaveamento = await _contexto.Chaveamentos.FindAsync(id);
            if (chaveamento == null)
            {
                return NotFound();
            }

            _contexto.Chaveamentos.Remove(chaveamento);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("buscar-de-categoria/{idCategoria}")]
        public async Task<ActionResult<IEnumerable<Chaveamento>>> GetChaveamentosPorCategoria(long idCategoria)
        {
            var chaveamentos = await _contexto.Chaveamentos
                .Where(c => c.CategoriaId == idCategoria)
                .ToListAsync();

            if (chaveamentos == null || !chaveamentos.Any())
            {
                return NotFound();
            }

            return chaveamentos;
        }

        private bool ChaveamentoExists(long id)
        {
            return _contexto.Chaveamentos.Any(e => e.Id == id);
        }
    }
}