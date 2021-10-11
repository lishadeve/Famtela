using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Cows.Commands.Delete
{
    public class DeleteCowCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCowCommandHandler : IRequestHandler<DeleteCowCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCowCommandHandler> _localizer;

        public DeleteCowCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCowCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCowCommand command, CancellationToken cancellationToken)
        {
            var cow = await _unitOfWork.Repository<Cow>().GetByIdAsync(command.Id);
            if (cow != null)
            {
                await _unitOfWork.Repository<Cow>().DeleteAsync(cow);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(cow.Id, _localizer["Cow Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Cow Record Not Found!"]);
            }
        }
    }
}