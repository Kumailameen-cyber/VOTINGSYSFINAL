using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice.Migrations
{
    /// <inheritdoc />
    public partial class clone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Elections",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 1, 3, 21, 53, 2, 452, DateTimeKind.Utc).AddTicks(9842), new DateTime(2026, 2, 3, 21, 53, 2, 452, DateTimeKind.Utc).AddTicks(9809), new DateTime(2026, 1, 3, 21, 53, 2, 452, DateTimeKind.Utc).AddTicks(9809) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 3, 21, 53, 2, 452, DateTimeKind.Utc).AddTicks(9176), "$2a$11$s8WK4Z8q/CD4RHA.0xRFv.3NVi5J36C9PLy5Gkh.qNL4zVjpoUP/G" });
        }
    }
}
