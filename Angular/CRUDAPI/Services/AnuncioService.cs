using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class AnuncioService
    {
        private readonly Contexto _contexto;

        public AnuncioService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Anuncio> ValidarAnuncio(Anuncio anuncio)
        {
            //TODO: Implemente a lógica de validação do Anuncio de inscrição aqui, se necessário

            return anuncio;
        }

        public bool AnuncioExists(long id)
        {
            return _contexto.Anuncios.Any(pi => pi.Id == id);
        }
    }
}
