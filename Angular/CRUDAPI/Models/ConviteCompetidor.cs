using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("ConvitesCompetidores")]
    public class ConviteCompetidor
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Convites")]
        public long ConviteId { get; set; }
        public virtual Convite? Convite { get; set; }

        [ForeignKey("Competidores")]
        public long CompetidorId { get; set; }
        public virtual Competidor? Competidor { get; set; }

        // Outros campos se necessário
    }
}
