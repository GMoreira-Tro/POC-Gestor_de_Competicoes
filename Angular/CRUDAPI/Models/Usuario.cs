using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CRUDAPI.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; }

        public ICollection<Inscricao> Inscricoes { get; set; }
    }
}