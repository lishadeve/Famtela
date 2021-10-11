using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Diseases.Commands.AddEdit
{
    public partial class AddEditDiseaseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditDiseaseCommandHandler : IRequestHandler<AddEditDiseaseCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditDiseaseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditDiseaseCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditDiseaseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditDiseaseCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var disease = _mapper.Map<Disease>(command);
                await _unitOfWork.Repository<Disease>().AddAsync(disease);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDiseasesCacheKey);
                return await Result<int>.SuccessAsync(disease.Id, _localizer["Disease Saved"]);
            }
            else
            {
                var disease = await _unitOfWork.Repository<Disease>().GetByIdAsync(command.Id);
                if (disease != null)
                {
                    disease.Name = command.Name ?? disease.Name;
                    await _unitOfWork.Repository<Disease>().UpdateAsync(disease);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDiseasesCacheKey);
                    return await Result<int>.SuccessAsync(disease.Id, _localizer["Disease Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Disease Not Found!"]);
                }
            }
        }
    }
}