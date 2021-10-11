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

namespace Famtela.Application.Features.ChickenExpenses.Commands.AddEdit
{
    public partial class AddEditChickenExpenseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitCost { get; set; }
        public decimal Transport { get; set; }
    }

    internal class AddEditChickenExpenseCommandHandler : IRequestHandler<AddEditChickenExpenseCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditChickenExpenseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditChickenExpenseCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditChickenExpenseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditChickenExpenseCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var chickenexpense = _mapper.Map<ChickenExpense>(command);
                await _unitOfWork.Repository<ChickenExpense>().AddAsync(chickenexpense);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllChickenExpensesCacheKey);
                return await Result<int>.SuccessAsync(chickenexpense.Id, _localizer["Chicken Expenses Record Saved"]);
            }
            else
            {
                var chickenexpense = await _unitOfWork.Repository<ChickenExpense>().GetByIdAsync(command.Id);
                if (chickenexpense != null)
                {
                    chickenexpense.Description = command.Description ?? chickenexpense.Description;
                    chickenexpense.Quantity = (command.Quantity == 1) ? chickenexpense.Quantity : command.Quantity;
                    chickenexpense.UnitCost = (command.UnitCost == 0) ? chickenexpense.UnitCost : command.UnitCost;
                    chickenexpense.Transport = (command.Transport == 0) ? chickenexpense.Transport : command.Transport;
                    await _unitOfWork.Repository<ChickenExpense>().UpdateAsync(chickenexpense);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllChickenExpensesCacheKey);
                    return await Result<int>.SuccessAsync(chickenexpense.Id, _localizer["Chicken Expenses Record Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Chicken Expenses Record Not Found!"]);
                }
            }
        }
    }
}