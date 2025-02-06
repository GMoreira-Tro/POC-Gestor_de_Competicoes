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
            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Competidores_CompetidorId",
                table: "Inscricoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Competidores_CompetidorId",
                table: "Inscricoes",
                column: "CompetidorId",
                principalTable: "Competidores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Competidores_CompetidorId",
                table: "Inscricoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Competidores_CompetidorId",
                table: "Inscricoes",
                column: "CompetidorId",
                principalTable: "Competidores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
