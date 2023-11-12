using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class incomGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "incomGroupId",
                table: "Incoms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IncomGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    GroupIdHome = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomGroups", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incoms_IncomGroups_incomGroupId",
                table: "Incoms");

            migrationBuilder.DropTable(
                name: "IncomGroups");

            migrationBuilder.DropIndex(
                name: "IX_Incoms_incomGroupId",
                table: "Incoms");

            migrationBuilder.DropColumn(
                name: "incomGroupId",
                table: "Incoms");
        }
    }
}
