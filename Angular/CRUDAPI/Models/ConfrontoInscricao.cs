using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("ConfrontoInscricao")]
    public class ConfrontoInscricao
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Confrontos")]
        public long ConfrontoId { get; set; }
        public virtual Confronto? Confronto { get; set; }
        [ForeignKey("Inscricoes")]
        public long InscricaoId { get; set; }
        public virtual Inscricao? Inscricao { get; set; }
        [ForeignKey("ConfrontoInscricao")]
        public long? ConfrontoInscricaoPaiId { get; set; }
        public virtual ConfrontoInscricao? ConfrontoInscricaoPai { get; set; }
        public bool WO { get; set; }
    }
}
