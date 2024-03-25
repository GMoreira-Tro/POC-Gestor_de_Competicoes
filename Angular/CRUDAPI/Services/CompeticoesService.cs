using System;
using System.Threading.Tasks;
using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRUDAPI.Services
{
    public class CompeticaoService
    {
        private readonly Contexto _contexto;
        private readonly GeoNamesService _geonamesService;

        public CompeticaoService(Contexto contexto, GeoNamesService geoNamesService)
        {
            _contexto = contexto;
            _geonamesService = geoNamesService;
        }

        public async Task<Competicao> ValidarCompeticao(Competicao competicao)
        {
            if (string.IsNullOrWhiteSpace(competicao.Titulo))
            {
                throw new CampoObrigatorioException("Título da competição é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(competicao.Modalidade))
            {
                throw new CampoObrigatorioException("Modalidade da competição é obrigatória.");
            }

            if (competicao.DataInicio == default)
            {
                throw new CampoObrigatorioException("Data de início da competição é obrigatória.");
            }

            if(!competicao.Estado.IsNullOrEmpty())
            {
                // Valida se o estado pertence ao país
                var estadoPertenceAoPais = await _geonamesService.EstadoPertenceAoPais(competicao.Pais, competicao.Estado);
                if (!estadoPertenceAoPais)
                {
                    throw new EstadoNaoPertenceAoPaisException(); // Indica que o estado não pertence ao país
                }

            }

            if(!competicao.Cidade.IsNullOrEmpty())
            {
                // Valida se a cidade pertence ao estado
                var cidadePertenceAoEstado = await _geonamesService.CidadePertenceAoPaisEEstado(competicao.Pais, competicao.Estado, competicao.Cidade);
                if (!cidadePertenceAoEstado)
                {
                    throw new CidadeNaoPertenceAoEstadoException(); // Indica que a cidade não pertence ao estado
                }
            }

            competicao.Usuario = await _contexto.Usuarios.FindAsync(competicao.IdCriadorUsuario);
            competicao.Categorias ??= [];

            return competicao;
        }

        public bool CompeticaoExists(long id)
        {
            return _contexto.Competicoes.Any(e => e.Id == id);
        }
    }
}
