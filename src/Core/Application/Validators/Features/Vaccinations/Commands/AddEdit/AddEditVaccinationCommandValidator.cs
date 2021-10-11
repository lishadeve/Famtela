using Famtela.Application.Features.Vaccinations.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Vaccinations.Commands.AddEdit
{
    public class AddEditVaccinationCommandValidator : AbstractValidator<AddEditVaccinationCommand>
    {
        public AddEditVaccinationCommandValidator(IStringLocalizer<AddEditVaccinationCommandValidator> localizer)
        {
            RuleFor(request => request.Administration)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Means of administration is required!"]);
            RuleFor(request => request.Remarks)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Remark is required!"]);
            RuleFor(request => request.AgeId)
                .GreaterThan(0).WithMessage(x => localizer["Age is required!"]);
            RuleFor(request => request.DiseaseId)
                .GreaterThan(0).WithMessage(x => localizer["Disease is required!"]);
        }
    }
}