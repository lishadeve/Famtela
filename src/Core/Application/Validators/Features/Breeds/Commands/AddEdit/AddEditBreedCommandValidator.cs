using Famtela.Application.Features.Breeds.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.Breeds.Commands.AddEdit
{
    public class AddEditBreedCommandValidator : AbstractValidator<AddEditBreedCommand>
    {
        public AddEditBreedCommandValidator(IStringLocalizer<AddEditBreedCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}