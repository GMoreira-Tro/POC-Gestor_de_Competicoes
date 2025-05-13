using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CRUDAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Txid = table.Column<string>(type: "text", nullable: false),
                    PagadorId = table.Column<long>(type: "bigint", nullable: false),
                    FavorecidoId = table.Column<long>(type: "bigint", nullable: false),
                    InfoPagamento = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Sobrenome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    EmailConfirmado = table.Column<bool>(type: "boolean", nullable: false),
                    TokenConfirmacao = table.Column<string>(type: "text", nullable: false),
                    SenhaHash = table.Column<string>(type: "text", nullable: false),
                    SenhaValidada = table.Column<bool>(type: "boolean", nullable: false),
                    Pais = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Cidade = table.Column<string>(type: "text", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CpfCnpj = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    ImagemUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Premios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    DataEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PagamentoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Premios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Premios_Pagamentos_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Competicoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    Modalidade = table.Column<string>(type: "text", nullable: false),
                    Pais = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: true),
                    Cidade = table.Column<string>(type: "text", nullable: true),
                    BannerImagem = table.Column<string>(type: "text", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CriadorUsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ChavePix = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competicoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competicoes_Usuarios_CriadorUsuarioId",
                        column: x => x.CriadorUsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Competidores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    ImagemUrl = table.Column<string>(type: "text", nullable: true),
                    CriadorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competidores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competidores_Usuarios_CriadorId",
                        column: x => x.CriadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotificadoId = table.Column<long>(type: "bigint", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    DataPublicacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: false),
                    TipoAnuncio = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Usuarios_NotificadoId",
                        column: x => x.NotificadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    CompeticaoId = table.Column<long>(type: "bigint", nullable: false),
                    ValorInscricao = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_Competicoes_CompeticaoId",
                        column: x => x.CompeticaoId,
                        principalTable: "Competicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chaveamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    CategoriaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chaveamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chaveamentos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscricoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoriaId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PagamentoId = table.Column<long>(type: "bigint", nullable: true),
                    CompetidorId = table.Column<long>(type: "bigint", nullable: false),
                    Posição = table.Column<int>(type: "integer", nullable: true),
                    WO = table.Column<bool>(type: "boolean", nullable: false),
                    PremioResgatavelId = table.Column<long>(type: "bigint", nullable: true),
                    CompetidorId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscricoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscricoes_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscricoes_Competidores_CompetidorId",
                        column: x => x.CompetidorId,
                        principalTable: "Competidores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscricoes_Competidores_CompetidorId1",
                        column: x => x.CompetidorId1,
                        principalTable: "Competidores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inscricoes_Pagamentos_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscricoes_Premios_PremioResgatavelId",
                        column: x => x.PremioResgatavelId,
                        principalTable: "Premios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Confrontos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChaveamentoId = table.Column<long>(type: "bigint", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataTermino = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Local = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confrontos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Confrontos_Chaveamentos_ChaveamentoId",
                        column: x => x.ChaveamentoId,
                        principalTable: "Chaveamentos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConfrontoInscricao",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConfrontoId = table.Column<long>(type: "bigint", nullable: false),
                    InscricaoId = table.Column<long>(type: "bigint", nullable: false),
                    ConfrontoInscricaoPaiId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfrontoInscricao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfrontoInscricao_ConfrontoInscricao_ConfrontoInscricaoPai~",
                        column: x => x.ConfrontoInscricaoPaiId,
                        principalTable: "ConfrontoInscricao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfrontoInscricao_Confrontos_ConfrontoId",
                        column: x => x.ConfrontoId,
                        principalTable: "Confrontos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfrontoInscricao_Inscricoes_InscricaoId",
                        column: x => x.InscricaoId,
                        principalTable: "Inscricoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CompeticaoId",
                table: "Categorias",
                column: "CompeticaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Chaveamentos_CategoriaId",
                table: "Chaveamentos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Competicoes_CriadorUsuarioId",
                table: "Competicoes",
                column: "CriadorUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Competidores_CriadorId",
                table: "Competidores",
                column: "CriadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfrontoInscricao_ConfrontoId",
                table: "ConfrontoInscricao",
                column: "ConfrontoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfrontoInscricao_ConfrontoInscricaoPaiId",
                table: "ConfrontoInscricao",
                column: "ConfrontoInscricaoPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfrontoInscricao_InscricaoId",
                table: "ConfrontoInscricao",
                column: "InscricaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Confrontos_ChaveamentoId",
                table: "Confrontos",
                column: "ChaveamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_CategoriaId",
                table: "Inscricoes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_CompetidorId",
                table: "Inscricoes",
                column: "CompetidorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_CompetidorId1",
                table: "Inscricoes",
                column: "CompetidorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_PagamentoId",
                table: "Inscricoes",
                column: "PagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_PremioResgatavelId",
                table: "Inscricoes",
                column: "PremioResgatavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_NotificadoId",
                table: "Notificacoes",
                column: "NotificadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Premios_PagamentoId",
                table: "Premios",
                column: "PagamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfrontoInscricao");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "Confrontos");

            migrationBuilder.DropTable(
                name: "Inscricoes");

            migrationBuilder.DropTable(
                name: "Chaveamentos");

            migrationBuilder.DropTable(
                name: "Competidores");

            migrationBuilder.DropTable(
                name: "Premios");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Competicoes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
