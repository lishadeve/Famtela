using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Famtela.Domain.Entities.Chicken;

namespace Famtela.Application.Features.Consumptions.Commands.AddEdit
{
    public partial class AddEditConsumptionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public decimal Grams { get; set; }
        [Required]
        public string Remarks { get; set; }
        [Required]
        public int AgeId { get; set; }
        [Required]
        public int TypeofFeedId { get; set; }
    }

    internal class AddEditConsumptionCommandHandler : IRequestHandler<AddEditConsumptionCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditConsumptionCommandHandler> _localizer;

        public AddEditConsumptionCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditConsumptionCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditConsumptionCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Consumption>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Remarks == command.Remarks, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Consumption remark already exists."]);
            }

            if (command.Id == 0)
            {
                var vaccination = _mapper.Map<Consumption>(command);
                await _unitOfWork.Repository<Consumption>().AddAsync(vaccination);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(vaccination.Id, _localizer["Consumption Record Saved"]);
            }
            else
            {
                var vaccination = await _unitOfWork.Repository<Consumption>().GetByIdAsync(command.Id);
                if (vaccination != null)
                {
                    vaccination.Remarks = command.Remarks ?? vaccination.Remarks;
                    vaccination.Grams = (command.Grams == 0) ? vaccination.Grams : command.Grams;
                    vaccination.AgeId = (command.AgeId == 0) ? vaccination.AgeId : command.AgeId;
                    vaccination.TypeofFeedId = (command.TypeofFeedId == 0) ? vaccination.TypeofFeedId : command.TypeofFeedId;
                    await _unitOfWork.Repository<Consumption>().UpdateAsync(vaccination);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(vaccination.Id, _localizer["Consumption Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Consumption Not Found!"]);
                }
            }
        }
    }
}