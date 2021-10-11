using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Application.Specifications.Chicken;
using Famtela.Domain.Entities.Chicken;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Features.ChickenExpenses.Queries.Export
{
    public class ExportChickenExpensesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportChickenExpensesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportChickenExpensesQueryHandler : IRequestHandler<ExportChickenExpensesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportChickenExpensesQueryHandler> _localizer;

        public ExportChickenExpensesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportChickenExpensesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportChickenExpensesQuery request, CancellationToken cancellationToken)
        {
            var chickenexpenseFilterSpec = new ChickenExpenseFilterSpecification(request.SearchString);
            var chickenexpenses = await _unitOfWork.Repository<ChickenExpense>().Entities
                .Specify(chickenexpenseFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(chickenexpenses, mappers: new Dictionary<string, Func<ChickenExpense, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Description"], item => item.Description },
                { _localizer["Quantity"], item => item.Quantity },
                { _localizer["Unit Cost"], item => item.UnitCost },
                { _localizer["Transport"], item => item.Transport }
            }, sheetName: _localizer["Chicken Expenses"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
