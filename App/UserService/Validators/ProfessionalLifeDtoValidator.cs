using FluentValidation;
using UserService.Categories;
using UserService.Dto.Profile;

namespace UserService.Validators;

public class ProfessionalLifeDtoValidator : AbstractValidator<ProfessionalLifeDto>
{
    public ProfessionalLifeDtoValidator()
    {
        RuleFor(p => p.LevelOfEducation)
            .NotEmpty().WithMessage("The level of education cannot be empty")
            .Must(ValidLevelOfEducation).WithMessage("Invalid level of education");
        RuleFor(p => p.Industry)
            .NotEmpty().WithMessage("Industry cannot be empty")
            .Must(ValidIndustry).WithMessage("Invalid industry");
        RuleFor(p => p.Income)
            .NotEmpty().WithMessage("Income cannot be empty")
            .Must(ValidIncome).WithMessage("Invalid income");
        RuleFor(p => p.WorkingHours)
            .NotEmpty().WithMessage("Working hours cannot be empty")
            .Must(ValidWorkingHours).WithMessage("Invalid working hours");
    }

    private bool ValidLevelOfEducation(string givenLevelOfEducation)
    {
        bool validEducationLevel = false;
        foreach (LevelsOfEducation level in Enum.GetValues(typeof(LevelsOfEducation)))
        {
            if (level.ToString().Equals(givenLevelOfEducation))
            {
                validEducationLevel = true;
                break;
            }   
        }
        return validEducationLevel;
    }

    private bool ValidIndustry(string givenIndustry)
    {
        bool validIndustry = false;
        foreach (Industries industry in Enum.GetValues(typeof(Industries)))
        {
            if (industry.ToString().Equals(givenIndustry))
            {
                validIndustry = true;
                break;
            }   
        }
        return validIndustry;
    }

    private bool ValidIncome(string givenIncome)
    {
        bool validIncome = false;
        foreach (Incomes income in Enum.GetValues(typeof(Incomes)))
        {
            if (income.ToString().Equals(givenIncome))
            {
                validIncome = true;
                break;
            }   
        }
        return validIncome;
    }

    private bool ValidWorkingHours(string givenWorkingHours)
    {
        bool validWorkingHour = false;
        foreach (WorkingHours workingHour in Enum.GetValues(typeof(WorkingHours)))
        {
            if (workingHour.ToString().Equals(givenWorkingHours))
            {
                validWorkingHour = true;
                break;
            }   
        }
        return validWorkingHour;
    }
}