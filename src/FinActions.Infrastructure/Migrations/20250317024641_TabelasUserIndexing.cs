using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinActions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TabelasUserIndexing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_UserId_Id",
                table: "Movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_ContasBancarias_UserId_Id",
                table: "ContasBancarias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_UserId_Id",
                table: "Categorias");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_UserId",
                table: "Movimentacoes",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContasBancarias_UserId",
                table: "ContasBancarias",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_UserId",
                table: "Categorias",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_UserId",
                table: "Movimentacoes");

            migrationBuilder.DropIndex(
                name: "IX_ContasBancarias_UserId",
                table: "ContasBancarias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_UserId",
                table: "Categorias");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_UserId_Id",
                table: "Movimentacoes",
                columns: new[] { "UserId", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContasBancarias_UserId_Id",
                table: "ContasBancarias",
                columns: new[] { "UserId", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_UserId_Id",
                table: "Categorias",
                columns: new[] { "UserId", "Id" },
                unique: true);
        }
    }
}
