using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.ChickenExpenses.Commands.Delete
{
    public class DeleteChickenExpenseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteChickenExpenseCommandHandler : IRequestHandler<DeleteChickenExpenseCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteChickenExpenseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteChickenExpenseCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteChickenExpenseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteChickenExpenseCommand command, CancellationToken cancellationToken)
        {
            var chickenexpense = await _unitOfWork.Repository<ChickenExpense>().GetByIdAsync(command.Id);
            if (chickenexpense != null)
            {
                await _unitOfWork.Repository<ChickenExpense>().DeleteAsync(chickenexpense);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllChickenExpensesCacheKey);
                return await Result<int>.SuccessAsync(chickenexpense.Id, _localizer["Chicken Expenses Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Chicken Expenses Record Not Found!"]);
            }
        }
    }
}