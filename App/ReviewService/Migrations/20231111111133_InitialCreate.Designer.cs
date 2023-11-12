﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReviewService.Models;

#nullable disable

namespace ReviewService.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231111111133_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ReviewService.Models.Business", b =>
                {
                    b.Property<Guid>("BusinessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BusinessId");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("ReviewService.Models.Response", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ReviewId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.HasIndex("ReviewId")
                        .IsUnique()
                        .HasFilter("[ReviewId] IS NOT NULL");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("ReviewService.Models.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("ReviewService.Models.VerifiedOrder", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "BusinessId");

                    b.HasIndex("BusinessId");

                    b.ToTable("VerifiedOrders");
                });

            modelBuilder.Entity("ReviewService.Models.Response", b =>
                {
                    b.HasOne("ReviewService.Models.Business", "Business")
                        .WithMany("Responses")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReviewService.Models.Review", "Review")
                        .WithOne("Response")
                        .HasForeignKey("ReviewService.Models.Response", "ReviewId");

                    b.Navigation("Business");

                    b.Navigation("Review");
                });

            modelBuilder.Entity("ReviewService.Models.Review", b =>
                {
                    b.HasOne("ReviewService.Models.Business", "Business")
                        .WithMany("Reviews")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("ReviewService.Models.VerifiedOrder", b =>
                {
                    b.HasOne("ReviewService.Models.Business", "Business")
                        .WithMany("VerifiedOrders")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("ReviewService.Models.Business", b =>
                {
                    b.Navigation("Responses");

                    b.Navigation("Reviews");

                    b.Navigation("VerifiedOrders");
                });

            modelBuilder.Entity("ReviewService.Models.Review", b =>
                {
                    b.Navigation("Response")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
