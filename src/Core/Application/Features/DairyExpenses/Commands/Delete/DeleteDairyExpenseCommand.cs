using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.DairyExpenses.Commands.Delete
{
    public class DeleteDairyExpenseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDairyExpenseCommandHandler : IRequestHandler<DeleteDairyExpenseCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteDairyExpenseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteDairyExpenseCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteDairyExpenseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDairyExpenseCommand command, CancellationToken cancellationToken)
        {
            var dairyexpense = await _unitOfWork.Repository<DairyExpense>().GetByIdAsync(command.Id);
            if (dairyexpense != null)
            {
                await _unitOfWork.Repository<DairyExpense>().DeleteAsync(dairyexpense);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDairyExpensesCacheKey);
                return await Result<int>.SuccessAsync(dairyexpense.Id, _localizer["Dairy Expenses Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Dairy Expenses Record Not Found!"]);
            }
        }
    }
}