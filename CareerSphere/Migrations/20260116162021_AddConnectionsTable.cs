using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSphere.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    followingId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    followerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => new { x.followerId, x.followingId });
                    table.ForeignKey(
                        name: "FK_Connections_Users_followerId",
                        column: x => x.followerId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_Users_followingId",
                        column: x => x.followingId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_followingId",
                table: "Connections",
                column: "followingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections");
        }
    }
}
