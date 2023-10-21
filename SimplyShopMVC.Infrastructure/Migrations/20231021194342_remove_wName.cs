using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class remove_wName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WDescription",
                table: "ItemWarehouses");

            migrationBuilder.DropColumn(
                name: "WName",
                table: "ItemWarehouses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WDescription",
                table: "ItemWarehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WName",
                table: "ItemWarehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
