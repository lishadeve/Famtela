using Famtela.Application.Features.Consumptions.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Consumptions.Commands.AddEdit
{
    public class AddEditConsumptionCommandValidator : AbstractValidator<AddEditConsumptionCommand>
    {
        public AddEditConsumptionCommandValidator(IStringLocalizer<AddEditConsumptionCommandValidator> localizer)
        {
            RuleFor(request => request.Remarks)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Remark is required!"]);
            RuleFor(request => request.AgeId)
                .GreaterThan(0).WithMessage(x => localizer["Age is required!"]);
            RuleFor(request => request.TypeofFeedId)
                .GreaterThan(0).WithMessage(x => localizer["Type of feed is required!"]);
            RuleFor(request => request.Grams)
                .GreaterThan(0).WithMessage(x => localizer["Grams must be greater than 0!"]);
        }
    }
}