﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Modules.Listings.Data;

#nullable disable

namespace Modules.Listings.Data.Migrations
{
    [DbContext(typeof(ListingDbContext))]
    partial class ListingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Listings")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Modules.Listings.Entities.Facility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Facilities", "Listings");
                });

            modelBuilder.Entity("Modules.Listings.Entities.Listing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccommodationType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("AvailableFrom")
                        .HasColumnType("date");

                    b.Property<DateTime>("AvailableUntil")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<Guid>("HostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MainLocation")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SubLocation")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("MainLocation");

                    b.HasIndex("SubLocation");

                    b.ToTable("Listings", "Listings");
                });

            modelBuilder.Entity("Modules.Listings.Entities.ListingFacility", b =>
                {
                    b.Property<Guid>("ListingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FacilityId")
                        .HasColumnType("int");

                    b.HasKey("ListingId", "FacilityId");

                    b.HasIndex("FacilityId");

                    b.ToTable("ListingFacilities", "Listings");
                });

            modelBuilder.Entity("Modules.Listings.Entities.ListingImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AltText")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid>("ListingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("ListingId");

                    b.ToTable("ListingImages", "Listings");
                });

            modelBuilder.Entity("Modules.Listings.Entities.ListingFacility", b =>
                {
                    b.HasOne("Modules.Listings.Entities.Facility", "Facility")
                        .WithMany("ListingFacilities")
                        .HasForeignKey("FacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Modules.Listings.Entities.Listing", "Listing")
                        .WithMany("ListingFacilities")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Facility");

                    b.Navigation("Listing");
                });

            modelBuilder.Entity("Modules.Listings.Entities.ListingImage", b =>
                {
                    b.HasOne("Modules.Listings.Entities.Listing", "Listing")
                        .WithMany("Images")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Listing");
                });

            modelBuilder.Entity("Modules.Listings.Entities.Facility", b =>
                {
                    b.Navigation("ListingFacilities");
                });

            modelBuilder.Entity("Modules.Listings.Entities.Listing", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("ListingFacilities");
                });
#pragma warning restore 612, 618
        }
    }
}
