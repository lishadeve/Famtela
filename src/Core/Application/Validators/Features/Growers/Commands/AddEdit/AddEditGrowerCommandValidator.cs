using Famtela.Application.Features.Growers.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Growers.Commands.AddEdit
{
    public class AddEditGrowerCommandValidator : AbstractValidator<AddEditGrowerCommand>
    {
        public AddEditGrowerCommandValidator(IStringLocalizer<AddEditGrowerCommandValidator> localizer)
        {
            RuleFor(request => request.NumberofBirds)
                .GreaterThan(0).WithMessage(x => localizer["Number of birds must be greater than 0"]);
            RuleFor(request => request.TypeofFeed)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Type of feed is required!"]);
        }
    }
}