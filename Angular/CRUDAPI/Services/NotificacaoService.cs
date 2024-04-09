using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class NotificacaoService
    {
        private readonly Contexto _contexto;

        public NotificacaoService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Notificacao> ValidarNotificacao(Notificacao notificacao)
        {
            //TODO: Implemente a lógica de validação do Notificacao de inscrição aqui, se necessário

            return notificacao;
        }

        public bool NotificacaoExists(long id)
        {
            return _contexto.Notificacoes.Any(pi => pi.Id == id);
        }
    }
}
