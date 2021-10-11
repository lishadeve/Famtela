using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Vaccinations.Commands.Delete
{
    public class DeleteVaccinationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteVaccinationCommandHandler : IRequestHandler<DeleteVaccinationCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteVaccinationCommandHandler> _localizer;

        public DeleteVaccinationCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteVaccinationCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteVaccinationCommand command, CancellationToken cancellationToken)
        {
            var vaccination = await _unitOfWork.Repository<Vaccination>().GetByIdAsync(command.Id);
            if (vaccination != null)
            {
                await _unitOfWork.Repository<Vaccination>().DeleteAsync(vaccination);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(vaccination.Id, _localizer["Vaccination Record Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Vaccination Record Not Found!"]);
            }
        }
    }
}