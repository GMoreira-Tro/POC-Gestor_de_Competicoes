using System;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class ConfrontoService
    {
        private readonly Contexto _contexto;

        public ConfrontoService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Confronto> ValidarConfronto(Confronto confronto)
        {
            if (confronto.DataInicio >= confronto.DataTermino)
            {
                throw new Exception("A data de início do confronto deve ser anterior à data de término.");
            }
            confronto.ConfrontoInscricoes ??= [];
            return confronto;
        }

        public bool ConfrontoExists(long id)
        {
            return _contexto.Confrontos.Any(e => e.Id == id);
        }
    }
}
