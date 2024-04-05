using Microsoft.EntityFrameworkCore;
using StatisticsService.Dto;
using StatisticsService.Models;
using StatisticsService.Utils;

namespace StatisticsService.Repositories;

public class StatsRepo : IStatsRepo
{
    private readonly DataContext dataContext;
    private readonly IJwtService jwtService;
    public StatsRepo(DataContext dataContext, IJwtService jwtService)
    {
        this.dataContext = dataContext;
        this.jwtService = jwtService;
    }

    public async Task<ApiResponse<object, Exception>> GetBusinessStats(Guid businessId, DateTime startTime, DateTime endTime)
    {
        if (startTime.AddHours(2) > DateTime.UtcNow)
            return new ApiResponse<object, Exception>(new Exception(ExceptionMessages.NOT_VALID));

        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == businessId);
        if (business is null)
            return new ApiResponse<object, Exception>(new Exception(ExceptionMessages.BUSINESS_NOT_FOUND));
        var userId = jwtService.GetUserId();
        if (business.UserId != userId)
            return new ApiResponse<object, Exception>(new Exception(ExceptionMessages.UNAUTHORIZED));

        var eduStats = await dataContext.EducationStats
            .Where(e => e.BusinessId == businessId && e.StartTime >= startTime && e.EndTime <= endTime)
            .ToListAsync();
        var expenseStats = await dataContext.ExpenseStats
            .Where(e => e.BusinessId == businessId && e.StartTime >= startTime && e.EndTime <= endTime)
            .ToListAsync();
        var hobbyStats = await dataContext.HobbyStats
            .Where(e => e.BusinessId == businessId && e.StartTime >= startTime && e.EndTime <= endTime)
            .ToListAsync();
        var incomeStats = await dataContext.IncomeStats
            .Where(e => e.BusinessId == businessId && e.StartTime >= startTime && e.EndTime <= endTime)
            .ToListAsync();
        var industryStats = await dataContext.IndustryStats
            .Where(e => e.BusinessId == businessId && e.StartTime >= startTime && e.EndTime <= endTime)
            .ToListAsync();
        var personalStats = await dataContext.PersonalStats
            .Where(e => e.BusinessId == businessId && e.StartTime >= startTime && e.EndTime <= endTime)
            .ToListAsync();
        var userStats = await dataContext.UserStats
            .Where(e => e.BusinessId == businessId && e.StartTime >= startTime && e.EndTime <= endTime)
            .ToListAsync();
        var workingHoursStats = await dataContext.WorkingHoursStats
            .Where(e => e.BusinessId == businessId && e.StartTime >= startTime && e.EndTime <= endTime)
            .ToListAsync();

        var response = new
        {
            userStats = HandleUserStats(userStats),
            eduStats = HandleEduStats(eduStats),
            expenseStats = HandleExpenseStats(expenseStats),
            hobbyStats = HandleHobbyStats(hobbyStats),
            incomeStats = HandleIncomeStats(incomeStats),
            industryStats = HandleIndustryStats(industryStats),
            personalStats = HandlePersonalStats(personalStats),
            workingHoursStats = HandleWorkingHoursStats(workingHoursStats)
        };
        return new ApiResponse<object, Exception>(response);
    }

    private object HandleEduStats(List<EducationStats> eduStats)
    {
        var EduStats = new
        {
            SumPrimary = eduStats.Sum(e => e.Primary),
            SumSecondary = eduStats.Sum(e => e.Secondary),
            SumHigher = eduStats.Sum(e => e.Higher),
            SumTechnical = eduStats.Sum(e => e.Technical)
        };
        return EduStats;
    }

    private object HandleExpenseStats(List<ExpenseStats> expenseStats)
    {
        var ExpenseStats = new
        {
            SumHousingExpenses = expenseStats.Sum(h => h.HousingExpenses),
            SumUtilities = expenseStats.Sum(h => h.Utilities),
            SumTransportationCosts = expenseStats.Sum(h => h.TransportationCosts),
            SumFoodAndGroceries = expenseStats.Sum(h => h.FoodAndGroceries),
            SumHealthcareExpenses = expenseStats.Sum(h => h.HealthcareExpenses),
            SumEducationExpenses = expenseStats.Sum(h => h.EducationExpenses),
            SumDebtPayments = expenseStats.Sum(h => h.DebtPayments),
            SumInsurancePremiums = expenseStats.Sum(h => h.InsurancePremiums),
            SumEntertainmentAndLeisure = expenseStats.Sum(h => h.EntertainmentAndLeisure),
            SumPersonalCareAndGrooming = expenseStats.Sum(h => h.PersonalCareAndGrooming),
            SumClothingAndApparel = expenseStats.Sum(h => h.ClothingAndApparel),
            SumSavingsAndInvestments = expenseStats.Sum(h => h.SavingsAndInvestments),
            SumTaxes = expenseStats.Sum(h => h.Taxes),
            SumChildcareAndEducation = expenseStats.Sum(h => h.ChildcareAndEducation),
            SumHomeAndGarden = expenseStats.Sum(h => h.HomeAndGarden),
            SumSubscriptionsAndMemberships = expenseStats.Sum(h => h.SubscriptionsAndMemberships),
            SumCharitableDonations = expenseStats.Sum(h => h.CharitableDonations),
            SumTravelAndVacation = expenseStats.Sum(h => h.TravelAndVacation),
            SumLegalAndFinancialServices = expenseStats.Sum(h => h.LegalAndFinancialServices),
            SumEmergencyFundAndContingencies = expenseStats.Sum(h => h.EmergencyFundAndContingencies),
            SumOther = expenseStats.Sum(h => h.Other)
        };
        return ExpenseStats;
    }

    private object HandleHobbyStats(List<HobbyStats> hobbyStats)
    {
        var HobbyStats = new
        {
            SumReading = hobbyStats.Sum(h => h.Reading),
            SumGardening = hobbyStats.Sum(h => h.Gardening),
            SumCookingAndBaking = hobbyStats.Sum(h => h.CookingAndBaking),
            SumPaintingAndDrawing = hobbyStats.Sum(h => h.PaintingAndDrawing),
            SumSportsAndPhysicalActivities = hobbyStats.Sum(h => h.SportsAndPhysicalActivities),
            SumPhotography = hobbyStats.Sum(h => h.Photography),
            SumPlayingMusicalInstruments = hobbyStats.Sum(h => h.PlayingMusicalInstruments),
            SumTraveling = hobbyStats.Sum(h => h.Traveling),
            SumCrafting = hobbyStats.Sum(h => h.Crafting),
            SumFishing = hobbyStats.Sum(h => h.Fishing),
            SumModelBuilding = hobbyStats.Sum(h => h.ModelBuilding),
            SumCollecting = hobbyStats.Sum(h => h.Collecting),
            SumHikingAndCamping = hobbyStats.Sum(h => h.HikingAndCamping),
            SumDIYHomeImprovement = hobbyStats.Sum(h => h.DIYHomeImprovement),
            SumBirdWatching = hobbyStats.Sum(h => h.BirdWatching),
            SumCycling = hobbyStats.Sum(h => h.Cycling),
            SumMeditationAndMindfulness = hobbyStats.Sum(h => h.MeditationAndMindfulness),
            SumDancing = hobbyStats.Sum(h => h.Dancing),
            SumVolunteering = hobbyStats.Sum(h => h.Volunteering),
            SumBoardGamesAndPuzzles = hobbyStats.Sum(h => h.BoardGamesAndPuzzles),
            SumWriting = hobbyStats.Sum(h => h.Writing),
            SumFitnessAndExercise = hobbyStats.Sum(h => h.FitnessAndExercise),
            SumComicsAndGraphicNovels = hobbyStats.Sum(h => h.ComicsAndGraphicNovels),
            SumSculpting = hobbyStats.Sum(h => h.Sculpting),
            SumAstrologyAndAstronomy = hobbyStats.Sum(h => h.AstrologyAndAstronomy),
            SumSewingAndQuilting = hobbyStats.Sum(h => h.SewingAndQuilting),
            SumArchery = hobbyStats.Sum(h => h.Archery),
            SumGenealogy = hobbyStats.Sum(h => h.Genealogy),
            SumMetalDetecting = hobbyStats.Sum(h => h.MetalDetecting),
            SumVintageAndClassicCars = hobbyStats.Sum(h => h.VintageAndClassicCars),
            SumOther = hobbyStats.Sum(h => h.Other)
        };
        return HobbyStats;
    }

    private object HandleIncomeStats(List<IncomeStats> incomeStats)
    {
        var IncomeStats = new
        {
            SumPovertyLine = incomeStats.Sum(i => i.PovertyLine),
            SumLowIncome = incomeStats.Sum(i => i.LowIncome),
            SumLowerMiddleIncome = incomeStats.Sum(i => i.LowerMiddleIncome),
            SumMedianIncome = incomeStats.Sum(i => i.MedianIncome),
            SumUpperMiddleIncome = incomeStats.Sum(i => i.UpperMiddleIncome),
            SumHighIncome = incomeStats.Sum(i => i.HighIncome),
            SumVeryHighIncome = incomeStats.Sum(i => i.VeryHighIncome)
        };
        return IncomeStats;
    }

    private object HandleIndustryStats(List<IndustryStats> industryStats)
    {
        var IndustryStats = new
        {
            SumInformationTechnology = industryStats.Sum(i => i.InformationTechnology),
            SumHealthcare = industryStats.Sum(i => i.Healthcare),
            SumFinanceAndBanking = industryStats.Sum(i => i.FinanceAndBanking),
            SumEducation = industryStats.Sum(i => i.Education),
            SumEngineering = industryStats.Sum(i => i.Engineering),
            SumManufacturingAndProduction = industryStats.Sum(i => i.ManufacturingAndProduction),
            SumRetail = industryStats.Sum(i => i.Retail),
            SumHospitalityAndTourism = industryStats.Sum(i => i.HospitalityAndTourism),
            SumMarketingAndAdvertising = industryStats.Sum(i => i.MarketingAndAdvertising),
            SumGovernmentAndPublicAdministration = industryStats.Sum(i => i.GovernmentAndPublicAdministration),
            SumLegal = industryStats.Sum(i => i.Legal),
            SumAgriculture = industryStats.Sum(i => i.Agriculture),
            SumMediaAndJournalism = industryStats.Sum(i => i.MediaAndJournalism),
            SumArtAndDesign = industryStats.Sum(i => i.ArtAndDesign),
            SumNonprofitAndSocialServices = industryStats.Sum(i => i.NonprofitAndSocialServices),
            SumEnvironmentalAndSustainability = industryStats.Sum(i => i.EnvironmentalAndSustainability),
            SumConstructionAndSkilledTrades = industryStats.Sum(i => i.ConstructionAndSkilledTrades),
            SumAutomotive = industryStats.Sum(i => i.Automotive),
            SumTransportationAndLogistics = industryStats.Sum(i => i.TransportationAndLogistics),
            SumSportsAndFitness = industryStats.Sum(i => i.SportsAndFitness),
            SumOnlineBusinesses = industryStats.Sum(i => i.OnlineBusinesses),
            SumPharmaceuticalsAndHealthcareResearch = industryStats.Sum(i => i.PharmaceuticalsAndHealthcareResearch),
            SumEntertainmentAndArts = industryStats.Sum(i => i.EntertainmentAndArts),
            SumTechStartupsAndEntrepreneurship = industryStats.Sum(i => i.TechStartupsAndEntrepreneurship),
            SumRealEstateAndPropertyManagement = industryStats.Sum(i => i.RealEstateAndPropertyManagement),
            SumFoodAndBeverage = industryStats.Sum(i => i.FoodAndBeverage),
            SumAviationAndAerospace = industryStats.Sum(i => i.AviationAndAerospace),
            SumRenewableEnergyAndGreenTechnology = industryStats.Sum(i => i.RenewableEnergyAndGreenTechnology),
            SumFitnessAndWellness = industryStats.Sum(i => i.FitnessAndWellness),
            SumEmergencyServicesAndPublicSafety = industryStats.Sum(i => i.EmergencyServicesAndPublicSafety),
            SumStudent = industryStats.Sum(i => i.Student),
            SumRetired = industryStats.Sum(i => i.Retired)
        };
        return IndustryStats;
    }

    private object HandlePersonalStats(List<PersonalStats> personalStats)
    {
        var PersonalStats = new
        {
            SumStayHome = personalStats.Sum(p => p.StayHome),
            SumGoOut = personalStats.Sum(p => p.GoOut),
            SumMarried = personalStats.Sum(p => p.Married),
            SumSingle = personalStats.Sum(p => p.Single)
        };
        return PersonalStats;
    }

    private object HandleUserStats(List<UserStats> userStats)
    {
        var UserStats = new
        {
            SumTotalUsers = userStats.Sum(u => u.TotalUsers),
            SumMales = userStats.Sum(u => u.Males),
            SumFemales = userStats.Sum(u => u.Females),
            SumYoungers = userStats.Sum(u => u.Youngers),
            SumAdults = userStats.Sum(u => u.Adults),
            SumOlderAdults = userStats.Sum(u => u.OlderAdults)
        };
        return UserStats;
    }

    private object HandleWorkingHoursStats(List<WorkingHoursStats> workingHoursStats)
    {
        var WorkingHoursStats = new
        {
            SumFullTimeEmployment = workingHoursStats.Sum(w => w.FullTimeEmployment),
            SumPartTimeEmployment = workingHoursStats.Sum(w => w.PartTimeEmployment),
            SumOvertimeHours = workingHoursStats.Sum(w => w.OvertimeHours),
            SumShiftWork = workingHoursStats.Sum(w => w.ShiftWork),
            SumFlexibleHours = workingHoursStats.Sum(w => w.FlexibleHours),
            SumSeasonalWork = workingHoursStats.Sum(w => w.SeasonalWork),
            SumFreelancingWork = workingHoursStats.Sum(w => w.FreelancingWork),
            SumContractWork = workingHoursStats.Sum(w => w.ContractWork),
            SumRemoteWork = workingHoursStats.Sum(w => w.RemoteWork),
            SumNightShifts = workingHoursStats.Sum(w => w.NightShifts),
            SumWeekendWork = workingHoursStats.Sum(w => w.WeekendWork),
            SumUnemployed = workingHoursStats.Sum(w => w.Unemployed)
        };
        return WorkingHoursStats;
    }


}