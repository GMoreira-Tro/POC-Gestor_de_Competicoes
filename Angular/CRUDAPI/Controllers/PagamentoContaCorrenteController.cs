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
    public class PagamentoContaCorrenteController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly PagamentoContaCorrenteService _pagamentoContaCorrenteService;

        public PagamentoContaCorrenteController(Contexto contexto, PagamentoContaCorrenteService pagamentoContaCorrenteService)
        {
            _contexto = contexto;
            _pagamentoContaCorrenteService = pagamentoContaCorrenteService;
        }

        // GET: api/PagamentoContaCorrente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagamentoContaCorrente>>> GetContasCorrente()
        {
            return await _contexto.PagamentoContaCorrentes.ToListAsync();
        }

        // GET: api/PagamentoContaCorrente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PagamentoContaCorrente>> GetPagamentoContaCorrente(long id)
        {
            var PagamentoContaCorrente = await _contexto.PagamentoContaCorrentes.FindAsync(id);

            if (PagamentoContaCorrente == null)
            {
                return NotFound();
            }

            return PagamentoContaCorrente;
        }

        // POST: api/PagamentoContaCorrente
        [HttpPost]
        public async Task<ActionResult<PagamentoContaCorrente>> PostPagamentoContaCorrente(PagamentoContaCorrente PagamentoContaCorrente)
        {
            try
            {
                PagamentoContaCorrente = await _pagamentoContaCorrenteService.ValidarPagamentoContaCorrente(PagamentoContaCorrente);
                _contexto.PagamentoContaCorrentes.Add(PagamentoContaCorrente);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPagamentoContaCorrente), new { id = PagamentoContaCorrente.Id }, PagamentoContaCorrente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao salvar o PagamentoContaCorrente: {ex.Message}");
            }
        }

        // PUT: api/PagamentoContaCorrente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPagamentoContaCorrente(long id, PagamentoContaCorrente PagamentoContaCorrente)
        {
            if (id != PagamentoContaCorrente.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(PagamentoContaCorrente).State = EntityState.Modified;

            try
            {
                PagamentoContaCorrente = await _pagamentoContaCorrenteService.ValidarPagamentoContaCorrente(PagamentoContaCorrente);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagamentoContaCorrenteExists(id))
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

        // DELETE: api/PagamentoContaCorrente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PagamentoContaCorrente>> DeletePagamentoContaCorrente(long id)
        {
            var PagamentoContaCorrente = await _contexto.PagamentoContaCorrentes.FindAsync(id);
            if (PagamentoContaCorrente == null)
            {
                return NotFound();
            }

            _contexto.PagamentoContaCorrentes.Remove(PagamentoContaCorrente);
            await _contexto.SaveChangesAsync();

            return PagamentoContaCorrente;
        }

        private bool PagamentoContaCorrenteExists(long id)
        {
            return _contexto.PagamentoContaCorrentes.Any(e => e.Id == id);
        }
    }
}
