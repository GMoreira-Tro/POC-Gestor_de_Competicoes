using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CRUDAPI.Models
{
    public enum Status
    {
        [EnumMember(Value = "pendente")]
        PENDENTE,

        [EnumMember(Value = "paga")]
        PAGA,

        [EnumMember(Value = "aceita")]
        ACEITA,

        [EnumMember(Value = "recusada")]
        RECUSADA
    }
    /// <summary>
    /// Tabela responsável por gerir transações financeiras.
    /// </summary>
    [Table("Pagamentos")]
    public class Pagamento
    {
        [Key]
        public long Id { get; set; }

        public decimal ValorPago { get; set; }
        public string Moeda { get; set; } = "";
        public DateTime DataRequisicao { get; set; }
        public DateTime? DataRecebimento { get; set; }
        /// <summary>
        /// Id de quem solicitou uma transação financeira.
        /// </summary>
        [ForeignKey("Usuarios")]
        public long SolicitanteId { get; set; }
        /// <summary>
        /// Quem solicitou uma transação financeira.
        /// </summary>
        public Usuario? Solicitante { get; set; }
        /// <summary>
        /// Id de quem irá pagar uma requisição ou receber de volta o dinheiro de algum estorno.
        /// </summary>
        [ForeignKey("Usuarios")]
        public long PagadorRecebedorId { get; set; }
        /// <summary>
        /// Quem irá pagar uma requisição ou receber de volta o dinheiro de algum estorno.
        /// </summary>
        public Usuario? PagadorRecebedor { get; set; }
        /// <summary>
        /// Id do Admin que irá aprovar a transação financeira.
        /// </summary>
        [ForeignKey("Usuarios")]
        public long AprovadorId { get; set; }
        /// <summary>
        /// Admin que irá aprovar a transação financeira.
        /// </summary>
        public Usuario? Aprovador { get; set; }
        public string Motivo { get; set; } = "";
        public string ObservacaoSolicitante { get; set; } = "";
        public string ObservacaoPagador { get; set; } = "";

        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
    }
}
