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

namespace Famtela.Application.Features.Statuses.Queries.Export
{
    public class ExportStatusesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportStatusesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportStatusesQueryHandler : IRequestHandler<ExportStatusesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportStatusesQueryHandler> _localizer;

        public ExportStatusesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportStatusesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportStatusesQuery request, CancellationToken cancellationToken)
        {
            var statusFilterSpec = new StatusFilterSpecification(request.SearchString);
            var statuses = await _unitOfWork.Repository<Status>().Entities
                .Specify(statusFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(statuses, mappers: new Dictionary<string, Func<Status, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name }
            }, sheetName: _localizer["Statuses"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
