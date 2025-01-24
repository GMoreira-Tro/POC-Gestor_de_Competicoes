using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Services
{
    public class PagamentoContaCorrenteService
    {
        private readonly Contexto _contexto;

        public PagamentoContaCorrenteService(Contexto contexto)
        {
            _contexto = contexto;
        }

       public async Task<PagamentoContaCorrente> ValidarPagamentoContaCorrente(PagamentoContaCorrente pagamentoContaCorrente)
        {
            // Verifica se o PagamentoId está definido
            if (pagamentoContaCorrente.PagamentoId <= 0)
            {
                throw new CampoObrigatorioException("PagamentoId");
            }

            // Verifica se a ContaCorrenteId está definida
            if (pagamentoContaCorrente.ContaCorrenteId <= 0)
            {
                throw new CampoObrigatorioException("ContaCorrenteId");
            }

            // Verifica se já existem duas contas correntes associadas ao mesmo PagamentoId
            var contasExistentes = await _contexto.PagamentoContaCorrentes
                .Where(p => p.PagamentoId == pagamentoContaCorrente.PagamentoId)
                .ToListAsync();

            if (contasExistentes.Count >= 2)
            {
                throw new InvalidOperationException($"Já existem duas contas correntes associadas ao Pagamento com ID {pagamentoContaCorrente.PagamentoId}.");
            }

            // Verifica se a conta corrente solicitante já existe
            if (contasExistentes.Any(c => c.ContaCorrenteSolicitante))
            {
                if (pagamentoContaCorrente.ContaCorrenteSolicitante)
                {
                    throw new InvalidOperationException("Já existe uma Conta Corrente solicitante associada a este Pagamento.");
                }
            }

            // Verifica se a nova conta corrente a ser associada não é do solicitante, se já houver uma
            if (contasExistentes.Count == 1 && !contasExistentes.First().ContaCorrenteSolicitante && !pagamentoContaCorrente.ContaCorrenteSolicitante)
            {
                throw new InvalidOperationException("Se não houver uma Conta Corrente solicitante, a nova conta deve ser do solicitante.");
            }

            return pagamentoContaCorrente;
        }


        public bool PagamentoContaCorrenteExists(long id)
        {
            return _contexto.PagamentoContaCorrentes.Any(pi => pi.Id == id);
        }
    }
}
