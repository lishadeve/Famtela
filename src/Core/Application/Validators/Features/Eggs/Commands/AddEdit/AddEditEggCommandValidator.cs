using Famtela.Application.Features.Eggs.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Eggs.Commands.AddEdit
{
    public class AddEditEggCommandValidator : AbstractValidator<AddEditEggCommand>
    {
        public AddEditEggCommandValidator(IStringLocalizer<AddEditEggCommandValidator> localizer)
        {
            RuleFor(request => request.Sold)
                .GreaterThan(0).WithMessage(x => localizer["Number of eggs sold must be greater than 0!"]);
            RuleFor(request => request.UnitPrice)
                .GreaterThan(0).WithMessage(x => localizer["Unit price must be greater than 0!"]);
        }
    }
}