using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class conCatGru : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConnectCategoryGroupId",
                table: "IncomGroups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConnectCategoryGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    GroupItemId = table.Column<int>(type: "int", nullable: false),
                    ItemTagId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectCategoryGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectCategoryGroups_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectCategoryGroups_GroupItems_GroupItemId",
                        column: x => x.GroupItemId,
                        principalTable: "GroupItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectCategoryGroups_ItemTags_ItemTagId",
                        column: x => x.ItemTagId,
                        principalTable: "ItemTags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomGroups_ConnectCategoryGroupId",
                table: "IncomGroups",
                column: "ConnectCategoryGroupId",
                unique: true,
                filter: "[ConnectCategoryGroupId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectCategoryGroups_CategoryId",
                table: "ConnectCategoryGroups",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectCategoryGroups_GroupItemId",
                table: "ConnectCategoryGroups",
                column: "GroupItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectCategoryGroups_ItemTagId",
                table: "ConnectCategoryGroups",
                column: "ItemTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomGroups_ConnectCategoryGroups_ConnectCategoryGroupId",
                table: "IncomGroups",
                column: "ConnectCategoryGroupId",
                principalTable: "ConnectCategoryGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomGroups_ConnectCategoryGroups_ConnectCategoryGroupId",
                table: "IncomGroups");

            migrationBuilder.DropTable(
                name: "ConnectCategoryGroups");

            migrationBuilder.DropIndex(
                name: "IX_IncomGroups_ConnectCategoryGroupId",
                table: "IncomGroups");

            migrationBuilder.DropColumn(
                name: "ConnectCategoryGroupId",
                table: "IncomGroups");
        }
    }
}
