using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.WeightEstimates.Commands.AddEdit
{
    public partial class AddEditWeightEstimateCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public decimal CM { get; set; }
        public decimal KG { get; set; }
        public string Remarks { get; set; }
    }

    internal class AddEditWeightEstimateCommandHandler : IRequestHandler<AddEditWeightEstimateCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditWeightEstimateCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditWeightEstimateCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditWeightEstimateCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditWeightEstimateCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var weightestimate = _mapper.Map<WeightEstimate>(command);
                await _unitOfWork.Repository<WeightEstimate>().AddAsync(weightestimate);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWeightEstimatesCacheKey);
                return await Result<int>.SuccessAsync(weightestimate.Id, _localizer["Weight Estimate Saved"]);
            }
            else
            {
                var weightestimate = await _unitOfWork.Repository<WeightEstimate>().GetByIdAsync(command.Id);
                if (weightestimate != null)
                {
                    weightestimate.CM = (command.CM == 0) ? weightestimate.CM : command.CM;
                    weightestimate.KG = (command.KG == 0) ? weightestimate.KG : command.KG;
                    weightestimate.Remarks = command.Remarks ?? weightestimate.Remarks;
                    await _unitOfWork.Repository<WeightEstimate>().UpdateAsync(weightestimate);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllWeightEstimatesCacheKey);
                    return await Result<int>.SuccessAsync(weightestimate.Id, _localizer["Weight Estimate Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Weight Estimate Not Found!"]);
                }
            }
        }
    }
}