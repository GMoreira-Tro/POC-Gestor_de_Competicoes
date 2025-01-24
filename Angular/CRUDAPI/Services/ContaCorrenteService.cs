using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class ContaCorrenteService
    {
        private readonly Contexto _contexto;

        public ContaCorrenteService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<ContaCorrente> ValidarContaCorrente(ContaCorrente contaCorrente)
        {
            if(contaCorrente.Saldo < 0)
            {
                throw new Exception("O saldo não pode ser negativo.");
            }
            return contaCorrente;
        }

        public bool ContaCorrenteExists(long id)
        {
            return _contexto.ContasCorrentes.Any(e => e.Id == id);
        }
    }
}
