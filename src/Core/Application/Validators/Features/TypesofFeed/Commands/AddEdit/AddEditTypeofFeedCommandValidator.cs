using Famtela.Application.Features.TypesofFeed.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.TypesofFeed.Commands.AddEdit
{
    public class AddEditTypeofFeedCommandValidator : AbstractValidator<AddEditTypeofFeedCommand>
    {
        public AddEditTypeofFeedCommandValidator(IStringLocalizer<AddEditTypeofFeedCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}