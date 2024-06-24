using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class favorite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteItems_ItemId",
                table: "FavoriteItems",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteItems");
        }
    }
}
