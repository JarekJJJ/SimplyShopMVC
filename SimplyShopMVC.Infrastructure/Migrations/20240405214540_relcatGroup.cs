using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class relcatGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupItemId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_GroupItemId",
                table: "Categories",
                column: "GroupItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_GroupItems_GroupItemId",
                table: "Categories",
                column: "GroupItemId",
                principalTable: "GroupItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_GroupItems_GroupItemId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_GroupItemId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "GroupItemId",
                table: "Categories");
        }
    }
}
