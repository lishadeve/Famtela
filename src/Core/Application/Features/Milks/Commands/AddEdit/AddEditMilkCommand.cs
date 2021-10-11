using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Milks.Commands.AddEdit
{
    public partial class AddEditMilkCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public decimal Morning { get; set; }
        public decimal Evening { get; set; }
        public string Remarks { get; set; }
        public int CowId { get; set; }
    }

    internal class AddEditMilkCommandHandler : IRequestHandler<AddEditMilkCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditMilkCommandHandler> _localizer;

        public AddEditMilkCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditMilkCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditMilkCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Milk>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Remarks == command.Remarks, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Milk remark already exists."]);
            }

            if (command.Id == 0)
            {
                var milk = _mapper.Map<Milk>(command);
                await _unitOfWork.Repository<Milk>().AddAsync(milk);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(milk.Id, _localizer["Milk Record Saved"]);
            }
            else
            {
                var milk = await _unitOfWork.Repository<Milk>().GetByIdAsync(command.Id);
                if (milk != null)
                {
                    milk.Remarks = command.Remarks ?? milk.Remarks;
                    milk.Morning = (command.Morning == 0) ? milk.Morning : command.Morning;
                    milk.Evening = (command.Evening == 0) ? milk.Evening : command.Evening;
                    milk.CowId = (command.CowId == 0) ? milk.CowId : command.CowId;
                    await _unitOfWork.Repository<Milk>().UpdateAsync(milk);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(milk.Id, _localizer["Milk Record Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Milk Record Not Found!"]);
                }
            }
        }
    }
}