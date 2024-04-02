using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class InscricaoService
    {
        private readonly Contexto _contexto;
        public InscricaoService(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<Inscricao> ValidarInscricao(Inscricao inscricao)
        {
            if (inscricao.CategoriaId <= 0)
            {
                throw new CampoObrigatorioException("Categoria");
            }

            if (inscricao.UsuarioId <= 0)
            {
                throw new CampoObrigatorioException("Usuário");
            }

            inscricao.Usuario = await _contexto.FindAsync<Usuario>(inscricao.UsuarioId);

            if (!Enum.IsDefined(typeof(StatusPagamento), inscricao.StatusPagamento))
            {
                throw new StatusInscricaoInvalidoException();
            }


            if (string.IsNullOrWhiteSpace(inscricao.NomeAtleta))
            {
                throw new CampoObrigatorioException("Nome do Atleta");
            }

            inscricao.ConfrontoInscricoes ??= [];
            return inscricao;
        }

        public bool InscricaoExists(long id)
        {
            return _contexto.Inscricoes.Any(e => e.Id == id);
        }
    }
}
