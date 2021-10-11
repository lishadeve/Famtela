using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Eggs.Commands.Delete
{
    public class DeleteEggCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteEggCommandHandler : IRequestHandler<DeleteEggCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteEggCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteEggCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteEggCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteEggCommand command, CancellationToken cancellationToken)
        {
            var egg = await _unitOfWork.Repository<Egg>().GetByIdAsync(command.Id);
            if (egg != null)
            {
                await _unitOfWork.Repository<Egg>().DeleteAsync(egg);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllEggsCacheKey);
                return await Result<int>.SuccessAsync(egg.Id, _localizer["Eggs Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Eggs Record Not Found!"]);
            }
        }
    }
}