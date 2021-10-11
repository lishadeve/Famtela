using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Tags.Commands.AddEdit
{
    public partial class AddEditTagCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditTagCommandHandler : IRequestHandler<AddEditTagCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditTagCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditTagCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditTagCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditTagCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var tag = _mapper.Map<Tag>(command);
                await _unitOfWork.Repository<Tag>().AddAsync(tag);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTagsCacheKey);
                return await Result<int>.SuccessAsync(tag.Id, _localizer["Tag Saved"]);
            }
            else
            {
                var tag = await _unitOfWork.Repository<Tag>().GetByIdAsync(command.Id);
                if (tag != null)
                {
                    tag.Name = command.Name ?? tag.Name;
                    await _unitOfWork.Repository<Tag>().UpdateAsync(tag);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTagsCacheKey);
                    return await Result<int>.SuccessAsync(tag.Id, _localizer["Tag Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Tag Not Found!"]);
                }
            }
        }
    }
}