using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDAPI.Models;

namespace CRUDAPI.Services
{
    public class CategoriaService
    {
        private readonly Contexto _contexto;

        public CategoriaService(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<Categoria> ValidarCategoria(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nome))
            {
                throw new CampoObrigatorioException("O nome da categoria é obrigatório.");
            }

            if (categoria.CompeticaoId <= 0)
            {
                throw new CampoObrigatorioException("O ID da competição é obrigatório.");
            }

            return categoria;
        }

         public bool CategoriaExists(long id)
        {
            return _contexto.Categorias.Any(e => e.Id == id);
        }
    }
}
