using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class PagamentoService
    {
        private readonly Contexto _contexto;

        public PagamentoService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Pagamento> ValidarPagamento(Pagamento pagamento)
        {
            //TODO: Implemente a lógica de validação do pagamento de inscrição aqui, se necessário

            return pagamento;
        }

        public bool PagamentoExists(long id)
        {
            return _contexto.Pagamentos.Any(pi => pi.Id == id);
        }
    }
}
