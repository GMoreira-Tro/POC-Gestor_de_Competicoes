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
    public class ContaCorrenteController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly ContaCorrenteService _contaCorrenteService;

        public ContaCorrenteController(Contexto contexto, ContaCorrenteService contaCorrenteService)
        {
            _contexto = contexto;
            _contaCorrenteService = contaCorrenteService;
        }

        // GET: api/ContaCorrente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContaCorrente>>> GetContasCorrente()
        {
            return await _contexto.ContasCorrentes.ToListAsync();
        }

        // GET: api/ContaCorrente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContaCorrente>> GetContaCorrente(long id)
        {
            var ContaCorrente = await _contexto.ContasCorrentes.FindAsync(id);

            if (ContaCorrente == null)
            {
                return NotFound();
            }

            return ContaCorrente;
        }

        // POST: api/ContaCorrente
        [HttpPost]
        public async Task<ActionResult<ContaCorrente>> PostContaCorrente(ContaCorrente ContaCorrente)
        {
            try
            {
                ContaCorrente = await _contaCorrenteService.ValidarContaCorrente(ContaCorrente);
                _contexto.ContasCorrentes.Add(ContaCorrente);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetContaCorrente), new { id = ContaCorrente.Id }, ContaCorrente);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao salvar o ContaCorrente: {ex.Message}");
            }
        }

        // PUT: api/ContaCorrente/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContaCorrente(long id, ContaCorrente ContaCorrente)
        {
            if (id != ContaCorrente.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(ContaCorrente).State = EntityState.Modified;

            try
            {
                ContaCorrente = await _contaCorrenteService.ValidarContaCorrente(ContaCorrente);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContaCorrenteExists(id))
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

        // DELETE: api/ContaCorrente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContaCorrente>> DeleteContaCorrente(long id)
        {
            var ContaCorrente = await _contexto.ContasCorrentes.FindAsync(id);
            if (ContaCorrente == null)
            {
                return NotFound();
            }

            _contexto.ContasCorrentes.Remove(ContaCorrente);
            await _contexto.SaveChangesAsync();

            return ContaCorrente;
        }

        private bool ContaCorrenteExists(long id)
        {
            return _contexto.ContasCorrentes.Any(e => e.Id == id);
        }
    }
}
