using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("ConfrontoInscricao")]
    public class ConfrontoInscricao
    {
        [ForeignKey("Confrontos")]
        [Required]
        public long ConfrontoId { get; set; }
        public virtual Confronto? Confronto { get; set; }

        [ForeignKey("Inscricoes")]
        [Required]
        public long InscricaoId { get; set; }
        public virtual Inscricao? Inscricao { get; set; }
    }
}
