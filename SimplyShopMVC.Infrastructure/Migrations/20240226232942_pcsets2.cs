using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class pcsets2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PcSets_GroupItemId",
                table: "PcSets",
                column: "GroupItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PcSets_GroupItems_GroupItemId",
                table: "PcSets",
                column: "GroupItemId",
                principalTable: "GroupItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PcSets_GroupItems_GroupItemId",
                table: "PcSets");

            migrationBuilder.DropIndex(
                name: "IX_PcSets_GroupItemId",
                table: "PcSets");
        }
    }
}
