using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class relacje02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectCategoryTags_ItemTags_CategoryId",
                table: "ConnectCategoryTags");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectCategoryTags_ItemTagId",
                table: "ConnectCategoryTags",
                column: "ItemTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectCategoryTags_ItemTags_ItemTagId",
                table: "ConnectCategoryTags",
                column: "ItemTagId",
                principalTable: "ItemTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectCategoryTags_ItemTags_ItemTagId",
                table: "ConnectCategoryTags");

            migrationBuilder.DropIndex(
                name: "IX_ConnectCategoryTags_ItemTagId",
                table: "ConnectCategoryTags");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectCategoryTags_ItemTags_CategoryId",
                table: "ConnectCategoryTags",
                column: "CategoryId",
                principalTable: "ItemTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
