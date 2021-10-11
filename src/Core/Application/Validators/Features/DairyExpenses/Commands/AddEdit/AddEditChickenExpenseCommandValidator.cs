using Famtela.Application.Features.DairyExpenses.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.DairyExpenses.Commands.AddEdit
{
    public class AddEditDairyExpenseCommandValidator : AbstractValidator<AddEditDairyExpenseCommand>
    {
        public AddEditDairyExpenseCommandValidator(IStringLocalizer<AddEditDairyExpenseCommandValidator> localizer)
        {
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
            RuleFor(request => request.Quantity)
                .GreaterThan(0).WithMessage(x => localizer["Quantity is required!"]);
            RuleFor(request => request.UnitCost)
                .GreaterThan(0).WithMessage(x => localizer["Unit cost is required!"]);
        }
    }
}