using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migracao62 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerImagem",
                table: "Notificacoes");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Notificacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "Notificacoes");

            migrationBuilder.AddColumn<string>(
                name: "BannerImagem",
                table: "Notificacoes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
