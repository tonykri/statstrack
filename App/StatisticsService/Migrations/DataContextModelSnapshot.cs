﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StatisticsService.Models;

#nullable disable

namespace StatisticsService.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StatisticsService.Models.Business", b =>
                {
                    b.Property<Guid>("BusinessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BusinessId");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("StatisticsService.Models.EducationStats", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Higher")
                        .HasColumnType("int");

                    b.Property<int>("Primary")
                        .HasColumnType("int");

                    b.Property<int>("Secondary")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Technical")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("BusinessId");

                    b.ToTable("EducationStats");
                });

            modelBuilder.Entity("StatisticsService.Models.ExpenseStats", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CharitableDonations")
                        .HasColumnType("int");

                    b.Property<int>("ChildcareAndEducation")
                        .HasColumnType("int");

                    b.Property<int>("ClothingAndApparel")
                        .HasColumnType("int");

                    b.Property<int>("DebtPayments")
                        .HasColumnType("int");

                    b.Property<int>("EducationExpenses")
                        .HasColumnType("int");

                    b.Property<int>("EmergencyFundAndContingencies")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("EntertainmentAndLeisure")
                        .HasColumnType("int");

                    b.Property<int>("FoodAndGroceries")
                        .HasColumnType("int");

                    b.Property<int>("HealthcareExpenses")
                        .HasColumnType("int");

                    b.Property<int>("HomeAndGarden")
                        .HasColumnType("int");

                    b.Property<int>("HousingExpenses")
                        .HasColumnType("int");

                    b.Property<int>("InsurancePremiums")
                        .HasColumnType("int");

                    b.Property<int>("LegalAndFinancialServices")
                        .HasColumnType("int");

                    b.Property<int>("Other")
                        .HasColumnType("int");

                    b.Property<int>("PersonalCareAndGrooming")
                        .HasColumnType("int");

                    b.Property<int>("SavingsAndInvestments")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("SubscriptionsAndMemberships")
                        .HasColumnType("int");

                    b.Property<int>("Taxes")
                        .HasColumnType("int");

                    b.Property<int>("TransportationCosts")
                        .HasColumnType("int");

                    b.Property<int>("TravelAndVacation")
                        .HasColumnType("int");

                    b.Property<int>("Utilities")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("BusinessId");

                    b.ToTable("ExpenseStats");
                });

            modelBuilder.Entity("StatisticsService.Models.HobbyStats", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Archery")
                        .HasColumnType("int");

                    b.Property<int>("AstrologyAndAstronomy")
                        .HasColumnType("int");

                    b.Property<int>("BirdWatching")
                        .HasColumnType("int");

                    b.Property<int>("BoardGamesAndPuzzles")
                        .HasColumnType("int");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Collecting")
                        .HasColumnType("int");

                    b.Property<int>("ComicsAndGraphicNovels")
                        .HasColumnType("int");

                    b.Property<int>("CookingAndBaking")
                        .HasColumnType("int");

                    b.Property<int>("Crafting")
                        .HasColumnType("int");

                    b.Property<int>("Cycling")
                        .HasColumnType("int");

                    b.Property<int>("DIYHomeImprovement")
                        .HasColumnType("int");

                    b.Property<int>("Dancing")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Fishing")
                        .HasColumnType("int");

                    b.Property<int>("FitnessAndExercise")
                        .HasColumnType("int");

                    b.Property<int>("Gardening")
                        .HasColumnType("int");

                    b.Property<int>("Genealogy")
                        .HasColumnType("int");

                    b.Property<int>("HikingAndCamping")
                        .HasColumnType("int");

                    b.Property<int>("MeditationAndMindfulness")
                        .HasColumnType("int");

                    b.Property<int>("MetalDetecting")
                        .HasColumnType("int");

                    b.Property<int>("ModelBuilding")
                        .HasColumnType("int");

                    b.Property<int>("Other")
                        .HasColumnType("int");

                    b.Property<int>("PaintingAndDrawing")
                        .HasColumnType("int");

                    b.Property<int>("Photography")
                        .HasColumnType("int");

                    b.Property<int>("PlayingMusicalInstruments")
                        .HasColumnType("int");

                    b.Property<int>("Reading")
                        .HasColumnType("int");

                    b.Property<int>("Sculpting")
                        .HasColumnType("int");

                    b.Property<int>("SewingAndQuilting")
                        .HasColumnType("int");

                    b.Property<int>("SportsAndPhysicalActivities")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Traveling")
                        .HasColumnType("int");

                    b.Property<int>("VintageAndClassicCars")
                        .HasColumnType("int");

                    b.Property<int>("Volunteering")
                        .HasColumnType("int");

                    b.Property<int>("Writing")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("BusinessId");

                    b.ToTable("HobbyStats");
                });

            modelBuilder.Entity("StatisticsService.Models.IncomeStats", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("HighIncome")
                        .HasColumnType("int");

                    b.Property<int>("LowIncome")
                        .HasColumnType("int");

                    b.Property<int>("LowerMiddleIncome")
                        .HasColumnType("int");

                    b.Property<int>("MedianIncome")
                        .HasColumnType("int");

                    b.Property<int>("PovertyLine")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UpperMiddleIncome")
                        .HasColumnType("int");

                    b.Property<int>("VeryHighIncome")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("BusinessId");

                    b.ToTable("IncomeStats");
                });

            modelBuilder.Entity("StatisticsService.Models.IndustryStats", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Agriculture")
                        .HasColumnType("int");

                    b.Property<int>("ArtAndDesign")
                        .HasColumnType("int");

                    b.Property<int>("Automotive")
                        .HasColumnType("int");

                    b.Property<int>("AviationAndAerospace")
                        .HasColumnType("int");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ConstructionAndSkilledTrades")
                        .HasColumnType("int");

                    b.Property<int>("Education")
                        .HasColumnType("int");

                    b.Property<int>("EmergencyServicesAndPublicSafety")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Engineering")
                        .HasColumnType("int");

                    b.Property<int>("EntertainmentAndArts")
                        .HasColumnType("int");

                    b.Property<int>("EnvironmentalAndSustainability")
                        .HasColumnType("int");

                    b.Property<int>("FinanceAndBanking")
                        .HasColumnType("int");

                    b.Property<int>("FitnessAndWellness")
                        .HasColumnType("int");

                    b.Property<int>("FoodAndBeverage")
                        .HasColumnType("int");

                    b.Property<int>("GovernmentAndPublicAdministration")
                        .HasColumnType("int");

                    b.Property<int>("Healthcare")
                        .HasColumnType("int");

                    b.Property<int>("HospitalityAndTourism")
                        .HasColumnType("int");

                    b.Property<int>("InformationTechnology")
                        .HasColumnType("int");

                    b.Property<int>("Legal")
                        .HasColumnType("int");

                    b.Property<int>("ManufacturingAndProduction")
                        .HasColumnType("int");

                    b.Property<int>("MarketingAndAdvertising")
                        .HasColumnType("int");

                    b.Property<int>("MediaAndJournalism")
                        .HasColumnType("int");

                    b.Property<int>("NonprofitAndSocialServices")
                        .HasColumnType("int");

                    b.Property<int>("OnlineBusinesses")
                        .HasColumnType("int");

                    b.Property<int>("PharmaceuticalsAndHealthcareResearch")
                        .HasColumnType("int");

                    b.Property<int>("RealEstateAndPropertyManagement")
                        .HasColumnType("int");

                    b.Property<int>("RenewableEnergyAndGreenTechnology")
                        .HasColumnType("int");

                    b.Property<int>("Retail")
                        .HasColumnType("int");

                    b.Property<int>("Retired")
                        .HasColumnType("int");

                    b.Property<int>("SportsAndFitness")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Student")
                        .HasColumnType("int");

                    b.Property<int>("TechStartupsAndEntrepreneurship")
                        .HasColumnType("int");

                    b.Property<int>("TransportationAndLogistics")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("BusinessId");

                    b.ToTable("IndustryStats");
                });

            modelBuilder.Entity("StatisticsService.Models.PersonalStats", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("GoOut")
                        .HasColumnType("int");

                    b.Property<int>("Married")
                        .HasColumnType("int");

                    b.Property<int>("Single")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("StayHome")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("BusinessId");

                    b.ToTable("PersonalStats");
                });

            modelBuilder.Entity("StatisticsService.Models.UserStats", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Adults")
                        .HasColumnType("int");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Females")
                        .HasColumnType("int");

                    b.Property<int>("Males")
                        .HasColumnType("int");

                    b.Property<int>("OlderAdults")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TotalUsers")
                        .HasColumnType("int");

                    b.Property<int>("Youngers")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("BusinessId");

                    b.ToTable("UserStats");
                });

            modelBuilder.Entity("StatisticsService.Models.WorkingHoursStats", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ContractWork")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("FlexibleHours")
                        .HasColumnType("int");

                    b.Property<int>("FreelancingWork")
                        .HasColumnType("int");

                    b.Property<int>("FullTimeEmployment")
                        .HasColumnType("int");

                    b.Property<int>("NightShifts")
                        .HasColumnType("int");

                    b.Property<int>("OvertimeHours")
                        .HasColumnType("int");

                    b.Property<int>("PartTimeEmployment")
                        .HasColumnType("int");

                    b.Property<int>("RemoteWork")
                        .HasColumnType("int");

                    b.Property<int>("SeasonalWork")
                        .HasColumnType("int");

                    b.Property<int>("ShiftWork")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Unemployed")
                        .HasColumnType("int");

                    b.Property<int>("WeekendWork")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("BusinessId");

                    b.ToTable("WorkingHoursStats");
                });

            modelBuilder.Entity("StatisticsService.Models.EducationStats", b =>
                {
                    b.HasOne("StatisticsService.Models.Business", "Business")
                        .WithMany("EducationStats")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("StatisticsService.Models.ExpenseStats", b =>
                {
                    b.HasOne("StatisticsService.Models.Business", "Business")
                        .WithMany("ExpenseStats")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("StatisticsService.Models.HobbyStats", b =>
                {
                    b.HasOne("StatisticsService.Models.Business", "Business")
                        .WithMany("HobbyStats")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("StatisticsService.Models.IncomeStats", b =>
                {
                    b.HasOne("StatisticsService.Models.Business", "Business")
                        .WithMany("IncomeStats")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("StatisticsService.Models.IndustryStats", b =>
                {
                    b.HasOne("StatisticsService.Models.Business", "Business")
                        .WithMany("IndustryStats")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("StatisticsService.Models.PersonalStats", b =>
                {
                    b.HasOne("StatisticsService.Models.Business", "Business")
                        .WithMany("PersonalStats")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("StatisticsService.Models.UserStats", b =>
                {
                    b.HasOne("StatisticsService.Models.Business", "Business")
                        .WithMany("UserStats")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("StatisticsService.Models.WorkingHoursStats", b =>
                {
                    b.HasOne("StatisticsService.Models.Business", "Business")
                        .WithMany("WorkingHoursStats")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("StatisticsService.Models.Business", b =>
                {
                    b.Navigation("EducationStats");

                    b.Navigation("ExpenseStats");

                    b.Navigation("HobbyStats");

                    b.Navigation("IncomeStats");

                    b.Navigation("IndustryStats");

                    b.Navigation("PersonalStats");

                    b.Navigation("UserStats");

                    b.Navigation("WorkingHoursStats");
                });
#pragma warning restore 612, 618
        }
    }
}
