using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Ages.Commands.AddEdit
{
    public partial class AddEditAgeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditAgeCommandHandler : IRequestHandler<AddEditAgeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditAgeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditAgeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditAgeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditAgeCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var age = _mapper.Map<Age>(command);
                await _unitOfWork.Repository<Age>().AddAsync(age);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllAgesCacheKey);
                return await Result<int>.SuccessAsync(age.Id, _localizer["Age Saved"]);
            }
            else
            {
                var age = await _unitOfWork.Repository<Age>().GetByIdAsync(command.Id);
                if (age != null)
                {
                    age.Name = command.Name ?? age.Name;
                    await _unitOfWork.Repository<Age>().UpdateAsync(age);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllAgesCacheKey);
                    return await Result<int>.SuccessAsync(age.Id, _localizer["Age Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Age Not Found!"]);
                }
            }
        }
    }
}