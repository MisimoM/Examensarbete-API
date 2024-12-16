using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Listings.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Listings");

            migrationBuilder.CreateTable(
                name: "Listings",
                schema: "Listings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccommodationType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MainLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SubLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_MainLocation",
                schema: "Listings",
                table: "Listings",
                column: "MainLocation");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_SubLocation",
                schema: "Listings",
                table: "Listings",
                column: "SubLocation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Listings",
                schema: "Listings");
        }
    }
}
