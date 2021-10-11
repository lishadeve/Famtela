using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Milks.Commands.Delete
{
    public class DeleteMilkCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteMilkCommandHandler : IRequestHandler<DeleteMilkCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteMilkCommandHandler> _localizer;

        public DeleteMilkCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteMilkCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteMilkCommand command, CancellationToken cancellationToken)
        {
            var milk = await _unitOfWork.Repository<Milk>().GetByIdAsync(command.Id);
            if (milk != null)
            {
                await _unitOfWork.Repository<Milk>().DeleteAsync(milk);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(milk.Id, _localizer["Milk Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Milk Record Not Found!"]);
            }
        }
    }
}