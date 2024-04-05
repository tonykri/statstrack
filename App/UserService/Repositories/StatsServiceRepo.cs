using Microsoft.EntityFrameworkCore;
using UserService.Categories;
using UserService.Dto;
using UserService.Models;

namespace UserService.Repositories;

public class StatsServiceRepo : IStatsServiceRepo
{
    private readonly DataContext dataContext;
    public StatsServiceRepo(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<List<UserAccountDto>> GetBusinessStats(UserIdsDto userIds)
    {
        var profiles = new List<UserAccountDto>();
        foreach (var userId in userIds.UserIds)
        {
            var user = await dataContext.Users
                .Include(u => u.UserPersonalLife)
                .Include(u => u.UserProfessionalLife)
                .Include(u => u.Expenses)
                .Include(u => u.Hobbies)
                .FirstOrDefaultAsync(u => u.Id == userId && u.ProfileStage.Equals(ProfileStages.Completed));
            if (user is null)
                continue;

            var profile = new UserAccountDto();
            if (user.Birthdate is not null)
                profile.Birthdate = (DateTime)user.Birthdate;
            if (user.Gender is not null)
                profile.Gender = user.Gender;
            if (user.UserProfessionalLife is not null)
            {
                profile.LevelOfEducation = user.UserProfessionalLife.LevelOfEducation;
                profile.Industry = user.UserProfessionalLife.Industry;
                profile.Income = user.UserProfessionalLife.Income;
                profile.WorkingHours = user.UserProfessionalLife.WorkingHours;
            }
            if (user.UserPersonalLife is not null)
            {
                profile.Married = user.UserPersonalLife.Married;
                profile.StayHome = user.UserPersonalLife.StayHome;
            }
            foreach (var hobby in user.Hobbies)
            {
                profile.Hobbies.Add(hobby.UserHobby);
            }
            foreach (var expense in user.Expenses)
            {
                profile.Expenses.Add(expense.UserExpense);
            }
            profiles.Add(profile);
        }
        return profiles;
    }
}