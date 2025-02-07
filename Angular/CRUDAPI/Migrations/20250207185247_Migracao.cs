using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Confrontos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataTermino = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confrontos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Txid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PagadorId = table.Column<long>(type: "bigint", nullable: false),
                    FavorecidoId = table.Column<long>(type: "bigint", nullable: false),
                    InfoPagamento = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sobrenome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailConfirmado = table.Column<bool>(type: "bit", nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenhaValidada = table.Column<bool>(type: "bit", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CpfCnpj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagamentoId = table.Column<long>(type: "bigint", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataPublicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BannerImagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoAnuncio = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Pagamentos_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamentos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Premios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEntrega = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PagamentoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Premios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Premios_Pagamentos_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamentos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Competicoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modalidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerImagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriadorUsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
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
                name: "UsuarioNotificacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioAnunciante = table.Column<bool>(type: "bit", nullable: false),
                    NotificacaoId = table.Column<long>(type: "bigint", nullable: false),
                    Lido = table.Column<bool>(type: "bit", nullable: false),
                    DataLeitura = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioNotificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioNotificacoes_Notificacoes_NotificacaoId",
                        column: x => x.NotificacaoId,
                        principalTable: "Notificacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioNotificacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompeticaoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_Competicoes_CompeticaoId",
                        column: x => x.CompeticaoId,
                        principalTable: "Competicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscricoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaId = table.Column<long>(type: "bigint", nullable: false),
                    PagamentoId = table.Column<long>(type: "bigint", nullable: false),
                    CompetidorId = table.Column<long>(type: "bigint", nullable: false),
                    Posição = table.Column<int>(type: "int", nullable: true),
                    WO = table.Column<bool>(type: "bit", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscricoes_Premios_PremioResgatavelId",
                        column: x => x.PremioResgatavelId,
                        principalTable: "Premios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConfrontoInscricao",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfrontoId = table.Column<long>(type: "bigint", nullable: false),
                    InscricaoId = table.Column<long>(type: "bigint", nullable: false),
                    ConfrontoInscricaoPaiId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfrontoInscricao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfrontoInscricao_ConfrontoInscricao_ConfrontoInscricaoPaiId",
                        column: x => x.ConfrontoInscricaoPaiId,
                        principalTable: "ConfrontoInscricao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConfrontoInscricao_Confrontos_ConfrontoId",
                        column: x => x.ConfrontoId,
                        principalTable: "Confrontos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfrontoInscricao_Inscricoes_InscricaoId",
                        column: x => x.InscricaoId,
                        principalTable: "Inscricoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CompeticaoId",
                table: "Categorias",
                column: "CompeticaoId");

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
                name: "IX_Notificacoes_PagamentoId",
                table: "Notificacoes",
                column: "PagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Premios_PagamentoId",
                table: "Premios",
                column: "PagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioNotificacoes_NotificacaoId",
                table: "UsuarioNotificacoes",
                column: "NotificacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioNotificacoes_UsuarioId",
                table: "UsuarioNotificacoes",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfrontoInscricao");

            migrationBuilder.DropTable(
                name: "UsuarioNotificacoes");

            migrationBuilder.DropTable(
                name: "Confrontos");

            migrationBuilder.DropTable(
                name: "Inscricoes");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Competidores");

            migrationBuilder.DropTable(
                name: "Premios");

            migrationBuilder.DropTable(
                name: "Competicoes");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
