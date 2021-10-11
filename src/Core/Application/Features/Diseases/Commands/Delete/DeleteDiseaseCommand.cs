using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Diseases.Commands.Delete
{
    public class DeleteDiseaseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDiseaseCommandHandler : IRequestHandler<DeleteDiseaseCommand, Result<int>>
    {
        private readonly IVaccinationRepository _vaccinationRepository;
        private readonly IStringLocalizer<DeleteDiseaseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteDiseaseCommandHandler(IUnitOfWork<int> unitOfWork, IVaccinationRepository vaccinationRepository, IStringLocalizer<DeleteDiseaseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _vaccinationRepository = vaccinationRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDiseaseCommand command, CancellationToken cancellationToken)
        {
            var isDiseaseUsed = await _vaccinationRepository.IsDiseaseUsed(command.Id);
            if (!isDiseaseUsed)
            {
                var disease = await _unitOfWork.Repository<Disease>().GetByIdAsync(command.Id);
                if (disease != null)
                {
                    await _unitOfWork.Repository<Disease>().DeleteAsync(disease);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDiseasesCacheKey);
                    return await Result<int>.SuccessAsync(disease.Id, _localizer["Disease Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Disease Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}