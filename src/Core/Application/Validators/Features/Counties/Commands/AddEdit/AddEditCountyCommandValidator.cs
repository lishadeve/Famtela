using Famtela.Application.Features.Counties.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Counties.Commands.AddEdit
{
    public class AddEditCountyCommandValidator : AbstractValidator<AddEditCountyCommand>
    {
        public AddEditCountyCommandValidator(IStringLocalizer<AddEditCountyCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}