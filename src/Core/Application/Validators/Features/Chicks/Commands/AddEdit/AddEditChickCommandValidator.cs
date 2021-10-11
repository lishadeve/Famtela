using Famtela.Application.Features.Chicks.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Chicks.Commands.AddEdit
{
    public class AddEditChickCommandValidator : AbstractValidator<AddEditChickCommand>
    {
        public AddEditChickCommandValidator(IStringLocalizer<AddEditChickCommandValidator> localizer)
        {
            RuleFor(request => request.NumberofBirds)
                .GreaterThan(0).WithMessage(x => localizer["Number of birds must be greater than 0!"]);
            RuleFor(request => request.TypeofFeed)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Type of feed is required!"]);
        }
    }
}