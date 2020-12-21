using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBazar.API.Migrations
{
    public partial class AuditInformation3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Categories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Categories");
        }
    }
}
