using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.API.Migrations
{
    public partial class UpdatedContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ad_Category_CategoryId",
                table: "Ad");

            migrationBuilder.DropForeignKey(
                name: "FK_Ad_Users_UserId",
                table: "Ad");

            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Ad_AdId",
                table: "Photo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photo",
                table: "Photo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ad",
                table: "Ad");

            migrationBuilder.RenameTable(
                name: "Photo",
                newName: "Photos");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Ad",
                newName: "Ads");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_AdId",
                table: "Photos",
                newName: "IX_Photos_AdId");

            migrationBuilder.RenameIndex(
                name: "IX_Ad_UserId",
                table: "Ads",
                newName: "IX_Ads_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ad_CategoryId",
                table: "Ads",
                newName: "IX_Ads_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ads",
                table: "Ads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Categories_CategoryId",
                table: "Ads",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Users_UserId",
                table: "Ads",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Ads_AdId",
                table: "Photos",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Categories_CategoryId",
                table: "Ads");

            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Users_UserId",
                table: "Ads");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Ads_AdId",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ads",
                table: "Ads");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photo");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "Ads",
                newName: "Ad");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_AdId",
                table: "Photo",
                newName: "IX_Photo_AdId");

            migrationBuilder.RenameIndex(
                name: "IX_Ads_UserId",
                table: "Ad",
                newName: "IX_Ad_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ads_CategoryId",
                table: "Ad",
                newName: "IX_Ad_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photo",
                table: "Photo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ad",
                table: "Ad",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ad_Category_CategoryId",
                table: "Ad",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ad_Users_UserId",
                table: "Ad",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Ad_AdId",
                table: "Photo",
                column: "AdId",
                principalTable: "Ad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
