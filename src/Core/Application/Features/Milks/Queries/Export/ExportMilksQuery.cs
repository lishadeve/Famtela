using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Famtela.Application.Specifications.Dairy;
using Famtela.Domain.Entities.Dairy;

namespace Famtela.Application.Features.Milks.Queries.Export
{
    public class ExportMilksQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportMilksQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportMilksQueryHandler : IRequestHandler<ExportMilksQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportMilksQueryHandler> _localizer;

        public ExportMilksQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportMilksQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportMilksQuery request, CancellationToken cancellationToken)
        {
            var milkFilterSpec = new MilkFilterSpecification(request.SearchString);
            var milks = await _unitOfWork.Repository<Milk>().Entities
                .Specify(milkFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(milks, mappers: new Dictionary<string, Func<Milk, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Morning"], item => item.Morning },
                { _localizer["Evening"], item => item.Evening },
                { _localizer["Remarks"], item => item.Remarks },
            }, sheetName: _localizer["Milk"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}