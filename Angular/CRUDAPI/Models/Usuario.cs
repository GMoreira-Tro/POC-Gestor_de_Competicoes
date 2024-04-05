using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
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
        public bool EmailConfirmado {get; set; } = false;

        [Required]
        public string SenhaHash { get; set; } = "";
        [Required]
        public string Pais { get; set; } = "";
        [Required]
        public string Estado { get; set; } = "";
        [Required]
        public string Cidade { get; set; } = "";

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string CpfCnpj { get; set; } = "";

        public ICollection<Inscricao>? Inscricoes { get; set; }
        public ICollection<Competidor>? Competidores { get; set; }
        public ICollection<UsuarioNotificacao>? AnunciosRecebidos { get; set; }
    }
}