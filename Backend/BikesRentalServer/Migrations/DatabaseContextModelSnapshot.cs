﻿// <auto-generated />
using System;
using BikesRentalServer.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BikesRentalServer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("BikesRentalServer.Models.Bike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("StationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.HasIndex("UserId");

                    b.ToTable("Bikes");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Malfunction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BikeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DetectionDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ReportingUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BikeId");

                    b.HasIndex("ReportingUserId");

                    b.ToTable("Malfunctions");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BikeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BikeId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("BikesRentalServer.Models.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("BikesRentalServer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

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
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
