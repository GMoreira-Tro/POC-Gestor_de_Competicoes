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

            var competidor = await _contexto.Competidores.FindAsync(inscricao.CompetidorId);
            if (competidor == null)
            {
                throw new KeyNotFoundException($"Competidor com ID {inscricao.CompetidorId} não encontrado.");
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
