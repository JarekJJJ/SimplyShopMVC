using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class GroupItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupItemId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceMarkupA = table.Column<int>(type: "int", nullable: false),
                    PriceMarkupB = table.Column<int>(type: "int", nullable: false),
                    PriceMarkupC = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_GroupItemId",
                table: "Items",
                column: "GroupItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_GroupItems_GroupItemId",
                table: "Items",
                column: "GroupItemId",
                principalTable: "GroupItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_GroupItems_GroupItemId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "GroupItems");

            migrationBuilder.DropIndex(
                name: "IX_Items_GroupItemId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "GroupItemId",
                table: "Items");
        }
    }
}
