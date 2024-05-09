using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CRUDAPI.Models
{
    public enum TipoPagamento
    {
        CartaoDeCredito = 1,
        Boleto = 2,
        /// <summary>
        /// (TEF): O comprador optou por pagar a transação com débito online de algum dos bancos conveniados.
        /// </summary>
        DebitoOnLineTEF = 3,
        /// <summary>
        /// O comprador optou por pagar a transação utilizando o saldo de sua conta PagSeguro.
        /// </summary>
        SaldoPagSeguro = 4,
        /// <summary>
        /// O comprador escolheu pagar sua transação através de seu celular Oi. 
        /// </summary>
        OiPago = 5,
        DepositoEmConta = 6,
        Dinheiro = 7
    };
    public enum Status
    {
        Pendente,

        Paga,

        Aceita,

        Recusada
    }
    /// <summary>
    /// Tabela responsável por gerir transações financeiras.
    /// </summary>
    [Table("Pagamentos")]
    public class Pagamento
    {
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Valor { get; set; }
        public string Moeda { get; set; } = "";
        public DateTime DataRequisicao { get; set; }
        public DateTime? DataRecebimento { get; set; }
        /// <summary>
        /// Id de quem solicitou uma transação financeira.
        /// </summary>
        [ForeignKey("ContasCorrente")]
        public long SolicitanteId { get; set; }
        /// <summary>
        /// Quem solicitou uma transação financeira.
        /// </summary>
        public ContaCorrente? Solicitante { get; set; }
        /// <summary>
        /// Id de quem irá pagar uma requisição ou receber de volta o dinheiro de algum estorno.
        /// </summary>
        [ForeignKey("ContasCorrente")]
        public long PagadorRecebedorId { get; set; }
        /// <summary>
        /// Quem irá pagar uma requisição ou receber de volta o dinheiro de algum estorno.
        /// </summary>
        public ContaCorrente? PagadorRecebedor { get; set; }
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
        public string ObservacaoPagadorRecebedor { get; set; } = "";
        
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        [EnumDataType(typeof(TipoPagamento))]
        public TipoPagamento TipoPagamento { get; set; }
        /// <summary>
        /// Token de acesso para a API do PagSeguro.
        /// </summary>
        public string TokenPagSeguro { get; set; } = "";
        //TODO: Pensar como seria o registro de pagamentos em dinheiro.
        //TODO: Ao confirmar um Pagamento, disparar um e-mail para as partes envolvidas.
    }
}
