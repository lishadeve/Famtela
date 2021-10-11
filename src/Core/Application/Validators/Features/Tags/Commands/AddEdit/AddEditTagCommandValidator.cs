using Famtela.Application.Features.Tags.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Tags.Commands.AddEdit
{
    public class AddEditTagCommandValidator : AbstractValidator<AddEditTagCommand>
    {
        public AddEditTagCommandValidator(IStringLocalizer<AddEditTagCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}