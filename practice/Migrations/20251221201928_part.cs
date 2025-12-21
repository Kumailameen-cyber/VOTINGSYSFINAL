using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice.Migrations
{
    /// <inheritdoc />
    public partial class part : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElectionId",
                table: "Candidates",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Elections",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 12, 21, 20, 19, 28, 192, DateTimeKind.Utc).AddTicks(3241), new DateTime(2026, 1, 21, 20, 19, 28, 192, DateTimeKind.Utc).AddTicks(3206), new DateTime(2025, 12, 21, 20, 19, 28, 192, DateTimeKind.Utc).AddTicks(3205) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 21, 20, 19, 28, 192, DateTimeKind.Utc).AddTicks(2574), "$2a$11$vEMGkThBJo.Zp.k.XI8/SOL5mHrAsSyt7u1TKo9nDFp0QoQRH78xu" });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ElectionId",
                table: "Candidates",
                column: "ElectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Elections_ElectionId",
                table: "Candidates",
                column: "ElectionId",
                principalTable: "Elections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Elections_ElectionId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_ElectionId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ElectionId",
                table: "Candidates");

            migrationBuilder.UpdateData(
                table: "Elections",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 12, 21, 17, 3, 49, 226, DateTimeKind.Utc).AddTicks(9607), new DateTime(2026, 1, 21, 17, 3, 49, 226, DateTimeKind.Utc).AddTicks(9576), new DateTime(2025, 12, 21, 17, 3, 49, 226, DateTimeKind.Utc).AddTicks(9575) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 21, 17, 3, 49, 226, DateTimeKind.Utc).AddTicks(8483), "$2a$11$M6Uxt9orbVd4nIppFUwduewAceJloUUQuw6E/yZbiab2ua5h8c/H2" });
        }
    }
}
