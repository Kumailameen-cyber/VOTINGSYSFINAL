using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice.Migrations
{
    /// <inheritdoc />
    public partial class nullwali : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_ElectionId_VoterId",
                table: "Votes");

            migrationBuilder.AlterColumn<int>(
                name: "ElectionId",
                table: "Votes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_ElectionId_VoterId",
                table: "Votes",
                columns: new[] { "ElectionId", "VoterId" },
                unique: true,
                filter: "[ElectionId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_ElectionId_VoterId",
                table: "Votes");

            migrationBuilder.AlterColumn<int>(
                name: "ElectionId",
                table: "Votes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_ElectionId_VoterId",
                table: "Votes",
                columns: new[] { "ElectionId", "VoterId" },
                unique: true);
        }
    }
}
