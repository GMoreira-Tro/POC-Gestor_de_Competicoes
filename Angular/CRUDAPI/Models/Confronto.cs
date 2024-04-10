using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Informações da logística de um duelo entre Competidores.
    /// </summary>
    [Table("Confrontos")]
    public class Confronto
    {
        [Key]
        public long Id { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataTermino { get; set; }
        public string Local { get; set; } = "";
        public ICollection<ConfrontoInscricao> ConfrontoInscricoes { get; set; } = new List<ConfrontoInscricao>();
    }
}
