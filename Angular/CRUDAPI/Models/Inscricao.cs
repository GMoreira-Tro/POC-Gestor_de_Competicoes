using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Inscrição referente a participação de um Competidor em uma Categoria de uma Competição.
    /// </summary>
    [Table("Inscricoes")]
    public class Inscricao
    {
        [Key]
        public long Id { get; set; }
        
        [ForeignKey("Categorias")]
        public long CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }

        /// <summary>
        /// Id do Usuário que requisitou a Inscrição de um de seus Competidores.
        /// </summary>
        [ForeignKey("Usuarios")]
        public long UsuarioId { get; set; }
        /// <summary>
        /// Usuário que requisitou a Inscrição de um de seus Competidores.
        /// </summary>
        public virtual Usuario? Usuario { get; set; }
        [ForeignKey("Pagamentos")]
        public long PagamentoId { get; set; }
        public Pagamento? Pagamento { get; set; }

        //TODO: Pensar em tela pro front-end de cadastrar Competidores de um Usuário
        /// <summary>
        /// Id do Atleta ou Clube que efetivamente irá competir.
        /// </summary>
        [ForeignKey("Competidores")]
        public long CompetidorId { get; set; }
        /// <summary>
        /// Atleta ou Clube que efetivamente irá competir.
        /// </summary>
        public virtual Competidor? Competidor { get; set; }
        public int? Posição { get; set; } // 1º lugar, 2º lugar, etc
        /// <summary>
        /// Indica se o inscrito deu WO.
        /// </summary>
        public bool WO { get; set; }
        public ICollection<ConfrontoInscricao>? ConfrontoInscricoes { get; set; }
    }
}
