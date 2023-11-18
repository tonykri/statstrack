using FluentValidation;
using UserService.Categories;
using UserService.Dto.Profile;

namespace UserService.Validators;

public class HobbiesDtoValidator : AbstractValidator<HobbiesDto>
{
    public HobbiesDtoValidator()
    {
        RuleFor(h => h.Hobbies)
            .NotEmpty().WithMessage("You must have at least one hobby")
            .Must(ValidNoOfHobbies).WithMessage("Hobbies must be between 1 and 7")
            .Must(ValidHobbies).WithMessage("Invalid hobby");
    }

    private bool ValidNoOfHobbies(List<string> hobbies)
    {
        if(hobbies.Count < 1 || hobbies.Count > 7)
            return false;
        return true;
    }

    private bool ValidHobbies(List<string> hobbies)
    {
        bool validHobby = false;
        foreach(string givenHobby in hobbies)
        {
            validHobby = false;
            foreach(Hobbies hobby in Enum.GetValues(typeof(Hobbies)))
            {
                if(hobby.ToString().Equals(givenHobby))
                {
                    validHobby = true;
                    break;
                }
            }
        }
        return validHobby;
    }
}