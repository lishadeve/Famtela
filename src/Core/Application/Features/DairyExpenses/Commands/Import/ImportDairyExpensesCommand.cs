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
using Famtela.Application.Features.DairyExpenses.Commands.AddEdit;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.DairyExpenses.Commands.Import
{
    public partial class ImportDairyExpensesCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportDairyExpensesCommandHandler : IRequestHandler<ImportDairyExpensesCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditDairyExpenseCommand> _addDairyExpenseValidator;
        private readonly IStringLocalizer<ImportDairyExpensesCommandHandler> _localizer;

        public ImportDairyExpensesCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditDairyExpenseCommand> addDairyExpenseValidator,
            IStringLocalizer<ImportDairyExpensesCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addDairyExpenseValidator = addDairyExpenseValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportDairyExpensesCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, DairyExpense, object>>
            {
                { _localizer["Description"], (row,item) => item.Description = row[_localizer["Description"]].ToString() },
                { _localizer["Quantity"], (row,item) => item.Quantity = int.TryParse(row[_localizer["Quantity"]].ToString(), out var quantity) ? quantity : 1 },
                { _localizer["Unit Cost"], (row,item) => item.UnitCost = decimal.TryParse(row[_localizer["Unit Cost"]].ToString(), out var unitcost) ? unitcost : 0 },
                { _localizer["Transport"], (row,item) => item.Transport = decimal.TryParse(row[_localizer["Transport"]].ToString(), out var transport) ? transport : 0 }
            }, _localizer["Dairy Expenses"]));

            if (result.Succeeded)
            {
                var importedDairyExpenses = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var dairyexpense in importedDairyExpenses)
                {
                    var validationResult = await _addDairyExpenseValidator.ValidateAsync(_mapper.Map<AddEditDairyExpenseCommand>(dairyexpense), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<DairyExpense>().AddAsync(dairyexpense);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(dairyexpense.Description) ? $"{dairyexpense.Description} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDairyExpensesCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}