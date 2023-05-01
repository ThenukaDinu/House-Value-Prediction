﻿// <auto-generated />
using System;
using Micro_House_Manage_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Micro_House_Manage_API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230501145236_housePhotosReferences")]
    partial class housePhotosReferences
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Models.House", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("A1stFlrSF")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FullBath")
                        .HasColumnType("int");

                    b.Property<int>("GarageArea")
                        .HasColumnType("int");

                    b.Property<int>("GarageCars")
                        .HasColumnType("int");

                    b.Property<int>("GrLivArea")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OverallQual")
                        .HasColumnType("int");

                    b.Property<double>("PredictedPrice")
                        .HasColumnType("float");

                    b.Property<double>("PredictedPriceLKR")
                        .HasColumnType("float");

                    b.Property<int>("TotRmsAbvGrd")
                        .HasColumnType("int");

                    b.Property<int>("TotalBsmtSF")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("YearBuilt")
                        .HasColumnType("int");

                    b.Property<int>("YearRemodAdd")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("Models.HousePhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("HouseId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HouseId");

                    b.ToTable("HousePhotos");
                });

            modelBuilder.Entity("Models.Inquiry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("HouseId")
                        .HasColumnType("int");

                    b.Property<int>("InquiryStatus")
                        .HasColumnType("int");

                    b.Property<int?>("ListingId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResponseMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ResponseTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HouseId");

                    b.HasIndex("ListingId");

                    b.ToTable("Inquiries");
                });

            modelBuilder.Entity("Models.Listing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HouseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFeatured")
                        .HasColumnType("bit");

                    b.Property<double>("ListingPrice")
                        .HasColumnType("float");

                    b.Property<int>("ListingStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HouseId")
                        .IsUnique();

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("Models.HousePhoto", b =>
                {
                    b.HasOne("Models.House", null)
                        .WithMany("HousePhotos")
                        .HasForeignKey("HouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Inquiry", b =>
                {
                    b.HasOne("Models.House", "House")
                        .WithMany("Inquiries")
                        .HasForeignKey("HouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Listing", "Listing")
                        .WithMany()
                        .HasForeignKey("ListingId");

                    b.Navigation("House");

                    b.Navigation("Listing");
                });

            modelBuilder.Entity("Models.Listing", b =>
                {
                    b.HasOne("Models.House", "House")
                        .WithOne("Listing")
                        .HasForeignKey("Models.Listing", "HouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("House");
                });

            modelBuilder.Entity("Models.House", b =>
                {
                    b.Navigation("HousePhotos");

                    b.Navigation("Inquiries");

                    b.Navigation("Listing");
                });
#pragma warning restore 612, 618
        }
    }
}
