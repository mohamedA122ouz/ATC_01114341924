using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eventsBook.Migrations
{
    /// <inheritdoc />
    public partial class makecountmoreeffecient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "count",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "count" },
                values: new object[] { new DateTime(2025, 5, 31, 20, 54, 53, 206, DateTimeKind.Local).AddTicks(6049), 0 });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "count",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "count",
                table: "Events");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2025, 5, 26, 7, 59, 57, 126, DateTimeKind.Local).AddTicks(4694));
        }
    }
}
