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

        /// <summary>
        /// Id do Pagamento necessário para criar esta Notificação.
        /// </summary>
        [ForeignKey("Pagamentos")]
        public long? PagamentoId { get; set; }
        /// <summary>
        /// Pagamento necessário para criar esta Notificação.
        /// </summary>
        public Pagamento Pagamento { get; set; }

        [Required]
        public string Titulo { get; set; } = "";

        public string? Descricao { get; set; } = "";

        [Required]
        public DateTime DataPublicacao { get; set; }
        public DateTime? DataExpiracao { get; set; }

        [ForeignKey("Usuarios")]
        public long AnuncianteId { get; set; }
        public virtual Usuario? Anunciante { get; set; }

        /// <summary>
        /// Imagem "rosto" da Notificação.
        /// </summary>
        public string? BannerImagem { get; set; }

        public string? TipoAnuncio { get; set; }
        /// <summary>
        /// Usuários aos quais esta Notificação será direcionada.
        /// </summary>
        public ICollection<UsuarioNotificacao> UsuariosAlvo { get; set; } = new List<UsuarioNotificacao>();
    }
}
