using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice.Migrations
{
    /// <inheritdoc />
    public partial class t1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Elections",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 4, 14, 16, 24, 697, DateTimeKind.Utc).AddTicks(4572), new DateTime(2026, 2, 4, 14, 16, 24, 697, DateTimeKind.Utc).AddTicks(4539), new DateTime(2026, 1, 4, 14, 16, 24, 697, DateTimeKind.Utc).AddTicks(4538) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 4, 14, 16, 24, 697, DateTimeKind.Utc).AddTicks(3451), "$2a$11$4ALuvHO86P8KYBwStm4cz.plxQCC51UyTXnLLfxwFaxOhS6KCBFtK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Elections",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 4, 11, 22, 27, 716, DateTimeKind.Utc).AddTicks(673), new DateTime(2026, 2, 4, 11, 22, 27, 716, DateTimeKind.Utc).AddTicks(618), new DateTime(2026, 1, 4, 11, 22, 27, 716, DateTimeKind.Utc).AddTicks(615) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 4, 11, 22, 27, 715, DateTimeKind.Utc).AddTicks(9490), "$2a$11$TQC3bSuS8iptPSYgQSkD8eOwTOeM6oOc/Kj6IN4RSuenXsvH3Mt32" });
        }
    }
}
