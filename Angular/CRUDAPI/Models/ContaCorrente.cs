using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CRUDAPI.Models
{
    [Table("ContasCorrente")]
    public class ContaCorrente
    {
        //TODO: estudar a API do PagSeguro (Boleto, cartão de crédito e Pix)
        [Key]
        public long Id { get; set; }

        public decimal Saldo { get; set; }
        [ForeignKey("Usuarios")]
        public long UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<PagamentoContaCorrente> PagamentoContasCorrente { get; set; } = new List<PagamentoContaCorrente>();
    }
}