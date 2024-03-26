using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class PagamentoInscricaoService
    {
        private readonly Contexto _contexto;

        public PagamentoInscricaoService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<PagamentoInscricao> ValidarPagamentoInscricao(PagamentoInscricao pagamentoInscricao)
        {
            // Implemente a lógica de validação do pagamento de inscrição aqui, se necessário

            return pagamentoInscricao;
        }

        public bool PagamentoInscricaoExists(long id)
        {
            return _contexto.PagamentoInscricoes.Any(pi => pi.Id == id);
        }
    }
}
