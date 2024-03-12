using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAPI.Models
{
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
