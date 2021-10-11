using Famtela.Application.Features.Statuses.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Statuses.Commands.AddEdit
{
    public class AddEditStatusCommandValidator : AbstractValidator<AddEditStatusCommand>
    {
        public AddEditStatusCommandValidator(IStringLocalizer<AddEditStatusCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}