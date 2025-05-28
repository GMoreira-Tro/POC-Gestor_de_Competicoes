using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Representa um chaveamento de confrontos, com estrutura armazenada como JSON.
    /// </summary>
    [Table("Chaveamentos")]
    public class Chaveamento
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [ForeignKey("Categoria")]
        public long CategoriaId { get; set; }

        public virtual Categoria? Categoria { get; set; }

        /// <summary>
        /// JSON serializado com a árvore completa de confrontos.
        /// </summary>
        [Required]
        public string ArvoreConfrontos { get; set; } = string.Empty;
    }
}
