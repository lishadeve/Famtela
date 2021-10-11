using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Application.Requests;
using Famtela.Shared.Constants.Application;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Famtela.Domain.Entities.Chicken;
using Famtela.Application.Features.ChickenExpenses.Commands.AddEdit;

namespace Famtela.Application.Features.ChickenExpenses.Commands.Import
{
    public partial class ImportChickenExpensesCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportChickenExpensesCommandHandler : IRequestHandler<ImportChickenExpensesCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditChickenExpenseCommand> _addChickenExpenseValidator;
        private readonly IStringLocalizer<ImportChickenExpensesCommandHandler> _localizer;

        public ImportChickenExpensesCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditChickenExpenseCommand> addChickenExpenseValidator,
            IStringLocalizer<ImportChickenExpensesCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addChickenExpenseValidator = addChickenExpenseValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportChickenExpensesCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, ChickenExpense, object>>
            {
                { _localizer["Description"], (row,item) => item.Description = row[_localizer["Description"]].ToString() },
                { _localizer["Quantity"], (row,item) => item.Quantity = int.TryParse(row[_localizer["Quantity"]].ToString(), out var quantity) ? quantity : 1 },
                { _localizer["Unit Cost"], (row,item) => item.UnitCost = decimal.TryParse(row[_localizer["Unit Cost"]].ToString(), out var unitcost) ? unitcost : 0 },
                { _localizer["Transport"], (row,item) => item.Transport = decimal.TryParse(row[_localizer["Transport"]].ToString(), out var transport) ? transport : 0 }
            }, _localizer["Chicken Expenses"]));

            if (result.Succeeded)
            {
                var importedChickenExpenses = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var chickenexpense in importedChickenExpenses)
                {
                    var validationResult = await _addChickenExpenseValidator.ValidateAsync(_mapper.Map<AddEditChickenExpenseCommand>(chickenexpense), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<ChickenExpense>().AddAsync(chickenexpense);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(chickenexpense.Description) ? $"{chickenexpense.Description} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllChickenExpensesCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}