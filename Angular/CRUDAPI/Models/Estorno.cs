using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Estornos")]
    public class Estorno
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Inscricao")]
        public long InscricaoId { get; set; }
        public virtual Inscricao? Inscricao { get; set; }

        public decimal ValorEstornado { get; set; }
        public string Moeda { get; set; } = "";
        public string Motivo { get; set; } = "";
        public DateTime DataEstorno { get; set; }
        public bool EstornoAutomatico { get; set; }
        public string Observacao { get; set; } = "";
    }
}
