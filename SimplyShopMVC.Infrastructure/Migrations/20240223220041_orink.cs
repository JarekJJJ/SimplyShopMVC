using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class orink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrinkGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrinkGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    warehouseId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrinkGroupId = table.Column<int>(type: "int", nullable: false),
                    printerProducent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    symbol_producenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ean = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stan_magazynowy = table.Column<int>(type: "int", nullable: false),
                    cenaNetto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    waga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    imgLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orinks_OrinkGroups_OrinkGroupId",
                        column: x => x.OrinkGroupId,
                        principalTable: "OrinkGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orinks_Warehouses_warehouseId",
                        column: x => x.warehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orinks_OrinkGroupId",
                table: "Orinks",
                column: "OrinkGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Orinks_warehouseId",
                table: "Orinks",
                column: "warehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orinks");

            migrationBuilder.DropTable(
                name: "OrinkGroups");
        }
    }
}
