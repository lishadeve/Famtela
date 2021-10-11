using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;

namespace Famtela.Application.Features.TypesofFarming.Commands.AddEdit
{
    public partial class AddEditTypeofFarmingCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditTypeofFarmingCommandHandler : IRequestHandler<AddEditTypeofFarmingCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditTypeofFarmingCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditTypeofFarmingCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditTypeofFarmingCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditTypeofFarmingCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var typeoffarming = _mapper.Map<TypeofFarming>(command);
                await _unitOfWork.Repository<TypeofFarming>().AddAsync(typeoffarming);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTypesofFarmingCacheKey);
                return await Result<int>.SuccessAsync(typeoffarming.Id, _localizer["Type of farming Saved"]);
            }
            else
            {
                var typeoffarming = await _unitOfWork.Repository<TypeofFarming>().GetByIdAsync(command.Id);
                if (typeoffarming != null)
                {
                    typeoffarming.Name = command.Name ?? typeoffarming.Name;
                    await _unitOfWork.Repository<TypeofFarming>().UpdateAsync(typeoffarming);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTypesofFarmingCacheKey);
                    return await Result<int>.SuccessAsync(typeoffarming.Id, _localizer["Type of farming Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Type of farming Not Found!"]);
                }
            }
        }
    }
}