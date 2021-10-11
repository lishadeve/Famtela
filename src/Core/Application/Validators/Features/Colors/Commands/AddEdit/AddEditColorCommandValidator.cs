using Famtela.Application.Features.Colors.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Colors.Commands.AddEdit
{
    public class AddEditColorCommandValidator : AbstractValidator<AddEditColorCommand>
    {
        public AddEditColorCommandValidator(IStringLocalizer<AddEditColorCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}