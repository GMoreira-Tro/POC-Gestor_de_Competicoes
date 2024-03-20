using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CRUDAPI.Services
{
    public class UsuarioService
    {
        private readonly Contexto _contexto;
        private readonly GeoNamesService _geonamesService;
        private const string GeoNamesBaseUrl = "http://api.geonames.org";

        public UsuarioService(Contexto contexto, GeoNamesService geonamesService)
        {
            _contexto = contexto;
            _geonamesService = geonamesService;
        }

        public async Task ValidarUsuario(Usuario usuario)
        {
            // Verifica se o e-mail já está cadastrado
            var emailExistente = await _contexto.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (emailExistente)
            {
                throw new EmailJaCadastradoException(); // Indica que o e-mail já está cadastrado
            }

            // Verifica se o CPF/CNPJ já está cadastrado
            var cpfCnpjExistente = await _contexto.Usuarios.AnyAsync(u => u.CpfCnpj == usuario.CpfCnpj);
            if (cpfCnpjExistente)
            {
                throw new CpfCnpjJaCadastradoException(); // Indica que o CPF/CNPJ já está cadastrado
            }

            // Valida o CPF/CNPJ
            if (!ValidarCPFOuCNPJ(usuario.CpfCnpj))
            {
                throw new CpfCnpjInvalidoException(); // Indica que o CPF/CNPJ é inválido
            }

            if (string.IsNullOrWhiteSpace(usuario.Nome))
            {
                throw new CampoObrigatorioException(usuario.Nome);
            }

            if (string.IsNullOrEmpty(usuario.Sobrenome))
            {
                throw new CampoObrigatorioException(usuario.Sobrenome);
            }

            if (string.IsNullOrEmpty(usuario.Pais))
            {
                throw new CampoObrigatorioException(usuario.Pais);
            }

            if (string.IsNullOrEmpty(usuario.Estado))
            {
                throw new CampoObrigatorioException(usuario.Estado);
            }

            if (string.IsNullOrEmpty(usuario.Cidade))
            {
                throw new CampoObrigatorioException(usuario.Cidade);
            }

            // Valida se o estado pertence ao país
            var estadoPertenceAoPais = await _geonamesService.EstadoPertenceAoPais(usuario.Pais, usuario.Estado);
            if (!estadoPertenceAoPais)
            {
                throw new EstadoNaoPertenceAoPaisException(); // Indica que o estado não pertence ao país
            }

            // Valida se a cidade pertence ao estado
            var cidadePertenceAoEstado = await _geonamesService.CidadePertenceAoEstado(usuario.Estado, usuario.Cidade);
            if (!cidadePertenceAoEstado)
            {
                throw new CidadeNaoPertenceAoEstadoException(); // Indica que a cidade não pertence ao estado
            }
        }

        // Função para validar CPF ou CNPJ
        public bool ValidarCPFOuCNPJ(string documento)
        {
            CPFCNPJ.Main main = new();
            return main.IsValidCPFCNPJ(documento);
        }

        public bool UsuarioExists(long id)
        {
            return _contexto.Usuarios.Any(e => e.Id == id);
        }

    }
}
