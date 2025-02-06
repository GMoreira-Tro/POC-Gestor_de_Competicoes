using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;

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
            // Verifica se a categoria está definida
            if (inscricao.CategoriaId <= 0)
            {
                throw new CampoObrigatorioException("Categoria");
            }

            // Verifica se já existe uma inscrição com o mesmo TimeID e CategoriaID
            var inscricaoExistente = await _contexto.Inscricoes
                .AnyAsync(i => i.CompetidorId == inscricao.CompetidorId && i.CategoriaId == inscricao.CategoriaId && i.Id != inscricao.Id);

            if (inscricaoExistente)
            {
                throw new InvalidOperationException($"O Competidor com ID {inscricao.CompetidorId} já possui uma inscrição na Categoria com ID {inscricao.CategoriaId}.");
            }

            inscricao.ConfrontoInscricoes ??= new List<ConfrontoInscricao>();
            return inscricao;
        }


        public bool InscricaoExists(long id)
        {
            return _contexto.Inscricoes.Any(e => e.Id == id);
        }
    }
}
