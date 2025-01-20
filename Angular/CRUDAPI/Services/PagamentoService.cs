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
            if (pagamento.Valor <= 0)
            {
                throw new CampoObrigatorioException("Valor deve ser maior que zero.");
            }

            if (string.IsNullOrWhiteSpace(pagamento.Moeda))
            {
                throw new CampoObrigatorioException("Moeda é obrigatória.");
            }

            if (pagamento.DataRequisicao > DateTime.Now)
            {
                throw new CampoObrigatorioException("Data de requisição não pode ser no futuro.");
            }

            if (!Enum.IsDefined(typeof(Status), pagamento.Status))
            {
                throw new CampoObrigatorioException("Status inválido.");
            }

            var aprovador = await _contexto.Usuarios.FindAsync(pagamento.AprovadorId);
            if (aprovador == null)
            {
                throw new KeyNotFoundException($"Aprovador com ID {pagamento.AprovadorId} não encontrado.");
            }

            if (string.IsNullOrWhiteSpace(pagamento.TokenPagSeguro))
            {
                throw new CampoObrigatorioException("Token do PagSeguro é obrigatório.");
            }

            return pagamento;
        }

        public bool PagamentoExists(long id)
        {
            return _contexto.Pagamentos.Any(pi => pi.Id == id);
        }
    }
}
