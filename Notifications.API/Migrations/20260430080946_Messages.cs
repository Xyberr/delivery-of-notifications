using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Notifications.API.Migrations
{
    /// <inheritdoc />
    public partial class Messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContactTypes",
                columns: new[] { "Id", "Code", "CreatedAt", "DeletedAt", "Description", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, 0, new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4815), null, "Email address", false, "Email", new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4816) },
                    { 2L, 1, new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4817), null, "Phone number", false, "Phone", new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4818) }
                });

            migrationBuilder.UpdateData(
                table: "DeliveryStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4915), new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4915) });

            migrationBuilder.UpdateData(
                table: "DeliveryStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4917), new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4917) });

            migrationBuilder.UpdateData(
                table: "DeliveryStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4918), new DateTime(2026, 4, 30, 8, 9, 46, 1, DateTimeKind.Utc).AddTicks(4919) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContactTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ContactTypes",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.UpdateData(
                table: "DeliveryStatuses",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 30, 8, 2, 3, 604, DateTimeKind.Utc).AddTicks(1814), new DateTime(2026, 4, 30, 8, 2, 3, 604, DateTimeKind.Utc).AddTicks(1816) });

            migrationBuilder.UpdateData(
                table: "DeliveryStatuses",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 30, 8, 2, 3, 604, DateTimeKind.Utc).AddTicks(1819), new DateTime(2026, 4, 30, 8, 2, 3, 604, DateTimeKind.Utc).AddTicks(1819) });

            migrationBuilder.UpdateData(
                table: "DeliveryStatuses",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 30, 8, 2, 3, 604, DateTimeKind.Utc).AddTicks(1821), new DateTime(2026, 4, 30, 8, 2, 3, 604, DateTimeKind.Utc).AddTicks(1821) });
        }
    }
}
