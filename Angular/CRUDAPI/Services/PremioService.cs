using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class PremioService
    {
        private readonly Contexto _contexto;

        public PremioService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Premio> ValidarPremio(Premio premio)
        {
            //TODO: Implemente a lógica de validação do Premio de inscrição aqui, se necessário

            return premio;
        }

        public bool PremioExists(long id)
        {
            return _contexto.Premios.Any(pi => pi.Id == id);
        }
    }
}
