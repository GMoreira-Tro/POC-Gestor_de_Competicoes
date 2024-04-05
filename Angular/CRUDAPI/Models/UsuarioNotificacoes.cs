using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAPI.Models
{
    [Table("UsuarioNotificacoes")]
    public class UsuarioNotificacao
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Usuario")]
        public long UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("Notificacoes")]
        public long NotificacaoId { get; set; }
        public virtual Notificacao? Notificacao { get; set; }
        public bool Lido { get; set; }
        public DateTime? DataLeitura { get; set; }
    }
}
