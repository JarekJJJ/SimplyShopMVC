using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class messageTickets2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageTickets_MessageTicketId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "MessageTicketId",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageTickets_MessageTicketId",
                table: "Messages",
                column: "MessageTicketId",
                principalTable: "MessageTickets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageTickets_MessageTicketId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "MessageTicketId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageTickets_MessageTicketId",
                table: "Messages",
                column: "MessageTicketId",
                principalTable: "MessageTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
