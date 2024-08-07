using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatisticsService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.BusinessId);
                });

            migrationBuilder.CreateTable(
                name: "EducationStats",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Primary = table.Column<int>(type: "int", nullable: false),
                    Secondary = table.Column<int>(type: "int", nullable: false),
                    Higher = table.Column<int>(type: "int", nullable: false),
                    Technical = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationStats", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_EducationStats_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseStats",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HousingExpenses = table.Column<int>(type: "int", nullable: false),
                    Utilities = table.Column<int>(type: "int", nullable: false),
                    TransportationCosts = table.Column<int>(type: "int", nullable: false),
                    FoodAndGroceries = table.Column<int>(type: "int", nullable: false),
                    HealthcareExpenses = table.Column<int>(type: "int", nullable: false),
                    EducationExpenses = table.Column<int>(type: "int", nullable: false),
                    DebtPayments = table.Column<int>(type: "int", nullable: false),
                    InsurancePremiums = table.Column<int>(type: "int", nullable: false),
                    EntertainmentAndLeisure = table.Column<int>(type: "int", nullable: false),
                    PersonalCareAndGrooming = table.Column<int>(type: "int", nullable: false),
                    ClothingAndApparel = table.Column<int>(type: "int", nullable: false),
                    SavingsAndInvestments = table.Column<int>(type: "int", nullable: false),
                    Taxes = table.Column<int>(type: "int", nullable: false),
                    ChildcareAndEducation = table.Column<int>(type: "int", nullable: false),
                    HomeAndGarden = table.Column<int>(type: "int", nullable: false),
                    SubscriptionsAndMemberships = table.Column<int>(type: "int", nullable: false),
                    CharitableDonations = table.Column<int>(type: "int", nullable: false),
                    TravelAndVacation = table.Column<int>(type: "int", nullable: false),
                    LegalAndFinancialServices = table.Column<int>(type: "int", nullable: false),
                    EmergencyFundAndContingencies = table.Column<int>(type: "int", nullable: false),
                    Other = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseStats", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_ExpenseStats_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HobbyStats",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reading = table.Column<int>(type: "int", nullable: false),
                    Gardening = table.Column<int>(type: "int", nullable: false),
                    CookingAndBaking = table.Column<int>(type: "int", nullable: false),
                    PaintingAndDrawing = table.Column<int>(type: "int", nullable: false),
                    SportsAndPhysicalActivities = table.Column<int>(type: "int", nullable: false),
                    Photography = table.Column<int>(type: "int", nullable: false),
                    PlayingMusicalInstruments = table.Column<int>(type: "int", nullable: false),
                    Traveling = table.Column<int>(type: "int", nullable: false),
                    Crafting = table.Column<int>(type: "int", nullable: false),
                    Fishing = table.Column<int>(type: "int", nullable: false),
                    ModelBuilding = table.Column<int>(type: "int", nullable: false),
                    Collecting = table.Column<int>(type: "int", nullable: false),
                    HikingAndCamping = table.Column<int>(type: "int", nullable: false),
                    DIYHomeImprovement = table.Column<int>(type: "int", nullable: false),
                    BirdWatching = table.Column<int>(type: "int", nullable: false),
                    Cycling = table.Column<int>(type: "int", nullable: false),
                    MeditationAndMindfulness = table.Column<int>(type: "int", nullable: false),
                    Dancing = table.Column<int>(type: "int", nullable: false),
                    Volunteering = table.Column<int>(type: "int", nullable: false),
                    BoardGamesAndPuzzles = table.Column<int>(type: "int", nullable: false),
                    Writing = table.Column<int>(type: "int", nullable: false),
                    FitnessAndExercise = table.Column<int>(type: "int", nullable: false),
                    ComicsAndGraphicNovels = table.Column<int>(type: "int", nullable: false),
                    Sculpting = table.Column<int>(type: "int", nullable: false),
                    AstrologyAndAstronomy = table.Column<int>(type: "int", nullable: false),
                    SewingAndQuilting = table.Column<int>(type: "int", nullable: false),
                    Archery = table.Column<int>(type: "int", nullable: false),
                    Genealogy = table.Column<int>(type: "int", nullable: false),
                    MetalDetecting = table.Column<int>(type: "int", nullable: false),
                    VintageAndClassicCars = table.Column<int>(type: "int", nullable: false),
                    Other = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HobbyStats", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_HobbyStats_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncomeStats",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PovertyLine = table.Column<int>(type: "int", nullable: false),
                    LowIncome = table.Column<int>(type: "int", nullable: false),
                    LowerMiddleIncome = table.Column<int>(type: "int", nullable: false),
                    MedianIncome = table.Column<int>(type: "int", nullable: false),
                    UpperMiddleIncome = table.Column<int>(type: "int", nullable: false),
                    HighIncome = table.Column<int>(type: "int", nullable: false),
                    VeryHighIncome = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeStats", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_IncomeStats_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndustryStats",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InformationTechnology = table.Column<int>(type: "int", nullable: false),
                    Healthcare = table.Column<int>(type: "int", nullable: false),
                    FinanceAndBanking = table.Column<int>(type: "int", nullable: false),
                    Education = table.Column<int>(type: "int", nullable: false),
                    Engineering = table.Column<int>(type: "int", nullable: false),
                    ManufacturingAndProduction = table.Column<int>(type: "int", nullable: false),
                    Retail = table.Column<int>(type: "int", nullable: false),
                    HospitalityAndTourism = table.Column<int>(type: "int", nullable: false),
                    MarketingAndAdvertising = table.Column<int>(type: "int", nullable: false),
                    GovernmentAndPublicAdministration = table.Column<int>(type: "int", nullable: false),
                    Legal = table.Column<int>(type: "int", nullable: false),
                    Agriculture = table.Column<int>(type: "int", nullable: false),
                    MediaAndJournalism = table.Column<int>(type: "int", nullable: false),
                    ArtAndDesign = table.Column<int>(type: "int", nullable: false),
                    NonprofitAndSocialServices = table.Column<int>(type: "int", nullable: false),
                    EnvironmentalAndSustainability = table.Column<int>(type: "int", nullable: false),
                    ConstructionAndSkilledTrades = table.Column<int>(type: "int", nullable: false),
                    Automotive = table.Column<int>(type: "int", nullable: false),
                    TransportationAndLogistics = table.Column<int>(type: "int", nullable: false),
                    SportsAndFitness = table.Column<int>(type: "int", nullable: false),
                    OnlineBusinesses = table.Column<int>(type: "int", nullable: false),
                    PharmaceuticalsAndHealthcareResearch = table.Column<int>(type: "int", nullable: false),
                    EntertainmentAndArts = table.Column<int>(type: "int", nullable: false),
                    TechStartupsAndEntrepreneurship = table.Column<int>(type: "int", nullable: false),
                    RealEstateAndPropertyManagement = table.Column<int>(type: "int", nullable: false),
                    FoodAndBeverage = table.Column<int>(type: "int", nullable: false),
                    AviationAndAerospace = table.Column<int>(type: "int", nullable: false),
                    RenewableEnergyAndGreenTechnology = table.Column<int>(type: "int", nullable: false),
                    FitnessAndWellness = table.Column<int>(type: "int", nullable: false),
                    EmergencyServicesAndPublicSafety = table.Column<int>(type: "int", nullable: false),
                    Student = table.Column<int>(type: "int", nullable: false),
                    Retired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryStats", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_IndustryStats_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalStats",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StayHome = table.Column<int>(type: "int", nullable: false),
                    GoOut = table.Column<int>(type: "int", nullable: false),
                    Married = table.Column<int>(type: "int", nullable: false),
                    Single = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalStats", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_PersonalStats_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStats",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalUsers = table.Column<int>(type: "int", nullable: false),
                    Males = table.Column<int>(type: "int", nullable: false),
                    Females = table.Column<int>(type: "int", nullable: false),
                    Youngers = table.Column<int>(type: "int", nullable: false),
                    Adults = table.Column<int>(type: "int", nullable: false),
                    OlderAdults = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStats", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserStats_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingHoursStats",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullTimeEmployment = table.Column<int>(type: "int", nullable: false),
                    PartTimeEmployment = table.Column<int>(type: "int", nullable: false),
                    OvertimeHours = table.Column<int>(type: "int", nullable: false),
                    ShiftWork = table.Column<int>(type: "int", nullable: false),
                    FlexibleHours = table.Column<int>(type: "int", nullable: false),
                    SeasonalWork = table.Column<int>(type: "int", nullable: false),
                    FreelancingWork = table.Column<int>(type: "int", nullable: false),
                    ContractWork = table.Column<int>(type: "int", nullable: false),
                    RemoteWork = table.Column<int>(type: "int", nullable: false),
                    NightShifts = table.Column<int>(type: "int", nullable: false),
                    WeekendWork = table.Column<int>(type: "int", nullable: false),
                    Unemployed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingHoursStats", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_WorkingHoursStats_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationStats_BusinessId",
                table: "EducationStats",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseStats_BusinessId",
                table: "ExpenseStats",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_HobbyStats_BusinessId",
                table: "HobbyStats",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeStats_BusinessId",
                table: "IncomeStats",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustryStats_BusinessId",
                table: "IndustryStats",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalStats_BusinessId",
                table: "PersonalStats",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStats_BusinessId",
                table: "UserStats",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHoursStats_BusinessId",
                table: "WorkingHoursStats",
                column: "BusinessId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationStats");

            migrationBuilder.DropTable(
                name: "ExpenseStats");

            migrationBuilder.DropTable(
                name: "HobbyStats");

            migrationBuilder.DropTable(
                name: "IncomeStats");

            migrationBuilder.DropTable(
                name: "IndustryStats");

            migrationBuilder.DropTable(
                name: "PersonalStats");

            migrationBuilder.DropTable(
                name: "UserStats");

            migrationBuilder.DropTable(
                name: "WorkingHoursStats");

            migrationBuilder.DropTable(
                name: "Businesses");
        }
    }
}
