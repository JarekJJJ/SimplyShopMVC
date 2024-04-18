using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class conCatGru4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomGroups_ConnectCategoryGroups_ConnectCategoryGroupId",
                table: "IncomGroups");

            migrationBuilder.DropIndex(
                name: "IX_IncomGroups_ConnectCategoryGroupId",
                table: "IncomGroups");

            migrationBuilder.DropColumn(
                name: "ConnectCategoryGroupId",
                table: "IncomGroups");

            migrationBuilder.AddColumn<int>(
                name: "IncomGroupId",
                table: "ConnectCategoryGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ConnectCategoryGroups_IncomGroupId",
                table: "ConnectCategoryGroups",
                column: "IncomGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectCategoryGroups_IncomGroups_IncomGroupId",
                table: "ConnectCategoryGroups",
                column: "IncomGroupId",
                principalTable: "IncomGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectCategoryGroups_IncomGroups_IncomGroupId",
                table: "ConnectCategoryGroups");

            migrationBuilder.DropIndex(
                name: "IX_ConnectCategoryGroups_IncomGroupId",
                table: "ConnectCategoryGroups");

            migrationBuilder.DropColumn(
                name: "IncomGroupId",
                table: "ConnectCategoryGroups");

            migrationBuilder.AddColumn<int>(
                name: "ConnectCategoryGroupId",
                table: "IncomGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncomGroups_ConnectCategoryGroupId",
                table: "IncomGroups",
                column: "ConnectCategoryGroupId",
                unique: true,
                filter: "[ConnectCategoryGroupId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomGroups_ConnectCategoryGroups_ConnectCategoryGroupId",
                table: "IncomGroups",
                column: "ConnectCategoryGroupId",
                principalTable: "ConnectCategoryGroups",
                principalColumn: "Id");
        }
    }
}
