using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Notificacoes")]
    public class Notificacao
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Titulo { get; set; } = "";

        public string? Descricao { get; set; } = "";

        [Required]
        public DateTime DataPublicacao { get; set; }
        public DateTime? DataExpiracao { get; set; }

        [ForeignKey("Usuarios")]
        public long AnuncianteId { get; set; }
        public virtual Usuario? Anunciante { get; set; }

        public string? BannerImagem { get; set; }

        public string? TipoAnuncio { get; set; }
        public ICollection<UsuarioNotificacao>? UsuariosAlvo { get; set; }
    }
}
