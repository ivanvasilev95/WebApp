using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBazar.API.Migrations
{
    public partial class AuditInformation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Photos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Photos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Ads",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Ads",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Ads",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Ads");
        }
    }
}
