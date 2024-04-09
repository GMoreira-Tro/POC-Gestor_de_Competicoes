using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Convites")]
    public class Convite
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public DateTime DataEnvio { get; set; }

        public DateTime? DataResposta { get; set; }

        // Relacionamento muitos-para-muitos com a classe Competidor
        public virtual ICollection<ConviteCompetidor> ConvitesCompetidor { get; set; }
    }
}
