using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Services
{
    public class ConfrontoInscricaoService
    {
        private readonly Contexto _contexto;

        public ConfrontoInscricaoService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<ConfrontoInscricao> ValidarConfrontoInscricao(ConfrontoInscricao confrontoInscricao)
        {
            // Verifica se o confronto existe
            var confronto = await _contexto.Confrontos.FindAsync(confrontoInscricao.ConfrontoId);
            if (confronto == null)
            {
                throw new KeyNotFoundException($"Confronto com ID {confrontoInscricao.ConfrontoId} não encontrado.");
            }

            // Verifica se a inscrição existe
            var inscricao = await _contexto.Inscricoes.FindAsync(confrontoInscricao.InscricaoId);
            if (inscricao == null)
            {
                throw new KeyNotFoundException($"Inscrição com ID {confrontoInscricao.InscricaoId} não encontrada.");
            }

            // Verifica se já existe uma associação com o mesmo par ConfrontoId e InscricaoId
            var existeParDuplicado = await _contexto.ConfrontoInscricoes
                .AnyAsync(ci => ci.ConfrontoId == confrontoInscricao.ConfrontoId && ci.InscricaoId == confrontoInscricao.InscricaoId);

            if (existeParDuplicado)
            {
                throw new InvalidOperationException($"Já existe uma associação com o ConfrontoId {confrontoInscricao.ConfrontoId} e InscricaoId {confrontoInscricao.InscricaoId}.");
            }

            // Verifica se a CategoriaId da inscrição é a mesma do chaveamento do confronto
            var chaveamento = await _contexto.Chaveamentos.FindAsync(confronto.ChaveamentoId);
            if (chaveamento?.CategoriaId != inscricao.CategoriaId)
            {
                throw new InvalidOperationException("A CategoriaId da inscrição deve ser a mesma do chaveamento do confronto.");
            }

            // Verifica se já existem duas inscrições associadas ao confronto
            var inscricoesNoConfronto = await _contexto.ConfrontoInscricoes
                .Where(ci => ci.ConfrontoId == confrontoInscricao.ConfrontoId)
                .Select(ci => ci.InscricaoId)
                .ToListAsync();

            if (inscricoesNoConfronto.Count >= 2)
            {
                throw new InvalidOperationException($"O Confronto de Inscrições com ID {confrontoInscricao.ConfrontoId} já possui 2 inscrições.");
            }

            // Se já houver uma inscrição associada ao confronto, verifica se as categorias são iguais
            if (inscricoesNoConfronto.Count == 1)
            {
                var outraInscricao = await _contexto.Inscricoes.FindAsync(inscricoesNoConfronto.First());
                if (outraInscricao?.CategoriaId != inscricao.CategoriaId)
                {
                    throw new InvalidOperationException("As duas inscrições associadas ao confronto devem pertencer à mesma categoria.");
                }
            }

            return confrontoInscricao;
        }


        public bool ConfrontoInscricaoExists(long id)
        {
            return _contexto.ConfrontoInscricoes.Any(ci => ci.Id == id);
        }
    }
}
