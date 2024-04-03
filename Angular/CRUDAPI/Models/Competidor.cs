using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CRUDAPI.Models
{
    public enum TipoCompetidor
    {
        [EnumMember(Value = "Atleta")]
        ATLETA,
        [EnumMember(Value = "Clube")]
        CLUBE
    }
    [Table("Competidores")]
    public class Competidor
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        public string Nome { get; set; } = "";
        [Required]
        public string Email { get; set; } = "";

        [EnumDataType(typeof(TipoCompetidor))]
        public TipoCompetidor Tipo { get; set; }

        [ForeignKey("Usuarios")]
        public long CriadorId { get; set; }
        public virtual Usuario? Criador { get; set; }
    }
}
