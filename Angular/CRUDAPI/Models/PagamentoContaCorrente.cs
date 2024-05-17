using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Tabela que ligará duas Contas Correntes (uma de um pagador e outra de um recebedor) com um pagamento.
    /// </summary>
    [Table("PagamentoContasCorrente")]
    public class PagamentoContaCorrente
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Pagamentos")]
        public long PagamentoId { get; set; }
        public virtual Pagamento? Pagamento { get; set; }

        [ForeignKey("ContasCorrente")]
        public long ContaCorrenteId { get; set; }
        public virtual ContaCorrente? ContaCorrente { get; set; }

        /// <summary>
        /// Indica se a Conta Corrente vinculada é do Solicitante. Caso contrário, será do Pagador ou Recebedor do Pagamento.
        /// </summary>
        public bool ContaCorrenteSolicitante { get; set; }

        public string Observacao { get; set; } = "";
    }
}
