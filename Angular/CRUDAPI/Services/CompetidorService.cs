using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class CompetidorService
    {
        private readonly Contexto _contexto;

        public CompetidorService(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<Competidor> ValidarCompetidor(Competidor competidor)
        {
            if (string.IsNullOrWhiteSpace(competidor.Nome))
            {
                throw new CampoObrigatorioException("O nome do competidor é obrigatório.");
            }

            return competidor;
        }

         public bool CompetidorExists(long id)
        {
            return _contexto.Competidores.Any(e => e.Id == id);
        }
    }
}
