using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Refere-se a mensagens da Plataforma relevantes aos Usuários. Um anúncio de Competição, uma Promoção, um E-mail de confirmação...
    /// </summary>
    [Table("Notificacoes")]
    public class Notificacao
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Usuarios")]
        public long NotificadoId { get; set; }
        public virtual Usuario? Notificado { get; set; }

        [Required]
        public string Titulo { get; set; } = "";

        public string? Descricao { get; set; } = "";

        [Required]
        public DateTime DataPublicacao { get; set; }
        public DateTime? DataExpiracao { get; set; }

        public string Link { get; set; } = "";

        public string? TipoAnuncio { get; set; }
    }
}
