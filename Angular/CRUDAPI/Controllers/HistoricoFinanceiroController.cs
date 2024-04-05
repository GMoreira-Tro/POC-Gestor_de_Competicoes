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
    public class HistoricoFinanceiroController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly HistoricoFinanceiroService _historicoFinanceiroService;

        public HistoricoFinanceiroController(Contexto contexto, HistoricoFinanceiroService historicoFinanceiroService)
        {
            _contexto = contexto;
            _historicoFinanceiroService = historicoFinanceiroService;
        }

        // GET: api/HistoricoFinanceiro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricoFinanceiro>>> GetHistoricosFinanceiros()
        {
            return await _contexto.HistoricosFinanceiros.ToListAsync();
        }

        // GET: api/HistoricoFinanceiro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoricoFinanceiro>> GetHistoricoFinanceiro(long id)
        {
            var historicoFinanceiro = await _contexto.HistoricosFinanceiros.FindAsync(id);

            if (historicoFinanceiro == null)
            {
                return NotFound();
            }

            return historicoFinanceiro;
        }

        // PUT: api/HistoricoFinanceiro/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoricoFinanceiro(long id, HistoricoFinanceiro historicoFinanceiro)
        {
            if (id != historicoFinanceiro.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(historicoFinanceiro).State = EntityState.Modified;

            try
            {
                historicoFinanceiro = await _historicoFinanceiroService.ValidarHistoricoFinanceiro(historicoFinanceiro);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_historicoFinanceiroService.HistoricoFinanceiroExists(id))
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

        // POST: api/HistoricoFinanceiro
        [HttpPost]
        public async Task<ActionResult<HistoricoFinanceiro>> PostHistoricoFinanceiro(HistoricoFinanceiro historicoFinanceiro)
        {
            try
            {
                historicoFinanceiro = await _historicoFinanceiroService.ValidarHistoricoFinanceiro(historicoFinanceiro);

                _contexto.HistoricosFinanceiros.Add(historicoFinanceiro);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetHistoricoFinanceiro), new { id = historicoFinanceiro.Id }, historicoFinanceiro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/HistoricoFinanceiro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoricoFinanceiro(long id)
        {
            var historicoFinanceiro = await _contexto.HistoricosFinanceiros.FindAsync(id);
            if (historicoFinanceiro == null)
            {
                return NotFound();
            }

            _contexto.HistoricosFinanceiros.Remove(historicoFinanceiro);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
