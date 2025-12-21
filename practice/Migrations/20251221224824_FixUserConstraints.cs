using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice.Migrations
{
    /// <inheritdoc />
    public partial class FixUserConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Elections",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 12, 21, 22, 48, 24, 351, DateTimeKind.Utc).AddTicks(3574), new DateTime(2026, 1, 21, 22, 48, 24, 351, DateTimeKind.Utc).AddTicks(3537), new DateTime(2025, 12, 21, 22, 48, 24, 351, DateTimeKind.Utc).AddTicks(3535) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 21, 22, 48, 24, 351, DateTimeKind.Utc).AddTicks(2662), "$2a$11$g/UrwkCTPLsiYIYIHaCaV.Tp/0uSkOpB.I7zSD2dV/orTPBDlXake" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Elections",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 12, 21, 22, 41, 39, 674, DateTimeKind.Utc).AddTicks(8358), new DateTime(2026, 1, 21, 22, 41, 39, 674, DateTimeKind.Utc).AddTicks(8306), new DateTime(2025, 12, 21, 22, 41, 39, 674, DateTimeKind.Utc).AddTicks(8303) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 21, 22, 41, 39, 672, DateTimeKind.Utc).AddTicks(1275), "$2a$11$9.XIhR4O6l1/mENkUUoRi.lgr66Udjau/qyib.2oHsDWn6T4uusU2" });
        }
    }
}
