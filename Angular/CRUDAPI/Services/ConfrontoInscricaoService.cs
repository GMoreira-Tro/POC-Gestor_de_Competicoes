using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

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
            var confronto = await _contexto.Confrontos.FindAsync(confrontoInscricao.ConfrontoId);
            var inscricao = await _contexto.Inscricoes.FindAsync(confrontoInscricao.InscricaoId);

            if (confronto == null)
            {
                throw new KeyNotFoundException($"Confronto com ID {confrontoInscricao.ConfrontoId} não encontrado.");
            }

            if (inscricao == null)
            {
                throw new KeyNotFoundException($"Inscrição com ID {confrontoInscricao.InscricaoId} não encontrada.");
            }
            
            return confrontoInscricao;
        }

        public bool ConfrontoInscricaoExists(long id)
        {
            return _contexto.ConfrontoInscricoes.Any(ci => ci.Id == id);
        }
    }
}
