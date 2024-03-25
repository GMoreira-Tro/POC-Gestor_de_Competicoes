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
            if (inscricao.IdCategoria <= 0)
            {
                throw new CampoObrigatorioException("Categoria");
            }

            inscricao.Categoria = await _contexto.FindAsync<Categoria>(inscricao.IdCategoria);

            if (inscricao.IdUsuario <= 0)
            {
                throw new CampoObrigatorioException("Usuário");
            }

            inscricao.Usuario = await _contexto.FindAsync<Usuario>(inscricao.IdUsuario);

            if (string.IsNullOrWhiteSpace(inscricao.Status))
            {
                throw new CampoObrigatorioException("Status");
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
