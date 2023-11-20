using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class omnibusEan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OmnibusPrices_Items_ItemId",
                table: "OmnibusPrices");

            migrationBuilder.DropIndex(
                name: "IX_OmnibusPrices_ItemId",
                table: "OmnibusPrices");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "OmnibusPrices");

            migrationBuilder.AddColumn<string>(
                name: "Ean",
                table: "OmnibusPrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ean",
                table: "OmnibusPrices");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "OmnibusPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OmnibusPrices_ItemId",
                table: "OmnibusPrices",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OmnibusPrices_Items_ItemId",
                table: "OmnibusPrices",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
