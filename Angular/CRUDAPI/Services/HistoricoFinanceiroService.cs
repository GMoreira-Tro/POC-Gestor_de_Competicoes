using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Services
{
    public class HistoricoFinanceiroService
    {
        private readonly Contexto _contexto;

        public HistoricoFinanceiroService(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<HistoricoFinanceiro> ValidarHistoricoFinanceiro(HistoricoFinanceiro historicoFinanceiro)
        {
            //TODO: implementar validação
            var usuarioExistente = await _contexto.HistoricosFinanceiros.AnyAsync(u => u.UsuarioId == historicoFinanceiro.UsuarioId);
            if (usuarioExistente)
            {
                throw new HistoricoFinanceiroJaPossuiUsuarioException();
            }

            return historicoFinanceiro;
        }

         public bool HistoricoFinanceiroExists(long id)
        {
            return _contexto.HistoricosFinanceiros.Any(e => e.Id == id);
        }
    }
}
