﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserService.Models;

#nullable disable

namespace UserService.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UserService.Models.EmailCode", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CodeCreated")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.ToTable("EmailCodes");
                });

            modelBuilder.Entity("UserService.Models.Expense", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserExpense")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "UserExpense");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("UserService.Models.Hobby", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserHobby")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "UserHobby");

                    b.ToTable("Hobbies");
                });

            modelBuilder.Entity("UserService.Models.PersonalLife", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Married")
                        .HasColumnType("bit");

                    b.Property<bool>("StayHome")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.ToTable("PersonalLife");
                });

            modelBuilder.Entity("UserService.Models.ProfessionalLife", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Income")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Industry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LevelOfEducation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkingHours")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("ProfessionalLife");
                });

            modelBuilder.Entity("UserService.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoOfBusinesses")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileStage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserService.Models.EmailCode", b =>
                {
                    b.HasOne("UserService.Models.User", "User")
                        .WithOne("UserEmailCode")
                        .HasForeignKey("UserService.Models.EmailCode", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Models.Expense", b =>
                {
                    b.HasOne("UserService.Models.User", "User")
                        .WithMany("Expenses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Models.Hobby", b =>
                {
                    b.HasOne("UserService.Models.User", "User")
                        .WithMany("Hobbies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Models.PersonalLife", b =>
                {
                    b.HasOne("UserService.Models.User", "User")
                        .WithOne("UserPersonalLife")
                        .HasForeignKey("UserService.Models.PersonalLife", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Models.ProfessionalLife", b =>
                {
                    b.HasOne("UserService.Models.User", "User")
                        .WithOne("UserProfessionalLife")
                        .HasForeignKey("UserService.Models.ProfessionalLife", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Models.User", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("Hobbies");

                    b.Navigation("UserEmailCode");

                    b.Navigation("UserPersonalLife");

                    b.Navigation("UserProfessionalLife");
                });
#pragma warning restore 612, 618
        }
    }
}
