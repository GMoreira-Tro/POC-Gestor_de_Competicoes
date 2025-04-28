using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace CRUDAPI.Services
{
    public partial class UsuarioService
    {
        private readonly Contexto _contexto;
        private readonly GeoNamesService _geonamesService;
        private const string GeoNamesBaseUrl = "https://api.geonames.org";
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public UsuarioService(Contexto contexto, GeoNamesService geonamesService, EmailService emailService,
            IConfiguration configuration)
        {
            _contexto = contexto;
            _geonamesService = geonamesService;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<Usuario> ValidarUsuario(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome))
            {
                throw new CampoObrigatorioException("Nome");
            }

            if (string.IsNullOrEmpty(usuario.Sobrenome))
            {
                throw new CampoObrigatorioException("Sobrenome");
            }

            if (string.IsNullOrEmpty(usuario.Pais))
            {
                throw new CampoObrigatorioException("País");
            }

            if (string.IsNullOrEmpty(usuario.Estado))
            {
                throw new CampoObrigatorioException("Estado");
            }

            if (string.IsNullOrEmpty(usuario.Cidade))
            {
                throw new CampoObrigatorioException("Cidade");
            }

            if (string.IsNullOrEmpty(usuario.CpfCnpj))
            {
                throw new CampoObrigatorioException("Cpf/Cnpf");
            }

            if (string.IsNullOrEmpty(usuario.Email))
            {
                throw new CampoObrigatorioException("Email");
            }

            if(!Validators.IsValidEmail(usuario.Email))
            {
                throw new EmailInvalidoException();
            }

            var usuarioComEmailExistente = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);
            if (usuarioComEmailExistente != null && usuarioComEmailExistente.Id != usuario.Id)
            {
                throw new EmailJaCadastradoException(); // Indica que o e-mail já está cadastrado
            }

            if (!usuario.SenhaValidada && Validators.ValidarSenha(usuario.SenhaHash))
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);

                // Hash da senha com o salt
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash, salt);
                usuario.SenhaValidada = true;
            }

            // Verifica se o CPF/CNPJ já está cadastrado
            usuario.CpfCnpj = Regex.Replace(usuario.CpfCnpj, @"\D", ""); // Remove tudo que não for número
            var usuarioComCpfCnpjExistente = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.CpfCnpj == usuario.CpfCnpj);
            if (usuarioComCpfCnpjExistente != null && usuarioComCpfCnpjExistente.Id != usuario.Id)
            {
                throw new CpfCnpjJaCadastradoException(); // Indica que o CPF/CNPJ já está cadastrado
            }

            // Valida o CPF/CNPJ
            if (!ValidarCPFOuCNPJ(usuario.CpfCnpj))
            {
                throw new CpfCnpjInvalidoException(); // Indica que o CPF/CNPJ é inválido
            }

            // Valida se o estado pertence ao país
            var estadoPertenceAoPais = await _geonamesService.EstadoPertenceAoPais(usuario.Pais, usuario.Estado);
            if (!estadoPertenceAoPais)
            {
                throw new EstadoNaoPertenceAoPaisException(); // Indica que o estado não pertence ao país
            }

            // Valida se a cidade pertence ao estado
            var cidadePertenceAoEstado = await _geonamesService.CidadePertenceAoPaisEEstado(usuario.Pais, usuario.Estado, usuario.Cidade);
            if (!cidadePertenceAoEstado)
            {
                throw new CidadeNaoPertenceAoEstadoException(); // Indica que a cidade não pertence ao estado
            }

            return usuario;
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

        public async Task<bool> EnviarTokenConfirmacao(string tokenConfirmacao, string email)
        {
            // Envia e-mail de confirmação
            //TODO: Substituir pelo seu domínio
            string confirmationLink = $"{_configuration["Configuration:BaseUrl"]}/email-confirmado/{tokenConfirmacao}";
            await _emailService.SendEmailAsync(email, "Confirme seu e-mail", 
                $"Clique no link para confirmar seu e-mail: <a href='{confirmationLink}'>Confirmar</a>");

            return true;
        }
        public string GenerateToken(string email)
        {
            using (var sha256 = SHA256.Create())
            {
                // Combina o e-mail com a data/hora atual para gerar um hash único
                string data = email + DateTime.UtcNow.Ticks;
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hashBytes)
                            .Replace("+", "") // Remove caracteres problemáticos em URLs
                            .Replace("/", "")
                            .Replace("=", "");
            }
        }
    }
}
