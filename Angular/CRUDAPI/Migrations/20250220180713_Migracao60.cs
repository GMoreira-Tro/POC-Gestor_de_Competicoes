using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migracao60 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Pagamentos_PagamentoId",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_PagamentoId",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "PagamentoId",
                table: "Notificacoes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PagamentoId",
                table: "Notificacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_PagamentoId",
                table: "Notificacoes",
                column: "PagamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Pagamentos_PagamentoId",
                table: "Notificacoes",
                column: "PagamentoId",
                principalTable: "Pagamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
