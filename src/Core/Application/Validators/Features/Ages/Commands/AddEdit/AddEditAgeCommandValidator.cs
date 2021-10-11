using Famtela.Application.Features.Ages.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Ages.Commands.AddEdit
{
    public class AddEditAgeCommandValidator : AbstractValidator<AddEditAgeCommand>
    {
        public AddEditAgeCommandValidator(IStringLocalizer<AddEditAgeCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}