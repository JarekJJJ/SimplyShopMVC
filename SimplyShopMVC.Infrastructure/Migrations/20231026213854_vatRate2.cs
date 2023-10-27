using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class vatRate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VatRate",
                table: "ItemWarehouses",
                newName: "VatRateId");

            migrationBuilder.CreateTable(
                name: "VatRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemWarehouses_VatRateId",
                table: "ItemWarehouses",
                column: "VatRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemWarehouses_VatRates_VatRateId",
                table: "ItemWarehouses",
                column: "VatRateId",
                principalTable: "VatRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemWarehouses_VatRates_VatRateId",
                table: "ItemWarehouses");

            migrationBuilder.DropTable(
                name: "VatRates");

            migrationBuilder.DropIndex(
                name: "IX_ItemWarehouses_VatRateId",
                table: "ItemWarehouses");

            migrationBuilder.RenameColumn(
                name: "VatRateId",
                table: "ItemWarehouses",
                newName: "VatRate");
        }
    }
}
