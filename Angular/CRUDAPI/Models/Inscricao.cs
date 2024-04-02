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
        public long CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }

        [ForeignKey("Usuarios")]
        public long UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        [EnumDataType(typeof(StatusPagamento))]
        public StatusPagamento StatusPagamento { get; set; }

        //TODO: Pensar em tela pro front-end de cadastrar Competidores de um Usuário
        [ForeignKey("Competidores")]
        public long CompetidorId { get; set; }
        public virtual Competidor? Competidor { get; set; }
        public int? Posição { get; set; } // 1º lugar, 2º lugar, etc
        public ICollection<ConfrontoInscricao>? ConfrontoInscricoes { get; set; }
    }
}
