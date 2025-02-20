using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migracao61 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioNotificacoes");

            migrationBuilder.AddColumn<long>(
                name: "NotificadoId",
                table: "Notificacoes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_NotificadoId",
                table: "Notificacoes",
                column: "NotificadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Usuarios_NotificadoId",
                table: "Notificacoes",
                column: "NotificadoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Usuarios_NotificadoId",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_NotificadoId",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "NotificadoId",
                table: "Notificacoes");

            migrationBuilder.CreateTable(
                name: "UsuarioNotificacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificacaoId = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    DataLeitura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Lido = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioAnunciante = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioNotificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioNotificacoes_Notificacoes_NotificacaoId",
                        column: x => x.NotificacaoId,
                        principalTable: "Notificacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioNotificacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioNotificacoes_NotificacaoId",
                table: "UsuarioNotificacoes",
                column: "NotificacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioNotificacoes_UsuarioId",
                table: "UsuarioNotificacoes",
                column: "UsuarioId");
        }
    }
}
