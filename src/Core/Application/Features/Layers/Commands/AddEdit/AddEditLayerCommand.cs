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

namespace Famtela.Application.Features.Layers.Commands.AddEdit
{
    public partial class AddEditLayerCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public int NumberofBirds { get; set; }
        public string Disease { get; set; }
        public int Mortality { get; set; }
        public string Vaccination { get; set; }
        public string Medication { get; set; }
        public decimal Feed { get; set; }
        [Required]
        public string TypeofFeed { get; set; }
        [Required]
        public int Eggs { get; set; }
        public string Remarks { get; set; }
    }

    internal class AddEditLayerCommandHandler : IRequestHandler<AddEditLayerCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditLayerCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditLayerCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditLayerCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditLayerCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var layer = _mapper.Map<Layer>(command);
                await _unitOfWork.Repository<Layer>().AddAsync(layer);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllLayersCacheKey);
                return await Result<int>.SuccessAsync(layer.Id, _localizer["Layers Record Saved"]);
            }
            else
            {
                var layer = await _unitOfWork.Repository<Layer>().GetByIdAsync(command.Id);
                if (layer != null)
                {
                    layer.Disease = command.Disease ?? layer.Disease;
                    layer.Medication = command.Medication ?? layer.Medication;
                    layer.Vaccination = command.Vaccination ?? layer.Vaccination;
                    layer.TypeofFeed = command.TypeofFeed ?? layer.TypeofFeed;
                    layer.Remarks = command.Remarks ?? layer.Remarks;
                    layer.NumberofBirds = (command.NumberofBirds == 0) ? layer.NumberofBirds : command.NumberofBirds;
                    layer.Mortality = (command.Mortality == 0) ? layer.Mortality : command.Mortality;
                    layer.Eggs = (command.Eggs == 0) ? layer.Eggs : command.Eggs;
                    layer.Feed = (command.Feed == 0) ? layer.Feed : command.Feed;
                    await _unitOfWork.Repository<Layer>().UpdateAsync(layer);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllLayersCacheKey);
                    return await Result<int>.SuccessAsync(layer.Id, _localizer["Layers Record Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Layers Record Not Found!"]);
                }
            }
        }
    }
}