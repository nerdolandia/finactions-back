using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinActions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUsuariosPapeis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosPapeis_Papeis_PapeisId",
                table: "UsuariosPapeis");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosPapeis_Usuarios_UsuariosId",
                table: "UsuariosPapeis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuariosPapeis",
                table: "UsuariosPapeis");

            migrationBuilder.RenameColumn(
                name: "UsuariosId",
                table: "UsuariosPapeis",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "PapeisId",
                table: "UsuariosPapeis",
                newName: "PapelId");

            migrationBuilder.RenameIndex(
                name: "IX_UsuariosPapeis_UsuariosId",
                table: "UsuariosPapeis",
                newName: "IX_UsuariosPapeis_UsuarioId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UsuariosPapeis",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuariosPapeis",
                table: "UsuariosPapeis",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosPapeis_PapelId",
                table: "UsuariosPapeis",
                column: "PapelId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosPapeis_Papeis_PapelId",
                table: "UsuariosPapeis",
                column: "PapelId",
                principalTable: "Papeis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosPapeis_Usuarios_UsuarioId",
                table: "UsuariosPapeis",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosPapeis_Papeis_PapelId",
                table: "UsuariosPapeis");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosPapeis_Usuarios_UsuarioId",
                table: "UsuariosPapeis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuariosPapeis",
                table: "UsuariosPapeis");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosPapeis_PapelId",
                table: "UsuariosPapeis");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsuariosPapeis");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "UsuariosPapeis",
                newName: "UsuariosId");

            migrationBuilder.RenameColumn(
                name: "PapelId",
                table: "UsuariosPapeis",
                newName: "PapeisId");

            migrationBuilder.RenameIndex(
                name: "IX_UsuariosPapeis_UsuarioId",
                table: "UsuariosPapeis",
                newName: "IX_UsuariosPapeis_UsuariosId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuariosPapeis",
                table: "UsuariosPapeis",
                columns: new[] { "PapeisId", "UsuariosId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosPapeis_Papeis_PapeisId",
                table: "UsuariosPapeis",
                column: "PapeisId",
                principalTable: "Papeis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosPapeis_Usuarios_UsuariosId",
                table: "UsuariosPapeis",
                column: "UsuariosId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
