using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Premiação recebida por uma Inscrição ao final da sua participação em uma Competição.
    /// </summary>
    [Table("Premios")]
    public class Premio
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public long? PagamentoResgatavelId { get; set; }
        public virtual Pagamento? PagamentoResgatavel { get; set; }

        public DateTime DataEntrega { get; set; }

    }
}
