using Famtela.Application.Features.Diseases.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Diseases.Commands.AddEdit
{
    public class AddEditDiseaseCommandValidator : AbstractValidator<AddEditDiseaseCommand>
    {
        public AddEditDiseaseCommandValidator(IStringLocalizer<AddEditDiseaseCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}