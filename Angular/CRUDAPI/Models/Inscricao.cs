using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Inscricoes")]
    public class Inscricao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }
        public virtual Categoria Categoria { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Required]
        public string Status { get; set; } // pendente, aceita, recusada
        [Required]
        public string NomeAtleta { get; set; }
        public string? Equipe { get; set; }
    }
}