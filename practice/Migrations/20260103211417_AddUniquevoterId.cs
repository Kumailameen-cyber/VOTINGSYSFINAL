using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquevoterId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "cnic",
                table: "Users",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Elections",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 3, 21, 14, 16, 179, DateTimeKind.Utc).AddTicks(2545), new DateTime(2026, 2, 3, 21, 14, 16, 179, DateTimeKind.Utc).AddTicks(2462), new DateTime(2026, 1, 3, 21, 14, 16, 179, DateTimeKind.Utc).AddTicks(2459) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 3, 21, 14, 16, 179, DateTimeKind.Utc).AddTicks(166), "$2a$11$4a9fbRVeaPV7Kg1Dz6BNMe27G1hR44HQERnKwFtHjoU8PeAvkNlwW" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "cnic",
                table: "Users",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Elections",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 23, 44, 93, DateTimeKind.Utc).AddTicks(1262), new DateTime(2026, 2, 2, 21, 23, 44, 93, DateTimeKind.Utc).AddTicks(1169), new DateTime(2026, 1, 2, 21, 23, 44, 93, DateTimeKind.Utc).AddTicks(1167) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 2, 21, 23, 44, 92, DateTimeKind.Utc).AddTicks(8547), "$2a$11$vNUBiHQyg7ePeR6LfbpbH.zSo1oDa1.af9nfC2g18a.7cbH3Rwi9i" });
        }
    }
}
