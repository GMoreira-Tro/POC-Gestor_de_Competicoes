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
    public class PagamentoController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly PagamentoService _pagamentoService;

        public PagamentoController(Contexto contexto, PagamentoService pagamentoService)
        {
            _contexto = contexto;
            _pagamentoService = pagamentoService;
        }

        // GET: api/Pagamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentos()
        {
            return await _contexto.Pagamentos.ToListAsync();
        }

        // GET: api/Pagamento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pagamento>> GetPagamento(long id)
        {
            var pagamento = await _contexto.Pagamentos.FindAsync(id);

            if (pagamento == null)
            {
                return NotFound();
            }

            return pagamento;
        }

        // POST: api/Pagamento
        [HttpPost]
        public async Task<ActionResult<Pagamento>> PostPagamento(Pagamento pagamento)
        {
            try
            {
                pagamento = await _pagamentoService.ValidarPagamento(pagamento);

                _contexto.Pagamentos.Add(pagamento);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPagamento), new { id = pagamento.Id }, pagamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Pagamento/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPagamento(long id, Pagamento pagamento)
        {
            if (id != pagamento.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(pagamento).State = EntityState.Modified;

            try
            {
                pagamento = await _pagamentoService.ValidarPagamento(pagamento);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_pagamentoService.PagamentoExists(id))
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

        // DELETE: api/Pagamento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePagamento(long id)
        {
            var pagamento = await _contexto.Pagamentos.FindAsync(id);
            if (pagamento == null)
            {
                return NotFound();
            }

            _contexto.Pagamentos.Remove(pagamento);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
