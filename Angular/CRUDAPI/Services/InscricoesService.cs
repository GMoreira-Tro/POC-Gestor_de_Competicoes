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
        public void ValidarCamposObrigatorios(Inscricao inscricao)
        {
            if (inscricao.IdCategoria <= 0)
            {
                throw new CampoObrigatorioException("Categoria");
            }

            if (inscricao.IdUsuario <= 0)
            {
                throw new CampoObrigatorioException("Usuário");
            }

            if (string.IsNullOrWhiteSpace(inscricao.Status))
            {
                throw new CampoObrigatorioException("Status");
            }

            if (string.IsNullOrWhiteSpace(inscricao.NomeAtleta))
            {
                throw new CampoObrigatorioException("Nome do Atleta");
            }
        }

        public bool InscricaoExists(long id)
        {
            return _contexto.Inscricoes.Any(e => e.Id == id);
        }
    }
}
