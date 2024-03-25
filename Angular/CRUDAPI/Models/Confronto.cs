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

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataTermino { get; set; }
        public ICollection<ConfrontoInscricao>? ConfrontoInscricoes { get; set; }
    }
}
