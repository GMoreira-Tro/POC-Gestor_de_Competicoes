using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("UsuarioAnuncios")]
    public class UsuarioAnuncio
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Usuario")]
        public long UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("Anuncio")]
        public long AnuncioId { get; set; }
        public virtual Anuncio? Anuncio { get; set; }
    }
}
