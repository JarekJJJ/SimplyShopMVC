using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class DeliveryUpg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryTime",
                table: "ItemWarehouses");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryTime",
                table: "Warehouses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryTime",
                table: "Warehouses");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryTime",
                table: "ItemWarehouses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
