using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class UsuarioNotificacaoService
    {
        private readonly Contexto _contexto;

        public UsuarioNotificacaoService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<UsuarioNotificacao> ValidarUsuarioNotificacao(UsuarioNotificacao usuarioNotificacao)
        {
            //TODO: Implemente a lógica de validação do UsuarioNotificacao de inscrição aqui, se necessário

            return usuarioNotificacao;
        }

        public bool UsuarioNotificacaoExists(long id)
        {
            return _contexto.UsuarioNotificacoes.Any(pi => pi.Id == id);
        }
    }
}
