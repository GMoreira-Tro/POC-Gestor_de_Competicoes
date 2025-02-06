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

        [ForeignKey("Pagamentos")]
        public long PagamentoId { get; set; }
        public virtual Pagamento? Pagamento { get; set; }

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
        /// <summary>
        /// Número da Posição após o término da participação da Inscrição. 
        /// 1º lugar, 2º lugar, etc...
        /// </summary>
        public int? Posição { get; set; }
        /// <summary>
        /// Indica se o inscrito deu WO.
        /// </summary>
        public bool WO { get; set; }
        [ForeignKey("Premios")]
        /// <summary>
        /// Id do prêmio que essa Inscrição poderá resgatar ao final da sua participação na Competição.
        /// </summary>
        public long? PremioResgatavelId { get; set; }
        /// <summary>
        /// Prêmio que essa Inscrição poderá resgatar ao final da sua participação na Competição.
        /// </summary>
        public virtual Premio? PremioResgatavel { get; set; }
        public ICollection<ConfrontoInscricao> ConfrontoInscricoes { get; set; } = new List<ConfrontoInscricao>();
    }
}
