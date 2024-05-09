using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CRUDAPI.Models
{
    public enum Role
    {
        Admin,
        Cliente
    }
    /// <summary>
    /// Usuário do Sistema. Pode ser tanto uma pessoa física quanto jurídica. Os Usuários serão responsáveis por cadastrar os Competidores e as Competições.
    /// Admins também deverão ter seus respectivos Usuários cadastrados.
    /// </summary>
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Nome { get; set; } = "";
        [Required]
        public string Sobrenome { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        /// <summary>
        /// Indica se o usuário confirmou seu cadastro.
        /// </summary>
        public bool EmailConfirmado {get; set; } = false;

        /// <summary>
        /// Senha salva após encriptação.
        /// </summary>
        [Required]
        public string SenhaHash { get; set; } = "";
        /// <summary>
        /// Sigla do País de acordo com a API geonames (BR, US, AR...).
        /// </summary>
        [Required]
        public string Pais { get; set; } = "";
        /// <summary>
        /// Nome do Estado de acordo com a API geonames.
        /// </summary>
        [Required]
        public string Estado { get; set; } = "";
        /// <summary>
        /// Nome da Cidade de acordo com a API geonames.
        /// </summary>
        [Required]
        public string Cidade { get; set; } = "";

        [Required]
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// CPF ou CNPJ visto que o Usuário pode ser tanto uma pessoa física quanto jurídica.
        /// </summary>
        [Required]
        public string CpfCnpj { get; set; } = "";

        /// <summary>
        /// Permissão de acesso do Usuário.
        /// </summary>
        [EnumDataType(typeof(Role))]
        public Role Role { get; set; }

        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
        public ICollection<Competidor> Competidores { get; set; } = new List<Competidor>();
        public ICollection<UsuarioNotificacao> AnunciosRecebidos { get; set; } = new List<UsuarioNotificacao>();
    }
}