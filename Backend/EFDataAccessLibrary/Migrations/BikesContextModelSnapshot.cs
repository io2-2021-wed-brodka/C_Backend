﻿// <auto-generated />
using System;
using EFDataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFDataAccessLibrary.Migrations
{
    [DbContext(typeof(BikesContext))]
    partial class BikesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EFDataAccessLibrary.Models.Bike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int?>("StationId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.HasIndex("UserId");

                    b.ToTable("Bikes");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.BikeStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BikeStations");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Malfunction", b =>
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

            modelBuilder.Entity("EFDataAccessLibrary.Models.Rental", b =>
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

            modelBuilder.Entity("EFDataAccessLibrary.Models.Reservation", b =>
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

            modelBuilder.Entity("EFDataAccessLibrary.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Lastname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Bike", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.BikeStation", "Station")
                        .WithMany("Bikes")
                        .HasForeignKey("StationId");

                    b.HasOne("EFDataAccessLibrary.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Station");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Malfunction", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Bike", "Bike")
                        .WithMany()
                        .HasForeignKey("BikeId");

                    b.HasOne("EFDataAccessLibrary.Models.User", "ReportingUser")
                        .WithMany()
                        .HasForeignKey("ReportingUserId");

                    b.Navigation("Bike");

                    b.Navigation("ReportingUser");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Rental", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Bike", "Bike")
                        .WithMany()
                        .HasForeignKey("BikeId");

                    b.HasOne("EFDataAccessLibrary.Models.User", "User")
                        .WithMany("Rentals")
                        .HasForeignKey("UserId");

                    b.Navigation("Bike");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.Reservation", b =>
                {
                    b.HasOne("EFDataAccessLibrary.Models.Bike", "Bike")
                        .WithMany()
                        .HasForeignKey("BikeId");

                    b.HasOne("EFDataAccessLibrary.Models.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId");

                    b.Navigation("Bike");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.BikeStation", b =>
                {
                    b.Navigation("Bikes");
                });

            modelBuilder.Entity("EFDataAccessLibrary.Models.User", b =>
                {
                    b.Navigation("Rentals");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
