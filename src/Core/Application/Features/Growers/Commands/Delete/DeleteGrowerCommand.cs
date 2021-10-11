using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Growers.Commands.Delete
{
    public class DeleteGrowerCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteGrowerCommandHandler : IRequestHandler<DeleteGrowerCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteGrowerCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteGrowerCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteGrowerCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteGrowerCommand command, CancellationToken cancellationToken)
        {
            var grower = await _unitOfWork.Repository<Grower>().GetByIdAsync(command.Id);
            if (grower != null)
            {
                await _unitOfWork.Repository<Grower>().DeleteAsync(grower);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGrowersCacheKey);
                return await Result<int>.SuccessAsync(grower.Id, _localizer["Growers Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Growers Record Not Found!"]);
            }
        }
    }
}