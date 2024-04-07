using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StatisticsService.Dto;
using StatisticsService.Models;
using StatisticsService.Utils;

namespace StatisticsService.Services;

public class StatsService : IStatsService
{
    private readonly string baseUrl = "http://localhost:4004";
    private readonly string userServiceEndpoint = "";
    private readonly string businessServiceEndpoint = "";

    private readonly IJwtService jwtService;
    private readonly DataContext dataContext;
    public StatsService(IJwtService jwtService, DataContext dataContext)
    {
        this.jwtService = jwtService;
        this.dataContext = dataContext;
    }

    private async Task<List<Guid>> GetUserIdsAsync(double businessLat, double businessLong, DateTime startTime, DateTime endTime)
    {
        List<Guid>? userIds = new List<Guid>();
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string apiUrl = $"{baseUrl}?businessLat={businessLat}&businessLong={businessLong}&startTime={startTime}&endTime={endTime}";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                string responseContent = await response.Content.ReadAsStringAsync();
                List<Guid>? temp = JsonConvert.DeserializeObject<List<Guid>>(responseContent);
                if (temp is not null)
                    userIds = temp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return userIds;
        }
    }

    private async Task<List<UserAccountDto>> GetUserProfilesAsync(List<Guid> userIds)
    {
        List<UserAccountDto> userProfiles = new List<UserAccountDto>();
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string baseUrl = "";
                string apiUrl = $"{baseUrl}/{userServiceEndpoint}";
                var requestData = new Dictionary<string, List<Guid>>
                {
                    { "UserIds", userIds }
                };
                string json = JsonConvert.SerializeObject(requestData);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                string token = jwtService.CreateToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                string responseContent = await response.Content.ReadAsStringAsync();
                List<UserAccountDto>? temp = JsonConvert.DeserializeObject<List<UserAccountDto>>(responseContent);
                if (temp is not null)
                    userProfiles = temp;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return userProfiles;
        }
    }

    private async Task<BusinessLocationDto?> GetBusinessLocationAsync(Guid businessId)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string baseUrl = "";
                string apiUrl = $"{baseUrl}/{businessServiceEndpoint}?businessid={businessId}";

                string token = jwtService.CreateToken();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                string responseContent = await response.Content.ReadAsStringAsync();
                BusinessLocationDto? businessLocation = JsonConvert.DeserializeObject<BusinessLocationDto>(responseContent);
                return businessLocation;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }

    public async void CreateHourlyStatsAsync(Guid businessId, DateTime startTime, DateTime endTime)
    {
        var business = await dataContext.Businesses.FirstOrDefaultAsync(b => b.BusinessId == businessId);
        if (business is null)
            return;

        BusinessLocationDto? businessLocation = await GetBusinessLocationAsync(businessId);
        if (businessLocation is null)
            return;
        List<Guid> ids = await GetUserIdsAsync(businessLocation.Latitude, businessLocation.Longitude, startTime, endTime);
        List<UserAccountDto> userProfiles = await GetUserProfilesAsync(ids);

        var educationStats = new EducationStats(business, startTime, endTime);
        var expenseStats = new ExpenseStats(business, startTime, endTime);
        var hobbyStats = new HobbyStats(business, startTime, endTime);
        var incomeStats = new IncomeStats(business, startTime, endTime);
        var industryStats = new IndustryStats(business, startTime, endTime);
        var personalStats = new PersonalStats(business, startTime, endTime);
        var userStats = new UserStats(business, startTime, endTime);
        var workingHours = new WorkingHoursStats(business, startTime, endTime);

        foreach (var user in userProfiles)
        {
            UpdateUserStats(user, userStats);
            UpdateWorkingHoursStats(user, workingHours);
            UpdatePersonalStatsStats(user, personalStats);
            UpdateIndustryStats(user, industryStats);
            UpdateIncomeStats(user, incomeStats);
            UpdateHobbyStats(user, hobbyStats);
            UpdateExpenseStats(user, expenseStats);
            UpdateEducationStats(user, educationStats);
        }

        dataContext.UserStats.Add(userStats);
        dataContext.WorkingHoursStats.Add(workingHours);
        dataContext.IncomeStats.Add(incomeStats);
        dataContext.EducationStats.Add(educationStats);
        dataContext.IndustryStats.Add(industryStats);
        dataContext.PersonalStats.Add(personalStats);
        dataContext.HobbyStats.Add(hobbyStats);
        dataContext.ExpenseStats.Add(expenseStats);

        await dataContext.SaveChangesAsync();
    }

    private static void UpdateUserStats(UserAccountDto user, UserStats userStats)
    {
        userStats.TotalUsers += 1;

        if (user.Gender.Equals("Male"))
            userStats.Males++;
        else
            userStats.Females++;

        if (user.Birthdate.AddYears(50) > DateTime.UtcNow)
            userStats.OlderAdults++;
        else if (user.Birthdate.AddYears(30) > DateTime.UtcNow)
            userStats.Adults++;
        else
            userStats.Youngers++;
    }
    private static void UpdateWorkingHoursStats(UserAccountDto user, WorkingHoursStats workingHours)
    {
        switch (user.WorkingHours)
        {
            case "FullTime_Employment":
                workingHours.FullTimeEmployment++;
                break;
            case "PartTime_Employment":
                workingHours.PartTimeEmployment++;
                break;
            case "Overtime_Hours":
                workingHours.OvertimeHours++;
                break;
            case "Shift_Work":
                workingHours.ShiftWork++;
                break;
            case "Flexible_Hours":
                workingHours.FlexibleHours++;
                break;
            case "Seasonal_Work":
                workingHours.SeasonalWork++;
                break;
            case "Freelancing_Work":
                workingHours.FreelancingWork++;
                break;
            case "Contract_Work":
                workingHours.ContractWork++;
                break;
            case "Remote_Work":
                workingHours.RemoteWork++;
                break;
            case "Night_Shifts":
                workingHours.NightShifts++;
                break;
            case "Weekend_Work":
                workingHours.WeekendWork++;
                break;
            case "Unemployed":
                workingHours.Unemployed++;
                break;
        }

    }
    private static void UpdatePersonalStatsStats(UserAccountDto user, PersonalStats personalStats)
    {
        if (user.StayHome)
            personalStats.StayHome++;
        else
            personalStats.GoOut++;

        if (user.Married)
            personalStats.Married++;
        else
            personalStats.Single++;
    }
    private static void UpdateIndustryStats(UserAccountDto user, IndustryStats industryStats)
    {
        switch (user.Industry)
        {
            case "Information_Technology":
                industryStats.InformationTechnology++;
                break;
            case "Healthcare":
                industryStats.Healthcare++;
                break;
            case "Finance_and_Banking":
                industryStats.FinanceAndBanking++;
                break;
            case "Education":
                industryStats.Education++;
                break;
            case "Engineering":
                industryStats.Engineering++;
                break;
            case "Manufacturing_and_Production":
                industryStats.ManufacturingAndProduction++;
                break;
            case "Retail":
                industryStats.Retail++;
                break;
            case "Hospitality_and_Tourism":
                industryStats.HospitalityAndTourism++;
                break;
            case "Marketing_and_Advertising":
                industryStats.MarketingAndAdvertising++;
                break;
            case "Government_and_Public_Administration":
                industryStats.GovernmentAndPublicAdministration++;
                break;
            case "Legal":
                industryStats.Legal++;
                break;
            case "Agriculture":
                industryStats.Agriculture++;
                break;
            case "Media_and_Journalism":
                industryStats.MediaAndJournalism++;
                break;
            case "Art_and_Design":
                industryStats.ArtAndDesign++;
                break;
            case "Nonprofit_and_Social_Services":
                industryStats.NonprofitAndSocialServices++;
                break;
            case "Environmental_and_Sustainability":
                industryStats.EnvironmentalAndSustainability++;
                break;
            case "Construction_and_Skilled_Trades":
                industryStats.ConstructionAndSkilledTrades++;
                break;
            case "Automotive":
                industryStats.Automotive++;
                break;
            case "Transportation_and_Logistics":
                industryStats.TransportationAndLogistics++;
                break;
            case "Sports_and_Fitness":
                industryStats.SportsAndFitness++;
                break;
            case "Online_Businesses":
                industryStats.OnlineBusinesses++;
                break;
            case "Pharmaceuticals_and_Healthcare_Research":
                industryStats.PharmaceuticalsAndHealthcareResearch++;
                break;
            case "Entertainment_and_Arts":
                industryStats.EntertainmentAndArts++;
                break;
            case "Tech_Startups_and_Entrepreneurship":
                industryStats.TechStartupsAndEntrepreneurship++;
                break;
            case "Real_Estate_and_Property_Management":
                industryStats.RealEstateAndPropertyManagement++;
                break;
            case "Food_and_Beverage":
                industryStats.FoodAndBeverage++;
                break;
            case "Aviation_and_Aerospace":
                industryStats.AviationAndAerospace++;
                break;
            case "Renewable_Energy_and_Green_Technology":
                industryStats.RenewableEnergyAndGreenTechnology++;
                break;
            case "Fitness_and_Wellness":
                industryStats.FitnessAndWellness++;
                break;
            case "Emergency_Services_and_Public_Safety":
                industryStats.EmergencyServicesAndPublicSafety++;
                break;
            case "Student":
                industryStats.Student++;
                break;
            case "Retired":
                industryStats.Retired++;
                break;
        }
    }
    private static void UpdateIncomeStats(UserAccountDto user, IncomeStats incomeStats)
    {
        switch (user.Income)
        {
            case "Poverty_Line":
                incomeStats.PovertyLine++;
                break;
            case "Low_Income":
                incomeStats.LowIncome++;
                break;
            case "Lower_Middle_Income":
                incomeStats.LowerMiddleIncome++;
                break;
            case "Median_Income":
                incomeStats.MedianIncome++;
                break;
            case "Upper_Middle_Income":
                incomeStats.UpperMiddleIncome++;
                break;
            case "High_Income":
                incomeStats.HighIncome++;
                break;
            case "Very_High_Income":
                incomeStats.VeryHighIncome++;
                break;
        }
    }
    private static void UpdateHobbyStats(UserAccountDto user, HobbyStats hobbyStats)
    {
        foreach (var input in user.Hobbies)
        {
            switch (input)
            {
                case "Reading":
                    hobbyStats.Reading++;
                    break;
                case "Gardening":
                    hobbyStats.Gardening++;
                    break;
                case "Cooking_and_Baking":
                    hobbyStats.CookingAndBaking++;
                    break;
                case "Painting_and_Drawing":
                    hobbyStats.PaintingAndDrawing++;
                    break;
                case "Sports_and_Physical_Activities":
                    hobbyStats.SportsAndPhysicalActivities++;
                    break;
                case "Photography":
                    hobbyStats.Photography++;
                    break;
                case "Playing_Musical_Instruments":
                    hobbyStats.PlayingMusicalInstruments++;
                    break;
                case "Traveling":
                    hobbyStats.Traveling++;
                    break;
                case "Crafting":
                    hobbyStats.Crafting++;
                    break;
                case "Fishing":
                    hobbyStats.Fishing++;
                    break;
                case "Model_Building":
                    hobbyStats.ModelBuilding++;
                    break;
                case "Collecting":
                    hobbyStats.Collecting++;
                    break;
                case "Hiking_and_Camping":
                    hobbyStats.HikingAndCamping++;
                    break;
                case "DIY_Home_Improvement":
                    hobbyStats.DIYHomeImprovement++;
                    break;
                case "Bird_Watching":
                    hobbyStats.BirdWatching++;
                    break;
                case "Cycling":
                    hobbyStats.Cycling++;
                    break;
                case "Meditation_and_Mindfulness":
                    hobbyStats.MeditationAndMindfulness++;
                    break;
                case "Dancing":
                    hobbyStats.Dancing++;
                    break;
                case "Volunteering":
                    hobbyStats.Volunteering++;
                    break;
                case "Board_Games_and_Puzzles":
                    hobbyStats.BoardGamesAndPuzzles++;
                    break;
                case "Writing":
                    hobbyStats.Writing++;
                    break;
                case "Fitness_and_Exercise":
                    hobbyStats.FitnessAndExercise++;
                    break;
                case "Comics_and_Graphic_Novels":
                    hobbyStats.ComicsAndGraphicNovels++;
                    break;
                case "Sculpting":
                    hobbyStats.Sculpting++;
                    break;
                case "Astrology_and_Astronomy":
                    hobbyStats.AstrologyAndAstronomy++;
                    break;
                case "Sewing_and_Quilting":
                    hobbyStats.SewingAndQuilting++;
                    break;
                case "Archery":
                    hobbyStats.Archery++;
                    break;
                case "Genealogy":
                    hobbyStats.Genealogy++;
                    break;
                case "Metal_Detecting":
                    hobbyStats.MetalDetecting++;
                    break;
                case "Vintage_and_Classic_Cars":
                    hobbyStats.VintageAndClassicCars++;
                    break;
                case "Other":
                    hobbyStats.Other++;
                    break;
            }
        }
    }
    private static void UpdateExpenseStats(UserAccountDto user, ExpenseStats expenseStats)
    {
        foreach (var input in user.Expenses)
        {
            switch (input)
            {
                case "Housing_Expenses":
                    expenseStats.HousingExpenses++;
                    break;
                case "Utilities":
                    expenseStats.Utilities++;
                    break;
                case "Transportation_Costs":
                    expenseStats.TransportationCosts++;
                    break;
                case "Food_and_Groceries":
                    expenseStats.FoodAndGroceries++;
                    break;
                case "Healthcare_Expenses":
                    expenseStats.HealthcareExpenses++;
                    break;
                case "Education_Expenses":
                    expenseStats.EducationExpenses++;
                    break;
                case "Debt_Payments":
                    expenseStats.DebtPayments++;
                    break;
                case "Insurance_Premiums":
                    expenseStats.InsurancePremiums++;
                    break;
                case "Entertainment_and_Leisure":
                    expenseStats.EntertainmentAndLeisure++;
                    break;
                case "Personal_Care_and_Grooming":
                    expenseStats.PersonalCareAndGrooming++;
                    break;
                case "Clothing_and_Apparel":
                    expenseStats.ClothingAndApparel++;
                    break;
                case "Savings_and_Investments":
                    expenseStats.SavingsAndInvestments++;
                    break;
                case "Taxes":
                    expenseStats.Taxes++;
                    break;
                case "Childcare_and_Education":
                    expenseStats.ChildcareAndEducation++;
                    break;
                case "Home_and_Garden":
                    expenseStats.HomeAndGarden++;
                    break;
                case "Subscriptions_and_Memberships":
                    expenseStats.SubscriptionsAndMemberships++;
                    break;
                case "Charitable_Donations":
                    expenseStats.CharitableDonations++;
                    break;
                case "Travel_and_Vacation":
                    expenseStats.TravelAndVacation++;
                    break;
                case "Legal_and_Financial_Services":
                    expenseStats.LegalAndFinancialServices++;
                    break;
                case "Emergency_Fund_and_Contingencies":
                    expenseStats.EmergencyFundAndContingencies++;
                    break;
                case "Other":
                    expenseStats.Other++;
                    break;
            }
        }
    }
    private static void UpdateEducationStats(UserAccountDto user, EducationStats educationStats)
    {
        switch (user.LevelOfEducation)
        {
            case "Primary":
                educationStats.Primary++;
                break;
            case "Secondary":
                educationStats.Secondary++;
                break;
            case "Higher":
                educationStats.Higher++;
                break;
            case "Technical":
                educationStats.Technical++;
                break;
        }
    }

}