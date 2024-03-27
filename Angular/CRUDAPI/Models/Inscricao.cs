using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CRUDAPI.Models
{
    public enum StatusPagamento
    {
        [EnumMember(Value = "pendente")]
        PENDENTE,

        [EnumMember(Value = "paga")]
        PAGA,

        [EnumMember(Value = "aceita")]
        ACEITA,

        [EnumMember(Value = "recusada")]
        RECUSADA
    }

    [Table("Inscricoes")]
    public class Inscricao
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Categorias")]
        public long IdCategoria { get; set; }
        public virtual Categoria? Categoria { get; set; }

        [ForeignKey("Usuarios")]
        public long IdUsuario { get; set; }
        public virtual Usuario? Usuario { get; set; }

        [Required]
        [EnumDataType(typeof(StatusPagamento))]
        public StatusPagamento StatusPagamento { get; set; }

        public string? NomeAtleta { get; set; }
        public string? Equipe { get; set; }
        public int? Posição { get; set; } // 1º lugar, 2º lugar, etc
        public long? InscricaoEtapaAnteriorId { get; set; }
        public virtual Inscricao? InscricaoEtapaAnterior { get; set; }
        public ICollection<ConfrontoInscricao>? ConfrontoInscricoes { get; set; }
    }
}
