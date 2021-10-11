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

namespace Famtela.Application.Features.Chicks.Commands.AddEdit
{
    public partial class AddEditChickCommand : IRequest<Result<int>>
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

    internal class AddEditChickCommandHandler : IRequestHandler<AddEditChickCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditChickCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditChickCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditChickCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditChickCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var chick = _mapper.Map<Chick>(command);
                await _unitOfWork.Repository<Chick>().AddAsync(chick);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllChicksCacheKey);
                return await Result<int>.SuccessAsync(chick.Id, _localizer["Chicks Record Saved"]);
            }
            else
            {
                var chick = await _unitOfWork.Repository<Chick>().GetByIdAsync(command.Id);
                if (chick != null)
                {
                    chick.Disease = command.Disease ?? chick.Disease;
                    chick.Medication = command.Medication ?? chick.Medication;
                    chick.Vaccination = command.Vaccination ?? chick.Vaccination;
                    chick.TypeofFeed = command.TypeofFeed ?? chick.TypeofFeed;
                    chick.Remarks = command.Remarks ?? chick.Remarks;
                    chick.NumberofBirds = (command.NumberofBirds == 0) ? chick.NumberofBirds : command.NumberofBirds;
                    chick.Mortality = (command.Mortality == 0) ? chick.Mortality : command.Mortality;
                    chick.Feed = (command.Feed == 0) ? chick.Feed : command.Feed;
                    await _unitOfWork.Repository<Chick>().UpdateAsync(chick);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllChicksCacheKey);
                    return await Result<int>.SuccessAsync(chick.Id, _localizer["Chicks Record Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Chicks Record Not Found!"]);
                }
            }
        }
    }
}