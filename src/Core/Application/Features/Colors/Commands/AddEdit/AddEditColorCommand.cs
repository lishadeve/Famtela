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

namespace Famtela.Application.Features.Colors.Commands.AddEdit
{
    public partial class AddEditColorCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    internal class AddEditColorCommandHandler : IRequestHandler<AddEditColorCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditColorCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditColorCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditColorCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditColorCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var color = _mapper.Map<Color>(command);
                await _unitOfWork.Repository<Color>().AddAsync(color);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllColorsCacheKey);
                return await Result<int>.SuccessAsync(color.Id, _localizer["Color Saved"]);
            }
            else
            {
                var color = await _unitOfWork.Repository<Color>().GetByIdAsync(command.Id);
                if (color != null)
                {
                    color.Name = command.Name ?? color.Name;
                    await _unitOfWork.Repository<Color>().UpdateAsync(color);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllColorsCacheKey);
                    return await Result<int>.SuccessAsync(color.Id, _localizer["Color Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Color Not Found!"]);
                }
            }
        }
    }
}