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

namespace Famtela.Application.Features.Counties.Commands.AddEdit
{
    public partial class AddEditCountyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditCountyCommandHandler : IRequestHandler<AddEditCountyCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditCountyCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditCountyCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCountyCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCountyCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var county = _mapper.Map<County>(command);
                await _unitOfWork.Repository<County>().AddAsync(county);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCountiesCacheKey);
                return await Result<int>.SuccessAsync(county.Id, _localizer["County Saved"]);
            }
            else
            {
                var county = await _unitOfWork.Repository<County>().GetByIdAsync(command.Id);
                if (county != null)
                {
                    county.Name = command.Name ?? county.Name;
                    await _unitOfWork.Repository<County>().UpdateAsync(county);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCountiesCacheKey);
                    return await Result<int>.SuccessAsync(county.Id, _localizer["County Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["County Not Found!"]);
                }
            }
        }
    }
}