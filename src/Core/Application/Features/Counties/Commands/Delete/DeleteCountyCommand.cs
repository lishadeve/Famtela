using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;

namespace Famtela.Application.Features.Counties.Commands.Delete
{
    public class DeleteCountyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCountyCommandHandler : IRequestHandler<DeleteCountyCommand, Result<int>>
    {
        private readonly IFarmProfileRepository _farmprofileRepository;
        private readonly IStringLocalizer<DeleteCountyCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteCountyCommandHandler(IUnitOfWork<int> unitOfWork, IFarmProfileRepository farmprofileRepository, IStringLocalizer<DeleteCountyCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _farmprofileRepository = farmprofileRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCountyCommand command, CancellationToken cancellationToken)
        {
            var isCountyUsed = await _farmprofileRepository.IsCountyUsed(command.Id);
            if (!isCountyUsed)
            {
                var county = await _unitOfWork.Repository<County>().GetByIdAsync(command.Id);
                if (county != null)
                {
                    await _unitOfWork.Repository<County>().DeleteAsync(county);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCountiesCacheKey);
                    return await Result<int>.SuccessAsync(county.Id, _localizer["County Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["County Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}