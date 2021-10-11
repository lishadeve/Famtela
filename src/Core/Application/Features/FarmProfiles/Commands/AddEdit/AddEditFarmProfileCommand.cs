using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Famtela.Application.Features.FarmProfiles.Commands.AddEdit
{
    public partial class AddEditFarmProfileCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string FarmName { get; set; }
        [Required]
        public int CountyId { get; set; }
        [Required]
        public int TypeofFarmingId { get; set; }
    }

    internal class AddEditFarmProfileCommandHandler : IRequestHandler<AddEditFarmProfileCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditFarmProfileCommandHandler> _localizer;

        public AddEditFarmProfileCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditFarmProfileCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditFarmProfileCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<FarmProfile>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.FarmName == command.FarmName, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Farm Name already exists."]);
            }

            if (command.Id == 0)
            {
                var farmprofile = _mapper.Map<FarmProfile>(command);
                await _unitOfWork.Repository<FarmProfile>().AddAsync(farmprofile);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(farmprofile.Id, _localizer["Farm Profile Saved"]);
            }
            else
            {
                var farmprofile = await _unitOfWork.Repository<FarmProfile>().GetByIdAsync(command.Id);
                if (farmprofile != null)
                {
                    farmprofile.FarmName = command.FarmName ?? farmprofile.FarmName;
                    farmprofile.CountyId = (command.CountyId == 0) ? farmprofile.CountyId : command.CountyId;
                    farmprofile.TypeofFarmingId = (command.TypeofFarmingId == 0) ? farmprofile.TypeofFarmingId : command.TypeofFarmingId;
                    await _unitOfWork.Repository<FarmProfile>().UpdateAsync(farmprofile);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(farmprofile.Id, _localizer["Farm Profile Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Farm Profile Not Found!"]);
                }
            }
        }
    }
}