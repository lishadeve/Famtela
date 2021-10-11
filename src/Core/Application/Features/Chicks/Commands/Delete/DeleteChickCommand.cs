using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Chicks.Commands.Delete
{
    public class DeleteChickCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteChickCommandHandler : IRequestHandler<DeleteChickCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteChickCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteChickCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteChickCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteChickCommand command, CancellationToken cancellationToken)
        {
            var chick = await _unitOfWork.Repository<Chick>().GetByIdAsync(command.Id);
            if (chick != null)
            {
                await _unitOfWork.Repository<Chick>().DeleteAsync(chick);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllChicksCacheKey);
                return await Result<int>.SuccessAsync(chick.Id, _localizer["Chicks Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Chicks Record Not Found!"]);
            }
        }
    }
}