using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("PagamentoInscricao")]
    public class PagamentoInscricao
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Inscricao")]
        public long InscricaoId { get; set; }
        public decimal ValorPago { get; set; }
        public string Moeda { get; set; } = "";
        public DateTime DataPagamento { get; set; }
    }
}