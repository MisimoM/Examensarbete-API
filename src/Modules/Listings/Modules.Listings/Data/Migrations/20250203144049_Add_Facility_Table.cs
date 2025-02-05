using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Listings.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Facility_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facilities",
                schema: "Listings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListingFacilities",
                schema: "Listings",
                columns: table => new
                {
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingFacilities", x => new { x.ListingId, x.FacilityId });
                    table.ForeignKey(
                        name: "FK_ListingFacilities_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalSchema: "Listings",
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingFacilities_Listings_ListingId",
                        column: x => x.ListingId,
                        principalSchema: "Listings",
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingFacilities_FacilityId",
                schema: "Listings",
                table: "ListingFacilities",
                column: "FacilityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingFacilities",
                schema: "Listings");

            migrationBuilder.DropTable(
                name: "Facilities",
                schema: "Listings");
        }
    }
}
