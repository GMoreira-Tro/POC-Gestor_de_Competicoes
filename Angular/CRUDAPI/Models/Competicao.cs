using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Competicoes")]
    public class Competicao
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string? Descricao { get; set; }
        [Required]
        public string Modalidade { get; set; }

        public string? Pais { get; set; }

        public string? Estado { get; set; }

        public string? Cidade { get; set; }
        public string? BannerImagem { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        [ForeignKey("Usuarios")]
        public long IdCriadorUsuario { get; set; }
        public virtual Usuario? Usuario { get; set; }

        // Propriedade de navegação para as categorias associadas
        public ICollection<Categoria>? Categorias { get; set; }
    }
}