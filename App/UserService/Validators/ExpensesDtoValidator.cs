using FluentValidation;
using UserService.Categories;
using UserService.Dto.Profile;

namespace UserService.Validators;

public class ExpensesDtoValidator : AbstractValidator<ExpensesDto>
{
    public ExpensesDtoValidator()
    {
        RuleFor(e => e.Expenses)
            .NotEmpty().WithMessage("You must have at least one expense")
            .Must(ValidNoOfExpenses).WithMessage("Expenses must be between 1 and 7")
            .Must(ValidExpenses).WithMessage("Invalid Expense");
    }

    private bool ValidNoOfExpenses(List<string> expenses)
    {
        if(expenses.Count < 1 || expenses.Count > 7)
            return false;
        return true;
    }

    private bool ValidExpenses(List<string> expenses)
    {
        bool validExpense = false;
        foreach(string givenExpense in expenses)
        {
            validExpense = false;
            foreach(Expenses expense in Enum.GetValues(typeof(Expenses)))
            {
                if(expense.ToString().Equals(givenExpense))
                {
                    validExpense = true;
                    break;
                }
            }
        }
        return validExpense;
    }
}