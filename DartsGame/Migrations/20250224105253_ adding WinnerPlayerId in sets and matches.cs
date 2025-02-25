using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DartsGame.Migrations
{
    /// <inheritdoc />
    public partial class addingWinnerPlayerIdinsetsandmatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WinnerPlayerId",
                table: "Sets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WinnerPlayerId",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinnerPlayerId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "WinnerPlayerId",
                table: "Matches");
        }
    }
}
