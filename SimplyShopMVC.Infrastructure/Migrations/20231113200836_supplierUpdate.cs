using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class supplierUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incoms_IncomGroups_incomGroupId",
                table: "Incoms");

            migrationBuilder.DropIndex(
                name: "IX_Incoms_incomGroupId",
                table: "Incoms");

            migrationBuilder.DropColumn(
                name: "incomGroupId",
                table: "Incoms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "incomGroupId",
                table: "Incoms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Incoms_incomGroupId",
                table: "Incoms",
                column: "incomGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incoms_IncomGroups_incomGroupId",
                table: "Incoms",
                column: "incomGroupId",
                principalTable: "IncomGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
