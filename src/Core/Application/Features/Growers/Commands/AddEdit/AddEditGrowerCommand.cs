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

namespace Famtela.Application.Features.Growers.Commands.AddEdit
{
    public partial class AddEditGrowerCommand : IRequest<Result<int>>
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
        public string Remarks { get; set; }
    }

    internal class AddEditGrowerCommandHandler : IRequestHandler<AddEditGrowerCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditGrowerCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditGrowerCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditGrowerCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditGrowerCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var grower = _mapper.Map<Grower>(command);
                await _unitOfWork.Repository<Grower>().AddAsync(grower);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGrowersCacheKey);
                return await Result<int>.SuccessAsync(grower.Id, _localizer["Growers Record Saved"]);
            }
            else
            {
                var grower = await _unitOfWork.Repository<Grower>().GetByIdAsync(command.Id);
                if (grower != null)
                {
                    grower.Disease = command.Disease ?? grower.Disease;
                    grower.Medication = command.Medication ?? grower.Medication;
                    grower.Vaccination = command.Vaccination ?? grower.Vaccination;
                    grower.TypeofFeed = command.TypeofFeed ?? grower.TypeofFeed;
                    grower.Remarks = command.Remarks ?? grower.Remarks;
                    grower.NumberofBirds = (command.NumberofBirds == 0) ? grower.NumberofBirds : command.NumberofBirds;
                    grower.Mortality = (command.Mortality == 0) ? grower.Mortality : command.Mortality;
                    grower.Feed = (command.Feed == 0) ? grower.Feed : command.Feed;
                    await _unitOfWork.Repository<Grower>().UpdateAsync(grower);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGrowersCacheKey);
                    return await Result<int>.SuccessAsync(grower.Id, _localizer["Growers Record Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Growers Record Not Found!"]);
                }
            }
        }
    }
}