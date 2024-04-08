using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Informações do duelo entre Inscrições feitas para os Competidores.
    /// </summary>
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
        /// <summary>
        /// Id do ConfrontoInscricao anterior que classificou os inscritos para este.
        /// </summary>
        [ForeignKey("ConfrontoInscricao")]
        public long? ConfrontoInscricaoPaiId { get; set; }
        /// <summary>
        /// ConfrontoInscricao anterior que classificou os inscritos para este.
        /// </summary>
        public virtual ConfrontoInscricao? ConfrontoInscricaoPai { get; set; }
    }
}
