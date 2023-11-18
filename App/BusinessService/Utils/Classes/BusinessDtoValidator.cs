using BusinessService.Categories;
using BusinessService.Dto;
using FluentValidation;

namespace BusinessService.Utils;

public class BusinessDtoValidator : AbstractValidator<BusinessDto>
{
    public BusinessDtoValidator()
    {
        RuleFor(b => b.Brand)
            .NotEmpty().WithMessage("Brand cannot be empty")
            .MinimumLength(2).WithMessage("Brand must be at least 2 characters")
            .MaximumLength(50).WithMessage("Brand must be maximum 50 characters");
        RuleFor(b => b.Description)
            .NotEmpty().WithMessage("Description cannot be empty")
            .MinimumLength(5).WithMessage("Description must be at least 5 characters")
            .MaximumLength(200).WithMessage("Description must be maximum 200 characters");
        RuleFor(b => b.Category)
            .NotEmpty().WithMessage("Category cannot be empty")
            .Must(ValidCategory).WithMessage("Invalid category");
        RuleFor(b => b.Address)
            .NotEmpty().WithMessage("Address cannot be empty")
            .MinimumLength(5).WithMessage("Address must be at least 5 characters")
            .MaximumLength(50).WithMessage("Address must be maximum 50 characters");
        RuleFor(b => b.Latitude)
            .NotEmpty().WithMessage("Latitude cannot be empty");
        RuleFor(b => b.Longitude)
            .NotEmpty().WithMessage("Longitude cannot be empty");
    }

    private bool ValidCategory(string arg)
    {
        bool validCategory = false;
        foreach (Businesses business in Enum.GetValues(typeof(Businesses)))
        {
            if (business.ToString().Equals(arg))
            {
                validCategory = true;
                break;
            }   
        }
        return validCategory;
    }
}