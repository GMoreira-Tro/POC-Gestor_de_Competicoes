using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class ConviteService
    {
        private readonly Contexto _contexto;

        public ConviteService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Convite> ValidarConvite(Convite convite)
        {
            //TODO: Implemente a lógica de validação do Convite de inscrição aqui, se necessário

            return convite;
        }

        public bool ConviteExists(long id)
        {
            return _contexto.Premios.Any(pi => pi.Id == id);
        }
    }
}
