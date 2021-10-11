using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Application.Specifications.Dairy;
using Famtela.Domain.Entities.Dairy;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Features.DairyExpenses.Queries.Export
{
    public class ExportDairyExpensesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportDairyExpensesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportDairyExpensesQueryHandler : IRequestHandler<ExportDairyExpensesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportDairyExpensesQueryHandler> _localizer;

        public ExportDairyExpensesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportDairyExpensesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportDairyExpensesQuery request, CancellationToken cancellationToken)
        {
            var dairyexpenseFilterSpec = new DairyExpenseFilterSpecification(request.SearchString);
            var dairyexpenses = await _unitOfWork.Repository<DairyExpense>().Entities
                .Specify(dairyexpenseFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(dairyexpenses, mappers: new Dictionary<string, Func<DairyExpense, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Description"], item => item.Description },
                { _localizer["Quantity"], item => item.Quantity },
                { _localizer["Unit Cost"], item => item.UnitCost },
                { _localizer["Transport"], item => item.Transport }
            }, sheetName: _localizer["Dairy Expenses"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
