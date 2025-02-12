using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;

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
                throw new CampoObrigatorioException("Nome");
            }

            if (string.IsNullOrEmpty(competidor.Email))
            {
                throw new CampoObrigatorioException("Email");
            }

            if(!Validators.IsValidEmail(competidor.Email))
            {
                throw new EmailInvalidoException();
            }

            var emailExistente = await _contexto.Competidores
            .AnyAsync(c => c.CriadorId == competidor.CriadorId && c.Email == competidor.Email &&
            c.Id != competidor.Id);

            if (emailExistente)
            {
                throw new EmailJaCadastradoEntreCompetidoresException(); // Indica que o email já está cadastrado entre os competidores do usuário
            }

            return competidor;
        }

        public bool CompetidorExists(long id)
        {
            return _contexto.Competidores.Any(e => e.Id == id);
        }
    }
}
