using Famtela.Application.Features.WeightEstimates.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.WeightEstimates.Commands.AddEdit
{
    public class AddEditWeightEstimateCommandValidator : AbstractValidator<AddEditWeightEstimateCommand>
    {
        public AddEditWeightEstimateCommandValidator(IStringLocalizer<AddEditWeightEstimateCommandValidator> localizer)
        {
            RuleFor(request => request.CM)
                .GreaterThan(0).WithMessage(x => localizer["Length must be greater than 0!"]);
            RuleFor(request => request.KG)
                .GreaterThan(0).WithMessage(x => localizer["Weight must be greater than 0!"]);
        }
    }
}