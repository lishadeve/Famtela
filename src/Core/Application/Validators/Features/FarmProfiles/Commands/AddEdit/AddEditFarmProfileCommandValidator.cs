using Famtela.Application.Features.FarmProfiles.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.FarmProfiles.Commands.AddEdit
{
    public class AddEditFarmProfileCommandValidator : AbstractValidator<AddEditFarmProfileCommand>
    {
        public AddEditFarmProfileCommandValidator(IStringLocalizer<AddEditFarmProfileCommandValidator> localizer)
        {
            RuleFor(request => request.FarmName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Farm name is required!"]);
            RuleFor(request => request.CountyId)
                .GreaterThan(0).WithMessage(x => localizer["County is required!"]);
            RuleFor(request => request.TypeofFarmingId)
                .GreaterThan(0).WithMessage(x => localizer["Type of farming is required!"]);
        }
    }
}