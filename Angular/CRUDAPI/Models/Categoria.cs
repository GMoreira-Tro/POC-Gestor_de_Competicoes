using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        // Relacionamento com a competição
        public int CompeticaoId { get; set; }
        public Competicao Competicao { get; set; }
        // Propriedade de navegação para as inscrições na categoria
        public ICollection<Inscricao> Inscricoes { get; set; }
    }
}
