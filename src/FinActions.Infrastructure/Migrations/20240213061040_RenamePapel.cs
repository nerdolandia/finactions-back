using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinActions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamePapel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Papeis",
                newName: "Nome");

            migrationBuilder.RenameIndex(
                name: "IX_Papeis_Name",
                table: "Papeis",
                newName: "IX_Papeis_Nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Papeis",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Papeis_Nome",
                table: "Papeis",
                newName: "IX_Papeis_Name");
        }
    }
}
