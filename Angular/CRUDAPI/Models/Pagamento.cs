using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Pagamentos")]
    public class Pagamento
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Inscricoes")]
        public long InscricaoId { get; set; }
        public decimal ValorPago { get; set; }
        public string Moeda { get; set; } = "";
        public DateTime DataPagamento { get; set; }
    }
}