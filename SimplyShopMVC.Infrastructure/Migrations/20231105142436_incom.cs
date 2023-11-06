using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class incom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VatRateName",
                table: "ItemWarehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Incoms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    warehouseId = table.Column<int>(type: "int", nullable: false),
                    grupa_towarowa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nazwa_grupy_towarowej = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    symbol_produktu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nazwa_produktu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    symbol_producenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ean = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nazwa_producenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stan_magazynowy = table.Column<int>(type: "int", nullable: false),
                    cena = table.Column<int>(type: "int", nullable: false),
                    dlugosc = table.Column<int>(type: "int", nullable: false),
                    szeroksc = table.Column<int>(type: "int", nullable: false),
                    wysokosc = table.Column<int>(type: "int", nullable: false),
                    waga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incoms_Warehouses_warehouseId",
                        column: x => x.warehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incoms_warehouseId",
                table: "Incoms",
                column: "warehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incoms");

            migrationBuilder.DropColumn(
                name: "VatRateName",
                table: "ItemWarehouses");
        }
    }
}
