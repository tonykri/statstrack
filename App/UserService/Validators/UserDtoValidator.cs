using FluentValidation;
using UserService.Categories;
using UserService.Dto.Profile;

namespace UserService.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(user => user.Birthdate)
            .NotEmpty().WithMessage("Birthdate cannot be empty")
            .Must(ValidAge).WithMessage("You must be between 18 and 120 years old");
        RuleFor(user => user.PhoneNumber)
            .NotEmpty().WithMessage("Phone number cannot be empty")
            .Matches("^[0-9]{10}$").WithMessage("Invalid phone number format. It should contain 10 numeric digits");
        RuleFor(user => user.DialingCode)
            .NotEmpty().WithMessage("Dialing code cannot be empty")
            .Must(ValidDialingCode).WithMessage("Invalid dialing code format");
        RuleFor(user => user.Gender)
            .NotEmpty().WithMessage("Gender cannot be empty")
            .Must(gender => gender.Equals("Male") || gender.Equals("Female")).WithMessage("Gender must be Male or Female");
        RuleFor(user => user.Country)
            .NotEmpty().WithMessage("Country cannot be empty")
            .Must(ValidCountry).WithMessage("Invalid country");
    }

    private bool ValidAge(DateOnly birthDate)
    {
        int age = DateTime.Today.Year - birthDate.Year;
        return age >= 18 && age <= 120;
    }

    private bool ValidDialingCode(string arg)
    {
        bool validDialingCode = false;
        foreach (DialingCodes code in Enum.GetValues(typeof(DialingCodes)))
        {
            if ((int)code == int.Parse(arg))
            {
                validDialingCode = true;
                break;
            }   
        }
        return validDialingCode;
    }   

    private bool ValidCountry(string arg)
    {
        bool validCountry = false;
        foreach (Countries country in Enum.GetValues(typeof(Countries)))
        {
            if (country.ToString().Equals(arg))
            {
                validCountry = true;
                break;
            }   
        }
        return validCountry;
    }
}