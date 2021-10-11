using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Features.FarmProfiles.Commands.Delete
{
    public class DeleteFarmProfileCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteFarmProfileCommandHandler : IRequestHandler<DeleteFarmProfileCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteFarmProfileCommandHandler> _localizer;

        public DeleteFarmProfileCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteFarmProfileCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteFarmProfileCommand command, CancellationToken cancellationToken)
        {
            var farmprofile = await _unitOfWork.Repository<FarmProfile>().GetByIdAsync(command.Id);
            if (farmprofile != null)
            {
                await _unitOfWork.Repository<FarmProfile>().DeleteAsync(farmprofile);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(farmprofile.Id, _localizer["Farm Profile Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Farm Profile Not Found!"]);
            }
        }
    }
}