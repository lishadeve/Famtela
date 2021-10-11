using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.TypesofFeed.Commands.Delete
{
    public class DeleteTypeofFeedCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteTypeofFeedCommandHandler : IRequestHandler<DeleteTypeofFeedCommand, Result<int>>
    {
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly IStringLocalizer<DeleteTypeofFeedCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteTypeofFeedCommandHandler(IUnitOfWork<int> unitOfWork, IConsumptionRepository consumptionRepository, IStringLocalizer<DeleteTypeofFeedCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _consumptionRepository = consumptionRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTypeofFeedCommand command, CancellationToken cancellationToken)
        {
            var isTypeofFeedUsed = await _consumptionRepository.IsTypeofFeedUsed(command.Id);
            if (!isTypeofFeedUsed)
            {
                var typeoffeed = await _unitOfWork.Repository<TypeofFeed>().GetByIdAsync(command.Id);
                if (typeoffeed != null)
                {
                    await _unitOfWork.Repository<TypeofFeed>().DeleteAsync(typeoffeed);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTypesofFeedCacheKey);
                    return await Result<int>.SuccessAsync(typeoffeed.Id, _localizer["Type of Feed Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Type of Feed Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}