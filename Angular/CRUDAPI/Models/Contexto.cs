using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Models
{
    public class Contexto : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Competicao> Competicoes { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração das chaves estrangeiras e índices únicos

            modelBuilder.Entity<Inscricao>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Inscricoes)
                .HasForeignKey(i => i.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.Competicao)
                .WithMany(c => c.Categorias)
                .HasForeignKey(c => c.CompeticaoId)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.CpfCnpj)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.SenhaHash)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
