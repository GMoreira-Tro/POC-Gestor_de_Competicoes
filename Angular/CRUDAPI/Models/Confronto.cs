using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Confrontos")]
    public class Confronto
    {
        [Key]
        public long Id { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataTermino { get; set; }
        [Required]
        public string Local { get; set; }
        public ICollection<ConfrontoInscricao>? ConfrontoInscricoes { get; set; }
    }
}
