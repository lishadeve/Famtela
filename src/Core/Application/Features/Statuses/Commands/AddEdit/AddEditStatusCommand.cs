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

namespace Famtela.Application.Features.Statuses.Commands.AddEdit
{
    public partial class AddEditStatusCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditStatusCommandHandler : IRequestHandler<AddEditStatusCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditStatusCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditStatusCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditStatusCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditStatusCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var status = _mapper.Map<Status>(command);
                await _unitOfWork.Repository<Status>().AddAsync(status);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStatusesCacheKey);
                return await Result<int>.SuccessAsync(status.Id, _localizer["Status Saved"]);
            }
            else
            {
                var status = await _unitOfWork.Repository<Status>().GetByIdAsync(command.Id);
                if (status != null)
                {
                    status.Name = command.Name ?? status.Name;
                    await _unitOfWork.Repository<Status>().UpdateAsync(status);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStatusesCacheKey);
                    return await Result<int>.SuccessAsync(status.Id, _localizer["Status Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Status Not Found!"]);
                }
            }
        }
    }
}