using Famtela.Application.Features.TypesofFarming.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Validators.Features.TypesofFarming.Commands.AddEdit
{
    public class AddEditTypeofFarmingCommandValidator : AbstractValidator<AddEditTypeofFarmingCommand>
    {
        public AddEditTypeofFarmingCommandValidator(IStringLocalizer<AddEditTypeofFarmingCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
        }
    }
}