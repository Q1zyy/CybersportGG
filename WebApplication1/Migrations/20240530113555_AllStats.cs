using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AllStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_MatchesStats_MatchStatsId",
                table: "PlayerStats");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStats_MatchStatsId",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "MatchStatsId",
                table: "PlayerStats");

            migrationBuilder.CreateTable(
                name: "PlayerMatchesStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    Headshots = table.Column<int>(type: "int", nullable: false),
                    MatchStatsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMatchesStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMatchesStats_MatchesStats_MatchStatsId",
                        column: x => x.MatchStatsId,
                        principalTable: "MatchesStats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMatchesStats_MatchStatsId",
                table: "PlayerMatchesStats",
                column: "MatchStatsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerMatchesStats");

            migrationBuilder.AddColumn<int>(
                name: "MatchStatsId",
                table: "PlayerStats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_MatchStatsId",
                table: "PlayerStats",
                column: "MatchStatsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_MatchesStats_MatchStatsId",
                table: "PlayerStats",
                column: "MatchStatsId",
                principalTable: "MatchesStats",
                principalColumn: "Id");
        }
    }
}
