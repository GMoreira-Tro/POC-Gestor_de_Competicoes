using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    public class Competicao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Descricao { get; set; }

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