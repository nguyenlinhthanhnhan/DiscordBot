using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    LeagueId = table.Column<Guid>(type: "char(36)", nullable: false),
                    LeagueName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.LeagueId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JoinedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalVouch = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLeagueVouches",
                columns: table => new
                {
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    LeagueId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Vouch = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLeagueVouches", x => new { x.UserId, x.LeagueId });
                    table.ForeignKey(
                        name: "FK_UserLeagueVouches_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLeagueVouches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VouchUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserVouchId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Reason = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserLeagueVouchUserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    UserLeagueVouchLeagueId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VouchUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VouchUser_UserLeagueVouches_UserLeagueVouchUserId_UserLeague~",
                        columns: x => new { x.UserLeagueVouchUserId, x.UserLeagueVouchLeagueId },
                        principalTable: "UserLeagueVouches",
                        principalColumns: new[] { "UserId", "LeagueId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLeagueVouches_LeagueId",
                table: "UserLeagueVouches",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_VouchUser_UserLeagueVouchUserId_UserLeagueVouchLeagueId",
                table: "VouchUser",
                columns: new[] { "UserLeagueVouchUserId", "UserLeagueVouchLeagueId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VouchUser");

            migrationBuilder.DropTable(
                name: "UserLeagueVouches");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
