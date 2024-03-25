using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDAPI.Migrations
{
    /// <inheritdoc />
    public partial class migracaoBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sobrenome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailConfirmado = table.Column<bool>(type: "bit", nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CpfCnpj = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
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
                    IdCriadorUsuario = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competicoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competicoes_Usuarios_IdCriadorUsuario",
                        column: x => x.IdCriadorUsuario,
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
                    IdCategoria = table.Column<long>(type: "bigint", nullable: false),
                    CategoriaId = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeAtleta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Equipe = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscricoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscricoes_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inscricoes_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Confrontos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeOuAtleta1Id = table.Column<long>(type: "bigint", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataTermino = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confrontos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Confrontos_Inscricoes_TimeOuAtleta1Id",
                        column: x => x.TimeOuAtleta1Id,
                        principalTable: "Inscricoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfrontoInscricao",
                columns: table => new
                {
                    ConfrontoId = table.Column<long>(type: "bigint", nullable: false),
                    InscricaoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfrontoInscricao", x => new { x.ConfrontoId, x.InscricaoId });
                    table.ForeignKey(
                        name: "FK_ConfrontoInscricao_Confrontos_ConfrontoId",
                        column: x => x.ConfrontoId,
                        principalTable: "Confrontos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConfrontoInscricao_Inscricoes_InscricaoId",
                        column: x => x.InscricaoId,
                        principalTable: "Inscricoes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CompeticaoId",
                table: "Categorias",
                column: "CompeticaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Competicoes_IdCriadorUsuario",
                table: "Competicoes",
                column: "IdCriadorUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ConfrontoInscricao_InscricaoId",
                table: "ConfrontoInscricao",
                column: "InscricaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Confrontos_TimeOuAtleta1Id",
                table: "Confrontos",
                column: "TimeOuAtleta1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_CategoriaId",
                table: "Inscricoes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_IdUsuario",
                table: "Inscricoes",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CpfCnpj",
                table: "Usuarios",
                column: "CpfCnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_SenhaHash",
                table: "Usuarios",
                column: "SenhaHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfrontoInscricao");

            migrationBuilder.DropTable(
                name: "Confrontos");

            migrationBuilder.DropTable(
                name: "Inscricoes");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Competicoes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
