using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Consumptions.Commands.Delete
{
    public class DeleteConsumptionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteConsumptionCommandHandler : IRequestHandler<DeleteConsumptionCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteConsumptionCommandHandler> _localizer;

        public DeleteConsumptionCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteConsumptionCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteConsumptionCommand command, CancellationToken cancellationToken)
        {
            var consumption = await _unitOfWork.Repository<Consumption>().GetByIdAsync(command.Id);
            if (consumption != null)
            {
                await _unitOfWork.Repository<Consumption>().DeleteAsync(consumption);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(consumption.Id, _localizer["Consumption Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Consumption Record Not Found!"]);
            }
        }
    }
}