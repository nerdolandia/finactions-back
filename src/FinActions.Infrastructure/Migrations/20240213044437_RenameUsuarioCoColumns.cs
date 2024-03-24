using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinActions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameUsuarioCoColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedTime",
                table: "Usuarios",
                newName: "EditedDate");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Usuarios",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Usuarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EditedBy",
                table: "Usuarios",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "EditedDate",
                table: "Usuarios",
                newName: "DeletedTime");
        }
    }
}
