using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Layers.Commands.Delete
{
    public class DeleteLayerCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteLayerCommandHandler : IRequestHandler<DeleteLayerCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteLayerCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteLayerCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteLayerCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteLayerCommand command, CancellationToken cancellationToken)
        {
            var layer = await _unitOfWork.Repository<Layer>().GetByIdAsync(command.Id);
            if (layer != null)
            {
                await _unitOfWork.Repository<Layer>().DeleteAsync(layer);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllLayersCacheKey);
                return await Result<int>.SuccessAsync(layer.Id, _localizer["Layers Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Layers Record Not Found!"]);
            }
        }
    }
}