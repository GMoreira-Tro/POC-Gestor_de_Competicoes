using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Models
{
    /// <summary>
    /// Clase que contém todas as referências as tabelas do Banco de Dados bem como as definições dos relacionamentos.
    /// </summary>
    public class Contexto : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Competicao> Competicoes { get; set; }
        public DbSet<Competidor> Competidores { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Confronto> Confrontos { get; set; }
        public DbSet<ConfrontoInscricao> ConfrontoInscricoes { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<UsuarioNotificacao> UsuarioNotificacoes { get; set; }
        public DbSet<Premio> Premios { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        /// <summary>
        /// Função que define o relacionamento entre as tabelas do Banco de Dados.
        /// </summary>
        /// <param name="modelBuilder">Construtor do modelo de relacionamento entre tabelas.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inscricao>()
                .HasOne(i => i.Competidor)
                .WithMany()
                .HasForeignKey(i => i.CompetidorId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.Competicao)
                .WithMany(c => c.Categorias)
                .HasForeignKey(c => c.CompeticaoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConfrontoInscricao>()
                .HasOne(ci => ci.Confronto)
                .WithMany(c => c.ConfrontoInscricoes)
                .HasForeignKey(ci => ci.ConfrontoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConfrontoInscricao>()
                .HasOne(ci => ci.Inscricao)
                .WithMany(i => i.ConfrontoInscricoes)
                .HasForeignKey(ci => ci.InscricaoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConfrontoInscricao>()
                .HasOne(ci => ci.ConfrontoInscricaoPai)
                .WithMany()
                .HasForeignKey(ci => ci.ConfrontoInscricaoPaiId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inscricao>()
                .HasOne(i => i.Categoria)
                .WithMany(c => c.Inscricoes)
                .HasForeignKey(i => i.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inscricao>()
                .HasOne(i => i.Pagamento)
                .WithMany()
                .HasForeignKey(i => i.PagamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inscricao>()
                .HasOne(i => i.PremioResgatavel)
                .WithMany()
                .HasForeignKey(i => i.PremioResgatavelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notificacao>()
                .HasOne(n => n.Pagamento)
                .WithMany()
                .HasForeignKey(n => n.PagamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsuarioNotificacao>()
                .HasOne(un => un.Usuario)
                .WithMany(u => u.AnunciosRecebidos)
                .HasForeignKey(un => un.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsuarioNotificacao>()
                .HasOne(un => un.Notificacao)
                .WithMany(n => n.UsuariosAlvo)
                .HasForeignKey(un => un.NotificacaoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Premio>()
                .HasOne(p => p.Pagamento)
                .WithMany()
                .HasForeignKey(p => p.PagamentoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
