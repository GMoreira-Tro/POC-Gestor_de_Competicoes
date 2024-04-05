using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Pagamentos")]
    public class Pagamento
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Usuarios")]
        public long? UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }
        public decimal ValorPago { get; set; }
        public string Moeda { get; set; } = "";
        public string? MeioPagamento { get; set; } = "";
        /// <summary>
        /// Refere-se se o pagamento é remetente a uma inscrição de Competição, Marketing, afiliação, mensalidade...
        /// </summary>
        public string? TipoPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}