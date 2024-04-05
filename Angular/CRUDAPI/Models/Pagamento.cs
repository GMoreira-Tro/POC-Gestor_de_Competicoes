using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Pagamentos")]
    public class Pagamento
    {
        [Key]
        public long Id { get; set; }

        public decimal ValorPago { get; set; }
        public string Moeda { get; set; } = "";
        public DateTime DataRequisicao { get; set; }
        public DateTime? DataRecebimento { get; set; }
        [ForeignKey("Usuarios")]
        public long SolicitanteId { get; set; }
        public Usuario? Solicitante { get; set; }
        [ForeignKey("Usuarios")]
        public long PagadorId { get; set; }
        public Usuario? Pagador { get; set; }
        public string Motivo { get; set; } = "";
        public string ObservacaoSolicitante { get; set; } = "";
        public string ObservacaoPagador { get; set; } = "";
    }
}
