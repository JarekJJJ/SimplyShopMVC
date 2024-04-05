using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class relCatInc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "IncomGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncomGroups_CategoryId",
                table: "IncomGroups",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomGroups_Categories_CategoryId",
                table: "IncomGroups",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomGroups_Categories_CategoryId",
                table: "IncomGroups");

            migrationBuilder.DropIndex(
                name: "IX_IncomGroups_CategoryId",
                table: "IncomGroups");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "IncomGroups");
        }
    }
}
