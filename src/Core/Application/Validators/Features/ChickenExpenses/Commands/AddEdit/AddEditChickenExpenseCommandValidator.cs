using Famtela.Application.Features.ChickenExpenses.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.ChickenExpenses.Commands.AddEdit
{
    public class AddEditChickenExpenseCommandValidator : AbstractValidator<AddEditChickenExpenseCommand>
    {
        public AddEditChickenExpenseCommandValidator(IStringLocalizer<AddEditChickenExpenseCommandValidator> localizer)
        {
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
            RuleFor(request => request.Quantity)
                .GreaterThan(0).WithMessage(x => localizer["Quantity must be greater than 0!"]);
            RuleFor(request => request.UnitCost)
                .GreaterThan(0).WithMessage(x => localizer["Unit cost must be greater than 0!"]);
        }
    }
}