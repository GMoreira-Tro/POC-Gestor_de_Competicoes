using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class UsuarioAnuncioService
    {
        private readonly Contexto _contexto;

        public UsuarioAnuncioService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<UsuarioAnuncio> ValidarUsuarioAnuncio(UsuarioAnuncio usuarioAnuncio)
        {
            //TODO: Implemente a lógica de validação do UsuarioAnuncio de inscrição aqui, se necessário

            return usuarioAnuncio;
        }

        public bool UsuarioAnuncioExists(long id)
        {
            return _contexto.UsuarioAnuncios.Any(pi => pi.Id == id);
        }
    }
}
