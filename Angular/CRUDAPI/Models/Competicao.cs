using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Competicoes")]
    public class Competicao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Descricao { get; set; }

        public string? Pais { get; set; }

        public string? Provincia { get; set; }
        
        public string? Cidade { get; set; }
        public string? BannerImagem { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }

        [ForeignKey("Usuario")]
        public int IdCriadorUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        // Propriedade de navegação para as categorias associadas
        public ICollection<Categoria> Categorias { get; set; }
    }
}