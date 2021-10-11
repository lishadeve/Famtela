using Famtela.Application.Features.Layers.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Layers.Commands.AddEdit
{
    public class AddEditLayerCommandValidator : AbstractValidator<AddEditLayerCommand>
    {
        public AddEditLayerCommandValidator(IStringLocalizer<AddEditLayerCommandValidator> localizer)
        {
            RuleFor(request => request.TypeofFeed)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Type of feed is required!"]);
            RuleFor(request => request.NumberofBirds)
                .GreaterThan(0).WithMessage(x => localizer["Number of birds must be greater than 0!"]);
            RuleFor(request => request.Eggs)
                .GreaterThan(0).WithMessage(x => localizer["Number of eggs must be greater than 0!"]);
        }
    }
}