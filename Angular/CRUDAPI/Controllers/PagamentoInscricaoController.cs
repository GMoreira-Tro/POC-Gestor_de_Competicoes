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
    public class PagamentoInscricaoController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly PagamentoInscricaoService _pagamentoInscricaoService;

        public PagamentoInscricaoController(Contexto contexto, PagamentoInscricaoService pagamentoInscricaoService)
        {
            _contexto = contexto;
            _pagamentoInscricaoService = pagamentoInscricaoService;
        }

        // GET: api/PagamentoInscricao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagamentoInscricao>>> GetPagamentoInscricoes()
        {
            return await _contexto.PagamentoInscricoes.ToListAsync();
        }

        // GET: api/PagamentoInscricao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PagamentoInscricao>> GetPagamentoInscricao(long id)
        {
            var pagamentoInscricao = await _contexto.PagamentoInscricoes.FindAsync(id);

            if (pagamentoInscricao == null)
            {
                return NotFound();
            }

            return pagamentoInscricao;
        }

        // POST: api/PagamentoInscricao
        [HttpPost]
        public async Task<ActionResult<PagamentoInscricao>> PostPagamentoInscricao(PagamentoInscricao pagamentoInscricao)
        {
            try
            {
                pagamentoInscricao = await _pagamentoInscricaoService.ValidarPagamentoInscricao(pagamentoInscricao);

                _contexto.PagamentoInscricoes.Add(pagamentoInscricao);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPagamentoInscricao), new { id = pagamentoInscricao.Id }, pagamentoInscricao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/PagamentoInscricao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPagamentoInscricao(long id, PagamentoInscricao pagamentoInscricao)
        {
            if (id != pagamentoInscricao.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(pagamentoInscricao).State = EntityState.Modified;

            try
            {
                pagamentoInscricao = await _pagamentoInscricaoService.ValidarPagamentoInscricao(pagamentoInscricao);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_pagamentoInscricaoService.PagamentoInscricaoExists(id))
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

        // DELETE: api/PagamentoInscricao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePagamentoInscricao(long id)
        {
            var pagamentoInscricao = await _contexto.PagamentoInscricoes.FindAsync(id);
            if (pagamentoInscricao == null)
            {
                return NotFound();
            }

            _contexto.PagamentoInscricoes.Remove(pagamentoInscricao);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}
