using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Ages.Commands.Delete
{
    public class DeleteAgeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteAgeCommandHandler : IRequestHandler<DeleteAgeCommand, Result<int>>
    {
        private readonly IVaccinationRepository _vaccinationRepository;
        private readonly IStringLocalizer<DeleteAgeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteAgeCommandHandler(IUnitOfWork<int> unitOfWork, IVaccinationRepository vaccinationRepository, IStringLocalizer<DeleteAgeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _vaccinationRepository = vaccinationRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteAgeCommand command, CancellationToken cancellationToken)
        {
            var isAgeUsed = await _vaccinationRepository.IsAgeUsed(command.Id);
            if (!isAgeUsed)
            {
                var age = await _unitOfWork.Repository<Age>().GetByIdAsync(command.Id);
                if (age != null)
                {
                    await _unitOfWork.Repository<Age>().DeleteAsync(age);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllAgesCacheKey);
                    return await Result<int>.SuccessAsync(age.Id, _localizer["Age Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Age Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}