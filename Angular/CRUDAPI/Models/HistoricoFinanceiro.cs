using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("HistoricosFinanceiros")]
    public class HistoricoFinanceiro
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Usuarios")]
        public long UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<Pagamento>? Pagamentos { get; set; }

        public virtual ICollection<Estorno>? Estornos { get; set; }

        public decimal BonusExtra { get; set; } // Recompensas ou promoções recebidas eventualmente
        public bool Ativo { get; set; } 
    }
}
