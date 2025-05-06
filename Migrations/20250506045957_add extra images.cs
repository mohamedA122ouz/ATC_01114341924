using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eventsBook.Migrations
{
    /// <inheritdoc />
    public partial class addextraimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2025, 5, 26, 7, 59, 57, 126, DateTimeKind.Local).AddTicks(4694));

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "EventId", "Url" },
                values: new object[,]
                {
                    { 3, 2, "/images/picnic2.jpg" },
                    { 4, 2, "/images/picnic3.jpg" },
                    { 5, 2, "/images/picnic4.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2025, 5, 26, 7, 28, 46, 589, DateTimeKind.Local).AddTicks(3973));
        }
    }
}
