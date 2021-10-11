using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.WeightEstimates.Commands.Delete
{
    public class DeleteWeightEstimateCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteWeightEstimateCommandHandler : IRequestHandler<DeleteWeightEstimateCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteWeightEstimateCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteWeightEstimateCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteWeightEstimateCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteWeightEstimateCommand command, CancellationToken cancellationToken)
        {
            var weightestimate = await _unitOfWork.Repository<WeightEstimate>().GetByIdAsync(command.Id);
            if (weightestimate != null)
            {
                await _unitOfWork.Repository<WeightEstimate>().DeleteAsync(weightestimate);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWeightEstimatesCacheKey);
                return await Result<int>.SuccessAsync(weightestimate.Id, _localizer["Weight Estimates Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Weight Estimates Not Found!"]);
            }
        }
    }
}