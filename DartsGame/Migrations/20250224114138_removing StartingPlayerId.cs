using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DartsGame.Migrations
{
    /// <inheritdoc />
    public partial class removingStartingPlayerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK_Legs_Players_StartingPlayerId",
                table: "Legs");

            // Drop the index associated with the column
            migrationBuilder.DropIndex(
                name: "IX_Legs_StartingPlayerId",
                table: "Legs");

            // Drop the column itself
            migrationBuilder.DropColumn(
                name: "StartingPlayerId",
                table: "Legs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add the column back in case of a rollback
            migrationBuilder.AddColumn<Guid>(
                name: "StartingPlayerId",
                table: "Legs",
                nullable: true);

            // Recreate the index for the column
            migrationBuilder.CreateIndex(
                name: "IX_Legs_StartingPlayerId",
                table: "Legs",
                column: "StartingPlayerId");

            // Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Legs_Players_StartingPlayerId",
                table: "Legs",
                column: "StartingPlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}