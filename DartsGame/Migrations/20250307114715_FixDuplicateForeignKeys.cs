using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DartsGame.Migrations
{
    /// <inheritdoc />
    public partial class FixDuplicateForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegStats_Legs_LegId1",
                table: "LegStats");

            migrationBuilder.DropForeignKey(
                name: "FK_LegStats_Players_PlayerId1",
                table: "LegStats");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchStats_Matches_MatchId1",
                table: "MatchStats");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchStats_Players_PlayerId1",
                table: "MatchStats");

            migrationBuilder.DropForeignKey(
                name: "FK_SetStats_Players_PlayerId1",
                table: "SetStats");

            migrationBuilder.DropForeignKey(
                name: "FK_SetStats_Sets_SetId1",
                table: "SetStats");

            migrationBuilder.AlterColumn<Guid>(
                name: "SetId1",
                table: "SetStats",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId1",
                table: "SetStats",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId1",
                table: "MatchStats",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId1",
                table: "MatchStats",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId1",
                table: "LegStats",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "LegId1",
                table: "LegStats",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_LegStats_Legs_LegId1",
                table: "LegStats",
                column: "LegId1",
                principalTable: "Legs",
                principalColumn: "LegId");

            migrationBuilder.AddForeignKey(
                name: "FK_LegStats_Players_PlayerId1",
                table: "LegStats",
                column: "PlayerId1",
                principalTable: "Players",
                principalColumn: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchStats_Matches_MatchId1",
                table: "MatchStats",
                column: "MatchId1",
                principalTable: "Matches",
                principalColumn: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchStats_Players_PlayerId1",
                table: "MatchStats",
                column: "PlayerId1",
                principalTable: "Players",
                principalColumn: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetStats_Players_PlayerId1",
                table: "SetStats",
                column: "PlayerId1",
                principalTable: "Players",
                principalColumn: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetStats_Sets_SetId1",
                table: "SetStats",
                column: "SetId1",
                principalTable: "Sets",
                principalColumn: "SetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegStats_Legs_LegId1",
                table: "LegStats");

            migrationBuilder.DropForeignKey(
                name: "FK_LegStats_Players_PlayerId1",
                table: "LegStats");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchStats_Matches_MatchId1",
                table: "MatchStats");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchStats_Players_PlayerId1",
                table: "MatchStats");

            migrationBuilder.DropForeignKey(
                name: "FK_SetStats_Players_PlayerId1",
                table: "SetStats");

            migrationBuilder.DropForeignKey(
                name: "FK_SetStats_Sets_SetId1",
                table: "SetStats");

            migrationBuilder.AlterColumn<Guid>(
                name: "SetId1",
                table: "SetStats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId1",
                table: "SetStats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId1",
                table: "MatchStats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId1",
                table: "MatchStats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId1",
                table: "LegStats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LegId1",
                table: "LegStats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LegStats_Legs_LegId1",
                table: "LegStats",
                column: "LegId1",
                principalTable: "Legs",
                principalColumn: "LegId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LegStats_Players_PlayerId1",
                table: "LegStats",
                column: "PlayerId1",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchStats_Matches_MatchId1",
                table: "MatchStats",
                column: "MatchId1",
                principalTable: "Matches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchStats_Players_PlayerId1",
                table: "MatchStats",
                column: "PlayerId1",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetStats_Players_PlayerId1",
                table: "SetStats",
                column: "PlayerId1",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SetStats_Sets_SetId1",
                table: "SetStats",
                column: "SetId1",
                principalTable: "Sets",
                principalColumn: "SetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
