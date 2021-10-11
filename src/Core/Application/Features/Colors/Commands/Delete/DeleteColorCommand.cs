using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Famtela.Shared.Constants.Application;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Colors.Commands.Delete
{
    public class DeleteColorCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteColorCommandHandler : IRequestHandler<DeleteColorCommand, Result<int>>
    {
        private readonly ICowRepository _cowRepository;
        private readonly IStringLocalizer<DeleteColorCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteColorCommandHandler(IUnitOfWork<int> unitOfWork, ICowRepository cowRepository, IStringLocalizer<DeleteColorCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _cowRepository = cowRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteColorCommand command, CancellationToken cancellationToken)
        {
            var isColorUsed = await _cowRepository.IsColorUsed(command.Id);
            if (!isColorUsed)
            {
                var color = await _unitOfWork.Repository<Color>().GetByIdAsync(command.Id);
                if (color != null)
                {
                    await _unitOfWork.Repository<Color>().DeleteAsync(color);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllColorsCacheKey);
                    return await Result<int>.SuccessAsync(color.Id, _localizer["Color Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Color Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}