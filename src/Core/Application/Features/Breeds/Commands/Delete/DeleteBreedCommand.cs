using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Breeds.Commands.Delete
{
    public class DeleteBreedCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteBreedCommandHandler : IRequestHandler<DeleteBreedCommand, Result<int>>
    {
        private readonly ICowRepository _cowRepository;
        private readonly IStringLocalizer<DeleteBreedCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteBreedCommandHandler(IUnitOfWork<int> unitOfWork, ICowRepository cowRepository, IStringLocalizer<DeleteBreedCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _cowRepository = cowRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteBreedCommand command, CancellationToken cancellationToken)
        {
            var isBreedUsed = await _cowRepository.IsBreedUsed(command.Id);
            if (!isBreedUsed)
            {
                var breed = await _unitOfWork.Repository<Breed>().GetByIdAsync(command.Id);
                if (breed != null)
                {
                    await _unitOfWork.Repository<Breed>().DeleteAsync(breed);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBreedsCacheKey);
                    return await Result<int>.SuccessAsync(breed.Id, _localizer["Breed Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Breed Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}