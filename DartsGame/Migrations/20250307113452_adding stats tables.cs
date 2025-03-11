using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DartsGame.Migrations
{
    /// <inheritdoc />
    public partial class addingstatstables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegStats",
                columns: table => new
                {
                    LegStatsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LegId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PPD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    First9PPD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DartsThrown = table.Column<int>(type: "int", nullable: false),
                    CheckoutPoints = table.Column<int>(type: "int", nullable: false),
                    CheckoutPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Count60Plus = table.Column<int>(type: "int", nullable: false),
                    Count100Plus = table.Column<int>(type: "int", nullable: false),
                    Count140Plus = table.Column<int>(type: "int", nullable: false),
                    Count180s = table.Column<int>(type: "int", nullable: false),
                    LegId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegStats", x => x.LegStatsId);
                    table.ForeignKey(
                        name: "FK_LegStats_Legs_LegId",
                        column: x => x.LegId,
                        principalTable: "Legs",
                        principalColumn: "LegId");
                    table.ForeignKey(
                        name: "FK_LegStats_Legs_LegId1",
                        column: x => x.LegId1,
                        principalTable: "Legs",
                        principalColumn: "LegId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId");
                    table.ForeignKey(
                        name: "FK_LegStats_Players_PlayerId1",
                        column: x => x.PlayerId1,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchStats",
                columns: table => new
                {
                    MatchStatsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetsWin = table.Column<int>(type: "int", nullable: false),
                    LegsWin = table.Column<int>(type: "int", nullable: false),
                    PPD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    First9PPD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CheckoutPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BestCheckoutPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Count60Plus = table.Column<int>(type: "int", nullable: false),
                    Count100Plus = table.Column<int>(type: "int", nullable: false),
                    Count140Plus = table.Column<int>(type: "int", nullable: false),
                    Count180s = table.Column<int>(type: "int", nullable: false),
                    MatchId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchStats", x => x.MatchStatsId);
                    table.ForeignKey(
                        name: "FK_MatchStats_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "MatchId");
                    table.ForeignKey(
                        name: "FK_MatchStats_Matches_MatchId1",
                        column: x => x.MatchId1,
                        principalTable: "Matches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId");
                    table.ForeignKey(
                        name: "FK_MatchStats_Players_PlayerId1",
                        column: x => x.PlayerId1,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetStats",
                columns: table => new
                {
                    SetStatsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LegsWin = table.Column<int>(type: "int", nullable: false),
                    PPD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    First9PPD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CheckoutPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BestCheckoutPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Count60Plus = table.Column<int>(type: "int", nullable: false),
                    Count100Plus = table.Column<int>(type: "int", nullable: false),
                    Count140Plus = table.Column<int>(type: "int", nullable: false),
                    Count180s = table.Column<int>(type: "int", nullable: false),
                    SetId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetStats", x => x.SetStatsId);
                    table.ForeignKey(
                        name: "FK_SetStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId");
                    table.ForeignKey(
                        name: "FK_SetStats_Players_PlayerId1",
                        column: x => x.PlayerId1,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetStats_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "SetId");
                    table.ForeignKey(
                        name: "FK_SetStats_Sets_SetId1",
                        column: x => x.SetId1,
                        principalTable: "Sets",
                        principalColumn: "SetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegStats_LegId",
                table: "LegStats",
                column: "LegId");

            migrationBuilder.CreateIndex(
                name: "IX_LegStats_LegId1",
                table: "LegStats",
                column: "LegId1");

            migrationBuilder.CreateIndex(
                name: "IX_LegStats_PlayerId",
                table: "LegStats",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LegStats_PlayerId1",
                table: "LegStats",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStats_MatchId",
                table: "MatchStats",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStats_MatchId1",
                table: "MatchStats",
                column: "MatchId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStats_PlayerId",
                table: "MatchStats",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStats_PlayerId1",
                table: "MatchStats",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_SetStats_PlayerId",
                table: "SetStats",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_SetStats_PlayerId1",
                table: "SetStats",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_SetStats_SetId",
                table: "SetStats",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_SetStats_SetId1",
                table: "SetStats",
                column: "SetId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegStats");

            migrationBuilder.DropTable(
                name: "MatchStats");

            migrationBuilder.DropTable(
                name: "SetStats");
        }
    }
}
