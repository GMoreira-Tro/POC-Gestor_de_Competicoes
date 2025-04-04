using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Representa uma estrutura de chaveamento para uma categoria de competição.
    /// </summary>
    [Table("Chaveamentos")]
    public class Chaveamento
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        [ForeignKey("Categoria")]
        public long CategoriaId { get; set; }

        public virtual Categoria? Categoria { get; set; }

        public virtual ICollection<Confronto> Confrontos { get; set; } = new List<Confronto>();
    }
}
