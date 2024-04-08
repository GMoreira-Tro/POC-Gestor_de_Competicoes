using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Categoria associada a uma Competição onde os Competidores serão inscritos. Exemplos de Nomes: Sênior Masculino Faixa Branca até 67kg,
    /// Feminino Juvenil, Sub-21 Masculino Faixa Marrom/Preta até 80kg...
    /// </summary>
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// Exemplos de Nomes: Sênior Masculino Faixa Branca até 67kg,
        /// Feminino Juvenil, Sub-21 Masculino Faixa Marrom/Preta até 80kg...
        /// </summary>
        [Required]
        public string Nome { get; set; } = "";
        public string? Descricao { get; set; }
        [ForeignKey("Competicoes")]
        public long CompeticaoId { get; set; }
        public virtual Competicao? Competicao { get; set; }
        public ICollection<Inscricao>? Inscricoes { get; set; }
    }
}
