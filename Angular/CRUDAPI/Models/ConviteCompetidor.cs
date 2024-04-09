using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("ConvitesCompetidores")]
    public class ConviteCompetidor
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Convite")]
        public long ConviteId { get; set; }
        public virtual Convite Convite { get; set; }

        [ForeignKey("Competidor")]
        public long CompetidorId { get; set; }
        public virtual Competidor Competidor { get; set; }

        // Outros campos se necessário
    }
}
