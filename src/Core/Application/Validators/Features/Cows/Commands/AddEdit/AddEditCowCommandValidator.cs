using Famtela.Application.Features.Cows.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Cows.Commands.AddEdit
{
    public class AddEditCowCommandValidator : AbstractValidator<AddEditCowCommand>
    {
        public AddEditCowCommandValidator(IStringLocalizer<AddEditCowCommandValidator> localizer)
        {
            RuleFor(request => request.EarTagNumber)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Cow name is required!"]);
            RuleFor(request => request.BreedId)
                .GreaterThan(0).WithMessage(x => localizer["Breed is required!"]);
            RuleFor(request => request.ColorId)
                .GreaterThan(0).WithMessage(x => localizer["Color is required!"]);
            RuleFor(request => request.StatusId)
                .GreaterThan(0).WithMessage(x => localizer["Status is required!"]);
            RuleFor(request => request.TagId)
                .GreaterThan(0).WithMessage(x => localizer["Tag is required!"]);
        }
    }
}