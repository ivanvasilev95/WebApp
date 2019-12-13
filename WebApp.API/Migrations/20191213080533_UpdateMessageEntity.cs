using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.API.Migrations
{
    public partial class UpdateMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RecipientDeleted",
                table: "Messages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SenderDeleted",
                table: "Messages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientDeleted",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderDeleted",
                table: "Messages");
        }
    }
}
