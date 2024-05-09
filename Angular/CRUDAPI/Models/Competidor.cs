using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Enum que contém as opões ATLETA e CLUBE.
    /// </summary>
    public enum TipoCompetidor
    {
        Atleta,
        Clube
    }
    /// <summary>
    /// Atleta ou Clube que irá competir em Competições. Cadastrado via algum Usuário.
    /// </summary>
    [Table("Competidores")]
    public class Competidor
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Nome { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        /// <summary>
        /// Indica se o Competidor é um Atleta ou um Clube.
        /// </summary>
        [EnumDataType(typeof(TipoCompetidor))]
        public TipoCompetidor Tipo { get; set; }

        /// <summary>
        /// Id do Usuário responsável por criar este Competidor.
        /// </summary>
        [ForeignKey("Usuarios")]
        public long CriadorId { get; set; }
        /// <summary>
        /// Usuário responsável por criar este Competidor.
        /// </summary>
        public virtual Usuario? Criador { get; set; }
        public virtual ICollection<ConviteCompetidor> ConvitesCompetidor { get; set; } = new List<ConviteCompetidor>();
    }
}
