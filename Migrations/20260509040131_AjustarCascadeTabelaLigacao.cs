using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModuloMVC.Migrations
{
    /// <inheritdoc />
    public partial class AjustarCascadeTabelaLigacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContatoTarefa_Contato_ContatosEnvolvidosId",
                table: "ContatoTarefa");

            migrationBuilder.DropForeignKey(
                name: "FK_ContatoTarefa_Tarefa_TarefasEnvolvidasId",
                table: "ContatoTarefa");

            migrationBuilder.AddForeignKey(
                name: "FK_ContatoTarefa_Contato_ContatosEnvolvidosId",
                table: "ContatoTarefa",
                column: "ContatosEnvolvidosId",
                principalTable: "Contato",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContatoTarefa_Tarefa_TarefasEnvolvidasId",
                table: "ContatoTarefa",
                column: "TarefasEnvolvidasId",
                principalTable: "Tarefa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContatoTarefa_Contato_ContatosEnvolvidosId",
                table: "ContatoTarefa");

            migrationBuilder.DropForeignKey(
                name: "FK_ContatoTarefa_Tarefa_TarefasEnvolvidasId",
                table: "ContatoTarefa");

            migrationBuilder.AddForeignKey(
                name: "FK_ContatoTarefa_Contato_ContatosEnvolvidosId",
                table: "ContatoTarefa",
                column: "ContatosEnvolvidosId",
                principalTable: "Contato",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContatoTarefa_Tarefa_TarefasEnvolvidasId",
                table: "ContatoTarefa",
                column: "TarefasEnvolvidasId",
                principalTable: "Tarefa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
