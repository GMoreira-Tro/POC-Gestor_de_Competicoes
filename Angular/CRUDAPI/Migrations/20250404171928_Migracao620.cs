using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migracao620 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChaveamentoId",
                table: "Confrontos",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Chaveamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Confrontos_ChaveamentoId",
                table: "Confrontos",
                column: "ChaveamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Chaveamentos_CategoriaId",
                table: "Chaveamentos",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Confrontos_Chaveamentos_ChaveamentoId",
                table: "Confrontos",
                column: "ChaveamentoId",
                principalTable: "Chaveamentos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Confrontos_Chaveamentos_ChaveamentoId",
                table: "Confrontos");

            migrationBuilder.DropTable(
                name: "Chaveamentos");

            migrationBuilder.DropIndex(
                name: "IX_Confrontos_ChaveamentoId",
                table: "Confrontos");

            migrationBuilder.DropColumn(
                name: "ChaveamentoId",
                table: "Confrontos");
        }
    }
}
