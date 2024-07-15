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
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<UsuarioNotificacao> UsuarioNotificacoes { get; set; }
        public DbSet<Premio> Premios { get; set; }
        public DbSet<Convite> Convites { get; set; }
        public DbSet<ConviteCompetidor> ConvitesCompetidores { get; set; }
        public DbSet<ContaCorrente> ContasCorrentes { get; set; }
        public DbSet<PagamentoContaCorrente> PagamentoContaCorrentes { get; set; }

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        /// <summary>
        /// Função que define o relacionamento entre as tabelas do Banco de Dados.
        /// </summary>
        /// <param name="modelBuilder">Construtor do modelo de relacionamento entre tabelas.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração das chaves estrangeiras e índices únicos

            modelBuilder.Entity<Inscricao>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Inscricoes)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Competidor>()
                .HasOne(i => i.Criador)
                .WithMany(u => u.Competidores)
                .HasForeignKey(i => i.CriadorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.Competicao)
                .WithMany(c => c.Categorias)
                .HasForeignKey(c => c.CompeticaoId)
                .IsRequired();

            modelBuilder.Entity<Competicao>()
                .HasOne(c => c.CriadorUsuario)
                .WithMany()
                .HasForeignKey(c => c.CriadorUsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.CpfCnpj)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.SenhaHash)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ConfrontoInscricao>()
                .HasKey(ci => new { ci.ConfrontoId, ci.InscricaoId });

            modelBuilder.Entity<ConfrontoInscricao>()
                .HasOne(ci => ci.Confronto)
                .WithMany(c => c.ConfrontoInscricoes)
                .HasForeignKey(ci => ci.ConfrontoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ConfrontoInscricao>()
                .HasOne(ci => ci.Inscricao)
                .WithMany(i => i.ConfrontoInscricoes)
                .HasForeignKey(ci => ci.InscricaoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Pagamento>()
                .Property(p => p.Valor)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PagamentoContaCorrente>()
                .HasOne(ci => ci.Pagamento)
                .WithMany(i => i.PagamentoContasCorrente)
                .HasForeignKey(ci => ci.PagamentoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PagamentoContaCorrente>()
                .HasOne(ci => ci.ContaCorrente)
                .WithMany(i => i.PagamentoContasCorrente)
                .HasForeignKey(ci => ci.ContaCorrenteId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ContaCorrente>()
                .Property(c => c.Saldo)
                .HasPrecision(18, 2);

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

            base.OnModelCreating(modelBuilder);
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseLazyLoadingProxies();
        // }

    }
}
