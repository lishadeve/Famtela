using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;

namespace Famtela.Application.Features.TypesofFarming.Commands.Delete
{
    public class DeleteTypeofFarmingCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteTypeofFarmingCommandHandler : IRequestHandler<DeleteTypeofFarmingCommand, Result<int>>
    {
        private readonly IFarmProfileRepository _farmprofileRepository;
        private readonly IStringLocalizer<DeleteTypeofFarmingCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteTypeofFarmingCommandHandler(IUnitOfWork<int> unitOfWork, IFarmProfileRepository farmprofileRepository, IStringLocalizer<DeleteTypeofFarmingCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _farmprofileRepository = farmprofileRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTypeofFarmingCommand command, CancellationToken cancellationToken)
        {
            var isTypeofFarmingUsed = await _farmprofileRepository.IsTypeofFarmingUsed(command.Id);
            if (!isTypeofFarmingUsed)
            {
                var typeoffarming = await _unitOfWork.Repository<TypeofFarming>().GetByIdAsync(command.Id);
                if (typeoffarming != null)
                {
                    await _unitOfWork.Repository<TypeofFarming>().DeleteAsync(typeoffarming);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTypesofFarmingCacheKey);
                    return await Result<int>.SuccessAsync(typeoffarming.Id, _localizer["Type of farming Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Type of farming Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}