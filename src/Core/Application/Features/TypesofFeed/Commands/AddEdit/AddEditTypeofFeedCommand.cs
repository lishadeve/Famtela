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

namespace Famtela.Application.Features.TypesofFeed.Commands.AddEdit
{
    public partial class AddEditTypeofFeedCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditTypeofFeedCommandHandler : IRequestHandler<AddEditTypeofFeedCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditTypeofFeedCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditTypeofFeedCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditTypeofFeedCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditTypeofFeedCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var typeoffeed = _mapper.Map<TypeofFeed>(command);
                await _unitOfWork.Repository<TypeofFeed>().AddAsync(typeoffeed);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTypesofFeedCacheKey);
                return await Result<int>.SuccessAsync(typeoffeed.Id, _localizer["Type of Feed Saved"]);
            }
            else
            {
                var typeoffeed = await _unitOfWork.Repository<TypeofFeed>().GetByIdAsync(command.Id);
                if (typeoffeed != null)
                {
                    typeoffeed.Name = command.Name ?? typeoffeed.Name;
                    await _unitOfWork.Repository<TypeofFeed>().UpdateAsync(typeoffeed);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTypesofFeedCacheKey);
                    return await Result<int>.SuccessAsync(typeoffeed.Id, _localizer["Type of Feed Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Type of Feed Not Found!"]);
                }
            }
        }
    }
}