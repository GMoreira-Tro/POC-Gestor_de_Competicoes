using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class ConviteCompetidorService
    {
        private readonly Contexto _contexto;

        public ConviteCompetidorService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<ConviteCompetidor> ValidarConviteCompetidor(ConviteCompetidor ConviteCompetidor)
        {            
            return ConviteCompetidor;
        }

        public bool ConviteCompetidorExists(long id)
        {
            return _contexto.ConvitesCompetidores.Any(ci => ci.Id == id);
        }
    }
}
