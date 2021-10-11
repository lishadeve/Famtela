using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Statuses.Commands.Delete
{
    public class DeleteStatusCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteStatusCommandHandler : IRequestHandler<DeleteStatusCommand, Result<int>>
    {
        private readonly ICowRepository _cowRepository;
        private readonly IStringLocalizer<DeleteStatusCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteStatusCommandHandler(IUnitOfWork<int> unitOfWork, ICowRepository cowRepository, IStringLocalizer<DeleteStatusCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _cowRepository = cowRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteStatusCommand command, CancellationToken cancellationToken)
        {
            var isStatusUsed = await _cowRepository.IsStatusUsed(command.Id);
            if (!isStatusUsed)
            {
                var status = await _unitOfWork.Repository<Status>().GetByIdAsync(command.Id);
                if (status != null)
                {
                    await _unitOfWork.Repository<Status>().DeleteAsync(status);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStatusesCacheKey);
                    return await Result<int>.SuccessAsync(status.Id, _localizer["Status Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Status Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}