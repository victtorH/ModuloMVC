using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModuloMVC.Migrations.TEnancyDBMigrations
{
    /// <inheritdoc />
    public partial class Ajusfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contato",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contato_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tarefa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefa", x => x.Id);
                    table.CheckConstraint("CK_Tarefa_TituloOuDescricao_Requerido", "([Titulo] IS NOT NULL AND [Titulo] <> '') OR ([Descricao] IS NOT NULL AND [Descricao] <> '')");
                    table.ForeignKey(
                        name: "FK_Tarefa_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContatoTarefa",
                columns: table => new
                {
                    ContatosEnvolvidosId = table.Column<int>(type: "int", nullable: false),
                    TarefasEnvolvidasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContatoTarefa", x => new { x.ContatosEnvolvidosId, x.TarefasEnvolvidasId });
                    table.ForeignKey(
                        name: "FK_ContatoTarefa_Contato_ContatosEnvolvidosId",
                        column: x => x.ContatosEnvolvidosId,
                        principalTable: "Contato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContatoTarefa_Tarefa_TarefasEnvolvidasId",
                        column: x => x.TarefasEnvolvidasId,
                        principalTable: "Tarefa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contato_UserId",
                table: "Contato",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContatoTarefa_TarefasEnvolvidasId",
                table: "ContatoTarefa",
                column: "TarefasEnvolvidasId");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefa_UserId",
                table: "Tarefa",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContatoTarefa");

            migrationBuilder.DropTable(
                name: "Contato");

            migrationBuilder.DropTable(
                name: "Tarefa");
        }
    }
}
