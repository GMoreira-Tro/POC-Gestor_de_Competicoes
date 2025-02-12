using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDAPI.Migrations
{
    /// <inheritdoc />
    public partial class Migracao5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Competicoes_CompeticaoId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfrontoInscricao_ConfrontoInscricao_ConfrontoInscricaoPaiId",
                table: "ConfrontoInscricao");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfrontoInscricao_Confrontos_ConfrontoId",
                table: "ConfrontoInscricao");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfrontoInscricao_Inscricoes_InscricaoId",
                table: "ConfrontoInscricao");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Categorias_CategoriaId",
                table: "Inscricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Pagamentos_PagamentoId",
                table: "Inscricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Premios_PremioResgatavelId",
                table: "Inscricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Pagamentos_PagamentoId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Premios_Pagamentos_PagamentoId",
                table: "Premios");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioNotificacoes_Notificacoes_NotificacaoId",
                table: "UsuarioNotificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioNotificacoes_Usuarios_UsuarioId",
                table: "UsuarioNotificacoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Competicoes_CompeticaoId",
                table: "Categorias",
                column: "CompeticaoId",
                principalTable: "Competicoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfrontoInscricao_ConfrontoInscricao_ConfrontoInscricaoPaiId",
                table: "ConfrontoInscricao",
                column: "ConfrontoInscricaoPaiId",
                principalTable: "ConfrontoInscricao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfrontoInscricao_Confrontos_ConfrontoId",
                table: "ConfrontoInscricao",
                column: "ConfrontoId",
                principalTable: "Confrontos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfrontoInscricao_Inscricoes_InscricaoId",
                table: "ConfrontoInscricao",
                column: "InscricaoId",
                principalTable: "Inscricoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Categorias_CategoriaId",
                table: "Inscricoes",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Pagamentos_PagamentoId",
                table: "Inscricoes",
                column: "PagamentoId",
                principalTable: "Pagamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Premios_PremioResgatavelId",
                table: "Inscricoes",
                column: "PremioResgatavelId",
                principalTable: "Premios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Pagamentos_PagamentoId",
                table: "Notificacoes",
                column: "PagamentoId",
                principalTable: "Pagamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Premios_Pagamentos_PagamentoId",
                table: "Premios",
                column: "PagamentoId",
                principalTable: "Pagamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioNotificacoes_Notificacoes_NotificacaoId",
                table: "UsuarioNotificacoes",
                column: "NotificacaoId",
                principalTable: "Notificacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioNotificacoes_Usuarios_UsuarioId",
                table: "UsuarioNotificacoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Competicoes_CompeticaoId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfrontoInscricao_ConfrontoInscricao_ConfrontoInscricaoPaiId",
                table: "ConfrontoInscricao");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfrontoInscricao_Confrontos_ConfrontoId",
                table: "ConfrontoInscricao");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfrontoInscricao_Inscricoes_InscricaoId",
                table: "ConfrontoInscricao");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Categorias_CategoriaId",
                table: "Inscricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Pagamentos_PagamentoId",
                table: "Inscricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscricoes_Premios_PremioResgatavelId",
                table: "Inscricoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Pagamentos_PagamentoId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Premios_Pagamentos_PagamentoId",
                table: "Premios");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioNotificacoes_Notificacoes_NotificacaoId",
                table: "UsuarioNotificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioNotificacoes_Usuarios_UsuarioId",
                table: "UsuarioNotificacoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Competicoes_CompeticaoId",
                table: "Categorias",
                column: "CompeticaoId",
                principalTable: "Competicoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfrontoInscricao_ConfrontoInscricao_ConfrontoInscricaoPaiId",
                table: "ConfrontoInscricao",
                column: "ConfrontoInscricaoPaiId",
                principalTable: "ConfrontoInscricao",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfrontoInscricao_Confrontos_ConfrontoId",
                table: "ConfrontoInscricao",
                column: "ConfrontoId",
                principalTable: "Confrontos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfrontoInscricao_Inscricoes_InscricaoId",
                table: "ConfrontoInscricao",
                column: "InscricaoId",
                principalTable: "Inscricoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Categorias_CategoriaId",
                table: "Inscricoes",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Pagamentos_PagamentoId",
                table: "Inscricoes",
                column: "PagamentoId",
                principalTable: "Pagamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscricoes_Premios_PremioResgatavelId",
                table: "Inscricoes",
                column: "PremioResgatavelId",
                principalTable: "Premios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Pagamentos_PagamentoId",
                table: "Notificacoes",
                column: "PagamentoId",
                principalTable: "Pagamentos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Premios_Pagamentos_PagamentoId",
                table: "Premios",
                column: "PagamentoId",
                principalTable: "Pagamentos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioNotificacoes_Notificacoes_NotificacaoId",
                table: "UsuarioNotificacoes",
                column: "NotificacaoId",
                principalTable: "Notificacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioNotificacoes_Usuarios_UsuarioId",
                table: "UsuarioNotificacoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
