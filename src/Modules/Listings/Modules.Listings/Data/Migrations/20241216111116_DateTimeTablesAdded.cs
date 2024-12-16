using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Listings.Data.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFrom",
                schema: "Listings",
                table: "Listings",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableUntil",
                schema: "Listings",
                table: "Listings",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                schema: "Listings",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "AvailableUntil",
                schema: "Listings",
                table: "Listings");
        }
    }
}
