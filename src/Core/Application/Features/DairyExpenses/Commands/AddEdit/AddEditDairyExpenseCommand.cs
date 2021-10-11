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

namespace Famtela.Application.Features.DairyExpenses.Commands.AddEdit
{
    public partial class AddEditDairyExpenseCommand : IRequest<Result<int>>
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

    internal class AddEditDairyExpenseCommandHandler : IRequestHandler<AddEditDairyExpenseCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditDairyExpenseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditDairyExpenseCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditDairyExpenseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditDairyExpenseCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var dairyexpense = _mapper.Map<DairyExpense>(command);
                await _unitOfWork.Repository<DairyExpense>().AddAsync(dairyexpense);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDairyExpensesCacheKey);
                return await Result<int>.SuccessAsync(dairyexpense.Id, _localizer["Dairy Expenses Record Saved"]);
            }
            else
            {
                var dairyexpense = await _unitOfWork.Repository<DairyExpense>().GetByIdAsync(command.Id);
                if (dairyexpense != null)
                {
                    dairyexpense.Description = command.Description ?? dairyexpense.Description;
                    dairyexpense.Quantity = (command.Quantity == 1) ? dairyexpense.Quantity : command.Quantity;
                    dairyexpense.UnitCost = (command.UnitCost == 0) ? dairyexpense.UnitCost : command.UnitCost;
                    dairyexpense.Transport = (command.Transport == 0) ? dairyexpense.Transport : command.Transport;
                    await _unitOfWork.Repository<DairyExpense>().UpdateAsync(dairyexpense);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDairyExpensesCacheKey);
                    return await Result<int>.SuccessAsync(dairyexpense.Id, _localizer["Dairy Expenses Record Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Dairy Expenses Record Not Found!"]);
                }
            }
        }
    }
}