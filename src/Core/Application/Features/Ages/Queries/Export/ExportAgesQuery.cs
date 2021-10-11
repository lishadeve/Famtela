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

namespace Famtela.Application.Features.Ages.Queries.Export
{
    public class ExportAgesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportAgesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportAgesQueryHandler : IRequestHandler<ExportAgesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportAgesQueryHandler> _localizer;

        public ExportAgesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportAgesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportAgesQuery request, CancellationToken cancellationToken)
        {
            var ageFilterSpec = new AgeFilterSpecification(request.SearchString);
            var ages = await _unitOfWork.Repository<Age>().Entities
                .Specify(ageFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(ages, mappers: new Dictionary<string, Func<Age, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name }
            }, sheetName: _localizer["Ages"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
