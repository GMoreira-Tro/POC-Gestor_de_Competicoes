using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class Contexto : DbContext
    {
        public DbSet<Pessoa> Pessoas;

        public Contexto(DbContextOptions<Contexto> opcoes) : base(opcoes)
        {
            
        }
    }
}