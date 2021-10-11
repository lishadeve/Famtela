using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Domain.Entities.Catalog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Application.Specifications.Catalog;
using Famtela.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Famtela.Domain.Entities.Chicken;
using Famtela.Application.Specifications.Chicken;

namespace Famtela.Application.Features.Consumptions.Queries.Export
{
    public class ExportConsumptionsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportConsumptionsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportConsumptionsQueryHandler : IRequestHandler<ExportConsumptionsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportConsumptionsQueryHandler> _localizer;

        public ExportConsumptionsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportConsumptionsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportConsumptionsQuery request, CancellationToken cancellationToken)
        {
            var consumptionFilterSpec = new ConsumptionFilterSpecification(request.SearchString);
            var consumptions = await _unitOfWork.Repository<Consumption>().Entities
                .Specify(consumptionFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(consumptions, mappers: new Dictionary<string, Func<Consumption, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Remarks"], item => item.Remarks }
            }, sheetName: _localizer["Consumption Records"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}