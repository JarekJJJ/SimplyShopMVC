using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyShopMVC.Infrastructure.Migrations
{
    public partial class messageTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageTicketId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MessageTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    StatusTicket = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTickets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageTicketId",
                table: "Messages",
                column: "MessageTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageTickets_MessageTicketId",
                table: "Messages",
                column: "MessageTicketId",
                principalTable: "MessageTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageTickets_MessageTicketId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageTickets");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageTicketId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageTicketId",
                table: "Messages");
        }
    }
}
