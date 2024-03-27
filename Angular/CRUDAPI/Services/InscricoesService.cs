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

            if (inscricao.IdUsuario <= 0)
            {
                throw new CampoObrigatorioException("Usuário");
            }

            inscricao.Usuario = await _contexto.FindAsync<Usuario>(inscricao.IdUsuario);

            if (!Enum.IsDefined(typeof(StatusPagamento), inscricao.StatusPagamento))
            {
                throw new StatusInscricaoInvalidoException();
            }


            if (string.IsNullOrWhiteSpace(inscricao.NomeAtleta))
            {
                throw new CampoObrigatorioException("Nome do Atleta");
            }

            Inscricao? inscricaoEtapaAnterior = await _contexto.FindAsync<Inscricao>(inscricao.InscricaoEtapaAnteriorId);
            Categoria? categoriaEtapaAnterior = await _contexto.FindAsync<Categoria>(inscricaoEtapaAnterior?.IdCategoria);
            Categoria? categoriaAtual = await _contexto.FindAsync<Categoria>(inscricao?.IdCategoria);
            Competicao? competicaoAtual =  await _contexto.FindAsync<Competicao>(categoriaAtual?.CompeticaoId);

            if (inscricao?.InscricaoEtapaAnteriorId != null && categoriaEtapaAnterior?.CompeticaoId != competicaoAtual?.EtapaAnteriorId)
            {
                throw new InscricaoDaEtapaAnteriorDeveSerReferenteACompeticaoDaEtapaAnteriorException();
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
