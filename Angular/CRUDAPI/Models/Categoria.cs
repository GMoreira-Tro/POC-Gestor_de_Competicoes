using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        // Relacionamento com a competição
        [ForeignKey("Competicoes")]
        public long CompeticaoId { get; set; }
        public virtual Competicao? Competicao { get; set; }
        // Propriedade de navegação para as inscrições na categoria
        public ICollection<Inscricao>? Inscricoes { get; set; }
    }
}
