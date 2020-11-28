using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBazar.API.Migrations
{
    public partial class IsApprovedPropertyAddedToAds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Ads",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Ads");
        }
    }
}
