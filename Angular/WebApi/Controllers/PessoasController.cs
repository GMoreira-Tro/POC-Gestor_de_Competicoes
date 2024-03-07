using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers {
    [ApiController]
    [Route ("api/[controller]")]
    public class PessoasController : ControllerBase {
        private readonly Contexto _contexto;

        public PessoasController (Contexto contexto) {
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetTodasPessoasAsync () {
            return await _contexto.Pessoas.ToListAsync ();
        }

        [HttpGet ("{pessoaId}")]
        public async Task<ActionResult<Pessoa>> GetPessoaPeloIdAsync (int pessoaId) {
            Pessoa pessoa = await _contexto.Pessoas.FindAsync (pessoaId);

            if (pessoa == null)
                return NotFound ();

            return pessoa;
        }

        [HttpPost]
        public async Task<ActionResult<Pessoa>> SalvarPessoaAsync (Pessoa pessoa) {
            await _contexto.Pessoas.AddAsync (pessoa);
            await _contexto.SaveChangesAsync ();

            return Ok ();
        }

        [HttpPut]
        public async Task<ActionResult> AtualizarPessoaAsync (Pessoa pessoa) {
            _contexto.Pessoas.Update (pessoa);
            await _contexto.SaveChangesAsync ();

            return Ok ();
        }

        [HttpDelete ("{pessoaId}")]
        public async Task<ActionResult> ExcluirPessoaAsync (int pessoaId) {
            Pessoa pessoa = await _contexto.Pessoas.FindAsync (pessoaId);
            if (pessoa == null)
                return NotFound ();

            _contexto.Remove (pessoa);
            await _contexto.SaveChangesAsync ();
            return Ok ();
        }

    }
}