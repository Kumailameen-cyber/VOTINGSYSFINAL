using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
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
                values: new object[] { new DateTime(2026, 1, 3, 17, 29, 13, 850, DateTimeKind.Utc).AddTicks(4842), new DateTime(2026, 2, 3, 17, 29, 13, 850, DateTimeKind.Utc).AddTicks(4813), new DateTime(2026, 1, 3, 17, 29, 13, 850, DateTimeKind.Utc).AddTicks(4813) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 3, 17, 29, 13, 850, DateTimeKind.Utc).AddTicks(4159), "$2a$11$PoYqum.Iy7mUSzwQe3gw.uMtoBeXFXzbrJHFhccGCqD9G.2UWty7q" });
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
