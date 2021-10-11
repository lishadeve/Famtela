using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Famtela.Application.Extensions;
using Famtela.Application.Interfaces.Repositories;
using Famtela.Application.Interfaces.Services;
using Famtela.Application.Specifications.Catalog;
using Famtela.Domain.Entities.Catalog;
using Famtela.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Famtela.Application.Features.Counties.Queries.Export
{
    public class ExportCountiesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCountiesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCountiesQueryHandler : IRequestHandler<ExportCountiesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCountiesQueryHandler> _localizer;

        public ExportCountiesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCountiesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCountiesQuery request, CancellationToken cancellationToken)
        {
            var countyFilterSpec = new CountyFilterSpecification(request.SearchString);
            var counties = await _unitOfWork.Repository<County>().Entities
                .Specify(countyFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(counties, mappers: new Dictionary<string, Func<County, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name }
            }, sheetName: _localizer["Counties"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
