using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Evento competitivo de alguma Modalidade divulgado por um Usuário. Pode ser dividido em múltiplas etapas.
    /// </summary>
    [Table("Competicoes")]
    public class Competicao
    {
        //TODO Pensar sobre conceito de Competição Fechada ou Aberta
        //TODO Amarrar de forma mais sólida o Pagamento para a Competição
        [Key]
        public long Id { get; set; }

        [Required]
        public string Titulo { get; set; } = "";

        public string? Descricao { get; set; }
        /// <summary>
        /// Sobre o que se trata a competição: Judô, Muay Thai, Natação, Vôlei, Pega-Pega, Queimada, League of Legends, Rpg...
        /// </summary>
        [Required]
        public string Modalidade { get; set; } = "";

        /// <summary>
        /// Sigla do País de acordo com a API geonames (BR, US, AR...).
        /// </summary>
        public string? Pais { get; set; }
        /// <summary>
        /// Nome do Estado de acordo com a API geonames.
        /// </summary>
        public string? Estado { get; set; }
        /// <summary>
        /// Bone da Cidade de acordo com a API geonames.
        /// </summary>
        public string? Cidade { get; set; }
        /// <summary>
        /// Imagem "rosto" da competição.
        /// </summary>
        public string? BannerImagem { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        /// <summary>
        /// Id do Usuário que criou a Competição.
        /// </summary>
        [ForeignKey("Usuarios")]
        public long CriadorUsuarioId { get; set; }
        /// <summary>
        /// Usuário que criou a Competição.
        /// </summary>
        public virtual Usuario? CriadorUsuario { get; set; }
        /// <summary>
        /// Id do Convite necessário para uma Inscrição entrar nessa Competição.
        /// </summary>
        [ForeignKey("Convites")]
        public long? ConviteNecessarioId { get; set; }
        /// <summary>
        /// Convite necessário para uma Inscrição entrar nessa Competição.
        /// </summary>
        public virtual Convite? ConviteNecessario  { get; set; }

        public ICollection<Categoria>? Categorias { get; set; }
    }
}