﻿// <auto-generated />
using System;
using BikesRentalServer.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BikesRentalServer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210329002417_AddUserAuthorizationData")]
    partial class AddUserAuthorizationData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BikesRentalServer.Models.Bike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StationId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.HasIndex("UserId");

                    b.ToTable("Bikes");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Malfunction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BikeId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DetectionDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ReportingUserId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BikeId");

                    b.HasIndex("ReportingUserId");

                    b.ToTable("Malfunctions");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Rental", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BikeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BikeId");

                    b.HasIndex("UserId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BikeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BikeId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("BikesRentalServer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Bike", b =>
                {
                    b.HasOne("BikesRentalServer.Models.Station", "Station")
                        .WithMany("Bikes")
                        .HasForeignKey("StationId");

                    b.HasOne("BikesRentalServer.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Station");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Malfunction", b =>
                {
                    b.HasOne("BikesRentalServer.Models.Bike", "Bike")
                        .WithMany()
                        .HasForeignKey("BikeId");

                    b.HasOne("BikesRentalServer.Models.User", "ReportingUser")
                        .WithMany()
                        .HasForeignKey("ReportingUserId");

                    b.Navigation("Bike");

                    b.Navigation("ReportingUser");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Rental", b =>
                {
                    b.HasOne("BikesRentalServer.Models.Bike", "Bike")
                        .WithMany()
                        .HasForeignKey("BikeId");

                    b.HasOne("BikesRentalServer.Models.User", "User")
                        .WithMany("Rentals")
                        .HasForeignKey("UserId");

                    b.Navigation("Bike");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Reservation", b =>
                {
                    b.HasOne("BikesRentalServer.Models.Bike", "Bike")
                        .WithMany()
                        .HasForeignKey("BikeId");

                    b.HasOne("BikesRentalServer.Models.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId");

                    b.Navigation("Bike");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Station", b =>
                {
                    b.Navigation("Bikes");
                });

            modelBuilder.Entity("BikesRentalServer.Models.User", b =>
                {
                    b.Navigation("Rentals");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
