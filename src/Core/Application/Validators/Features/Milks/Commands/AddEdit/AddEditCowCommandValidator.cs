using Famtela.Application.Features.Milks.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Milks.Commands.AddEdit
{
    public class AddEditCowCommandValidator : AbstractValidator<AddEditMilkCommand>
    {
        public AddEditCowCommandValidator(IStringLocalizer<AddEditCowCommandValidator> localizer)
        {
            RuleFor(request => request.Morning)
                .GreaterThan(0).WithMessage(x => localizer["Morning value must be greater than 0!"]);
            RuleFor(request => request.Evening)
                .GreaterThan(0).WithMessage(x => localizer["Evening value must be greater than 0!"]);
        }
    }
}