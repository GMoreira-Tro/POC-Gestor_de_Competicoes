using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class EstornoService
    {
        private readonly Contexto _contexto;

        public EstornoService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Estorno> ValidarEstorno(Estorno estorno)
        {
            //TODO: Implemente a lógica de validação do Estorno de inscrição aqui, se necessário

            return estorno;
        }

        public bool EstornoExists(long id)
        {
            return _contexto.Estornos.Any(pi => pi.Id == id);
        }
    }
}
